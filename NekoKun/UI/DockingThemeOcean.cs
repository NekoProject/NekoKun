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
            this.AutoHideStripSkin.TabGradient.TextColor = SystemColors.ControlDarkDark;

            this.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = SystemColors.ControlLightLight;
            this.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = SystemColors.ControlLightLight;
            this.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = SystemColors.ControlLight;
            this.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = SystemColors.ControlLight;

            this.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.EndColor = Color.FromArgb(191, 219, 255);

            this.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor = SystemColors.Control;
            this.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor = SystemColors.Control;

            this.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor = Color.Transparent;
            this.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor = Color.Transparent;
            this.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.TextColor = SystemColors.ControlDarkDark;

            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.LinearGradientMode = LinearGradientMode.Vertical;
            this.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.TextColor = SystemColors.ActiveCaptionText;

            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = Color.FromArgb(191, 219, 255);
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.LinearGradientMode = LinearGradientMode.Vertical;
            this.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.TextColor = SystemColors.InactiveCaptionText;
        }
    }
}
