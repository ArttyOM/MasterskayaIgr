namespace Code.Saves
{
    public abstract class StorageData<T> where T : struct
    {
        private readonly IPersistentStorage _storage;
        protected T Data;

        public StorageData(IPersistentStorage storage)
        {
            _storage = storage;
        }

        public void Initialize()
        {
            var hasSave = _storage.HasKey(StorageKey);
            if(hasSave) Load();
            else Create();
        }

        protected abstract string StorageKey { get; }

        public void Load() => Data = _storage.LoadData<T>(StorageKey);

        protected abstract T CreateNew();
        public void Create()
        {
            Data = CreateNew();
        }
        
        public void Clear()
        {
            _storage.DeleteData(StorageKey);
            Create();
        }

        protected virtual void BeforeSave() {}

        public void Save()
        {
            BeforeSave();
            _storage.StoreData(StorageKey, Data);
        }

        
    }
}