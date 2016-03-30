using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public class FocusablePanel : ScrollableControl
    {
        public FocusablePanel() : base()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Tab) return false;
            if (keyData == (Keys.Tab | Keys.Shift)) return false; 
            return true;
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            this.Invalidate();
            base.OnLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.Focused)
            {
                var rc = this.ClientRectangle;
                rc.Inflate(-1, -1);
                ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
            }
        }
    }
}
