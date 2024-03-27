$(".corporate-find").hide();
$("#corporate-box-link").click(function () {
    $(".individual-find").fadeOut(100);
    $(".corporate-find").delay(100).fadeIn(100);
    $("#individual-box-link").removeClass("active");
    $("#corporate-box-link").addClass("active");
});


$("#individual-box-link").click(function () {
    $(".individual-find").delay(100).fadeIn(100);;
    $(".corporate-find").fadeOut(100);
    $("#individual-box-link").addClass("active");
    $("#corporate-box-link").removeClass("active");
});