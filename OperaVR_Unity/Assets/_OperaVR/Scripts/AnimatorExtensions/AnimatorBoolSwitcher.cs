using System.Collections;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorBoolSwitcher : MonoBehaviour
    {
        [SerializeField]
        private float _delay = 0f;

        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void Switch(string parameterKey)
        {
            StartCoroutine(DelayedSwitch(parameterKey));
        }

        private IEnumerator DelayedSwitch(string parameterKey)
        {
            yield return new WaitForSeconds(_delay);
            _anim.SetBool(parameterKey, !_anim.GetBool(parameterKey));
        }
    }
}
