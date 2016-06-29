/*
  

*/

$(function () {
    $.ajaxSetup({
        error: function (jqXHR, exception) {
            if (jqXHR.status == 404) {
                alert('Request page not found [404]');
            } else if (jqXHR.status == 500) {
                alert('Terjadi Kesalahan Server [500].');
            } /*else if (exception === 'parsererror') {
                alert('Requested JSON parse failed.');
            }*/
            else if (exception === 'timeout') {
                alert('Time out error.');
            } else if (exception === 'abort') {
                alert('Ajax request aborted.');
            } else {
                alert('Uncaught Error.\n' + jqXHR.responseText);
            }
        },
        complete: function (jqXHR, exception) {
            if (jqXHR.status == 302) {
                location.href = jqXHR.getResponseHeader("Location");
            }
        
        }
    });
});

ajax_error_callback = function (xhr, status, data) {
    $.notifyClose();
    var stat = parseInt(xhr.status);
    if (stat == 0) return;
    $.notify({
        icon: 'fa fa-exclamation-triangle',
        title: status,
        message: xhr.responseText
    }, {
        type: "warning",
        newest_on_top: true,
        delay: 0,
        allow_dismiss:true
    });
};

/*NOTIFICATION*/
var prenotification = null;
showSavingNotification = function(title)
{
    if(title==null)
    {
        title="Menyimpan data";
    }
    
    prenotification = $.notify({
        icon: "fa fa-floppy-o",
        title: title,
        message: "Melakukan proses. Jangan tutup halaman ini."
    }, {
        allow_dismiss: false,
        delay: 0,
        newest_on_top: true

    });
}

showNotificationSaveError = function (data, title) {
    console.log(data);
    $.notifyClose();
    if (!title)
        title = "Terjadi kesalahan";

    if (!data.errors) {
        $.notify({
            icon: 'fa fa-exclamation-triangle',
            title: "Terjadi kesalahan.",
            message: data
        }, {
            type: "info",
            newest_on_top: true,
            delay: 0
        });
        return;
    }
    var ul = "<ul>"; ;
    $(".has-error").removeClass("has-error");
    for (var i = 0; i < data.errors.length; i++) {
        $("#" + data.errors[i].Key).parent().parent().addClass("has-error");
        for (var j = 0; j < data.errors[i].Value.length; j++) {
            ul += "<li>" + data.errors[i].Value[j] + "</li>";
        }
    }
    ul += "</ul>";

    $.notify({
        icon: 'fa fa-exclamation-triangle',
        title: title,
        message: ul
    }, {
        type: "warning",
        newest_on_top: true,
        delay: 3000
    });
}

showNotification = function (message) {
    $.notifyClose();
    $.notify({
        icon: 'fa fa-floppy-o',
        title: "Notification.",
        message: message
    }, {
        type: "info",
        newest_on_top: true,
        delay: 3000
    });
}

showFailedNotification = function (message) {
    $.notifyClose();
    $.notify({
        icon: 'fa fa-floppy-o',
        title: "Error.",
        message: message
    }, {
        type: "error",
        newest_on_top: true,
        delay: 3000
    });
}

deleteComfirmModal = function (_callback) {
    bootbox.confirm({
        //size: 'small', //large
        message: "<h4>Apakah Anda yakin ingin menghapus?</h4> Anda tidak akan dapat memulihkan data yang telah terhapus!",
        callback: _callback
    });
}

function goBack() {
    window.history.back();
}