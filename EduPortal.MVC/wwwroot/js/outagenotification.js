
$(function () {
    loadDistricts();
    loadDates();

    $('#ilce').on('change', function () {
        loadOutages();
    });

    $('#tarih').on('change', function () {
        loadOutages();
    });
});

function loadDistricts() {
    fetch('/OutageNotification/GetDistricts')
        .then(response => response.json())
        .then(data => {
            const districtSelect = $('#ilce');
            districtSelect.empty().append('<option value="">İlçe Seçin</option>');
            data.forEach(district => {
                const option = $('<option></option>').val(district).text(district);
                districtSelect.append(option);
            });
        });
}

function loadDates() {
    fetch('/OutageNotification/GetDates')
        .then(response => response.json())
        .then(data => {
            const dateSelect = $('#tarih');
            dateSelect.empty().append('<option value="">Tarih Seçin</option>');
            data.forEach(date => {
                const option = $('<option></option>').val(date).text(date);
                dateSelect.append(option);
            });
        });
}

function loadOutages() {
    const selectedDistrict = $('#ilce').val();
    const selectedDate = $('#tarih').val();

    if (selectedDistrict && selectedDate) {
        // Burada planlı kesinti ve arıza bilgilerini yükleme işlemi yapabilirsiniz
        fetch(`/OutageNotification/GetOutages?district=${selectedDistrict}&date=${selectedDate}`)
            .then(response => response.json())
            .then(data => {
                // Örneğin, gelen verileri ekranda göstermek için uygun bir işlem yapabilirsiniz
                console.log(data); // Verileri konsola yazdırma örneği
            });
    }
}

//$(function () {
//    loadDistricts();
//    loadDates();
//    loadNeighbourhoods();
//});






//function loadDistricts() {

//        console.log("trigger district")


//    fetch('/OutageNotification/GetDistricts')
//        .then(response => response.json())
//        .then(data => {
//            const districtSelect = $('#ilce');
//            districtSelect.empty().append('<option value="">İlçe Seçin</option>');
//            data.forEach(district => {
//                const option = $('<option></option>').val(district).text(district);
//                districtSelect.append(option);
//            });
//        });
//}

//function loadNeighbourhoods() {

//    console.log("trigger district")


//    fetch('/OutageNotification/GetNeighbourhoods')
//        .then(response => response.json())
//        .then(data => {
//            const districtSelect2 = $('#mahalle');
//            districtSelect2.empty().append('<option value="">Mahalle Seçin</option>');
//            data.forEach(neighbourhood => {
//                const option = $('<option></option>').val(neighbourhood).text(neighbourhood);
//                districtSelect2.append(option);
//            });
//        });
//}


//function loadDates() {
//    fetch('/OutageNotification/GetDates')
//        .then(response => response.json())
//        .then(data => {
//            const dateSelect = $('#tarih');
//            dateSelect.empty().append('<option value="">Tarih Seçin</option>');
//            data.forEach(date => {
//                const option = $('<option></option>').val(date).text(date);
//                dateSelect.append(option);
//            });
//        });
//}

