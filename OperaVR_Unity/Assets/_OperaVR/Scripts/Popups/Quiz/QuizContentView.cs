using UnityEngine;

namespace OperaVR
{
    public class QuizContentView : MonoBehaviour
    {
        [SerializeField]
        private TextView _textView;

        [SerializeField]
        private ImageView _imageView;

        //TODO AudioView

        public void LoadContent(QuizContent content)
        {
            switch (content.Type)
            {
                case QuizContent.QuizContentType.Text:
                    _textView.TextDisplayer.text = content.Text;
                    break;
                case QuizContent.QuizContentType.Audio:
                    //TODO
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
            //TODO AudioView
        }

        public void SetInteractable(bool isInteractable, bool isSelected)
        {
            _textView.SetInteractable(isInteractable);
            _imageView.SetInteractable(isInteractable, isSelected);
            //TODO AudioView
        }

        public void SetHidden(bool isHidden, bool isSelected)
        {
            _textView.SetHidden(isHidden);
            _imageView.SetHidden(isHidden, isSelected);
            //TODO AudioView
        }
    }
}
