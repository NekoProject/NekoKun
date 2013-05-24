using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NekoKun.ObjectEditor
{
    public class StructEditor : AbstractObjectEditor
    {
        protected List<View> views;
        protected new Struct selectedItem;
        protected Dictionary<string, StructField> fields;
        protected UI.LynnTabControl tab = new NekoKun.UI.LynnTabControl();
        public StructEditor(Dictionary<string, object> Params, StructField[] fields): base(null)
        {
            this.fields = new Dictionary<string, StructField>();
            foreach (var field in fields)
            {
                this.fields.Add(field.ID, field);
            }

            this.views = new List<View>();
            View rawView = new View();
            rawView.Name = "原始视图";
            rawView.ID = "Raw";
            this.views.Add(rawView);
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

            tab.TabPages.Clear();
            foreach (View view in views)
            {
                var page = new System.Windows.Forms.TabPage(view.Name);
                page.BackColor = Color.Transparent;
                page.Padding = new System.Windows.Forms.Padding(5);
                tab.TabPages.Add(page);
            }
            if (tab.TabCount > 1)
            {
                tab.SelectedIndex = 1;
            }
            tab.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(StructEditor_Deselecting);
            tab.Selected += new System.Windows.Forms.TabControlEventHandler(StructEditor_Selected);

        }

        void StructEditor_Deselecting(object sender, System.Windows.Forms.TabControlCancelEventArgs e)
        {
            this.Commit();
        }

        void StructEditor_Selected(object sender, System.Windows.Forms.TabControlEventArgs e)
        {
            UpdateView(this.views[tab.SelectedIndex]);
        }

        void UpdateView(View view)
        {
            if (view.Layout == null)
            {
                if (view.LayoutInfo == null)
                {
                    StructField[] field = new StructField[this.fields.Values.Count];
                    this.fields.Values.CopyTo(field, 0);
                    view.Layout = new RawLayout(field, CreateControl);
                }
                else
                {
                    view.Layout = Program.CreateInstanceFromTypeName(
                        view.LayoutInfo.Attributes["Type"].Value,
                        view.LayoutInfo,
                        new CreateControlDelegate(CreateControl)
                    ) as IStructEditorLayout;
                }
                tab.TabPages[this.views.IndexOf(view)].Controls.Add(view.Layout as System.Windows.Forms.Control);
                view.IsFresh = false;
            }
            if (view.IsFresh == false)
            {
                foreach (var item in view.Editors)
                {
                    item.Value.SelectedItem = this.selectedItem[view.EditorsR[item.Value]];
                }
                view.IsFresh = true;
            }
        }

        System.Windows.Forms.Control CreateControl(System.Xml.XmlNode node)
        {
            if (node.Name == "Control")
            {
                AbstractObjectEditor editor = Program.CreateInstanceFromTypeName(
                    node.Attributes["Editor"].Value,
                    Program.BuildParameterDictionary(node)
                ) as AbstractObjectEditor;
                editor.DirtyChanged += new EventHandler(editor_DirtyChanged);
                StructField item = this.fields[node.Attributes["ID"].Value];
                this.views[tab.SelectedIndex].Editors.Add(item, editor);
                this.views[tab.SelectedIndex].EditorsR.Add(editor, item);
                return editor.Control;
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

        public override void Commit()
        {
            foreach (var item in this.views[tab.SelectedIndex].Editors)
            {
                item.Value.Commit();
                this.selectedItem[this.views[tab.SelectedIndex].EditorsR[item.Value]] = item.Value.SelectedItem;
            }

            foreach (var view in this.views)
            {
                if (view != this.views[tab.SelectedIndex])
                    view.IsFresh = false;
            }

        }

        void editor_DirtyChanged(object sender, EventArgs e)
        {
            MakeDirty();
        }

        protected class View
        {
            public string Name;
            public string ID;
            public bool IsFresh;
            public System.Xml.XmlNode LayoutInfo;
            public IStructEditorLayout Layout;
            public Dictionary<StructField, AbstractObjectEditor> Editors = new Dictionary<StructField, AbstractObjectEditor>();
            public Dictionary<AbstractObjectEditor, StructField> EditorsR = new Dictionary<AbstractObjectEditor, StructField>();
        }

        protected override void InitControl()
        {
            this.selectedItem = (Struct)base.selectedItem;
            foreach (var view in this.views)
            {
                view.IsFresh = false;
            }
            UpdateView(this.views[tab.SelectedIndex]);
        }

        public override Control Control
        {
            get { return this.tab; }
        }
    }
}
