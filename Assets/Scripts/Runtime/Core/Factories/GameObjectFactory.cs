using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Runtime.Core.Factories
{
    public class GameObjectFactory
    {
        private const bool WorldPositionStays = false;
        private readonly IObjectResolver _objectResolver;
        private readonly IAssetProvider _assetProvider;

        public GameObjectFactory(IObjectResolver objectResolver, IAssetProvider assetProvider)
        {
            _objectResolver = objectResolver;
            _assetProvider = assetProvider;
        }
        
        public async UniTask<GameObject> Create(string address, Vector2 point = default, Quaternion rotation = default, Transform parent = null)
        {
            var instance = await _assetProvider.Load<GameObject>(address);
            var newObject = _objectResolver.Instantiate(instance, point, rotation, parent);
            newObject.transform.SetParent(parent, WorldPositionStays);
            return newObject;
        }

        public async UniTask<T> Create<T>(string address, Vector2 point = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
            var instance = await _assetProvider.Load<GameObject>(address);
            var newObject = _objectResolver.Instantiate(instance.GetComponent<T>(), point, rotation, parent);
            newObject.transform.SetParent(parent, WorldPositionStays);
            return newObject;
        }

        public T Create<T>(GameObject view, Vector2 point = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
            var newObject = _objectResolver.Instantiate(view.GetComponent<T>(), point, rotation);
            newObject.transform.SetParent(parent, WorldPositionStays);
            return newObject;
        }

        public async UniTask<T> CreateAndRegister<T>(string address, Vector2 point = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
            var component = await Create<T>(address, point, rotation, parent);
            _objectResolver.Inject(component);
            return component;
        }
    }
}