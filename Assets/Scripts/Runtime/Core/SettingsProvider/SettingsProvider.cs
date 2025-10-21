using System;
using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;

namespace Application.Services
{
    public class SettingsProvider : ISettingProvider
    {
        private const string StorageConfig = nameof(StorageConfig);

        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<Type, BaseSettings> _settings = new();

        public SettingsProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var storageConfig = await _assetProvider.Load<StorageConfig>(StorageConfig);

            foreach (var config in storageConfig.BaseSettingsList)
                Set(config);
        }

        public T Get<T>() where T : BaseSettings
        {
            if(_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];

                return setting as T;
            }

            throw new Exception("No setting found");
        }

        private void Set(BaseSettings config)
        {
            if(_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}