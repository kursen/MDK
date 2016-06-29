var jcrop_api, boundx, boundy;


$(function () {
    'use strict';

    $('#fileupload').fileupload({
        dataType: 'json',
        add: function (e, data) {

            $("#placeholder_upload").text("Sedang mengunggah...");
            data.formData = { userid: $("#userid").val() };
            data.submit();
        },
        done: function (e, data) {
            loadimage(true)
            $("#placeholder_upload").text('');

        }
    });

    loadimage();
    initFileupload();





});

initFileupload = function () {
    $('#fileupload').fileupload('option', {
        url: '/HRD/Employee/UploadPhoto',
        maxFileSize: 5000000,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        process: [
                {
                    action: 'load',
                    fileTypes: /^image\/(gif|jpeg|png)$/,
                    maxFileSize: 20000000 // 20MB
                },
                {
                    action: 'resize',
                    maxWidth: 1440,
                    maxHeight: 900
                },
                {
                    action: 'save'
                }
            ]
    });

}



initCropping = function () {

    if (jcrop_api) {
        jcrop_api.destroy();
        $("#btncrop").addClass("disabled");
        $("#btncrop").prop("disabled", true);
        $("#btninit").html("<i class='icon-wrench icon-white'></i>  Edit");
        $("#imgTarget").css("visibility", "visible");

        $('#fileupload').fileupload('enable');
        $('#fileupload').prop("disabled", false);
        $("#btncrop").removeClass("disabled");
        jcrop_api = false;
    } else {

        $('#imgTarget').Jcrop({
            onChange: updatePreview
        , onSelect: updatePreview
        , aspectRatio: 150 / 200
        }
    , function () {
        // Use the API to get the real image size
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];
        // Store the API in the jcrop_api variable
        jcrop_api = this;
        jcrop_api.animateTo([0, 0, 150, 200]);
    });
        $("#btncrop").removeClass("disabled");
        $("#btninit").html("<i class='icon-off icon-white'></i>  Batal");
        $("#btncrop").prop("disabled", false);
        $('#fileupload').fileupload('disable');
        $('#fileupload').prop("disabled", true);
        $("#fileupload").addClass("disabled");
    }
}

function updatePreview(c) {
    $("#x").val(c.x);
    $("#y").val(c.y);
    $("#w").val(c.w);
    $("#h").val(c.h);
};

function loadimage(reload) {


    var userid = $("#userid").val();
    if (userid) {
        var d = new Date();
		if (reload)
		{
            window.location.href = "/HRD/Employee/EditPhoto/?id=" + userid + "&t=" + d.getTime();
            return
        }


        $("#imgTarget").attr("src", "/HRD/Employee/employeephoto/?id=" + userid + "&t=" + d.getTime());
    }
};

function cropPhoto() {
    if (confirm("Crop foto sudah benar?")) {
        var data = $("#frmEditPhoto").submit();
    }
}


var navigateToDetailPage = function () {
    var userid = $("#userid").val();
    window.location = "/HRD/Employee/Detail/" + userid;

};