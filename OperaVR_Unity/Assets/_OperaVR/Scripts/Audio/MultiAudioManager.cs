using System.Collections.Generic;
using UnityEngine;

namespace OperaVR
{
    public class MultiAudioManager : MonoBehaviour
    {
        public static MultiAudioManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private List<float> CurrentClipsPlaying;
        
        private const int MAX_CLIPS_PLAYING = 3;

        private void Update()
        {
            CurrentClipsPlaying ??= new List<float>();
            
            for (var i = CurrentClipsPlaying.Count - 1; i >= 0; i--)
            {
                var clipPlayingEndTime = CurrentClipsPlaying[i];
                if (clipPlayingEndTime < Time.time)
                {
                    CurrentClipsPlaying.RemoveAt(i);
                }
            }
        }

        public bool TryPlay(AudioClip clip)
        {
            CurrentClipsPlaying ??= new List<float>();

            if (CurrentClipsPlaying.Count >= MAX_CLIPS_PLAYING)
            {
                return false;
            }
            
            CurrentClipsPlaying.Add(Time.time + clip.length);
            return true;
        }
    }
}
