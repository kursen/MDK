//*************************
//    View: /DataMaster/Machines
//*************************


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/
$(document).ready(function () {
    GetList();
    $('[data-toggle="modal"]').click(function () {
        $('#ID').remove();
        fnReset();
    });
});

GetList = function () {
    var attr = _attrCRUD();
    attr.url = {
        "Read": "Production/DataMaster/GetMachineTypeList",
        "Delete": "Production/DataMaster/DeleteMachineType"
    };
    attr.dataTable.columns = [
        { "data": "MachineType" },
        { "data": "Description" },
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
