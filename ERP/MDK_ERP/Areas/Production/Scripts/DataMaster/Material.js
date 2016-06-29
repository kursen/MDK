//*************************
//    View: /DataMaster/Material
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
        "Read": "Production/DataMaster/GetListMaterial",
        "Delete": "Production/DataMaster/DeleteMaterial",
        "Detail": "Production/DataMaster/checkRasio",
        "Update": "Production/DataMaster/Material_"
    };
    attr.usingId.modalId2 = "#modal-Data1"
    attr.dataTable.order = [[0, 'asc']];
    attr.dataTable.columns = [
                {
                    "data": "Code",
                    "className": "td-center"
                },
                { "data": "Name" },
                { "data": "Type" },
                { "data": "isInventory",
                    "render": function (data, type, row) {
                        if (data == true) {
                            return '<i class = "icon-check"></i>';
                        } else {
                            return '<i class = "icon-uncheck"></i>';
                        }
                        return data;
                    },
                    "className": "dt-body-center"
                },
                {
                    "data": "Unit",
                    "className": "td-center"
                },
                {
                    "data": "Stock",
                    "className": "td-right"
                },
                { "data": "MachineType" },
                {
                    //"className": "action",
                    "data": null,
                    "bSortable": false,
                    "defaultContent": "" +
                        "<div align='left'>" +
                        "  <a href='javascript:void(0)' class='edit2 icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></a>" +
                    //"  <a href ='#' class='icon-list-alt btn btn-success btn-xs' data-toggle='' data-target='' data-placement='right' title='Tambah Rasio'></a>" +
                        "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                        "</div>"
                }
            ];

    fnGetList(attr);
}