// Javascript to toggle the background colors of the register and //login tabs.
$(function() {
    $('#login-form-link').click(function(e) {
    	$("#login-form").delay(100).fadeIn(100);
 		$("#register-form").fadeOut(100);
		$('.register').css({"background":"#0269c2"});
		$('.login').css({"background":"#fdfdfd"});
		$('.login').css({"color":"#000"});
		$('.register').css({"color":"#fff"});
		e.preventDefault();
	});
	
	$('#register-form-link').click(function(e) {
		$("#register-form").delay(100).fadeIn(100);
 		$("#login-form").fadeOut(100);
 		$('.login').css({"background":"#0269c2"});
 		$('.login').css({"color":"#fff"});
 		$('.register').css({"color":"#000"});
		$('.register').css({"background":"#fdfdfd"});
		
		e.preventDefault();
	});

});