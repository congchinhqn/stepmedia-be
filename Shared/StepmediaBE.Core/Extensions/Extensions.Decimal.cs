using System;

namespace Metatrade.Core.Extensions
{
    public static partial class MiscExtensions
    {
        public static string FormatWeight(this decimal? weight)
        {
            if (weight == null)
                return "N/A";
            return weight.Value.FormatWeight();
        }
        public static string FormatWeight(this decimal weight)
        {
            if (weight < 1000)
                return $"{weight} gram";

            var  vl = weight % 1000;
            weight = (weight - vl) / 1000;

            var dec = string.Empty;
            if (vl >= 100)
            {
                var r = vl % 100;
                vl = (vl - r) / 100;
                if (r >= 50)
                {
                    vl += 1;
                }
                dec = $".{vl}";
            }
            return $"{weight}{dec} kg";
        }
    }
}