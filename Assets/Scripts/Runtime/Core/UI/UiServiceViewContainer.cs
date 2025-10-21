using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public sealed class UiServiceViewContainer : MonoBehaviour
    {
        [SerializeField] private List<UiScreen> _screensPrefab;
        [SerializeField] private List<BasePopup> _popupsPrefab;
        [SerializeField] private Transform _screenParent;

        public List<UiScreen> ScreensPrefab => _screensPrefab;
        public List<BasePopup> PopupsPrefab => _popupsPrefab;
        public Transform ScreenParent => _screenParent;
    }
}