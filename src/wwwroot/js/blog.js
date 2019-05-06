$('[data-toggle="tooltip"]').tooltip();

$('#blog_editor').summernote({
    height: 300,
    blockquoteBreakingLevel: 0,
    codermirror: {       
        matchBrackets: true,
        lineNumbers: true,
        theme: 'monokai',
        mode: 'text/x-csharp',
        //mode: 'text/html',+       
    }
});

$('.comment_editor').summernote({
    airMode: true,
    disableDragAndDrop: true,
    shortcuts: false,
    placeholder: 'Type message...',
    popover: {
        air: []
    }
});

$('.delete-item').click((e) => {
    let result = confirm('Do you want to delete this item? \nWARNING! this non refundable operation!');
    if (result == false) {
        e.preventDefault()
    }
});

function showReplyCommentBox(commentId = '') {
    collapseElement(`#${commentId}.card-footer`);
    $(`button#${commentId}`).hide();
    $(`#${commentId}.reply-commentbox`).removeClass('d-none');
}

function hideReplyCommentBox(commentId = '') {
    $(`button#${commentId}`).show();
    $(`#${commentId}.reply-commentbox`).addClass('d-none');
}

function collapseElement(targetElement = '') {
    if ($(targetElement).hasClass('show')) {
        $(targetElement).collapse('hide').removeClass('show');
    }
    else {
        $(targetElement).collapse('show').addClass('show');
    }
}