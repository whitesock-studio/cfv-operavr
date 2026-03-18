using UnityEngine;

namespace OperaVR
{
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        
        private NPCCharacter _character;
        public NPCCharacter Character => _character;
        
        public bool HasReachedDestination => _character.HasReachedDestination;
        
        public bool HasCharacter => _character != null;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(.3f, 2f, .3f));
        }

        private void Awake()
        {
            var characters = GetComponentsInChildren<NPCCharacter>(true);
            for (var i = characters.Length - 1; i >= 0; i--)
            {
                var character = characters[i];
                if (_character == null && character.gameObject.activeSelf)
                {
                    _character = character;
                    continue;
                }
                Destroy(character.gameObject);
            }

            _character.transform.SetParent(null);
            _character.SetName(_name);
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
        
        public void SetSpeeds(float runningSpeed, float walkingSpeed)
        {
            _character.Speed = runningSpeed;
            _character.Speed = walkingSpeed;
        }

        public void SetDestination(Vector3 destination)
        {
            _character.SetDestination(destination);
        }
    }
}
