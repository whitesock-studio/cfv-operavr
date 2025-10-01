using System;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class ProfilingQuizPopup : AQuizPopup
    {
        [SerializeField]
        private ProfileView _profileView;

        protected override void Awake()
        {
            base.Awake();
            _profileView.OnClose += OnProfileViewClosed;
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _profileView.gameObject.SetActive(false);
            ContentController.gameObject.SetActive(true);
        }

        protected override void OnQuizCompleted()
        {
            base.OnQuizCompleted();
            
            var score = 0;
            foreach(var quizChoicesSelected in ContentController.ChoicesSelected)
            {
                score += quizChoicesSelected.Sum(choice => choice.PointsGiven);
            }

            Debug.Log($"SCORE: {score}");
            if (QuizData.ProfilingData == null)
            {
                Debug.LogError("No profiling data found!");
                return;
            }
            var profile = QuizData.ProfilingData.GetData(score);
            Debug.Log($"Profile: {profile.name}");

            var minScore = 0f;
            var maxScore = 0f;

            foreach (var question in QuizData.Questions)
            {
                var copy = new QuizChoice[question.Choices.Length];
                question.Choices.CopyTo(copy, 0);
                Array.Sort(copy, (c1, c2) => c1.PointsGiven.CompareTo(c2.PointsGiven));

                var choicesLeft = question.MaxChoices;
                var minPoints = 0f;
                var maxPoints = 0f;

                for (var i = 0; i < copy.Length && choicesLeft > 0; i++)
                {
                    if (copy[i].PointsGiven >= 0)
                    {
                        break;
                    }

                    minPoints += copy[i].PointsGiven;
                    choicesLeft--;
                }

                choicesLeft = question.MaxChoices;
                for (var i = copy.Length - 1; i >= 0 && choicesLeft > 0; i--)
                {
                    if (copy[i].PointsGiven <= 0)
                    {
                        break;
                    }

                    maxPoints += copy[i].PointsGiven;
                    choicesLeft--;
                }

                minScore += minPoints;
                maxScore += maxPoints;
            }

            var normalizedScore = Mathf.InverseLerp(minScore, maxScore, score);

            LoadProfileData(profile, normalizedScore);
        }

        private void LoadProfileData(ProfileData data, float normalizedScore)
        {
            ContentController.gameObject.SetActive(false);
            _profileView.gameObject.SetActive(true);
            _profileView.LoadProfile(data, normalizedScore);
        }

        private void OnProfileViewClosed()
        {
            PopupsManager.Instance.ClosePopup(this);
        }
    }
}