using Application.GameState.Controllers;
using Application.Services.Audio;
using Core;
using Core.Services.Audio;
using Core.StateMachine;
using Core.UI;
using Runtime.Core;
using Runtime.Core.Factories;
using Runtime.Core.Logger;
using Runtime.Core.Models;
using VContainer;
using VContainer.Unity;
using SettingsProvider = Application.Services.SettingsProvider;

namespace Application.GameStateMachine
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterStateMachine(builder);
            RegisterLogger(builder);
            RegisterFactories(builder);
            RegisterServices(builder);
            RegisterStates(builder);
            RegisterControllers(builder);
            RegisterModels(builder);
        }

        private void RegisterModels(IContainerBuilder builder)
        {
            builder.Register<LevelModel>(Lifetime.Singleton);
        }

        private void RegisterControllers(IContainerBuilder builder)
        {
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void RegisterFactories(IContainerBuilder builder) =>
            builder.Register<GameObjectFactory>(Lifetime.Scoped);

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IAssetProvider, AssetProvider>(Lifetime.Scoped);
            builder.Register<ISettingProvider, SettingsProvider>(Lifetime.Scoped);
            builder.Register<IUiService, UiService>(Lifetime.Scoped);
            builder.Register<IAudioService, AudioService>(Lifetime.Scoped);
            builder.Register<IRandomService, SimpleRandomService>(Lifetime.Scoped);
        }

        private void RegisterLogger(IContainerBuilder builder) =>
            builder.Register<ILogger, SimpleLogger>(Lifetime.Singleton);

        private void RegisterStateMachine(IContainerBuilder builder) =>
            builder.Register<StateMachine>(Lifetime.Singleton);

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<GameState>(Lifetime.Singleton);
        }

        private void RegisterEntryPoint(IContainerBuilder builder) =>
            builder.RegisterEntryPoint<Bootstrapper>();
    }
}