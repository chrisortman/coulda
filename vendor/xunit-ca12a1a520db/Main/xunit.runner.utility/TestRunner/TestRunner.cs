using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Xunit
{
    /// <summary>
    /// Runs tests in an assembly, and transforms the XML results into calls to
    /// the provided <see cref="IRunnerLogger"/>.
    /// </summary>
    public class TestRunner : ITestRunner
    {
        readonly IExecutorWrapper wrapper;
        readonly IRunnerLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRunner"/> class.
        /// </summary>
        /// <param name="executorWrapper">The executor wrapper.</param>
        /// <param name="logger">The logger.</param>
        public TestRunner(IExecutorWrapper executorWrapper, IRunnerLogger logger)
        {
            wrapper = executorWrapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public virtual TestRunnerResult RunAssembly()
        {
            return RunAssembly(new IResultXmlTransform[0]);
        }

        /// <inheritdoc/>
        public virtual TestRunnerResult RunAssembly(IEnumerable<IResultXmlTransform> transforms)
        {
            XmlNode assemblyNode = null;
            logger.AssemblyStart(wrapper.AssemblyFilename, wrapper.ConfigFilename, wrapper.XunitVersion);

            TestRunnerResult result = PreserveWorkingDirectoryAndCatchExceptions(() =>
            {
                assemblyNode = wrapper.RunAssembly(node => XmlLoggerAdapter.LogNode(node, logger));
                return TestRunnerResult.NoTests;
            });

            if (result == TestRunnerResult.Failed)
                return TestRunnerResult.Failed;
            if (assemblyNode == null)
                return TestRunnerResult.NoTests;

            string assemblyXml = assemblyNode.OuterXml;

            foreach (IResultXmlTransform transform in transforms)
                transform.Transform(assemblyXml);

            return ParseNodeForTestRunnerResult(assemblyNode);
        }

        /// <inheritdoc/>
        public virtual TestRunnerResult RunClass(string type)
        {
            return PreserveWorkingDirectoryAndCatchExceptions(() =>
            {
                XmlNode classNode = wrapper.RunClass(type, node => XmlLoggerAdapter.LogNode(node, logger));
                return ParseNodeForTestRunnerResult(classNode);
            });
        }

        /// <inheritdoc/>
        public virtual TestRunnerResult RunTest(string type, string method)
        {
            return PreserveWorkingDirectoryAndCatchExceptions(() =>
            {
                XmlNode classNode = wrapper.RunTest(type, method, node => XmlLoggerAdapter.LogNode(node, logger));
                return ParseNodeForTestRunnerResult(classNode);
            });
        }

        /// <inheritdoc/>
        public virtual TestRunnerResult RunTests(string type, List<string> methods)
        {
            return PreserveWorkingDirectoryAndCatchExceptions(() =>
            {
                XmlNode classNode = wrapper.RunTests(type, methods, node => XmlLoggerAdapter.LogNode(node, logger));
                return ParseNodeForTestRunnerResult(classNode);
            });
        }

        TestRunnerResult PreserveWorkingDirectoryAndCatchExceptions(TestRunnerDelegate func)
        {
            string workingDirectory = Directory.GetCurrentDirectory();

            try
            {
                return func();
            }
            catch (Exception ex)
            {
                logger.ExceptionThrown(wrapper.AssemblyFilename, ex);
                return TestRunnerResult.Failed;
            }
            finally
            {
                Directory.SetCurrentDirectory(workingDirectory);
            }
        }

        static TestRunnerResult ParseNodeForTestRunnerResult(XmlNode node)
        {
            string total = node.Attributes["total"].Value;
            string failed = node.Attributes["failed"].Value;

            if (total == "0")
                return TestRunnerResult.NoTests;
            if (failed == "0")
                return TestRunnerResult.Passed;

            return TestRunnerResult.Failed;
        }

        delegate TestRunnerResult TestRunnerDelegate();
    }
}