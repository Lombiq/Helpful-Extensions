function targetBlank() {
    var x = document.querySelectorAll("a");

    for (var i = 0; i < x.length; i++) {
        if (!x[i].href.match(/^mailto\:/) && (x[i].hostname != location.hostname)) {
            x[i].setAttribute("target", "_blank");
        }
    }
}

window.addEventListener("load", function () {
    window.setTimeout(targetBlank, 100);
}, false);

