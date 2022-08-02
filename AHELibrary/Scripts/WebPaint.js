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

var lineWidth = 1;
var color = "#ff0000";

// crop variables
var forceProportions = true;
var sourceX = 0;
var sourceY = 0;
var sourceWidth;
var sourceHeight;
var destX = 0;
var destY = 0;
var destWidth;
var destHeight;

function init() {
    renderCanvas = document.getElementById('renderCanvas');
    renderContext = renderCanvas.getContext('2d');

    tempCanvas = document.getElementById('tempCanvas');
    tempContext = tempCanvas.getContext('2d');

    sourceWidth = renderCanvas.width;
    sourceHeight = renderCanvas.height;
    destWidth = sourceWidth;
    destHeight = sourceHeight;
    console.log(destWidth);
    console.log(destHeight);

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
        inMemoryCanvas.width = renderCanvas.width;
        inMemoryCanvas.height = renderCanvas.height;
        inMemoryCanvas.getContext('2d').drawImage(img, 0, 0, renderCanvas.width, renderCanvas.height);
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
        x: Math.floor(event.clientX - rect.left) * sourceWidth / destWidth + sourceX,
        y: Math.floor(event.clientY - rect.top) * sourceHeight / destHeight + sourceY
    };
}

function setLineWidth(ddl) {
    lineWidth = ddl.value
}

function setAction(ddl) {
    action = ddl.value
}

function setColor(colorChoice) {
    color = colorChoice.value;
}

function eventListener() {
    if (action === 'Rechteck') {
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
        if (action === 'Rechteck') {
            canvasArray.push(copyCanvas(tempCanvas)); // add shape to array when drawing is finised.
        }
        if (action === 'Zuschneiden') {
            var w = endX - startX;
            var h = endY - startY;
            var offsetX = (w < 0) ? w : 0;
            var offsetY = (h < 0) ? h : 0;
            sourceX = offsetX + startX;
            sourceY = offsetY + startY;
            sourceWidth = Math.abs(w);
            sourceHeight = Math.abs(h);
        }
        tempContext.clearRect(0, 0, tempCanvas.width, tempCanvas.height); // shape no longer beeing drawn -> remove it from temp
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
        if (forceProportions && action === 'Zuschneiden') {
            endY = startY + (endY - startY) / Math.abs(endY - startY) * sourceHeight / sourceWidth * Math.abs(startX - pos.x);
        }
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
    ctx.globalAlpha = 1;
    ctx.strokeStyle = color;
    ctx.lineWidth = lineWidth
    ctx.setLineDash([]); // solid line
    if (action === 'Zuschneiden') {
        ctx.globalAlpha = 1;
        ctx.lineWidth = 2;
        ctx.strokeStyle = 'black';
        ctx.setLineDash([5, 15]);
    }
    ctx.strokeRect(startX + offsetX, startY + offsetY, width, height);
}

function render() {
    // render all stored Layers 
    var arrayLength = canvasArray.length;
    for (var i = 0; i < arrayLength; i++) {
        renderContext.drawImage(
            canvasArray[i],
            sourceX, sourceY,
            sourceWidth, sourceHeight,
            destX, destY,
            destWidth, destHeight
        );
    }

    // add shape currently beeing drawn on top
    renderContext.drawImage(
        tempCanvas,
        sourceX, sourceY,
        sourceWidth, sourceHeight,
        destX, destY,
        destWidth, destHeight
    );
}