using System.Collections.Generic;
using UnityEngine;

namespace FMG
{
    public abstract class CollectionOrder<T> : ScriptableObject
    {
        public abstract IEnumerable<T> Sort(IEnumerable<T> range);
    }
}