using Random = UnityEngine.Random;

namespace Sundry.Helper
{
    public static class Maths
    {
        private const string StringRange = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static float Remap(float value, float a, float b, float c, float d)
        {
            return c + (value - a) / (b - a) * (d - c);
        }


        public static string RandomString(int length)
        {
            var chars = new char[length];
            for (var i=0; i<length; i++)
            {
                var index = Random.Range(0, StringRange.Length);
                chars[i] = StringRange[index];
            }
            return string.Join("", chars);
        }
    }
}