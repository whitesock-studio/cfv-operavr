namespace OperaVR
{
	using TMPro;
	using UnityEngine;

    public class SubtitleDisplayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textDisplayer;
		[SerializeField] GameObject _textContainer;
		private SubtitleManager _manager;

		private void Awake()
		{
			_manager = FindFirstObjectByType<SubtitleManager>();
			if (_manager)
			{
				_manager.OnSubtitleChanged.AddListener(DisplayText);
			}
			DisplayText("");
		}
		private void DisplayText(string text)
        {
            _textDisplayer.text = text;
			_textContainer.SetActive(!string.IsNullOrEmpty(text));
		}
	}
}