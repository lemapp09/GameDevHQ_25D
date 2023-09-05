using UnityEngine;
using UnityEngine.Audio;

namespace GameDevHQ_25dCert
{

    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioClip[] _soundClips;
        [SerializeField] private AudioSource[] _audioSources;
        [SerializeField] private AudioMixer _mixer;

        private void Start() {
            _audioSources[0].clip = _soundClips[0];
            _audioSources[0].Play();
        }

        public void PlayManGrunting() {
            _audioSources[1].Play();
        }

        public void PlayElevator() {
            _audioSources[2].Play();
        }  

        public void StopElevator() {
            _audioSources[2].Stop();
        }    

        public void PlayManMoan() {
            _audioSources[3].Play();
        }       
    }
}
