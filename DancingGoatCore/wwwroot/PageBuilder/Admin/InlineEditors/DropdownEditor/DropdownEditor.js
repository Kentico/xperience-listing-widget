(function () {
    window.kentico.pageBuilder.registerInlineEditor("dropdown-editor", {
        init: function (options) {
            var editor = options.editor;
            var dropdown = editor.querySelector(".dropdown-selector");
            dropdown.addEventListener("change", function () {
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: dropdown.value,
                        name: options.propertyName
                    }
                });
                editor.dispatchEvent(event);
            });
            // Send updated properties in drop-down init to select correct item when depending field changes.
            var event = new CustomEvent("updateProperty", {
                detail: {
                    value: dropdown.value,
                    name: options.propertyName,
                    refreshMarkup: false,
                }
            });
            editor.dispatchEvent(event);
        }
    });
})();