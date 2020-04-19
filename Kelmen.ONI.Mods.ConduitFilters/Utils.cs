using System;

namespace Kelmen.ONI.Mods.ConduitFilters
{
    public static class Utils
    {
        public static void Log(string txt)
        {
            var ts = System.DateTime.UtcNow.ToString("[HH:mm:ss.fff]");
            Console.WriteLine($"{ts} : {txt}");
        }
        public static void Log(string source, Exception ex)
        {
            Log($"{source} : {ex}");
        }
    }
}
