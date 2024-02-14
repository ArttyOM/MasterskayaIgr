namespace Code.Saves
{
    public class PlayerSettings : StorageData<Settings>
    {
        public PlayerSettings(IPersistentStorage storage) : base(storage)
        {
            Initialize();
        }


        public void SetMusicVolume(float musicVolume)
        {
            Data.MusicVolume = musicVolume;
            Save();
        }

        public void SetSoundVolume(float soundVolume)
        {
            Data.SoundVolume = soundVolume;
            Save();
        }

        public float GetMusicVolume() => Data.MusicVolume;

        public float GetSoundVolume() => Data.SoundVolume;

        protected override string StorageKey => nameof(PlayerSettings);
        protected override Settings CreateNew()
        {
            return new Settings()
            {
                MusicVolume = .5f,
                SoundVolume = .5f
            };
        }
    }


    public struct Settings
    {
        public float MusicVolume;
        public float SoundVolume;
    }
}