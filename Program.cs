using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace flow
{
    class Program
    {
        static private IConfiguration Configuration { get; set; }

        static private readonly IReadOnlyDictionary<string, string> _defaultSettings = new Dictionary<string, string> {
            {"AppSettings:One","10"},
            {"AppSettings:Two", "20"},
            {"AppSettings:AppPath", "mem/path"},
            {"GlobalValue1", "1000"},
            {"GlobalValue2", "2000"}
        };

        static private readonly IDictionary<string, string> _clMaps= new Dictionary<string, string> {
            {"-o","GlobalValue1"},
            {"--one", "GlobalValue1"},
            {"-t", "GlobalValue2"},
            {"--two", "GlobalValue2"}
        };
        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(_defaultSettings)
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddCommandLine(args, _clMaps)
                .Build();

            Console.WriteLine($"AppContext.BaseDirectory - {AppContext.BaseDirectory}");
            //Console.WriteLine(Configuration);

            var _appSets = new flow.AppSettings();

            Configuration.GetSection(nameof(flow.AppSettings)).Bind(_appSets);

            Console.WriteLine($"Here is One - {_appSets.One.ToString()}");
            Console.WriteLine($"Here is Two - {_appSets.Two.ToString()}");
            Console.WriteLine($"Here is AppPath - {_appSets.AppPath.ToString()}");
            Console.WriteLine($"Here is GlobalValue1 - {Configuration["GlobalValue1"]}");
            Console.WriteLine($"Here is GlobalValue2 - {Configuration["GlobalValue2"]}");

            Console.ReadLine();
        }

      }

    public class AppSettings
    {
        public int One { get; set; }
        public int Two { get; set; }
        public string AppPath { get; set; }
    }
}
