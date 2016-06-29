function ShowAccountPopup(hiddenInputSiteId, hiddenInputTargetName, textBoxTargetName) {
    $("#EmployeeDialogContainer").dialog({
        bgiframe: true,
        resizable: true,
        height: 350,
        width: 500,
        modal: true,
        title: "Select Employee",
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            Ok: function () {
                var id = "";
                var name = "";

                $('#EmployeeGrid tbody tr').each(function (index) {
                    if ($(this).attr('class').indexOf('selected') != -1) {
                        id = $(this).attr('id');
                        name = $(this).find("td:eq(1)").text();

                        $("#" + hiddenInputTargetName).val(id);
                        $("#" + textBoxTargetName + "Txt").val(name);
                        $("#" + textBoxTargetName).val(name);
                        $("#" + hiddenInputSiteId).val($("#DialogEmployeeSiteId").val());
                    }
                })

                if (id == "")
                    alert('Please select an employee.');
                else
                    $(this).dialog('close');
            },
            Cancel: function () {
                $(this).dialog('close');
            }
        }
    });

    var siteId = $("#" + hiddenInputSiteId).val();

    $("#DialogEmployeeSiteId").val(siteId);
    GetDepartmentBySite(siteId);
}

function GetEmployeeByDepartment(departmentId) {
    var siteId = $("#DialogEmployeeSiteId").val();
    var employeeName = $("#EmployeeName").val();

    if (siteId == "")
        siteId = "0";

    $.ajax({
        type: "POST",
        url: "/Employee/GetEmployeeByDepartment/" + siteId + "/" + departmentId + "/" + employeeName,
        success: function (msg) {
            $("#EmployeeList").html(msg);
        },
        error: function (msg) {
            debugger;
        }
    });
}

function GetDepartmentBySite(siteId) {
    ClearDepartmentDropDown();

    if (siteId == "" || siteId == "0") {
        return;
    }

    var url = "/Department/GetDepartments/" + siteId;

    $.getJSON(url, null, function (data) {
        $.each(data, function (i, department) {
            $("#DialogEmployeeDepartmentId").append("<option value=" + department.Value + ">" + department.Text + "</option>");
        });
    });

    //$("#EmployeeGrid").remove();
}

function ClearDepartmentDropDown() {
    $("#DialogEmployeeDepartmentId").find("option").remove();
    $("#DialogEmployeeDepartmentId").append("<option value='0'>-- All Department --</option>");
}

function ShowContractorPopup(hiddenInputSiteId, hiddenInputTargetName, textBoxTargetName) {   
    $("#ContractorDialogContainer").dialog({
        bgiframe: true,
        resizable: false,
        height: 350,
        width: 500,
        modal: true,
        title: "Select Contractor",
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            Ok: function () {
                var id = "";
                var name = "";

                $('#ContractorGrid tbody tr').each(function (index) {
                    if ($(this).attr('class').indexOf('selected') != -1) {
                        id = $(this).attr('id');
                        name = $(this).find("td:eq(1)").text().trim();
                     
                        $("#" + hiddenInputTargetName).val(id);
                        $("#" + textBoxTargetName).val(name);
                        $("#" + textBoxTargetName + "Txt").val(name);
                        $("#" + hiddenInputSiteId).val($("#DialogContractorSiteId").val()); 
                    }
                })

                if (id == "")
                    alert('Please select the contractor.');
                else
                    $(this).dialog('close');
            },
            Cancel: function () {
                $(this).dialog('close');
            }
        }
    });

    var siteId = $("#" + hiddenInputSiteId).val();

    $("#DialogContractorSiteId").val(siteId);
    GetContractorBySite(siteId);
}

function GetContractorBySite(siteId) {

    var contractorName = $("#ContractorName").val();

    $.ajax({
        type: "POST",
        url: "/Contractor/GetContractorList/" + siteId + "/" + contractorName,
        success: function (msg) {
            $("#ContractorList").html(msg);
        },
        error: function (msg) {
            debugger;
        }
    });
}

function ResetName(tblWrapper) {
    $("#" + tblWrapper + " input:text").val("");
    $("#" + tblWrapper + " input:text").attr("readonly", "readonly");
    $("#" + tblWrapper + " a").attr("class", "hide");
}

function ActivateField(name, type) {
    if (type == 1) {
        //$("#" + name + "EmployeeName").removeAttr("disabled");
        $("#" + name + "ShowEmployee, #" + name + "HideEmployee").attr("class", "show");
    } else if (type == 2) {
        //$("#" + name + "ContractorName").removeAttr("disabled");
        $("#" + name + "ContractorRealName").removeAttr("disabled");
        $("#" + name + "ShowContractor, #" + name + "HideContractor").attr("class", "show");
    } else if (type == 3) {
        $("#" + name + "VisitorName").removeAttr("readonly");
    } else if (type == 4) {
        $("#" + name + "ThirdPartyName").removeAttr("readonly");
    }
}

function ShowHideItem(type, name) {
    $("#" + name + type).attr("class", "show");

    if (type == "Employee") {
        $("#" + name + "Contractor").attr("class", "hide");
        $("#" + name + "Visitor").attr("class", "hide");
        $("#" + name + "ThirdParty").attr("class", "hide");
    } else if (type == "Contractor") {
        $("#" + name + "Employee").attr("class", "hide");
        $("#" + name + "Visitor").attr("class", "hide");
        $("#" + name + "ThirdParty").attr("class", "hide");
    } else if (type == "Visitor") {
        $("#" + name + "Employee").attr("class", "hide");
        $("#" + name + "Contractor").attr("class", "hide");
        $("#" + name + "ThirdParty").attr("class", "hide");
    }
    else {
        $("#" + name + "Employee").attr("class", "hide");
        $("#" + name + "Contractor").attr("class", "hide");
        $("#" + name + "Visitor").attr("class", "hide");
    }
}
