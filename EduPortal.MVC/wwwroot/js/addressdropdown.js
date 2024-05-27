$(document).ready(function () {
    var addressData = null;

    // İlçe, mahalle, sokak ve kapı verilerini alacak fonksiyon
    function getAddressData() {
        console.log("trtiklrndiii ... ");
        $.ajax({
            type: 'GET',
            url: '/Subscriber/GetAddressData',
            dataType: 'json',
            success: function (data) {
                addressData = data;

                // Adres verileri alındıktan sonra dropdown listelerini doldur
                fillDropdowns();
            },
            error: function (xhr, status, error) {
                console.error("Adres verileri alınırken bir hata oluştu: " + error);
            }
        });
    }

    // Adres verilerini işleyecek ve dropdown listelerini dolduracak fonksiyon
    function fillDropdowns() {
        if (!addressData) return; // addressData tanımlı değilse işlem yapma

        // İlçe dropdown'ını doldur
        var ilceDropdown = $('#ilce');
        ilceDropdown.empty();
        $.each(addressData[0].ilceler, function (key, value) {
            ilceDropdown.append('<option value="' + value.ilceKimlikNo + '">' + value.adi + '</option>');
        });

        // addressData.ilceler içeriğini konsola yazdır
        console.log("Address Data İlçeler:", addressData[0].ilceler);

        // İlçe seçildiğinde diğer dropdown listelerini doldur
        ilceDropdown.change(function () {
            var ilceId = $(this).val();
            var mahalleler = addressData[0].mahalleler.filter(m => m.ilceKimlikNo == ilceId);
            $('#mahalle').empty().append('<option value="">Seçiniz</option>'); // Boş bir seçenek ekleyerek başlangıç durumunu belirle
            $.each(mahalleler, function (key, value) {
                $('#mahalle').append('<option value="' + value.mahalleKimlikNo + '">' + value.adi + '</option>');
            });
        });
        $('#mahalle').change(function () {
            var mahalleId = $(this).val();
            var sokaklar = addressData[0].sokaklar.filter(s => s.mahalleKimlikNo == mahalleId);
            $('#sokak').empty().append('<option value="">Seçiniz</option>'); // Boş bir seçenek ekleyerek başlangıç durumunu belirle
            $.each(sokaklar, function (key, value) {
                $('#sokak').append('<option value="' + value.sokakKimlikNo + '">' + value.adi + '</option>');
            });
        });
        $('#sokak').change(function () {
            var sokakId = $(this).val();
            var disKapilar = addressData[0].disKapilar.filter(d => d.sokakKimlikNo == sokakId);
            $('#dis_kapi').empty().append('<option value="">Seçiniz</option>'); // Boş bir seçenek ekleyerek başlangıç durumunu belirle
            $.each(disKapilar, function (key, value) {
                $('#dis_kapi').append('<option value="' + value.disKapiKimlikNo + '">' + value.adi + '</option>');
            });
        });
        $('#dis_kapi').change(function () {
            var disKapiId = $(this).val();
            var icKapilar = addressData[0].icKapilar.filter(i => i.disKapiKimlikNo == disKapiId);
            $('#ic_kapi').empty().append('<option value="">Seçiniz</option>'); // Boş bir seçenek ekleyerek başlangıç durumunu belirle
            $.each(icKapilar, function (key, value) {
                $('#ic_kapi').append('<option value="' + value.icKapiKimlikNo + '">' + value.icKapiNo + '</option>');
            });
        });


    }

    // İlk başta adres verilerini al
    getAddressData();

    // İç kapılar dropdown'una olay dinleyicisi ekleme
    //$(document).on('change', '#ic_kapi', function () {
    //    if (!addressData) return; // addressData tanımlı değilse işlem yapma
    //    var icKapiId = $(this).val();

    //    // İç kapılarla ilgili işlemleri buraya ekleyebilirsiniz
    //});
    $(document).on('change', '#ic_kapi', function () {
        if (!addressData) return;

        var icKapiId = $(this).val();

        // İç kapı seçimi değiştiğinde yapılacak işlemleri içeren bir obje oluştur
        var postData = {
            icKapiKimlikNo: icKapiId
        };



        // AJAX isteği gönder
        $.ajax({
            type: 'POST',
            url: '/Subscriber/GetAddressData', // Controller ve action'ı uygun şekilde belirtin
            data: postData,
            dataType: 'json', // Dönüş tipi olarak json belirtiyoruz
            success: function (response) {
                // İstek başarılıysa gelen değeri kullanabilirsiniz
                console.log("Seçilen iç kapı kimlik numarası: " + response.icKapiKimlikNo);
            },
            error: function (xhr, status, error) {
                console.error("İç kapıyla ilgili işlemler gerçekleştirilirken bir hata oluştu: " + error);
            }
        });
    });
    // Dropdown menüsündeki değişiklikleri dinle
    $(document).on('change', '#ic_kapi', function () {
        // Seçilen iç kapı numarasını al
        var selectedInternalDoorNumber = $(this).val();

        // İç kapı numarasını bir form alanına ata
        $('#internalDoorNumber').val(selectedInternalDoorNumber);
    });




});
