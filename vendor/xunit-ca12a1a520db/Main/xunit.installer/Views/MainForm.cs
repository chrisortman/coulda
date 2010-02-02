using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Xunit.Installer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            Text = Program.Name + " (build " + Assembly.GetExecutingAssembly().GetName().Version + ")";

            Font boldFont = new Font(labelStatusTdNet.Font, FontStyle.Bold);
            labelStatusMVC.Font = boldFont;
            labelStatusMVC2.Font = boldFont;
            labelStatusTdNet.Font = boldFont;
            labelStatusGuiExplorer.Font = boldFont;

            UpdateUI();
        }

        void UpdateUI()
        {
            UpdateUI(btnEnableMVC,
                     btnDisableMVC,
                     labelStatusMVC,
                     "ASP.NET MVC 1.0",
                     Mvc1Helper.CanBeEnabled,
                     Mvc1Helper.IsEnabled,
                     Mvc1Helper.InstalledXunitVersion);

            UpdateUI(btnEnableMVC2,
                     btnDisableMVC2,
                     labelStatusMVC2,
                     "ASP.NET MVC 2",
                     Mvc2Helper.CanBeEnabled,
                     Mvc2Helper.IsEnabled,
                     Mvc2Helper.InstalledXunitVersion);

            UpdateUI(btnEnableTdNet,
                     btnDisableTdNet,
                     labelStatusTdNet,
                     "TestDriven.NET 2.x",
                     TdNetHelper.CanBeEnabled,
                     TdNetHelper.IsEnabled,
                     () => "(Can run tests against any version of xUnit.net)");

            UpdateUI(btnEnableGuiExplorer,
                     btnDisableGuiExplorer,
                     labelStatusGuiExplorer,
                     "Project File Explorer Integration",
                     true,
                     GuiExplorerHelper.IsEnabled,
                     null);
        }

        static void UpdateUI(Control enable, Control disable, Control status, string toolName, bool isToolInstalled,
                             bool isRunnerInstalled, InstalledStatusDelegate installedStatusDelegate)
        {
            if (!isToolInstalled)
            {
                enable.Enabled = false;
                disable.Enabled = false;
                status.Text = toolName + " is not installed.\r\nYou must install it to enable this feature.";
                status.ForeColor = Color.DarkRed;
                return;
            }

            if (isRunnerInstalled)
            {
                enable.Enabled = false;
                disable.Enabled = true;
                status.Text = "Support for " + toolName + " is enabled.";
                status.ForeColor = Color.Green;

                if (installedStatusDelegate != null)
                    status.Text += Environment.NewLine + "  " + installedStatusDelegate();
            }
            else
            {
                enable.Enabled = true;
                disable.Enabled = false;
                status.Text = "Support for " + toolName + " is disabled.";
                status.ForeColor = Color.DarkGoldenrod;
            }
        }

        // Event handlers

        void btnDisableGuiExplorer_Click(object sender, EventArgs e)
        {
            GuiExplorerHelper.Disable();
            UpdateUI();
        }

        void btnDisableMVC_Click(object sender, EventArgs e)
        {
            if (!Mvc1Helper.Disable(this))
                MessageBox.Show("Uninstallation failed. Make sure Visual Studio is not running and try again.",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            UpdateUI();
        }

        void btnDisableMVC2_Click(object sender, EventArgs e)
        {
            if (!Mvc2Helper.Disable(this))
                MessageBox.Show("Uninstallation failed. Make sure Visual Studio is not running and try again.",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            UpdateUI();
        }

        void btnDisableTdNet_Click(object sender,
                                     EventArgs e)
        {
            if (!TdNetHelper.Disable())
                MessageBox.Show("Uninstallation failed. Make sure Visual Studio is not running and try again.",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            UpdateUI();
        }

        void btnEnableGuiExplorer_Click(object sender, EventArgs e)
        {
            GuiExplorerHelper.Enable();
            UpdateUI();
        }

        void btnEnableMVC_Click(object sender, EventArgs e)
        {
            if (!Mvc1Helper.Enable(this))
                MessageBox.Show("Installation failed. Make sure Visual Studio is not running and try again.",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            UpdateUI();
        }

        void btnEnableMVC2_Click(object sender, EventArgs e)
        {
            if (!Mvc2Helper.Enable(this))
                MessageBox.Show("Installation failed. Make sure Visual Studio is not running and try again.",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            UpdateUI();
        }

        void btnEnableTdNet_Click(object sender, EventArgs e)
        {
            if (!TdNetHelper.Enable())
                MessageBox.Show("Installation failed. Make sure Visual Studio is not running and try again.",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            UpdateUI();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public delegate string InstalledStatusDelegate();
}