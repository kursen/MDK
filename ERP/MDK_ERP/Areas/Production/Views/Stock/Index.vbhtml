@Code
    ViewData("Title") = "Persediaan Material"
    ViewBag.headIcon = "icon-book"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Production", "/Production"}, _
                                            {"Persediaan Material", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<div class="row">
    <div class="form form-horizontal">
        <div class="col-xs-6 col-sm-12 col-md-12 col-lg-7">
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label">Material</label>
            <div class="col-md-4 col-sm-4 col-xs-8">
                @Html.DropDownGroupListFor("MaterialList", ViewBag.MaterialList, New With {.class = "form-control select2"})
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label">Periode</label>
            <div class="col-md-4 col-sm-4 col-xs-8">
                <div class='input-group date' id='datetimepicker'>
                    <input type='button' class="form-control" />
                    <span class="input-group-addon">
                        <span class="icon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-4 text-left control-label"></label>
            <div class="col-md-4 col-sm-4 col-xs-8">
                <a onclick="" id="filter" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                <span class="loader hidden"></span>
            </div>
        </div>
    </div>
    </div>
</div>

<div class="row" style="margin-top:-20px;">
    <div class="col-sm-12">
            @Using Html.RowBox("box-table")
                @<div class="">
                    <table cellpadding="0" cellspacing="0" border="0" class="display table table-bordered" id="tb_stock" >
	                    <thead>
		                    <tr>
			                    <th rowspan="2">Tgl</th>
                                <th rowspan="2">Status</th>
			                    <th rowspan="2">Keterangan</th>
			                    <th colspan="2">Jumlah (Ton)</th>
			                    <th rowspan="2">Sisa</th>
                                @*<th rowspan="2"></th>*@
		                    </tr>
                            <tr>
			                    <th>Tambah</th>
			                    <th>Kurang</th>
                            </tr>
	                    </thead>
	                    <tbody>
	                    </tbody>
                    </table>
                </div>
            End Using
    </div>
</div>
<!-- Start Modal >> Distribution Stock Detail -->
    @Using Html.Modal("modal-detail", "Keterangan Distribusi Material")
        @<div>
            <div class="form-group">
                <label class="col-md-3 control-label">Kode Material</label>
                <div class="col-md-8">
                    <span class="span-form-control">0000001</span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Nama Material</label>
                <div class="col-md-8">
                    <span class="span-form-control">Grogol</span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Tanggal Distribusi</label>
                <div class="col-md-8">
                    <span class="span-form-control">1 Maret 2015 11.15</span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Berat (Ton)</label>
                <div class="col-md-8">
                    <span class="span-form-control">600</span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">No. Polisi</label>
                <div class="col-md-8">
                    <span class="span-form-control">BK 2343 SQ</span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Supir</label>
                <div class="col-md-8">
                    <span class="span-form-control">Pak Sukirman</span>
                </div>
                <div class="clear"></div>
            </div>
            <div class="form-group">
                <label class="col-md-3 control-label">Lokasi Tujuan/Asal</label>
                <div class="col-md-8">
                    <span class="span-form-control">PT. Bangun Sejahtera</span>
                </div>
                <div class="clear"></div>
            </div>
        </div>
    End Using
<!-- end Modal -->
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/MaterialStock.js")"></script>
End Section

