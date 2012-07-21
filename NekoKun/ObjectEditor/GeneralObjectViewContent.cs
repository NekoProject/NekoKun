using System;
using System.Collections.Generic;
using System.Text;

namespace orzTech.NekoKun.ObjectEditor
{
    public class GeneralObjectViewContent : AbstractObjectViewContent
    {
        System.Windows.Forms.PropertyGrid pg = new System.Windows.Forms.PropertyGrid();

        public GeneralObjectViewContent(object Object)
        {
            this.mObject = Object;
            pg.SelectedObject = Object;
            pg.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(pg_PropertyValueChanged);
        }

        private void pg_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            this.IsDirty = true;
        }

        public override System.Windows.Forms.Control Control
        {
            get { return pg; }
        }
    }
}
