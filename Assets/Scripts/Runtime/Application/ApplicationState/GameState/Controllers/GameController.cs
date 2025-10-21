using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Application.ApplicationState.GameState.Screens;
using Runtime.Core;
using Runtime.Core.Controllers;
using Runtime.Core.Factories;
using Runtime.Core.Models;
using UnityEngine;
using VContainer.Unity;

namespace Application.GameState.Controllers
{
    public class GameController : BaseController, ITickable
    {
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly LevelModel _levelModel;
        private readonly IUiService _uiService;
        private readonly IRandomService _randomService;

        private GameScreen _gameScreen;
        private InputController _inputController;
        private Fuel _fuel;

        public GameController(GameObjectFactory gameObjectFactory,
            LevelModel levelModel,
            IUiService uiService,
            IRandomService randomService)
        {
            _gameObjectFactory = gameObjectFactory;
            _levelModel = levelModel;
            _uiService = uiService;
            _randomService = randomService;
        }

        public override async UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken).Forget();

            var ground = await _gameObjectFactory.Create<SpriteRenderer>(ConstGame.Ground);
            var groundSize = ground.size;
            
            _inputController = await _gameObjectFactory.CreateAndRegister<InputController>(ConstGame.Tank);
            _fuel = await _gameObjectFactory.Create<Fuel>(ConstGame.Fuel, GetRandomPosition((int)groundSize.x / 2, (int)groundSize.y / 2));

            _levelModel.Camera.gameObject.GetComponent<CameraFollow>().SetTarget(_inputController.transform);

            _gameScreen = _uiService.GetScreen<GameScreen>(ConstScreens.GameScreen);
            _gameScreen.UpdateDistance(GetDistanceBetweenTargetAndPlayer());
            _gameScreen.ShowImmediately();
        }

        private Vector2 GetRandomPosition(int range, int range2) =>
            _randomService.GetRangeVector(range, range2);

        public void Tick()
        {
            if(CurrentPhaseTypeId != PhaseTypeId.Running || _gameScreen == null)
                return;
            
            _gameScreen.UpdateDistance(GetDistanceBetweenTargetAndPlayer());
        }

        private string GetDistanceBetweenTargetAndPlayer() =>
            $"{(int)Vector2.Distance(_inputController.transform.position, _fuel.transform.position)}m";
    }
}