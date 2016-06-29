@Code
    ViewData("Title") = "Pengaturan User"
    ViewBag.headIcon = "icon-user"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Pengaturan User", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-user-md'></i> Pengaturan User</h4>
<a class="btn btn-sm btn-success pull-right" href="@Url.Action("Create", "UserManagement")">
	<i class="icon-user-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
    @Using Html.RowBox("box-table")
        @<table id="tb_Data" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th>Nama User</th>
                    <th style="width:70px;min-width:70px;"></th>
                </tr>
            </thead>
        </table>
    End Using

@Section StyleSheet
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/ManagementUser.js")"></script>
End Section
