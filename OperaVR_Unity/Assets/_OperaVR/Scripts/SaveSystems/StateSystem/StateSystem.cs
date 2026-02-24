using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class StateSystem : MonoBehaviour
    {
        [SerializeField]
        private SaveSystemsSettings _settings;

        private float _nextSaveTime;
        
        private AStateTracker[] _stateTrackers;

        private void Awake()
        {
            _nextSaveTime = Time.time + _settings.StartDelay;
        }

        private void Start()
        {
            _stateTrackers = FindObjectsOfType<AStateTracker>(true);
            LoadTrackers(.5f);
        }

        private void Update()
        {
            if (Time.time < _nextSaveTime)
            {
                return;
            }

            _nextSaveTime = Time.time + _settings.TimeBetweenSaves;
            SaveTrackers();
        }

        public void LoadTrackers(float delay)
        {
            StartCoroutine(LoadCoroutine(delay));
        }

        private IEnumerator LoadCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            foreach (var tracker in _stateTrackers)
            {
                tracker.Load(_settings.SceneKey);
            }
        }

        public void SaveTrackers()
        {
            foreach (var tracker in _stateTrackers)
            {
                tracker.Save(_settings.SceneKey);
            }
        }
    }
}
