using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Core
{
    public class Logger : IOutputProvider, IDisposable
    {
        private NekoKun.UI.Scintilla editor;
        public string Name { get; set; }

        public Logger()
        {
            this.editor = new NekoKun.UI.Scintilla();
            this.editor.IsReadOnly = true;
        }
        
        public void Dispose()
        {
            OutputPad.Detach(this);
            this.editor.Dispose();
        }

        ~Logger()
        {
            Dispose();
        }

        public override string ToString()
        {
            return this.Name == null ? base.ToString() : (this.Name.Length > 0 ? this.Name : base.ToString());
        }

        public void Write(string str)
        {
            this.editor.IsReadOnly = false;
            this.editor.AppendText(str);
            this.editor.IsReadOnly = true;
            this.editor.Caret.Goto(this.editor.TextLength);
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

        public string LogText
        {
            set
            {
                this.editor.Text = value;
            }
            get { return this.editor.Text; }
        }

        public System.Windows.Forms.Control OutputViewContent
        {
            get { return this.editor; }
        }
    }
}
