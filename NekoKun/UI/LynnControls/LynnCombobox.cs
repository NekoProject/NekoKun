using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    public class LynnCombobox : ComboBox
    {
        private Color backColor = Color.White;
        private Color backColorAlt = Color.FromArgb(228, 236, 242);
        private Color foreColor = Color.Black;
        private Color selectedColor1 = Color.FromArgb(0, 100, 200);
        private Color selectedColor2 = Color.FromArgb(0, 158, 247);
        private Color selectedForeColor = Color.White;
        protected StringFormat stringFormat;
        internal NativeWindow native, native2;

        public LynnCombobox()
        {
            native = new NativeBorder(this, 0xf /* WM_PAINT */, true, false);
            native2 = new NativeBorder(this, 0x85 /* WM_NCPAINT */, true, false);

            base.DrawMode = DrawMode.OwnerDrawFixed;
            this.IntegralHeight = false;

            stringFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            stringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            OnFontChanged(EventArgs.Empty);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                Brush backalt = new SolidBrush(backColorAlt);
                Brush back = new SolidBrush(backColor);
                if (e.Index >= 0 && this.Items.Count > 0)
                {
                    Brush fore;

                    if ((e.State & DrawItemState.Selected) != DrawItemState.None)
                    {
                        fore = new SolidBrush(selectedForeColor);
                        e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, this.selectedColor1, this.selectedColor2, 90), e.Bounds);
                    }
                    else
                    {
                        fore = new SolidBrush(foreColor);
                        if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.None)
                            e.Graphics.FillRectangle(e.Index % 2 == 0 ? back : backalt, e.Bounds);
                        else
                            e.Graphics.FillRectangle(back, e.Bounds);
                    }

                    if ((e.State & DrawItemState.Focus) != DrawItemState.None)
                    {
                        Rectangle bound = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                        e.Graphics.DrawRectangle(new Pen(this.selectedColor1), bound);
                    }

                    DrawText(e.Index, GetString(e.Index), this.Font, fore, e.Bounds, this.stringFormat, e.Graphics, (e.State & DrawItemState.Selected) != DrawItemState.None);
                }
                if (e.Index == this.Items.Count - 1)
                {
                    int count = Math.Max(this.ClientSize.Height / this.ItemHeight - this.Items.Count, 0) + 1;
                    for (int i = this.Items.Count; i < this.Items.Count + count; i++)
                    {
                        e.Graphics.FillRectangle(i % 2 == 0 ? back : backalt, new Rectangle(e.Bounds.Left, e.Bounds.Top + (i - this.Items.Count + 1) * this.ItemHeight, e.Bounds.Width, this.ItemHeight));
                    }
                }
            }
            catch { }
        }

        protected virtual string GetString(int id)
        {
            return this.Items[id].ToString();
        }

        protected virtual void DrawText(int id, string str, Font font, Brush fc, Rectangle bounds, StringFormat sf, Graphics g, bool selected)
        {
            g.DrawString(str + "　", font, fc, bounds, sf);
        }
    }
}
