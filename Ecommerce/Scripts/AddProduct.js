$(function () {
    var x = 1;
    $(".btn-success").hide();
    $(".btn-success").click(function () {
        $("#add-image").append("<label for='Image'>Image</label><input type='file' class='form-control' name='imageFiles["+x+"]'  id='image-files-"+x+"'>");
        x++;
        $(".btn-success").hide();
    })

    $("#add-image,#image-files-" + x).on('change', function () {       
        $(".btn-success").show();
    });
});

