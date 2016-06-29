
@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Satuannya tersebut", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'><i class='icon-bookmark'></i> Daftar Satuan</h4>
<a onclick="fnDel()" data-toggle="modal" id="" data-target="#modal-Data" class="btn btn-sm btn-success pull-right" href="javascript:void(0)">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>Simbol</th>
                <th>Nama Satuan</th>
                <th>Rasio</th>
                <th class="action"></th>
            </tr>
        </thead>
    </table>
End Using

<!-- Start Dummy Form -->
<section class="">
<!-- start modal >> tipe Stok -->
    <!-- -->
    @Using Html.ModalForm("modal-Data", "Satuan Material", Url.Action("Unit_Save", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
        
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Satuan Ukur</label>
            <div class="col-md-8">
            @Html.Hidden("ID")
                @Html.TextBox("Unit", Nothing, New With {.class = "form-control", .style = "width:40%;"})
            </div>
            <div class="clear">
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Simbol</label>
            <div class="col-md-8">
                @Html.TextBox("Symbol", Nothing, New With {.class = "form-control", .style = "width:40%;"})
            </div>
            <div class="clear">
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Rasio Satuan</label>
            <div class="col-md-8">
                @Html.TextBox("Ratio", Nothing, New With {.class = "form-control text-right", .style = "width:40%;"})
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
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/MaterialUnit.js")"></script>
End Section
