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

        public static bool IsGreaterThan(this double x, double y, double acceptableVariance)
        {
            return x - y >= acceptableVariance;
        }

        public static double ToNumber(this string amount)
        {
            if (string.IsNullOrEmpty(amount))
            {
                return 0;
            }

            if (double.TryParse(amount ?? "0", out double dResult))
            {
                return dResult;
            }

            var allParts = amount.Split(' ');

            var decimalParts = allParts.Last().Split('/');
            if (decimalParts.Length > 1 && double.TryParse(decimalParts[0], out double numerator) &&
                                          double.TryParse(decimalParts[1], out double denominator) &&
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
