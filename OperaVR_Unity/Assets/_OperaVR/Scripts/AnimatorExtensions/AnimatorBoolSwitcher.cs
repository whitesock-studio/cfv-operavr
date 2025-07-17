using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorBoolSwitcher : MonoBehaviour
    {
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void Switch(string parameterKey)
        {
            _anim.SetBool(parameterKey, !_anim.GetBool(parameterKey));
        }
    }
}
