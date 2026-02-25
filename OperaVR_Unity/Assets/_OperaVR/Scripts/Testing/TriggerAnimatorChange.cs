using UnityEngine;

namespace OperaVR
{
    public class TriggerAnimatorChange : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        
        public void SetRandomFloat()
        {
            _animator.SetFloat("Float", Random.Range(-1f, 1f));
        }
        
        public void SetRandomInt()
        {
            _animator.SetInteger("Int", Random.Range(0, 2));
        }
        
        public void SetRandomBool()
        {
            _animator.SetBool("Bool", !_animator.GetBool("Bool"));
        }
    }
}
