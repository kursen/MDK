//*************************
//    View: /ProjectManager/CompanyInfo
//*************************

GetList = function () {
    var attr = _attrCRUD()
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = true;
    attr.dataTable.order = [[1, 'asc']];
    attr.url = {
        "Read": "ProjectManagement/CompanyInfo/getData",
        "Delete": "CompanyInfo/Delete",
        "Update": "ProjectManagement/CompanyInfo/Create",
        "Details": "ProjectManagement/CompanyInfo/Details",
        "RefreshTable": function () {
            return "/ProjectManagement/CompanyInfo/getData?name=" + $('#fltName').val()
        }
    };
    //attr.url.RefreshTable = attr.url.RefreshTable
    attr.dataTable.columns = [
        {
            "data": null,
            "bSortable":false,
            "className":"text-right"
        },
        {
            "data": "Name"
        },
        {
            "data": "Alias"
        },
        {
            "data": "City"
        },
        {
            "className": "action text-center",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                "  <button class='detail2 btn btn-warning btn-xs' title='Detail'><i class='fa fa-list-alt'></i></button>" +
                "  <button class='edit2  btn btn-primary btn-xs' title='Edit'><i class='fa fa-edit'></i></button>" +
                "  <button class='delete btn btn-danger btn-xs' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;

    fnGetList(attr);
}


/*--------------------------------------
WHEN WINDOWS READY TO OPEN
--------------------------------------*/

$(document).ready(function () {
    GetList();
});