//PayIsConfirm



document.getElementById("payButton").addEventListener("click", function () {
    var invoiceId = parseInt(this.getAttribute('data-invoice-id')); // Fatura ID'sini al ve integer'a dönüştür

    console.log("Invoice ID: " + invoiceId); // Fatura ID'sini konsola yazdır

    if (confirm("Fatura için ödeme aldınız mı?")) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/Invoice/Pay", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState == XMLHttpRequest.DONE) {
                if (xhr.status == 200) {
                    window.location.href = "/Invoice/Success";
                } else {
                    alert("Fatura ödendi.");
                }
            }
        };
    
        var data = JSON.stringify({ id: invoiceId }); // Fatura ID'sini JSON olarak dönüştür
        xhr.send(invoiceId); // JSON verisini gönder
    }
});
