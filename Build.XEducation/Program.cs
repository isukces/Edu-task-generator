#define _ILMERGE
using System.Runtime.CompilerServices;
using System.Text;
using iSukces.Build;

namespace Build.XEducation;

 

internal class Program
{
    private static string GetCurrentFile([CallerFilePath] string path = null) => path;

    private static void Main(string[] args)
    {

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var sln = new SlnFilePrepare
        {
            SlnSearchAssembly = typeof(Program).Assembly,
            SolutionName      = "XEducation.sln",
            // OutputSolution    = "XEducation.ForBuild.sln"
        };
        sln.OutputSolution = sln.SolutionName;
        sln.Load();
        
#if ILMERGE
        var woutWare       = new DllDir(PrvConfig.WoutWare);
        var winformsEmbedd = new DllDir(PrvConfig.WinformsEmbedd);
        var dontIlMerge    = woutWare + winformsEmbedd;
#endif

        const string main = "XEducation";

        var cfg = new BuildConfig
        {
            UpdateVersions        = true,
            Solution              = sln.Solution,
            InnoSetupSourceScript = Path.Combine(sln.SlnDir.FullName, "install", "XEducation7.iss"),
            InnoSetupCompilerExe  = FindInno(),
            SlnDir                = sln.SlnDir,
            SolutionShortFileName = sln.OutputSolution,
            CompiledBinary        = @"bin\{0}\net8.0-windows",
            InstallationFolder    = @"C:\Program Files\iSukces\XEducation",
            MainProjectFolder     = main,
            BuildConfiguration    = DebugOrRelease.Release,
            ExeName               = "XEducation.exe",
            // Nuget = Environment.GetEnvironmentVariable("NUGET_EXE") ?? throw new Exception("NUGET_EXE not set"),
#if ILMERGE
            IlMerge = new IlMergeConfig
            {
                Exclude = dontIlMerge,
                IlMergeExe = @"..\tools\ilmerge.exe",
                OutputExe = null,
                Flags = IlMergeFlags.Closed | IlMergeFlags.DeleteExcludedFiles,
                Target = IlMergeTarget.WinExe
            } 
#else
            IlMerge = null
#endif
        };
        cfg.PublishOutputDir = Path.Combine(sln.SlnDir.FullName, main, cfg.GetCompiledBinary()!);
        
        
        cfg.WithNoWarn(IgnoreWarnings.Get());

        cfg.ProcessesToKillBeforeCompile.Add(cfg.ExeName);
        cfg.SkipClearBinObj.Add(new FileInfo(GetCurrentFile()).Directory.FullName);

        try
        {
            var b = new BuildingScript(cfg)
            {
                /*HotOutput = new DirectorySynchronizeItem(
                    @"E:\Dropbox\#AlpexOprogramowanie\Install\HOT-2",
                    SyncFlags.TargetFolderExists
                )*/
            };
            b.Run();
        }
        catch (Exception e)
        {
            ExConsole.WriteException(e);
        }

        Console.WriteLine("Press ENTER");
        Console.ReadLine();
    }

    private static string FindInno()
    {
        var p = @"C:\Program Files (x86)\Inno Setup 6\Compil32.exe";
        if (File.Exists(p))
            return p;
        p = @"C:\Users\psteclik\AppData\Local\Programs\Inno Setup 6\Compil32.exe";
        if (File.Exists(p))
            return p;
        throw new Exception("Inno Setup compiler not found");
    }
}