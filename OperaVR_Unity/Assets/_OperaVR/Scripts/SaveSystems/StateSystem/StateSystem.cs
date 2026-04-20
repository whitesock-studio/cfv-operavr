using System.Collections;
using System.Collections.Generic;
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

        private const int TRACKERS_SAVED_PER_FRAME = 2;

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
            LoadTrackers(1f);
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
            
            StartCoroutine(SaveTrackers());
        }
        
        public void LoadTrackers(float delay)
        {
            StartCoroutine(LoadCoroutine(delay));
        }

        private IEnumerator LoadCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
         
            Debug.Log("----- Start Load -----");
            _stateTrackers = FindObjectsOfType<AStateTracker>(true);
            
            var trackersPerFrame = 5;
            var t = 0;
            foreach (var tracker in _stateTrackers)
            {
                tracker.Load(_settings.SceneKey);
                Debug.Log($"----- {tracker.name} : Loaded");
                t++;
                if (t >= TRACKERS_SAVED_PER_FRAME)
                {
                    t = 0;
                    yield return new WaitForSeconds(.1f);            
                }
            }
            Debug.Log("----- Load Completed -----");
        }

        public IEnumerator SaveTrackers()
        {
            Debug.Log("----- Start Save -----");
            _stateTrackers = FindObjectsOfType<AStateTracker>(true);
            
            var t = 0;
            foreach (var tracker in _stateTrackers)
            {
                if (tracker.IsDirty)
                {
                    tracker.Save(_settings.SceneKey);
                    Debug.Log($"----- {tracker.name} : Saved");
                    t++;
                }
                if (t >= TRACKERS_SAVED_PER_FRAME)
                {
                    t = 0;
                    yield return null;
                }
            }
            Debug.Log("----- Save Completed -----");
            _nextSaveTime = Time.time + _settings.TimeBetweenSaves;
        }
    }
}
