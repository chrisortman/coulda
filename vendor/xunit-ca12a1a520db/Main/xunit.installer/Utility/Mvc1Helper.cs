using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Xunit.Installer
{
    public static class Mvc1Helper
    {
        const string TEMPLATE_NAME_CSHARP = "XunitMvcTestProjectTemplate.cs.zip";
        const string TEMPLATE_NAME_VB = "XunitMvcTestProjectTemplate.vb.zip";

        public static bool CanBeEnabled
        {
            get
            {
                using (RegistryKey key = OpenMvcKey())
                    return key != null;
            }
        }

        public static bool IsEnabled
        {
            get
            {
                using (RegistryKey key = OpenMvcKey())
                {
                    if (key == null)
                        return false;

                    using (RegistryKey templateKey = key.OpenSubKey("xUnit.net"))
                        return templateKey != null;
                }
            }
        }

        public static bool Disable(Form owner)
        {
            using (RegistryKey key = OpenMvcKey())
                key.DeleteSubKeyTree("xUnit.net");

            UninstallZipFiles(VS2008Helper.GetTestProjectTemplatePath("CSharp"));
            UninstallZipFiles(VS2008Helper.GetTestProjectTemplatePath("VisualBasic"));

            VS2008Helper.ResetVisualStudio(owner);
            return true;
        }

        public static bool Enable(Form owner)
        {
            ResourceHelper.WriteResourceToFile("Xunit.Installer.Templates.MVC1-CS-VS2008.zip",
                                               Path.Combine(VS2008Helper.GetTestProjectTemplatePath("CSharp"), TEMPLATE_NAME_CSHARP));
            ResourceHelper.WriteResourceToFile("Xunit.Installer.Templates.MVC1-VB-VS2008.zip",
                                               Path.Combine(VS2008Helper.GetTestProjectTemplatePath("VisualBasic"), TEMPLATE_NAME_VB));

            using (RegistryKey key = OpenMvcKey())
            using (RegistryKey templateKey = key.CreateSubKey("xUnit.net"))
            {
                using (RegistryKey csharpKey = templateKey.CreateSubKey("C#"))
                {
                    csharpKey.SetValue("AdditionalInfo", "http://www.codeplex.com/xunit");
                    csharpKey.SetValue("Package", "");
                    csharpKey.SetValue("Path", @"CSharp\Test");
                    csharpKey.SetValue("Template", TEMPLATE_NAME_CSHARP);
                    csharpKey.SetValue("TestFrameworkName", "xUnit.net build " + Assembly.GetExecutingAssembly().GetName().Version);
                }
                using (RegistryKey vbKey = templateKey.CreateSubKey("VB"))
                {
                    vbKey.SetValue("AdditionalInfo", "http://www.codeplex.com/xunit");
                    vbKey.SetValue("Package", "");
                    vbKey.SetValue("Path", @"VisualBasic\Test");
                    vbKey.SetValue("Template", TEMPLATE_NAME_VB);
                    vbKey.SetValue("TestFrameworkName", "xUnit.net build " + Assembly.GetExecutingAssembly().GetName().Version);
                }
            }

            VS2008Helper.ResetVisualStudio(owner);
            return true;
        }

        public static string InstalledXunitVersion()
        {
            using (RegistryKey key = OpenMvcKey())
            using (RegistryKey csharpKey = key.OpenSubKey(@"xUnit.net\C#"))
                return String.Format("(With templates for {0})", csharpKey.GetValue("TestFrameworkName"));
        }

        static RegistryKey OpenMvcKey()
        {
            return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates", true);
        }

        static void UninstallZipFiles(string templatePath)
        {
            foreach (string file in Directory.GetFiles(templatePath, "XunitMvcTestProjectTemplate.*"))
                File.Delete(Path.Combine(templatePath, file));

            foreach (string directory in Directory.GetDirectories(templatePath))
                UninstallZipFiles(Path.Combine(templatePath, directory));
        }
    }
}