using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using CsvHelper;

namespace flow
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var _httpClient = new WebClient();
            _httpClient.Credentials = new NetworkCredential("", "");

            _httpClient.UploadData(@"https://disk.jinr.ru/remote.php/dav/files/regata/1107798.cnf", "PUT", File.ReadAllBytes(@"D:\Spectra\2020\01\dji-1\1107798.cnf"));

        }
    }
}
