using UnityEngine;

namespace OperaVR
{
    public class TrackPlayerGroup : MonoBehaviour
    {
        private TrackPlayer[] _trackPlayers;
        
        private void Awake()
        {
            _trackPlayers = GetComponentsInChildren<TrackPlayer>(true);
            foreach (var trackPlayer in _trackPlayers)
            {
                trackPlayer.OnPlay += TrackPlayerActivated;
                trackPlayer.OnUnpause += TrackPlayerActivated;
            }
        }

        private void TrackPlayerActivated(TrackPlayer player)
        {
            foreach (var trackPlayer in _trackPlayers)
            {
                if (trackPlayer == player)
                {
                    continue;
                }

                if (trackPlayer.IsPlaying)
                {
                    trackPlayer.Pause();
                }
            }
        }
    }
}
