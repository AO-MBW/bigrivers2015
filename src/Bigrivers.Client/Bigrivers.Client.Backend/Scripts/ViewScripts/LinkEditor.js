$(function () {
    $("#SelectLinkType").change(ChangeLinkType);
    $("#SelectInternalType").change(ChangeInternalType);

    ChangeLinkType();
    ChangeInternalType();
});

var ChangeLinkType = function () {
    $(".linkType").hide();
    $("#" + $("#SelectLinkType").val()).show();
};

var ChangeInternalType = function () {
    $(".InternalType").hide();
    $("#" + $("#SelectInternalType").val()).show();
};