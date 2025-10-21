using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Application.Services
{
    [CreateAssetMenu(menuName = "Config/StorageConfig", fileName = "StorageConfig", order = 0)]
    public class StorageConfig : BaseSettings
    {
        [field: SerializeField] public List<BaseSettings> BaseSettingsList { get; private set; }
    }
}