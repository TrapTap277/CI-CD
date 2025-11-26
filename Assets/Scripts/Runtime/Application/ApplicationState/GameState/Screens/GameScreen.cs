using Core.UI;
using TMPro;

namespace Runtime.Application.ApplicationState.GameState.Screens
{
    public class GameScreen : UiScreen
    {
        private TextMeshProUGUI _radarDistanceText;

        public void UpdateDistance(string distance) =>
            _radarDistanceText.SetText(distance);
    }
}