using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace NekoKun.Core
{
    public static class DrawingHelper
    {

        public static System.Drawing.Image DecodeBase64Image(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static System.Drawing.Color ParseColor(string name)
        {
            System.Drawing.Color col = System.Drawing.Color.FromName(name);
            if (!col.IsKnownColor)
            {
                System.Text.RegularExpressions.Match match;
                match = System.Text.RegularExpressions.Regex.Match(name, @"#?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})#?");
                if (match.Success)
                {
                    return System.Drawing.Color.FromArgb(System.Convert.ToInt32(match.Groups[1].Value, 16), System.Convert.ToInt32(match.Groups[2].Value, 16), System.Convert.ToInt32(match.Groups[3].Value, 16));
                }
                match = System.Text.RegularExpressions.Regex.Match(name, @"#?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})#?");
                if (match.Success)
                {
                    return System.Drawing.Color.FromArgb(System.Convert.ToInt32(match.Groups[1].Value, 16), System.Convert.ToInt32(match.Groups[2].Value, 16), System.Convert.ToInt32(match.Groups[3].Value, 16), System.Convert.ToInt32(match.Groups[4].Value, 16));
                }
                return System.Drawing.Color.Empty;
            }
            else
                return System.Drawing.Color.FromName(name);
        }

    }
}
