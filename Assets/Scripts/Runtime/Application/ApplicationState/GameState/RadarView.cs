using System.Globalization;
using Application.Services;
using Core.EventBus;
using TMPro;
using UnityEngine;

namespace Application.GameState
{
    public class RadarView : MonoBehaviour, IObjectDistanceUpdater
    {
        [SerializeField] private TextMeshProUGUI _radarText;
        
        private void Awake() =>
            EventBus.Subscribe(this);

        private void OnDestroy() =>
            EventBus.UnSubscribe(this);

        public void UpdateDistance(float distance) =>
            _radarText.SetText(distance.ToString(CultureInfo.InvariantCulture));
    }

}