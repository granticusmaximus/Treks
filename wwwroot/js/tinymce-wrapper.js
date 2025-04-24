export function initEditor(editorId, content, uploadUrl) {
    tinymce.init({
        selector: `#${editorId}`,
        height: 300,
        plugins: 'image code link lists media table',
        toolbar: 'undo redo | formatselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | image media link',
        automatic_uploads: true,
        images_upload_url: uploadUrl,
        setup: function (editor) {
            editor.on('init', function () {
                editor.setContent(content || '');
            });
        }
    });
}

export function getEditorContent(editorId) {
    return tinymce.get(editorId)?.getContent() || '';
}

export function setEditorContent(editorId, content) {
    tinymce.get(editorId)?.setContent(content || '');
}