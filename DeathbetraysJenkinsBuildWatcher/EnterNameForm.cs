using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeathbetraysJenkinsBuildWatcher
{
    public partial class EnterNameForm : Form
    {
        public bool Cancelled
        {
            get { return m_cancelled; }
            private set { m_cancelled = value; }
        }
        private bool m_cancelled = true;

        public string NewName
        {
            get { return m_name; }
            private set { m_name = value; }
        }
        private string m_name = null;

        public List<string> ExistingNames
        {
            get { return m_existingNames; }
            private set { m_existingNames = value; }
        }
        private List<string> m_existingNames = new List<string>();


        public EnterNameForm()
        {
            InitializeComponent();
        }

        public void Init(List<string> _existingNames)
        {
            tbName.Text = string.Empty;

            ExistingNames = _existingNames;
            Cancelled = true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            NewName = tbName.Text.Trim();
            if (string.IsNullOrEmpty(m_name))
            {
                MessageBox.Show("Please enter a valid name.", "Invalid Name", MessageBoxButtons.OK);
                return;
            }

            bool inUse = false;
            foreach (string i in ExistingNames)
            {
                if (NewName == i)
                {
                    inUse = true;
                    break;
                }
            }

            if (inUse)
            {
                MessageBox.Show("Group name is already in use.", "Invalid Name", MessageBoxButtons.OK);
                return;
            }

            Cancelled = false;
            Close();
        }

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!e.Handled && e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnConfirm_Click(btnConfirm, null);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EnterNameForm_Shown(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
