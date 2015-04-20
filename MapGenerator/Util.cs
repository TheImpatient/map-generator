using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public abstract class Util
    {
        private static Random ran;

        public static int GetRandom(int min, int max)
        {
            if (ran == null)
            {
                ran = new Random();
            }
            return ran.Next(min, max);
        }

        public static Color HsvToRgb(float hue, float saturation, float value)
        {
            while (hue > 1f) { hue -= 1f; }
            while (hue < 0f) { hue += 1f; }
            while (saturation > 1f) { saturation -= 1f; }
            while (saturation < 0f) { saturation += 1f; }
            while (value > 1f) { value -= 1f; }
            while (value < 0f) { value += 1f; }
            if (hue > 0.999f) { hue = 0.999f; }
            if (hue < 0.001f) { hue = 0.001f; }
            if (saturation > 0.999f) { saturation = 0.999f; }
            if (saturation < 0.001f) { return Color.FromArgb((int)(value * 255), (int)(value * 255), (int)(value * 255)); }
            if (value > 0.999f) { value = 0.999f; }
            if (value < 0.001f) { value = 0.001f; }

            float h6 = hue * 6f;
            if (h6 == 6f) { h6 = 0f; }
            int ihue = (int)(h6);
            float p = value * (1f - saturation);
            float q = value * (1f - (saturation * (h6 - (float)ihue)));
            float t = value * (1f - (saturation * (1f - (h6 - (float)ihue))));
            switch (ihue)
            {
                case 0:
                    return Color.FromArgb((int)(value * 255), (int)(t * 255), (int)(p * 255));
                case 1:
                    return Color.FromArgb((int)(q * 255), (int)(value * 255), (int)(p * 255));
                case 2:
                    Color colorfdfd = Color.FromArgb((int)(p * 255), (int)(value * 255), (int)(t * 255));
                    return Color.FromArgb((int)(p * 255), (int)(value * 255), (int)(t * 255));
                case 3:
                    return Color.FromArgb((int)(p * 255), (int)(q * 255), (int)(value * 255));
                case 4:
                    return Color.FromArgb((int)(t * 255), (int)(p * 255), (int)(value * 255));
                default:
                    return Color.FromArgb((int)(value * 255), (int)(p * 255), (int)(q * 255));
            }
        }
    }
}
