using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class ObjectFileEditor : AbstractEditor
    {
        public ObjectFileEditor(ObjectFile file, AbstractObjectEditor editor)
            : base(file)
        {
            System.Windows.Forms.Control ed = editor.Control;
            ed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(ed);

            editor.DirtyChanged += new EventHandler(editor_DirtyChanged);
            editor.SelectedItem = file.Contents;
        }

        void editor_DirtyChanged(object sender, EventArgs e)
        {
            File.MakeDirty();
        }
    }
}
