$(function () {
    toggleInput();
    $("#TicketRequired").change(toggleInput);
});

var toggleInput = function() {
    if ($("#TicketRequired").is(":checked")) {
        $("#Price").attr("disabled", false);
    } else {
        $("#Price").attr("disabled", true);            
    }
}