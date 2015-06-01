$(document).ready(function () {
	if ($("#link-upload-type").val() == "True") $("#link-upload-type").val(true);
	if ($("#link-upload-type").val() == "False") $("#link-upload-type").val(false);

	$("#link-upload-button").click(function () {
		$(this).removeClass("non-selected").addClass("selected");
		$("#link-gallery-button").removeClass("selected").addClass("non-selected");
		$("#link-file-gallery").hide("blind");
		$("#link-file-upload").show("blind");
		$("#link-file-select-container").children("#link-upload-type").val("true");
	});

	$("#link-gallery-button").click(function () {
		$(this).removeClass("non-selected").addClass("selected");
		$("#link-upload-button").removeClass("selected").addClass("non-selected");
		$("#link-file-upload").hide("blind");
		$("#link-file-gallery").show("blind").css("display", "inline-block");
		$("#link-file-select-container").children("input[type='hidden']").val("false");
	});

	$(".link-file-gallery-item-container").click(function() {
		$("#link-select-existing-key").val($(this).data("file-key"));

		var blocks = $(".link-file-gallery-item-container");
		// Set default item in selectlist
		$.each(blocks, function() {
			$(this).removeClass("selected").addClass("non-selected");
		});
		$(this).removeClass("non-selected").addClass("selected");
	});
});