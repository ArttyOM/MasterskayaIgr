using UnityEngine;
using UnityEngine.Events;

namespace MasterServerToolkit.Utils
{
    public partial class Tweener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static TweenerActionInfo DelayedCall(float time, UnityAction callback)
        {
            var currentTime = 0f;

            var info = Start(() =>
            {
                if (currentTime / time < 1f)
                {
                    currentTime += Time.deltaTime;
                    return false;
                }
                else
                {
                    callback?.Invoke();
                    return true;
                }
            });

            return info;
        }
    }
}