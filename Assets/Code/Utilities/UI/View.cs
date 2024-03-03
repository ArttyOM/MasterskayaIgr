using UnityEngine;

namespace FMG
{
    public abstract class View<T> : MonoBehaviour
    {
        public abstract void Render(T data);
    }
}