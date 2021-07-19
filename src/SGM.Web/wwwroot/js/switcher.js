/*-----------------------------------------------------------------------------------
/* Styles Switcher
-----------------------------------------------------------------------------------*/


jQuery(document).ready(function($) {
	

	$("ul.pattern .color1" ).click(function(){
		$("#option-color").attr("href", "css/color/default.css" );
		return false;
	});
	
	$("ul.pattern .color2" ).click(function(){
		$("#option-color").attr("href", "css/color/blue.css" );
		return false;
	});
	
	$("ul.pattern .color3" ).click(function(){
		$("#option-color").attr("href", "css/color/green.css" );
		return false;
	});

	$("ul.pattern .color4" ).click(function(){
		$("#option-color").attr("href", "css/color/purple.css" );
		return false;
	});
	$("ul.pattern .color5" ).click(function(){
		$("#option-color").attr("href", "css/color/skyblue.css" );
		return false;
	});
	$("ul.pattern .color6" ).click(function(){
		$("#option-color").attr("href", "css/color/yellow.css" );
		return false;
	});
	
	$("#color-switcher .bottom a.settings").click(function(e){
		e.preventDefault();
		var div = $("#color-switcher");
		if (div.css("left") === "-189px") {
			$("#color-switcher").animate({
				left: "0px"
			}); 
		} else {
			$("#color-switcher").animate({
				left: "-189px"
			});
		}
	})
	
	$("ul.pattern li a").click(function(e){
		e.preventDefault();
		$(this).parent().parent().find("a").removeClass("active");
		$(this).addClass("active");
	})

	$("#color-switcher").animate({
		left: "-189px"
	}); 
			
});