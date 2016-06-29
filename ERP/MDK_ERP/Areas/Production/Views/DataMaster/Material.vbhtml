@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Daftar Material", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-book'></i> Daftar Material dipakai</h4>
<a class="btn btn-sm btn-success pull-right" href="@Url.Action("Material_", "DataMaster")">
    <i class="icon-plus"></i> Tambah Baru</a>
<div class="clear">
</div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>
                    Kode
                </th>
                <th>
                    Nama
                </th>
                <th>
                    Jenis Material
                </th>
                <th>
                    Termasuk Stok
                </th>
                <th>
                    Satuan
                </th>
                <th>
                    Stok Awal
                </th>
                <th>
                    Mesin Produksi
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
End Using
<!-- Start Dummy Form -->
<section class="">
    <!--Modal Satuan-->
    @Using Html.ModalForm("modal-Data1", "Tambah Rasio", Url.Action("MWR_Save", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            @*<label class="col-md-3 control-label">
            Nama Material</label>*@
            <div class="col-sm-5">
                @Html.Hidden("ID")
            </div>
        </div>
        Using Html.RowBox("box-table")
        @<table id="tb_ratio" class="table">
            <thead>
                <tr>
                    <th>
                        Satuan
                    </th>
                    <th>
                        Pilih
                    </th>
                    <th>
                        Nilai
                    </th>
                    <th class="action">
                    </th>
                </tr>
            </thead>
            <tbody>
                @Code
            Dim item = ViewData("listRatio")
            For Each list In item
                End Code
                <tr>
                    <td>
                        @list.Symbol
                    </td>
                    <td>
                        @Html.CheckBox("Check")
                    </td>
                    <td>
                        @Html.Hidden("ratioVal", list.ID)@Html.TextBox("Weight", Nothing, New With {.class = "form-control", .style = "width:30%;margin-left:50%;"})
                    </td>
                </tr>
                @code
            Next
                End Code
            </tbody>
        </table>
        End Using
    End Using
    <!--End modal-->
</section>
@Section StyleSheet
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/Material.js")"></script>
End Section