﻿$("[data-toggle=\"tooltip\"]").tooltip();

$(".delete-item").click((e) => {
    const result = confirm("Do you want to delete this item? \nWARNING! this non refundable operation!");
    if (result === false) {
        e.preventDefault();
    }
});

function showReplyCommentBox(commentId = "") {
    collapseElement(`#${commentId}.card-footer`);
    $(`button#${commentId}`).hide();
    $(`#${commentId}.reply-commentbox`).removeClass("d-none");
}

function hideReplyCommentBox(commentId = "") {
    $(`button#${commentId}`).show();
    $(`#${commentId}.reply-commentbox`).addClass("d-none");
}

function collapseElement(targetElement = "") {
    if ($(targetElement).hasClass("show")) {
        $(targetElement).collapse("hide").removeClass("show");
    }
    else {
        $(targetElement).collapse("show").addClass("show");
    }
}