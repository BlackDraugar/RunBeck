using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBeckTest
{
    class Program
    {
        static string FileType { get; set; }
        static int NumOfRecords { get; set; }

        static string FileLocation { get; set; }
        

        //NOTE: There is a CSVReader nutget package which I did not use for this project because I did not know if that was allowed.
        //NOTE: The app generates 2 possible files. Those are created in C:\Temp. The path can be change in the code at any time

        static void Main(string[] args)
        {
            ReadFile();  // Read File will call the analyze file and Create the bad and good files. 

        }

        static void ReadFile()
        {

            Console.WriteLine("Where is the file located");
            FileLocation = Console.ReadLine();
            Console.WriteLine("Is the file format CSV (comma-separated values) or TSV (tab-separated values)?");

            FileType = string.Empty;
            while (FileType != "1" || FileType != "2")
            {
                Console.WriteLine("Please select 1 for CVS and 2 for TSV");
                FileType = Console.ReadLine();
                if (FileType == "1" | FileType == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You did not select the correct option. Please try again");
                    continue;

                }
            }

            #region Validating # of Records
            Console.WriteLine("How many records should each record contain?");
            bool parseNumber = false;
            while (parseNumber == false)
            {
                int numRecords;
                parseNumber = int.TryParse(Console.ReadLine(), out numRecords);
                if (!parseNumber)
                {
                    Console.WriteLine("Please select a Number");
                    continue;
                }
                else
                {
                    NumOfRecords = numRecords;
                    break;
                };
            }

            #endregion

            if (FileType == "1") // CSV type
            {
                AnalyseFile(';', ',');
            }

            if (FileType == "2")  // TSV Type
            {
                AnalyseFile('\n', '\t');
            }


        }

        static void AnalyseFile(char SplitFile, char SplitLine)
        {
            var file = File.ReadAllLines(FileLocation).Select(a => a.Split(SplitFile));
            var docHeather = file.ToList()[0];

            List<string[]> goodlist = new List<string[]>();
            List<string[]> badlist = new List<string[]>();
            foreach (var line in file)
            {
                var splitLine = line.Select(x => x.Split(SplitLine)).ToList();

                if (docHeather[0] == line[0])
                { // Removes the header from the equation
                    continue;
                }

                if (splitLine.Count() > 0)
                {
                    var splitList = splitLine[0].ToList();

                    var count = splitLine[0].Count();

                    if (count == NumOfRecords)
                    {
                        goodlist.Add(line);
                    }
                    else
                    {
                        badlist.Add(line);
                    }
                }
            }

            if (goodlist.Count > 0)
            {
                using (var goodFile = File.CreateText("C:\\Temp\\Goodlist.csv"))
                {
                    foreach (var arr in goodlist)
                    {
                        goodFile.WriteLine(arr.FirstOrDefault());
                    }
                }
            }

            if (badlist.Count > 0)
            {
                using (var badFile = File.CreateText("C:\\Temp\\Badlist.csv"))
                {
                    foreach (var arr in badlist)
                    {
                        badFile.WriteLine(arr.FirstOrDefault());
                    }
                }
            }

        }
        
    }

}
