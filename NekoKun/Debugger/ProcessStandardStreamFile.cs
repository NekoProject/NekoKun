using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace NekoKun.Debugger
{
    public class ProcessStandardStreamFile : AbstractFile
    {
        public string Buffer;
        public Process Process;
        public TextReader StandardOutputReader;
        public TextReader StandardErrorReader;
        public StreamWriter StandardInputWriter;
        BackgroundWorker outputWorker;
        BackgroundWorker errorWorker;
        public bool Active = false;
        public bool IsGame = false;

        public ProcessStandardStreamFile(Process Process, bool isGame)
            : this(Process)
        {
            this.IsGame = isGame;
        }
        public ProcessStandardStreamFile(Process Process)
            : base(String.Format(@"\\.\NekoKun\Process\{0}\StandardStream", Process.Id))
        {
            this.Process = Process;
            this.Active = true;
            
            this.Process.Exited += new EventHandler(Process_Exited);

            this.outputWorker = new BackgroundWorker();
            this.outputWorker.WorkerReportsProgress = true;
            this.outputWorker.WorkerSupportsCancellation = true;
            this.outputWorker.DoWork += new DoWorkEventHandler(outputWorker_DoWork);
            this.outputWorker.ProgressChanged += new ProgressChangedEventHandler(outputWorker_ProgressChanged);

            this.errorWorker = new BackgroundWorker();
            this.errorWorker.WorkerReportsProgress = true;
            this.errorWorker.WorkerSupportsCancellation = true;
            this.errorWorker.DoWork += new DoWorkEventHandler(errorWorker_DoWork);
            this.errorWorker.ProgressChanged += new ProgressChangedEventHandler(errorWorker_ProgressChanged);

            this.StandardOutputReader = TextReader.Synchronized(Process.StandardOutput);
            this.StandardErrorReader = TextReader.Synchronized(Process.StandardError);
            this.StandardInputWriter = this.Process.StartInfo.StandardOutputEncoding == null ?
                this.Process.StandardInput :
                new StreamWriter(this.Process.StandardInput.BaseStream, this.Process.StartInfo.StandardOutputEncoding);

            this.outputWorker.RunWorkerAsync();
            this.errorWorker.RunWorkerAsync();
        }

       void outputWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.Active == false) return;
            if (e.UserState is OutputEvent)
            {
                OutputEvent outputEvent = e.UserState as OutputEvent;
                this.Write(outputEvent.Output);
            }
        }

        void outputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (outputWorker.CancellationPending == false)
            {
                int count = 0;
                char[] buffer = new char[1024];
                do
                {
                    StringBuilder builder = new StringBuilder();
                    try
                    {
                        count = this.StandardOutputReader.Read(buffer, 0, 1024);
                        builder.Append(buffer, 0, count);
                        outputWorker.ReportProgress(0, new OutputEvent() { Output = builder.ToString() });
                    }
                    catch { return; }
                    if (outputWorker.CancellationPending) return;
                    if (this.Process.HasExited) this.Process_Exited(sender, e);
                } while (count > 0);
                
                System.Threading.Thread.Sleep(200);
            }
        }


        // the wonderful way from http://www.codeproject.com/Articles/335909/Embedding-a-Console-in-a-C-Application

        void errorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.Active == false) return;
            if (e.UserState is ErrorEvent)
            {
                ErrorEvent errorEvent = e.UserState as ErrorEvent;

                this.Write(errorEvent.Error);
            }
        }

        void errorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (errorWorker.CancellationPending == false)
            {
                int count = 0;
                char[] buffer = new char[1024];
                do
                {
                    StringBuilder builder = new StringBuilder();
                    try
                    {
                        count = this.StandardErrorReader.Read(buffer, 0, 1024);
                        builder.Append(buffer, 0, count);
                        errorWorker.ReportProgress(0, new ErrorEvent() { Error = builder.ToString() });
                    }
                    catch { return; }
                    if (outputWorker.CancellationPending) return;
                    if (this.Process.HasExited) this.Process_Exited(sender, e);
                } while (count > 0);

                System.Threading.Thread.Sleep(200);
            }
        }

        void Process_Exited(object sender, EventArgs e)
        {
            if (this.Active)
            {
                this.Active = false;
                this.outputWorker.CancelAsync();
                this.errorWorker.CancelAsync();
                this.outputWorker.Dispose();
                this.errorWorker.Dispose();
                this.StandardErrorReader = null;
                this.StandardInputWriter = null;
                this.StandardOutputReader = null;

                this.Write(String.Format("\n\n# [{1}] {0} 已经退出，返回值为 {2}。\n", Process.ProcessName, Process.Id, Process.ExitCode));

                if (this.IsGame)
                {
                    var curSel = Process.ExitCode / 65536;
                    var curLine = Process.ExitCode % 65536 - 1;
                    // this.Write(String.Format("# 脚本 {0} 的 {1} 行发生了错误。\n", Workbench.Instance.ScriptList.scripts[curSel].ToString(), curLine + 1));
                }
            }
        }

        public override string ToString()
        {
            return String.Format("[{1}] {0}", Process.ProcessName, Process.Id);
        }

        protected override void Save()
        {
            return;
        }

        public override AbstractEditor CreateEditor()
        {
            return new ProcessStandardStreamEditor(this);
        }

        public void Write(string text)
        {
            if (text == null) return;

            this.Buffer += text;

            if (this.Editor != null)
                (this.Editor as ProcessStandardStreamEditor).Append(text);
        }

        public void WriteToSTDIN(string text)
        {
            this.Buffer += text;
            try
            {
                this.StandardInputWriter.WriteLine(text);
                this.StandardInputWriter.Flush();
            }
            catch { }
        }

        internal class OutputEvent
        {
            public string Output
            {
                get;
                set;
            }
        }

        internal class ErrorEvent
        {
            public string Error
            {
                get;
                set;
            }
        }
    }
}