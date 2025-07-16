using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class DotPagesTracker : APagesTracker
    {
        [SerializeField]
        private GameObject _pageTrackerDotPrefab;

        private List<PageTrackerDot> _pool = new();

        private void Awake()
        {
            _pool = GetComponentsInChildren<PageTrackerDot>(true).ToList();
        }

        public override void UpdateTracker(int pagesNumber, int selectedPageIndex)
        {
            var current = 0;
            for (var i = 0; i < pagesNumber; i++)
            {
                if (_pool.Count <= i)
                {
                    InstantiateNewPoolElement();
                }
                var pageTrackerDot = _pool[i];
                pageTrackerDot.gameObject.SetActive(true);
                pageTrackerDot.SetOn(i == selectedPageIndex);
                current++;
            }
            for (var i = current; i < _pool.Count; i++)
            {
                var pageTrackerDot = _pool[i];
                pageTrackerDot.gameObject.SetActive(false);
            }
        }

        private void InstantiateNewPoolElement()
        {
            var newElement = Instantiate(_pageTrackerDotPrefab, transform);
            var pageTrackerDot = newElement.GetComponent<PageTrackerDot>();
            _pool.Add(pageTrackerDot);
        }
    }
}
