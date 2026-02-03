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
            _audioSource?.Play();
        }
    }
}
