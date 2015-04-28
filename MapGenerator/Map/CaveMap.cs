using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    class CaveMap : Map
    {
        public int PercentAreWalls { get; set; }
        public CaveMap(int mapWidth, int mapHeight)
            : base(mapWidth, mapHeight)
        {
            PercentAreWalls = 45;
        }

        public CaveMap(int mapWidth, int mapHeight, int seed, int percentAreWalls)
            : base(mapWidth, mapHeight, seed)
        {
            PercentAreWalls = percentAreWalls;
        }

        public override void Generate()
        {
            RandomFillMap(PercentAreWalls);
            MakeCaverns(2);
            ConnectClusters(25);
        }

        private void ColorCluster()
        {
            int i = 2;
            while (!GetFirstOccurance((int)Tiles.Empty).Equals(new Point(-1, -1)))
            {
                BucketFillFirstOccurance(i, (int)Tiles.Empty);
                i++;
            }

        }

        //TODO: Seperate calculating closestpoints into their own functions
        private void ConnectClusters(int radius)
        {
            //Flood fill clusters with their own unique values
            ColorCluster();

            if (GetNumClusters() == 0)
            {
                //Nothing to connect
                return;
            }

            //Divide clusters into pointlists
            Dictionary<int, List<Point>> allClusters = getAllClusters();

            //Get main cluster
            int largestSize = allClusters.Max(y => y.Value.Count);
            KeyValuePair<int, List<Point>> largestCluster = allClusters.Where(x => x.Value.Count == largestSize).FirstOrDefault();

            //Get all non-main clusters of the apropriate size
            Dictionary<int, List<Point>> smallerClusters = allClusters.Where(x => x.Value.Count != largestSize && x.Value.Count >= 15).ToDictionary(x => x.Key, x => x.Value);

            //Get all clusters that are too small
            Dictionary<int, List<Point>> smallestClusters = allClusters.Where(x => x.Value.Count != largestSize && x.Value.Count < 15).ToDictionary(x => x.Key, x => x.Value);

            //Remove too small clusters
            RemoveClusters(smallestClusters, smallestClusters.Keys.ToList<int>());
            Dictionary<int, List<Point>> validClusters = allClusters;
            foreach (int i in smallestClusters.Keys.ToList<int>())
            {
                validClusters.Remove(i);
            }

            //Calculate all cluster centers
            Dictionary<int, Point> clusterCenters = new Dictionary<int, Point>();
            foreach (KeyValuePair<int, List<Point>> c in validClusters) { clusterCenters[c.Key] = GetPointCenter(c.Value); }


            Dictionary<int, List<Point>> pointsInRadius;
            List<int> keyForRemoval = new List<int>();
            Dictionary<int, Point> closestMainPoints = new Dictionary<int, Point>();
            Point closest = new Point();

            //Calculate closest points of main cluster to other cluster centers
            foreach (KeyValuePair<int, Point> c in clusterCenters)
            {
                pointsInRadius = PointsInRadius(c.Value, radius);

                //Check is clustercenters is within radius
                if (pointsInRadius.ContainsKey(largestCluster.Key))
                {
                    closest = GetClosestPoint(pointsInRadius[largestCluster.Key], c.Value);
                    closestMainPoints.Add(c.Key, closest);
                }
                else
                {
                    keyForRemoval.Add(c.Key);
                }
            }
            //Remove too far away clusters
            RemoveClusters(validClusters, keyForRemoval);
            foreach (int i in keyForRemoval)
            {
                validClusters.Remove(i);
            }


            Dictionary<int, Point> closestOtherPoints = new Dictionary<int, Point>();

            //Calculate nearest point to calculated closestPoints of corresponding clusters
            foreach (KeyValuePair<int, Point> c in closestMainPoints)
            {
                pointsInRadius = PointsInRadius(c.Value, radius/5);
                if (pointsInRadius.ContainsKey(c.Key))
                {
                    closest = GetClosestPoint(pointsInRadius[c.Key], c.Value);
                    closestOtherPoints.Add(c.Key, closest);
                }
                else
                {
                    keyForRemoval.Add(c.Key);
                }
            }

            //Remove too far away clusters
            RemoveClusters(validClusters, keyForRemoval);
            foreach (int i in keyForRemoval)
            {
                validClusters.Remove(i);
                closestMainPoints.Remove(i);
            }

            //Draw line between two closest points
            Dictionary<int, List<Point>> main = new Dictionary<int, List<Point>>();
            Dictionary<int, List<Point>> other = new Dictionary<int, List<Point>>();

            foreach (KeyValuePair<int, Point> c in closestMainPoints)
            {
                if (c.Key != largestCluster.Key) {
                    main = PointsInRadius(c.Value, 1);
                    other = PointsInRadius(closestOtherPoints[c.Key], radius / 10);

                    foreach (Point m in main[largestCluster.Key])
                    {
                        foreach (Point o in other[c.Key])
                        {
                            DrawLine(m, o, (int)Tiles.Empty);
                        }
                        
                    }
                }
            }

            //Merge Clusters into one value
            EmptyNonWalls();
        }

        private void RemoveClusters(Dictionary<int, List<Point>> clusters, List<int> keys)
        {
            foreach (KeyValuePair<int, List<Point>> c in clusters)
            {
                foreach (Point p in c.Value)
                {
                    if (keys.Contains(c.Key))
                    {
                        Coords[p.X, p.Y] = (int)Tiles.Wall;
                    }
                }
            }
        }


        private void DrawLine(Point from, Point to, int value)
        {
            int dX = to.X - from.X;
            int dY = to.Y - from.Y;

            double rico = 0;

            if (dX != 0)
            {
                //Rico = Delta x / Delta y
                rico = ((double)dY / (double)dX);

            }
            else if (dY > 0)
            {
                //Rico is infinity
                rico = double.PositiveInfinity;
            }
            else if (dY < 0)
            {
                //Rico is -infinity
                rico = double.NegativeInfinity;
            }

            double b = (-1 * (rico * from.X)) + from.Y;

            int _x = 0;
            int _y = 0;

            if (Math.Abs(dX) > Math.Abs(dY)) //Loop over X-axis
            {
                if (dX >= 0) //Loop forwards
                {
                    for (int i = from.X; i < dX + from.X; i++)
                    {
                        _x = i;
                        _y = (int)(Math.Round(rico * _x) + b);
                        Coords[_x, _y] = value;
                    }
                }
                if (dX < 0) //Loop forwards
                {
                    for (int i = from.X; i > dX + from.X; i--)
                    {
                        _x = i;
                        _y = (int)(Math.Round(rico * _x) + b);
                        Coords[_x, _y] = value;
                    }
                }
            }
            else if (Math.Abs(dX) <= Math.Abs(dY)) //Loop over Y-axis
            {
                if (dY >= 0) //Loop forwards
                {
                    for (int i = from.Y; i < dY + from.Y; i++)
                    {
                        _y = i;
                        _x = (int)(Math.Round((_y - b) / rico));
                        if (double.IsInfinity(rico))
                        {
                            Coords[from.X, _y] = value;
                        }
                        else
                        {
                            Coords[_x, _y] = value;
                        }
                    }
                }
                if (dY < 0) //Loop backwards
                {
                    for (int i = from.Y; i > dY + from.Y; i--)
                    {
                        _y = i;
                        _x = (int)(Math.Round((_y - b) / rico));
                        if (double.IsInfinity(rico))
                        {
                            Coords[from.X, _y] = value;
                        }
                        else
                        {
                            Coords[_x, _y] = value;
                        }
                    }
                }
            }
        }



        private int GetNumClusters()
        {
            return getAllClusters().Keys.Count();
        }

        private Point GetClosestPoint(List<Point> points, Point point)
        {
            double distance = double.PositiveInfinity;
            Point closest = new Point();

            foreach (Point p in points)
            {
                var _d = Util.Distance(p, point);

                if (_d < distance)
                {
                    distance = _d;
                    closest = p;
                }
            }
            return closest;
        }

        private Point GetPointCenter(List<Point> points)
        {
            int Xs = 0;
            int Ys = 0;

            foreach (Point p in points)
            {
                Xs += p.X;
                Ys += p.Y;
            }

            return new Point((int)Math.Round((double)(Xs / points.Count)), (int)Math.Round((double)(Ys / points.Count)));
        }

        private Dictionary<int, List<Point>> getAllClusters()
        {
            Dictionary<int, List<Point>> allClusters = new Dictionary<int, List<Point>>();
            for (int x = 0, y = 0; y <= MapHeight - 1; y++)
            {
                for (x = 0; x <= MapWidth - 1; x++)
                {
                    if (Coords[x, y] > (int)Tiles.Wall)
                    {
                        if (!allClusters.ContainsKey(Coords[x, y]))
                        {
                            allClusters[Coords[x, y]] = new List<Point>();
                        }
                        allClusters[Coords[x, y]].Add(new Point(x, y));
                    }
                }
            }
            return allClusters;
        }

        public override string getDescription()
        {
            string returnString = string.Join("  ",
                                              "Width:",
                                              MapWidth.ToString(),
                                              "\tHeight:",
                                              MapHeight.ToString(),
                                              "\t% Walls:",
                                              PercentAreWalls.ToString(),
                                              "\tSeed:",
                                              Seed.ToString(),
                                              "\tClusters:",
                                              GetNumClusters(),
                                              Environment.NewLine
                                             );
            return returnString;
        }

        public void MakeCaverns()
        {
            for (int x = 0, y = 0; y <= MapHeight - 1; y++)
            {
                for (x = 0; x <= MapWidth - 1; x++)
                {
                    Coords[x, y] = PlaceWallLogic(x, y);
                }
            }
        }

        public void MakeCaverns(int iterations)
        {
            //Iterations
            for (int i = 0; i < iterations; i++)
            {
                //Alternate directions every other
                if(i%2 == 0){
                    for (int x = 0, y = 0; y <= MapHeight - 1; y++)
                    {
                        for (x = 0; x <= MapWidth - 1; x++)
                        {
                            Coords[x, y] = PlaceWallLogic(x, y);
                        }
                    }
                }
                else
                {
                    for (int x = 0, y = MapHeight-1; y > 0 - 1; y--)
                    {
                        for (x = MapWidth-1; x > 0 - 1; x--)
                        {
                            Coords[x, y] = PlaceWallLogic(x, y);
                        }
                    }
                }
            }

        }

        public void RandomFillMap(int percent)
        //TODO: Have 2d arr of illegal spaces to check up with
        {
            //Reset generator
            ranGen = new RandomGenerator(Seed);

            // New, empty map
            Coords = this.Coords = new int[MapWidth, MapHeight];

            int mapMiddle = 0; // Temp variable
            for (int column = 0, row = 0; row < MapHeight; row++)
            {
                for (column = 0; column < MapWidth; column++)
                {
                    // If coordinants lie on the the edge of the map (creates a border)
                    if (column == 0)
                    {
                        Coords[column, row] = 1;
                    }
                    else if (row == 0)
                    {
                        Coords[column, row] = 1;
                    }
                    else if (column == MapWidth - 1)
                    {
                        Coords[column, row] = 1;
                    }
                    else if (row == MapHeight - 1)
                    {
                        Coords[column, row] = 1;
                    }
                    // Else, fill with a wall a random percent of the time
                    else
                    {
                        mapMiddle = (MapHeight / 2);

                        //Fill middle with empty space to avoid seperation
                        if (row == mapMiddle)
                        {
                            //Coords[column, row] = 0;
                            Coords[column, row] = ranGen.RandomPercent(percent) ? 1 : 0;
                        }
                        else
                        {
                            Coords[column, row] = ranGen.RandomPercent(percent) ? 1 : 0;
                        }
                    }
                }
            }
        }

        public int PlaceWallLogic(int x, int y)
        {
            int numWalls = GetAdjacentWalls(x, y, 1, 1);


            if (Coords[x, y] == 1)
            {
                if (numWalls >= 4)
                {
                    return 1;
                }
                if (numWalls < 2)
                {
                    return 0;
                }

            }
            else
            {
                if (numWalls >= 5)
                {
                    return 1;
                }
            }
            return 0;
        }

        public override string ToString()
        {
            string returnString = string.Join(" ",
                                              "Width:",
                                              MapWidth.ToString(),
                                              "\tHeight:",
                                              MapHeight.ToString(),
                                              "\t% Walls:",
                                              PercentAreWalls.ToString(),
                                              "\tSeed:",
                                              Seed.ToString(),
                                              Environment.NewLine
                                             );
            return returnString;
        }
    }
}
