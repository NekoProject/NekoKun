using System;
using System.Collections.Generic;
using System.Text;
/*
namespace NekoKun.RPGMaker
{
    public class AudioFileEditor: UI.LynnButton, IObjectEditor
    {
        protected RubyObject orig;
        protected bool changed;
        protected AudioFile audioFile;

        public AudioFileEditor(Dictionary<string, object> Params)
        {
            this.Click += new EventHandler(AudioFileEditor_Click);
        }

        void AudioFileEditor_Click(object sender, EventArgs e)
        {
            AudioFile file = this.audioFile.Clone() as AudioFile;

            if (!this.audioFile.Equals(file))
            {
                this.audioFile = file;
                changed = true;
                UpdateText();
                if (this.DirtyChanged != null)
                    this.DirtyChanged(this, null);
            }
        }

        public void Commit()
        {
            if (this.RequestCommit != null)
                this.RequestCommit(this, null);
        }

        public object SelectedItem
        {
            get
            {
                if (!changed)
                    return orig;
                return this.audioFile.ToRubyObject();
            }
            set
            {
                orig = value as RubyObject;
                changed = false;
                RubyObject obj = orig;
                if (obj != null)
                    this.audioFile = new AudioFile(obj);
                else
                    this.audioFile = new AudioFile();
                UpdateText();
            }
        }

        private void UpdateText()
        {
            this.Text = this.audioFile.ToString();
        }

        public event EventHandler RequestCommit;
        public event EventHandler DirtyChanged;
    }
}

*/