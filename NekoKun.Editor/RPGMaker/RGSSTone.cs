using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class RGSSTone
    {
        int red, green, blue, gray;

        public RGSSTone(int Red, int Green, int Blue) : this(Red, Green, Blue, 0) { }

        public RGSSTone(int Red, int Green, int Blue, int Gray)
        {
            red = Red;
            green = Green;
            blue = Blue;
            gray = Gray;
        }

        public int Red { get { return red; } set { red = value; } }
        public int Green { get { return green; } set { green = value; } }
        public int Blue { get { return blue; } set { blue = value; } }
        public int Gray { get { return gray; } set { gray = value; } }

        public override string ToString()
        {
            return "Ruby::Tone {Red=" + red.ToString() + ", Green=" + green.ToString() + ", Blue=" + blue.ToString() + ", Gray=" + gray.ToString() + "}";
        }
    }
}
