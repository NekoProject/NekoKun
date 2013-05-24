using System;
using System.Collections.Generic;

using System.Text;

namespace NekoKun
{
    public abstract class AbstractFile
    {
        public string filename;
        protected AbstractEditor editor;
        protected bool isDirty = false;
        protected bool pendingDelete = false;

        public AbstractFile(string filename)
        {
            this.IsSubfileProvider = false;
            this.IsHidden = false;
            this.filename = filename;
            FileManager.Open(this);
        }

        protected abstract void Save();

        public void Commit()
        {
            if (this.Editor != null)
                this.Editor.Commit();

            if (this.pendingDelete == true)
            {
                this.Delete();
                this.IsDirty = false;
                return;
            }

            Save();
            this.IsDirty = false;
        }

        protected virtual void Delete()
        {
            System.IO.File.Delete(this.filename);
        }

        public void PendingDelete()
        {
            this.pendingDelete = true;

            if (this.editor != null)
            {
                this.editor.Commit();
                this.editor.Close();
            }

            this.MakeDirty();
        }

        public abstract AbstractEditor CreateEditor();

        public override string ToString()
        {
            return System.IO.Path.GetFileNameWithoutExtension(this.filename);
        }

        public void ShowEditor()
        {
            if (this.editor == null)
                this.editor = CreateEditor();

            if (this.editor.IsDisposed)
                this.editor = CreateEditor();

            if (this.editor.Visible == true)
            {
                this.editor.Activate();
                return;
            }

            this.editor.Show(Workbench.Instance.DockPanel);
        }

        public AbstractEditor Editor
        {
            get
            {
                if (this.editor == null)
                    return null;

                if (this.editor.IsDisposed)
                    return null;

                return this.editor;
            }
        }

        public bool IsDirty
        {
            get { return this.isDirty; }
            protected set {
                if (this.isDirty == false && value == true)
                {
                    this.isDirty = true;

                    FileManager.AddPendingChange(this);
                }
                else if (value == false && this.isDirty == true)
                {
                    this.isDirty = false;

                    FileManager.RemovePendingChange(this);
                }
            }
        }

        public void MakeDirty()
        {
            this.IsDirty = true;
        }

        public virtual void Goto(NavPoint pt)
        {
            this.ShowEditor();
        }

        public bool IsSubfileProvider
        {
            get;
            protected set;
        }

        public virtual AbstractFile[] Subfiles
        {
            get
            {
                return null;
            }
        }

        public bool IsHidden
        {
            get;
            protected set;
        }
    }
}
