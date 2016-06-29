//
//  Helper for correct size of Messages page
//
function MessagesMenuWidth() {
    var W = window.innerWidth;
    var W_menu = $('#sidebar-left').outerWidth();
    var w_messages = (W - W_menu) * 16.666666666666664 / 100;
    $('#messages-menu').width(w_messages);
}

//
//  Screensaver function
//  used on locked screen, and write content to element with id - canvas
//
function ScreenSaver(){
	var canvas = document.getElementById("canvas");
	var ctx = canvas.getContext("2d");
	// Size of canvas set to fullscreen of browser
	var W = window.innerWidth;
	var H = window.innerHeight;
	canvas.width = W;
	canvas.height = H;
	// Create array of particles for screensaver
	var particles = [];
	for (var i = 0; i < 25; i++) {
		particles.push(new Particle());
	}
	function Particle(){
		// location on the canvas
		this.location = {x: Math.random()*W, y: Math.random()*H};
		// radius - lets make this 0
		this.radius = 0;
		// speed
		this.speed = 3;
		// random angle in degrees range = 0 to 360
		this.angle = Math.random()*360;
		// colors
		var r = Math.round(Math.random()*255);
		var g = Math.round(Math.random()*255);
		var b = Math.round(Math.random()*255);
		var a = Math.random();
		this.rgba = "rgba("+r+", "+g+", "+b+", "+a+")";
	}
	// Draw the particles
	function draw() {
		// re-paint the BG
		// Lets fill the canvas black
		// reduce opacity of bg fill.
		// blending time
		ctx.globalCompositeOperation = "source-over";
		ctx.fillStyle = "rgba(0, 0, 0, 0.02)";
		ctx.fillRect(0, 0, W, H);
		ctx.globalCompositeOperation = "lighter";
		for(var i = 0; i < particles.length; i++){
			var p = particles[i];
			ctx.fillStyle = "white";
			ctx.fillRect(p.location.x, p.location.y, p.radius, p.radius);
			// Lets move the particles
			// So we basically created a set of particles moving in random direction
			// at the same speed
			// Time to add ribbon effect
			for(var n = 0; n < particles.length; n++){
				var p2 = particles[n];
				// calculating distance of particle with all other particles
				var yd = p2.location.y - p.location.y;
				var xd = p2.location.x - p.location.x;
				var distance = Math.sqrt(xd*xd + yd*yd);
				// draw a line between both particles if they are in 200px range
				if(distance < 200){
					ctx.beginPath();
					ctx.lineWidth = 1;
					ctx.moveTo(p.location.x, p.location.y);
					ctx.lineTo(p2.location.x, p2.location.y);
					ctx.strokeStyle = p.rgba;
					ctx.stroke();
					//The ribbons appear now.
				}
			}
			// We are using simple vectors here
			// New x = old x + speed * cos(angle)
			p.location.x = p.location.x + p.speed*Math.cos(p.angle*Math.PI/180);
			// New y = old y + speed * sin(angle)
			p.location.y = p.location.y + p.speed*Math.sin(p.angle*Math.PI/180);
			// You can read about vectors here:
			// http://physics.about.com/od/mathematics/a/VectorMath.htm
			if(p.location.x < 0) p.location.x = W;
			if(p.location.x > W) p.location.x = 0;
			if(p.location.y < 0) p.location.y = H;
			if(p.location.y > H) p.location.y = 0;
		}
	}
	setInterval(draw, 30);
};
//
//  Helper for open ModalBox with requested header, content and bottom
//
//
function OpenModalBox(header, inner, bottom){
	var modalbox = $('#modalbox');
	modalbox.find('.modal-header-name span').html(header);
	modalbox.find('.devoops-modal-inner').html(inner);
	modalbox.find('.devoops-modal-bottom').html(bottom);
	modalbox.fadeIn('fast');
	$('body').addClass("body-expanded");
}
//
//  Close modalbox
//
//
function CloseModalBox(){
	var modalbox = $('#modalbox');
	modalbox.fadeOut('fast', function(){
		modalbox.find('.modal-header-name span').children().remove();
		modalbox.find('.devoops-modal-inner').children().remove();
		modalbox.find('.devoops-modal-bottom').children().remove();
		$('body').removeClass("body-expanded");
	});
}

function LoadAjaxContent(url) {
	$('.preloader').show();
	$.ajax({
         cache: false,
		mimeType : 'text/html; charset=utf-8', // ! Need set mimeType only when run from local file
		url : url,
		type : 'GET',
		success : function (data) {
			$('#ajax-content').html(data);
			$('.preloader').hide();

		},
		error : function (jqXHR, textStatus, errorThrown) {
			alert(errorThrown);
			$('.preloader').hide();
		},
		dataType : "html",
		async : false
	});
}


var _setSideMenuDisplayState = function () {
    var _isshown = !$("div#main").hasClass("sidebar-show");
    $.post("/Home/SetDisplaystate", { displaystate: _isshown });
};

