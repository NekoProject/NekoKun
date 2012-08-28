using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseFile : ObjectEditor.ObjectFile
    {
        protected string title;
        protected string className;
        protected bool arrayMode;
        protected Dictionary<string, ObjectEditor.StructField> fields;
        protected object contents;
        protected ObjectEditor.StructField idField;
        protected System.Xml.XmlNodeList views;
        protected string clipboardFormat;

        public bool ArrayMode { get { return this.arrayMode; } }
        public Dictionary<string, ObjectEditor.StructField> Fields { get { return this.fields; } }

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
            this.views = (node["Views"] as System.Xml.XmlNodeList);
            if (node.ContainsKey("ClipboardFormat"))
            {
                this.clipboardFormat = (node["ClipboardFormat"] as string);
            }
            if (this.clipboardFormat == null)
            {
                this.clipboardFormat = this.filename;
            }

            this.fields = new Dictionary<string, NekoKun.ObjectEditor.StructField>();
            var fields = node["Fields"] as System.Xml.XmlNodeList;
            foreach (System.Xml.XmlNode item in fields)
            {
                if (item.Name != "Field") continue;
                var field = new ObjectEditor.StructField(item);
                this.fields.Add(field.ID, field);
                if (field.Name == "ID")
                    idField = field;
            }

            this.Load();
        }

        protected void Load()
        {
            using (var fs = new System.IO.FileStream(this.filename, System.IO.FileMode.Open))
            {
                Object obj = RubyBindings.RubyMarshal.Load(fs);
                if (!this.arrayMode)
                {
                    this.contents = LoadItem(obj);
                }
                else
                {
                    var objl = obj as List<object>;
                    objl.RemoveAt(0);
                    this.contents = new List<object>(Array.ConvertAll<Object, ObjectEditor.Struct>(objl.ToArray(), LoadItem));
                }
            }   
        }

        protected ObjectEditor.Struct LoadItem(object RubyObj)
        {
            if (RubyObj is RubyBindings.RubyNil)
                return new ObjectEditor.Struct();
            if (RubyObj is RubyBindings.RubyExpendObject)
                return LoadItem((RubyObj as RubyBindings.RubyExpendObject).BaseObject);
            if (RubyObj is RubyBindings.RubyExtendedObject)
                return LoadItem((RubyObj as RubyBindings.RubyExtendedObject).BaseObject);

            ObjectEditor.Struct dict = new NekoKun.ObjectEditor.Struct();

            foreach (var item in (RubyObj as RubyBindings.RubyObject).Variables)
            {
                if (!this.fields.ContainsKey(item.Key.GetString()))
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    RubyBindings.RubyMarshal.Dump(ms, item.Value);
                    byte[] buf = ms.ToArray();

                    this.fields.Add(
                        item.Key.GetString(), 
                        new ObjectEditor.StructField(
                            item.Key.GetString(),
                            delegate
                            {
                                System.IO.MemoryStream ms2 = new System.IO.MemoryStream(buf);
                                return RubyBindings.RubyMarshal.Load(ms2);
                            }
                        )
                    );
                }

                var field = this.fields[item.Key.GetString()];
                dict.Add(field, item.Value);
            }

            return dict;
        }

        protected override void Save()
        {
            object dump;
            if (this.ArrayMode)
            {
                
                var list = new List<object>();
                list.Add(RubyBindings.RubyNil.Instance);
                foreach (var item in this.contents as List<object>)
                {
                    var obj = CreateRubyObject(this.className, item as ObjectEditor.Struct);
                    if (this.idField != null)
                        obj.Variables[RubyBindings.RubySymbol.GetSymbol(idField.ID)] = list.Count + 1;

                    list.Add(obj);
                }
                dump = list;
            }
            else
            {
                dump = CreateRubyObject(this.className, this.contents as ObjectEditor.Struct);
            }

            var file = new System.IO.FileStream(this.filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            RubyBindings.RubyMarshal.Dump(file, dump);
            file.Close();
        }

        private RubyBindings.RubyObject CreateRubyObject(string className, ObjectEditor.Struct item)
        {
            RubyBindings.RubyObject obj = new NekoKun.RubyBindings.RubyObject();
            obj.ClassName = RubyBindings.RubySymbol.GetSymbol(className);
            foreach (var kv in item)
	        {
                if (kv.Key.ID.StartsWith("@"))
                    obj.Variables[RubyBindings.RubySymbol.GetSymbol(kv.Key.ID)] = kv.Value;
	        }
            return obj;
        }

        public override AbstractEditor CreateEditor()
        {
            ObjectEditor.StructField[] fields = new NekoKun.ObjectEditor.StructField[this.fields.Count];
            this.fields.Values.CopyTo(fields, 0);

            Dictionary<string, object> param = new Dictionary<string, object>();
            param["Views"] = this.views;

            if (this.arrayMode)
            {
                var array = new ObjectEditor.ArrayEditor(new ObjectEditor.StructEditor(param, fields));
                array.ClipboardFormat = this.clipboardFormat;
                array.LoadObject = (item) =>
                {
                    var ms = new System.IO.MemoryStream(item);
                    ms.Seek(4, System.IO.SeekOrigin.Begin);
                    return LoadItem((RubyBindings.RubyMarshal.Load(ms) as List<object>)[0]);
                };
                array.DumpObject = (item) =>
                {
                    var ms = new System.IO.MemoryStream();
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    RubyBindings.RubyMarshal.Dump(ms, new List<object>() { this.CreateRubyObject(this.className, item as ObjectEditor.Struct) });
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
                    bw.Write((int)ms.Length - 4);
                    return ms.ToArray();
                };
                array.CreateDefaultObject = () =>
                {
                    ObjectEditor.Struct obj = new ObjectEditor.Struct();
                    foreach (var item in this.fields.Values)
                    {
                        obj[item] = item.DefaultValue;
                    }
                    return obj;
                };
                return new ObjectEditor.ObjectFileEditor(this, array);
            }
            else
                return new ObjectEditor.ObjectFileEditor(this, new ObjectEditor.StructEditor(param, fields));
        }

        public override string ToString()
        {
            return this.title;
        }

        public override object Contents
        {
            get { return this.contents; }
        }
    }
}
