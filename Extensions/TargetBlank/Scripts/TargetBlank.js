document.currentScript.setAttribute('defer', 'defer');

function targetBlank() {
    var x = document.querySelectorAll("a");

    for (var i = 0; i < x.length; i++) {
        console.log(x[i].href);

        if (!x[i].href.match(/^mailto\:/) && (x[i].hostname != location.hostname)) {

            x[i].setAttribute("target", "_blank");
        }
    }
}

targetBlank();
