$(function () {
    var asyncloadingimages = $(".async-load-image");

    $.each(asyncloadingimages, function () {
        if (!fileExists($(this).data("cdnurl"))) {
            $(this).attr("src", $(this).data("cdnurl"));
            console.log("cdn");
        }
        else {
            $(this).attr("src", $(this).data("azureurl"));
            console.log("azure");
        }
    });
});

function fileExists(url) {
    var statuscode ;
    $.ajax({
        type: "HEAD",
        url: url,
        success: function () {
            statuscode = 200;
        }
    });
    return false;
}