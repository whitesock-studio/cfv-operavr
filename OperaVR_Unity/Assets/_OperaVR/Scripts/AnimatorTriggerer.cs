using System.Globalization;
using UnityEngine;

namespace OperaVR
{
    public class AnimatorTriggerer : MonoBehaviour
    {
        private Transform[] _children;
        
        private void Awake()
        {
            _children = GetComponentsInChildren<Transform>(true);
        }

        public void SetActive(string nameAndState)
        {
           var parts = nameAndState.Split(':');

           foreach (var child in _children)
           {
               if (child.name != parts[0])
               {
                   continue;
               }

               switch (parts[1])
               {
                   default:
                   case "on":
                       child.gameObject.SetActive(true);
                       break;
                   case "off":
                       child.gameObject.SetActive(false);
                       break;
               }
               return;
           }
        }

        public void SetTrigger(string nameAndKey)
        {
           var parts = nameAndKey.Split(':');

           foreach (var child in _children)
           {
               if (child.name != parts[0])
               {
                   continue;
               }

               if (!child.gameObject.TryGetComponent<Animator>(out var animator))
               {
                   continue;
               }
               
               animator.SetTrigger(parts[1]);
               return;
           }
        }
        
        public void SetSpeed(string nameAndKey)
        {
            var parts = nameAndKey.Split(':');

            foreach (var child in _children)
            {
                if (child.name != parts[0])
                {
                    continue;
                }

                if (!child.gameObject.TryGetComponent<Animator>(out var animator))
                {
                    continue;
                }
               
                animator.SetFloat("Speed", float.Parse(parts[1], 
                    NumberStyles.Float, CultureInfo.InvariantCulture));
                return;
            }
        }
    }
}
