@Code
    ViewData("Title") = "Input Data Timbangan"
    ViewBag.headIcon = "icon-filter"
    'ViewBag.showHeader = False
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Daftar Timbangan Masuk", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class="icon-list-alt"></i> Daftar Data Timbangan Masuk</h4>
<a class="btn btn-sm btn-success pull-right" href="@Url.Action("DataScalesIn_", "Stock")">
	<i class="icon-plus"></i> Tambah Data Masuk
</a>
<div class="clear"></div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>Tanggal Masuk</th>
                <th>No Polisi</th>
                <th>Sopir</th>
                <th>Perusahaan</th>
                <th>Material</th>
                <th>Berat 1</th>
                <th>Keterangan</th>
                <th>Clerk 1</th>
                <th class="action"></th>
            </tr>
       </thead>
    </table>
End Using


@Section StyleSheet
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Inventory/DataScales.js")"></script>
End Section