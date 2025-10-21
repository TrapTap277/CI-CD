using System.Threading;
using Core;
using Core.StateMachine;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Core.Models;
using UnityEngine;

namespace Application.GameStateMachine
{
    public class BootstrapState : BaseState
    {
        private readonly ISettingProvider _settingProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly LevelModel _levelModel;
        private readonly IUiService _uiService;

        public BootstrapState(StateMachine stateMachine,
            ISettingProvider settingProvider,
            IAssetProvider assetProvider,
            LevelModel levelModel,
            IUiService uiService) : base(stateMachine)
        {
            _settingProvider = settingProvider;
            _assetProvider = assetProvider;
            _levelModel = levelModel;
            _uiService = uiService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            base.Enter(cancellationToken).Forget();

            _levelModel.Camera = Camera.main;
            await _assetProvider.Initialize();
            await _settingProvider.Initialize();
            await _uiService.Initialize();
            
            GoTo<GameState>(CancellationToken.None).Forget();
        }
    }
}