using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class EnumEditor : UI.LynnCombobox, IObjectEditor
    {
        protected string orig;
        protected object ori;
        protected Enum enu;

        public EnumEditor(Dictionary<string, object> Params)
        {
            if (DirtyChanged != null) DirtyChanged.ToString();

            this.enu = (ProjectManager.Components["Enums"] as EnumProvider).Enums[Params["Source"] as string];

            this.Items.AddRange(this.enu.GetKeys());

            this.DropDownHeight = this.ItemHeight * 20;
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.SelectedIndexChanged += new EventHandler(EnumEditor_SelectedIndexChanged);
        }

        void EnumEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Commit();
        }

        public void Commit()
        {
            if (this.RequestCommit != null && orig != (base.SelectedItem as string))
                this.RequestCommit(this, null);
        }

        protected override string GetString(int id)
        {
            return this.enu[this.Items[id] as string];
        }

        public new object SelectedItem
        {
            get
            {
                if (ori == null)
                {
                    int result;
                    if (Int32.TryParse(base.SelectedItem == null ? "" : base.SelectedItem as string, out result))
                    {
                        return result;
                    }
                    else
                    {
                        return base.SelectedItem as string;
                    }
                }
                if (ori is string)
                    return base.SelectedItem as string;
                else
                    return Int32.Parse(base.SelectedItem == null ? "" : base.SelectedItem as string);
            }
            set
            {
                this.orig = value.ToString();
                this.ori = value;
                base.SelectedItem = value.ToString();
            }
        }

        public event EventHandler RequestCommit;
        public event EventHandler DirtyChanged;
    }
}
