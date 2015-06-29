$(function () {
    var asyncloadingimages = $(".async-load-image");

    $.each(asyncloadingimages, function () {
        if (fileAvailable($(this).data("cdnurl"))) {
            $(this).attr("src", $(this).data("cdnurl"));
        }
        else {
            $(this).attr("src", $(this).data("azureurl"));
        }
    });
});

function fileAvailable(url) {
    window.status = false;
    $.ajax({
        type: "HEAD",
        url: url,
        success: function () {
            window.status = true;
        }
    });
    return window.status;
}