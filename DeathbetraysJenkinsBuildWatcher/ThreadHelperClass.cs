using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeathbetraysJenkinsBuildWatcher
{
    public static class ThreadHelperClass
    {
        private delegate void SetTextCallback(Form _form, Control _control, string _text);
        public static void SetText(Form _form, Control _control, string _text)
        {
            if (_control.InvokeRequired)
            {
                SetTextCallback cb = new SetTextCallback(SetText);
                _form.Invoke(cb, new object[] { _form, _control, _text });
            }
            else
            {
                _control.Text = _text;
            }
        }


        private delegate void SetTextAndColorCallback(Form _form, Control _control, string _text, Color _color);
        public static void SetTextAndColor(Form _form, Control _control, string _text, Color _color)
        {
            if (_control.InvokeRequired)
            {
                SetTextAndColorCallback cb = new SetTextAndColorCallback(SetTextAndColor);
                _form.Invoke(cb, new object[] { _form, _control, _text, _color });
            }
            else
            {
                _control.Text = _text;
                _control.ForeColor = _color;
            }
        }

        public delegate void RecallCallback();
        public static bool Recall(Form _form, Control _control, RecallCallback _callback)
        {
            if (_control.InvokeRequired)
            {
                _form.Invoke(_callback);
                return true;
            }

            return false;
        }
    }
}
