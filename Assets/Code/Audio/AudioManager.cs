using Code.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        
        public void SetMusicVolumeFromNormalized(float value)
        {
            var volume = value.ConvertLinearToDecibel();
            _mixer.SetFloat("MusicVolume", volume);
        }
        public void SetSoundVolumeFromNormalized(float value)
        {
            var volume = value.ConvertLinearToDecibel();
            _mixer.SetFloat("SoundVolume", volume);
        }
    }
}