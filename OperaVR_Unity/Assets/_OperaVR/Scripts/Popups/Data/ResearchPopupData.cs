using System.Linq;
using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/Research", fileName = "ResearchData_")]
    public class ResearchPopupData : APopupData
    {
        public Sprite Image;
        public string Link;
        public string[] AcceptedCodes;

        public bool IsCodeCorrect(string code)
        {
            return AcceptedCodes.Contains(code);
        }
    }
}