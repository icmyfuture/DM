$(function() {
	toggleDiv();
	fadeH2();
	fadeImagesB();
});

function toggleDiv() {
	var div = $("#myDiv");
	$("#inputButton").click(function() {
		div.slideToggle(2000);
	});
}

function fadeH2() {
	var fadeCount = 0;
	var timer = setInterval(function() {
		fadeCount++;
		if (fadeCount > 100) {
			clearInterval(timer);
		} else {
			$("#hideH2").fadeOut(5000).slideDown(1000);
		}
	}, 7000);
}

function fadeImagesA() {
	var imageCache = [];
	$("#images img").each(function() {
		//preload image
		var image = new Image();
		image.src = $(this).attr("src");
		image.title = $(this).attr("alt");
		imageCache.push(image);
	});

	var imageCounter = 0;
	var nextImage;
	setInterval(function() {
		$("#caption").fadeOut(1000);
		$("#image").fadeOut(1000, function() {
			imageCounter = (imageCounter + 1) % imageCache.length;
			nextImage = imageCache[imageCounter];
			$("#image").attr("src", nextImage.src).fadeIn(1000);
			$("#caption").text(nextImage.title).fadeIn(1000);
		});
	}, 3000);
}

var nextImage = $("#images img:first-child");
function fadeImagesB() {
	var timer = setInterval(intervalFunc, 3000);
	$("caption").toggle(function() {
		clearInterval(timer);
	}, function() {
		setInterval(intervalFunc, 3000);
	});
}

function intervalFunc() {
	$("#caption").fadeOut(1000);
	$("#image").fadeOut(1000, function() {
		if (nextImage.next().length == 0) {
			nextImage = $("#images img:first-child");
		} else {
			nextImage = nextImage.next();
		}
		$("#image").attr("src", nextImage.attr("src")).fadeIn(1000);
		$("#caption").text(nextImage.attr("alt")).fadeIn(1000);
	});
}
