using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.Projectiles
{
    /// <summary>Inspectable ReactiveProperty.</summary>
    [Serializable]
    public class Vector2IntReactiveProperty : ReactiveProperty<Vector2Int>
    {
        
        
        public static readonly IEqualityComparer<Vector2Int> Vector2IntComparer = new Vector2IntEqualityComparer();

        public Vector2IntReactiveProperty()
        {
        }

        public Vector2IntReactiveProperty(Vector2Int initialValue)
            : base(initialValue)
        {
        }

        protected override IEqualityComparer<Vector2Int> EqualityComparer => Vector2IntComparer;
    };
    
    sealed class Vector2IntEqualityComparer : IEqualityComparer<Vector2Int>
    {
        public bool Equals(Vector2Int self, Vector2Int vector)
        {
            return self.x.Equals(vector.x) && self.y.Equals(vector.y);
        }

        public int GetHashCode(Vector2Int obj)
        {
            return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2;
        }
    }

}