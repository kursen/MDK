//*************************
//    View: /DataMaster/Stock/InitialStock
//*************************

$(function () {
//    datePickerLinked_ByDate($("#dtpDateInput"));
    GetList();
});

GetList = function () {
    var attr = _attrCRUD();
//    attr.dataTable.ajax.data = function (d) {
//        d.DateInput = $("#dtpDateInput input").val()
//    }
    attr.url = {
        "Read": "Production/Stock/GetInitialStockList",
        "Delete": "Production/Stock/DeleteInitialStock"
    };
    attr.dataTable.columns = [
        {
            "data": "Tanggal",
            "className": "text-center"
        },
        {
            "data": "NamaMaterial"
        },
        {
            "data": "JumlahInput",
            "className": "td-right"
        },
        { "data": "Satuan" },
        { "data": "Description" },
        { 
            "bSortable":false,
            "defaultContent": "<div align='center'><button id='Edit' class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>\
            <button id='Delete' class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button></div>"
        }
    ];
    attr.dataTable.columnDefs = [
        {
            "render": function (data, type, row) {
                return formatShortDate(data);
            },
            "targets": 0
        }
    ]

    fnGetList(attr);
}
