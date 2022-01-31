(function () {
    window.kentico.pageBuilder.registerInlineEditor("dropdown-editor", {
        init: function (options) {
            var editor = options.editor;
            var dropdown = editor.querySelector(".dropdown-selector");
            var reset = editor.dataset.reset === "true";

            // Reset property to keep consistent values in properties.
            if (reset) {
                var resetEvent = new CustomEvent("updateProperty", {
                    detail: {
                        value: null,
                        name: options.propertyName,
                        refreshMarkup: false,
                    }
                });
                editor.dispatchEvent(resetEvent);
            }

            dropdown.addEventListener("change", function () {
                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: dropdown.value,
                        name: options.propertyName,
                    }
                });
                editor.dispatchEvent(event);
            });
        }
    });
})();