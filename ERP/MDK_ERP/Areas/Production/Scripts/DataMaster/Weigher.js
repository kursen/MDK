//*************************
//    View: /DataMaster/Weigher
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
        "Read": "Production/DataMaster/Weigher_GetList",
        "Delete": "Production/DataMaster/Weigher_Delete"
    };
    attr.dataTable.columns = [
        { "data": null },
        { "data": null },
        { "data": null },
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

    fnGetList(attr);
}