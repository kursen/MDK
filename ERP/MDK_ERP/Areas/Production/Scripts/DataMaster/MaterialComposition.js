//*************************
//    View: /DataMaster/MaterialComposition
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
var tempOption;
var selectComp;
$(document).ready(function () {

    GetList();

    $('#btn-submit').attr('disabled', 'disabled');
    $('.sum-box').hide();
    $('#IDMaterial').change(function () {
        var obj = $(this);
        if ($(this).val() != '') {
            $('.sum-box, .measurement').show();
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/Production/DataMaster/getMeasurementUnit",
                data: { IdMaterial: $(this).val() },
                success: function (get) {
                    $('.measurement').text(get.data.Symbol);
                    return true;
                },
                error: function (xhr, status, data) {
                    alert("Failed" + xhr.status + status + data)
                }
            });
        } else {
            $('.sum-box, .measurement').hide();
        }
    });

    $('[data-toggle="modal"]').click(function () {
        $('#modal-Data1').modal('show');
        selectComp = $('#dropdown').clone().addClass("select2");
        tempOption = 0;
        $('.sum-box, .measurement').hide();
        fnReset($('#modal-Data1').find('form').attr('id'));
        $('#addcomp').removeAttr('disabled');
        $('#tb_comp tbody').empty();
    });

    //untuk update komposisi
    fnInsert('#form-Data1', '#modal-Data1');

    $('#addcomp').click(function () {
        $('#btn-submit').attr('disabled', 'disabled');
        //        var SelectComp = $('#dropdown drl').find("select option[value='" + tempOption + "']");
        selectComp.find("select option[value='" + tempOption + "']").remove();
        var strHtml = '';
        var tBody = $('#tb_comp tbody');
        strHtml += "<tr>";
        strHtml += '<td id="komp">' + '<input type="hidden" id="IDMaterialComposition" name="IDMaterialComposition"/>' + selectComp.html() + '</td>';
        strHtml += "<td id='jlh'><div class='input-group'>" +
                    $('#AmountInput').html() +
                   "</div></td>";
        strHtml += '<td>';
        strHtml += '<a href=javascript:void(0) class="icon-paste btn btn-success btn-xs" data-toggle="tooltip" data-placement="right" title="Simpan"></a>';
        strHtml += '<a style="display:none;" href=javascript:void(0) class="icon-edit btn btn-primary btn-xs" data-toggle="tooltip" data-placement="right" title="Edit"></a>';
        strHtml += '&nbsp;<a href=javascript:void(0) class="icon-remove btn btn-danger btn-xs" data-toggle="tooltip" data-placement="right" title="Hapus"></a></td>';
        strHtml += "</tr>";
        tBody.append(strHtml);
        //$('.spin').spinner();
        addHandler(tBody);
        $(this).attr('disabled', 'disabled');
    });
    function addHandler(tBody) {
        $(tBody).find('.icon-paste').click(function () {
            $('#addcomp').removeAttr('disabled');
            var InputSatuan = $(this).parent().parent().find("select");
            var InputWeight = $(this).parent().parent().find('.amount');
            var satuan = InputSatuan.val();
            var weight = InputWeight.val();
            //tempOption = satuan;
            if (satuan != 0) {
                if (weight != '') {
                    $('#btn-submit').removeAttr('disabled');
                    $(this).parent().parent().find('#IDMaterialComposition').val(satuan);
                    tempOption = $(this).parent().parent().find('#IDMaterialComposition').val();
                    InputSatuan.attr('disabled', 'disabled');
                    InputWeight.attr('readonly', true);
                    $(this).parent().parent().find(".icon-edit").show();
                    $(this).hide();
                } else {
                    swal('Error ', 'Isi Jumlah(Angka)', 'error');
                }
            } else {
                swal('Error ', 'Pilih Komposisi', 'error');
            }
        });

        $('#form-Data1 .icon-edit').click(function () {
            $('#btn-submit').attr('disabled', 'disabled');
            var obj = $(this).parent().parent();
            obj.find('#IDMaterialComposition').val('');
            obj.find('select').removeAttr('disabled');
            obj.find('.amount').removeAttr('readonly');
            $(this).hide();
            $(this).parent().parent().find('.icon-paste').show();
        });
        $('#form-Data1 .icon-remove').click(function () {
            $('#btn-submit').removeAttr('disabled');
            $(this).parent().parent().remove();
            $('#addcomp').removeAttr('disabled');
        });
    }

    $(".numeric").numeric({ decimal: "," });
    $('#tb_Data tbody')
    .on('click', '.icon-edit', function () {
        var a = $(".numeric").val();
        $(".numeric").val(a.replace('.',','));
    })
});

GetList = function () {
    var attr = _attrCRUD();
    attr.url = {
        "Read": "Production/DataMaster/MC_GetList",
        "Delete": "Production/DataMaster/MC_Delete"
    };
    attr.usingId.modalId = "#modal-Data";
    attr.dataTable.columns = [
        { "data": "produk" },
        { "data": "Composition" },
        {
            "className": "text-right", "data": "Amount", "mRender": function (data) { var d = parseFloat(data); return d.toFixed(2).replace('.',','); }
        },
        {
            "className": "text-left", "data": "Unit"
        },
        {
            "className": "text-right", "data": "ConversionAmount", "mRender": function (data) { var d = parseFloat(data); return d.toFixed(2).replace('.', ','); }
        },
        {
            "className": "text-left", "data": "CompositionUnit"
        },
        {
            "className": "text-center", "data": "MachineType"
        },
        {
            "className": "action",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div align='center'>" +
                "  <button class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>" +
                "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bLengthChange = false;
    attr.dataTable.bSort = false;
    attr.dataTable.bPaginate = false;

    fnGetList(attr);

    $('#tb_Data').rowGrouping({
        "bExpandableGrouping": true,
        "iGroupingColumnIndex": 0,
        "iExpandGroupOffset": -1,
        //"bExpandSingleGroup": true,
        "fnGroupLabelFormat": function (label) {
            return " " + label + "<div class='pull-right'>" +
                "  <button class='icon-trash removeAll btn btn-warning btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                "</div>";
        }
    });

    $('#tb_Data tbody')
    //for remove All
        .on('click', '.removeAll.icon-trash', function () {
            var Data = GenTable.row($(this).parents('tr').next()).data();
            swal({
                title: "Apakah Anda yakin ingin menghapus?",
                text: "Anda tidak akan dapat memulihkan data yang telah terhapus!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ya",
                cancelButtonText: "Tidak",
                closeOnConfirm: false
            },
            function () {
                fnDelete(Data.IDMaterial, "Production/DataMaster/MC_DeleteAll");
            });
        });

    setTimeout(function () {
        $(".group-item-expander").click();
    }, 600);
}