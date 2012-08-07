using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class LogFile : AbstractFile
    {
        private string log;

        public LogFile()
            : base(@"\\.\NekoKun\Log", false)
        {
            this.log = "";
        }

        public override string ToString()
        {
            return "日志";
        }

        protected override void Save()
        {
            return;
        }

        public void Write(string str)
        {
            this.log += str;
            if (this.Editor != null)
            {
                (this.Editor as LogEditor).Append(str);
            }
        }

        public void Write(string format, params object[] args)
        {
            this.Write(String.Format(format, args));
        }

        public void Write<T>(T arg)
        {
            this.Write(arg.ToString());
        }

        public void WriteLine(string str)
        {
            this.Write(str);
            this.Write(System.Environment.NewLine);
        }

        public void WriteLine(string format, params object[] args)
        {
            this.Write(String.Format(format, args));
            this.Write(System.Environment.NewLine);
        }

        public void WriteLine<T>(T arg)
        {
            this.Write<T>(arg);
            this.Write(System.Environment.NewLine);
        }

        public void Log(string message)
        {
            this.WriteLine("[{0}] {1}", System.DateTime.Now.ToLongTimeString(), message);
        }

        public void Log(string format, params object[] args)
        {
            this.Log(String.Format(format, args));
        }

        public override AbstractEditor CreateEditor()
        {
            return new LogEditor(this);
        }

        public string LogText
        {
            set
            {
                this.log = value;
                
                if (editor != null)
                {
                    (this.Editor as LogEditor).Editor.Text = this.log;
                }
            }
            get { return this.log; }
        }

    }
}
