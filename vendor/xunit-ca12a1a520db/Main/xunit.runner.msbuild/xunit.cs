using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Xunit.Runner.MSBuild
{
    public class xunit : Task
    {
        public xunit()
        {
            ShadowCopy = true;
            TeamCity = Environment.GetEnvironmentVariable("TEAMCITY_PROJECT_NAME") != null;
        }

        [Required]
        public ITaskItem Assembly { get; set; }

        public string ConfigFile { get; set; }

        [Output]
        public int ExitCode { get; private set; }

        public ITaskItem Html { get; set; }

        public ITaskItem NUnitXml { get; set; }

        public bool ShadowCopy { get; set; }

        public bool TeamCity { get; set; }

        public bool Verbose { get; set; }

        public string WorkingFolder { get; set; }

        public ITaskItem Xml { get; set; }

        public override bool Execute()
        {
            try
            {
                string assemblyFilename = Assembly.GetMetadata("FullPath");

                if (WorkingFolder != null)
                    Directory.SetCurrentDirectory(WorkingFolder);

                using (ExecutorWrapper wrapper = new ExecutorWrapper(assemblyFilename, ConfigFile, ShadowCopy))
                {
                    Log.LogMessage(MessageImportance.High, "xUnit.net MSBuild runner ({0}-bit .NET {1})", IntPtr.Size * 8, Environment.Version);
                    Log.LogMessage(MessageImportance.High, "Test assembly: {0}", assemblyFilename);
                    Log.LogMessage(MessageImportance.High, "xunit.dll version: {0}", wrapper.XunitVersion);

                    IRunnerLogger logger =
                        TeamCity ? (IRunnerLogger)new TeamCityLogger(Log) :
                        Verbose ? new VerboseLogger(Log) :
                        new StandardLogger(Log);

                    List<IResultXmlTransform> transforms = new List<IResultXmlTransform>();

                    using (Stream htmlStream = ResourceStream("HTML.xslt"))
                    using (Stream nunitStream = ResourceStream("NUnitXml.xslt"))
                    {
                        if (Xml != null)
                            transforms.Add(new NullTransformer(Xml.GetMetadata("FullPath")));
                        if (Html != null)
                            transforms.Add(new XslStreamTransformer(htmlStream, Html.GetMetadata("FullPath")));
                        if (NUnitXml != null)
                            transforms.Add(new XslStreamTransformer(nunitStream, NUnitXml.GetMetadata("FullPath")));

                        TestRunner runner = new TestRunner(wrapper, logger);
                        if (runner.RunAssembly(transforms) == TestRunnerResult.Failed)
                        {
                            ExitCode = -1;
                            return false;
                        }

                        ExitCode = 0;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exception e = ex;

                while (e != null)
                {
                    Log.LogError(e.GetType().FullName + ": " + e.Message);

                    foreach (string stackLine in e.StackTrace.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                        Log.LogError(stackLine);

                    e = e.InnerException;
                }

                ExitCode = -1;
                return false;
            }
        }

        static Stream ResourceStream(string xmlResourceName)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Xunit.Runner.MSBuild." + xmlResourceName);
        }
    }
}