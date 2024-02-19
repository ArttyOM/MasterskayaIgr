using UnityEngine;

namespace Code.Utilities
{
    public static class DecibelExtensions
    {
        public static float ConvertLinearToDecibel(this float linearVolume)
        {
            return Mathf.Log10(Mathf.Max(0.0001f, linearVolume)) * 20.0f;
        }

        public static float ConvertDecibelToLinear(this float decibelVolume)
        {
            return Mathf.Pow(10, decibelVolume / 20.0f);
        }
    }
}