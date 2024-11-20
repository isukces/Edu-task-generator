using iSukces.Build;

namespace Build.XEducation;

public static class PrvConfig
{
    private static string ThirdParty(BuildConfig cfg, params string[] pathItems)
    {
        var dir = Path.Combine(cfg.SlnDir.FullName, "..", "thirdParty");
        foreach (var item in pathItems)
            dir = Path.Combine(dir, item);
        dir = new DirectoryInfo(dir).FullName;
        return dir;
    }

    #region Properties

    public static string GetWinformsEmbedd(BuildConfig cfg) => ThirdParty(cfg, "WinformsEmbedd");
    public static string GetWoutWare(BuildConfig cfg) => ThirdParty(cfg, "WoutWare");

    #endregion
}