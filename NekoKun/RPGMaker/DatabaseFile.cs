using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseFile : AbstractFile
    {
        protected string title;
        protected string className;
        protected bool arrayMode;
        protected Dictionary<string, DatabaseField> fields;
        protected System.Xml.XmlNode layoutInfo;
        protected List<DatabaseItem> contents;
        protected string layout;
        protected DatabaseField idField;
        /*
		<Property Name="Title">公共事件</Property>
		<Property Name="FileName">Data\CommonEvents.rxdata</Property>
		<Property Name="ClassName">RPG::CommonEvent</Property>
		<Property Name="Fields">
			<Field ID="@id" Name="ID" Editor="NekoKun.IntegerEditor"></Field>
			<Field ID="@switch_id" Name="开关编号" Editor="NekoKun.RPGMaker.SwitchIDEditor"></Field>
			<Field ID="@trigger" Name="触发方式" Editor="NekoKun.EnumEditor">
				<Property Name="Source">CommonEventTrigger</Property>
			</Field>
			<Field ID="@name" Name="名称" Editor="NekoKun.SingleTextEditor"></Field>
			<Field ID="@list" Name="事件内容" Editor="NekoKun.RPGMaker.EventCommandListEditor">
				<Property Name="Source">EventCommands</Property>
			</Field>
		</Property>
		<Property Name="Layout">
        */
        public bool ArrayMode { get { return this.arrayMode; } }
        public Dictionary<string, DatabaseField> Fields { get { return this.fields; } }
        public List<DatabaseItem> Contents { get { return this.contents; } }
        public System.Xml.XmlNode LayoutInfo { get { return this.layoutInfo; } }
        public string Layout { get { return this.layout; } }

        public DatabaseFile(Dictionary<string, object> node)
            : base(
                System.IO.Path.Combine(
                    ProjectManager.ProjectDir,
                    node["FileName"].ToString()
                )
              )
        {
            this.title = node["Title"] as string;
            this.className = node["ClassName"] as string;
            if (this.className.StartsWith("[") && this.className.EndsWith("]"))
            {
                this.arrayMode = true;
                this.className = this.className.Substring(1, this.className.Length - 2);
            }
            this.layoutInfo = (node["Layout"] as System.Xml.XmlNodeList)[0];
            this.layout = (node["Layout"] as System.Xml.XmlNodeList)[0].Attributes["Type"].Value;

            this.fields = new Dictionary<string, DatabaseField>();
            var fields = node["Fields"] as System.Xml.XmlNodeList;
            foreach (System.Xml.XmlNode item in fields)
            {
                if (item.Name != "Field") continue;
                var field = new DatabaseField(item);
                this.fields.Add(field.ID, field);
                if (field.Name == "ID")
                    idField = field;
            }

            this.Load();
        }

        protected void Load()
        {
            Object obj = RubyBindings.RubyMarshal.Load(new System.IO.FileStream(this.filename, System.IO.FileMode.Open));
            if (!this.arrayMode)
            {
                this.contents = new List<DatabaseItem>();
                contents.Add(LoadItem(obj));
            }
            else
            {
                var objl = obj as List<object>;
                objl.RemoveAt(0);
                this.contents = new List<DatabaseItem>(Array.ConvertAll<Object, DatabaseItem>(objl.ToArray(), LoadItem));
            }
                
        }

        protected DatabaseItem LoadItem(object RubyObj)
        {
            if (RubyObj is RubyBindings.RubyNil)
                return new DatabaseItem(new Dictionary<DatabaseField, object>());
            if (RubyObj is RubyBindings.RubyExpendObject)
                return LoadItem((RubyObj as RubyBindings.RubyExpendObject).BaseObject);
            if (RubyObj is RubyBindings.RubyExtendedObject)
                return LoadItem((RubyObj as RubyBindings.RubyExtendedObject).BaseObject);
            Dictionary<DatabaseField, object> dict = new Dictionary<DatabaseField, object>();

            foreach (var item in (RubyObj as RubyBindings.RubyObject).Variables)
            {
                //System.Diagnostics.Debug.Assert(this.fields.ContainsKey(item.Key.GetString()), String.Format("RM 又卖萌了：{0} 竟然含有 {1}。", this.className, item.Key.GetString()));
                if (!this.fields.ContainsKey(item.Key.GetString()))
                    this.fields.Add(item.Key.GetString(), new DatabaseField(item.Key.GetString()));

                var field = this.fields[item.Key.GetString()];
                dict.Add(field, item.Value);
            }

            return new DatabaseItem(dict);
        }

        protected override void Save()
        {
            object dump;
            if (this.ArrayMode)
            {
                
                var list = new List<object>();
                list.Add(RubyBindings.RubyNil.Instance);
                foreach (var item in this.contents)
                {
                    var obj = CreateRubyObject(this.className, item);
                    if (this.idField != null)
                        obj.Variables[RubyBindings.RubySymbol.GetSymbol(idField.ID)] = list.Count + 1;

                    list.Add(obj);
                }
                dump = list;
            }
            else
            {
                dump = CreateRubyObject(this.className, this.contents[0]);
            }

            var file = new System.IO.FileStream(this.filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            RubyBindings.RubyMarshal.Dump(file, dump);
            file.Close();
        }

        private RubyBindings.RubyObject CreateRubyObject(string className, DatabaseItem item)
        {
            RubyBindings.RubyObject obj = new NekoKun.RubyBindings.RubyObject();
            obj.ClassName = RubyBindings.RubySymbol.GetSymbol(className);
            foreach (var kv in item.Items)
	        {
                obj.Variables[RubyBindings.RubySymbol.GetSymbol(kv.Key.ID)] = kv.Value;
	        }
            return obj;
        }

        public override AbstractEditor CreateEditor()
        {
            return new DatabaseEditor(this);
        }
    }
}
