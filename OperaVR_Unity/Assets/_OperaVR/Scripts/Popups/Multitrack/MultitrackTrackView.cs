using UnityEngine;

namespace OperaVR
{
    public class MultitrackTrackView : MonoBehaviour
    {
        public TrackPlayer TrackPlayer;

        private AudioClip _loadedClip;
        public AudioClip LoadedClip => _loadedClip;

        public void LoadClip(AudioClip clip)
        {
            _loadedClip = clip;
            TrackPlayer.LoadClip(clip);
            Stop();
        }

        public void Play()
        {
            TrackPlayer.Play();
        }

        public void Pause()
        {
            TrackPlayer.Pause();
        }

        public void Unpause()
        {
            TrackPlayer.Unpause();
        }

        public void Stop()
        {
            TrackPlayer.Stop();
        }
    }
}
