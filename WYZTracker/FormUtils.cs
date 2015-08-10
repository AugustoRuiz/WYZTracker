using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public class FormUtils
    {
        public static Control FindFocused(Control current)
        {
            Control result = null;
            if (current.Focused)
            {
                result = current;
            }
            else
            {
                if (current.HasChildren)
                {
                    int childIndex = 0;
                    do
                    {
                        result = FindFocused(current.Controls[childIndex++]);
                    } while (result == null && childIndex < current.Controls.Count);
                }
            }
            return result;
        }
    }
}
