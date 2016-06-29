
@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Daftar Inventaris Kendaraan", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-truck'></i> Daftar Inventaris Kendaraan</h4>
<a data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>Kode</th>
                <th>Nama</th>
            </tr>
        </thead>
    </table>
End Using

<!-- Start Dummy Form -->
<section class="">
<!-- start modal >> tipe Stok -->
    <!-- -->
    @Using Html.ModalForm("modal-Data", "Inventaris Kendaraan", Url.Action("", ""), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">TextBox1</label>
            <div class="col-md-8">
                @Html.Hidden("Id")
                @Html.TextBox("TextBox1", Nothing, New With {.class = "form-control"})
            </div>
            <div class="clear">
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">TextBox2</label>
            <div class="col-md-8">
                @Html.TextBox("TextBox2", Nothing, New With {.class = "form-control"})
            </div>
            <div class="clear">
            </div>
        </div>
    End Using
    <!-- end >> tipe Stok -->
<!-- end modal -->

</section>


@Section StyleSheet
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/Vehicle.js")"></script>
End Section
