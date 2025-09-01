using UnityEngine;

namespace OperaVR
{
    public class MovementTools : MonoBehaviour
    {
        public void ActivatePlayerInputs()
        {
            InputsManager.Instance.RestoreInputs();
        }

        public void DeactivatePlayerInputs()
        {
            InputsManager.Instance.BlockInputs();
        }

        public void DeactivatePlayerInputsExceptLookAndAction()
        {
            InputsManager.Instance.BlockInputsExceptLookAndActions();
        }
    }
}
