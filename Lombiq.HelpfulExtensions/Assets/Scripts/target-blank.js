(function initTargetBlank() {
    function addTargetBlank() {
        const links = document.querySelectorAll('a');
        const currentHostname = window.location.hostname;

        for (let i = 0; i < links.length; i++) {
            if (links[i].hostname !== currentHostname &&
                // eslint-disable-next-line no-script-url
                !links[i].href.startsWith('javascript:') &&
                !links[i].href.startsWith('mailto:')) {
                links[i].setAttribute('target', '_blank');
            }
        }
    }
    document.addEventListener('DOMContentLoaded', addTargetBlank);
})();
