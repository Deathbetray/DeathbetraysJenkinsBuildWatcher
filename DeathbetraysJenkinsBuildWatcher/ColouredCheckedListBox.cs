using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Can't put in the main form's namespace otherwise the form designer complains.
//namespace DeathbetraysJenkinsBuildWatcher
//{
public class ColouredCheckedListBox : CheckedListBox
{
    public delegate Color DetermineItemColour(ColouredCheckedListBox _object, int _itemIdx);
    public DetermineItemColour DetermineItemColourHandler;

    public bool ShowCheckbox
    {
        get { return m_showCheckbox; }
        set { m_showCheckbox = value; }
    }
    private bool m_showCheckbox = true;

    public Color CheckedItemColor
    {
        get { return m_checkedItemColour; }
        set
        {
            m_checkedItemColour = value;
            Invalidate();
        }
    }
    private Color m_checkedItemColour = Color.Green;

    public Color UncheckedItemColor
    {
        get { return m_uncheckedItemColour; }
        set
        {
            m_uncheckedItemColour = value;
            Invalidate();
        }
    }
    private Color m_uncheckedItemColour = Color.Red;

    public ColouredCheckedListBox()
    {
        DoubleBuffered = true;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        bool isChecked = false;
        try
        {
            isChecked = GetItemChecked(e.Index); //For some reason e.State doesn't work so we have to do this instead.
        }
        catch (Exception /*ex*/)
        {
            // Invalid index (0 is an invalid index in the designer, apparently).
            return;
        }

        Size checkSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
        int dx = (e.Bounds.Height - checkSize.Width) / 2;
        e.DrawBackground();

        if (ShowCheckbox)
        {
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(dx, e.Bounds.Top + dx), GetCheckBoxState(isChecked));
        }

        using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center })
        {
            using (Brush brush = new SolidBrush(GetColour(e.Index)))
            {
                int left = ShowCheckbox ? e.Bounds.Height : e.Bounds.Left;
                e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, new Rectangle(left, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height), sf);
            }
        }
    }

    private System.Windows.Forms.VisualStyles.CheckBoxState GetCheckBoxState(bool isChecked)
    {
        if (Enabled)
        {
            if (isChecked)
            {
                return System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
            }
            else
            {
                return System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
            }
        }
        else
        {
            if (isChecked)
            {
                return System.Windows.Forms.VisualStyles.CheckBoxState.CheckedDisabled;
            }
            else
            {
                return System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled;
            }
        }
    }

    private Color GetColour(int _index)
    {
        bool isChecked = GetItemChecked(_index);
        if (!Enabled)
        {
            return SystemColors.GrayText;
        }
        else
        {
            if (DetermineItemColourHandler != null)
            {
                return DetermineItemColourHandler(this, _index);
            }
            else
            {
                if (isChecked)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.Red;
                }
            }
        }

        //if (isChecked)
        //{
        //    return CheckedItemColor;
        //}
        //else
        //{
        //    if (Enabled)
        //    {
        //        return UncheckedItemColor;
        //    }
        //    else
        //    {
        //        return SystemColors.GrayText;
        //    }
        //}
    }
}
//}
