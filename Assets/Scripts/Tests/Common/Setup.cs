using NSubstitute;
using Unity.FPS.Gameplay;
using UnityEngine;

namespace Tests.Common
{
    public class Setup
    {
        public static HealthPickup HealthPickup()
        {
            var healthPickup = Create.HealthPickup();
            var destroyMock = Create.DestroyMock();
            destroyMock
                .WhenForAnyArgs(x => x.Destroy(null))
                .Do(info => Object.DestroyImmediate(info.Arg<GameObject>()));
        
            healthPickup.DestroyService = destroyMock;
            Create.AudioManager();
        
            return healthPickup;
        }
    }
}