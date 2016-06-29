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
        "Read": "Production/DataMaster/GetMachineList",
        "Delete": "Production/DataMaster/DeleteMachine"
    };
    attr.dataTable.columns = [
        { "data": "MachineName" },
        { "data": "MachineType" },
        { "data": "SeriesNumber" },
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
