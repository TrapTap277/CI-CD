using UnityEngine;

namespace Core.Extensions
{
    public static class Vector3Extensions
    {
        public static void SetX(this Transform transform, float x)
        {
            var vector3 = transform.position;
            vector3.x = x;
            transform.position = vector3;
        }

        public static void SetY(this Transform transform, float y)
        {
            var vector3 = transform.position;
            vector3.y = y;
            transform.position = vector3;
        }
    }
}