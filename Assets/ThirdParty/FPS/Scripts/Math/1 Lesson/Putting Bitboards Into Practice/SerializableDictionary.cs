using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FPS.Scripts.Math._1_Lesson.Putting_Bitboards_Into_Practice
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private List<TKeyValue<TKey, TValue>> _dictionary;
        
        public Dictionary<TKey, TValue> Dictionary => _dictionary.ToDictionary(x => x.Key, x => x.Value);
        
        public TValue this[TKey key] => Dictionary[key];
    }
}