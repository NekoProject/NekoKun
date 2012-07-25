using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    public class DockingThemeOcean : WeifenLuo.WinFormsUI.Docking.DockPanelSkin
    {
        public DockingThemeOcean()
        {
            this.AutoHideStripSkin.DockStripGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.AutoHideStripSkin.DockStripGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.AutoHideStripSkin.TabGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.AutoHideStripSkin.TabGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.AutoHideStripSkin.TabGradient.TextColor = Color.Black;
            this.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.White;
            this.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.White;
            this.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.TextColor = Color.Black;
            this.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.TextColor = Color.Black;
            this.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor = Color.White;
            this.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor = Color.White;
            this.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.TextColor = Color.Black;
            this.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.TextColor = Color.Black;
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.LinearGradientMode = LinearGradientMode.Vertical;
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.TextColor = Color.Black;
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.LinearGradientMode = LinearGradientMode.Vertical;
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.TextColor = Color.Black;
        }
    }
}
