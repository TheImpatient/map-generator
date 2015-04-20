using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public class RandomGenerator
    {
        private Random ran;
        private int seed;

        public RandomGenerator(int _seed)
        {
            seed = _seed;
            ran = new Random(seed);
        }

        public RandomGenerator()
        {
            ran = new Random();
        }

        public void Reset()
        {
            ran = seed == 0 ? new Random() : new Random(seed);
        }

        public bool RandomPercent(int percent)
        {
            if (percent >= ran.Next(1, 101))
            {
                return true;
            }
            return false;
        }
        public int GetRandom(int min, int max)
        {
            return ran.Next(min, max);
        }
    }
}
