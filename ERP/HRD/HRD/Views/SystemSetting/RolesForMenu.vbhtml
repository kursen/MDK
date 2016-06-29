@ModelType ERPBase.SideBarMenu
@Code
    ViewData("Title") = "Roles For Side Menu Link"
    Dim _mainEntities = New ERPBase.MainEntities
    Dim availableRoles = Roles.GetAllRoles()
                         
                         
End Code
@Functions
    Function WriteCheckboxRole(ByVal rolename As String) As String
        Dim rvalue As String = " <div class='checkbox-inline'>" &
                        "<label >" &
                            "<input data-rolename='{0}'  {1} type='checkbox' class='rolecheckbox'>&nbsp;<i class='fa fa-square-o '></i></label></div>"
        
        
        Dim active = Model.SideBarRoles.Where(Function(m) m.RoleName.Equals(rolename)).Count > 0
        
        If active Then
            rvalue = String.Format(rvalue, rolename, "checked='checked'")
        Else
            rvalue = String.Format(rvalue, rolename, "")
        End If
        Return rvalue
    End Function
End Functions
<h2>
    Menu Label: @Model.Label</h2>
<h3>
    Link : @Model.ActionLink</h3>
<h2>
    Available Roles</h2>
    @Html.Hidden("menuid", Model.ID)

@Using Html.BeginJUIBox("Available Roles")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="table-responsive">
                <table class="table table-bordered table-heading table-striped">
                    <thead>
                        <tr>
                            <th>
                                Nama Role
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                        </tr>
                        @For Each item In availableRoles
                            @<tr>
                                <td>@item
                                </td>
                                <td class="text-center">
                                    @Html.Raw(WriteCheckboxRole(item))
                                </td>
                            </tr>
    Next
                    </tbody>
                </table>
            </div>
        </div>
    </div>
End Using
<script type='text/javascript'>

    $(function () {

        $(".rolecheckbox").change(function () {
            var _rolename = $(this).data("rolename");
            var _isChecked = $(this).is(":checked");
            var _menuid = $("#menuid").val();

            $.ajax({
                type: 'POST',
                url: "/SystemSetting/setmenurole",
                data: { MenuId: _menuid, Rolename: _rolename, isActive: _isChecked },
                success: function (data) { 
                
                
                },
                error: ajax_error_callback,
                dataType: 'json'
            });


        });

    });


</script>
