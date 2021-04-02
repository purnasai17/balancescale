using System;
using System.Collections.Generic;
using BalanceScaleSelenium.Helper;
using System.Runtime.InteropServices;
using FluentAssertions;
using OpenQA.Selenium;
using System.Linq;
using System.Runtime.CompilerServices;
using BalanceScaleSelenium.Configuration;
using Serilog;
using Xunit;

namespace BalanceScaleSelenium.Tests {
    public class FakeNumber {
        private readonly string _baseUrl;
        private List<int> _leftOutNumber = new List<int>();
        private List<string> _weighings = null;

        public FakeNumber() {
            this._baseUrl = ConfigurationReader.Baseurl;
            _weighings = new List<string>();
        }

        [Fact]
        [Trait("Category", "SmokeTest")]
        public void EightCoinsWithChrome() {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(8);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, FindElementsHelper.BrowserType.Chrome);
        }

        [SkippableTheory]
        [Trait("Category", "Regression")]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void EightCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(8);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }

        [SkippableTheory]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void SevenCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(7);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }

        [SkippableTheory]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void SixCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(6);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }

        [SkippableTheory]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void FiveCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(5);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }

        [SkippableTheory]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void FourCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(4);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }

        [SkippableTheory]
        [Trait("Category", "Regression")]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void ThreeCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(3);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }


        [SkippableTheory]
        [InlineData(FindElementsHelper.BrowserType.Chrome)]
        [InlineData(FindElementsHelper.BrowserType.Edge)]
        [InlineData(FindElementsHelper.BrowserType.Firefox)]
        [InlineData(FindElementsHelper.BrowserType.InternetExplorer)]
        public void TwoCoins(FindElementsHelper.BrowserType browserName) {
            var inputArray = GetUniqueNumbers.GetRandomNumbers(2);
            var valueTuple = SplitArray(inputArray);
            FindFackNumber(valueTuple.Item1, valueTuple.Item2, browserName);
        }


        private void FindFackNumber(List<int> leftBowl, List<int> rightBowl, FindElementsHelper.BrowserType browserName,
                                    int testFail = 0,
                                    [CallerMemberName] string memberName = "") {
            IWebDriver driver = null;
            try {
                // Arrange
                Skip.If(
                        !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        && browserName == FindElementsHelper.BrowserType.InternetExplorer,
                        ($"{browserName} is only supported on Windows."));

                Skip.If(
                        RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        && browserName == FindElementsHelper.BrowserType.Edge,
                        $"{browserName} is not supported on Linux.");

                Skip.If(
                        RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                        && browserName == FindElementsHelper.BrowserType.Edge,
                        $"{browserName} is not supported on macOS in GitHub Actions.");

                string result;

                // Act
                driver = FindElementsHelper.InitBrowser(browserName);
                driver.Navigate().GoToUrl(_baseUrl);
                driver.Manage().Window.Maximize();
                driver.Title.Should().Be("React App");
                _leftOutNumber = ConfigurationReader.InputValues.Except(leftBowl.Union(rightBowl)).ToList();
                Log.Information("Selenium test -> website is opened in Chrome browser");
                var filterResult = new List<int>();

                do {
                    if (filterResult.Count > 1) {
                        var valueTuple = SplitArray(filterResult);
                        leftBowl = valueTuple.Item1;
                        rightBowl = valueTuple.Item2;
                    }

                    result = GetResult(leftBowl, rightBowl, driver);
                    filterResult = FilterResults(result, leftBowl, rightBowl, _leftOutNumber, driver);
                } while (filterResult.Count > 1 || result == "=" && filterResult.Count > 1);

                // Assert
                FindElementsHelper.FindElementById(driver, $"coin_{filterResult.FirstOrDefault() + testFail}").Click();
                driver.SwitchTo().Alert().Text.Should().Be("Yay! You find it!");
                driver.SwitchTo().Alert().Accept();
            } finally {
                // Close the browser window that the driver has focus of
                driver?.Quit();
                //Closes all browser windows and safely ends the session
                driver?.Dispose();
            }
        }

        private static(List<int>, List<int>) SplitArray(IReadOnlyCollection<int> array) {
            var averageBowlCount = array.Count() / 2;
            var leftBowl = array.Take(averageBowlCount).ToList();
            var rightBowl = array.Skip(averageBowlCount).ToList();
            return (leftBowl, rightBowl);
        }

        private List<int> FilterResults(string result, List<int> leftBowl, List<int> rightBowl,
                                        ICollection<int> leftOutNumber, IWebDriver driver) {
            List<int> weighs;
            string weighings;
            switch (result) {
                case "=":
                    weighings = $"[{string.Join(",", leftBowl)}] = [{string.Join(",", rightBowl)}]";
                    _weighings.Add(weighings);
                    var x = FindElementsHelper.FindElementsByTagName(driver, "li")[_weighings.Count - 1].Text;
                    // var x = driver.FindElements(By.TagName("li"));
                    _weighings.Should().Contain(x);
                    driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[4]/button[1]")).Click();
                    FindElementsHelper.FindElementById(driver, "reset").Text.Should().Be("?");
                    if (leftBowl.Count() > rightBowl.Count()) {
                        return leftBowl;
                    } else if (rightBowl.Count() > leftBowl.Count()) {
                        return rightBowl;
                    }

                    var enumerable = new List<int>().Concat(leftOutNumber).ToList();
                    leftOutNumber.Clear();
                    return enumerable;
                case ">":
                    weighings = $"[{string.Join(",", leftBowl)}] > [{string.Join(",", rightBowl)}]";
                    _weighings.Add(weighings);
                    var y = FindElementsHelper.FindElementsByTagName(driver, "li")[_weighings.Count - 1].Text;
                    weighings.Should().Contain(y);
                    driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[4]/button[1]")).Click();
                    FindElementsHelper.FindElementById(driver, "reset").Text.Should().Be("?");
                    if (leftBowl.Count() == rightBowl.Count()) return rightBowl;
                    weighs = rightBowl.Concat(leftOutNumber).ToList();
                    leftOutNumber.Clear();
                    return weighs;
                case "<":
                    weighings = $"[{string.Join(",", leftBowl)}] < [{string.Join(",", rightBowl)}]";
                    _weighings.Add(weighings);
                    var z = FindElementsHelper.FindElementsByTagName(driver, "li")[_weighings.Count - 1].Text;
                    _weighings.Should().Contain(z);
                    driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[4]/button[1]")).Click();
                    FindElementsHelper.FindElementById(driver, "reset").Text.Should().Be("?");
                    if (leftBowl.Count() == rightBowl.Count()) return leftBowl;
                    weighs = leftBowl.Concat(leftOutNumber).ToList();
                    leftOutNumber.Clear();
                    return weighs;
                default:
                    Log.Information("Test failed check and Re-run the test");
                    return new List<int>();
            }
        }

        private static string GetResult(IReadOnlyList<int> leftBowl, IReadOnlyList<int> rightBowl, IWebDriver driver) {
            var bowls = FindElementsHelper.FindElementsByClassName(driver, "game-board");
            bowls[0].Text.Should().Be("left bowl");
            bowls[1].Text.Should().Be("right bowl");

            FindElementsHelper.FindElementById(driver, "reset").Text.Should().Be("?");
            foreach (var item in bowls)
                switch (item.Text) {
                    case "left bowl": {
                        for (var i = 0; i < leftBowl.Count; i++) {
                            var leftGrid = FindElementsHelper.FindElementById(driver, $"left_{i}");
                            leftGrid.SendKeys(leftBowl[i].ToString());
                        }

                        break;
                    }
                    case "right bowl": {
                        for (var i = 0; i < rightBowl.Count; i++) {
                            var rightGrid = FindElementsHelper.FindElementById(driver, $"right_{i}");
                            rightGrid.SendKeys(rightBowl[i].ToString());
                        }

                        break;
                    }
                }
            FindElementsHelper.FindElementById(driver, "weigh").Click();
            return FindElementsHelper.FindElementById(driver, "reset").Text;
        }
    }
}