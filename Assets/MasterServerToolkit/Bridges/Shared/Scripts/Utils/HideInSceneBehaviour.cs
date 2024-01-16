using UnityEngine;
using UnityEngine.SceneManagement;

namespace MasterServerToolkit.Bridges
{
    public class HideInSceneBehaviour : MonoBehaviour
    {
        #region INSPECTOR

        [Header("Settings")] [SerializeField] private string sceneName;

        #endregion

        private void Start()
        {
            gameObject.SetActive(SceneManager.GetActiveScene().name != sceneName);
        }
    }
}