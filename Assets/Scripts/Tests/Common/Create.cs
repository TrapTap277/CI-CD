using NSubstitute;
using Unity.FPS.AI;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEditor;
using UnityEngine;
using static Application.Constants.PathConstants;

namespace Tests.Common
{
    public class Create
    {
        public static PlayerCharacterController Player() =>
            Object.Instantiate(AssetDatabase.LoadAssetAtPath<PlayerCharacterController>(PlayerPath));

        public static HealthPickup HealthPickup() =>
            Object.Instantiate(AssetDatabase.LoadAssetAtPath<HealthPickup>(HealBoost));

        public static AudioManager AudioManager() =>
            Object.Instantiate(AssetDatabase.LoadAssetAtPath<AudioManager>(AudioManagerPath));

        public static IDestroyService DestroyMock() =>
            Substitute.For<IDestroyService>();

        public static void Enemy(Vector3 positionInFrontOfPlayer)
        {
            var enemyReference = AssetDatabase.LoadAssetAtPath<EnemyController>(EnemyPath);
            Object.Instantiate(enemyReference, positionInFrontOfPlayer, Quaternion.identity);
        }
    }
}