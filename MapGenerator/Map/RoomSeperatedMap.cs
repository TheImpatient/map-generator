using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    class RoomSeperatedMap : Map
    {
        public RoomSeperatedMap(int mapWidth, int mapHeight, int seed)
            : base(mapWidth, mapHeight, seed)
        {
            //PercentAreWalls = percentAreWalls;
        }

        public override void Generate()
        {
            BlankMap();
            MakeRooms();
            //MakeCorridors();
            //ConnectRooms();
        }

        private void MakeRooms()
        {
            DrawRectangle(new Point(50, 50), 6, 10);
        }

        private void MakeCorridors()
        {
            throw new NotImplementedException();
        }

        private void ConnectRooms()
        {
            throw new NotImplementedException();
        }

        private bool DrawRectangle(Point start, int width, int height)
        {
            if (IsOutOfBounds(start.X, start.Y) && RectangleEmpty(start, width, height)) { return false; }

            for (int x = start.X; x < start.X + width; x++)
            {
                Coords[x, start.Y] = (int)Tiles.Wall;
                Coords[x, start.Y + height] = (int)Tiles.Wall;
            }

            for (int y = start.Y + 1; y < (start.X + height) - 1; y++)
            {
                Coords[start.X, y] = (int)Tiles.Wall;
                Coords[start.X + width, y] = (int)Tiles.Wall;
            }

            return true;
        }

        private bool RectangleEmpty(Point start, int width, int height)
        {
            for (int i = 0; i < width * height; i++)
            {
                if (!IsEmpty((i % width) + start.X, (width % i) + start.Y))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
