(function () {
    window.kentico.pageBuilder.registerInlineEditor("dropdown-editor", {
        init: function (options) {
            var editor = options.editor;
            var dropdown = editor.querySelector(".dropdown-selector");
            var reset = editor.querySelector(".dropdown-value").dataset.reset;
            
            // Reset property to keep consistent values in properties.
            if (reset != null) {
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