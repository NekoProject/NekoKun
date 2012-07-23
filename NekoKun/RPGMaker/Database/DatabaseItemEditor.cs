using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public abstract class DatabaseItemEditor : System.Windows.Forms.Panel
    {
        protected Dictionary<DatabaseField, IObjectEditor> editors;
        private Dictionary<IObjectEditor, DatabaseField> editorre;
        protected System.Xml.XmlNode param;
        protected DatabaseFile file;
        protected DatabaseItem selectedItem;

        public DatabaseItemEditor(System.Xml.XmlNode param, DatabaseFile file)
        {
            this.file = file;
            this.param = param;
            this.editors = new Dictionary<DatabaseField, IObjectEditor>();
            this.editorre = new Dictionary<IObjectEditor, DatabaseField>();
            foreach (var item in this.file.Fields)
            {
                if (item.Value.Name == "ID") continue;

                IObjectEditor editor;
                editor = Program.CreateInstanceFromTypeName(item.Value.EditorTypeName, item.Value.EditorParams) as IObjectEditor;
                editor.RequestCommit += new EventHandler(editor_RequestCommit);
                editor.DirtyChanged += new EventHandler(editor_DirtyChanged);
                editors.Add(item.Value, editor);
                editorre.Add(editor, item.Value);
            }
        }

        void editor_DirtyChanged(object sender, EventArgs e)
        {
            this.file.MakeDirty();
        }

        void editor_RequestCommit(object sender, EventArgs e)
        {
            IObjectEditor editor = sender as IObjectEditor;
            var l1 = this.selectedItem[this.editorre[sender as IObjectEditor]];
            var l2 = editor.SelectedItem;

            if ((l1 as IComparable == null || l1.GetType() == l2.GetType()) ? !l1.Equals(l2) : (l1 as IComparable).CompareTo(l2) != 0)
            {
                this.file.MakeDirty();
                this.selectedItem[this.editorre[sender as IObjectEditor]] = editor.SelectedItem;
            }
        }

        public DatabaseItem SelectedItem
        {
            get{return this.selectedItem;}
            set
            {
                if (value == null)
                    return;
                if (this.selectedItem != null)
                {
                    this.Commit();
                }
                
                this.selectedItem = value;
                foreach (var item in editors)
                {
                    var editor = (item.Value as IObjectEditor);
                    editor.RequestCommit -= new EventHandler(editor_RequestCommit);
                    editor.SelectedItem = this.selectedItem[editorre[item.Value]];
                    editor.RequestCommit += new EventHandler(editor_RequestCommit);
                }
            }
        }

        public void Commit()
        {
            foreach (var item in editors)
            {
                (item.Value as IObjectEditor).Commit();
            }
        }
    }
}
