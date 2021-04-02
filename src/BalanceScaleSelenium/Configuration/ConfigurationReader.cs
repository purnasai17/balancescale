using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BalanceScaleSelenium.Configuration {
    public static class ConfigurationReader {
        private const string Path = "Settings.json";
        private const string AspnetcoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private static readonly IConfigurationRoot Configuration;

        static ConfigurationReader() {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(DirectoryPath)
                          .AddJsonFile(Path);
            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(Configuration)
                         .CreateLogger();
            try {
                Log.Information("Starting up");
                Log.Information($"Environment Variable: {GetEnvironmentVariable}");
            } catch (Exception ex) {
                Log.Fatal(ex, "Application start-up failed");
            }
        }

        private static string DirectoryPath =>
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;

        private static string GetEnvironmentVariable =>
            Environment.GetEnvironmentVariable(AspnetcoreEnvironment) ?? "Stage";

        public static string Get(string keyName) =>
            Configuration.GetSection(keyName).Value;

        public static T Get<T>(string keyName) =>
            Configuration.GetSection(keyName).Get<T>();

        public static string Baseurl =>
            Get($"Environment:{GetEnvironmentVariable}:BaseUrl");

        public static IEnumerable<int> InputValues =>
            Get<List<int>>("Data:InputNumbers");
    }
}