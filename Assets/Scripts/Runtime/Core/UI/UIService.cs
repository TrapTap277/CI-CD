using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Factories;
using UnityEngine;
using VContainer;

namespace Core.UI
{
    public sealed class UiService : IUiService
    {
        private readonly Dictionary<string, UiScreen> _shownScreens = new Dictionary<string, UiScreen>();
        
        private IAssetProvider _assetProvider;
        private Dictionary<string, GameObject> _screenPrototypes;
        private Dictionary<string, GameObject> _popupPrototypes;
        private GameObjectFactory _factory;
        private UiServiceViewContainer _uiServiceViewContainer;

        public Dictionary<string, UiScreen> ShownScreens => _shownScreens;

        [Inject] 
        private void Construct(GameObjectFactory factory, IAssetProvider assetProvider)
        {
            _factory = factory;
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            GameObject container = await _assetProvider.Instantiate(ConstScreens.UiServiceViewContainer);
            _uiServiceViewContainer = container.GetComponent<UiServiceViewContainer>();

            _screenPrototypes = new Dictionary<string, GameObject>(_uiServiceViewContainer.ScreensPrefab.Count);
            foreach (var screen in _uiServiceViewContainer.ScreensPrefab)
            {
                if (!_screenPrototypes.ContainsKey(screen.Id))
                {
                    _screenPrototypes.Add(screen.Id, screen.gameObject);
                }
            }

            _popupPrototypes = new Dictionary<string, GameObject>(_uiServiceViewContainer.PopupsPrefab.Count);
            foreach (var popup in _uiServiceViewContainer.PopupsPrefab)
            {
                if (!_popupPrototypes.ContainsKey(popup.Id))
                {
                    _popupPrototypes.Add(popup.Id, popup.gameObject);
                }
            }
        }

        public bool IsScreenShowed(string id)
        {
            return TryGetShownScreen(id, out _);
        }

        public async UniTask ShowScreen(string id, CancellationToken cancellationToken = default)
        {
            if (TryGetShownScreen(id, out UiScreen screen))
            {
                await screen.ShowAsync(cancellationToken);
            }
            else
            {
                screen = CreateScreen(id);
                _shownScreens.Add(id, screen);
                await screen.ShowAsync(cancellationToken);
            }
        }

        public T GetScreen<T>(string id) where T : UiScreen
        {
            if (!TryGetShownScreen(id, out UiScreen screen))
            {
                screen = CreateScreen(id);
                _shownScreens.Add(id, screen);
                screen.HideImmediately(false);
            }

            return screen as T;
        }

        public async UniTask HideScreen(string id, bool destroy, CancellationToken cancellationToken = default)
        {
            if (TryGetShownScreen(id, out UiScreen screen))
            {
                await screen.HideAsync(destroy, cancellationToken);

                if(destroy)
                    _shownScreens.Remove(id);
            }
        }

        public void HideScreenImmediately(bool destroy, string id)
        {
            if (TryGetShownScreen(id, out UiScreen screen))
            {
                 screen.HideImmediately(destroy);

                 if(destroy)
                     _shownScreens.Remove(id);
            }
        }

        public async UniTask<BasePopup> ShowPopup(string id, BasePopupData data = null, CancellationToken cancellationToken = default)
        {
            if (_popupPrototypes.TryGetValue(id, out GameObject prototype))
            {
                var popup = _factory.Create<BasePopup>(prototype, default, default, _uiServiceViewContainer.ScreenParent);
                await popup.Show(data, cancellationToken);
                return popup;
            }

            throw new ArgumentException($"Prototype for '{id}' is not registered.");
        }

        public T GetPopup<T>(string id) where T : BasePopup
        {
            if (_popupPrototypes.TryGetValue(id, out GameObject prototype))
            {
                var popup = _factory.Create<T>(prototype, default, default, _uiServiceViewContainer.ScreenParent);
                popup.HideImmediately();
                return popup;
            }

            throw new ArgumentException($"Prototype for '{id}' is not registered.");
        }

        public void HideAllScreensImmediately(bool destroy)
        {
            foreach (var screen in _shownScreens)
                screen.Value.HideImmediately(destroy);

            _shownScreens.Clear();
        }

        public async UniTask HideAllAsyncScreens(bool destroy, CancellationToken cancellationToken = default)
        {
            List<UniTask> hiddenScreens = new List<UniTask>();
            foreach (var screen in _shownScreens)
                hiddenScreens.Add(screen.Value.HideAsync(destroy, cancellationToken));

            _shownScreens.Clear();
            await UniTask.WhenAll(hiddenScreens);
        }

        private bool TryGetShownScreen(string id, out UiScreen screen)
        {
            if (_shownScreens.TryGetValue(id, out screen))
                return true;

            return false;
        }

        private UiScreen CreateScreen(string id)
        {
            if (_screenPrototypes.TryGetValue(id, out GameObject prototype))
                return _factory.Create<UiScreen>(prototype, default, default, _uiServiceViewContainer.ScreenParent);

            throw new ArgumentException($"Prototype for '{id}' is not registered.");
        }
    }
}