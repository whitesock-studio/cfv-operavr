using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/Multitrack", fileName = "MultitrackPopupDataData_")]
    public class MultitrackPopupData : APopupData
    {
        public MultitrackClip[] TrackClips;
    }

    [System.Serializable]
    public class MultitrackClip
    {
        public string Name;
        public AudioClip Clip;
    }
}