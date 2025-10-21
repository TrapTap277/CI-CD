using System;
using UnityEngine;

namespace FPS.Scripts.Math._1_Lesson.Putting_Bitboards_Into_Practice
{
    [Serializable]
    public class TKeyValue<TKey, TValue>
    {
        [field: SerializeField] public TKey Key { get; private set; }
        [field: SerializeField] public TValue Value { get; private set; }
    }
}