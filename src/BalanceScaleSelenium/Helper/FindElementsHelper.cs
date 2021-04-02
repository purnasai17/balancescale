using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using BalanceScaleSelenium.Tests;
using OpenQA.Selenium.IE;

namespace BalanceScaleSelenium.Helper {
    public static class FindElementsHelper {
        public enum BrowserType {
            Chrome,
            Firefox,
            Edge,
            InternetExplorer
        }

        public static IWebDriver InitBrowser(BrowserType browserName) {
            var driverDirectory = System.IO.Path.GetDirectoryName(typeof(FakeNumber).Assembly.Location) ?? ".";
            var isDebuggerAttached = System.Diagnostics.Debugger.IsAttached;

            return browserName switch {
                       BrowserType.Chrome => CreateChromeDriver(driverDirectory, isDebuggerAttached),
                       BrowserType.Edge => CreateEdgeDriver(driverDirectory, isDebuggerAttached),
                       BrowserType.Firefox => CreateFirefoxDriver(driverDirectory, isDebuggerAttached),
                       BrowserType.InternetExplorer => new InternetExplorerDriver(driverDirectory,
                           new InternetExplorerOptions() { IgnoreZoomLevel = true }),
                       _ => throw new NotSupportedException($"The browser '{browserName}' is not supported."),
                   };
        }


        private static IWebDriver CreateChromeDriver(string driverDirectory, bool isDebuggerAttached) {
            var options = new ChromeOptions();

            if (!isDebuggerAttached) {
                options.AddArgument("--headless");
            }

            // HACK Workaround for "(unknown error: DevToolsActivePort file doesn't exist)"
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                options.AddArgument("--no-sandbox");
            }

            return new ChromeDriver(driverDirectory, options);
        }

        private static IWebDriver CreateEdgeDriver(string driverDirectory, bool isDebuggerAttached) {
            var options = new EdgeOptions() { UseChromium = true };

            if (!isDebuggerAttached) {
                options.AddArgument("--headless");
            }

            return new EdgeDriver(driverDirectory, options);
        }

        private static IWebDriver CreateFirefoxDriver(string driverDirectory, bool isDebuggerAttached) {
            var options = new FirefoxOptions();

            if (!isDebuggerAttached) {
                options.AddArgument("--headless");
            }

            return new FirefoxDriver(driverDirectory, options);
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByClassName(
            IWebDriver driver, string className, int timeout = 5) {
            var driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return driverWait.Until(a => driver.FindElements(By.ClassName(className)));
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByTagName(IWebDriver driver, string tagName, int timeout = 5) {
            var driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return driverWait.Until(x => x.FindElements(By.TagName(tagName)));
        }

        public static IWebElement FindElementByClassName(IWebDriver driver, string className, int timeout = 5) {
            var driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return driverWait.Until(x => x.FindElement(By.ClassName(className)));
        }

        public static IWebElement FindElementById(IWebDriver driver, string id, int timeout = 5) {
            var driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return driverWait.Until(x => x.FindElement(By.Id(id)));
        }
    }
}