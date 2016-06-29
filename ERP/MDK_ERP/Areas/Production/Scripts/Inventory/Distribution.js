//*************************
//    View: /DataMaster/Distribution
//*************************


$(function () {

    datePickerLinked_ByDate($("#datetimepicker1"), $("#datetimepicker2"));

    ReceivedTable();

    DeliveryTable();
});

// -- Pengiriman --
DeliveryTable = function () {
    var attr = varAttr();
    attr.dataTable.columns = [
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": ''
        },
        { "data": "Material" },
        {
            "data": "DateDist",
            "className": "text-center"
        },
        {
            "data": "Netto",
            "className": "td-right"
        },
        { "data": "Place" },
        { "data": "Company" }
    ];
    attr.usingId.tableId = "#tb_Delivered";
    attr.url.Read = "Production/Stock/GetDeliveredData";
    fnGetList(attr, true, "deliveredDist");
}

// -- Penerimaan --
ReceivedTable = function () {
    var attr = varAttr();
    attr.usingId.tableId = "#tb_Received";
    attr.url.Read = "Production/Stock/GetReceivedData";
    fnGetList(attr, true, "receivedDist");
}

// >> Declare Variable <<
varAttr = function () {
    var attr = _attrCRUD();
    attr.dataTable.ajax.data = function (d) {
        d.startDate = $("#datetimepicker1 input").val(),
        d.endDate = $("#datetimepicker2 input").val()
    }
    attr.dataTable.columns = [
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": ''
        },
        { "data": "Material" },
        {
            "data": "DateDist",
            "className": "text-center"
        },
        {
            "data": "Netto",
            "className": "td-right"
        },
        { "data": "Company" },
        { "data": "Place" }
    ];
    attr.dataTable.columnDefs = [
        {
            "render": function (data, type, row) {
                return formatCompleteDate(data);
            },
            "targets": 2
        },
        {
            "render": function (data, type, row) {
                return numberFormat(data);
            },
            "targets": 3
        }
    ];
    attr.dataTable.order = [[1, "asc"]];
    attr.dataTable.bSort = false;
    attr.dataTable.autoWidth = false;

    return attr;
}

