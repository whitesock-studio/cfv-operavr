using SpatialSys.UnitySDK;
using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class SpaceObjectSnapper : MonoBehaviour
    {
        private void Awake()
        {
            SpatialBridge.spaceContentService.onObjectSpawned += OnObjectSpawned;
        }

        private void OnObjectSpawned(IReadOnlySpaceObject readOnlySpaceObject)
        {
            if (!isActiveAndEnabled)
                return;

            StartCoroutine(SnapObjectNextFrame(readOnlySpaceObject));
        }

        private IEnumerator SnapObjectNextFrame(IReadOnlySpaceObject readOnlySpaceObject)
        {
            yield return null;

            var spaceObject = (ISpaceObject)readOnlySpaceObject;
            spaceObject.position = transform.position;
            spaceObject.rotation = transform.rotation;
            spaceObject.scale = transform.localScale;
        }
    }
}
