using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class NavPoint
    {
        public string FileIdentify;

        private string friendlyName;
        public NavPoint(AbstractFile file)
        {
            this.FileIdentify = file.filename;
            this.friendlyName = file.ToString();
        }

        public void Goto()
        {
            FileManager.Find(this.FileIdentify).Goto(this);
            try
            {
                FileManager.Find(this.FileIdentify).Editor.Focus();
            }
            catch { }
        }

        public override string ToString()
        {
            return this.friendlyName;
        }
    }
}
