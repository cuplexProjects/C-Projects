using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OrganaizeTV_Series.Configuration;
using OrganaizeTV_Series.Models;
using OrganaizeTV_Series.Services;

namespace OrganaizeTV_Series
{
    static class Program
    {
        static void Main(string[] args)
        {
            int argumentIndex = 0;
            if (args.Length == 0)
            {
                Console.WriteLine("Missing -path");
                PrintHelp();
                return;
            }

            NameValueCollection commandAndValueCollection = new NameValueCollection { { args[argumentIndex++], args.Length == 1 ? "" : args[argumentIndex++] } };
            var settingsManager = new AppSettingsManager();

            while (argumentIndex >= args.Length)
            {
                commandAndValueCollection.Add(args[argumentIndex++], argumentIndex >= args.Length ? "" : args[argumentIndex++]);
            }

            InputArgumentsModel argumentInputs = new InputArgumentsModel { SeasonName = "Season ", AutoCreateSeasonsFolders = true };
            switch (commandAndValueCollection.GetKey(0).ToLower())
            {
                case "-h":
                case "-help":
                    PrintHelp();
                    return;

                case "-basepath":
                case "-path":
                case "-p":

                    string filePath = commandAndValueCollection[0];
                    if (string.IsNullOrWhiteSpace(filePath) || !DirectoryExists(filePath))
                    {
                        Console.WriteLine("Invalid path selected or audit rights makes it inaccessable.");
                        return;
                    }

                    argumentInputs.BasePath = filePath;

                    break;
                default:
                    Console.WriteLine("Could not parse first argument");
                    return;
            }

            // Remove already processed arg
            commandAndValueCollection.Remove(commandAndValueCollection.GetKey(0));
            foreach (string key in commandAndValueCollection)
            {
                switch (key.ToLower())
                {
                    case "customseasonnaming":
                    case "customnaming":
                        string customName = commandAndValueCollection[key].Trim();
                        if (string.IsNullOrEmpty(customName) || !ValidSeasonPattern(customName))
                        {
                            Console.WriteLine("Invalid format for CustomSeasonNaming");
                            Console.WriteLine("A valid Pattern would be ");
                            return;
                        }

                        argumentInputs.SeasonName = commandAndValueCollection[key];
                        break;

                    case "-h":
                    case "-help":
                        PrintHelp();
                        return;

                    default:
                        Console.WriteLine($"Invalid command {key}: {commandAndValueCollection[key]}");


                        break;
                }
            }

            // wait here until the job finishes 
            var workflowService = new DiskStructureService(settingsManager, argumentInputs);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            workflowService.StartWorkflow();
            stopwatch.Stop();;

            Console.WriteLine($"Completed reorganization of folder structure in {stopwatch.Elapsed}");
        }

        private static bool DirectoryExists(string directory)
        {
            try
            {
                return Directory.Exists(directory);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        private static bool ValidSeasonPattern(string seasonPattern)
        {
            //Regex dirNameRegex= new Regex(@"^ (\w +)(\s | -|\.| _)[\d]{ 1,6}$");
            Regex dirNameRegex = new Regex(@"^(\w+)[ _.-]{1}(\d{1,4})$");

            return dirNameRegex.IsMatch(seasonPattern);
        }

        private static void PrintHelp()
        {
            string applicationFileName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName(false).Name);
            Console.WriteLine("Name".ToUpper());
            Console.WriteLine($"    {applicationFileName}");

            Console.WriteLine("Syntax".ToUpper());
            Console.WriteLine($"    [{applicationFileName} [-BasePath, -Path, p] <string>] [[-CustomSeasonNaming, -customNaming] <string> (Default \"Season {'n'}\")]");
            Console.WriteLine("     [[-AutoCreate-SeasonsFolders, -ac] <yes/no> (default yes)] [-help, -h] (Displayes this help)");

            Console.WriteLine("Description".ToUpper());
            Console.WriteLine("The command rearanges folders to a standard tree level with -Title/Season(n)/-Original series folder, in case of files not having a normal parrent series folder with the syntax (SeriesName.S01.E01,....)");
            Console.WriteLine();
            Console.WriteLine("Remarks".ToUpper());
            Console.WriteLine("    ");
        }
    }
}
