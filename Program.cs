using System;
using System.IO;
using System.CommandLine.DragonFruit;

namespace flow
{
    class Program
    {
        /// <summary>
        /// General information about flow.exe
        /// </summary>
        /// <param name="intoption">Here is desc of intOption</param>
        /// <param name="boolOption">Here is desc of boolOption</param>
        /// <param name="fileOption">Here is desc of fileOption</param>
        static void Main(int intoption = 42, bool boolOption = false, FileInfo fileOption = null)
        {
            Console.WriteLine($"The value for --int-option is: {intoption}");
            Console.WriteLine($"The value for --bool-option is: {boolOption}");
            Console.WriteLine($"The value for --file-option is: {fileOption?.FullName ?? "null"}");

        }
    }
}
