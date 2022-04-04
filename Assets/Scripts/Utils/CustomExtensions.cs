using System.Collections.Generic;
using UnityEngine;

namespace CustomExtensions
{
    public static class ListExtension
    {
        public static T RandomElement<T>(this List<T> list)
        {
            int index = Random.Range(0, list.Count);
            return list[index];
        }
    }
}