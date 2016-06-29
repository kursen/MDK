@Code
    ViewData("Title") = "Distribusi Material"
    ViewBag.headIcon = "icon-exchange"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Production", "/Production"}, _
                                            {"Distribusi Material", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

@Html.Partial("_DoubleDatePickerPartial")

<div class="row" style="margin-top:-20px;">
    <div class="col-sm-12">
        <div class="tabbable"> <!-- Only required for left/right tabs -->
        <ul class="nav nav-pills">
            <li>
                <a class="btn btn-sm btn-success pull-right" href="@Url.Action("DataScales", "Stock")">
	                <i class="icon-pencil"></i> Input Data Timbangan
                </a>
            </li>
            <li class="pull-right"><a href="#tab2" data-toggle="tab" class="btn btn-sm">Penerimaan</a></li>
            <li class="active pull-right"><a href="#tab1" data-toggle="tab" class="btn btn-sm">Pengiriman</a></li>
            <li class="pull-right"><label class="" style="padding-top: 12px;">Berdasarkan :</label></li>
        </ul>
        <div class="tab-content">
            <!-- Section Tab 1 > Pengiriman -->
            <div class="tab-pane active" id="tab1">
                @Using Html.RowBox("box-table")
                    @<table id="tb_Delivered" class="display" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Material</th>
                                <th>Tanggal</th>
                                <th>Berat (Ton)</th>
                                <th>Sumber</th>
                                <th>Tujuan</th>
                            </tr>
                        </thead>
                    </table>
                End Using
            </div>

            <!-- Section Tab 2 > Penerimaan -->
            <div class="tab-pane" id="tab2">
                @Using Html.RowBox("box-table")
                    @<table id="tb_Received" class="display" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Material</th>
                                <th>Tanggal</th>
                                <th>Berat (Ton)</th>
                                <th>Sumber</th>
                                <th>Tujuan</th>
                            </tr>
                        </thead>
                    </table>
                End Using
            </div>
        </div>
    </div>
    </div>
</div>

@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />

    <style type="text/css">
    .nav > li > a.btn-success:focus, .nav > li > a.btn-success:hover {
        text-decoration: none;
        background-color: #329E56;
    }
    </style>
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DataTables/js/jquery.dataTables.forHelper.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Inventory/Distribution.js")"></script>
End Section