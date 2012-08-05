using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class StructEditor : System.Windows.Forms.TabControl, IObjectEditor
    {
        protected List<View> views;
        protected Struct selectedItem;
        protected Dictionary<string, StructField> fields;
        public StructEditor(Dictionary<string, object> Params, StructField[] fields)
        {
            this.fields = new Dictionary<string, StructField>();
            foreach (var field in fields)
            {
                this.fields.Add(field.ID, field);
            }

            this.views = new List<View>();
            //View rawView = new View();
            //rawView.Name = "原始视图";
            //rawView.ID = "Raw";
            //rawView.Layout = new RawLayout
            foreach (System.Xml.XmlNode item in Params["Views"] as System.Xml.XmlNodeList)
            {
                if (item.Name == "View")
                {
                    View view = new View();
                    view.Name = item.Attributes["Name"].Value;
                    view.ID = item.Attributes["ID"].Value;
                    view.LayoutInfo = item.FirstChild;
                    this.views.Add(view);
                }
            }

            this.TabPages.Clear();
            foreach (View view in views)
            {
                var page = new System.Windows.Forms.TabPage(view.Name);
                page.BackColor = System.Drawing.Color.Transparent;
                this.TabPages.Add(page);
            }
            this.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(StructEditor_Selecting);
            this.Selected += new System.Windows.Forms.TabControlEventHandler(StructEditor_Selected);

        }

        void StructEditor_Selecting(object sender, System.Windows.Forms.TabControlCancelEventArgs e)
        {
            this.Commit();
        }

        void StructEditor_Selected(object sender, System.Windows.Forms.TabControlEventArgs e)
        {
            UpdateView(this.views[this.SelectedIndex]);
        }

        void UpdateView(View view)
        {
            if (view.Layout == null)
            {
                view.Layout = Program.CreateInstanceFromTypeName(
                    view.LayoutInfo.Attributes["Type"].Value,
                    view.LayoutInfo,
                    new CreateControlDelegate(CreateControl)
                ) as IStructEditorLayout;
                this.TabPages[this.views.IndexOf(view)].Controls.Add(view.Layout as System.Windows.Forms.Control);
                view.IsFresh = false;
            }
            if (view.IsFresh == false)
            {
                foreach (var item in view.Editors)
                {
                    var editor = item.Value;
                    editor.RequestCommit -= new EventHandler(editor_RequestCommit);
                    editor.SelectedItem = this.selectedItem[view.EditorsR[item.Value]];
                    editor.RequestCommit += new EventHandler(editor_RequestCommit);
                }
                view.IsFresh = true;
            }
        }

        System.Windows.Forms.Control CreateControl(System.Xml.XmlNode node)
        {
            if (node.Name == "Control")
            {
                IObjectEditor editor = Program.CreateInstanceFromTypeName(
                    node.Attributes["Editor"].Value,
                    Program.BuildParameterDictionary(node)
                ) as IObjectEditor;
                editor.RequestCommit += new EventHandler(editor_RequestCommit);
                editor.DirtyChanged += new EventHandler(editor_DirtyChanged);
                StructField item = this.fields[node.Attributes["ID"].Value];
                this.views[this.SelectedIndex].Editors.Add(item, editor);
                this.views[this.SelectedIndex].EditorsR.Add(editor, item);
                return editor as System.Windows.Forms.Control;
            }
            else if (node.Name == "Layout")
            {
                IStructEditorLayout layout = Program.CreateInstanceFromTypeName(
                    node.Attributes["Type"].Value,
                    node,
                    new CreateControlDelegate(CreateControl)
                ) as IStructEditorLayout;
                return layout as System.Windows.Forms.Control;
            }
            return null;
        }

        void editor_RequestCommit(object sender, EventArgs e)
        {
            IObjectEditor editor = sender as IObjectEditor;
            var l1 = this.selectedItem[this.views[this.SelectedIndex].EditorsR[sender as IObjectEditor]];
            var l2 = editor.SelectedItem;

            if ((l1 ==null && l2 != null) || ((l1 as IComparable == null || l1.GetType() == l2.GetType()) ? !l1.Equals(l2) : (l1 as IComparable).CompareTo(l2) != 0))
            {
                if (this.DirtyChanged != null)
                    this.DirtyChanged(sender, null);
                this.selectedItem[this.views[this.SelectedIndex].EditorsR[sender as IObjectEditor]] = editor.SelectedItem;
            }
        }

        public void Commit()
        {
            foreach (var item in this.views[this.SelectedIndex].Editors)
            {
                item.Value.Commit();
            }
        }

        void editor_DirtyChanged(object sender, EventArgs e)
        {
            if (this.DirtyChanged != null)
                this.DirtyChanged(sender, null);
        }


        public object SelectedItem
        {
            get{
                return this.selectedItem;
            }
            set
            {
                if (value == null)
                    return;
                if (this.selectedItem != null)
                {
                    this.Commit();
                }
                this.selectedItem = value as Struct;
                foreach (var view in this.views)
                {
                    view.IsFresh = false;
                }
                UpdateView(this.views[this.SelectedIndex]);
            }
        }

        public event EventHandler DirtyChanged;

        public event EventHandler RequestCommit;

        protected class View
        {
            public string Name;
            public string ID;
            public bool IsFresh;
            public System.Xml.XmlNode LayoutInfo;
            public IStructEditorLayout Layout;
            public Dictionary<StructField, IObjectEditor> Editors = new Dictionary<StructField,IObjectEditor>();
            public Dictionary<IObjectEditor, StructField> EditorsR = new Dictionary<IObjectEditor,StructField>();
        }
    }
}
