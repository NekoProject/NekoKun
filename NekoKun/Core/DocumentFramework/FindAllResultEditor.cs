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
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document;

            this.view = new NekoKun.UI.NavPointListView();
            this.view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.view.ContextMenuStrip = new EditContextMenuStrip(this);
            this.Controls.Add(this.view);

            Array.ForEach<NavPoint>(file.Result, this.view.AddItem);
            this.view.SetKeyword(file.Keyword);
        }
    }
}
