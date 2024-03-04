using UnityEngine;

namespace FMG
{
    public abstract class CollectionFilter<T> : ScriptableObject
    {
        public abstract bool Satisfy(T item);
    }
}