//*************************
//    View: /AMPProcess/Index
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {

    datePickerLinked_ByDate($("#datetimepicker1"), $("#datetimepicker2"));
    GetList();

    $('#btn-print').on('click', function () {
        window.location.href = parseUrl("Production/Reports/PrintWeightScales?startDate=" + $("#datetimepicker1 input").val() + "&endDate=" + $("#datetimepicker2 input").val());
    });
});

GetList = function () {
    var attr = _attrCRUD();
    attr.url = {
        "Read": "Production/Reports/GetListWeightScales"
    };
    attr.dataTable.ajax.data = function (d) {
        d.startDate = $("#datetimepicker1 input").val(),
        d.endDate = $("#datetimepicker2 input").val()
    }
    attr.dataTable.bSort = false;
    attr.dataTable.columns = [
        {
            "data": "NoRecord",
            "defaultContent": "",
            "className": "td-center"
        },
        {
            "data": "Masuk",
            "defaultContent": "",
            "className": "td-center"
        },
        { 
            "data": "Keluar",
            "defaultContent": "",
            "className": "td-center"
        },
        {
            "data": "NoPolisi",
            "defaultContent": "-",
            "className": "td-center"
        },
        {
            "data": "Perusahaan",
            "defaultContent": "-"
        },
        {
            "data": "Barang",
            "defaultContent": "-"
        },
        {
            "data": "Keterangan",
            "defaultContent": ""
        }
    ];
        attr.dataTable.columnDefs = [
        {
            "render": function (data, type, row) {
                return formatCompleteDate(data);
            },
            "targets": 1
        },
        {
            "render": function (data, type, row) {
                return formatCompleteDate(data);
            },
            "targets": 2
        }
    ]

    fnGetList(attr);
}
