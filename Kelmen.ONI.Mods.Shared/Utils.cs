﻿using System;

namespace Kelmen.ONI.Mods.Shared
{
    internal static class Utils
    {
        //static string Source = null;
        public static void Log(string txt)
        {
            //if (Source == null)
            //{
            //    var asmbN = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            //    Source = $"mod {asmbN.Name} , ver {asmbN.Version}";
            //}
            
            var ts = System.DateTime.UtcNow.ToString("[HH:mm:ss.fff]");
            //Console.WriteLine($"{Source} , {ts} : {txt}");
            Console.WriteLine($"{ts} : {txt}");
        }
    }

    public static class Utils2
    {
        public static void Log(string txt)
        {
            var ts = System.DateTime.UtcNow.ToString("[HH:mm:ss.fff]");
            //Console.WriteLine($"{Source} , {ts} : {txt}");
            Console.WriteLine($"{ts} : {txt}");
        }
        public static void Log(string source, Exception ex)
        {
            Log(source + " : " + ex.ToString());
        }
    }
}
