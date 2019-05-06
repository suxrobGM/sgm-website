function checkExtension(event) {
    let file = document.querySelector("#select_photo");
    if (/\.(jpe?g|png|gif)$/i.test(file.files[0].name) === false) {
        $("#upload_fail_text").text("This is not valid image file");        
    }
    else {
        loadFile(event);
        $("#upload_fail_text").text("");
    }
}

function loadFile(event) {
    var reader = new FileReader();
    reader.onload = () => {
        $("#profile_photo").attr("src", reader.result);
    };
    reader.readAsDataURL(event.target.files[0]);
}