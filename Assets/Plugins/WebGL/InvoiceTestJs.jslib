mergeInto(LibraryManager.library, {
    SendInvoice: function (urlPtr) {

        const url = UTF8ToString(urlPtr);

        console.log("JS: Opening invoice with URL:", url);

        if (typeof window.Telegram === "undefined") {
            console.error("JS ERROR: Telegram API not found.");
            return;
        }

        const tg = window.Telegram.WebApp;

        if (!tg) {
            console.error("JS ERROR: Telegram WebApp is not available.");
            return;
        }

        // Основной вызов API
        try {
            tg.openInvoice(url, function (result) {
                console.log("JS invoice result:", result);

                // При желании можно отправлять в C#
                // unityInstance.SendMessage("GameObjectName", "OnInvoiceResult", JSON.stringify(result));
            });
        }
        catch (err) {
            console.error("JS INVOICE ERROR:", err);
        }
    }
});