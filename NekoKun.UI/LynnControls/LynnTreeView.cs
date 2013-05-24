using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NekoKun.UI
{
    public class LynnTreeView : TreeView
    {
        private Color backColor = Color.White;
        private Color foreColor = Color.Black;
        private Color selectedColor1 = Color.FromArgb(0, 100, 200);
        private Color selectedColor2 = Color.FromArgb(0, 158, 247);
        private Color selectedForeColor = Color.White;
        protected System.Drawing.Drawing2D.LinearGradientBrush backBrush;
        protected StringFormat stringFormat, stringFormat2;
        protected System.Windows.Forms.VisualStyles.VisualStyleRenderer renderOpened;
        protected System.Windows.Forms.VisualStyles.VisualStyleRenderer renderClosed;

        public LynnTreeView()
        {
            this.FullRowSelect = true;
            this.ShowRootLines = false;
            this.LabelEdit = false;
            this.ShowLines = false;

            if (UIManager.Enabled)
            {
                stringFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                stringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                stringFormat2 = new StringFormat(stringFormat);
                stringFormat2.Alignment = StringAlignment.Center;

                this.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            
            Brush back = new SolidBrush(backColor);
            Brush fore;
            e.Graphics.FillRectangle(back, e.Bounds);
            if (this.SelectedNode == e.Node)
            {
                if (backBrush == null || backBrush.Rectangle.Height != e.Bounds.Height)
                    backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(Point.Empty, new Point(0, e.Bounds.Height), this.selectedColor1, this.selectedColor2);
                backBrush.ResetTransform();
                backBrush.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(0, 1, 1, 0, e.Bounds.Location.X, e.Bounds.Location.Y - e.Bounds.Height / 2));
                e.Graphics.FillRectangle(backBrush, e.Bounds);
                fore = new SolidBrush(selectedForeColor);
            }
            else
                fore = new SolidBrush(foreColor);

            if ((int)(e.State & TreeNodeStates.Focused) != 0)
            {
                Rectangle bound2 = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                e.Graphics.DrawRectangle(new Pen(this.selectedColor1), bound2);
            }

            Rectangle textBound = e.Bounds;
            int indent = this.Indent * e.Node.Level;
            if (this.ShowRootLines) indent += this.Indent;
            textBound.X += indent; textBound.Width -= indent;

            if (this.ShowPlusMinus && e.Node.Nodes.Count > 0)
            {
                Rectangle glyphBounds = new Rectangle(textBound.X - this.Indent, textBound.Y, this.Indent, textBound.Height);
                if (System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported)
                {
                    if (this.renderOpened == null)
                        this.renderOpened = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView.Glyph.Opened);
                    if (this.renderClosed == null)
                        this.renderClosed = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView.Glyph.Closed);

                    (e.Node.IsExpanded ? this.renderOpened : this.renderClosed).DrawBackground(e.Graphics, glyphBounds);
                }
                else
                {
                    DrawText(-1, e.Node.IsExpanded ? "-" : "+", this.Font, fore, glyphBounds, this.stringFormat2, e.Graphics, this.SelectedNode == e.Node);
                }
            }

            if (this.ImageList != null)
            {
                e.Graphics.DrawImage(GetImage(e.Node), textBound.X + 2, textBound.Y + (textBound.Height - this.ImageList.ImageSize.Height) / 2);
                textBound.X += this.ImageList.ImageSize.Width + 2;
                textBound.Width -= this.ImageList.ImageSize.Width + 2;
            }
            DrawText(e.Node.Index, e.Node.Text, this.Font, fore, textBound, this.stringFormat, e.Graphics, this.SelectedNode == e.Node);
        }

        protected virtual void DrawText(int id, string str, Font font, Brush fc, Rectangle bounds, StringFormat sf, Graphics g, bool selected)
        {
            g.DrawString(str + "　", font, fc, bounds, sf);
        }

        protected Image GetImage(TreeNode item)
        {
            if (item.ImageIndex >= 0)
                return this.ImageList.Images[item.ImageIndex];
            else if (this.ImageList.Images.ContainsKey(item.ImageKey))
                return this.ImageList.Images[item.ImageKey];
            else
                return this.ImageList.Images[0];
        }

    }
}
