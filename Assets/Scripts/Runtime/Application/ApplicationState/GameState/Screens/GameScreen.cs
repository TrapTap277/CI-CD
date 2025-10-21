using Core.UI;
using TMPro;
using UnityEngine;

namespace Runtime.Application.ApplicationState.GameState.Screens
{
    public class GameScreen : UiScreen
    {
        [SerializeField] private TextMeshProUGUI _radarDistanceText;

        public void UpdateDistance(string distance) =>
            _radarDistanceText.SetText(distance);
    }
}