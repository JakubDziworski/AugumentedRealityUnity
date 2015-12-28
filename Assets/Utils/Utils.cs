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
            Where(found => obj != found && found.gameObject.activeInHierarchy).
            OrderBy(found => Vector3.Distance(found.transform.position, obj.transform.position)).
            FirstOrDefault(found => Vector3.Distance(found.transform.position, obj.transform.position) < maxDistance);
        }

         static public GameObject GetChildGameObject(GameObject fromGameObject, string withName) {
         //Author: Isaac Dart, June-13.
         Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
         foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
         return null;
     }
    }
}
