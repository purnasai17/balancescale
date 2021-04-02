using System;
using System.Linq;
using System.Collections.Generic;
using BalanceScaleSelenium.Configuration;

namespace BalanceScaleSelenium.Helper {
    public static class GetUniqueNumbers {
        public static List<int> GetRandomNumbers(int count) {
            var randomNumbers = new List<int>();
            var random = new Random();
            var validNumbers = ConfigurationReader.InputValues.ToList();
            for (var i = 0; i < count; i++) {
                int number;
                do
                    number = random.Next(validNumbers.Count());
                while (randomNumbers.Contains(validNumbers[number]));
                randomNumbers.Add(validNumbers[number]);
            }
            return randomNumbers;
        }
    }
}