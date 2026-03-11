using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class StateSystem : MonoBehaviour
    {
        public static StateSystem Instance;
        
        [SerializeField]
        private SaveSystemsSettings _settings;
        public SaveSystemsSettings Settings => _settings;

        private float _nextSaveTime;
        
        private AStateTracker[] _stateTrackers;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _nextSaveTime = Time.time + _settings.StartDelay;
        }

        private void Start()
        {
            _stateTrackers = FindObjectsOfType<AStateTracker>(true);
            LoadTrackers(.5f);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
            {
                WorldData.ClearAllVariables();
                _nextSaveTime = Time.time + _settings.StartDelay;
                
                foreach (var quest in SpatialBridge.questService.quests.Values)
                {
                    if (quest.status != QuestStatus.None)
                    {
                        quest.Reset();
                    }
                }
                return;
            }
            
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
