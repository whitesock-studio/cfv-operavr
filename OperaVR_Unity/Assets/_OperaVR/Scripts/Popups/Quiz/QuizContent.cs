using UnityEngine;

namespace OperaVR
{
    [System.Serializable]
    public class QuizContent 
    {
        public enum QuizContentType
        {
            Text,
            Audio,
            Image
        }

        public QuizContentType Type;
        public string Text;
        public AudioClip AudioClip;
        public Sprite Image;
    }
}
