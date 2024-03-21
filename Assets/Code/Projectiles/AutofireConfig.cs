using UnityEngine;

namespace Code.Projectiles
{
    [CreateAssetMenu(fileName = "DefaultAutofireConfig", menuName = "Config/AutofireConfig", order = 0)]
    public class AutofireConfig : ScriptableObject
    {
        public float periodInSeconds = 2f;
    }
}