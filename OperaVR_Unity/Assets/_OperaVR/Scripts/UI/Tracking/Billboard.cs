using SpatialSys.UnitySDK;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private bool _canIncline = false;

    private void Update()
    {
        var direction = transform.position - SpatialBridge.cameraService.position;
        direction.y = _canIncline ? direction.y : 0;
        direction.Normalize();

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
