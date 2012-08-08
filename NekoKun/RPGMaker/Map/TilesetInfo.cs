using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class TilesetInfo
    {
        protected System.Drawing.Image tileset;
        protected List<System.Drawing.Image> images;
        public System.Drawing.Size TileSize;
        protected Dictionary<int, System.Drawing.Image> tiles;

        public TilesetInfo()
        {
            TileSize = new System.Drawing.Size(32, 32);
            tiles = new Dictionary<int, System.Drawing.Image>();
            string basePath = @"E:\Yichen Lu\rmxp\RPG Maker XP\RGSS\Standard\Graphics\";

            tileset = (System.Drawing.Image.FromFile(basePath + @"Tilesets\001-Grassland01.png"));

            images = new List<System.Drawing.Image>();
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\001-G_Water01.png"));
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\002-G_Shadow01.png"));
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\003-G_Ground01.png"));
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\040-Ground01.png"));
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\041-Grass01.png"));
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\007-G_Undulation01.png"));
            images.Add(System.Drawing.Image.FromFile(basePath + @"Autotiles\008-G_Undulation02.png"));
        }

        public System.Drawing.Image Tileset
        {
            get { return tileset; }
        }

        public List<System.Drawing.Image> Autotiles
        {
            get {
                return this.images;
            }
        }

        public System.Drawing.Image this[int id]
        {
            get
            {
                if (this.tiles.ContainsKey(id))
                    return this.tiles[id];
                else
                {
                    System.Drawing.Bitmap tile = new System.Drawing.Bitmap(this.TileSize.Width, this.TileSize.Height);
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(tile);
                    if (id >= 384)
                    {
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                            new System.Drawing.Point(
                                (id - 384) % 8 * this.TileSize.Width,
                                (id - 384) / 8 * this.TileSize.Height
                            ),
                            this.TileSize
                        );
                        g.DrawImage(this.tileset, 0, 0, rect, System.Drawing.GraphicsUnit.Pixel);
                    }

                    g.Dispose();
                    this.tiles.Add(id, tile);
                    return tile;
                }
            }
        }
    }
}
/*
     
    #==============================================================================
    # ** RPG::Cache
    #==============================================================================
     
    module RPG::Cache
      #--------------------------------------------------------------------------
      # * Auto-Tiles
      #--------------------------------------------------------------------------
      Autotiles = [
        [[27, 28, 33, 34], [ 5, 28, 33, 34], [27,  6, 33, 34], [ 5,  6, 33, 34],
         [27, 28, 33, 12], [ 5, 28, 33, 12], [27,  6, 33, 12], [ 5,  6, 33, 12]],
        [[27, 28, 11, 34], [ 5, 28, 11, 34], [27,  6, 11, 34], [ 5,  6, 11, 34],
         [27, 28, 11, 12], [ 5, 28, 11, 12], [27,  6, 11, 12], [ 5,  6, 11, 12]],
        [[25, 26, 31, 32], [25,  6, 31, 32], [25, 26, 31, 12], [25,  6, 31, 12],
         [15, 16, 21, 22], [15, 16, 21, 12], [15, 16, 11, 22], [15, 16, 11, 12]],
        [[29, 30, 35, 36], [29, 30, 11, 36], [ 5, 30, 35, 36], [ 5, 30, 11, 36],
         [39, 40, 45, 46], [ 5, 40, 45, 46], [39,  6, 45, 46], [ 5,  6, 45, 46]],
        [[25, 30, 31, 36], [15, 16, 45, 46], [13, 14, 19, 20], [13, 14, 19, 12],
         [17, 18, 23, 24], [17, 18, 11, 24], [41, 42, 47, 48], [ 5, 42, 47, 48]],
        [[37, 38, 43, 44], [37,  6, 43, 44], [13, 18, 19, 24], [13, 14, 43, 44],
         [37, 42, 43, 48], [17, 18, 47, 48], [13, 18, 43, 48], [ 1,  2,  7,  8]]
      ]
      #--------------------------------------------------------------------------
      # * Autotile Cache
      #
      #   @autotile_cache = {
      #     filename => { [autotile_id, frame_id, hue] => bitmap, ... },
      #     ...
      #    }
      #--------------------------------------------------------------------------
      @autotile_cache = {}
      #--------------------------------------------------------------------------
      # * Autotile Tile
      #--------------------------------------------------------------------------
      def self.autotile_tile(autotile, tile_id, hue = 0, frame_id = nil)
        # Configures Frame ID if not specified
        if frame_id.nil?
          # Animated Tiles
          frames = autotile.width / 96
          # Configures Animation Offset
          fc = Graphics.frame_count / Animated_Autotiles_Frames
          frame_id = (fc) % frames * 96
        end
     
        # Reconfigure Tile ID
        tile_id %= 48
        # Creates Bitmap
        bitmap = Bitmap.new(32, 32)
        # Collects Auto-Tile Tile Layout
        tiles = Autotiles[tile_id / 8][tile_id % 8]
        # Draws Auto-Tile Rects
        for i in 0...4
          tile_position = tiles[i] - 1
          src_rect = Rect.new(tile_position % 6 * 16 + frame_id,
            tile_position / 6 * 16, 16, 16)
          bitmap.blt(i % 2 * 16, i / 2 * 16, autotile, src_rect)
        end    
        # Set hue
        bitmap.hue_change(hue)
        # Return Auto-Tile
        bitmap
      end
     
    end
    RPG::Cache::Animated_Autotiles_Frames = 16
     
     
    module Tilemap_Options
      #--------------------------------------------------------------------------
      # * Tilemap Options
      #
      #
      #   Print Error Reports when not enough information set to tilemap
      #    - Print_Error_Logs          = true or false
      #
      #   Number of autotiles to refresh at edge of viewport
      #    - Viewport_Padding          = n
      #
      #   When maps are switch, automatically set
      #   $game_map.tileset_settings.flash_data (Recommended : False unless using
      #   flash_data)
      #    - Autoset_Flash_data        = true or false
      #
      #   Duration Between Flash Data Flashes
      #    - Flash_Duration            = n
      #
      #   Color of bitmap (Recommended to use low opacity value)
      #    - Flash_Bitmap_C            = Color.new(255, 255, 255, 50)
      #
      #   Update Flashtiles Default Setting
      #   Explanation : In the Flash Data Addition, because of lag, you may wish
      #   to toggle whether flash tiles flash or not. This is the default state.
      #    - Default_Update_Flashtiles = false
      #--------------------------------------------------------------------------
      Print_Error_Logs          = true
      Autoset_Flash_data        = true
      Viewport_Padding          = 2
      Flash_Duration            = 40
      Flash_Bitmap_C            = Color.new(255, 255, 255, 50)
      Default_Update_Flashtiles = false
    end
     
    #==============================================================================
    # ** Tilemap
    #==============================================================================
     
    class Tilemap
      #--------------------------------------------------------------------------
      # * Public Instance Variables
      #--------------------------------------------------------------------------
      attr_reader   :layers
      attr_accessor :tileset
      attr_accessor :autotiles
      attr_reader   :map_data
      attr_accessor :flash_data
      attr_accessor :priorities
      attr_accessor :visible
      attr_accessor :ox
      attr_accessor :oy
      attr_accessor :refresh_autotiles
      #--------------------------------------------------------------------------
      # * Object Initialization
      #--------------------------------------------------------------------------
      def initialize(viewport)
        # Saves Viewport
        @viewport = viewport
        # Creates Blank Instance Variables
        @layers            = []    # Refers to Array of Sprites or Planes
        @tileset           = nil   # Refers to Tileset Bitmap
        @autotiles         = []    # Refers to Array of Autotile Bitmaps
        @map_data          = nil   # Refers to 3D Array Of Tile Settings
        @flash_data        = nil   # Refers to 3D Array of Tile Flashdata
        @priorities        = nil   # Refers to Tileset Priorities
        @visible           = true  # Refers to Tilest Visibleness
        @ox                = 0     # Bitmap Offsets        
        @oy                = 0     # Bitmap Offsets
        @dispose           = false # Disposed Flag
        @refresh_autotiles = true  # Refresh Autotile Flag
      end
      #--------------------------------------------------------------------------
      # * Setup
      #--------------------------------------------------------------------------
      def setup
        # Creates Layers
        @layers = []
        for l in 0...3
          layer = Sprite.new(@viewport)
          layer.bitmap = Bitmap.new(@map_data.xsize * 32, @map_data.ysize * 32)
          layer.z = l * 150
          layer.zoom_x = 1.0
          layer.zoom_y = 1.0
          @layers << layer
        end
        # Update Flags
        @refresh_data = nil
        @zoom_x   = 1.0
        @zoom_y   = 1.0
        @tone     = nil
        @hue      = 0
        @tilesize = 32
      end
     
      #--------------------------------------------------------------------------
      # * Map Data=
      #--------------------------------------------------------------------------
      def map_data=(map_data)
        # Save Map Data
        @map_data = map_data
       
        setup
       
        # Refresh if able
        begin ; refresh ; rescue ; end
      end
      #--------------------------------------------------------------------------
      # * Dispose
      #--------------------------------------------------------------------------
      def dispose
        # Dispose Layers (Sprites)
        @layers.each { |layer| layer.dispose }
        # Set Disposed Flag to True
        @disposed = true
      end
      #--------------------------------------------------------------------------
      # * Disposed?
      #--------------------------------------------------------------------------
      def disposed?
        return @disposed
      end
      #--------------------------------------------------------------------------
      # * Viewport
      #--------------------------------------------------------------------------
      def viewport
        return @viewport
      end
      #--------------------------------------------------------------------------
      # * Frame Update
      #--------------------------------------------------------------------------
      def update
        # Set Refreshed Flag to On
        needs_refresh = true
        # If Map Data, Tilesize or HueChanges
        if @map_data != @refresh_data
          # Refresh Bitmaps
          refresh
          # Turns Refresh Flag to OFF
          needs_refresh = false
        end
        # Update layer Position offsets
        for layer in @layers
          layer.ox = @ox
          layer.oy = @oy
        end
        # If Refresh Autotiles, Needs Refreshed & Autotile Reset Frame
        if @refresh_autotiles && needs_refresh &&
           Graphics.frame_count % RPG::Cache::Animated_Autotiles_Frames == 0
          # Refresh Autotiles
          refresh_autotiles
        end
      end
      #--------------------------------------------------------------------------
      # * Refresh
      #--------------------------------------------------------------------------
      def refresh
        unless priorities.nil?
          # Saves Map Data & Tilesize
          @refresh_data = @map_data
          @hue      = 0
          @tilesize = 32
          # Passes Through Layers
          for z in 0...@map_data.zsize
            # Passes Through X Coordinates
            for x in 0...@map_data.xsize
              # Passes Through Z Coordinates
              for y in 0...@map_data.ysize
                # Collects Tile ID
                id = @map_data[x, y, z]
                # Skip if 0 tile
                next if id == 0
                # Passes Through All Priorities
                for p in 0..5
                  # Skip If Priority Doesn't Match
                  next unless p == @priorities[id]
                  # Cap Priority to Layer 3
                  p = 2 if p > 2
                  # Draw Tile
                  id < 384 ? draw_autotile(x, y, p, id) : draw_tile(x, y, p, id)
                end
              end
            end
          end
        end
      end  
      #--------------------------------------------------------------------------
      # * Refresh Auto-Tiles
      #--------------------------------------------------------------------------
      def refresh_autotiles
        # Auto-Tile Locations
        autotile_locations = Table.new(@map_data.xsize, @map_data.ysize,
          @map_data.zsize)
        # Get X Tiles
        x1 = [@ox / @tilesize - Tilemap_Options::Viewport_Padding, 0].max
        x2 = [@viewport.rect.width / @tilesize +
              Tilemap_Options::Viewport_Padding, @map_data.xsize].min
        # Get Y Tiles
        y1 = [@oy / @tilesize - Tilemap_Options::Viewport_Padding, 0].max
        y2 = [@viewport.rect.height / @tilesize +
              Tilemap_Options::Viewport_Padding, @map_data.ysize].min
        # Passes Through Layers
        for z in 0...@map_data.zsize
          # Passes Through X Coordinates
          for x in x1...x2
            # Passes Through Y Coordinates
            for y in y1...y2
              # Collects Tile ID
              id = @map_data[x, y, z]
              # Skip if 0 tile
              next if id == 0
              # Skip If Non-Animated Tile
              next unless @autotiles[id / 48 - 1].width / 96 > 1 if id < 384
              # Get Priority
              p = @priorities[id]
              # Cap Priority to Layer 3
              p = 2 if p > 2
              # If Autotile
              if id < 384
                # Draw Auto-Tile
                draw_autotile(x, y, p, id)
                for l in (p+1)...@map_data.zsize
                  id_l = @map_data[x, y, l]
                  draw_tile(x, y, p, id_l)
                end
                # Save Autotile Location
                autotile_locations[x, y, z] = 1
              # If Normal Tile
              else
                # If Autotile Drawn
                if autotile_locations[x, y, z] == 1
                  # Redraw Normal Tile
                  draw_tile(x, y, p, id)
                  # Draw Higher Tiles
                  for l in (p+1)...@map_data.zsize
                    id_l = @map_data[x, y, l]
                    draw_tile(x, y, p, id_l)
                  end
                end
              end
            end
          end
        end
      end    
      #--------------------------------------------------------------------------
      # * Draw Tile
      #--------------------------------------------------------------------------
      def draw_tile(x, y, z, id)
        rect = Rect.new((id - 384) % 8 * 32, (id - 384) / 8 * 32, 32, 32)
        x *= @tilesize
        y *= @tilesize
        if @tile_width == 32 && @tile_height == 32
          @layers[z].bitmap.blt(x, y, @tileset, rect)
        else
          dest_rect = Rect.new(x, y, @tilesize, @tilesize)
          @layers[z].bitmap.stretch_blt(dest_rect, @tileset, rect)
        end
      end
      #--------------------------------------------------------------------------
      # * Draw Auto-Tile
      #--------------------------------------------------------------------------
      def draw_autotile(x, y, z, tile_id)
        # Gets Autotile Filename
        autotile_num = tile_id / 48 - 1
        # Reconfigure Tile ID
        tile_id %= 48
        # Gets Generated Autotile Bitmap Section
        bitmap = RPG::Cache.autotile_tile(autotiles[autotile_num], tile_id, @hue)
       
        # Calculates Tile Coordinates
        x *= @tilesize
        y *= @tilesize
        @layers[z].bitmap.blt(x, y, bitmap, Rect.new(0, 0, 32, 32))
      end
      #--------------------------------------------------------------------------
      # * Collect Bitmap
      #--------------------------------------------------------------------------
      def bitmap
        # Creates New Blank Bitmap
        bitmap = Bitmap.new(@layers[0].bitmap.width, @layers[0].bitmap.height)
        # Passes Through All Layers
        for layer in @layers
          bitmap.blt(0, 0, layer.bitmap,
            Rect.new(0, 0, bitmap.width, bitmap.height))
        end
        # Return Bitmap
        return bitmap
      end
    end
*/
