$(function () {
    
    $("#datepicker1").datetimepicker({
        dateFormat: "d-m-yy",
        timeFormat: "h:mm:s"
    });
    $("#datepicker2").datetimepicker({
        dateFormat: "d-m-yy",
        timeFormat: "H:mm:ss"
    });

    var selects = $("select");
    // Set default item in selectlist
    $.each(selects, function () {
        var selected = $(this).data("selected");

        if (typeof (selected) != "undefined") {
            $(this).val(selected);
        }
    });
});