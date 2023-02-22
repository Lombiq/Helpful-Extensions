(function initTargetBlank() {
    function addTargetBlank() {
        const links = document.querySelectorAll('a');
        const currentHostname = window.location.hostname;

        for (let i = 0; i < links.length; i++) {
            if (links[i].hostname !== currentHostname &&
                // The no-script-url rule triggers to prevent the usage of javascript: URLs.
                // In the current script, we are not using them.
                // eslint-disable-next-line no-script-url
                !links[i].href.startsWith('javascript:') &&
                !links[i].href.startsWith('mailto:')) {
                links[i].setAttribute('target', '_blank');
            }
        }
    }
    document.addEventListener('DOMContentLoaded', addTargetBlank);
})();
