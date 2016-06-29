@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", "/Production/DataMaster"}, _
                                             {"Komposisi Material", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-stackexchange'></i> Komposisi Material</h4>
<div class="pull-right">
    @*@Using Html.BeginForm("Laporan", "DataMaster", Nothing, FormMethod.Post, New With {.Name = "form-report", .id = "form-report", .class = "form-inline inline"})
        @<div class="form-group">
            @Html.DropDownList("HotMix", New SelectList(DirectCast(ViewData("ListAsphal"), IEnumerable), "IDMaterial", "Name"), "Pilih Hot Mix", New With {.class = "form-control select2"})
        </div>
        @<div class="form-group">
            <button type="submit" class="btn btn-sm btn-primary" href="javascript:void(0)">
                <i class="icon-print"></i> Cetak Jurnal Hotmix
            </button>
        </div>
    End Using*@
    <a class="btn btn-sm btn-primary" href="@Url.Action("MaterialUnit","DataMaster")">
        <i class="icon-gear"></i> Pengaturan Satuan
    </a>
    <a data-toggle="modal" id="" class="btn btn-sm btn-success"
        href="javascript:void(0)"><i class="icon-plus"></i> Tambah Baru</a>
</div>
<div class="clear">
</div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th width="20%">
                    Produk
                </th>
                <th width="30%">
                    Komposisi
                </th>
                <th width="30px">
                    Jumlah
                </th>
                <th style="text-align:left !important;padding-left: 0px;"> (Satuan Produk)</th>
                <th width="30px">
                    Jumlah
                </th>
                <th style="text-align:left !important;padding-left: 0px;"> (Satuan Komposisi)</th>
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
<section class="dummy">
    <!--modal create-->
    @Using Html.ModalForm("modal-Data1", "Form Material", Url.Action("MC_Save", "DataMaster"), "", "POST", "form-Data1", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Material</label>
            <div class="col-md-7">
                <div class="input-group">
                    @Html.DropDownList("IDMaterial", New SelectList(DirectCast(ViewData("produk"), IEnumerable), "ID", "Name"), Nothing, New With {.class = "form-control select2", .style="width:100%"})
                    <span class="input-group-addon"> Satuan : <span class="measurement" style="display:none;"></span></span>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        Using Html.WriteSummaryBox("List Komposisi Material", "icon-list", "Komposisi Material")
            @<a id="addcomp" class=" btn btn-success btn-sm" href="javascript:void(0)"><i class="icon-plus"></i> Tambah Komposisi</a>
            @<table id="tb_comp" class="table">
                <colgroup>
                    <col style="width:50%;" />
                    <col style="width:30%;" />
                    <col style="width:20%;min-width:80px;" />
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            Komposisi
                        </th>
                        <th>
                            Jumlah
                        </th>
                        <th class="action">
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        End Using
    End Using
    <!--end of modal-->

    <div style="display: none;" id="dropdown">
        @*@Html.DropDownList("IDMaterialComposition", New SelectList(DirectCast(ViewData("listComp"), IEnumerable), "ID", "Name"), "Pilih Komposisi", New With {.class = "form-control"})*@
        @Html.DropDownGroupListFor("MaterialList", ViewBag.MaterialList, New With {.class = "form-control"})
    </div>

    <div style="display: none;" id="AmountInput">
        <input type='text' name='Amount' id='Amount' class='spin form-control amount text-right' />
        <span class='input-group-addon measurement'></span>
    </div>

    <!--modal edit-->
     @Using Html.ModalForm("modal-Data", "Edit Material", Url.Action("MC_Edit", "DataMaster"), "", "POST", "form-Data", "form-horizontal")
         @Html.Hidden("ID")
          @<div class="form-group">
                <label class="col-md-3 control-label">Nama Material</label>
                <div class="col-md-8">
                 @*@Html.DropDownList("IDMaterial", New SelectList(DirectCast(ViewData("AllProduk"), IEnumerable), "ID", "Name"), "Pilih Produk", New With {.class = "form-control"})*@
                    @Html.DropDownGroupListFor("IDMaterial", ViewData("AllProduk"), New With {.class = "form-control select2"})
                 </div>
                <div class="clear">
                </div>
            </div>
         @<div class="form-group">
         <label class="col-md-3 control-label">Komposisi</label>
          <div class="col-md-8">
           @*@Html.DropDownList("IDMaterialComposition", New SelectList(DirectCast(ViewData("listComp"), IEnumerable), "ID", "Name"), "Pilih Komposisi", New With {.class = "form-control select2"})*@
             @Html.DropDownGroupListFor("IDMaterialComposition", ViewBag.MaterialList, New With {.class = "form-control select2"})
          </div>
         </div>
          @<div class="form-group">
         <label class="col-md-3 control-label">Jumlah</label>
          <div class="col-md-3">
            <div class="input-group">
                @Html.TextBox("Amount", Nothing, New With {.class = "form-control text-right numeric"})
                <span class="input-group-addon measurement" style="display:block;"></span>
            </div>
          </div>
         </div>
     End Using
     <!--end of modal-->

</section>
<!-- End of Dummy Form -->


@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DataTables/js/jquery.dataTables.rowGrouping.js")"></script>

    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/jquery.numeric.js")"></script>

    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/DataMaster/MaterialComposition.js")"></script>
End Section
