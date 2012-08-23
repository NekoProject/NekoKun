using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class ComplexTextEditor: UI.RubyScintilla, IObjectEditor
    {
        protected bool supply;

        public ComplexTextEditor(Dictionary<string, object> Params)
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ContextMenuStrip = new EditContextMenuStrip(this);
            this.TextChanged += new EventHandler(ComplexTextEditor_TextChanged);
            if (DirtyChanged != null) DirtyChanged.ToString();
        }

        void ComplexTextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!supply) this.Commit();
        }

        public void Commit()
        {
            if (this.RequestCommit != null)
                this.RequestCommit(this, null);
        }

        public object SelectedItem
        {
            get
            {
                return this.Text;
            }
            set
            {
                supply = true;
                this.Text = value as string;
                supply = false;
            }
        }

        public event EventHandler RequestCommit;

        public event EventHandler DirtyChanged;


    }
}
