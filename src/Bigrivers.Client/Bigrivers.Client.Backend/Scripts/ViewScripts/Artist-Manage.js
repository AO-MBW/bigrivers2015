$(function () {
    var popovers = $(".popover-div");

    $.each(popovers, function() {
        $(this).hide();
    });
    $(".open-popover").click(OpenPopOver);
});

var OpenPopOver = function () {
    console.log("Hi!");
    $(".popover-div").data("id", $(this).data("id")).toggle();
};