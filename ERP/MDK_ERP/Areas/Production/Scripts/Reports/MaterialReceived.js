//*************************
//    View: /AMPProcess/Index
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {

    datePickerLinked_ByDate($("#datetimepicker1"), $("#datetimepicker2"));
    GetList();
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
            "data": null,
            "defaultContent": "",
            "className": "td-center"
        },
        {
            "data": null,
            "defaultContent": "",
            "className": "td-center"
        },
        {
            "data": null,
            "defaultContent": "",
            "className": "td-center"
        },
        {
            "data": null,
            "defaultContent": "-",
            "className": "td-center"
        },
        {
            "data": null,
            "defaultContent": "-"
        },
        {
            "data": null,
            "defaultContent": "-"
        },
        {
            "data": null,
            "defaultContent": ""
        },
        {
            "data": null,
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
