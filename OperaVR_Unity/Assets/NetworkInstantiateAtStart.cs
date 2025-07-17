using UnityEngine;

public class InstantiateSpatialObject : MonoBehaviour
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