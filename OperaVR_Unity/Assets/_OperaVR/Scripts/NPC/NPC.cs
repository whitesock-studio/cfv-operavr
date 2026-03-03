using SpatialSys.UnitySDK;
using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        
        [SerializeField]
        private GameObject _prefab;

        [SerializeField, Range(0, 359f)]
        private float _startingAngle;

        private NPCCharacter _character;
        public NPCCharacter Character => _character;
        
        public bool HasReachedDestination => _character.HasReachedDestination;
        
        public bool HasCharacter => _character != null;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(.3f, 2f, .3f));
            var forwardPoint = Quaternion.AngleAxis(_startingAngle, Vector3.up) * Vector3.forward;
            var lookingPoint = transform.position + forwardPoint;
            Gizmos.DrawLine(transform.position, lookingPoint);
            Gizmos.DrawWireCube(lookingPoint + Vector3.up * .3f, new Vector3(.3f, .6f, .3f));
        }

        private IEnumerator Start()
        {
            while (!SpatialBridge.actorService.localActor.avatar.isBodyLoaded)
            {
                yield return null;
            }

            yield return new WaitForSeconds(.5f);
            
            var npcObject = Instantiate(_prefab, transform.position, transform.rotation);
            _character = npcObject.GetComponent<NPCCharacter>();
            InitCharacter();
        }

        private void FixedUpdate()
        {
            if (!HasCharacter)
            {
                return;
            }
            transform.position = _character.transform.position;
            transform.rotation = _character.transform.rotation;
        }

        public void Sit(bool isSit)
        {
            if (isSit)
            {
                StartCoroutine(DelayedSit());       
                return;
            }
            _character.SetEmote(0);
        }
        
        private IEnumerator DelayedSit()
        {
            yield return new WaitForSeconds(1);
            _character.SetEmote(1);      
        }
        
        public void SetSpeeds(float runningSpeed, float walkingSpeed)
        {
            _character.Speed = runningSpeed;
            _character.Speed = walkingSpeed;
        }

        public void SetDestination(Vector3 destination)
        {
            _character.SetDestination(destination);
        }
        
        private void InitCharacter()
        {
            if (_character == null)
            {
                return;
            }
            
            var forwardPoint = Quaternion.AngleAxis(_startingAngle, Vector3.up) * Vector3.forward * .15f;
            var lookingPoint = transform.position + forwardPoint;
            _character.SetDestination(lookingPoint);
            _character.SetName(_name);
        }
    }
}
