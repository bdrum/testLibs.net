using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace flow
{
    // TODO: upload all file names to db. sort them. remember last uploaded file. start with it.
    // TODO: show active time between attempts

    public class InfoContext : DbContext
    {
        public DbSet<SharedSpectra> Spectra { get; set; }

        private readonly string connectionString = @"Data Source=
        public InfoContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SharedSpectra>()
                   .HasKey(s => new { s.token});
        }
    }

    [Table("sharedspectra")]
    public class SharedSpectra
    {
        public string fileS { get; set; }
        public string token { get; set; }
    }

    class Program
    {
        public static int RunNumber = 0;
        public static int WaitingTime = 0;

        public static async Task Main(string[] args)
        {

            //var tasks = new List<Task<string>>();

            //for (var i = 0; i < 500; ++i)
            //    tasks.Add(Test());

            //await Task.WhenAll(tasks.ToArray());

            //foreach (var t in tasks)
            //{
            //    if (!t.Result.Contains("200 OK"))
            //        Console.WriteLine($"{t.Id}--{t.Result}");
            //}

            //return;

            Console.WriteLine($"Start uploading process:");
            var s = Stopwatch.StartNew();
            await Fill();
            s.Stop();
            var et = s.ElapsedMilliseconds / 1000;
            Console.WriteLine($"Number of attempts is {RunNumber}");
            Console.WriteLine($"Elapsed time: {et} sec");
            Console.WriteLine($"Waiting time: {RunNumber*60} sec");
            Console.WriteLine($"Working time: {et - WaitingTime} sec");
        }

        public static async Task Fill()
        {
            var s = Stopwatch.StartNew();
            try
            {
                Console.WriteLine($"Start filling session: #{RunNumber}");
                RunNumber++;

                var filesFull = Directory.EnumerateFiles(@"D:\Spectra\2020\09\kji", "*.cnf", SearchOption.AllDirectories).ToList();
                Console.WriteLine($"Number of file on disk: {filesFull.Count}");

                //var fileNames = filesFull.Select(ff => Path.GetFileNameWithoutExtension(ff)).ToList();
                ////files = files.Take(200);
                //List<string> spectras = null;
                //using (var ic = new InfoContext())
                //{
                //    spectras = ic.Spectra.Select(s => s.fileS).ToList();
                //}

                //Console.WriteLine($"Number of spectras on disk: {spectras.Count}");
                //var diff = fileNames.Except(spectras).ToList();
                //foreach (var d in diff)
                //    Console.WriteLine($"The difference: {d}");


                //return;


                var tasks = new List<Task>();

                using (var ic = new InfoContext())
                {
                    foreach (var file in filesFull)
                    {
                        if (ic.Spectra.Where(s => s.fileS == Path.GetFileNameWithoutExtension(file)).Any()) continue;
                        tasks.Add(ProcessFile(file));
                    }
                }
                Console.WriteLine($"Number of tasks:{tasks.Count()}");

                await Task.WhenAll(tasks.ToArray());

            }
            catch (Exception ex)
            {
                var et = s.ElapsedMilliseconds / 1000;
                Console.WriteLine($"Number of attempts is {RunNumber}");
                Console.WriteLine($"Elapsed time: {et} sec");
                Console.WriteLine($"Waiting time: {RunNumber * 60} sec");
                Console.WriteLine($"Working time: {et - WaitingTime} sec");

                if (RunNumber == 250) return;
                Console.WriteLine(ex.ToString());
                Thread.Sleep(TimeSpan.FromSeconds(60));
                await Fill();
            }
        }

        private static async Task ProcessFile(string file)
        {
            var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            if (await Regata.Utilities.WebDavClientApi.UploadFile(file, ct.Token))
            {
                var _ic = new InfoContext();
                if (_ic.Spectra.Where(s => s.fileS == file).Any()) return;
                var tkn = await Regata.Utilities.WebDavClientApi.MakeShareable(file, ct.Token);
                await _ic.Spectra.AddAsync(new SharedSpectra() { token = tkn, fileS = Path.GetFileNameWithoutExtension(file) });
                await _ic.SaveChangesAsync();
            }
        }

        private static async Task<string> Test()
        {
            var path = @"D:\Spectra\2020\01\dji-1\1107778.cnf";
            var _httpClient = new HttpClient();
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(":")));
            string _hostBase = @"https://disk.jinr.ru";
            string _hostWebDavAPI = @"/remote.php/dav/files/regata";
            string _hostOCSApi = @"/ocs/v2.php/apps/files_sharing/api/v1/shares";
            var request = new HttpRequestMessage(new HttpMethod("PROPFIND"), $"{_hostBase}{_hostWebDavAPI}/{path.Substring(Path.GetPathRoot(path).Length)}");
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            //HttpContent bytesContent = new ByteArrayContent(File.ReadAllBytes(path));
            //var response = await _httpClient.PutAsync($"{_hostBase}{_hostWebDavAPI}/{path.Substring(Path.GetPathRoot(path).Length)}", bytesContent).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync();

            
        }




    }
}
