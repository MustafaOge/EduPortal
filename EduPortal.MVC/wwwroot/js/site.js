﻿//PayIsConfirm



//document.getElementById("payButton").addEventListener("click", function () {
//    var invoiceIdInput = document.getElementById('invoiceIdInput');


//    console.log("Invoice ID: " + invoiceId); // Fatura ID'sini konsola yazdır

//    if (confirm("Fatura için ödeme aldınız mı?")) {
//        var xhr = new XMLHttpRequest();
//        xhr.open("POST", "/Invoice/Pay", true);
//        xhr.setRequestHeader("Content-Type", "application/json");
//        xhr.onreadystatechange = function () {
//            if (xhr.readyState == XMLHttpRequest.DONE) {
//                if (xhr.status == 200) {
//                    window.location.href = "/Invoice/Success";
//                } else {
//                    alert("Fatura ödendi.");
//                }
//            }
//        };

//        xhr.send(invoiceIdInput); // JSON verisini gönder
//    }
//});

document.getElementById("payButton").addEventListener("click", function (event) {
 // Formun varsayılan davranışını engelle

    var invoiceIdInput = document.getElementById('invoiceIdInput');

    console.log("Invoice ID: " + invoiceIdInput); // Fatura ID'sini konsola yazdır


        if (confirm("Fatura için ödeme aldınız mı?")) {
            var xhr = new XMLHttpRequest();
            xhr.open("POST", "/Invoice/.ççPay", true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onreadystatechange = function () {
                if (xhr.readyState == XMLHttpRequest.DONE) {
                    //if (xhr.status == 200) {
                    //    window.location.href = "/Invoice/Success";
                    //} else {
                    //    alert("Fatura ödendi.");
                    //}
                }
            };

            xhr.send(invoiceIdInput); // JSON verisini gönder
    }
});
