using UnityEngine;

namespace MasterServerToolkit.Utils
{
    public delegate void FloatTweenCallback(float newValue);

    public partial class Tweener
    {
        public static TweenerActionInfo Tween(float from, float to, float time, FloatTweenCallback callback)
        {
            var currentTime = 0f;
            var difference = Mathf.Abs(from - to);
            var negative = to < from;

            return Start(() =>
            {
                if (from == to)
                {
                    callback?.Invoke(to);
                    return true;
                }

                if (currentTime / time < 1f)
                {
                    currentTime += Time.deltaTime;

                    if (negative)
                        callback?.Invoke(from - difference * (currentTime / time));
                    else
                        callback?.Invoke(from + difference * (currentTime / time));

                    return false;
                }
                else
                {
                    callback.Invoke(to);
                    return true;
                }
            });
        }
    }
}