using System.Collections;
using System.Globalization;
using UnityEngine;

namespace OperaVR
{
    public class PathTriggerer : MonoBehaviour
    {
        private Transform[] _children;
        
        private void Awake()
        {
            _children = GetComponentsInChildren<Transform>(true);
        }

        public void StartPath(string objectName)
        {
           foreach (var child in _children)
           {
               if (child.name != objectName)
               {
                   continue;
               }

               var pathStarter = child.gameObject.GetComponent<CharacterPathStarter>();
               if (pathStarter == null)
               {
                   continue;
               }
               pathStarter.StartPath();
               return;
           }
        }
    }
}
