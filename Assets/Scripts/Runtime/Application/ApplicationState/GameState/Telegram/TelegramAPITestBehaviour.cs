using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TelegramAPITestBehaviour : MonoBehaviour
    {
        private const string Url = "";
        private const string DLLName = "__Internal";

        [SerializeField] private Button _invoiceButton;

        // private void Start() =>
        //     _invoiceButton.onClick.AddListener(OnInvoiceButtonClicked);

        // private void OnInvoiceButtonClicked() =>
        //     SendInvoice(Url, () => { Debug.Log($"invoice url: {Url}"); });

        // [DllImport(DLLName)]
        // private static extern void SendInvoice(string url, Action callback);
    }
}