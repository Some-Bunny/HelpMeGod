using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out T t))
            {
                return t;
            }
            else
            {
                return gameObject.AddComponent<T>();
            }
        }
    }
}
