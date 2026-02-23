using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _clip;

        [SerializeField]
        private float volume = 0.5f;

        public void Play()
        {
            if (_audioSource == null)
            {
                GameObject listenerObject;
                var listener = FindFirstObjectByType<AudioListener>();
                if (listener != null)
                {
                    listenerObject = listener.gameObject;
                }
                else
                {
                    listenerObject = new GameObject("Dummy audio");
                }

                _audioSource = listenerObject.GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    _audioSource = listenerObject.AddComponent<AudioSource>();
                    _audioSource.playOnAwake = false;
                    _audioSource.spatialBlend = 0;
                }
            }
            if (_clip != null)
            {
                _audioSource.clip = _clip;  
            }
            if (_audioSource == null)
            {
                return;
            }
            _audioSource.volume = volume;
            _audioSource.Play();
        }
    }
}
