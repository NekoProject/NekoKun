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

        protected object supp;

        public object SelectedItem
        {
            get
            {
                if (supp is FuzzyData.FuzzyString)
                    return new FuzzyData.FuzzyString(this.Text).Encode((supp as FuzzyData.FuzzyString).Encoding);
                else
                    return this.Text;
            }
            set
            {
                supply = true;
                supp = value;
                if (supp is FuzzyData.FuzzyString)
                    this.Text = (value as FuzzyData.FuzzyString).Text;
                else
                    this.Text = value as string;
                supply = false;
            }
        }
        public event EventHandler RequestCommit;

        public event EventHandler DirtyChanged;


    }
}
