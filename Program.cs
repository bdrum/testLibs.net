using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace WorkFlowCore
{

    public class A
    { }
    public class Program
    {

        static void Main(string[] args)
        {
            string fileName = "labels.json";
            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) };
            string jsonString = File.ReadAllText(fileName);
            var a = JsonSerializer.Deserialize<A>(jsonString, options);
        }
    }
}