(function () {
    window.kentico.pageBuilder.registerInlineEditor("dropdown-editor", {
        init: function (options) {
            var editor = options.editor;
            var dropdown = editor.querySelector(".dropdown-selector");
            var update = editor.dataset.updateOnInit;

            // Update property in a state with a validated value from the model to keep consistent values in properties.
            if (update == "True") {
                var resetEvent = new CustomEvent("updateProperty", {
                    detail: {
                        value: dropdown.value,
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