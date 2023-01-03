using System;
using System.Numerics;

namespace geostorm
{
    class Calc
    {
        public static float DegreesToRad(float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public static float RadToDregrees(float rad)
        {
            return (float)(rad * 180 / Math.PI);
        }

        public static Vector2 PlanRotation(Vector2 point, float angle)
        {
            Vector2 result;
            result.X = (float)(point.X * Math.Cos(angle) - point.Y * Math.Sin(angle));
            result.Y = (float)(point.X * Math.Sin(angle) + point.Y * Math.Cos(angle));

            return result;
        }

        public static float Lerp(float t, float a, float b)
        {
            return a + (b - a) * t;
        }
    }
}
