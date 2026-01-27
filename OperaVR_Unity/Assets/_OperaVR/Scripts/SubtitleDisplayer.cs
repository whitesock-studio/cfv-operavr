namespace OperaVR
{
	using TMPro;
	using UnityEngine;

    public class SubtitleDisplayer : MonoBehaviour
    {
	    public static SubtitleDisplayer Instance;
	    
        [SerializeField] TextMeshProUGUI _textDisplayer;
		[SerializeField] GameObject _textContainer;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DisplayText("");
		}
		
		public void DisplayText(string text)
        {
            _textDisplayer.text = text;
			_textContainer.SetActive(!string.IsNullOrEmpty(text));
		}
	}
}