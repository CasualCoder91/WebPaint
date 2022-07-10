function test() {
    var canvas = document.getElementById('imageCanvas');
    var context = canvas.getContext('2d');
    context.fillStyle = "rgba(156, 170, 193, 1)";
    context.fillRect(30, 30, 70, 90);
    context.fillStyle = "rgba(0, 109, 141, 1)";
    context.fillRect(10, 10, 70, 90);
}

function drawImage(imageURL) {
	var mycanvas = document.getElementById('imageCanvas');
	var ctx = mycanvas.getContext('2d');
	var img = new Image();
    img.src = imageURL;
	img.onload = function () {
		ctx.drawImage(img, 20, 20);
	}
}