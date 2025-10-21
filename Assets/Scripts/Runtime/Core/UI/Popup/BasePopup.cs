using System;
using System.Threading;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Core.UI
{
    public class BasePopup : MonoBehaviour
    {
        private const string OpenPopupSound = "OpenPopup";
        private const string ClosePopupSound = "ClosePopupSound";

        private IAudioService _audioService;
        private bool _isSoundEnable = true;
        private UniTaskCompletionSource _completionSource;

        [SerializeField] protected string _id;

        public UnityEvent ShowEvent;
        public UnityEvent HideEvent;
        public UnityEvent HideImmediatelyEvent;

        public event Action DestroyPopupEvent;

        public string Id => _id;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public virtual UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _completionSource?.TrySetCanceled();

            _completionSource = new UniTaskCompletionSource();
            cancellationToken.Register(() =>
            {
                _completionSource.TrySetCanceled();
            });

            TryPlaySound(OpenPopupSound);
            ShowEvent?.Invoke();
            return UniTask.CompletedTask;
        }

        public virtual void Hide()
        {
            _completionSource?.TrySetCanceled();

            HideEvent?.Invoke();
        }

        public virtual void HideImmediately()
        {
            _completionSource?.TrySetCanceled();

            HideImmediatelyEvent?.Invoke();
        }

        public virtual void DestroyPopup()
        {
            _completionSource?.TrySetCanceled();

            DestroyPopupEvent?.Invoke();
            TryPlaySound(ClosePopupSound);
            Destroy(gameObject);
        }

        public void EnableSound(bool enable)
        {
            _isSoundEnable = enable;
        }

        protected void TryPlaySound(string soundName)
        {
            if(_isSoundEnable)
                _audioService.PlaySound(soundName);
        }
    }
}