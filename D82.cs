using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;

namespace aoc2019
{
    internal class D82
    {
        internal string Answer()
        {
            var input = File.ReadAllText("d8.txt").AsSpan();
            var layers = new List<Layer>();
            int nextReadPos = 0;
            while (nextReadPos < input.Length)
            {
                var s = input.Slice(nextReadPos, width * height);
                layers.Add(Layer.FromString(s));
                nextReadPos += width * height;
            }
            
            var render = new Layer();
            foreach (var l in layers)
            {
                for (int i = 0; i < width*height; i++)
                {
                    if (render.GetData(i) == Color.Transparent)
                        render.SetData(i, l.GetData(i));
                }
            }

            var str = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (render.GetData(y * width + x)) {
                        case Color.Black: str.Append(" ");break;
                        case Color.White: str.Append("0");break;
                        case Color.Transparent: str.Append(" ");break;
                    }
                }
                str.AppendLine();
            }
            return str.ToString();
        }
        const int width = 25;
        const int height = 6;
        public enum Color : int {
            Black = 0,
            White = 1,
            Transparent = 2
        }
        internal class Layer
        {
            private int[] data;
            public Layer()
            {
                data = new int[width * height];
                Array.Fill(data, (int)Color.Transparent);
            }
            public static Layer FromString(ReadOnlySpan<char> s)
            {
                Debug.Assert(s.Length == width * height);
                var l = new Layer();
                for (int i = 0; i < s.Length; i++)
                {
                    l.data[i] = s[i] - '0';
                }
                return l;
            }
            public Color GetData(int i)
            {
                Debug.Assert(i < data.Length);
                return (Color)data[i];
            }
            public void SetData(int i, Color c)
            {
                Debug.Assert(i < data.Length);
                data[i] = (int)c;
            }

            public int CountNumberOfDigits(int digit)
            {
                int count = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == digit) count++;
                }
                return count;
            }
        }
    }
}