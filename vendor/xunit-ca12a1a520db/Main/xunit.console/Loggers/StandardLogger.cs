using System;
using System.Globalization;

namespace Xunit.ConsoleClient
{
    public class StandardLogger : Logger
    {
        readonly bool silent;
        int testCount = 0;
        readonly int totalCount;

        public StandardLogger(bool silent, int totalCount)
        {
            this.silent = silent;
            this.totalCount = totalCount;
        }

        public override void AssemblyFinished(string assemblyFilename, int total, int failed, int skipped, double time)
        {
            base.AssemblyFinished(assemblyFilename, total, failed, skipped, time);

            if (!silent)
                Console.Write("\r");

            Console.WriteLine("{0} total, {1} failed, {2} skipped, took {3} seconds", total, failed, skipped, time.ToString("0.000", CultureInfo.InvariantCulture));
        }

        public override bool ClassFailed(string className, string exceptionType, string message, string stackTrace)
        {
            if (!silent)
                Console.Write("\r");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} [FIXTURE FAIL]", className);
            Console.ResetColor();

            Console.WriteLine(Indent(message));

            if (stackTrace != null)
            {
                Console.WriteLine(Indent("Stack Trace:"));
                Console.WriteLine(Indent(StackFrameTransformer.TransformStack(stackTrace)));
            }

            Console.WriteLine();
            return true;
        }

        public override void ExceptionThrown(string assemblyFilename, Exception exception)
        {
            throw new System.NotImplementedException();
        }

        public override void TestFailed(string name, string type, string method, double duration, string output, string exceptionType, string message, string stackTrace)
        {
            if (!silent)
                Console.Write("\r");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} [FAIL]", name);
            Console.ResetColor();

            Console.WriteLine(Indent(message));

            if (stackTrace != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(Indent("Stack Trace:"));
                Console.ResetColor();

                Console.WriteLine(Indent(StackFrameTransformer.TransformStack(stackTrace)));
            }

            Console.WriteLine();
        }

        public override bool TestFinished(string name, string type, string method)
        {
            if (!silent)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\rTests complete: {0} of {1}", ++testCount, totalCount);
                Console.ResetColor();
            }

            return true;
        }

        public override void TestSkipped(string name, string type, string method, string reason)
        {
            if (!silent)
                Console.Write("\r");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0} [SKIP]", name);
            Console.ResetColor();

            Console.WriteLine(Indent(reason));
            Console.WriteLine();
        }

        public override bool TestStart(string name, string type, string method)
        {
            return true;
        }

        // Helpers

        string Indent(string message)
        {
            return Indent(message, 0);
        }

        string Indent(string message, int additionalSpaces)
        {
            string result = "";
            string indent = "".PadRight(additionalSpaces + 3);

            foreach (string line in message.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                result += indent + line + Environment.NewLine;

            return result.TrimEnd();
        }
    }
}