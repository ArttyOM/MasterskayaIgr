using UnityEngine;

namespace Code.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayZone : MonoBehaviour
    {
        public Collider2D GetCollider => GetComponent<Collider2D>();
    }
}