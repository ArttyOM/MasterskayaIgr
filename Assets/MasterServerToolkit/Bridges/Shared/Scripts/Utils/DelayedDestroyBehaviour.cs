using UnityEngine;

namespace MasterServerToolkit.Bridges
{
    public class DelayedDestroyBehaviour : MonoBehaviour
    {
        #region INSPECTOR

        [Header("Settings")] [SerializeField] private float delayTime = 0f;

        #endregion

        private void Start()
        {
            Destroy(gameObject, delayTime);
        }
    }
}