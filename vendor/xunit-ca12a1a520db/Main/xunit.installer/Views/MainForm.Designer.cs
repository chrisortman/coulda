namespace Xunit.Installer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExit = new System.Windows.Forms.Button();
            this.groupTdNet = new System.Windows.Forms.GroupBox();
            this.btnDisableTdNet = new System.Windows.Forms.Button();
            this.btnEnableTdNet = new System.Windows.Forms.Button();
            this.labelStatusTdNet = new System.Windows.Forms.Label();
            this.groupMVC = new System.Windows.Forms.GroupBox();
            this.btnDisableMVC = new System.Windows.Forms.Button();
            this.btnEnableMVC = new System.Windows.Forms.Button();
            this.labelStatusMVC = new System.Windows.Forms.Label();
            this.groupMVC2 = new System.Windows.Forms.GroupBox();
            this.btnDisableMVC2 = new System.Windows.Forms.Button();
            this.btnEnableMVC2 = new System.Windows.Forms.Button();
            this.labelStatusMVC2 = new System.Windows.Forms.Label();
            this.btnDisableGuiExplorer = new System.Windows.Forms.Button();
            this.groupGuiExplorer = new System.Windows.Forms.GroupBox();
            this.btnEnableGuiExplorer = new System.Windows.Forms.Button();
            this.labelStatusGuiExplorer = new System.Windows.Forms.Label();
            this.groupTdNet.SuspendLayout();
            this.groupMVC.SuspendLayout();
            this.groupMVC2.SuspendLayout();
            this.groupGuiExplorer.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(393, 370);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(87, 25);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupTdNet
            // 
            this.groupTdNet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTdNet.Controls.Add(this.btnDisableTdNet);
            this.groupTdNet.Controls.Add(this.btnEnableTdNet);
            this.groupTdNet.Controls.Add(this.labelStatusTdNet);
            this.groupTdNet.Location = new System.Drawing.Point(14, 13);
            this.groupTdNet.Name = "groupTdNet";
            this.groupTdNet.Size = new System.Drawing.Size(472, 79);
            this.groupTdNet.TabIndex = 0;
            this.groupTdNet.TabStop = false;
            this.groupTdNet.Text = "TestDriven.NET";
            // 
            // btnDisableTdNet
            // 
            this.btnDisableTdNet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableTdNet.Location = new System.Drawing.Point(378, 46);
            this.btnDisableTdNet.Name = "btnDisableTdNet";
            this.btnDisableTdNet.Size = new System.Drawing.Size(87, 25);
            this.btnDisableTdNet.TabIndex = 2;
            this.btnDisableTdNet.Text = "&Disable";
            this.btnDisableTdNet.UseVisualStyleBackColor = true;
            this.btnDisableTdNet.Click += new System.EventHandler(this.btnDisableTdNet_Click);
            // 
            // btnEnableTdNet
            // 
            this.btnEnableTdNet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableTdNet.Location = new System.Drawing.Point(378, 15);
            this.btnEnableTdNet.Name = "btnEnableTdNet";
            this.btnEnableTdNet.Size = new System.Drawing.Size(87, 25);
            this.btnEnableTdNet.TabIndex = 1;
            this.btnEnableTdNet.Text = "&Enable";
            this.btnEnableTdNet.UseVisualStyleBackColor = true;
            this.btnEnableTdNet.Click += new System.EventHandler(this.btnEnableTdNet_Click);
            // 
            // labelStatusTdNet
            // 
            this.labelStatusTdNet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatusTdNet.Location = new System.Drawing.Point(7, 15);
            this.labelStatusTdNet.Name = "labelStatusTdNet";
            this.labelStatusTdNet.Size = new System.Drawing.Size(363, 56);
            this.labelStatusTdNet.TabIndex = 0;
            this.labelStatusTdNet.Text = "(TD.NET Status)";
            this.labelStatusTdNet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupMVC
            // 
            this.groupMVC.Controls.Add(this.btnDisableMVC);
            this.groupMVC.Controls.Add(this.btnEnableMVC);
            this.groupMVC.Controls.Add(this.labelStatusMVC);
            this.groupMVC.Location = new System.Drawing.Point(14, 103);
            this.groupMVC.Name = "groupMVC";
            this.groupMVC.Size = new System.Drawing.Size(472, 81);
            this.groupMVC.TabIndex = 2;
            this.groupMVC.TabStop = false;
            this.groupMVC.Text = "ASP.NET MVC 1.0 (C#/VB.net, VS2008)";
            // 
            // btnDisableMVC
            // 
            this.btnDisableMVC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableMVC.Location = new System.Drawing.Point(378, 45);
            this.btnDisableMVC.Name = "btnDisableMVC";
            this.btnDisableMVC.Size = new System.Drawing.Size(87, 25);
            this.btnDisableMVC.TabIndex = 2;
            this.btnDisableMVC.Text = "Dis&able";
            this.btnDisableMVC.UseVisualStyleBackColor = true;
            this.btnDisableMVC.Click += new System.EventHandler(this.btnDisableMVC_Click);
            // 
            // btnEnableMVC
            // 
            this.btnEnableMVC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableMVC.Location = new System.Drawing.Point(378, 14);
            this.btnEnableMVC.Name = "btnEnableMVC";
            this.btnEnableMVC.Size = new System.Drawing.Size(87, 25);
            this.btnEnableMVC.TabIndex = 1;
            this.btnEnableMVC.Text = "E&nable";
            this.btnEnableMVC.UseVisualStyleBackColor = true;
            this.btnEnableMVC.Click += new System.EventHandler(this.btnEnableMVC_Click);
            // 
            // labelStatusMVC
            // 
            this.labelStatusMVC.Location = new System.Drawing.Point(7, 22);
            this.labelStatusMVC.Name = "labelStatusMVC";
            this.labelStatusMVC.Size = new System.Drawing.Size(363, 48);
            this.labelStatusMVC.TabIndex = 0;
            this.labelStatusMVC.Text = "(ASP.NET MVC 1.0 Status)";
            this.labelStatusMVC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupMVC2
            // 
            this.groupMVC2.Controls.Add(this.btnDisableMVC2);
            this.groupMVC2.Controls.Add(this.btnEnableMVC2);
            this.groupMVC2.Controls.Add(this.labelStatusMVC2);
            this.groupMVC2.Location = new System.Drawing.Point(14, 190);
            this.groupMVC2.Name = "groupMVC2";
            this.groupMVC2.Size = new System.Drawing.Size(472, 81);
            this.groupMVC2.TabIndex = 3;
            this.groupMVC2.TabStop = false;
            this.groupMVC2.Text = "ASP.NET MVC 2 (C#/VB.net, VS2008)";
            // 
            // btnDisableMVC2
            // 
            this.btnDisableMVC2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableMVC2.Location = new System.Drawing.Point(378, 45);
            this.btnDisableMVC2.Name = "btnDisableMVC2";
            this.btnDisableMVC2.Size = new System.Drawing.Size(87, 25);
            this.btnDisableMVC2.TabIndex = 2;
            this.btnDisableMVC2.Text = "Disab&le";
            this.btnDisableMVC2.UseVisualStyleBackColor = true;
            this.btnDisableMVC2.Click += new System.EventHandler(this.btnDisableMVC2_Click);
            // 
            // btnEnableMVC2
            // 
            this.btnEnableMVC2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableMVC2.Location = new System.Drawing.Point(378, 14);
            this.btnEnableMVC2.Name = "btnEnableMVC2";
            this.btnEnableMVC2.Size = new System.Drawing.Size(87, 25);
            this.btnEnableMVC2.TabIndex = 1;
            this.btnEnableMVC2.Text = "Ena&ble";
            this.btnEnableMVC2.UseVisualStyleBackColor = true;
            this.btnEnableMVC2.Click += new System.EventHandler(this.btnEnableMVC2_Click);
            // 
            // labelStatusMVC2
            // 
            this.labelStatusMVC2.Location = new System.Drawing.Point(7, 22);
            this.labelStatusMVC2.Name = "labelStatusMVC2";
            this.labelStatusMVC2.Size = new System.Drawing.Size(363, 48);
            this.labelStatusMVC2.TabIndex = 0;
            this.labelStatusMVC2.Text = "(ASP.NET MVC 2 Status)";
            this.labelStatusMVC2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDisableGuiExplorer
            // 
            this.btnDisableGuiExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableGuiExplorer.Location = new System.Drawing.Point(378, 45);
            this.btnDisableGuiExplorer.Name = "btnDisableGuiExplorer";
            this.btnDisableGuiExplorer.Size = new System.Drawing.Size(87, 25);
            this.btnDisableGuiExplorer.TabIndex = 2;
            this.btnDisableGuiExplorer.Text = "Disable";
            this.btnDisableGuiExplorer.UseVisualStyleBackColor = true;
            this.btnDisableGuiExplorer.Click += new System.EventHandler(this.btnDisableGuiExplorer_Click);
            // 
            // groupGuiExplorer
            // 
            this.groupGuiExplorer.Controls.Add(this.btnDisableGuiExplorer);
            this.groupGuiExplorer.Controls.Add(this.btnEnableGuiExplorer);
            this.groupGuiExplorer.Controls.Add(this.labelStatusGuiExplorer);
            this.groupGuiExplorer.Location = new System.Drawing.Point(14, 277);
            this.groupGuiExplorer.Name = "groupGuiExplorer";
            this.groupGuiExplorer.Size = new System.Drawing.Size(472, 81);
            this.groupGuiExplorer.TabIndex = 4;
            this.groupGuiExplorer.TabStop = false;
            this.groupGuiExplorer.Text = "xUnit.net GUI Project File Windows Explorer Integration";
            // 
            // btnEnableGuiExplorer
            // 
            this.btnEnableGuiExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableGuiExplorer.Location = new System.Drawing.Point(378, 14);
            this.btnEnableGuiExplorer.Name = "btnEnableGuiExplorer";
            this.btnEnableGuiExplorer.Size = new System.Drawing.Size(87, 25);
            this.btnEnableGuiExplorer.TabIndex = 1;
            this.btnEnableGuiExplorer.Text = "Enable";
            this.btnEnableGuiExplorer.UseVisualStyleBackColor = true;
            this.btnEnableGuiExplorer.Click += new System.EventHandler(this.btnEnableGuiExplorer_Click);
            // 
            // labelStatusGuiExplorer
            // 
            this.labelStatusGuiExplorer.Location = new System.Drawing.Point(7, 22);
            this.labelStatusGuiExplorer.Name = "labelStatusGuiExplorer";
            this.labelStatusGuiExplorer.Size = new System.Drawing.Size(363, 48);
            this.labelStatusGuiExplorer.TabIndex = 0;
            this.labelStatusGuiExplorer.Text = "(Project File Integration Status)";
            this.labelStatusGuiExplorer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 408);
            this.Controls.Add(this.groupGuiExplorer);
            this.Controls.Add(this.groupMVC2);
            this.Controls.Add(this.groupMVC);
            this.Controls.Add(this.groupTdNet);
            this.Controls.Add(this.btnExit);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupTdNet.ResumeLayout(false);
            this.groupMVC.ResumeLayout(false);
            this.groupMVC2.ResumeLayout(false);
            this.groupGuiExplorer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupTdNet;
        private System.Windows.Forms.Button btnDisableTdNet;
        private System.Windows.Forms.Button btnEnableTdNet;
        private System.Windows.Forms.Label labelStatusTdNet;
        private System.Windows.Forms.GroupBox groupMVC;
        private System.Windows.Forms.Button btnDisableMVC;
        private System.Windows.Forms.Button btnEnableMVC;
        private System.Windows.Forms.Label labelStatusMVC;
        private System.Windows.Forms.GroupBox groupMVC2;
        private System.Windows.Forms.Button btnDisableMVC2;
        private System.Windows.Forms.Button btnEnableMVC2;
        private System.Windows.Forms.Label labelStatusMVC2;
        private System.Windows.Forms.Button btnDisableGuiExplorer;
        private System.Windows.Forms.GroupBox groupGuiExplorer;
        private System.Windows.Forms.Button btnEnableGuiExplorer;
        private System.Windows.Forms.Label labelStatusGuiExplorer;
    }
}