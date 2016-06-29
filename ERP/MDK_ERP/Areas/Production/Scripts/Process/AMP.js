$(document).ready(function () {
    GetList();
});
GetList = function () {
    var attr = _attrCRUD();
    attr.url = {
        "Read": "Production/Process/getListAmp",
        "Delete": "Production/Process/AmpDelete",
        "Update": "Production/Process/EditAmp"
    };
    attr.dataTable.columns = [
        { "data": "Produk" },
        { "data": "DateUse", "className": "text-center" },
        { "data": "Operator" },
        { "data": "Shift" },
        { "data": "PackageName" },
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
                return formatShortDate(data);
            },
            "targets": 1
        }
    ];
    attr.dataTable.bSort = false;
    fnGetList(attr);
}