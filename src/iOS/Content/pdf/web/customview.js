PDFJS.disableWorker = false;

var pdfDoc = null,
    pageNum = 1,
    pages = document.getElementById("pages");

function renderPage(num) {

    var pdfPage = pdfDoc.getPage(num);

    pdfPage.then(function (page) {

        var canvas = document.createElement('canvas');
        pages.appendChild(canvas);

        canvas.width = window.innerWidth;
        var viewportScale = page.getViewport(canvas.width / page.getViewport(1.0).width);
        var viewport = page.getViewport(viewportScale.scale);
        canvas.height = viewport.height;
        canvas.width = viewport.width;


        var ctx = canvas.getContext('2d');
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        page.render(renderContext);
    });
}

PDFJS.getDocument(url).then(function(_pdfDoc) {
    pdfDoc = _pdfDoc;
    var pages = pdfDoc.numPages;

    for (var i = 0; i < pages; i++)
        renderPage(i + 1);
});