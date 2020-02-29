using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace flow
{
    class Program
    {
        static private IConfiguration Configuration { get; set; }

        static private readonly IReadOnlyDictionary<string, string> _defaultSettings = new Dictionary<string, string> {
            {"One","10"},
            {"Two", "20"},
            {"AppPath", "mem/path"}
        };
        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(_defaultSettings)
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            Console.WriteLine($"AppContext.BaseDirectory - {AppContext.BaseDirectory}");

            var _appSets = new flow.AppSettings();

            Configuration.GetSection(nameof(flow.AppSettings)).Bind(_appSets);

            Console.WriteLine($"Here is One - {_appSets.One.ToString()}");
            Console.WriteLine($"Here is Two - {_appSets.Two.ToString()}");
            Console.WriteLine($"Here is AppPath - {_appSets.AppPath.ToString()}");

            Console.WriteLine(AppSettings.TestUsing());

            Console.ReadLine();
        }
    }

    public class AppSettings
    {
        public int One { get; set; }
        public int Two { get; set; }
        public string AppPath { get; set; }

        public static string TestUsing()
        {
            using (var archiveContents = System.IO.File.OpenRead(@"D:\GoogleDrive\Job\flnp\dev\tests\TestAutoUpdateRepo\Releases\TestAutoUpdateRepo-1.2.7-full1.nupkg"))
            {
                return archiveContents.Name;
            }
        }
    }
}
