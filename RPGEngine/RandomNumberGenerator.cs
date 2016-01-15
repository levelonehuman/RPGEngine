using System;
using System.Security.Cryptography;

namespace RPGEngine
{
    /// <summary>
    /// Generates a "better" random number, upper bound inclusive
    /// </summary>
    public static class RandomNumberGenerator
    {
        static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int GetNumberBetween(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // Add one to the range, to allow for the rounding done with Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }
    }
}
