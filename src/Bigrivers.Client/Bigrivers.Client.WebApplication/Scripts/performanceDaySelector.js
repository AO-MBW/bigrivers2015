﻿$(".day-btn").click(function () {
    var btns = $(".day-btn");
    $.each(btns, function () {
        $(this).removeClass("selected").addClass("non-selected");
    });
    $(this).removeClass("non-selected").addClass("selected");

    var tables = $(".daytable");
    $.each(tables, function () {
        $(this).removeClass("selected").addClass("non-selected");
    });
    $("div#" + $(this).attr("id")).removeClass("non-selected").addClass("selected");
    console.log("#" + $(this).attr("id"));
});