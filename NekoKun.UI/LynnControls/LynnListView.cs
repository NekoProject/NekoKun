using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NekoKun.UI
{
    public class LynnListView : ListView
    {
        private Color backColor = Color.White;
        private Color backColorAlt = Color.FromArgb(228, 236, 242);
        private Color foreColor = Color.Black;
        private Color selectedColor1 = Color.FromArgb(0, 100, 200);
        private Color selectedColor2 = Color.FromArgb(0, 158, 247);
        private Color selectedForeColor = Color.White;
        protected System.Drawing.Drawing2D.LinearGradientBrush backBrush;
        protected StringFormat stringFormat, stringFormat2;
        public LynnListView()
        {
            this.FullRowSelect = true;

            if (UIManager.Enabled)
            {
                this.OwnerDraw = true;
                stringFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                stringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                stringFormat2 = new StringFormat(stringFormat);
                stringFormat2.Alignment = StringAlignment.Center;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            try
            {
                Brush backalt = new SolidBrush(backColorAlt);
                Brush back = new SolidBrush(backColor);
                if (e.ItemIndex >= 0 && this.Items.Count > 0)
                {
                    Rectangle boundsback;
                    if (this.View == View.Details)
                        boundsback = new Rectangle(0, e.Bounds.Top, this.ClientSize.Width, e.Bounds.Height);
                    else
                        boundsback = e.Bounds;

                    e.Graphics.FillRectangle(
                        this.View == View.Details ?
                            e.ItemIndex % 2 == 0 ? back : backalt :
                            back,
                        boundsback
                    );
                    if (this.SelectedItems.Contains(e.Item))
                    {
                        if (backBrush == null || backBrush.Rectangle.Height != e.Bounds.Height)
                            backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(Point.Empty, new Point(0, e.Bounds.Height), this.selectedColor1, this.selectedColor2);
                        backBrush.ResetTransform();
                        backBrush.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(0, 1, 1, 0, e.Bounds.Location.X, e.Bounds.Location.Y - e.Bounds.Height / 2));
                        e.Graphics.FillRectangle(backBrush, e.Bounds);
                    }

                    if ((int)(e.State & ListViewItemStates.Focused) != 0)
                    {
                        Rectangle bound2 = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                        e.Graphics.DrawRectangle(new Pen(this.selectedColor1), bound2);
                    }
                }
                if (this.View == View.Details && e.ItemIndex == this.Items.Count - 1)
                {
                    int count = Math.Max(this.ClientSize.Height / e.Bounds.Height, 0) + 1;
                    for (int i = this.Items.Count; i < this.Items.Count + count; i++)
                    {
                        e.Graphics.FillRectangle(i % 2 == 0 ? back : backalt, new Rectangle(0, e.Bounds.Top + (i - this.Items.Count + 1) * e.Bounds.Height, this.ClientSize.Width, e.Bounds.Height));
                    }
                }
                Brush fore;
                if (this.SelectedItems.Contains(e.Item))
                    fore = new SolidBrush(selectedForeColor);
                else
                    fore = new SolidBrush(foreColor);

                Rectangle bound = e.Bounds;
                switch (this.View)
                {
                    case View.Details:
                        if (this.Columns.Count != 0)
                            bound.Width = this.Columns[0].Width;
                        e.Graphics.SetClip(bound);
                        if (this.SmallImageList != null)
                        {
                            e.Graphics.DrawImage(GetImage(e.Item, false), bound.X + 4, bound.Y + (bound.Height - this.SmallImageList.ImageSize.Height) / 2);
                            bound.X += this.SmallImageList.ImageSize.Width + 4;
                            bound.Width -= this.SmallImageList.ImageSize.Width + 4;
                        }
                        DrawText(e.ItemIndex, e.Item.Text, this.Font, fore, bound, this.stringFormat, e.Graphics, this.SelectedItems.Contains(e.Item));
                        for (int i = 1; i < e.Item.SubItems.Count; i++)
                        {
                            var item = e.Item.SubItems[i];
                            e.Graphics.SetClip(item.Bounds);
                            DrawText(e.ItemIndex, item.Text, item.Font, fore, item.Bounds, this.stringFormat, e.Graphics, this.SelectedItems.Contains(e.Item));
                        }
                        break;
                    case View.LargeIcon:
                        e.Graphics.SetClip(bound);
                        if (this.LargeImageList != null)
                        {
                            e.Graphics.DrawImage(GetImage(e.Item, true), bound.X + (bound.Width - this.LargeImageList.ImageSize.Width) / 2, bound.Y + 4);
                            bound.Y += this.LargeImageList.ImageSize.Height + 4;
                            bound.Height -= this.LargeImageList.ImageSize.Height + 4;
                        }
                        DrawText(e.ItemIndex, e.Item.Text, this.Font, fore, bound, this.stringFormat2, e.Graphics, this.SelectedItems.Contains(e.Item));
                        break;
                    case View.List:
                    case View.SmallIcon:
                        e.Graphics.SetClip(bound);
                        if (this.SmallImageList != null)
                        {
                            e.Graphics.DrawImage(GetImage(e.Item, false), bound.X + 4, bound.Y + (bound.Height - this.SmallImageList.ImageSize.Height) / 2);
                            bound.X += this.SmallImageList.ImageSize.Width + 4;
                            bound.Width -= this.SmallImageList.ImageSize.Width + 4;
                        }
                        DrawText(e.ItemIndex, e.Item.Text, this.Font, fore, bound, this.stringFormat, e.Graphics, this.SelectedItems.Contains(e.Item));
                        break;
                    case View.Tile:
                        e.Graphics.SetClip(bound);
                        if (this.LargeImageList != null)
                        {
                            e.Graphics.DrawImage(GetImage(e.Item, true), bound.X + 4, bound.Y + (bound.Height - this.LargeImageList.ImageSize.Height) / 2);
                            bound.X += this.LargeImageList.ImageSize.Width + 4;
                            bound.Width -= this.LargeImageList.ImageSize.Width + 4;
                        }
                        int lines = Math.Min(Math.Max(this.Columns.Count, 1), 3);
                        Rectangle smallbound = new Rectangle(bound.Left, bound.Top, bound.Width, bound.Height / lines);
                        for (int i = 0; i < lines; i++)
                        {
                            DrawText(e.ItemIndex, e.Item.SubItems[i].Text, this.Font, fore, smallbound, this.stringFormat, e.Graphics, this.SelectedItems.Contains(e.Item));
                            smallbound.Y += smallbound.Height;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }

        protected Image GetImage(ListViewItem item, bool large)
        {
            ImageList list = large ? this.LargeImageList : this.SmallImageList;
            if (item.ImageIndex >= 0)
                return list.Images[item.ImageIndex];
            else if (list.Images.ContainsKey(item.ImageKey))
                return list.Images[item.ImageKey];
            else
                return list.Images[0];
        }

        protected virtual void DrawText(int id, string str, Font font, Brush fc, Rectangle bounds, StringFormat sf, Graphics g, bool selected)
        {
            g.DrawString(str + "　", font, fc, bounds, sf);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
        }
    }
}
