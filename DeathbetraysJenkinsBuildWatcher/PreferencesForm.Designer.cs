
namespace DeathbetraysJenkinsBuildWatcher
{
    partial class PreferencesForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.cbTimerUnits = new System.Windows.Forms.ComboBox();
			this.tbTimer = new System.Windows.Forms.TextBox();
			this.cbLogging = new System.Windows.Forms.CheckBox();
			this.cbLoggingVerbose = new System.Windows.Forms.CheckBox();
			this.btnDefaults = new System.Windows.Forms.Button();
			this.cbBuildBreakerNotifications = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Preferences";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(121, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Time Between Updates:";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(12, 177);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(91, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(185, 177);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(95, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Exit";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// cbTimerUnits
			// 
			this.cbTimerUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTimerUnits.FormattingEnabled = true;
			this.cbTimerUnits.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours"});
			this.cbTimerUnits.Location = new System.Drawing.Point(208, 46);
			this.cbTimerUnits.Name = "cbTimerUnits";
			this.cbTimerUnits.Size = new System.Drawing.Size(72, 21);
			this.cbTimerUnits.TabIndex = 5;
			this.cbTimerUnits.SelectedIndexChanged += new System.EventHandler(this.cbTimerUnits_SelectedIndexChanged);
			// 
			// tbTimer
			// 
			this.tbTimer.Location = new System.Drawing.Point(136, 46);
			this.tbTimer.MaxLength = 100;
			this.tbTimer.Name = "tbTimer";
			this.tbTimer.Size = new System.Drawing.Size(66, 20);
			this.tbTimer.TabIndex = 6;
			this.tbTimer.Text = "0";
			this.tbTimer.Validating += new System.ComponentModel.CancelEventHandler(this.tbTimer_Validating);
			// 
			// cbLogging
			// 
			this.cbLogging.AutoSize = true;
			this.cbLogging.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbLogging.Location = new System.Drawing.Point(12, 118);
			this.cbLogging.Name = "cbLogging";
			this.cbLogging.Size = new System.Drawing.Size(154, 17);
			this.cbLogging.TabIndex = 9;
			this.cbLogging.Text = "Enable Logging?                ";
			this.cbLogging.UseVisualStyleBackColor = true;
			this.cbLogging.CheckedChanged += new System.EventHandler(this.cbLogging_CheckedChanged);
			// 
			// cbLoggingVerbose
			// 
			this.cbLoggingVerbose.AutoSize = true;
			this.cbLoggingVerbose.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbLoggingVerbose.Location = new System.Drawing.Point(12, 141);
			this.cbLoggingVerbose.Name = "cbLoggingVerbose";
			this.cbLoggingVerbose.Size = new System.Drawing.Size(154, 17);
			this.cbLoggingVerbose.TabIndex = 10;
			this.cbLoggingVerbose.Text = "Enable Verbose Logging?  ";
			this.cbLoggingVerbose.UseVisualStyleBackColor = true;
			this.cbLoggingVerbose.CheckedChanged += new System.EventHandler(this.cbLoggingVerbose_CheckedChanged);
			// 
			// btnDefaults
			// 
			this.btnDefaults.Location = new System.Drawing.Point(172, 12);
			this.btnDefaults.Name = "btnDefaults";
			this.btnDefaults.Size = new System.Drawing.Size(108, 23);
			this.btnDefaults.TabIndex = 11;
			this.btnDefaults.Text = "Reset to Defaults";
			this.btnDefaults.UseVisualStyleBackColor = true;
			this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
			// 
			// cbBuildBreakerNotifications
			// 
			this.cbBuildBreakerNotifications.AutoSize = true;
			this.cbBuildBreakerNotifications.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbBuildBreakerNotifications.Location = new System.Drawing.Point(12, 83);
			this.cbBuildBreakerNotifications.Name = "cbBuildBreakerNotifications";
			this.cbBuildBreakerNotifications.Size = new System.Drawing.Size(156, 17);
			this.cbBuildBreakerNotifications.TabIndex = 12;
			this.cbBuildBreakerNotifications.Text = "Build Breaker Notifications?";
			this.cbBuildBreakerNotifications.UseVisualStyleBackColor = true;
			this.cbBuildBreakerNotifications.CheckedChanged += new System.EventHandler(this.cbBuildBreakerNotifications_CheckedChanged);
			// 
			// PreferencesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 212);
			this.Controls.Add(this.cbBuildBreakerNotifications);
			this.Controls.Add(this.btnDefaults);
			this.Controls.Add(this.cbLoggingVerbose);
			this.Controls.Add(this.cbLogging);
			this.Controls.Add(this.tbTimer);
			this.Controls.Add(this.cbTimerUnits);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.Name = "PreferencesForm";
			this.ShowInTaskbar = false;
			this.Text = "PreferencesForm";
			this.Shown += new System.EventHandler(this.PreferencesForm_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cbTimerUnits;
        private System.Windows.Forms.TextBox tbTimer;
        private System.Windows.Forms.CheckBox cbLogging;
        private System.Windows.Forms.CheckBox cbLoggingVerbose;
        private System.Windows.Forms.Button btnDefaults;
		private System.Windows.Forms.CheckBox cbBuildBreakerNotifications;
	}
}