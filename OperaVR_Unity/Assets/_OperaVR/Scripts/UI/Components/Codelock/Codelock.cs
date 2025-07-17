using TMPro;
using UnityEngine;

namespace OperaVR
{
    public class Codelock : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _inputField;

        private CodelockSlot[] _slots;

        private void Awake()
        {
            _inputField.onValueChanged.AddListener(ChangedInput);
            _slots = GetComponentsInChildren<CodelockSlot>(true);
        
            foreach (var slot in _slots)
            {
                slot.Reset();
            }
        }

        private void ChangedInput(string currentText)
        {

        }
    }
}
