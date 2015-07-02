$(function () {
    var firstAmount = $(".performance").first().attr("startTime");
    document.getElementById('slider').value = firstAmount;
    showAmount(firstAmount);
})
function showAmount(newAmount) {
    var sliderAmount = newAmount;
    $(".performance").each(function () {
        if (parseInt($(this).attr("startTime")) <= sliderAmount && parseInt($(this).attr("endTime")) > sliderAmount) {
            $(this).removeClass("hidden").addClass("show");
        }
        else if (parseInt($(this).prev().attr("startTime")) <= sliderAmount && parseInt($(this).prev().attr("endTime")) > sliderAmount) {
            $(this).removeClass("hidden").addClass("show");
        }
        else {
            $(this).removeClass("show").addClass("hidden");
        }
    });
    // Parsed Hours
    var parsedHour = Math.floor(sliderAmount / 60);
    // Parsed Minutes
    var parsedMinute = Math.round(((sliderAmount / 60) - parsedHour) * 60);

    // To account for midnight times
    if (parsedHour < 18) { parsedHour = parsedHour + 6; }
    else if (parsedHour >= 18) { parsedHour = parsedHour - 18; }

    // Adds extra 0 for single digit numbers
    if (parsedMinute == 0 || parsedMinute == 5) {
        parsedMinute = "0" + parsedMinute;
    }
    if (parsedHour < 10) {
        parsedHour = "0" + parsedHour;
    }

    // Values send to HTML <span>
    document.getElementById('slider-minute').innerHTML = parsedMinute;
    document.getElementById('slider-hour').innerHTML = parsedHour;
}