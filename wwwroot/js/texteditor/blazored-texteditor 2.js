window.QuillFunctions = {
    createQuill: function (quillElement, dotNetHelper) {
        const quill = new Quill(quillElement, {
            modules: {
                toolbar: [
                    ['bold', 'italic', 'underline'],
                    [{ list: 'ordered' }, { list: 'bullet' }],
                    ['link']
                ]
            },
            theme: 'snow'
        });

        quill.on('text-change', function () {
            dotNetHelper.invokeMethodAsync("QuillContentChanged", quill.root.innerHTML);
        });

        return quill;
    },

    getQuillContent: function (quill) {
        return quill.root.innerHTML;
    },

    loadQuillContent: function (quill, content) {
        quill.root.innerHTML = content;
    },

    disableQuillEditor: function (quill) {
        quill.enable(false);
    },

    enableQuillEditor: function (quill) {
        quill.enable(true);
    }
};