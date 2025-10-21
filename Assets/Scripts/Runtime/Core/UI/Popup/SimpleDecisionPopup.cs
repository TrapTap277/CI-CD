using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class SimpleDecisionPopup : BasePopup
    {
        [SerializeField] protected SimpleButton _confirmButton;
        [SerializeField] protected SimpleButton _cancelButton;
        [SerializeField] protected TextMeshProUGUI _message;
        
        public event Action OnConfirmButtonPressedEvent;
        public event Action OnCancelButtonPressedEvent;

        public override async UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _confirmButton?.Button.onClick.AddListener(OnConfirmButtonPressed);
            _cancelButton?.Button.onClick.AddListener(OnCancelButtonPressed);

            await base.Show(data, cancellationToken);
        }

        private void OnDestroy()
        {
            _confirmButton?.Button.onClick.RemoveListener(OnConfirmButtonPressed);
            _cancelButton?.Button.onClick.RemoveListener(OnCancelButtonPressed);
        }

        public void SetMessage(string message) =>
            _message.text = message;
        
        private void OnConfirmButtonPressed() =>
            OnConfirmButtonPressedEvent?.Invoke();
        
        private void OnCancelButtonPressed() =>
            OnCancelButtonPressedEvent?.Invoke();
    }
}