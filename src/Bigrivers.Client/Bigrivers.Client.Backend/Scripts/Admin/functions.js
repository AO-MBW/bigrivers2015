
function readThumb(input) {
	if(input.files && input.files[0]) {
		
		var reader = new FileReader();
		
		reader.onload = function (e) {
			$('#thumb')
				.attr('src', e.target.result)
				.width(150)
				.height(150);
		};
		reader.readAsDataURL(input.files[0]);
	}
}

$(document).ready(function() {

	$('.dropdown').click(function() {
		$('.sub_navigation').toggle("slide");
		});
		
	$(document).mouseup(function (e)
	{	
		var menuContainer = $('#accountContainer');
		var submenuContainer = $('.sub_navigation');
	
		if(!menuContainer.is(e.target) && menuContainer.has(e.target).length === 0)
		{
			submenuContainer.hide();
		}
	});
	
});
	