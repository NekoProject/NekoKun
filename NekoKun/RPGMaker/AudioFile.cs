using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.RPGMaker
{
    public class AudioFile: ICloneable, IEquatable<AudioFile>
    {
        public AudioFile()
        {
            this.Name = "";
            this.Volume = 100;
            this.Pitch = 100;
        }

        public AudioFile(RubyObject obj)
        {
            this.Name = (obj.InstanceVariable["@name"] as string) ?? "";
            this.Volume = Int32.Parse((obj.InstanceVariable["@volume"] ?? 100).ToString());
            this.Pitch = Int32.Parse((obj.InstanceVariable["@pitch"] ?? 100).ToString());
        }

        public RubyObject ToRubyObject()
        {
            RubyObject obj = new RubyObject();
            obj.ClassName = RubySymbol.GetSymbol("RPG::AudioFile");
            obj.InstanceVariable["@name"] = this.Name;
            obj.InstanceVariable["@volume"] = this.Volume;
            obj.InstanceVariable["@pitch"] = this.Pitch;
            return obj;
        }

        public string Name
        {
            get;
            set;
        }

        public int Volume
        {
            get;
            set;
        }

        public int Pitch
        {
            get;
            set;
        }

        public object Clone()
        {
            AudioFile file = new AudioFile();
            file.Name = this.Name;
            file.Pitch = this.Pitch;
            file.Volume = this.Volume;
            return file;
        }

        public bool Equals(AudioFile other)
        {
            if (!this.Name.Equals(other.Name))
                return false;
            if (this.Pitch != other.Pitch)
                return false;
            if (this.Volume != other.Volume)
                return false;
            return true;
        }

        public override string ToString()
        {
            return String.Format("{0}, @{1}, {2}%", this.Name, this.Pitch, this.Volume);
        }
    }
}
