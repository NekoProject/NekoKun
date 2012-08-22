using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class TilePanel : MapPanelBase
    {
        public event EventHandler<TileSelectedArgs> TileSelected;

        public TilePanel(TilesetInfo tileset)
            : base(new List<MapLayer>(new MapLayer[] { tileset.TilePanelData }), tileset)
        {
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(TilePanel_MouseDown);
        }

        void TilePanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Drawing.Point pt = PointToMapPoint(e.X, e.Y);
                if (MapPointValid(pt))
                {
                    if (this.TileSelected != null)
                        this.TileSelected(this, new TileSelectedArgs(this.layers[0].Data[pt.X, pt.Y]));
                }
            }
        }

        public class TileSelectedArgs : EventArgs
        {
            public short TileID;

            public TileSelectedArgs(short tileID)
                : base()
            {
                TileID = tileID;
            }
        }
    }
}
