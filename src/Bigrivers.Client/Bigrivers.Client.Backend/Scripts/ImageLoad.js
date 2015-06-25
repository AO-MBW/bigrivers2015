$(function () {
    var asyncloadingimages = $(".async-load-image");

    $.each(asyncloadingimages, function () {
        if (fileExists($(this).data("cdnurl")) == 200) {
            $(this).attr("src", $(this).data("cdnurl"));
        }
        else {
            $(this).attr("src", $(this).data("azureurl"));
        }
    });
});

function fileExists(url) {
    $.ajax({
        type: "HEAD",
        url: url,
        success: function () {
            return 200;
        }
    });
}