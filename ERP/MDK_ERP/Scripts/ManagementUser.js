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
        "Read": "UserManagement/getUserData",
        "Delete": "UserManagement/Delete",
        "ActivateUser": "UserManagement/ActivateToggle"
    };
    attr.dataTable.columns = [
        {
            "className": "",
            "data": "UserName"
        },
        {
            "className": "action",
            "data": "IsLock",
            "bSortable": false
        }
    ];
    attr.dataTable.columnDefs = [
        {
            "render": function (data, type, row) {
                return "<div align='center'>" +
                    //"  <button class='icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button> " +
                    (
                        data == true ? "<button class='icon-unlock btn btn-info btn-xs' data-toggle='tooltip' data-placement='bottom' title='Buka Kunci User'></button>"
                        : "<button class='icon-lock btn btn-warning btn-xs' data-toggle='tooltip' data-placement='bottom' title='Kunci User'></button>"

                    ) +
                    "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                    "</div>"
            },
            "targets": 1
        }
    ];

    fnGetList(attr);
}






