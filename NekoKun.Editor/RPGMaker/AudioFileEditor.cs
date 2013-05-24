using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.RPGMaker
{
    public class AudioFileEditor: AbstractObjectEditor
    {
        UI.LynnButton btn = new NekoKun.UI.LynnButton();

        public AudioFileEditor(Dictionary<string, object> Params)
            : base(Params)
        {
            btn.Click += new EventHandler(AudioFileEditor_Click);
        }

        protected RubyObject orig;
        protected bool changed;
        protected AudioFile audioFile;

        void AudioFileEditor_Click(object sender, EventArgs e)
        {
            AudioFile file = this.audioFile.Clone() as AudioFile;

            if (!this.audioFile.Equals(file))
            {
                this.audioFile = file;
                changed = true;
                UpdateText();
                this.MakeDirty();
            }
        }

        private void UpdateText()
        {
            btn.Text = this.audioFile.ToString();
        }

        protected override void InitControl()
        {
            orig = this.selectedItem as RubyObject;
            RubyObject obj = orig;
            if (obj != null)
                this.audioFile = new AudioFile(obj);
            else
                this.audioFile = new AudioFile();

            btn.Text = this.audioFile.ToString();
        }

        public override System.Windows.Forms.Control Control
        {
            get { return btn; }
        }

        public override void Commit()
        {
            this.selectedItem = audioFile;
        }
    }
}
