// displayed Canvas. Here we render the current image
var renderCanvas;
var renderContext;

// stores currently drawn Shape during mouse movement
var tempCanvas;
var tempContext;

// Last entry in the array represent the current image.
var canvasLog = new Array();

// selected action. Can be a Shape to be drawn or cut tool etc.
var action = "Rechteck"

// stores mouse position at mouse down / mouse up
var startX, endX, startY, endY;

var mouseIsDown = 0;

var lineWidth = 1;
var Color = "#FF0000";

// crop variables
var forceProportions = true;

var actionLog = new Array();

function init(color) {
    Color = color;

    renderCanvas = document.getElementById('renderCanvas');
    renderContext = renderCanvas.getContext('2d');

    tempCanvas = document.getElementById('tempCanvas');
    tempContext = tempCanvas.getContext('2d');

    // the initial canvas to be rendered. Just a white background.
    var initialCanvas = copyCanvas(tempCanvas);
    initialCanvas.getContext('2d').fillStyle = "white";
    initialCanvas.getContext('2d').fillRect(0, 0, initialCanvas.width, initialCanvas.height);
    canvasLog.push(initialCanvas);

    //console.log(destWidth);
    //console.log(destHeight);

    eventListener();
}

function test() {
    renderContext.fillStyle = "rgba(156, 170, 193, 1)";
    renderContext.fillRect(30, 30, 70, 90);
    renderContext.fillStyle = "rgba(0, 109, 141, 1)";
    renderContext.fillRect(10, 10, 70, 90);
    render();
}

function drawCanvas(destCanvas, sourceCanvas, fit = true) {
    if (sourceCanvas === undefined) {
        return;
    }

    var renderOffsetX = 0;
    var renderOffsetY = 0;
    var renderWidth = destCanvas.width;
    var renderHeight = destCanvas.height;

    if (fit) {
        var ratioX = sourceCanvas.width / destCanvas.width;
        var ratioY = sourceCanvas.height / destCanvas.height;
        //console.log(ratioX)
        //console.log(ratioY)
        if (ratioX > ratioY) {
            renderHeight = sourceCanvas.height / ratioX;
            renderOffsetY = (destCanvas.height - renderHeight) / 2;
        }
        else {
            renderWidth = sourceCanvas.width / ratioY;
            renderOffsetX = (destCanvas.width - renderWidth) / 2;
        }
    }

    destCanvas.getContext('2d').drawImage(sourceCanvas, renderOffsetX, renderOffsetY, renderWidth, renderHeight);
}

function loadImage(imageURL) {
    var img = new Image();
    img.src = imageURL;
    img.crossOrigin = "anonymous";
    img.onload = function () {
        // draw image on a new canvas keeping the original size
        var imageCanvas = document.createElement('canvas');
        imageCanvas.width = img.width;
        imageCanvas.height = img.height;
        imageCanvas.getContext('2d').drawImage(img, 0, 0, imageCanvas.width, imageCanvas.height);

        // create target canvas fitting proportions of the render canvas
        var inMemoryCanvas = copyCanvas(renderCanvas);

        // draw image onto target canvas -> scaling the image wile keeping proportions
        drawCanvas(inMemoryCanvas, imageCanvas, true);

        canvasLog.push(inMemoryCanvas);

        render();
    }
}

function copyCanvas(canvas) {
    var inMemoryCanvas = document.createElement('canvas');
    inMemoryCanvas.width = canvas.width;
    inMemoryCanvas.height = canvas.height;
    inMemoryCanvas.getContext('2d').drawImage(
        canvas
        , 0, 0
        , canvas.width, canvas.height
    );
    return inMemoryCanvas
}

function rotateCanvas(canvas, deg = 90) {
    var ctx = canvas.getContext("2d");
    var tempCanvas = document.createElement('canvas');
    var tempCtx = tempCanvas.getContext("2d");

    tempCanvas.width = canvas.width;
    tempCanvas.height = canvas.height;

    tempCtx.drawImage(canvas, 0, 0, canvas.width, canvas.height);

    canvas.width = tempCanvas.height;
    canvas.height = tempCanvas.width;

    ctx.clearRect(0, 0, canvas.width, canvas.height);
    
    ctx.translate(canvas.width / 2, canvas.height / 2);
    ctx.rotate(deg * Math.PI / 180);
    ctx.drawImage(tempCanvas, -canvas.height / 2, -canvas.width / 2, canvas.height, canvas.width);

    ctx.rotate(-deg * Math.PI / 180);
    ctx.translate(-canvas.width / 2, -canvas.height / 2);
}

function rotate(deg = 90) {
    var inMemoryCanvas = copyCanvas(canvasLog[canvasLog.length - 1]);
    rotateCanvas(inMemoryCanvas, deg);
    canvasLog.push(inMemoryCanvas);
    render();
}

function relativePos(event, element) {
    var rect = element.getBoundingClientRect();
    return {
        x: Math.floor(event.clientX - rect.left),
        y: Math.floor(event.clientY - rect.top)
    };
}

function setLineWidth(ddl) {
    lineWidth = ddl.value
}

function setAction(ddl) {
    action = ddl.value
}

function setColor(colorChoice) {
    Color = colorChoice.value;
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
            var currentCanvas = copyCanvas(canvasLog[canvasLog.length - 1]); // new canvas is old canvas
            drawCanvas(currentCanvas, tempCanvas) // draw shape on top of new canvas
            canvasLog.push(currentCanvas); // add new canvas to the Log
        }
        if (action === 'Zuschneiden') {
            if (forceProportions) {
                endY = startY + (endY - startY) / Math.abs(endY - startY) * renderCanvas.height / renderCanvas.width * Math.abs(startX - pos.x);
            }

            var w = endX - startX;
            var h = endY - startY;
            var offsetX = (w < 0) ? w : 0;
            var offsetY = (h < 0) ? h : 0;
            var sourceX = offsetX + startX;
            var sourceY = offsetY + startY;
            var sourceWidth = Math.abs(w);
            var sourceHeight = Math.abs(h);

            var currentCanvas = copyCanvas(canvasLog[canvasLog.length - 1]); // new canvas is old canvas

            currentCanvas.getContext('2d').drawImage(
                canvasLog[canvasLog.length - 1],
                sourceX, sourceY,
                sourceWidth, sourceHeight,
                0, 0,
                renderCanvas.width, renderCanvas.height
            );

            canvasLog.push(currentCanvas); // add new canvas to the Log

        }
        actionLog.push(action);
        tempContext.clearRect(0, 0, tempCanvas.width, tempCanvas.height); // shape no longer beeing drawn -> remove it from temp
    }
}

function undo() {
    if (canvasLog.length > 0) {
        canvasLog.pop();
        render();
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
            endY = startY + (endY - startY) / Math.abs(endY - startY) * renderCanvas.height / renderCanvas.width * Math.abs(startX - pos.x);
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
    ctx.strokeStyle = Color;
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

    // White background in case image does not fill canvas
    renderContext.fillStyle = "white";
    renderContext.fillRect(0, 0, renderCanvas.width, renderCanvas.height);

    // render current image
    if (canvasLog.length > 0) {
        drawCanvas(renderCanvas, canvasLog[canvasLog.length - 1])
    }

    // add shape currently beeing drawn on top
    drawCanvas(renderCanvas, tempCanvas, true);

}

function saveImageData() {
    try {
        var image = renderCanvas.toDataURL("image/png");
        image = image.replace('data:image/png;base64,', '');
        document.getElementById("imageData").value = image;
        console.log("test");
    } catch (error) {
        console.error(error);
    }
}