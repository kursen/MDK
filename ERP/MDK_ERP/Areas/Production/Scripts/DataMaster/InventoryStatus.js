//*************************
//    View: /DataMaster/Inventory Status
//*************************


/*--------------------------------------
    WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
    GetList();
});

GetList = function () {
    var attr = _attrCRUD()
    attr.url = {
        "Read": "Production/DataMaster/getInventoryData",
        "Delete": "Production/DataMaster/IS_Delete"
    };
    attr.dataTable.columns = [
        {
            "ClassName": "",
            "data": "StatusName"
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






