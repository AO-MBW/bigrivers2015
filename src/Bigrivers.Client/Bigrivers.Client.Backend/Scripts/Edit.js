$(function () {
    toggleInput();
    $("#TicketRequired").change(toggleInput);
});

var toggleInput = function() {
    $("#Price").attr("disabled", !$("#TicketRequired").is(":checked"));
}