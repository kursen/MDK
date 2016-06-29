//*************************
//    View: Production/Project/Index
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
    GetList();
    datePickerLinked_ByDate($("#dtpStartDate"), $("#dtpEndDate"));
    $('[data-toggle="modal"]').click(function () {
        fnReset();
        $('#Id').remove();
    });
    $('#addDetail').click(function () {
        $('#btn-submit').attr('disabled', 'disabled');
        $(this).attr('disabled', 'disabled');
        var strHtml = '';
        var tBody = $('#tb_DetailProject tbody');
        strHtml += "<tr>";
        strHtml += '<td style="visibility: hidden"><input id="IdRoad" name="IdRoad" type="hidden" value=""></td>';
        strHtml += '<td><input type="text" class="form-control" name="RoadName" style="width:100%;margin-left:0%;"/></td>';
        strHtml += '<td><input type="text" class="spin form-control" name="Length" value = "0" style="width:80%;margin-left:10%;"/></td>';
        strHtml += '<td>';
        strHtml += '<a href=javascript:void(0) class="icon-ok btn btn-success btn-xs" data-toggle="tooltip" data-placement="right" title="Simpan"></a>';
        strHtml += '<a href=javascript:void(0) class="icon-pencil btn btn-info btn-xs" style="display:none" data-toggle="tooltip" data-placement="right" title="Edit"></a>';
        strHtml += '&nbsp;<a href=javascript:void(0) class="icon-remove btn btn-danger btn-xs" data-toggle="tooltip" data-placement="right" title="Hapus"></a></td>';
        strHtml += "</tr>";
        tBody.append(strHtml);
        $('.spin').spinner();
        tBody.find('.icon-ok').click(function () {
            var roadNameObj = $(this).parents('tr').find('[name="RoadName"]');
            var lengthObj = $(this).parents('tr').find('[name="Length"]');

            var roadNameVal = roadNameObj.val();
            var lengthVal = lengthObj.val();

            if (roadNameVal == '' || lengthVal == '') {
                // alert('Kosong tu lae!!!');
            } else {
                roadNameObj.attr('readonly', true);
                lengthObj.attr('readonly', true);

                $(this).parents('tr').find('.icon-pencil').show();
                $(this).hide()
                $('#addDetail').removeAttr('disabled');
                $('#btn-submit').removeAttr('disabled');
            }
        });
        tBody.find('.icon-pencil').click(function () {
            var roadNameObj = $(this).parents('tr').find('[name="RoadName"]');
            var lengthObj = $(this).parents('tr').find('[name="Length"]');

            roadNameObj.removeAttr('readonly');
            lengthObj.removeAttr('readonly');

            $(this).hide()
            $(this).parents('tr').find('.icon-ok').show();
        });
        tBody.find('.icon-remove').click(function () {
            $(this).parents('tr').remove();
            $('#addDetail').removeAttr('disabled');
            $('#btn-submit').removeAttr('disabled');
        });
    });
});

GetList = function () {
    var attr = _attrCRUD();
    /*attr.dataTable.ajax.data = function (d) {
        d.startDate = $("#dtpStartDate input").val(),
        d.endDate = $("#dtpEndDate input").val()
    }*/
    attr.url = {
        "Read": "Production/Project/GetList",
        "Delete": "Production/Project/Delete"
    };
    attr.dataTable.columns = [
        {
            "data": "NoProject",
            "className": "td-center"
        },
        {
            "data": "PackageName"
        },
        {
            "data": "StartDate",
            "className": "text-center"
        },
        {
            "data": "EndDate",
            "className": "text-center"
        },
        {
            "data": "AgentName",
            "className": "text-center"
        },
        { "data": "Description" },
        {
            "bSortable": false,
            "defaultContent": "<div align='center'><button id='Edit' class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>\
        <button id='Delete' class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button></div>"
        }
    ];
    attr.dataTable.columnDefs = [
        {
            "render": function (data, type, row) {
                return formatShortDate(data);
            },
            "targets": 2
        },
        {
            "render": function (data, type, row) {
                return formatShortDate(data);
            },
            "targets": 3
        }
    ]

    fnGetList(attr);
}
