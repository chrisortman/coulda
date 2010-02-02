using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.UI;
using System.Xml;

namespace Xunit
{
    /// <summary>
    /// Wraps calls to the Executor. Used by runners to perform version-resilient test
    /// enumeration and execution.
    /// </summary>
    public class ExecutorWrapper : IExecutorWrapper
    {
        readonly AppDomain appDomain;
        readonly object executor;
        readonly AssemblyName xunitAssemblyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutorWrapper"/> class.
        /// </summary>
        /// <param name="assemblyFilename">The assembly filename.</param>
        /// <param name="configFilename">The config filename. If null, the default config filename will be used.</param>
        /// <param name="shadowCopy">Set to true to enable shadow copying; false, otherwise.</param>
        public ExecutorWrapper(string assemblyFilename, string configFilename, bool shadowCopy)
        {
            AssemblyFilename = assemblyFilename;
            ConfigFilename = configFilename;

            assemblyFilename = Path.GetFullPath(assemblyFilename);
            if (!File.Exists(assemblyFilename))
                throw new ArgumentException("Could not find file: " + assemblyFilename);

            if (configFilename == null)
                configFilename = GetDefaultConfigFile(assemblyFilename);

            if (configFilename != null)
                configFilename = Path.GetFullPath(configFilename);

            appDomain = CreateAppDomain(assemblyFilename, configFilename ?? "", shadowCopy);

            try
            {
                string xunitAssemblyFilename = Path.Combine(Path.GetDirectoryName(assemblyFilename), "xunit.dll");

                if (!File.Exists(xunitAssemblyFilename))
                    throw new ArgumentException("Could not find file: " + xunitAssemblyFilename);

                xunitAssemblyName = AssemblyName.GetAssemblyName(xunitAssemblyFilename);
                executor = CreateObject("Xunit.Sdk.Executor", AssemblyFilename);
            }
            catch (TargetInvocationException ex)
            {
                Dispose();
                RethrowWithNoStackTraceLoss(ex.InnerException);
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <inheritdoc/>
        public string AssemblyFilename { get; private set; }

        /// <inheritdoc/>
        public string ConfigFilename { get; private set; }

        /// <inheritdoc/>
        public string XunitVersion
        {
            get { return xunitAssemblyName.Version.ToString(); }
        }

        static AppDomain CreateAppDomain(string assemblyFilename, string configFilename, bool shadowCopy)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = Path.GetDirectoryName(assemblyFilename);
            setup.ApplicationName = Guid.NewGuid().ToString();

            if (shadowCopy)
            {
                setup.ShadowCopyFiles = "true";
                setup.ShadowCopyDirectories = setup.ApplicationBase;
                setup.CachePath = Path.Combine(Path.GetTempPath(), setup.ApplicationName);
            }

            setup.ConfigurationFile = configFilename;

            return AppDomain.CreateDomain(setup.ApplicationName, null, setup);
        }

        object CreateObject(string typeName, params object[] args)
        {
            try
            {
                return appDomain.CreateInstanceAndUnwrap(xunitAssemblyName.FullName, typeName, false, 0, null, args, null, null, null);
            }
            catch (TargetInvocationException ex)
            {
                RethrowWithNoStackTraceLoss(ex.InnerException);
                return null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (appDomain != null)
            {
                string cachePath = appDomain.SetupInformation.CachePath;

                AppDomain.Unload(appDomain);

                try
                {
                    if (cachePath != null)
                        Directory.Delete(cachePath, true);
                }
                catch { }
            }
        }

        /// <inheritdoc/>
        public XmlNode EnumerateTests()
        {
            XmlNodeCallbackWrapper wrapper = new XmlNodeCallbackWrapper(null);

            CreateObject("Xunit.Sdk.Executor+EnumerateTests", executor, wrapper);

            return wrapper.LastNode;
        }

        /// <inheritdoc/>
        public int GetAssemblyTestCount()
        {
            CallbackHandler<int> handler = new CallbackHandler<int>();

            CreateObject("Xunit.Sdk.Executor+AssemblyTestCount", executor, handler);

            return handler.Result;
        }

        static string GetDefaultConfigFile(string assemblyFile)
        {
            string configFilename = assemblyFile + ".config";

            if (File.Exists(configFilename))
                return configFilename;

            return null;
        }

        /// <inheritdoc/>
        public XmlNode RunAssembly(Predicate<XmlNode> callback)
        {
            XmlNodeCallbackWrapper wrapper = new XmlNodeCallbackWrapper(callback, "assembly");

            CreateObject("Xunit.Sdk.Executor+RunAssembly", executor, wrapper);

            wrapper.LastNodeArrived.WaitOne();

            return wrapper.LastNode;
        }

        /// <inheritdoc/>
        public XmlNode RunClass(string type, Predicate<XmlNode> callback)
        {
            XmlNodeCallbackWrapper wrapper = new XmlNodeCallbackWrapper(callback, "class");

            CreateObject("Xunit.Sdk.Executor+RunClass", executor, type, wrapper);

            wrapper.LastNodeArrived.WaitOne();

            return wrapper.LastNode;
        }

        /// <inheritdoc/>
        public XmlNode RunTest(string type, string method, Predicate<XmlNode> callback)
        {
            XmlNodeCallbackWrapper wrapper = new XmlNodeCallbackWrapper(callback, "class");

            CreateObject("Xunit.Sdk.Executor+RunTest", executor, type, method, wrapper);

            wrapper.LastNodeArrived.WaitOne();

            return wrapper.LastNode;
        }

        /// <inheritdoc/>
        public XmlNode RunTests(string type, List<string> methods, Predicate<XmlNode> callback)
        {
            XmlNodeCallbackWrapper wrapper = new XmlNodeCallbackWrapper(callback, "class");

            CreateObject("Xunit.Sdk.Executor+RunTests", executor, type, methods, wrapper);

            wrapper.LastNodeArrived.WaitOne();

            return wrapper.LastNode;
        }

        static void RethrowWithNoStackTraceLoss(Exception ex)
        {
            FieldInfo remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
            remoteStackTraceString.SetValue(ex, ex.StackTrace + Environment.NewLine);
            throw ex;
        }

        class CallbackHandler<T> : MarshalByRefObject, ICallbackEventHandler
        {
            public T Result { get; private set; }

            public CallbackHandler() { }

            public CallbackHandler(T defaultValue)
            {
                Result = defaultValue;
            }

            string ICallbackEventHandler.GetCallbackResult()
            {
                throw new NotImplementedException();
            }

            void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
            {
                Result = (T)Convert.ChangeType(eventArgument, typeof(T));
            }

            /// <summary/>
            public override Object InitializeLifetimeService()
            {
                return null;
            }
        }

        class XmlNodeCallbackWrapper : MarshalByRefObject, ICallbackEventHandler
        {
            readonly Predicate<XmlNode> callback;
            bool @continue = true;
            readonly string lastNodeName;

            public XmlNode LastNode { get; private set; }
            public ManualResetEvent LastNodeArrived { get; private set; }

            public XmlNodeCallbackWrapper(Predicate<XmlNode> callback)
                : this(callback, null) { }

            public XmlNodeCallbackWrapper(Predicate<XmlNode> callback, string lastNodeName)
            {
                this.callback = callback;
                this.lastNodeName = lastNodeName;

                LastNodeArrived = new ManualResetEvent(false);
            }

            public void RaiseCallbackEvent(string result)
            {
                if (result != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    LastNode = doc.ChildNodes[0];

                    if (callback != null)
                        @continue = callback(LastNode);

                    if (lastNodeName != null && LastNode.Name == lastNodeName)
                        LastNodeArrived.Set();
                }
            }

            public string GetCallbackResult()
            {
                return @continue.ToString();
            }

            /// <summary/>
            public override Object InitializeLifetimeService()
            {
                return null;
            }
        }
    }
}