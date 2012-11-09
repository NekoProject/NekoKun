using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class EnumEditor : AbstractObjectEditor
    {
        protected Enum enu;
        protected System.Windows.Forms.ComboBox control;

        public EnumEditor(Dictionary<string, object> Params) : base(Params)
        {
            this.enu = (ProjectManager.Components["Enums"] as EnumProvider).Enums[Params["Source"] as string];
            control = new EnumCombobox(enu);
            control.Items.AddRange(this.enu.GetKeys());
            control.DropDownHeight = control.ItemHeight * 20;
            control.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            control.SelectedIndexChanged += new EventHandler(EnumEditor_SelectedIndexChanged);
        }

        void EnumEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MakeDirty();
        }

        private class EnumCombobox : UI.LynnCombobox
        {
            private Enum enu;

            public EnumCombobox(Enum enu)
            {
                this.enu = enu;
            }

            protected override string GetString(int id)
            {
                return this.enu[this.Items[id] as string];
            }
        }


        public override void Commit()
        {
            if (selectedItem is string)
                selectedItem = control.SelectedItem as string;
            else
                selectedItem = Int32.Parse(control.SelectedItem == null ? "" : control.SelectedItem as string);
        }

        protected override void InitControl()
        {
            control.SelectedIndexChanged -= new EventHandler(EnumEditor_SelectedIndexChanged);
            try
            {
                control.SelectedItem = selectedItem.ToString();
            }
            catch
            {
                control.SelectedIndex = 0;
            }
            control.SelectedIndexChanged += new EventHandler(EnumEditor_SelectedIndexChanged);
        }

        public override System.Windows.Forms.Control Control
        {
            get { return this.control; }
        }
    }
}
