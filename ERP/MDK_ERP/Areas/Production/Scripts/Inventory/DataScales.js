//*************************
//    View: /DataMaster/DataScales
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
    
    GetList();
});

GetList = function () {
    var attr = _attrCRUD();
    attr.url = {
        "Read": "Production/Stock/Scales_GetList",
        "Delete": "Production/Stock/Scales_Delete",
        "Update": "Production/Stock/DataScalesOut_"
    };
    /*attr.dataTable.ajax.data = function (d) {
    d.startDate = $("#datetimepicker1 input").val(),
    d.endDate = $("#datetimepicker2 input").val()
    }*/
    attr.dataTable.columns = [
        {
            "data": "JamMasuk",
            "className": "td-center"
        },
        {
            "data": "NoPolisi",
            "className": "td-center"
        },
        { "data": "Sopir" },
        { "data": "NamaPeru" },
        { "data": "Material" },
        {
            "data": "Berat1",
            "className": "td-right"
        },
        { "data": "Keterangan" },
        {
            "data": "Clerk1",
            "className": "td-center"
        },
        {
            "className": "action",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div align='center'>" +
                "  <button class='edit2 icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>" +
                "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                "</div>"
        }
    ];
    attr.dataTable.columnDefs = [
        {
            "render": function (data, type, row) {
                return formatCompleteDate(data);
            },
            "targets": 0
        },
        {
            "render": function (data, type, row) {
                return numberFormat(data);
            },
            "targets": 5
        }
    ];
    //attr.dataTable.dom = '<"top">rt<"bottom"p><"clear">';

    fnGetList(attr);
}