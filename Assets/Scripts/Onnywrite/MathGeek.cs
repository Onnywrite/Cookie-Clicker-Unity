using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Onnywrite.Common
{
    public static class MathGeek
    {
        public static void MinMax(ref float min, ref float max)
        {
            var initMin = min;
            var initMax = max;
            min = Mathf.Min(initMin, initMax);
            max = Mathf.Max(initMin, initMax);
        }
        public static void MinMax(ref int min, ref int max) => MinMax(ref min, ref max);

        public static Vector3 RandomVec3(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            MinMax(ref minX, ref maxX);
            MinMax(ref minY, ref maxY);
            MinMax(ref minZ, ref maxZ);

            float x, y, z;
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            z = Random.Range(minZ, maxZ);
            return new(x, y, z);
        }

        public static Vector2 RandomVec2(float minX, float maxX, float minY, float maxY)
            => RandomVec3(minX, maxX, minY, maxY, 0f, 0f);

        public static bool InRange(float num, float minInclusive, float maxInclusive)
        {
            MinMax(ref minInclusive, ref maxInclusive);
            return num >= minInclusive && num <= maxInclusive;
        }

        public static T Pick<T>(this T[] array)
        {
            if (array.Length == 0) return default;
            return array[Random.Range(0, array.Length)];
        }
    }
}