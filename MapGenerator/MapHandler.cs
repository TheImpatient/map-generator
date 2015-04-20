using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    class MapHandler
    {
        Map map;

        public MapHandler(Map m)
        {
            map = m;
            map.Seed = m.Seed == 0 ? Util.GetRandom(0, 10000) : m.Seed;
        }

        public MapHandler(int seed)
        {
            map = new CaveMap(40, 21, 45, seed);
            map.Generate();
        }

        public Bitmap GetBitmap()
        {
            Bitmap bm = new Bitmap(map.MapWidth, map.MapHeight);
            Color c = new Color();
            RandomGenerator r = new RandomGenerator();
            Dictionary<int, Color> colors = new Dictionary<int, Color>();

            colors.Add(0,Color.White);
            colors.Add(1,Color.Black);

            for (int x = 0, y = 0; y <= map.MapHeight - 1; y++)
            {
                for (x = 0; x <= map.MapWidth - 1; x++)
                {
                    if (map.Coords[x, y] == 1)
                    {
                        c = Color.Black;
                    }
                    else if (map.Coords[x, y] == 0)
                    {
                        c = Color.White;
                    }
                    else if (map.Coords[x, y] >= 2)
                    {
                        if (!colors.ContainsKey(map.Coords[x, y]))
                        {
                            float randomcolor = r.GetRandom(0, 100);
                            randomcolor = randomcolor / 100f;
                            colors.Add(map.Coords[x, y], Util.HsvToRgb(randomcolor, 0.9f, 0.6f));
                        }
                        c = colors[map.Coords[x, y]];
                    }

                    bm.SetPixel(x, y, c);
                }
            }
            return bm;
        }

        public void Generate()
        {
            map.Generate();
        }
        public System.Drawing.Size GetSize()
        {
            return new System.Drawing.Size(map.MapWidth, map.MapHeight);
        }

        public void Resize(int width, int height)
        {
            map.MapWidth = width;
            map.MapHeight = height;
        }

        public void setSeed(int seed)
        {
            map.Seed = seed;
        }

        public void DeColor()
        {
            map.EmptyNonWalls();
        }

        public string getDescription()
        {
            return map.getDescription();
        }
    }
}
