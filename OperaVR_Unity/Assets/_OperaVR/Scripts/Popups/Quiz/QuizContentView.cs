using UnityEngine;

namespace OperaVR
{
    public class QuizContentView : MonoBehaviour
    {
        [SerializeField]
        private TextView _textView;

        [SerializeField]
        private ImageView _imageView;

        [SerializeField]
        private TrackPlayer _trackPlayer;

        public void LoadContent(QuizContent content)
        {
            _textView.gameObject.SetActive(content.Type == QuizContent.QuizContentType.Text);
            _imageView.gameObject.SetActive(content.Type == QuizContent.QuizContentType.Image);
            _trackPlayer.gameObject.SetActive(content.Type == QuizContent.QuizContentType.Audio);
            switch (content.Type)
            {
                case QuizContent.QuizContentType.Text:
                    _textView.TextDisplayer.text = content.Text;
                    break;
                case QuizContent.QuizContentType.Audio:
                    _trackPlayer.LoadClip(content.AudioClip);
                    break;
                case QuizContent.QuizContentType.Image:
                    _imageView.SetImageSprite(content.Image);
                    break;
            }
        }

        public void SetSelected(bool isSelected)
        {
            _textView.SetSelected(isSelected);
            _imageView.SetSelected(isSelected);
            //TODO TrackPlayer
        }

        public void SetInteractable(bool isInteractable, bool isSelected)
        {
            _textView.SetInteractable(isInteractable);
            _imageView.SetInteractable(isInteractable, isSelected);
            //TODO TrackPlayer
        }

        public void SetHidden(bool isHidden, bool isSelected)
        {
            _textView.SetHidden(isHidden);
            _imageView.SetHidden(isHidden, isSelected);
            //TODO TrackPlayer
        }
    }
}
