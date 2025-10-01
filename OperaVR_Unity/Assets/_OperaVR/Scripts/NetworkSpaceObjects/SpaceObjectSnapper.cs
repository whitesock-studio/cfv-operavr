using SpatialSys.UnitySDK;
using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class SpaceObjectSnapper : MonoBehaviour
    {
        [SerializeField]
        private float _size = 1f;

        private void OnDrawGizmos()
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            
            var color = Color.blue;
            color.a = .4f;
            Gizmos.color = color;
            Gizmos.DrawCube(Vector3.zero, new Vector3(_size, _size, .1f));
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_size, _size, .1f));
        }

        private void OnEnable()
        {
            SpatialBridge.spaceContentService.onObjectSpawned += OnObjectSpawned;
        }

        private void OnDisable()
        {
            SpatialBridge.spaceContentService.onObjectSpawned -= OnObjectSpawned;
        }

        private void OnObjectSpawned(IReadOnlySpaceObject readOnlySpaceObject)
        {
            //TryDespawnPreviousObject();

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
            spaceObject.scale = new Vector3(_size, _size, 1);
        }

        private void TryDespawnPreviousObject()
        {
            foreach (var spaceObject in SpatialBridge.spaceContentService.allObjects.Values)
            {
                if (Vector3.Distance(transform.position, spaceObject.position) > .1f)
                {
                    continue;
                }
                SpatialBridge.spaceContentService.DestroySpaceObject(spaceObject.objectID);
            }
        }
    }
}
