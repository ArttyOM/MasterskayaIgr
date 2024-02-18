using Code.DebugTools.Logger;
using Code.Main;
using UnityEngine;

namespace Code.Saves
{
    public class SaveManager : MonoBehaviour
    {
        private void Start()
        {
            Application.quitting += SaveProfile;

            InvokeRepeating(nameof(SaveProfile), 30, 30);
        }
        
        private void SaveProfile()
        {
            "Save Profile".Colored(Color.green).Log();
            ServiceLocator.Instance.Profile.Save();
            ServiceLocator.Instance.Settings.Save();
        }
        
    }
}