using System;
using System.Windows.Forms;

namespace DeathbetraysJenkinsBuildWatcher
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            lblVersion.Text = "v" + Globals.Version;
        }

        private void AboutForm_Shown(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
