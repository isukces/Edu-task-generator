#define _SKIP_COMPILE
#define _USE_ILREPACK

using System.Text;
using iSukces.Build;
using iSukces.Build.InnoSetup;

namespace Build.XEducation;

internal class BuildingScript : BuildingScriptBase
{
    public BuildingScript(BuildConfig cfg)
        : base(cfg)
    {
        accepted = "XEducation.exe,XEducation.exe.config"
            .Split(',')
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static FileCommand AcceptFileToInstall(FileInfo fileInfo, FileCommand command)
    {
        if (fileInfo.FullName.Contains("xInnoDependencyInstaller"))
        {
            // dontcopy noencryption
            command.Flags |= FileCommand.FileFlags.DontCopy | FileCommand.FileFlags.NoEncryption;
            return command;
        }

        command.Flags |= FileCommand.FileFlags.IgnoreVersion | FileCommand.FileFlags.ReplaceSameversion;

        return Accept(fileInfo) ? command : null;

        bool Accept(FileInfo fi)
        {
            if (string.Equals(fi.Name, "XEducation.dll.config", StringComparison.OrdinalIgnoreCase))
                return true;
            if (fi.Name.EndsWith(".deps.json", StringComparison.OrdinalIgnoreCase))
                return true;
            var ext = fi.Extension.ToLower();
            if (ext is ".dll" or ".exe")
                return true;
            return ext is not (".pdb" or ".xml" or ".config");
        }
    }

    private void A01_UpdateAssemblyCompiledAttribute()
    {
        var fileName = Path.Combine(Configuration.SlnDir.FullName, Configuration.MainProjectFolder, "Properties", "AssemblyInfo.cs");
        if (!File.Exists(fileName)) return;
        var lines = File.ReadAllText(fileName);
        lines = AssemblyCompiledManager.Replace(lines, DateTimeOffset.Now);
        lines = lines.TrimEnd() + "\r\n";
        File.WriteAllText(fileName, lines);
    }

    private bool A04_SyncEmbeddedBinaries()
    {
        var embedded = new FileSynchronizer
        {
            SourceDir = CompiledBinariesDir,
            TargetDir = PrvConfig.GetWinformsEmbedd(Configuration),
            Flags     = SyncFlags.OnlyExistingFiles
        };
        var result = embedded.Synchronize(AcceptFile);

        return result;
    }

    private string A06_InnoSetup(string version)
    {
        var issScript = Configuration.InnoSetupSourceScript;
        if (string.IsNullOrEmpty(issScript) || !File.Exists(issScript)) return null;

        var windows1250 = Encoding.GetEncoding(1250);
        var inno        = InnoSetupFile.Load(issScript, windows1250);
        var files       = new DirectoryInfo(CompiledBinariesDir).GetFiles("*", SearchOption.AllDirectories).ToList();
        var builder = new InnoSetupFileBuilder(inno)
        {
            RelativeToFile = issScript,
            BinaryDir      = CompiledBinariesDir
        };
        builder.DeleteAllFiles();

        var extra = new DirectoryInfo(Path.Combine(Configuration.SlnDir.FullName, @"..\install\scripts\xInnoDependencyInstaller\"));
        if (extra.Exists)
            files.AddRange(extra.GetFiles("netcorecheck*.exe"));
        builder.Add(files, AcceptFileToInstall);
        files.Insert(0, new FileInfo(Path.Combine(Configuration.SlnDir.FullName, "LICENSE.txt")));
        
        var installFile = inno.SetVersionsAndOutputBaseFilename(
            version, 
            "XEducation", 
            "Out");
        {
            var appName = "XEducation";
            inno["Setup", "VersionInfoDescription"] = appName;
            inno["Setup", "VersionInfoProductName"] = appName;
            inno["Setup", "AppName"] = appName;
        }

        {
            var acc = new HashSet<string>(StringComparer.InvariantCulture)
            {
                "unins000.dat",
                "unins000.exe"
            };

            builder.AddNotNeccesaryFiles(builder.BinaryDir, Configuration.InstallationFolder, f =>
            {
                if (acc.Contains(f.Name)) return true;
                //var rel = f.FullName[Configuration.InstallationFolder.Length..].TrimStart('\\');
                //if (rel.StartsWith(@"data\db\", StringComparison.OrdinalIgnoreCase)) return true;
                // if (rel.StartsWith("log\\", StringComparison.OrdinalIgnoreCase)) return true;
                return false;
            });
            // output Name 
        }
        inno.Save(builder.RelativeToFile, windows1250);
        
        var exe = Configuration.InnoSetupCompilerExe;
        if (string.IsNullOrEmpty(exe)) return null;
        var workingDir = ExeRunner.WorkingDir;
        var fi         = new FileInfo(issScript);
        ExeRunner.WorkingDir = fi.Directory!.FullName;
        ExeRunner.Execute(exe, "/cc", fi.Name);
        installFile          = Path.Combine(ExeRunner.WorkingDir, installFile);
        ExeRunner.WorkingDir = workingDir;
        return installFile;
    }

    private string A08_UpdateVersions()
    {
        var projs = Configuration.Solution.Projects
            .Where(a => !a.IsFolder)
            .Select(a => a.File).ToArray();
        const string mainName = "XEducation.csproj";
        return A08_UpdateVersions(projs, mainName, Configuration.UpdateVersions);
    }

    protected override bool AcceptFile(FileInfo file) => accepted.Contains(file.Name);


    public void Run()
    {
        KillBeforeCompile();
        var start = DateTime.Now;
        BuildUtils.DisplayDeletedFiles = false;
        BuildUtils.ClearBinObj(Configuration.SlnDir, Configuration.SkipClearBinObj);
        A01_UpdateAssemblyCompiledAttribute();
        var version = A08_UpdateVersions();

        try
        {
            while (true)
            {
                Repeat.RunTwice(() =>
                {
                    /*A03_DotnetPublish(x =>
                    {
                        x.AcceptFileAfterBuild = file =>
                        {
                            return DotnetPublishCli.StandardFilter(file, "XEducation");
                        };
                    });*/
                    //A03_CompileMsBuildAndNuget();
                    
                    Path.Combine(Configuration.SlnDir.FullName, Configuration.SolutionShortFileName);

                    ExeRunner.WorkingDir = Configuration.SlnDir.FullName;
                    ExeRunner.Execute("nuget", "restore", Configuration.SolutionShortFileName);
                    ExeRunner.Execute("dotnet", "build", Configuration.SolutionShortFileName, "-c", "RELEASE");
                });
                
                var changed = A04_SyncEmbeddedBinaries();
                if (!changed)
                    break;
            }
        }
        finally
        {
        }

        var installExe = A06_InnoSetup(version);

        ExConsole.WriteLine("Build time {0}", DateTime.Now - start);

        if (Configuration.IlMerge is not null)
            throw new NotSupportedException("IlMerge is not supported");
        //A05_IlMerge();

        if (string.IsNullOrEmpty(installExe))
            A08_SyncToHotOutput(HotOutput, AcceptFile);
        else
        {
            var src    = new FileInfo(installExe);
            var target = new FileInfo(Path.Combine(HotOutput.Folder, src.Name));
            if (target.Directory.Exists)
            {
                if (target.Exists)
                    target.Delete();
                File.Copy(src.FullName, target.FullName);
            }
        }

        ExConsole.WriteLine("Elapsed {0}", DateTime.Now - start);
    }

    #region Fields

    private readonly HashSet<string> accepted;

    #endregion
}
