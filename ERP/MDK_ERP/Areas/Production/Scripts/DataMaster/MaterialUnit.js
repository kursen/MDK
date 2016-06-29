//*************************
//    View: /DataMaster/MaterialUnit
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
        "Read": "Production/DataMaster/Unit_GetList",
        "Delete": "Production/DataMaster/Unit_Delete"
    };
    attr.dataTable.columns = [
        { "data": "Symbol" },
        { "data": "Unit" },
        {
            "className": "text-right", "data": "Ratio", "mRender": function (data) { return numberFormat(data) }
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
    attr.dataTable.bPaginate = false;

    fnGetList(attr);
}
fnDel = function () {
    fnReset();
    $("#ID").remove();
}