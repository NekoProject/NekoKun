using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NekoKun
{
    public class SettingsFile : AbstractFile
    {
        protected Dictionary<string, object> settings;

        public SettingsFile(string filename, bool project)
            : base(filename)
        {
            try
            {
                using (Stream stream = File.OpenRead(filename))
                {
                    settings = (Dictionary<string, object>)new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream);
                }
            }
            catch
            {
                this.settings = new Dictionary<string, object>();
            }
        }

        public SettingsFile(string filename)
            : this(filename, true)
        {
            
        }

        public object this[string key]
        {
            get {
                if (this.settings.ContainsKey(key))
                    return this.settings[key];
                else
                    return null;
            }
            set {
                this.MakeDirty();
                if (this.settings.ContainsKey(key))
                    this.settings[key] = value;
                else
                    this.settings.Add(key, value);
            }
        }

        protected override void Save()
        {
            using (Stream stream = File.OpenWrite(filename))
            {
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, this.settings);
            }
        }

        public override AbstractEditor CreateEditor()
        {
            throw new NotImplementedException();
        }
    }
}
