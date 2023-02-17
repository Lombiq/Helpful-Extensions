function targetBlank() {
    const links = document.querySelectorAll('a');
    const currentHostname = window.location.hostname;

    for (let i = 0; i < links.length; i++) {
        const link = links[i];
        const linkHostname = link.hostname;

        if (linkHostname !== currentHostname && link.href && !link.href.startsWith('#') && !link.getAttribute('rel') && !link.target) {
            link.setAttribute('target', '_blank');
        }
    }
}
window.addEventListener(
    'load',
    () => {
        window.setTimeout(targetBlank, 100)
    },
    false);
