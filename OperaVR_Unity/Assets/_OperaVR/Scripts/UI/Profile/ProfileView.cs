using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class ProfileView : MonoBehaviour
    {
        public Action OnClose;

        [Header("Image")]
        [SerializeField]
        private Image _image;

        [Header("Title")]
        [SerializeField]
        private TMP_Text _titleTextDisplayer;
        
        [SerializeField]
        private LocalizedText _titleLocalizedText;

        [Header("Score")]
        [SerializeField]
        private TMP_Text _scoreTextDisplayer;

        [SerializeField]
        private LocalizedText _scoreLocalizedText;
        
        [SerializeField]
        private TMP_Text _scoreValueTextDisplayer;

        [SerializeField]
        private Slider _scoreSlider;

        private const string SCORE_REGEX = @"{\d+}";

        [Header("Description")]
        [SerializeField]
        private TMP_Text _descriptionTextDisplayer;

        [SerializeField]
        private LocalizedText _descriptionLocalizedText;

        [Header("Close Button")]
        [SerializeField]
        private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnClose?.Invoke());
        }

        public void LoadProfile(ProfileData data, float normalizedScore)
        {
            _image.sprite = data.Image;

            _titleTextDisplayer.text = data.Title;
            if (!string.IsNullOrEmpty(data.TitleKey))
            {
                _titleLocalizedText.Key = data.TitleKey;
            }

            _scoreTextDisplayer.text = data.ScoreDescription;
            if (!string.IsNullOrEmpty(data.ScoreDescriptionKey))
            {
                _scoreLocalizedText.Key = data.ScoreDescriptionKey;
            }

            var percentageScore = Mathf.CeilToInt(normalizedScore * 100f).ToString() + "%";

            var text = _scoreTextDisplayer.text;
            var matches = Regex.Matches(text, SCORE_REGEX);
            if (matches.Count > 0)
            {
                text = text.Replace(matches[0].Value, percentageScore);
            }
            _scoreTextDisplayer.text = text;

            _scoreSlider.value = normalizedScore;
            _scoreValueTextDisplayer.text = percentageScore;

            _descriptionTextDisplayer.text = data.Description;
            if (!string.IsNullOrEmpty(data.DescriptionKey))
            {
                _descriptionLocalizedText.Key = data.DescriptionKey;
            }
        }
    }
}
