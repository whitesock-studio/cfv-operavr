using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    public class Codelock : MonoBehaviour
    {
        public Action OnUnlock;

        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private CodelockSlot[] _slots;

        private string[] _correctCodes;

        private void Awake()
        {
            _inputField.onValueChanged.AddListener(ChangedInput);
            _inputField.onSelect.AddListener(InputFieldSelect);
            _inputField.onEndEdit.AddListener(InputFieldDeselect);
        
            foreach (var slot in _slots)
            {
                slot.Reset();
            }
        }

        public void LoadCodes(string[] codes)
        {
            _inputField.text = "";
            _inputField.Select();
            _correctCodes = codes;
            ChangedInput("");
        }

        private void TryUnlock(string text)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (_correctCodes.Contains(text))
            {
                StartCoroutine(Unlock());
                return;
            }
            foreach (var slot in _slots)
            {
                slot.PulseOutcome(false);
            }
        }

        private void InputFieldSelect(string text)
        {
            _inputField.text = "";
            ChangedInput("");
        }

        private void InputFieldDeselect(string text)
        {
            foreach (var slot in _slots)
            {
                slot.SetIsEditing(false);
            }
        }

        private void ChangedInput(string currentText)
        {
            for (var i = 0; i < _slots.Length; i++)
            {
                var slot = _slots[i];
                slot.SetIsEditing(
                    EventSystem.current.currentSelectedGameObject == _inputField.gameObject && 
                    i == currentText.Length);
                if (currentText.Length > i)
                {
                    slot.SetText(currentText[i].ToString());
                    continue;
                }
                slot.Reset();
            }

            if (currentText.Length == _slots.Length)
            {
                TryUnlock(currentText);
            }
        }

        private IEnumerator Unlock()
        {
            foreach (var slot in _slots)
            {
                slot.PulseOutcome(true);
            }
            yield return new WaitForSeconds(1f);
            OnUnlock?.Invoke();
        }
    }
}
