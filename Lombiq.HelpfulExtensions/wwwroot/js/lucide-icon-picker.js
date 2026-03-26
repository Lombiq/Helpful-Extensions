(function () {
    const lucide = window.lucide;

    if (!lucide || !lucide.icons) return;

    const iconEntries = Object.keys(lucide.icons)
        .map(key => ({
            key,
            value: key
                .replace(/([a-z0-9])([A-Z])/g, "$1-$2")
                .replace(/([A-Z])([A-Z][a-z])/g, "$1-$2")
                .toLowerCase(),
        }))
        .sort((left, right) => left.value.localeCompare(right.value));

    const renderIcons = () => lucide.createIcons({
        attrs: {
            width: 18,
            height: 18,
            "stroke-width": 1.75,
        },
    });

    const getHiddenInput = root =>
        root.closest("[id$='_FieldWrapper']")?.querySelector("[data-lucide-value]") ??
        root.parentElement?.querySelector("[data-lucide-value]") ??
        root.querySelector("[data-lucide-value]");

    const updateSelection = (root, value) => {
        const hiddenInput = getHiddenInput(root);
        const preview = root.querySelector("[data-lucide-preview]");

        if (hiddenInput) hiddenInput.value = value || "";
        preview.innerHTML = value
            ? `<i data-lucide="${value}"></i>`
            : "";

        root.querySelectorAll("[data-lucide-icon]").forEach(option => {
            const isActive = option.dataset.lucideIcon === value;
            option.classList.toggle("active", isActive);
            option.setAttribute("aria-selected", isActive ? "true" : "false");
        });

        renderIcons();
    };

    const filterOptions = root => {
        const search = root.querySelector("[data-lucide-search]");
        const empty = root.querySelector("[data-lucide-empty]");
        const query = search.value.trim().toLowerCase();

        let visibleOptionCount = 0;

        root.querySelectorAll("[data-lucide-icon]").forEach(option => {
            const matches = !query || option.dataset.lucideIcon.includes(query);
            option.hidden = !matches;

            if (matches) visibleOptionCount++;
        });

        empty.classList.toggle("d-none", visibleOptionCount > 0);
    };

    const initializePicker = root => {
        if (root.dataset.lucideIconPickerInitialized === "true") return;
        root.dataset.lucideIconPickerInitialized = "true";

        const grid = root.querySelector("[data-lucide-grid]");
        const search = root.querySelector("[data-lucide-search]");
        const clear = root.querySelector("[data-lucide-clear]");
        const hiddenInput = getHiddenInput(root);

        const optionsMarkup = iconEntries.map(({ value }) =>
            `<button type="button" class="lucide-icon-picker__option" role="option" ` +
            `data-lucide-icon="${value}" aria-label="${value}" aria-selected="false" title="${value}">` +
            `<span class="lucide-icon-picker__icon" aria-hidden="true"><i data-lucide="${value}"></i></span>` +
            `<span class="lucide-icon-picker__label">${value}</span>` +
            `</button>`).join("");

        grid.innerHTML = optionsMarkup;

        grid.addEventListener("click", event => {
            const option = event.target.closest("[data-lucide-icon]");
            if (!option) return;

            updateSelection(root, option.dataset.lucideIcon);
        });

        search.addEventListener("input", () => filterOptions(root));
        clear.addEventListener("click", () => {
            search.value = "";
            updateSelection(root, "");
            filterOptions(root);
        });

        updateSelection(root, hiddenInput?.value);
        filterOptions(root);
    };

    const initialize = () => {
        document.querySelectorAll("[data-lucide-icon-picker]").forEach(initializePicker);
    };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", initialize, { once: true });
    }
    else {
        initialize();
    }
})();
