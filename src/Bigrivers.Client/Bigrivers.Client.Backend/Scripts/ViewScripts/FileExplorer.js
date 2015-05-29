$(document).ready(function () {
	if ($("#upload-type").val() == "True") $("#upload-type").val(true);
	if ($("#upload-type").val() == "False") $("#upload-type").val(false);

	$("#upload-button").click(function () {
		$(this).removeClass("non-selected").addClass("selected");
		$("#gallery-button").removeClass("selected").addClass("non-selected");
		$("#file-gallery").hide("blind");
		$("#file-upload").show("blind");
		$("#file-select-container").children("#upload-type").val("true");
	});

	$("#gallery-button").click(function () {
		$(this).removeClass("non-selected").addClass("selected");
		$("#upload-button").removeClass("selected").addClass("non-selected");
		$("#file-upload").hide("blind");
		$("#file-gallery").show("blind").css("display", "inline-block");
		$("#file-select-container").children("input[type='hidden']").val("false");
	});

	$(".file-gallery-item-container").click(function() {
		$("#select-existing-key").val($(this).data("file-key"));

		var blocks = $(".file-gallery-item-container");
		// Set default item in selectlist
		$.each(blocks, function() {
			$(this).removeClass("selected").addClass("non-selected");
		});
		$(this).removeClass("non-selected").addClass("selected");
	});
});