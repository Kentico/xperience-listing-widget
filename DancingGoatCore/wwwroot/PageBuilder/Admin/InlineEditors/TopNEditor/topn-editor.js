(function () {
    window.kentico.pageBuilder.registerInlineEditor("topn-editor", {
        init: function (options) {
            var editor = options.editor;
            var input = editor.querySelector("input");
            input.addEventListener("change", function () {
                if (!/^\d+$/.test(input.value) || parseInt(input.value) > 2147483647) {
                    input.value = 10;
                }

                var event = new CustomEvent("updateProperty", {
                    detail: {
                        value: input.value,
                        name: options.propertyName,
                        refreshMarkup: true,
                    }
                });
                editor.dispatchEvent(event);
            });


        }
    });
})();
