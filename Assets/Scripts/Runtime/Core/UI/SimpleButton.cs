using Application.Services.Audio;
using Core.Services.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Core.UI
{
    [RequireComponent(typeof(Button))]
    public class SimpleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Animation _pressAnimation;
        [SerializeField] private bool _playSoundOnPress = true;

        private IAudioService _audioService;
        public Button Button => _button;

        [Inject]
        private void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        public void SetText(string text) =>
            _buttonText.SetText(text);
        
        private void OnEnable() =>
            _button.onClick.AddListener(PlayPressAnimation);

        private void OnDisable() =>
            _button.onClick.RemoveListener(PlayPressAnimation);

        private void PlayPressAnimation()
        {
            if(_pressAnimation)
                _pressAnimation.Play();

            if(_playSoundOnPress)
                _audioService.PlaySound(ConstAudio.PressButtonSound);
        }
    }
}