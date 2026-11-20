using MelonLoader;

namespace ArrowModRenewed.src
{
    public static class BuildInfo
    {
        internal const string ModName = "ArrowModRenewed";
        internal const string ModAuthor = "leatherneck6017, ttr";
        internal const string ModVersion = "1.0.0";
    }

    internal class ArrowModRenewed : MelonMod
    {
        private static MelonLogger.Instance? Logger;

        public override void OnInitializeMelon()
        {
            Logger = LoggerInstance;
            Settings.OnLoad();
            LoggerInstance.Msg($"Version {BuildInfo.ModVersion} loaded!");
        }

        public static void Log(string msg)
        {
            //Logger?.Msg(msg);
        }

        public static void LogYellow(string msg)
        {
            Logger?.Msg(System.ConsoleColor.Yellow, msg);
        }
    }
}