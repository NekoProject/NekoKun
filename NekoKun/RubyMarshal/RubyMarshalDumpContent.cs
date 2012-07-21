using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using orzTech.NekoKun.Base;
using System.Collections.Generic;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    public class RubyMarshalDumpDisplayBinding : IDisplayBinding
    {
        public IViewContent OpenFile(string fileName)
        {
            //if (fileName.EndsWith("rvdata", false, null) || fileName.EndsWith("rxdata", false, null) || fileName.EndsWith("rvdata2", false, null))
            //{
                return new RubyMarshalDumpContent(fileName);
            //}
            //else
            //{ 
            //    return null; 
            //}
        }
    }

    public class RubyMarshalDumpContent : AbstractViewContent, IUndoHandler, IClipboardHandler
    {
        List<Object> rubyObjectList = null;
        ObjectEditor.ObjectEditor editor = new ObjectEditor.ObjectEditor();

        public RubyMarshalDumpContent()
        {
            this.UntitledName = "UntitledRubyMarshalDump";
            this.IsViewOnly = false;
            editor.Dock = DockStyle.Fill;
            //this.Icon = System.Drawing.Icon.FromHandle(ICSharpCode.Core.ResourceService.GetBitmap("Icons.Textbox").GetHicon());
        }

        public RubyMarshalDumpContent(string fileName)
            : this()
        {
            Load(fileName);
        }

        public override Control Control
        {
            get { return editor; }
        }

        public override void Load(string fileName)
        {
            Stream fileStream = File.OpenRead(fileName);
            rubyObjectList = new List<object>();
            Exception e = null;
            try
            {
                while (fileStream.Position < fileStream.Length)
                {
                    rubyObjectList.Add(RubyMarshal.Load(fileStream));
                }
            }
            catch (Exception ex) {
                e = ex;
            }
            fileStream.Close();

            editor.Object = rubyObjectList;
            SetTitleAndFileName(fileName);
            if (e != null)
            {
                editor.Object = e;
                this.IsDirty = true;
            }
        }

        public override void Save(string fileName)
        {
            //File.WriteAllText(fileName, textBox.Text);
            SetTitleAndFileName(fileName);
        }

        public override void Dispose()
        {
            editor.Dispose();
            base.Dispose();
        }

        #region IClipboardHandler implementation
        bool IClipboardHandler.CanPaste
        {
            get
            {
                return false;
            }
        }

        bool IClipboardHandler.CanCut
        {
            get
            {
                return false;
            }
        }

        bool IClipboardHandler.CanCopy
        {
            get
            {
                return false;
            }
        }

        bool IClipboardHandler.CanDelete
        {
            get
            {
                return false;
            }
        }

        void IClipboardHandler.Paste()
        {
            throw new NotImplementedException();
        }

        void IClipboardHandler.Cut()
        {
            throw new NotImplementedException();
        }

        void IClipboardHandler.Copy()
        {
            throw new NotImplementedException();
        }

        void IClipboardHandler.Delete()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUndoHandler implementation
        bool IUndoHandler.CanUndo
        {
            get
            {
                return false;
            }
        }

        bool IUndoHandler.CanRedo
        {
            get
            {
                return false;
            }
        }

        void IUndoHandler.Undo()
        {
            throw new NotImplementedException();
        }

        void IUndoHandler.Redo()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
