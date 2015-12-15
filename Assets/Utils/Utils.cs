using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Utils
{
    class Utils
    {
        
        public static T GetClosestObject<T>(T obj,float maxDistance = float.PositiveInfinity) where T : MonoBehaviour
        {
            return Object.FindObjectsOfType<T>().
            Where(puppet => obj != puppet && puppet.gameObject.activeInHierarchy).
            OrderBy(puppet => Vector3.Distance(puppet.transform.position, obj.transform.position)).
            FirstOrDefault(cube => Vector3.Distance(cube.transform.position, obj.transform.position) < maxDistance);
        }
    }
}
