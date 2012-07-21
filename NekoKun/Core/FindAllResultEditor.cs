using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class FindAllResultEditor : AbstractEditor
    {
        UI.NavPointListView view;

        public FindAllResultEditor(FindAllResultFile file)
            : base(file)
        {
            this.DockableAreas = WeifenLuo.WinFormsUI.DockAreas.DockBottom | WeifenLuo.WinFormsUI.DockAreas.DockLeft | WeifenLuo.WinFormsUI.DockAreas.DockRight | WeifenLuo.WinFormsUI.DockAreas.DockTop | WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.Document;

            this.view = new NekoKun.UI.NavPointListView();
            this.view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.view.ContextMenuStrip = new EditContextMenuStrip(this);
            this.Controls.Add(this.view);

            Array.ForEach<NavPoint>(file.Result, this.view.AddItem);
            this.view.SetKeyword(file.Keyword);
        }
    }
}
