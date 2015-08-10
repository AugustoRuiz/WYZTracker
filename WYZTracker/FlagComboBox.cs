using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace WYZTracker
{
    public class FlagComboBox : ComboBox
    {
        private const int HORIZONTAL_PADDING = 1;
        private const int VERTICAL_PADDING = 1;

        public FlagComboBox() :
            base()
        {
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (e.Index != -1)
            {
                string item = (string)this.Items[e.Index];
                string itemText = getCultureName(item);
                Size textSize = TextRenderer.MeasureText(itemText, this.Font);

                object resourceFlag = Properties.Resources.ResourceManager.GetObject(item);
                if (resourceFlag != null && resourceFlag is Image)
                {
                    Image flagImage = (Image)resourceFlag;
                    if (flagImage.Height > textSize.Height)
                    {
                        textSize.Height = flagImage.Height;
                    }
                    textSize.Width += flagImage.Width + 2 * HORIZONTAL_PADDING;
                }

                e.ItemHeight = textSize.Height + 2 * HORIZONTAL_PADDING;
                e.ItemWidth = textSize.Width + 2 * HORIZONTAL_PADDING;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                string item = (string)this.Items[e.Index];
                string itemText = getCultureName(item);
                object resourceFlag = Properties.Resources.ResourceManager.GetObject(item);
                int xCoord = HORIZONTAL_PADDING;

                if (resourceFlag != null && resourceFlag is Image)
                {
                    Image flagImage = (Image)resourceFlag;

                    Rectangle destRectangle = new Rectangle(HORIZONTAL_PADDING, 0, flagImage.Width, flagImage.Height);
                    destRectangle.Y = e.Bounds.Y + (int)Math.Round((e.Bounds.Height - flagImage.Height) / 2.0);

                    e.Graphics.DrawImage(flagImage, destRectangle);

                    xCoord += HORIZONTAL_PADDING + flagImage.Width;
                }
                Rectangle rect = e.Bounds;

                rect.X = xCoord;
                rect.Width -= xCoord;

                TextRenderer.DrawText(e.Graphics, itemText,
                    e.Font, rect,
                    e.ForeColor, e.BackColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
        }

        private string getCultureName(string item)
        {
            string nativeName = CultureInfo.GetCultureInfo(item).NativeName;
            if (!string.IsNullOrEmpty(nativeName))
            {
                nativeName = string.Format("{0}{1}", Char.ToUpper(nativeName[0]), nativeName.Substring(1));
            }
            return nativeName;
        }
    }
}
