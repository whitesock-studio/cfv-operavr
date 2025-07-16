using UnityEngine;

namespace OperaVR
{
    public abstract class APagesTracker : MonoBehaviour
    {
        public abstract void UpdateTracker(int pagesNumber, int selectedPageIndex);
    }
}
