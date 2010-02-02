using System;
using System.IO;
using System.Windows.Forms;

namespace Xunit.Gui
{
    static class Program
    {
        public const string REGISTRY_KEY_XUNIT = @"Software\Microsoft\xUnit.net";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RunnerForm form = null;

            if (args.Length == 0)
                form = new RunnerForm();
            else if (args.Length == 1 && IsProjectFilename(args[0]))
                form = new RunnerForm(args[0]);
            else
            {
                foreach (string assemblyFilename in args)
                    if (IsProjectFilename(assemblyFilename))
                    {
                        MessageBox.Show("The xUnit.net GUI command line can only accept a list of assemblies, or a single test project file.", "xUnit.net Test Runner", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                form = new RunnerForm(args);
            }

            Application.Run(form);
        }

        static bool IsProjectFilename(string filename)
        {
            return Path.GetExtension(filename).Equals(".xunit", StringComparison.OrdinalIgnoreCase);
        }
    }
}
