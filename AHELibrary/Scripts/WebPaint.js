// displayed Canvas. Here we render the current image
var renderCanvas;
var renderContext;

// stores currently drawn Shape during mouse movement
var tempCanvas;
var tempContext;

// Layers of the Image. All rendered on renderCanvas
var canvasArray = new Array();

// selected action. Can be a Shape to be drawn or maybe a cut tool etc.
var action = "Rechteck"

// stores mouse position at mouse down / mouse up
var startX, endX, startY, endY;

var mouseIsDown = 0;

var lineWidth = 10

function init() {
    renderCanvas = document.getElementById('renderCanvas');
    renderContext = renderCanvas.getContext('2d');

    tempCanvas = document.getElementById('tempCanvas');
    tempContext = renderCanvas.getContext('2d');

    eventListener();
}

function test() {
    renderContext.fillStyle = "rgba(156, 170, 193, 1)";
    renderContext.fillRect(30, 30, 70, 90);
    renderContext.fillStyle = "rgba(0, 109, 141, 1)";
    renderContext.fillRect(10, 10, 70, 90);
    render();
}

function drawImage(imageURL) {
	var img = new Image();
    img.src = imageURL;
    img.onload = function () {
        var inMemoryCanvas = document.createElement('canvas');
        inMemoryCanvas.getContext('2d').drawImage(img, 0, 0);
        canvasArray.push(inMemoryCanvas)
	}
}

function copyCanvas(canvas) {
    var inMemoryCanvas = document.createElement('canvas');
    inMemoryCanvas.width = canvas.width;
    inMemoryCanvas.height = canvas.height;
    inMemoryCanvas.getContext('2d').drawImage(
        tempCanvas
        , 0, 0
        , canvas.width, canvas.height
    );
    return inMemoryCanvas
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
        currX = e.clientX - renderCanvas.offsetLeft;
        currY = e.clientY - renderCanvas.offsetTop;
        if (drag = true) {
            radius_New += 2;

        }
        draw();
        if (DrawingTypes == "FreeDraw" || DrawingTypes == "Erase") {
        }
        else {
            ctx.clearRect(0, 0, renderCanvas.width, renderCanvas.height);
        }

    }
    drawOldShapes();
}

function setLineWidth(ddl) {
    lineWidth = ddl.value
}

function eventListener() {
    if (action == 'Rechteck') {
        renderCanvas.addEventListener("mousedown", function (e) {
            mouseDown(e);
            render();
        }, false);
        renderCanvas.addEventListener("mousemove", function (e) {
            mouseXY(e);
            render();
        }, false);
        renderCanvas.addEventListener("mouseup", function (e) {
            mouseUp(e);
            render();
        }, false);
    }
}
function mouseUp(event) {
    if (mouseIsDown !== 0) {
        mouseIsDown = 0;
        var pos = relativePos(event, renderCanvas);
        endX = pos.x;
        endY = pos.y;
        if (action == 'Rechteck') {
            canvasArray.push(copyCanvas(tempCanvas)); // add shape to array when drawing is finised.
            tempContext.clearRect(0, 0, tempCanvas.width, tempCanvas.height); // shape no longer beeing drawn -> remove it from temp
        }
    }
}

function mouseDown(event) {
    mouseIsDown = 1;
    var pos = relativePos(event, renderCanvas);
    startX = endX = pos.x;
    startY = endY = pos.y;
}

function mouseXY(event) {
    if (mouseIsDown !== 0) {
        var pos = relativePos(event, renderCanvas);
        endX = pos.x;
        endY = pos.y;
        drawSquare(tempCanvas, true);
    }
}

function drawSquare(cnv, clear) {
    var ctx = cnv.getContext("2d");
    if (clear && clear === true) {
        ctx.clearRect(0, 0, cnv.width, cnv.height);
    }
    // creating a square
    var w = endX - startX;
    var h = endY - startY;
    var offsetX = (w < 0) ? w : 0;
    var offsetY = (h < 0) ? h : 0;
    var width = Math.abs(w);
    var height = Math.abs(h);

    ctx.beginPath();
    ctx.globalAlpha = 0.7;
    ctx.strokeStyle = 'orange'
    ctx.lineWidth = lineWidth
    ctx.strokeRect(startX + offsetX, startY + offsetY, width, height);
}

function render() {
    // render all stored Layers 
    var arrayLength = canvasArray.length;
    for (var i = 0; i < arrayLength; i++) {
        renderContext.drawImage(
            canvasArray[i]
            , 0, 0
            , renderCanvas.width, renderCanvas.height
        );
    }

    // add shape currently beeing drawn on top
    renderContext.drawImage(
        tempCanvas
        , 0, 0
        , renderCanvas.width, renderCanvas.height
    );
}