@Code
    ViewData("Title") = "Jurnal Hotmix"
    ViewBag.headIcon = "icon-cogs"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Daftar Jurnal Hotmix", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-list-alt'></i> Daftar Jurnal Hotmix</h4>
<a class="btn btn-sm btn-success pull-right" href="@Url.Action("IndexAmp", "Process")"><i class="icon-plus"></i> Tambah Baru</a>
<div class="clear">
</div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>Produk</th>
                <th>Tanggal</th>
                <th>Operator</th>
                <th>Shift</th>
                <th>Project</th>
                <th style="min-width:70px;"></th>
            </tr>
        </thead>
    </table>
End Using
@Section StyleSheet
End Section
@Section jsScript
  <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
  <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Process/AMP.js")"></script>
End Section
