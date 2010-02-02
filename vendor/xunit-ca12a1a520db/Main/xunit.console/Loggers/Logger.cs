using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.ConsoleClient
{
    public class Logger : IRunnerLogger
    {
        public int TotalTests { get; set; }
        public int TotalFailures { get; set; }
        public int TotalSkips { get; set; }
        public double TotalTime { get; set; }

        public virtual void AssemblyFinished(string assemblyFilename, int total, int failed, int skipped, double time)
        {
            TotalTests = total;
            TotalFailures = failed;
            TotalSkips = skipped;
            TotalTime = time;
        }

        public virtual void AssemblyStart(string assemblyFilename, string configFilename, string xUnitVersion)
        {
        }

        public virtual bool ClassFailed(string className, string exceptionType, string message, string stackTrace)
        {
            return true;
        }

        public virtual void ExceptionThrown(string assemblyFilename, Exception exception)
        {
        }

        public virtual void TestFailed(string name, string type, string method, double duration, string output, string exceptionType, string message, string stackTrace)
        {
        }

        public virtual bool TestFinished(string name, string type, string method)
        {
            return true;
        }

        public virtual void TestPassed(string name, string type, string method, double duration, string output)
        {
        }

        public virtual void TestSkipped(string name, string type, string method, string reason)
        {
        }

        public virtual bool TestStart(string name, string type, string method)
        {
            return true;
        }
    }
}
