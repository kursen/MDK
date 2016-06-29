//*************************
//    View: /ProjectManager/CompanyInfo
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
        "Read": "ProjectManagement/CompanyInfo/getData",
        "Delete": "ProjectManagement/CompanyInfo/Delete",
        "Update": "ProjectManagement/CompanyInfo/Create"
    };
    attr.dataTable.columns = [
        {
            "ClassName": "",
            "data": "Name"
        },
        {
            "ClassName": "",
            "data": "Alias"
        },
        {
            "ClassName": "",
            "data": "City"
        },
        {
            "className": "action",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div align='center'>" +
                "  <button class='icon-list-alt btn btn-warning btn-xs' data-toggle='tooltip' data-placement='bottom' title='Detail'></button>" +
                "  <button class='edit2 icon-edit btn btn-primary btn-xs' data-toggle='tooltip' data-placement='bottom' title='Edit'></button>" +
                "  <button class='icon-remove btn btn-danger btn-xs' data-toggle='tooltip' data-placement='right' title='Hapus'></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;

    fnGetList(attr);
}






