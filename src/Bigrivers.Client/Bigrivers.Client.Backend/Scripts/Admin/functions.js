
function readThumb(input) {
	if(input.files && input.files[0]) {
		
		var reader = new FileReader();
		
		reader.onload = function (e) {
			$("#thumb")
				.attr("src", e.target.result)
				.width(150)
				.height(150);
		};
		reader.readAsDataURL(input.files[0]);
	}
}
	