using UnityEngine;

namespace Runtime.Core
{
    public interface IRandomService
    {
        public Vector2 GetRangeVector(int range1, int range2);
        public Vector2 GetRangeVector(int range);
        public int GetRange(int range1, int range2);
        public int GetRange(int range);
    }
}