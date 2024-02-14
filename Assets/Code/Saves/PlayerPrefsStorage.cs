using UnityEngine;

namespace Code.Saves
{
    public class PlayerPrefsStorage : IPersistentStorage
    {
        public T LoadData<T>(string key)
        {
            return PlayerPrefs.HasKey(key) ? JsonUtility.FromJson<T>(PlayerPrefs.GetString(key)) : default;
        }

        public void StoreData<T>(string key, T data)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public void DeleteData(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
    }
}