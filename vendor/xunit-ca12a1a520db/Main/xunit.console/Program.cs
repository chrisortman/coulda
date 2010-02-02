using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Xunit.ConsoleClient
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            Console.WriteLine("xUnit.net console test runner ({0}-bit .NET {1})", IntPtr.Size * 8, Environment.Version);
            Console.WriteLine("Copyright (C) 2007-10 Microsoft Corporation.");

            if (args.Length == 0 || args[0] == "/?")
            {
                PrintUsage();
                return -1;
            }

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            try
            {
                CommandLine commandLine = CommandLine.Parse(args);
                                
                int failCount = RunProject(commandLine.Project, commandLine.TeamCity, commandLine.Silent);

                if (commandLine.Wait)
                {
                    Console.WriteLine();
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.WriteLine();
                }

                return failCount;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine();
                Console.WriteLine("error: {0}", ex.Message);
                return -1;
            }
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            if (ex != null)
                Console.WriteLine(ex.ToString());
            else
                Console.WriteLine("Error of unknown type thrown in applicaton domain");

            Environment.Exit(1);
        }

        static void PrintUsage()
        {
            string executableName = Path.GetFileNameWithoutExtension(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            Console.WriteLine();
            Console.WriteLine("usage: {0} <xunitProjectFile> [options]", executableName);
            Console.WriteLine("usage: {0} <assemblyFile> [configFile] [options]", executableName);
            Console.WriteLine();
            Console.WriteLine("Valid options:");
            Console.WriteLine("  /silent                : do not output running test count");
            Console.WriteLine("  /teamcity              : forces TeamCity mode (normally auto-detected)");
            Console.WriteLine("  /wait                  : wait for input after completion");
            Console.WriteLine();
            Console.WriteLine("Valid options for assemblies only:");
            Console.WriteLine("  /noshadow              : do not shadow copy assemblies");
            Console.WriteLine("  /xml <filename>        : output results to Xunit-style XML file");

            foreach (TransformConfigurationElement transform in TransformFactory.GetInstalledTransforms())
            {
                string commandLine = "/" + transform.CommandLine + " <filename>";
                commandLine = commandLine.PadRight(22).Substring(0, 22);

                Console.WriteLine("  {0} : {1}", commandLine, transform.Description);
            }
        }

        static int RunProject(XunitProject project, bool teamcity, bool silent)
        {
            int totalAssemblies = 0;
            int totalTests = 0;
            int totalFailures = 0;
            int totalSkips = 0;
            double totalTime = 0;

            foreach (XunitProjectAssembly assembly in project.Assemblies)
                using (ExecutorWrapper wrapper = new ExecutorWrapper(assembly.AssemblyFilename, assembly.ConfigFilename, assembly.ShadowCopy))
                {
                    Console.WriteLine();
                    Console.WriteLine("xunit.dll:     Version {0}", wrapper.XunitVersion);
                    Console.WriteLine("Test assembly: {0}", Path.GetFullPath(assembly.AssemblyFilename));
                    Console.WriteLine();

                    try
                    {
                        List<IResultXmlTransform> transforms = TransformFactory.GetAssemblyTransforms(assembly);

                        Logger logger = teamcity
                            ? (Logger)new TeamCityLogger()
                            : new StandardLogger(silent, wrapper.GetAssemblyTestCount());

                        new TestRunner(wrapper, logger).RunAssembly(transforms);

                        ++totalAssemblies;
                        totalTests += logger.TotalTests;
                        totalFailures += logger.TotalFailures;
                        totalSkips += logger.TotalSkips;
                        totalTime += logger.TotalTime;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            if (!teamcity && totalAssemblies > 1)
            {
                Console.WriteLine();
                Console.WriteLine("=== {0} total, {1} failed, {2} skipped, took {3} seconds ===",
                                   totalTests, totalFailures, totalSkips, totalTime.ToString("0.000", CultureInfo.InvariantCulture));
            }

            return totalFailures;
        }
    }
}