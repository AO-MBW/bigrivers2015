$(".day-btn").click(function () {
    var btns = $(".day-btn");
    $.each(btns, function () {
        $(this).removeClass("selected").addClass("non-selected");
    });
    $(this).removeClass("non-selected").addClass("selected");

    var tables = $(".day-table");
    $.each(tables, function () {
        $(this).removeClass("selected").addClass("non-selected");
    });
    $("table#" + $(this).attr("id")).removeClass("non-selected").addClass("selected");
    console.log("#" + $(this).attr("id"));
});