using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Lanotalium.Plugin.Simple.Wrapper
{
    public static class ExternalBehaviour<T> where T : MonoBehaviour, new()
    {
        public static T Get(string objectName, out bool created)
        {
            GameObject i;
            if ((i = GameObject.Find(objectName)) == null)
            {
                i = new GameObject();
                i.name = objectName;

                created = true;
                return i.AddComponent<T>();
            }
            else
            {
                created = false;
                return i.GetComponent<T>();
            }
        }

        public static T Get(string objectName, GameObject prefab, GameObject parent, out bool created)
        {
            GameObject i;
            if ((i = GameObject.Find(objectName)) == null)
            {
                i = GameObject.Instantiate(prefab, parent.transform);
                i.name = objectName;

                created = true;
                return i.AddComponent<T>();
            }
            else
            {
                created = false;
                return i.GetComponent<T>();
            }
        }
    }
}
