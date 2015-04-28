using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    class Map
    {
        public enum Tiles : int
        {
            Empty = 0, Wall = 1
        }

        private int mapWidth;
        private int mapHeight;

        public int[,] Coords { get; set; }
        public int MapWidth { get { return mapWidth; } set { mapWidth = value > 1 ? value : 1; } }
        public int MapHeight { get { return mapHeight; } set { mapHeight = value > 1 ? value : 1; } }
        public int Seed { get; set; }

        protected RandomGenerator ranGen;

        public Map(int mapWidth, int mapHeight, int seed)
        {
            this.Coords = new int[mapWidth, mapHeight];
            Seed = seed;
            MapHeight = mapHeight;
            MapWidth = mapWidth;

            ranGen = new RandomGenerator(Seed);
        }

        public Map(int mapWidth, int mapHeight)
        {
            this.Coords = new int[mapWidth, mapHeight];
            MapHeight = mapHeight;
            MapWidth = mapWidth;

            ranGen = new RandomGenerator();
        }

        public virtual void Generate()
        {
            BlankMap();
        }

        public virtual string getDescription()
        {
            string returnString = string.Join("  ",
                                              "Width:",
                                              MapWidth.ToString(),
                                              "\tHeight:",
                                              MapHeight.ToString(),
                                              "\tSeed:",
                                              Seed.ToString(),
                                              Environment.NewLine
                                             );
            return returnString;
        }

        public override string ToString()
        {
            return string.Join("  ",
                                              "Width:",
                                              MapWidth.ToString(),
                                              "\tHeight:",
                                              MapHeight.ToString(),
                                              "\tSeed:",
                                              Seed.ToString(),
                                              Environment.NewLine
                                             );
        }

        public void BlankMap()
        {
            for (int x = 0, y = 0; y < MapHeight; y++)
            {
                for (x = 0; x < MapWidth; x++)
                {
                    Coords[x, y] = 0;
                }
            }
        }

        protected int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
        {
            int startX = x - scopeX;
            int startY = y - scopeY;
            int endX = x + scopeX;
            int endY = y + scopeY;

            int iX = startX;
            int iY = startY;

            int wallCounter = 0;

            for (iY = startY; iY <= endY; iY++)
            {
                for (iX = startX; iX <= endX; iX++)
                {
                    if (!(iX == x && iY == y))
                    {
                        if (IsWall(iX, iY))
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }
            return wallCounter;
        }

        public void BucketFillFirstOccurance(int replacement, int value)
        {
            FloodFill(GetFirstOccurance(value), replacement);
        }

        public void EmptyNonWalls()
        {
            for (int x = 0, y = 0; y <= MapHeight - 1; y++)
            {
                for (x = 0; x <= MapWidth - 1; x++)
                {
                    if (Coords[x, y] != (int)Tiles.Wall)
                    {
                        Coords[x, y] = (int)Tiles.Empty;
                    }
                }
            }
        }

        public void FloodFill(Point pt, int b)
        {
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(pt);
            while (q.Count > 0)
            {
                Point n = q.Dequeue();
                if (Coords[n.X, n.Y] != 0)
                    continue;
                Point w = n, e = new Point(n.X + 1, n.Y);
                while ((w.X > 0) && (Coords[w.X, w.Y] == 0))
                {
                    Coords[w.X, w.Y] = b;
                    if ((w.Y > 0) && (Coords[w.X, w.Y -1] == 0))
                        q.Enqueue(new Point(w.X, w.Y - 1));
                    if ((w.Y < MapHeight - 1) && (Coords[w.X, w.Y + 1] == 0))
                        q.Enqueue(new Point(w.X, w.Y + 1));
                    w.X--;
                }
                while ((e.X < MapWidth - 1) && (Coords[e.X, e.Y] == 0))
                {
                    Coords[e.X, e.Y] = b;
                    if ((e.Y > 0) && (Coords[e.X, e.Y - 1] == 0))
                        q.Enqueue(new Point(e.X, e.Y - 1));
                    if ((e.Y < MapHeight - 1) && (Coords[e.X, e.Y + 1] == 0))
                        q.Enqueue(new Point(e.X, e.Y + 1));
                    e.X++;
                }
            }
        }

        protected Dictionary<int, List<Point>> PointsInRadius(Point center, int radius)
        {
            Dictionary<int, List<Point>> points = new Dictionary<int, List<Point>>();

            for (int x = center.X - radius; x < center.X - radius + radius * 2; x++)
            {
                for (int y = center.Y - radius; y < center.Y - radius + radius * 2; y++)
                {
                    if (!IsOutOfBounds(x, y) && Util.Distance(center, new Point(x, y)) <= radius)
                    {
                        if (!points.ContainsKey(Coords[x, y]))
                        {
                            points[Coords[x, y]] = new List<Point>();
                        }
                        points[Coords[x, y]].Add(new Point(x, y));
                    }
                }
            }

            return points;
        }

        protected Point GetFirstOccurance(int value)
        {
            for (int x = 0, y = 0; y <= MapHeight - 1; y++)
            {
                for (x = 0; x <= MapWidth - 1; x++)
                {
                    if (Coords[x, y] == value)
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(-1, -1);
        }

        protected bool IsWall(int x, int y)
        {
            // Consider out-of-bound a wall
            if (IsOutOfBounds(x, y))
            {
                return true;
            }

            if (Coords[x, y] == (int)Tiles.Wall)
            {
                return true;
            }

            return false;
        }

        protected bool IsEmpty(int x, int y)
        {
            // Consider out-of-bound a wall
            if (IsOutOfBounds(x, y))
            {
                return false;
            }

            if (Coords[x, y] == (int)Tiles.Empty)
            {
                return true;
            }

            return false;
        }

        protected bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return true;
            }
            else if (x > MapWidth - 1 || y > MapHeight - 1)
            {
                return true;
            }
            return false;
        }
    }
}
