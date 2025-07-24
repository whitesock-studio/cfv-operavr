using System.Collections.Generic;
using UnityEngine;

namespace OperaVR
{
    public class PopupsManager : MonoBehaviour
    {
        public static PopupsManager Instance;

        [SerializeField]
        private Transform _popupsContainer;

        [SerializeField]
        private PopupTypesPairing[] _pairings;

        private Dictionary<PopupType, List<APopup>> _popupsPool = new();

        private List<APopup> _openPopups = new();

        private void Awake()
        {
            //Singleton Pattern
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            _popupsPool = new();
        }

        public APopup OpenPopup<T>(T data) where T : APopupData
        {
            if (!TryGetPairingFromType(data.PopupType, out var pairing))
            {
                return null;
            }

            if (!TryGetFreePopupInPool(data.PopupType, out var popup))
            {
                var newPopupObject = Instantiate(pairing.Prefab, _popupsContainer);
                popup = newPopupObject.GetComponent<APopup>();
                popup.Data = data;
                InsertInPool(popup);
            }

            SetPopupEnabled(popup, true);
            popup.Open(data);

            AddPopupToOpenPopups(popup);

            return popup;
        }

        public void ClosePopup(APopup popup)
        {
            popup.Close();

            RemovePopupFromOpenPopups(popup);

            if (!TryGetPairingFromType(popup.Data.PopupType, out var pairing))
            {
                return;
            }
            SetPopupEnabled(popup, false);
        }

        private void AddPopupToOpenPopups(APopup popup)
        {
            _openPopups ??= new ();
            if (!_openPopups.Contains(popup))
            {
                _openPopups.Add(popup);
            }
            if (popup.Data.InterruptsMovement)
            {
                SetCharacterMovementsEnabled(false);
            }
        }

        private void RemovePopupFromOpenPopups(APopup popup)
        {
            _openPopups ??= new();
            if (_openPopups.Contains(popup))
            {
                _openPopups.Remove(popup);
            }
            foreach (var pop in _openPopups)
            {
                if (pop.Data.InterruptsMovement)
                {
                    SetCharacterMovementsEnabled(false);
                    return;
                }
            }
            SetCharacterMovementsEnabled(true);
        }

        private void SetCharacterMovementsEnabled(bool value)
        {
            if (value)
            {
                InputsManager.Instance.RestoreInputs();
                return;
            }
            InputsManager.Instance.BlockInputs();
        }

        private bool TryGetFreePopupInPool(PopupType type, out APopup popup)
        {
            popup = null;
            if (!_popupsPool.ContainsKey(type))
            {
                return false;
            }
            foreach (var popupInPool in _popupsPool[type])
            {
                if (!IsPopupEnabled(popupInPool))
                {
                    popup = popupInPool;
                    return true;
                }
            }
            return false;
        }

        private void InsertInPool(APopup popup)
        {
            var type = popup.Data.PopupType;
            if (!_popupsPool.ContainsKey(type))
            {
                _popupsPool.Add(type, new List<APopup> { popup });
                return;
            }
            if (!_popupsPool[type].Contains(popup))
            {
                _popupsPool[type].Add(popup);
            }
        }

        private void SetPopupEnabled(APopup popup, bool value)
        {
            popup.gameObject.SetActive(value);
        }

        private bool IsPopupEnabled(APopup popup)
        {
            return popup.gameObject.activeSelf;
        }

        private bool TryGetPairingFromType(PopupType type, out PopupTypesPairing pairing)
        {
            foreach (var pair in _pairings)
            {
                if (type == pair.Type)
                {
                    pairing = pair;
                    return true;
                }
            }
            pairing = null;
            return false;
        }

        [System.Serializable]
        public class PopupTypesPairing
        {
            public PopupType Type;
            public GameObject Prefab;
        }
    }

}