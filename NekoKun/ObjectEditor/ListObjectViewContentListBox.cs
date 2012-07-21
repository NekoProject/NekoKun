using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace orzTech.NekoKun.ObjectEditor
{
    public class ListObjectViewContentListBox : ListBox
    {
        object Object;

        public ListObjectViewContentListBox(object Object)
        {
            this.Object = Object;
            this.IntegralHeight = false;

            foreach (object item in (Object as IEnumerable))
            {
                this.Items.Add(item);
            }
        }

        public object SelectedObject
        {
            get {
                if (this.SelectedItem is KeyValuePair<object, object>)
                {
                    KeyValuePair<object, object> pair = (KeyValuePair<object, object>)this.SelectedItem;
                    return pair.Value;
                }
                return this.SelectedItem;
            }
        }
    }
}
