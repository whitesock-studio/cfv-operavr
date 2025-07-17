using UnityEngine;

namespace OperaVR
{
    public class NetworkInstantiateAtStart : MonoBehaviour
    {
        public GameObject spatialPrefab;

        void Start()
        {
            if (spatialPrefab != null)
            {
                var newObject = Instantiate(spatialPrefab, transform);
            }
        }
    }
}