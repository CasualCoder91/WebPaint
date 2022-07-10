var context;
var canvas;

function init() {
    canvas = document.getElementById('imageCanvas');
    context = canvas.getContext('2d');
}

function test() {
    context.fillStyle = "rgba(156, 170, 193, 1)";
    context.fillRect(30, 30, 70, 90);
    context.fillStyle = "rgba(0, 109, 141, 1)";
    context.fillRect(10, 10, 70, 90);
}

function drawImage(imageURL) {
	var img = new Image();
    img.src = imageURL;
	img.onload = function () {
        context.drawImage(img, 20, 20);
	}
}

function relativePos(event, element) {
    var rect = element.getBoundingClientRect();
    return {
        x: Math.floor(event.clientX - rect.left),
        y: Math.floor(event.clientY - rect.top)
    };
}

function trackDrag(onMove) {
    function end(event) {
        removeEventListener("mousemove", onMove);
        removeEventListener("mouseup", end);
    }
    addEventListener("mousemove", onMove);
    addEventListener("mouseup", end);
}

function mouseMove(e) {
    if (drag) {
        rect.w = (e.pageX - this.offsetLeft) - rect.startX;

        rect.h = (e.pageY - this.offsetTop) - rect.startY;
        drawx = e.pageX - this.offsetLeft;
        drawy = e.pageY - this.offsetTop;
        prevX = currX;
        prevY = currY;
        currX = e.clientX - canvas.offsetLeft;
        currY = e.clientY - canvas.offsetTop;
        if (drag = true) {
            radius_New += 2;

        }
        draw();
        if (DrawingTypes == "FreeDraw" || DrawingTypes == "Erase") {
        }
        else {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }

    }
    drawOldShapes();
}

function drawRect(event) {
    context.strokeStyle = 'orange';
    
    var pos = relativePos(event, context.canvas);
    trackDrag(function (event) {
        context.beginPath();
        context.moveTo(pos.x, pos.y);
        newPos = relativePos(event, context.canvas);
        context.strokeRect(pos.x, pos.y, newPos.x - pos.x, newPos.y - pos.y);
    });
}

function addMousedownToCanvas(ddl) {
    init();

    canvas.addEventListener("mousedown", function (event) {
        if (event.which == 1) {
            if (ddl.value === "Rechteck") {
                drawRect(event);
            }
            event.preventDefault();
        }
    });
}