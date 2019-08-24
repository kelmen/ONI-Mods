﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kelmen.ONI.Mods.TemperatureFilters
{
    internal static class Utils
    {
        static string Source = null;
        public static void Log(string txt)
        {
            if (Source == null)
            {
                var asmbN = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                Source = $"mod {asmbN.Name} , ver {asmbN.Version}";
            }
            var ts = System.DateTime.UtcNow.ToString("[HH:mm:ss.fff]");
            Console.WriteLine($"{Source} , {ts} : {txt}");
        }
    }
}
