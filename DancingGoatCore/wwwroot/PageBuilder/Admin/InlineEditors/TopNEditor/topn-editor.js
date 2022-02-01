(function () {
    window.kentico.pageBuilder.registerInlineEditor("topn-editor", {
        init: function (options) {
            var editor = options.editor;
            var input = editor.querySelector("input");
            var defaultValue = 10;
            var maxValidValue = 2147483647;
            input.addEventListener("change", function () {
                if (!/^\d+$/.test(input.value) || parseInt(input.value) > maxValidValue) {
                    input.value = defaultValue;
                }

                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: parseInt(input.value),
                        name: options.propertyName,
                        refreshMarkup: true,
                    }
                });
                editor.dispatchEvent(event);
            });


        }
    });
})();
