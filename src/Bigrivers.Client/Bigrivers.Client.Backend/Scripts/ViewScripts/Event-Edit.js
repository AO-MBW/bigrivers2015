$(function () {
    toggleInput();
    $("#TicketRequired").change(toggleInput);
});

var toggleInput = function() {
    $("#Price").attr("disabled", !$("#TicketRequired").is(":checked"));
}

// CKEDITOR.replace("description");

$("#datepicker1").datetimepicker({
    dateFormat: "d-m-yy",
    timeFormat: "H:mm:ss"
});
$("#datepicker2").datetimepicker({
    dateFormat: "d-m-yy",
    timeFormat: "H:mm:ss"
});