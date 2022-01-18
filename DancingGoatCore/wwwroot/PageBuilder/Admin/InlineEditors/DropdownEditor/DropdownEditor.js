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
            // Update property in a state with a validated value from the model to keep consistent values in properties
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