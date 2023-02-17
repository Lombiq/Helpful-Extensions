(function initTargetBlank() {
    function targetBlank() {
        const links = document.querySelectorAll('a');
        const currentHostname = window.location.hostname;

        for (let i = 0; i < links.length; i++) {
            if (!links[i].href.match(/^mailto:/) &&
                (links[i].hostname !== currentHostname && (!links[i].href.match(/^javascript:/i)))) {
                links[i].setAttribute('target', '_blank');
            }
        }
    }
    window.addEventListener(
        'load',
        () => {
            window.setTimeout(targetBlank, 100);
        },
        false);
})();
