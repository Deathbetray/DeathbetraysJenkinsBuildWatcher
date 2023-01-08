
namespace DeathbetraysJenkinsBuildWatcher
{
    partial class DeathbetraysJenkinsBuildWatcher
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
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnNotMe = new System.Windows.Forms.Button();
			this.lblBuildBreaker = new System.Windows.Forms.Label();
			this.btnViewBrokenBuilds = new System.Windows.Forms.Button();
			this.lblBuildStatusColoured = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cclbBuildGroups = new ColouredCheckedListBox();
			this.cmsBuildGroups = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cclbBuildList = new ColouredCheckedListBox();
			this.cclbBuildListWatched = new ColouredCheckedListBox();
			this.lblConnectionStatus = new System.Windows.Forms.Label();
			this.tbUrl = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.msMainMenu = new System.Windows.Forms.MenuStrip();
			this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.panel1.SuspendLayout();
			this.msMainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnNotMe);
			this.panel1.Controls.Add(this.lblBuildBreaker);
			this.panel1.Controls.Add(this.btnViewBrokenBuilds);
			this.panel1.Controls.Add(this.lblBuildStatusColoured);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.cclbBuildGroups);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.cclbBuildList);
			this.panel1.Controls.Add(this.cclbBuildListWatched);
			this.panel1.Controls.Add(this.lblConnectionStatus);
			this.panel1.Controls.Add(this.tbUrl);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.tbPassword);
			this.panel1.Controls.Add(this.tbUsername);
			this.panel1.Controls.Add(this.btnLogin);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.btnRefresh);
			this.panel1.Controls.Add(this.msMainMenu);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(570, 351);
			this.panel1.TabIndex = 0;
			// 
			// btnNotMe
			// 
			this.btnNotMe.Location = new System.Drawing.Point(460, 113);
			this.btnNotMe.Name = "btnNotMe";
			this.btnNotMe.Size = new System.Drawing.Size(75, 23);
			this.btnNotMe.TabIndex = 36;
			this.btnNotMe.Text = "It Wasn\'t Me";
			this.btnNotMe.UseVisualStyleBackColor = true;
			this.btnNotMe.Click += new System.EventHandler(this.btnNotMe_Click);
			// 
			// lblBuildBreaker
			// 
			this.lblBuildBreaker.AutoSize = true;
			this.lblBuildBreaker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBuildBreaker.ForeColor = System.Drawing.Color.Red;
			this.lblBuildBreaker.Location = new System.Drawing.Point(344, 118);
			this.lblBuildBreaker.Name = "lblBuildBreaker";
			this.lblBuildBreaker.Size = new System.Drawing.Size(110, 13);
			this.lblBuildBreaker.TabIndex = 35;
			this.lblBuildBreaker.Text = "BUILD BREAKER!";
			// 
			// btnViewBrokenBuilds
			// 
			this.btnViewBrokenBuilds.Location = new System.Drawing.Point(205, 113);
			this.btnViewBrokenBuilds.Name = "btnViewBrokenBuilds";
			this.btnViewBrokenBuilds.Size = new System.Drawing.Size(118, 23);
			this.btnViewBrokenBuilds.TabIndex = 34;
			this.btnViewBrokenBuilds.Text = "View Broken Builds";
			this.btnViewBrokenBuilds.UseVisualStyleBackColor = true;
			this.btnViewBrokenBuilds.Click += new System.EventHandler(this.btnViewBrokenBuilds_Click);
			// 
			// lblBuildStatusColoured
			// 
			this.lblBuildStatusColoured.AutoSize = true;
			this.lblBuildStatusColoured.Location = new System.Drawing.Point(125, 118);
			this.lblBuildStatusColoured.Name = "lblBuildStatusColoured";
			this.lblBuildStatusColoured.Size = new System.Drawing.Size(53, 13);
			this.lblBuildStatusColoured.TabIndex = 31;
			this.lblBuildStatusColoured.Text = "Unknown";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(21, 118);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(98, 13);
			this.label9.TabIndex = 30;
			this.label9.Text = "Build Group Status:";
			// 
			// cclbBuildGroups
			// 
			this.cclbBuildGroups.CheckedItemColor = System.Drawing.Color.Green;
			this.cclbBuildGroups.ContextMenuStrip = this.cmsBuildGroups;
			this.cclbBuildGroups.FormattingEnabled = true;
			this.cclbBuildGroups.Location = new System.Drawing.Point(24, 157);
			this.cclbBuildGroups.Name = "cclbBuildGroups";
			this.cclbBuildGroups.ShowCheckbox = true;
			this.cclbBuildGroups.Size = new System.Drawing.Size(120, 154);
			this.cclbBuildGroups.TabIndex = 29;
			this.cclbBuildGroups.UncheckedItemColor = System.Drawing.Color.Red;
			this.cclbBuildGroups.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cclbBuildGroups_ItemCheck);
			this.cclbBuildGroups.SelectedIndexChanged += new System.EventHandler(this.cclbBuildGroups_SelectedIndexChanged);
			this.cclbBuildGroups.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cclbBuildGroups_KeyDown);
			// 
			// cmsBuildGroups
			// 
			this.cmsBuildGroups.Name = "cmsBuildGroups";
			this.cmsBuildGroups.Size = new System.Drawing.Size(61, 4);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(21, 141);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 13);
			this.label7.TabIndex = 28;
			this.label7.Text = "Build Groups:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(174, 141);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 13);
			this.label6.TabIndex = 27;
			this.label6.Text = "Watched Builds:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(360, 141);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 26;
			this.label2.Text = "Builds:";
			// 
			// cclbBuildList
			// 
			this.cclbBuildList.CheckedItemColor = System.Drawing.Color.Green;
			this.cclbBuildList.FormattingEnabled = true;
			this.cclbBuildList.Location = new System.Drawing.Point(363, 157);
			this.cclbBuildList.Name = "cclbBuildList";
			this.cclbBuildList.ShowCheckbox = false;
			this.cclbBuildList.Size = new System.Drawing.Size(195, 154);
			this.cclbBuildList.TabIndex = 25;
			this.cclbBuildList.UncheckedItemColor = System.Drawing.Color.Red;
			this.cclbBuildList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cclbBuildList_MouseDoubleClick);
			// 
			// cclbBuildListWatched
			// 
			this.cclbBuildListWatched.CheckedItemColor = System.Drawing.Color.Green;
			this.cclbBuildListWatched.FormattingEnabled = true;
			this.cclbBuildListWatched.Location = new System.Drawing.Point(177, 157);
			this.cclbBuildListWatched.Name = "cclbBuildListWatched";
			this.cclbBuildListWatched.ShowCheckbox = false;
			this.cclbBuildListWatched.Size = new System.Drawing.Size(180, 154);
			this.cclbBuildListWatched.TabIndex = 24;
			this.cclbBuildListWatched.UncheckedItemColor = System.Drawing.Color.Red;
			this.cclbBuildListWatched.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cclbBuildList_MouseDoubleClick);
			// 
			// lblConnectionStatus
			// 
			this.lblConnectionStatus.AutoSize = true;
			this.lblConnectionStatus.Location = new System.Drawing.Point(332, 86);
			this.lblConnectionStatus.Name = "lblConnectionStatus";
			this.lblConnectionStatus.Size = new System.Drawing.Size(73, 13);
			this.lblConnectionStatus.TabIndex = 13;
			this.lblConnectionStatus.Text = "Disconnected";
			this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbUrl
			// 
			this.tbUrl.Location = new System.Drawing.Point(85, 79);
			this.tbUrl.Name = "tbUrl";
			this.tbUrl.Size = new System.Drawing.Size(229, 20);
			this.tbUrl.TabIndex = 12;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(21, 82);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(23, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Url:";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(85, 53);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(229, 20);
			this.tbPassword.TabIndex = 10;
			this.tbPassword.UseSystemPasswordChar = true;
			// 
			// tbUsername
			// 
			this.tbUsername.Location = new System.Drawing.Point(85, 27);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(229, 20);
			this.tbUsername.TabIndex = 9;
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(320, 38);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(99, 48);
			this.btnLogin.TabIndex = 8;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(21, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Password:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(21, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Username:";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(24, 317);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(120, 23);
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "Refresh Now";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// msMainMenu
			// 
			this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.msMainMenu.Location = new System.Drawing.Point(0, 0);
			this.msMainMenu.Name = "msMainMenu";
			this.msMainMenu.Size = new System.Drawing.Size(570, 24);
			this.msMainMenu.TabIndex = 20;
			this.msMainMenu.Text = "Main Menu";
			// 
			// preferencesToolStripMenuItem
			// 
			this.preferencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem1,
            this.loadToolStripMenuItem1,
            this.loadToolStripMenuItem2});
			this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
			this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
			this.preferencesToolStripMenuItem.Text = "Preferences";
			// 
			// saveToolStripMenuItem1
			// 
			this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
			this.saveToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.saveToolStripMenuItem1.Text = "Open";
			this.saveToolStripMenuItem1.Click += new System.EventHandler(this.openPreferencesToolStripMenuItem_Click);
			// 
			// loadToolStripMenuItem1
			// 
			this.loadToolStripMenuItem1.Name = "loadToolStripMenuItem1";
			this.loadToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
			this.loadToolStripMenuItem1.Text = "Save";
			this.loadToolStripMenuItem1.Click += new System.EventHandler(this.savePreferencesToolStripMenuItem_Click);
			// 
			// loadToolStripMenuItem2
			// 
			this.loadToolStripMenuItem2.Name = "loadToolStripMenuItem2";
			this.loadToolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
			this.loadToolStripMenuItem2.Text = "Load";
			this.loadToolStripMenuItem2.Click += new System.EventHandler(this.loadPreferencesToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// notifyIcon
			// 
			this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.notifyIcon.BalloonTipTitle = "Build Status Update";
			this.notifyIcon.ContextMenuStrip = this.cmsNotifyIcon;
			this.notifyIcon.Text = "notifyIcon";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
			// 
			// cmsNotifyIcon
			// 
			this.cmsNotifyIcon.Name = "cmsNotifyIcon";
			this.cmsNotifyIcon.Size = new System.Drawing.Size(61, 4);
			// 
			// DeathbetraysJenkinsBuildWatcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(570, 351);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.Name = "DeathbetraysJenkinsBuildWatcher";
			this.Text = "DB\'s JenkinsBuildWatcher";
			this.Resize += new System.EventHandler(this.DeathbetraysJenkinsBuildWatcher_Resize);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.msMainMenu.ResumeLayout(false);
			this.msMainMenu.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private ColouredCheckedListBox cclbBuildListWatched;
        private ColouredCheckedListBox cclbBuildList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private ColouredCheckedListBox cclbBuildGroups;
        private System.Windows.Forms.Label lblBuildStatusColoured;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ContextMenuStrip cmsBuildGroups;
		private System.Windows.Forms.Button btnNotMe;
		private System.Windows.Forms.Label lblBuildBreaker;
		private System.Windows.Forms.Button btnViewBrokenBuilds;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem2;
	}
}

