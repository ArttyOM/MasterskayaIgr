using Code.DebugTools.Logger;
using UnityEngine;

namespace Code.Enemies
{
    public class CommonEnemy:MonoBehaviour
    {
        public Rigidbody2D GetKinematicRigidbody()
        {
            var allRigidbodies = GetComponentsInChildren<Rigidbody2D>();
            if (allRigidbodies == null || allRigidbodies.Length == 0)
            {
                "GetComponentsInChildren<Rigidbody2D> вернул пустой список".Colored(Color.red).LogError();
                return null;
            }
            foreach (Rigidbody2D rigidbody2D in allRigidbodies)
            {
                if (rigidbody2D.isKinematic) return rigidbody2D;
            }
            {
                "Среди rigidbody2d в дочерних объектах ни одного кинематичного".Colored(Color.red).LogError();
                return null;
            }
        }

        public float Speed { get; set; }
        
    }
}