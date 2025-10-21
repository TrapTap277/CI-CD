using UnityEngine;

namespace Runtime.Core
{
    public class SimpleRandomService : IRandomService
    {
        public int GetRange(int x, int y) =>
            Random.Range(x, y);

        public int GetRange(int value) =>
            Random.Range(-value, value);

        Vector2 IRandomService.GetRangeVector(int value) =>
            new Vector2(GetRange(value), GetRange(value));

        Vector2 IRandomService.GetRangeVector(int x, int y) =>
            new Vector2(GetRange(x), GetRange(y));

    }
}