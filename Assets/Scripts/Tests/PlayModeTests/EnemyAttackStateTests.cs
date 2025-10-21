using System.Collections;
using FluentAssertions;
using Tests.Common;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

namespace Tests.PlayModeTests
{
    public class EnemyAttackStateTests
    {
        private const int Threshold = 5;
        private const int EnemyCount = 1;
        private const float TimeToAssert = 10;
        private const float HardcodedTimeScale = 10;
        private const string MainScene = "MainScene";

        [UnityTest]
        public IEnumerator WhenThereIsAtLeastOneEnemyOnTheMap_AndPlayerIsInRangeOf5Meters_ThenEnemyShouldAttackThePlayer()
        {
            // Arrange.

            yield return SceneManager.LoadSceneAsync(MainScene);

            var playerHealth = GetPlayerHealth();
            
            // Act.

            CreateEnemies(playerHealth);
            SpeedUpTime();
            
            yield return new WaitForSeconds(TimeToAssert);

            // Assert.
            
            playerHealth.CurrentHealth
                .Should()
                .BeLessThan(playerHealth.MaxHealth);
        }

        private static void SpeedUpTime() =>
            Time.timeScale = HardcodedTimeScale;

        private void CreateEnemies(Health playerHealth)
        {
            for (var i = 0; i < EnemyCount; i++)
                Create.Enemy(GetPositionInFrontOfPlayer(playerHealth.transform.position));
        }

        private Vector3 GetPositionInFrontOfPlayer(Vector3 playerPosition) =>
            new(GetPositionWithThreshold(playerPosition.x), playerPosition.y, GetPositionWithThreshold(playerPosition.z));

        private static Health GetPlayerHealth() =>
            Object.FindFirstObjectByType<PlayerCharacterController>().GetComponent<Health>();

        private static float GetPositionWithThreshold(float position) =>
            Random.Range(position, position + Threshold);
    }
}
