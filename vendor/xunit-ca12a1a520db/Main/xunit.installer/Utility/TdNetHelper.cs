using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Xunit.Installer
{
    public class TdNetHelper
    {
        public static bool CanBeEnabled
        {
            get
            {
                using (RegistryKey runnerKey = OpenKey())
                    return (runnerKey != null);
            }
        }

        public static bool IsEnabled
        {
            get
            {
                using (RegistryKey runnerKey = OpenKey())
                {
                    if (runnerKey == null)
                        return false;

                    using (RegistryKey xunitKey = runnerKey.OpenSubKey("xunit"))
                        return xunitKey != null;
                }
            }
        }

        static string RunPath
        {
            get { return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath); }
        }

        public static bool Disable()
        {
            try
            {
                using (RegistryKey runnerKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MutantDesign\TestDriven.NET\TestRunners", true))
                    if (runnerKey != null)
                        runnerKey.DeleteSubKeyTree("xunit");

                using (RegistryKey runnerKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MutantDesign\TestDriven.NET\TestRunners", true))
                    if (runnerKey != null)
                        runnerKey.DeleteSubKeyTree("xunit");
            }
            catch (ArgumentException) { }

            return true;
        }

        public static bool Enable()
        {
            using (RegistryKey runnerKey = OpenKey())
            {
                if (runnerKey == null)
                    return true;

                string xunitPath = Path.Combine(RunPath, "xunit.dll");
                string runnerPath = Path.Combine(RunPath, "xunit.runner.tdnet.dll");

                if (!File.Exists(xunitPath))
                {
                    MessageBox.Show("Installation failed because the following file could not be found:\r\n\r\n" + xunitPath, Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!File.Exists(runnerPath))
                {
                    MessageBox.Show("Installation failed because the following file could not be found:\r\n\r\n" + runnerPath, Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (RegistryKey xunitKey = runnerKey.OpenSubKey("xunit", true) ?? runnerKey.CreateSubKey("xunit"))
                {
                    xunitKey.SetValue("", "4");
                    xunitKey.SetValue("AssemblyPath", runnerPath);
                    xunitKey.SetValue("TypeName", "Xunit.Runner.TdNet.TdNetRunner");
                }
            }

            return true;
        }

        static RegistryKey OpenKey()
        {
            return
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MutantDesign\TestDriven.NET\TestRunners", true) ??
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MutantDesign\TestDriven.NET\TestRunners", true);
        }
    }
}