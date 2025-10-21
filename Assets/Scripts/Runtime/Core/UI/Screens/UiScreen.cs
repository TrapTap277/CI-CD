using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public abstract class UiScreen : MonoBehaviour
    {
        [SerializeField] protected float _fadeDuration = 0.25f;
        [SerializeField] protected string _id;
        [SerializeField] private Image _fadeImage;

        private UniTaskCompletionSource _showCompletionSource;
        private UniTaskCompletionSource _hideCompletionSource;
        private Tween _showFadeTween;
        private Tween _hideFadeTween;

        public string Id => _id;

        public virtual async UniTask ShowAsync(CancellationToken cancellationToken = default)
        {
            _showCompletionSource = new UniTaskCompletionSource();
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 1);
            _fadeImage.gameObject.SetActive(true);
            gameObject.SetActive(true);

            _showFadeTween = _fadeImage.DOFade(0, _fadeDuration)
                .OnComplete(() => { _showCompletionSource?.TrySetResult(); })
                .From(0)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            cancellationToken.Register(() =>
            {
                _showFadeTween?.Kill();
                _showCompletionSource?.TrySetCanceled();
            });

            await _showCompletionSource.Task;
        }

        public virtual void ShowImmediately()
        {
            _fadeImage.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public virtual async UniTask HideAsync(bool destroy, CancellationToken cancellationToken = default)
        {
            _hideCompletionSource = new UniTaskCompletionSource();

            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 0);
            _fadeImage.gameObject.SetActive(true);

            _hideFadeTween = _fadeImage.DOFade(1, _fadeDuration)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
                {
                    _hideCompletionSource?.TrySetResult();
                    if(destroy)
                        DestroyScreen();
                    else
                        gameObject.SetActive(false);
                });

            cancellationToken.Register(() =>
            {
                _hideFadeTween?.Kill();
                _hideCompletionSource?.TrySetCanceled();
            });

            await _hideCompletionSource.Task;
        }

        public virtual void HideImmediately(bool destroy)
        {
            if (destroy)
            {
                DestroyScreen();
                return;
            }

            gameObject.SetActive(false);
        }

        public void DestroyScreen()
        {
            Destroy(gameObject);
        }
    }
}