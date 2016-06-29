var GenTable;

var slide = function () {
    if ($('.no-drop:first').is(":hidden")) {
        $('.btn-add').removeClass('btn-success').addClass('btn-danger').html('Tutup');
    } else {
        $('.btn-add').removeClass('btn-danger').addClass('btn-success').html('<span><i class="fa fa-plus"></i></span>Tambah Baru');
    }
    $(".no-drop:first").slideToggle("slow");
}

var fnReset2 = function (objform) {
    $(objform).trigger("reset");
    $('.form-group,.col-md-5', $(objform)).removeClass("has-error");
}

fnremoveid = function (obj) {
    $(obj).find('#ID').remove();
}

$(document).ready(function () {
    $('.no-drop:first').hide();

     $('#Volume_Mask').css('width', '100px');
     $('#V_LapMask').css('width', '100px');
    // capitaList('#Materials');
     justCapital('#Unit');

     $('#BeginDate,#EndDate').change(function () {
         var bDate = $('#BeginDate').val();
         var cDate = $('#EndDate').val();
         if (cDate != '') {
             var arrbDate = bDate.split('-');
             var arrcDate = cDate.split('-');
             var fDate = new Date(arrbDate[2], arrbDate[1] - 1, arrbDate[0]);
             var sDate = new Date(arrcDate[2], arrcDate[1] - 1, arrcDate[0]);
             if (fDate < sDate) {
                 $('.form-group').removeClass('has-error');
                 $('#btnSave').removeAttr('disabled');
             } else {
                 showNotificationSaveError("Tanggal Selesai Salah!");
                 $('#btnSave').attr('disabled', 'disabled');
                 $(this).closest('.form-group').addClass('has-error');
             }
         }
     });

    var idproject = $('#IDProjectInfo').val();
    GenTable=$('#tb_Data').DataTable({
        "ajax": "/Material/getData/"+idproject,
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "columns": [
        {
            "data": "Materials"
        },
        {
            "data": "Volume", "mRender": _fnRender2DigitDecimal, "sClass":"text-right"
        },
        {
            "data": "V_Lap", "mRender": _fnRender2DigitDecimal, "sClass":"text-right"
        },
        {
            "data": "Unit", "sClass":"text-center"
        },
         {
             "data": "BeginDate", "sClass":"text-center","mRender":_fnRenderNetDate
         },
          {
              "data": "EndDate", "sClass": "text-center", "mRender": _fnRenderNetDate
          },
        {
            "className": "action text-center",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                "  <button class='edit2  btn btn-primary btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Edit'><i class='fa fa-edit'></i></button>" +
                "  <button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
                "</div>"
        }
    ]
    });

    var sBody = $('#tb_Data tbody');
    sBody.on('click', '.edit2', function () {
        //fnReset2('form-data');
         fnReset2('#form-data');
        fnremoveid('#form-data');
        var data = GenTable.row($(this).parents('tr')).data();
        $('#Materials').val(data.Materials);
        $('#ProjectInfoID').val(idproject);
         $('#V_LapMask').val(data.V_Lap);
        $('#Volume_Mask').val(data.Volume);
        $('#Unit').val(data.Unit);
        $('#BeginDate').val(formatShortDate(data.BeginDate));
        $('#EndDate').val(formatShortDate(data.EndDate));
        $("<input type='hidden' name='ID' id='ID' value='"+data.ID+"' />").appendTo('#form-data');
        if ($('.no-drop:first').is(":hidden")) {
            slide();
        }
    })
    .on('click', '.delete', function () {
        var data = GenTable.row($(this).parents('tr')).data();
        var urlDelete = "/Material/Delete";
        deleteComfirmModal(function (result) {
        if (result){
            $.ajax({
                type: 'POST',
                url: urlDelete,
                data: { id: data.ID },
                dataType: 'json',
                beforeSend: function () {
                    showSavingNotification("Sedang memproses Data!");
                },
                success: function (get) {
                    if (get.stat == 1) {
                        showNotification("Data Telah terhapus!");
                    } else {
                        showFailedNotification(get.msg)
                    }
                    GenTable.ajax.reload();
                    return false;
                },
                error: ajax_error_callback
            });
        }
        });
    });

    $(document).on('click', '.btn-add', function () {
       fnReset2('#form-data');
       fnremoveid('#form-data');
       $('#ProjectInfoID').val(idproject);
       slide();
    });

    $('#form-data').submit(function(e){
        e.preventDefault();
         var vol = $.number($("#Volume_Mask").val(), 2, ",", ".");
         var vlap = $.number($("#V_LapMask").val(), 2, ",", ".");
         $("#V_Lap").val(vlap);
         $("#Volume").val(vol);
         var url = $(this).attr('action');
         var data = $(this).serialize();
         $.ajax({
             type: 'POST',
             dataType: 'json',
             url: url,
             data: data,
             beforeSend: function () {
                 showNotification("Sedang Menyimpan Data!");
             },
             success: function (get) {
                 if (get.stat == 1) {
                     showNotification("Data Berhasil Disimpan");
                     slide();
                 } else {
                     showNotificationSaveError(get);
                 }
                 GenTable.ajax.reload();
                 return false;
             },
             error: ajax_error_callback
         });
    });
    
     //set trigger of #idFilter button to refresh
    $('body').on('click', "#filterBtn", function () {
        $('.loader').removeClass("hidden");
        fnRefreshTable('#tb_Data', GenTable.ajax.url() + "?MaterialName=" + $("#fltName").val(), function () {
        $('.loader').addClass("hidden");
        });
    });

});

