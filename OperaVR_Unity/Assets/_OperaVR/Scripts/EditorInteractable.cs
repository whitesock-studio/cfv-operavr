using SpatialSys.UnitySDK;
using TMPro;
using UnityEngine;

namespace OperaVR
{
    public class EditorInteractable : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_debugText)
            {
                _debugText.gameObject.SetActive(_isClose);
            }
        }

        private TMP_Text _debugText;
        private float InteractiveRadius => _spatialInteractable.interactiveRadius;
        private SpatialInteractable _spatialInteractable;
        private IAvatar _avatar;
        private bool _isClose;
        
        private IAvatar Avatar
        {
            get
            {
                _avatar ??= SpatialBridge.actorService.localActor.avatar;
                return _avatar;
            }   
        }
        
        private void Awake()
        {
            _spatialInteractable = GetComponent<SpatialInteractable>();
            _debugText = GetComponentInChildren<TMP_Text>(true);
        }

        private void Update()
        {
            if (Avatar == null)
            {
                return;
            }

            if (Vector3.Distance(Avatar.position, transform.position) > InteractiveRadius)
            {
                if (!_isClose)
                {
                    return;
                }
                _isClose = false;
                Exit();
                return;
            }
            
            if (!_isClose)
            {
                _isClose = true;
                Enter();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Interact();
            }
        }

        private void Enter()
        {
            _spatialInteractable.onEnterEvent.unityEvent.Invoke();
        }
        
        private void Exit()
        {
            _spatialInteractable.onExitEvent.unityEvent.Invoke();
        }
        
        private void Interact()
        {
            _spatialInteractable.onInteractEvent.unityEvent.Invoke();
        }

#endif
    }
}
