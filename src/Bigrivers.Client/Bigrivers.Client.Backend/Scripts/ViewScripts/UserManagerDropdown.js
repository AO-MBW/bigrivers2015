$(document).ready(function() {

	$(".dropdown").click(function() {
		$(".sub_navigation").toggle("blind");
	});
		
	$(document).mouseup(function (e)
	{	
		var menuContainer = $("#accountContainer");
		var submenuContainer = $(".sub_navigation");
	
		if(!menuContainer.is(e.target) && menuContainer.has(e.target).length === 0)
		{
			submenuContainer.hide("blind");
		}
	});
	
});