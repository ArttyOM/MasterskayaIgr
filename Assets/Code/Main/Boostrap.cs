using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Main
{
    public static class Boostrap
    {
        private const string BootstrapScene = "Bootstrap";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Run()
        {
            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == BootstrapScene) return;
            }
            SceneManager.LoadScene(BootstrapScene, LoadSceneMode.Additive);
        }
    }
}