var _setSideMenuDisplayStateCallback = function (data) {
    
}

$(document).ready(function () {
    $('.show-sidebar').on('click', function (e) {
        e.preventDefault();
        $('div#main').toggleClass('sidebar-show');
        setTimeout(MessagesMenuWidth, 250);
        _setSideMenuDisplayState();
    });

    $('.preloader').hide();

    $('.main-menu').on('click', 'a', function (e) {
        var parents = $(this).parents('li');
        var li = $(this).closest('li.dropdown');
        var another_items = $('.main-menu li').not(parents);
        another_items.find('a').removeClass('active');
        another_items.find('a').removeClass('active-parent');
        if ($(this).hasClass('dropdown-toggle') || $(this).closest('li').find('ul').length == 0) {
            $(this).addClass('active-parent');
            var current = $(this).next();
            if (current.is(':visible')) {
                li.find("ul.dropdown-menu").slideUp('fast');
                li.find("ul.dropdown-menu a").removeClass('active')
            } else {
                another_items.find("ul.dropdown-menu").slideUp('fast');
                current.slideDown('fast');
            }
        } else {
            if (li.find('a.dropdown-toggle').hasClass('active-parent')) {
                var pre = $(this).closest('ul.dropdown-menu');
                pre.find("li.dropdown").not($(this).closest('li')).find('ul.dropdown-menu').slideUp('fast');
            }
        }
        if ($(this).hasClass('active') == false) {
            $(this).parents("ul.dropdown-menu").find('a').removeClass('active');
            $(this).addClass('active')
        }
        if ($(this).hasClass('ajax-link')) {
            e.preventDefault();
            if ($(this).hasClass('add-full')) {
                $('#content').addClass('full-content');
            } else {
                $('#content').removeClass('full-content');
            }
            var url = $(this).attr('href');
            var title = $(this).text();
            var state = {
                "url": url
            };
            window.history.pushState(state, title, url);
            LoadAjaxContent(url);
        }

        if ($(this).attr('href') == '#') {
            e.preventDefault();
        }

    });
    var height = window.innerHeight - 80;
    $('#main').css('min-height', height)
	.on('click', '.expand-link', function (e) {
	    var body = $('body');
	    e.preventDefault();
	    var box = $(this).closest('div.box');
	    var button = $(this).find('i');
	    button.toggleClass('fa-expand').toggleClass('fa-compress');
	    box.toggleClass('expanded');
	    body.toggleClass('body-expanded');
	    var timeout = 0;
	    if (body.hasClass('body-expanded')) {
	        timeout = 100;
	    }
	    setTimeout(function () {
	        box.toggleClass('expanded-padding');
	    }, timeout);
	    setTimeout(function () {
	        box.resize();
	        box.find('[id^=map-]').resize();
	    }, timeout + 50);
	})
	.on('click', '.collapse-link', function (e) {
	    e.preventDefault();
	    var box = $(this).closest('div.box');
	    var button = $(this).find('i');
	    var content = box.find('div.box-content');
	    content.slideToggle('fast');
	    button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
	    setTimeout(function () {
	        box.resize();
	        box.find('[id^=map-]').resize();
	    }, 50);
	})
	.on('click', '.close-link', function (e) {
	    e.preventDefault();
	    var content = $(this).closest('div.box');
	    content.remove();
	});
    $('#locked-screen').on('click', function (e) {
        e.preventDefault();
        $('body').addClass('body-screensaver');
        $('#screensaver').addClass("show");
        ScreenSaver();
    });
    $('body').on('click', 'a.close-link', function (e) {
        e.preventDefault();
        CloseModalBox();
    });
    $('#top-panel').on('click', 'a', function (e) {
        if ($(this).hasClass('ajax-link')) {
            e.preventDefault();
            if ($(this).hasClass('add-full')) {
                $('#content').addClass('full-content');
            } else {
                $('#content').removeClass('full-content');
            }
            //			var url = $(this).attr('href');
            //			window.location.hash = url;
            var url = $(this).attr('href');
            var title = $(this).text();
            var state = {
                "url": url
            };
            window.history.pushState(state, title, url);
            LoadAjaxContent(url);
        }
    });
    $('#search').on('keydown', function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('#content').removeClass('full-content');
            ajax_url = 'ajax/page_search.html';
            window.location = ajax_url;

        }
    });
    $('#screen_unlock').on('mouseover', function () {
        var header = 'Enter current username and password';
        var form = $('<div class="form-group"><label class="control-label">Username</label><input type="text" class="form-control" name="username" /></div>' +
				'<div class="form-group"><label class="control-label">Password</label><input type="password" class="form-control" name="password" /></div>');
        var button = $('<div class="text-center"><a href="index.html" class="btn btn-primary">Unlock</a></div>');
        OpenModalBox(header, form, button);
    });

    window.addEventListener("popstate", function (e) {

        LoadAjaxContent(e.state.url);

    });

});
