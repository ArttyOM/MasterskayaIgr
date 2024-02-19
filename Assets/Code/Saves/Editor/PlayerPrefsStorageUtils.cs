using UnityEditor;

namespace Code.Saves.Editor
{
    public static class PlayerPrefsStorageUtils 
    {
        [MenuItem("Tools/Storage/Clear Save")]
        public static void ClearPlayerProfile()
        {
            var storage = new PlayerPrefsStorage();
            storage.DeleteData("PlayerProfile");
        }
    }
}