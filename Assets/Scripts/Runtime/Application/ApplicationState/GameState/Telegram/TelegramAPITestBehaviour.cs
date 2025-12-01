using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TelegramAPITestBehaviour : MonoBehaviour
    {
        private const string Url = "https://t.me/your_bot?start=invoice_123";
        private const string DLLName = "__Internal";

        [SerializeField] private Button _invoiceButton;

        private void Start() =>
            _invoiceButton.onClick.AddListener(OnInvoiceButtonClicked);

        private void OnInvoiceButtonClicked() =>
            SendInvoice(Url);

        [DllImport(DLLName)]
        private static extern void SendInvoice(string url);
    }
}