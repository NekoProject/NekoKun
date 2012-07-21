using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    public class RubyObjectViewContentListBox: System.Windows.Forms.Panel
    {
        RubyObject Object;

        public RubyObjectViewContentListBox(RubyObject Object)
        {
            this.Object = Object;
            this.AutoScroll = true;
            foreach (KeyValuePair<RubySymbol, object> item in Object.Variables)
            {
                VariableListItem vi = null;
                vi = new VariableListItem(item.Key, item.Value);
                m_Items.Add(vi);
                this.Controls.Add(vi);
            }
            OnResize(EventArgs.Empty);
            this.AutoScrollPosition = new Point(0, 0);
        }

        private ArrayList m_Items = new ArrayList();

        private int LastWidth;

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            int posY = this.AutoScrollPosition.Y + this.Padding.Vertical;

            if (LastWidth != this.ClientRectangle.Right)
            {
                for (int i = 0; i < m_Items.Count; i++)
                {
                    ((VariableListItem)m_Items[i]).Left = this.Padding.Left;
                    ((VariableListItem)m_Items[i]).Top = posY;
                    ((VariableListItem)m_Items[i]).Width = this.ClientRectangle.Right - this.Padding.Horizontal;
                    posY += ((VariableListItem)m_Items[i]).Height + this.Padding.Vertical;
                }
                LastWidth = this.ClientRectangle.Right;
            }

            this.AutoScrollMinSize = new Size(0, posY);
            this.HorizontalScroll.Visible = false;
        }

        public class VariableListItem : Panel
        {
            Label title;
            ObjectEditor.ObjectEditor editor;

            RubySymbol name;
            object Object;
            
            public VariableListItem(RubySymbol Name, object Object)
            {
                this.name = Name;
                this.Object = Object;

                this.title = new Label();
                this.title.Text = Name.ToString();
                this.title.AutoSize = true;
                this.title.Left = this.Padding.Left;
                this.title.Top = this.Padding.Top;
                this.Controls.Add(this.title);

                this.editor = new ObjectEditor.ObjectEditor();
                this.editor.Object = Object;
                this.editor.Left = this.Padding.Left;
                this.editor.Top = this.Padding.Top + this.title.Height;
                this.editor.Width = this.Width - this.Padding.Horizontal;
                this.Height = this.editor.DefaultHeight + this.editor.Top + this.Padding.Bottom;
                this.editor.Height = this.Height - this.editor.Top - this.Padding.Bottom;
                this.Controls.Add(this.editor);
            }

            protected override void OnResize(EventArgs eventargs)
            {
                this.editor.Height = this.Height - this.editor.Top - this.Padding.Bottom;
                this.editor.Width = this.ClientRectangle.Width;
            }
        }
    }
}
