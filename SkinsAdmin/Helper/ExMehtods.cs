﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Helper
{
    public class ExMehtods
    {
        public static string FormatNumber(long num)
        {
            if (num >= 100000000)
            {
                return (num / 1000000D).ToString("0.M");
            }
            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.M");
            }
            if (num >= 100000)
            {
                return (num / 1000D).ToString("0.k");
            }
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.k");
            }
            if (num >= 1000)
            {
                return (num / 1000D).ToString("0.k");
            }

            return num.ToString("#,0");
        }
    }
}
