using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Xunit.ConsoleClient
{
    public class CommandLine
    {
        public delegate bool FileExists(string fileName);

        Stack<string> arguments = new Stack<string>();
        string executablePath;

        protected CommandLine(string[] args)
        {
            for (int i = args.Length - 1; i >= 0; i--)
                arguments.Push(args[i]);

            executablePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            TeamCity = Environment.GetEnvironmentVariable("TEAMCITY_PROJECT_NAME") != null;
            Project = Parse();
        }

        public XunitProject Project { get; protected set; }

        public bool Silent { get; protected set; }

        public bool TeamCity { get; protected set; }

        public bool Wait { get; protected set; }

        protected virtual XunitProject GetMultiAssemblyProject(string filename)
        {
            return XunitProject.Load(filename);
        }

        static XunitProject GetSingleAssemblyProject(Dictionary<string, string> transforms, string assemblyFile, string configFile, bool noShadow)
        {
            XunitProjectAssembly assembly = new XunitProjectAssembly { AssemblyFilename = assemblyFile, ConfigFilename = configFile, ShadowCopy = !noShadow };
            foreach (var transform in transforms)
                assembly.Output.Add(transform.Key, transform.Value);

            XunitProject project = new XunitProject();
            project.AddAssembly(assembly);
            return project;
        }

        static void GuardNoOptionValue(KeyValuePair<string, string> option)
        {
            if (option.Value != null)
                throw new ArgumentException(String.Format("error: unknown command line option: {0}", option.Value));
        }

        static void GuardNoProjectFile(XunitProject project, KeyValuePair<string, string> option)
        {
            if (project != null)
                throw new ArgumentException(String.Format("the {0} command line option isn't valid for .xunit projects", option.Key));
        }

        public static bool IsProjectFilename(string filename)
        {
            return Path.GetExtension(filename).Equals(".xunit", StringComparison.OrdinalIgnoreCase);
        }

        public static CommandLine Parse(string[] args)
        {
            return new CommandLine(args);
        }

        protected virtual XunitProject Parse()
        {
            return Parse(fileName => File.Exists(fileName));
        }

        protected XunitProject Parse(FileExists fileExists)
        {
            Dictionary<string, string> transforms = new Dictionary<string, string>();
            string configFile = null;
            bool noShadow = false;
            XunitProject project = null;

            string filename = arguments.Pop();
            if (!fileExists(filename))
                throw new ArgumentException(String.Format("file not found: {0}", filename));

            if (IsProjectFilename(filename))
            {
                project = GetMultiAssemblyProject(filename);
            }
            else
            {
                if (arguments.Count > 0 && !arguments.Peek().StartsWith("/"))
                {
                    configFile = arguments.Pop();

                    if (!fileExists(configFile))
                        throw new ArgumentException(String.Format("config file not found: {0}", configFile));
                }
            }

            while (arguments.Count > 0)
            {
                KeyValuePair<string, string> option = PopOption(arguments);
                string optionName = option.Key.ToLowerInvariant();

                if (!optionName.StartsWith("/"))
                    throw new ArgumentException(String.Format("unknown command line option: {0}", option.Key));

                if (optionName == "/wait")
                {
                    GuardNoOptionValue(option);
                    Wait = true;
                }
                else if (optionName == "/silent")
                {
                    GuardNoOptionValue(option);
                    Silent = true;
                }
                else if (optionName == "/teamcity")
                {
                    GuardNoOptionValue(option);
                    TeamCity = true;
                }
                else if (optionName == "/noshadow")
                {
                    GuardNoProjectFile(project, option);
                    GuardNoOptionValue(option);
                    noShadow = true;
                }
                else
                {
                    GuardNoProjectFile(project, option);

                    if (option.Value == null)
                        throw new ArgumentException(String.Format("missing filename for {0}", option.Key));

                    transforms.Add(optionName.Substring(1), option.Value);
                }
            }

            if (project == null)
                project = GetSingleAssemblyProject(transforms, filename, configFile, noShadow);

            return project;
        }

        static KeyValuePair<string, string> PopOption(Stack<string> arguments)
        {
            string option = arguments.Pop();
            string value = null;

            if (arguments.Count > 0 && !arguments.Peek().StartsWith("/"))
                value = arguments.Pop();

            return new KeyValuePair<string, string>(option, value);
        }
    }
}
