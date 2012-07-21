using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyTable
    {
        // Fields
        public readonly int size;
        public int[, ,] value;

        // Methods
        public RubyTable(int size, int x_size)
            : this(size, x_size, 1, 1)
        { }

        public RubyTable(int size, int x_size, int y_size)
            : this(size, x_size, y_size, 1)
        { }

        public RubyTable(int size, int x_size, int y_size, int z_size)
        {
            this.size = size;
            this.value = new int[x_size, y_size, z_size];
        }

        private int get_Value(int x, int y, int z)
        {
            return this.value[x, y, z];
        }

        public override string ToString()
        {
            string str = "Ruby::Table{Size = ";
            str = str + this.size.ToString() + "} [";
            for (int i = 0; i < this.value.GetLength(2); i++)
            {
                str = str + "[";
                for (int j = 0; j < this.value.GetLength(1); j++)
                {
                    str = str + "[";
                    for (int k = 0; k < this.value.GetLength(0); k++)
                    {
                        str = str + this.value[k, j, i].ToString() + "  ";
                    }
                    str = str + "]";
                }
                str = str + "]";
            }
            return (str + "]");
        }
    }
}
