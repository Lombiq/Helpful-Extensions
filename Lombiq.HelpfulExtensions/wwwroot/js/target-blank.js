(function initTargetBlank() {
    function addTargetBlank() {
        const links = Array.from(document.querySelectorAll('a'));
        const currentHostname = window.location.hostname;

        links
            .filter(link => link.hostname !== currentHostname)
            // The no-script-url rule triggers to prevent the usage of javascript: URLs.
            // In the current script, we are not using them.
            // eslint-disable-next-line no-script-url
            .filter(link => link.href && !link.href.startsWith('javascript:') && !link.href.startsWith('mailto:'))
            .forEach(link => link.setAttribute('target', '_blank'));
    }

    document.addEventListener('DOMContentLoaded', addTargetBlank);
})();
