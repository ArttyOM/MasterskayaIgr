using System.Collections.Generic;

namespace Code.Saves
{
    public interface IPersistentStorage
    {
        public T LoadData<T>(string key);
        public void StoreData<T>(string key, T data);
        public void DeleteData(string key);
        public bool HasKey(string key);
    }
}