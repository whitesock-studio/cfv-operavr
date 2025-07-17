using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class ProfilingQuizPopup : AQuizPopup
    {
        [SerializeField]
        private ProfileView _profileView;

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _profileView.gameObject.SetActive(false);
        }

        protected override void OnQuizCompleted()
        {
            base.OnQuizCompleted();
            
            var score = 0;
            foreach(var quizChoicesSelected in ChoicesSelected)
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
            
            LoadProfileData(profile);
        }

        private void LoadProfileData(ProfileData data)
        {
            _profileView.gameObject.SetActive(true);
            _profileView.LoadProfile(data);
        }
    }
}