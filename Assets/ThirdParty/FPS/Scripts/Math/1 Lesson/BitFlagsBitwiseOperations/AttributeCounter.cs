using System;
using UnityEngine;

namespace FPS.Scripts.Math._1_Lesson.BitFlagsBitwiseOperations
{
    public class AttributeCounter : MonoBehaviour
    {
        private const int Magic = 16;
        private const int Intelligence = 8;
        private const int Charisma = 4;
        private const int Fly = 2;
        private const int Invisible = 1;

        private PowersTypeId _attributes = PowersTypeId.None;
        
        private static PowersTypeId NecessaryPower => PowersTypeId.All;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out CharacterController _))
            {
                _attributes = PowersTypeId.All;
                _attributes ^= PowersTypeId.Fire;
                _attributes ^= PowersTypeId.Earth;
                _attributes ^= PowersTypeId.Wind;
                _attributes ^= PowersTypeId.Dark;
                _attributes ^= PowersTypeId.All;
                
                var attribute = Convert.ToString((int)_attributes, 2).PadLeft(8, '0');
                var necessaryPower = Convert.ToString((int)NecessaryPower, 2).PadLeft(8, '0');
                Debug.Log($"{attribute}");
                Debug.Log($"{necessaryPower}");
                
                
                if((_attributes & NecessaryPower) == NecessaryPower)
                {
                    Debug.Log($"{attribute} has {NecessaryPower}, : {Convert.ToString((int)NecessaryPower, 2).PadLeft(8, '0')}");
                }
            }
        }
    }

    [Flags]
    public enum PowersTypeId
    {
        None = 0,
        Fire = 1 << 0,
        Water = 1 << 1,
        Earth = 1 << 2,
        Ice = 1 << 3,
        Wind = 1 << 4,
        Light = 1 << 5,
        Dark = 1 << 6,
        All = Fire | Water | Earth | Ice | Wind | Light | Dark
    }
}