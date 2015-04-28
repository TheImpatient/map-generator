using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    class RoomSeperatedMap : Map
    {
        public RoomSeperatedMap(int mapWidth, int mapHeight, int seed, int percentAreWalls)
            : base(mapWidth, mapHeight, seed)
        {
            //PercentAreWalls = percentAreWalls;
        }

        public override void Generate()
        {
            //RandomFillMap(PercentAreWalls);
            //MakeCaverns(2);
            //ConnectClusters(25);
        }
    }
}
