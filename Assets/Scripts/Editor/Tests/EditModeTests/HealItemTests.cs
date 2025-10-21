using Editor.Tests.Common;
using FluentAssertions;
using NUnit.Framework;
using Unity.FPS.Game;

namespace Editor.Tests.EditModeTests
{
    public class HealItemTests
    {
        [Test]
        public void WhenPlayerCloseEnoughToHealItem_ThenHeShouldRestoreAtLeast10PercentOfHealth()
        {
            // Arrange.
        
            var player = Create.Player();
            var healthPickup = Setup.HealthPickup();
            var healthComponent = player.GetComponent<Health>();
            healthComponent.CurrentHealth = healthComponent.MaxHealth / 2;
            var healthBeforeRestore = healthComponent.CurrentHealth;
            
            // Act.
            
            healthPickup.OnPicked(player);
        
            // Assert.
            
            healthComponent.CurrentHealth
                .Should()
                .BeInRange(OfTenPercentMore(healthBeforeRestore), healthComponent.MaxHealth);
        }

        private static float OfTenPercentMore(float healthBeforeRestore) =>
            healthBeforeRestore * 1.1f;
    }
}