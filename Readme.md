# Balance Scale Selenium 

## Table of Contents
* [Let's get Started](#lets-get-started)
  * [Clone the git repo](#clone-the-git-repo)
* [Building the solution](#building-the-solution)
* [Restore the solution](#restore-the-solution)
* [Build the tests](#build-the-tests)
* [Run the tests](#run-the-tests)
* [Check the report](#check-the-report)
* [Dependencies](#dependencies)
* [Folder structure](#folder-structure)
    * [Configuration](#configuration)
    * [Dockerfile](#dockerfile)
    * [TestResults](#testresults)
    * [Tests](#tests)
* [Test Report](#test-report)    

## Let's get Started
> **_NOTE:_** For MacOS/Linux users, Edge and InternetExplorer tests will be skipped if the apps are not installed.
#### Clone the git repo
```bash
$ git clone git@github.com:purnasai17/balancescale.git
```
This page explains the UI testing framework built with .NET Core 3.1 using C# and Selenium.

Please ensure you have the [.NET Core 3.1 SDK installed](https://dotnet.microsoft.com/download).

## Building the solution
Please ensure you have the .NET Core 3.1 SDK installed.

MacOS/Linux: For [Homebrew](https://formulae.brew.sh/) users, `brew cask install dotnet-sdk` will install the long term support version.

Please install [Visual Studio Code](https://code.visualstudio.com/) and ensure the extension [C# for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) (powered by OmniSharp) is installed.

## Restore the solution
In root (*.sln) directory, restore the dependencies in the solution using the below command:

```cmd
dotnet restore
```

## Build the tests

In root (*.sln) directory, build the solution using the below command:

```cmd
dotnet build
```

## Run the tests
In root (*.sln) directory, By using the below command it will auto build and run the tests:

```cmd
dotnet test --logger:trx
```

## Check the report
When you run the `dotnet test --logger:trx` command you will see a `*.trx` file will be added to `src/BalanceScaleSelenium/TestResults` folder.

Please ensure you have the `allure` installed in you machine:
```cmd
allure --version
allure serve src/BalanceScaleSelenium/TestResults/
```


MacOS/Linux: For [Homebrew](https://formulae.brew.sh/) users, ```brew install allure``` will install the long term support version.

Windows: Allure is available from the [Scoop](https://scoop.sh/) commandline-installer `scoop install allure` 

## Dependencies
**XUnit** is a free, open-source, community-focused unit testing tool for the .NET Framework. We will be configuring it to be used as a Selenium C# framework for Web UI automation testing. Documentation: https://xunit.net/

**Selenium C# Framework** is an open-source test automation framework for automated cross-browser testing. It supports popular web browsers – Firefox, Chrome, Microsoft Edge, Internet Explorer, Safari, etc. To interact with the underlying web browser, a collection of language-specific bindings to drive the browser called Selenium WebDriver is used. Documentation: https://www.selenium.dev/

**Selenium Webdriver** allows for the interaction of the Selenium C# Framework with the web browser. Before you can make use of Selenium WebDriver commands in C#, the development environment should be setup i.e. Selenium WebDriver for the corresponding web browser should be installed on the machine. Documentation: https://www.selenium.dev/projects/

**Selenium WebDriver ChromeDriver** installs Chrome Driver (Win32, macOS, Linux64) for Selenium WebDriver. The chromedriver executable is not in the solution, but is copied into the output folder on the build process. This allows you to use Chrome as a default browser without having to install locally or in the pipeline, or commit a driver into the test repository. Documentation: https://github.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/

**Microsoft Extensions Configuration** is a provider used to build key/value-based configuration settings for use in an application. In this instance, builds the app with configured Settings.json. Documentation: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1

**Serilog** is a diagnostic logging library for .NET applications. It is easy to set up, has a clean API, and runs on all recent .NET platforms. While it's useful even in the simplest applications, Serilog's support for structured logging shines when instrumenting complex, distributed, and asynchronous applications and systems. Documentation: https://serilog.net/

## Selenium WebDriver Downloads
If you wish to use a local instance of Selenium Webdriver, be sure to download the Selenium WebDriver file for the browsers with which you wish to perform cross-browser testing from the links mentioned below:
* Opera
* Firefox
* Chrome
* Internet Explorer
* Microsoft Edge

## Folder structure
```
└── balancescale
    ├── Readme.md
    ├── BalanceScaleSelenium.sln
    ├── docker-compose.yml
    └── src
        └── BalanceScaleSelenium
            ├── Configuration
            ├── TestResults
            ├── Tests
            ├── Helper
            ├── Settings.json
            ├── Dockerfile
            └── BalanceScaleSelenium.csproj
```

In case if you I want to use API automation in feature I can simply add a folder in src.

### Configuration
This contains classes used to manage the configuration for the tests.

`ConfigurationReader.cs` contains the logic to obtain the JSON values from `Settings.json`

The ConfigurationReader will automatically replace any configuration setting values with the values set in the Environment Variables on the machine running the tests. if machine doesn't have Environment Variables it defaults to `Stage` environment

E.g. in Settings.json we are using the configuration setting (key-value pair) "BaseUrl":"http://ec2-54-208-152-154.compute-1.amazonaws.com/". If there is an Environment Variable set on the current machine/build agent using BaseUrl key, the value in Settings.json will be replaced.

### Dockerfile
Dockerfile contains all the commands a user could call on the command line to assemble an image. Using docker build users can create an automated build that executes several command-line instructions in succession. This page describes the commands you can use in a Dockerfile.

### TestResults
This folder will be automatically added when we run the `dotnet tests --logger:trx` command in command prompt. 

### Tests
This is the parent folder for all test code.

Both `Fact` and `Theory` attributes are defined by xUnit.net.

The `Fact` attribute is used by the xUnit.net test runner to identify a 'normal' unit test: a test method that takes no method arguments.
The `Theory` attribute, on the other, expects one or more DataAttribute instances to supply the values for a Parameterized Test's method arguments.

The solution tags (or annotates) tests as smoke tests using xUnit Traits. I.e. `[Trait("Category", "Smoke")]`, `[Trait("Category", "Regression")]`
Now you can filter the running tests by Category using `dotnet test --filter "Category=Smoke"`, `dotnet test --filter "Category=Regression"`

## Test Report
![Allure_Report](https://user-images.githubusercontent.com/23709744/113428484-de64ce00-93a4-11eb-8843-ba37759084cc.gif)
