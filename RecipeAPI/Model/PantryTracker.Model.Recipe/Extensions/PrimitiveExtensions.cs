using System;
using System.Linq;

namespace PantryTracker.Model.Extensions
{
    public static class PrimitiveExtensions
    {
        public static bool IsApproximatelyEqual(this double x, double y, double acceptableVariance)
        {
            return Math.Abs(x - y) < acceptableVariance;
        }

        public static bool IsGreaterThanOrEqualTo(this double x, double y, double acceptableVariance)
        {
            return (x - y) >= acceptableVariance;
        }

        /// <summary>
        /// Converts a string into a double -- mostly intendended to convert fractions.
        /// </summary>
        public static double? ToNumber(this string amount, double? defaultValue = 0)
        {
            if (string.IsNullOrEmpty(amount))
            {
                return defaultValue;
            }

            amount = amount.Trim();
            if (double.TryParse(amount ?? "0", out double dResult))
            {
                return dResult;
            }

            var fractionParts = amount.Split('/');
            amount = string.Join('/', fractionParts.Select(p => p.Trim()));

            var allParts = amount.Split(' ');

            var fractionPart = allParts.Last().Split('/');
            if (fractionPart.Length > 1 && double.TryParse(fractionPart[0], out double numerator) &&
                                          double.TryParse(fractionPart[1], out double denominator) &&
                                          denominator > 0)
            {
                if (allParts.Length > 1 && int.TryParse(allParts.First(), out int wholeNumber))
                {
                    return wholeNumber + Math.Round(numerator / denominator, 2);
                }

                return Math.Round(numerator / denominator, 2);
            }

            return 0;
        }
    }
}
