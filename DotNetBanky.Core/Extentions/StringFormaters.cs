﻿namespace DotNetBanky.Core.Extentions
{
    public static class DecimalFormaters
    {
        public static string ToStringFormated(this decimal value)
        {
            return string.Format("${0:0,0.00}", value);
        }
        public static string ToStringFormated(this decimal? value)
        {
            return string.Format("${0:0,0.00}", value);
        }
        public static string ToStringFormated(this int value)
        {
            return string.Format("{0:0,0}", value);
        }
        public static string ToStringFormated(this int? value)
        {
            return string.Format("{0:0,0}", value);
        }
    }

}
