using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class GameDestroyService : IDestroyService
    {
        public void Destroy(GameObject gameObject) =>
            Object.Destroy(gameObject);
    }
}