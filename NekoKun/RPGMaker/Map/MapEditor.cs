using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapEditor : AbstractEditor, IToolboxProvider
    {
        MapFile map;
        TilesetInfo tileset;

        MapPanel mapPanel;
        TilePanel tilePanel;
        System.Windows.Forms.Panel toolboxPanel;
        System.Windows.Forms.ToolStrip toolstrip;
        System.Windows.Forms.ToolStripButton buttonSelected;
        List<System.Windows.Forms.ToolStripButton> buttonLayers;

        public int TileID = 0;
        public int LayerID = 0;

        public MapEditor(MapFile file)
            : base(file)
        {
            map = file;
            this.tileset = map.TilesetFile[map.TilesetID];

            mapPanel = new MapPanel(map.Layers, tileset);
            mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(mapPanel);

            this.mapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(mapPanel_MouseDown);
            this.mapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(mapPanel_MouseMove);

            tilePanel = new TilePanel(tileset);
            tilePanel.TileSelected += new EventHandler<TilePanel.TileSelectedArgs>(tilePanel_TileSelected);
            tilePanel.Dock = System.Windows.Forms.DockStyle.Fill;

            toolstrip = new System.Windows.Forms.ToolStrip();
            toolstrip.Dock = System.Windows.Forms.DockStyle.Top;
            toolstrip.Stretch = true;
            toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;

            buttonSelected = new System.Windows.Forms.ToolStripButton();
            buttonSelected.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
            buttonSelected.AutoSize = true;
            buttonSelected.Image = tileset[0];
            buttonSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolstrip.Items.Add(buttonSelected);
            toolstrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            toolstrip.ImageScalingSize = this.tileset.TileSize;
            buttonLayers = new List<System.Windows.Forms.ToolStripButton>();
            for (int i = 0; i < this.map.Layers.Count; i++)
			{
                var laybtn = new System.Windows.Forms.ToolStripButton();
                laybtn.Text = string.Format("{0}", i);
                laybtn.CheckState = System.Windows.Forms.CheckState.Checked;
                laybtn.Tag = i;
                laybtn.Click += new EventHandler(laybtn_Click);
                if (this.map.Layers[i].Type == MapLayerType.HalfBlockShadow)
                {
                    laybtn.Enabled = false;
                }
                toolstrip.Items.Add(laybtn);
                buttonLayers.Add(laybtn);
			}
            buttonLayers[LayerID].Font = new System.Drawing.Font(buttonLayers[LayerID].Font, System.Drawing.FontStyle.Bold);

            toolboxPanel = new System.Windows.Forms.Panel();
            this.toolboxPanel.Controls.Add(tilePanel);
            this.toolboxPanel.Controls.Add(toolstrip);
        }

        void laybtn_Click(object sender, EventArgs e)
        {
            buttonLayers[LayerID].Font = new System.Drawing.Font(buttonLayers[LayerID].Font, System.Drawing.FontStyle.Regular);

            System.Windows.Forms.ToolStripButton button = sender as System.Windows.Forms.ToolStripButton;
            int id = (int)button.Tag;
            button.CheckState = button.CheckState == System.Windows.Forms.CheckState.Checked ? System.Windows.Forms.CheckState.Unchecked : System.Windows.Forms.CheckState.Checked;
            this.mapPanel.LayersVisible[id] = button.CheckState == System.Windows.Forms.CheckState.Checked;
            this.mapPanel.InvalidateContents();

            LayerID = id;
            buttonLayers[LayerID].Font = new System.Drawing.Font(buttonLayers[LayerID].Font, System.Drawing.FontStyle.Bold);
        }

        void tilePanel_TileSelected(object sender, TilePanel.TileSelectedArgs e)
        {
            TileID = e.TileID;
            this.buttonSelected.Image = this.tileset[TileID];
            buttonSelected.AutoSize = false;
        }

        void mapPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDrawCell(e.X, e.Y);
        }

        void MouseDrawCell(int x, int y)
        {
            System.Drawing.Point pt = mapPanel.PointToMapPoint(x, y);
            if (mapPanel.MapPointValid(pt))
            {
                this.map.Layers[LayerID].Data[pt.X, pt.Y] = TileID;
                mapPanel.DrawTile(pt.X, pt.Y);
            }
        }

        void mapPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                MouseDrawCell(e.X, e.Y);

            /*
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                this.mapPanel.Zoom *= 2;
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                this.mapPanel.Zoom /= 2;
            */
        }

        protected override void Dispose(bool disposing)
        {
            this.tileset.Dispose();
            base.Dispose(disposing);
        }

        public System.Windows.Forms.Control ToolboxControl
        {
            get { return toolboxPanel; }
        }
    }
}
