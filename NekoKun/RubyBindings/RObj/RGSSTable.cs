using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RGSSTable : IRubyUserdefinedDumpObject
    { 
        protected short[, ,] value;
        protected byte dimensions;

        public RGSSTable(int xsize)
            : this(1, xsize, 1, 1) { }

        public RGSSTable(int xsize, int ysize)
            : this(2, xsize, ysize, 1) { }

        public RGSSTable(int xsize, int ysize, int zsize)
            : this(3, xsize, ysize, zsize) { }

        internal RGSSTable(byte dimensions, int xsize, int ysize, int zsize)
        {
            this.dimensions = dimensions;
            value = new short[xsize, ysize, zsize];
        }

        public int XSize
        {
            get { return value.GetLength(0); }
        }

        public int YSize
        {
            get { return value.GetLength(1); }
        }

        public int ZSize
        {
            get { return value.GetLength(2); }
        }

        public short this[int x]
        {
            get { return this[x, 0, 0]; }
            set { this[x, 0, 0] = value; }
        }

        public short this[int x, int y]
        {
            get { return this[x, y, 0]; }
            set { this[x, y, 0] = value; }
        }

        public short this[int x, int y, int z]
        {
            get { return this.value[x, y, z]; }
            set { this.value[x, y, z] = value; }
        }

        public RGSSTable(byte[] raw)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream(raw);
            System.IO.BinaryReader r = new System.IO.BinaryReader(s);
            this.dimensions = (byte)r.ReadInt32();
            int x = r.ReadInt32();
            int y = r.ReadInt32();
            int z = r.ReadInt32();
            long check = r.ReadInt32();
            this.value = new short[x, y, z];
            for (int j = 0; j < z; j++)
            {
                for (int k = 0; k < y; k++)
                {
                    for (int l = 0; l < x; l++)
                    {
                        this[l, k, j] = r.ReadInt16();
                    }
                }
            }
        }

        public byte[] Dump()
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            System.IO.BinaryWriter w = new System.IO.BinaryWriter(s);
            w.Write((int)this.dimensions);
            w.Write((int)this.XSize);
            w.Write((int)this.YSize);
            w.Write((int)this.ZSize);
            w.Write((int)this.XSize * this.YSize * this.ZSize);
            for (int i = 0; i < this.ZSize; i++)
            {
                for (int j = 0; j < this.YSize; j++)
                {
                    for (int k = 0; k < this.XSize; k++)
                    {
                        w.Write(this[k, j, i]);
                    }
                }
            }
            w.Close();
            return s.ToArray();
        }

        public string ClassName
        {
            get { return "Table"; }
        }

    }
}
