@ModelType ProjectManagement.ProjectInfo
@code
    ViewData("Title") = "Mutual Check 0"
    
    Dim modelMutualCheck0 = CType(ViewData("MutualCheck0"), ProjectManagement.ProjectMutualCheck0)
    


        
    
End Code
@Helper WriteColWidths()
    Dim tblColWidths() As Integer = {90, 450, 60, 150, 100, 150, 60, 100, 150, 60, 120, 100, 150, 60, 100, 150, 60}
    For Each item In tblColWidths
    @Html.Raw(String.Format("<col style='width:{0}px'>", item.ToString()))
    Next
End Helper
@Html.Partial("ProjectPageMenu", Model)
@Using Html.BeginJUIBox("Mutual Check 0")
    
 

    @<div id="bottomNavBar" style="width: 50%; position: fixed; bottom: 0px; right: 30px;
        height: 50px; z-index: 1200;">
        <div style="padding: 13px; background-color: #6699FF; border-radius: 5px;">
            <div class="scroll-bar-wrap ui-widget-content ui-corner-bottom">
            </div>
            <div aria-disabled="false" class="scroll-bar ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                <a style="left: 0%;" class="ui-slider-handle ui-state-default ui-corner-all" href="#">
                    <span class="ui-icon ui-icon-grip-dotted-vertical"></span></a>
            </div>
        </div>
    </div>

    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <button type="button" class="btn btn-danger btn-label-left" id="btnPrint">
                    <span><i class="fa fa-print"></i></span>Print</button>
                <button type="button" class="btn btn-danger btn-label-left" id="btnRefresh">
                    <span><i class="fa fa-calculator"></i></span>Refresh
                </button>
                <div class="btn-group" data-toggle="buttons">
                    <label class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-lightbulb-o"></i></span>
                        <input type="checkbox" id="btnHighlight" autocomplete="off" />Highlight
                    </label>
                </div>
            </div>
        </div>
    </div>
    
    @<div style="position: relative">
        <div id="divWrapperTopLeft">
            <table class="table table-bordered" style="table-layout: fixed; display: none" role="grid"
                id="tblDataHeaderTopLeft">
                <colgroup>
                    @WriteColWidths()
                </colgroup>
                <thead>
                    <tr>
                        <th rowspan="3">
                            No. Item
                        </th>
                        <th rowspan="3">
                            Uraian
                        </th>
                        <th rowspan="3">
                            Sat
                        </th>
                        <th rowspan="3">
                            Harga Satuan
                        </th>
                        <th colspan="3">
                            Kontrak Awal
                        </th>
                        <th colspan="3">
                            Balance Budget
                        </th>
                        <th rowspan="3">
                            % Volume Terhadap Kontrak Awal
                        </th>
                        <th colspan="6">
                            Pekerjaan [+/-]
                        </th>
                    </tr>
                    <tr>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [+]
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [-]
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divWrapperTopRight">
            <table id="tblDataHeaderTopRight" class="table table-bordered" style="table-layout: fixed;
                display: none" role="grid">
                <colgroup>
                    @WriteColWidths()
                </colgroup>
                <thead>
                    <tr>
                        <th rowspan="3">
                            No. Item
                        </th>
                        <th rowspan="3">
                            Uraian
                        </th>
                        <th rowspan="3">
                            Sat
                        </th>
                        <th rowspan="3">
                            Harga Satuan
                        </th>
                        <th colspan="3">
                            Kontrak Awal
                        </th>
                        <th colspan="3">
                            Balance Budget
                        </th>
                        <th rowspan="3">
                            % Volume Terhadap Kontrak Awal
                        </th>
                        <th colspan="6">
                            Pekerjaan [+/-]
                        </th>
                    </tr>
                    <tr>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [+]
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [-]
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divWrapperTableLeft">
            <table id="tblDataLeft" class="table table-bordered dataTable" role="grid" style="table-layout: fixed">
                <colgroup>
                    @WriteColWidths()
                </colgroup>
                <thead>
                    <tr>
                        <th rowspan="3">
                            No. Item
                        </th>
                        <th rowspan="3">
                            Uraian
                        </th>
                        <th rowspan="3">
                            Sat
                        </th>
                        <th rowspan="3">
                            Harga Satuan
                        </th>
                        <th colspan="3">
                            Kontrak Awal
                        </th>
                        <th colspan="3">
                            Balance Budget
                        </th>
                        <th rowspan="3">
                            % Volume Terhadap Kontrak Awal
                        </th>
                        <th colspan="6">
                            Pekerjaan [+/-]
                        </th>
                    </tr>
                    <tr>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [+]
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [-]
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Jumlah
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            PPn 10%
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Total
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Dibulatkan
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                            VOL
                        </td>
                        <td>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <div id="divWrapperTable">
            <table class="table table-bordered" style="table-layout: fixed" role="grid" id="tblDataRight">
                <colgroup>
                    @WriteColWidths()
                </colgroup>
                <thead>
                    <tr>
                        <th rowspan="3">
                            No. Item
                        </th>
                        <th rowspan="3">
                            Uraian
                        </th>
                        <th rowspan="3">
                            Sat
                        </th>
                        <th rowspan="3">
                            Harga Satuan
                        </th>
                        <th colspan="3">
                            Kontrak Awal
                        </th>
                        <th colspan="3">
                            Balance Budget
                        </th>
                        <th rowspan="3">
                            % Volume Terhadap Kontrak Awal
                        </th>
                        <th colspan="6">
                            Pekerjaan [+/-]
                        </th>
                    </tr>
                    <tr>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th rowspan="2">
                            Volume
                        </th>
                        <th rowspan="2">
                            Jumlah Harga
                        </th>
                        <th rowspan="2">
                            Bobot
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [+]
                        </th>
                        <th colspan="3">
                            Pekerjaan Tambah [-]
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                        <th>
                            Volume
                        </th>
                        <th>
                            Jumlah Harga
                        </th>
                        <th>
                            Bobot
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Jumlah
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                            VOL
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Jumlah
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                            VOL
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Jumlah
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                            VOL
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            Jumlah
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td class="text-right">
                            VOL
                        </td>
                        <td>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <div class="clearfix">
        </div>
    </div>
    
    
    
End Using
@Using Html.BeginJUIBox("Approval")
    
    Using Html.BeginForm("SaveApprover", "Budget", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmApproval"})
    @<input type="hidden" id="MutualCheck0Id" name="id" value="@modelMutualCheck0.Id" />
    
    @Html.Hidden("ProjectInfoId",modelMutualCheck0.ProjectInfoId)
    @Html.WriteFormInput(Html.DateInput("DocumentDate", modelMutualCheck0.DocumentDate, New With {.class = "form-control"}), "Tanggal Dokumen")
    @Html.WriteFormInput(Html.TextBox("DocumentNumber", modelMutualCheck0.DocumentNumber, New With {.class = "form-control"}), "Nomor Dokumen")
    @<div class="form-group no-margin">
            <h4 class="col-lg-3 col-sm-4">Diajukan Oleh</h4>
        </div>
    @<hr class='no-margin' />
    @Html.WriteFormInput(Html.TextBox("ProposedByName", modelMutualCheck0.ProposedByName, New With {.class = "form-control"}), "Nama")
    @Html.WriteFormInput(Html.TextBox("ProposedByOccupation", modelMutualCheck0.ProposedByOccupation, New With {.class = "form-control"}), "Jabatan")
    @Html.WriteFormInput(Html.TextBox("ProposedByCompany", modelMutualCheck0.ProposedByCompany, New With {.class = "form-control"}), "Nama Perusahaan")
      @<div class="form-group no-margin">
            <h4 class="col-lg-3 col-sm-4">Diperiksa Oleh</h4>
        </div>
    @<hr class='no-margin' />
    @Html.WriteFormInput(Html.TextBox("CheckedByName", modelMutualCheck0.CheckedByName, New With {.class = "form-control"}), "Nama Pemeriksa")
    @Html.WriteFormInput(Html.TextBox("CheckedByOccupation", modelMutualCheck0.CheckedByOccupation, New With {.class = "form-control"}), "Jabatan")
    @Html.WriteFormInput(Html.TextBox("CheckedByCompany", modelMutualCheck0.CheckedByCompany, New With {.class = "form-control"}), "Nama Perusahaan")
      @<div class="form-group no-margin">
            <h4 class="col-lg-3 col-sm-4">Disetujui Oleh</h4>
        </div>
    @<hr class='no-margin' />
    @Html.WriteFormInput(Html.TextBox("ApprovedByName", modelMutualCheck0.ApprovedByName, New With {.class = "form-control"}), "Nama Pejabat")
    @Html.WriteFormInput(Html.TextBox("ApprovedByNIPP", modelMutualCheck0.ApprovedByNIPP, New With {.class = "form-control"}), "NIP")
    @Html.WriteFormInput(Html.TextBox("ApprovedByOccupation", modelMutualCheck0.ApprovedByOccupation, New With {.class = "form-control"}), "Jabatan")
    @Html.WriteFormInput(Html.TextBox("ApprovedByCompany", modelMutualCheck0.ApprovedByCompany, New With {.class = "form-control"}), "Nama Institusi")
    @<div class="well">
        <div class="col-sm-offset-4 col-sm-2">
            <button type="button" id="btnSave" class="btn btn-primary ">
                <span><i class="fa fa-arrow-circle-o-right"></i></span>Submit
            </button>
        </div>
        <div class="clearfix">
        </div>
    </div>
    End Using
    
End Using
<div class="" style="width: 100%; height: 20px; position: relative; background-color: rgb(235, 235, 235); z-index: 10009; margin: -20px 0px;"></div>

<style>
    .table
    {
        margin-bottom: 0px;
    }
    .double_top_border()
    {
        border-top-style: double;
    }
    .double_bottom_border()
    {
        border-bottom-style: double;
    }
    
    .tr_division()
    {
        background-color: #f0f0f0;
        font-weight: bold;
    }
    
    .tr_kumulatif_mingguan()
    {
        background-color: #f0f0f0;
        font-weight: bold;
    }
    .tr_kumulatif_bulanan()
    {
        background-color: #99CCFF;
        font-weight: bold;
    }
    
    .editable(-click, a.editable - click, a.editable - click) : hover()
    {
        text-decoration: none;
        border-bottom: 1px dashed #08C;
    }
    
    #divWrapperTopLeft
    {
        position: fixed;
        overflow: hidden;
        z-index: 1003;
        background-color: white;
        top: 0px;
        padding: 0px;
        margin: 0px;
        top: 80px;
    }
    
    #divWrapperTableLeft
    {
        position: absolute;
        top: 0px;
        padding: 90px 0px 0px 0px;
        z-index: 1002   ;
        width: 565px;
        overflow: hidden;
    }
    #tblDataLeft, #tblDataRight
    {
        table-layout: fixed;
        background-color: #fff;
    }
    
    #divWrapperTopRight
    {
        position: fixed;
        overflow: hidden;
        z-index: 1002;
        background-color: white;
        top: 0px;
        padding: 0px;
        margin: 0px;
        top: 80px;
    }
    #divWrapperTable
    {
        overflow: hidden;
        padding-top: 90px;
        margin: 0px;
        position:relative;
    }
    
    .scroll(-bar - wrap)
    {
        clear: left;
        padding: 0 4px 0 2px;
        margin: 0 -1px -1px -1px;
    }
    .scroll(-bar - wrap.ui - slider)
    {
        background: none;
        border: 0;
        height: 2em;
        margin: 0 auto;
    }
    .scroll(-bar - wrap.ui - handle - helper - parent)
    {
        position: relative;
        width: 100%;
        height: 100%;
        margin: 0 auto;
    }
    .scroll(-bar - wrap.ui - slider - handle)
    {
        top: .2em;
        height: 1.5em;
    }
    .scroll(-bar - wrap.ui - slider - handle.ui - icon)
    {
        margin: -8px auto 0;
        position: relative;
        top: 50%;
    }
    
    .cutofftext()
    {
        width: 420px;
        white-space: nowrap;
        text-overflow: ellipsis;
        overflow: hidden;
    }
    .bgwhite()
    {
        background-color: #fff;
        color: #000;
    }
    .highlight
    {
        background-color: #ffff00;
        color: #000;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<link href="../../../../plugins/bootstrap-editable/bootstrap-editable.css" rel="stylesheet"
    type="text/css" />
<script src="../../../../plugins/bootstrap-editable/bootstrap-editable.js" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblDataLeft = null;
    var tblDataRight = null;

    var initScrollbar = function () {
        var scrollPane = $("#divWrapperTopRight"),
          tblDataHeadTopRightContent = $("#tblDataHeaderTopRight");
        var tblDataContent = $("#tblDataRight");
        var scrollbar = $(".scroll-bar").slider({
            slide: function (event, ui) {
                if (tblDataContent.width() > scrollPane.width()) {
                    tblDataHeadTopRightContent.css("margin-left", Math.round(
                ui.value / 100 * (scrollPane.width() - tblDataHeadTopRightContent.width())
              ) + "px");
                    tblDataContent.css("margin-left", Math.round(
                ui.value / 100 * (scrollPane.width() - tblDataContent.width())
              ) + "px");
                } else {
                    tblDataHeadTopRightContent.css("margin-left", 0);
                    tblDataContent.css("margin-left", 0);
                }
            }
        });

        var handleHelper = scrollbar.find(".ui-slider-handle");
        if (handleHelper.has("span").length == 0) {
            handleHelper.append("<span class='ui-icon ui-icon-grip-dotted-vertical'></span>");
        }


        scrollPane.css("overflow", "hidden");

//        $("#tblDataRight").draggable({ axis: "x"});
    }   //end initScrollbar;

    initWrapper = function () {


        var columnWidth = $('#tblDataLeft thead:eq(0) tr:eq(0) th:eq(0)').outerWidth() + $('#tblDataLeft thead:eq(0) tr:eq(0) th:eq(1)').outerWidth();
        $("#divWrapperTableLeft").width(columnWidth);

        $("#divWrapperTopLeft").width($("#divWrapperTableLeft").width());
        $("#divWrapperTopRight").width($("#divWrapperTable").width());
        ///end init thewrapper

        $(window).scroll(function (event) {
            var scroll = $(window).scrollTop();
            var divOffset = $("#divWrapperTableLeft").offset();
            var divHeight = $("#divWrapperTableLeft").height() + divOffset.top-180;//180 is padding top+padding bottom;
            
            
            if ((scroll > divOffset.top) && (scroll < divHeight)) {
                $("#tblDataHeaderTopRight").show()
                $("#tblDataHeaderTopLeft").show()

                $("#tblDataHeaderTopRight").css("margin-left", $("#tblDataRight").css("margin-left"));
            } else {
                $("#tblDataHeaderTopRight").hide()
                $("#tblDataHeaderTopLeft").hide()

            };
        });
    }

    function MessagesMenuWidth() {
        var W = window.innerWidth;
        var W_menu = $('#sidebar-left').outerWidth();
        var w_messages = (W - W_menu) * 16.666666666666664 / 100;
        $('#messages-menu').width(w_messages);
        initWrapper();

    }

    var _loadContent = function () {
        var _ProjectId = $("#ProjectInfoId").val();
        $.ajax({
            url: '/Budget/GetMutualCheck0View',
            data: { id: _ProjectId },
            type: 'POST',
            success: _loadContentCallback,
            error: ajax_error_callback,
            datatype: 'json'
        });


    };

    var _loadContentCallback = function (data) {

        tblDataRight.clear();
        tblDataLeft.clear();
        tblDataRight.rows.add(data.data).draw();
        tblDataLeft.rows.add(data.data).draw();
    }

    var _sumColumns = function (api) {
        var sum_CostKontrakAwal = api.column(5, { page: 'current' }).data().sum();
        var sum_CostBalancedBudget = api.column(8, { page: 'current' }).data().sum();
        var sum_CostAddition = api.column(12, { page: 'current' }).data().sum();
        var sum_CostReduction = api.column(15, { page: 'current' }).data().sum();
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(1) > td:nth-child(4)").text($.number(sum_CostKontrakAwal, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(1) > td:nth-child(7)").text($.number(sum_CostBalancedBudget, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(1) > td:nth-child(11)").text($.number(sum_CostAddition, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(1) > td:nth-child(14)").text($.number(sum_CostReduction, 2, ",", "."));
        //PPN10%
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(2) > td:nth-child(4)").text($.number(sum_CostKontrakAwal * 0.1, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(2) > td:nth-child(7)").text($.number(sum_CostBalancedBudget * 0.1, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(2) > td:nth-child(11)").text($.number(sum_CostAddition * 0.1, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(2) > td:nth-child(14)").text($.number(sum_CostReduction * 0.1, 2, ",", "."));
        //TOTAL
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(3) > td:nth-child(4)").text($.number(sum_CostKontrakAwal * 0.1 + sum_CostKontrakAwal, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(3) > td:nth-child(7)").text($.number(sum_CostBalancedBudget * 0.1 + sum_CostBalancedBudget, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(3) > td:nth-child(11)").text($.number(sum_CostAddition * 0.1 + sum_CostAddition, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(3) > td:nth-child(14)").text($.number(sum_CostReduction * 0.1 + sum_CostReduction, 2, ",", "."));
        //Pembulatan. 
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(4) > td:nth-child(4)").text($.number(sum_CostKontrakAwal * 0.1 + sum_CostKontrakAwal, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(4) > td:nth-child(7)").text($.number(sum_CostBalancedBudget * 0.1 + sum_CostBalancedBudget, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(4) > td:nth-child(11)").text($.number(sum_CostAddition * 0.1 + sum_CostAddition, 2, ",", "."));
        $("#tblDataRight > tfoot:nth-child(3) > tr:nth-child(4) > td:nth-child(14)").text($.number(sum_CostReduction * 0.1 + sum_CostReduction, 2, ",", "."));
    }
    initTableData = function () {

        var cols = [
        //header
        {"data": "PaymentNumber", "sClass": "text-center" }, //0
         {"data": "TaskTitle" }, //1

        {"data": "UnitQuantity", "sClass": "text-center" }, //2 satuan
         {"data": "Value", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //3 harga satuan
        //kontrak awal
         {"data": "Quantity", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //4 volume
         {"data": "TotalItemValue", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //5 jumlah harga
         {"data": "TaskWeight", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //6 bobot
        //balance budget
         {"data": "QuantityMC0", "sClass": "text-right quantitymc0", "mRender": _fnRender2DigitDecimal }, //7 volume
         {"data": "TotalItemMC0value", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //8 jumlah harga
         {"data": "TaskWeightMC0", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //9 bobot

        //
         {"data": "DeltaPercentage", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //10 %volume terhadap kontrak awal

        //pekerjaan tambah [+]
         {"data": "AdditionVolume", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //11 volume
         {"data": "AdditionCost", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //12  jumlah harga
         {"data": "AdditionWeight", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //13 bobot
        //pekerjaan tambah [-]
         {"data": "ReductionVolume", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //14 volume
         {"data": "ReductionCost", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //15 jumlah harga
         {"data": "ReductionWeight", "sClass": "text-right", "mRender": _fnRender2DigitDecimal} //16 bobot
       ];

        var _coldefs = [];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = cols;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;

        datatableDefaultOptions.fnDrawCallback = function (settings) {
            var divisionId = 0;
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var colCount = 0;
            var numberOfColumns = 16; //
            api.column(0, { page: 'current' }).data().each(function (group, i) {

                var currentdivisionId = api.row(i).data().DivisionId;


                if (currentdivisionId != divisionId) {
                    var divisionNumber = api.row(i).data().DivisionNumber;
                    var divisionTitle = api.row(i).data().DivisionTitle;
                    var _html = "<tr class='tr_division'>";
                    _html += "<td class='text-center'>&nbsp;</td>";
                    _html += "<td colspan='" + numberOfColumns + "'>Divisi " + divisionNumber + "." + divisionTitle + "</td>";
                    _html += "</tr>";

                    $(rows).eq(i).before(_html);


                    divisionId = currentdivisionId;
                }


            });
            _sumColumns(api);
            setHightlight();
        }
        tblDataRight = $("#tblDataRight").DataTable(datatableDefaultOptions)
	        .on("draw.dt", function () {
	            initWrapper();
	            initScrollbar();

	        });

        tblDataLeft = $("#tblDataLeft").DataTable(datatableDefaultOptions)
	        .on("draw.dt", function () {
	            initWrapper();
	            initScrollbar();

	        });
    }

    var initEditable = function () {

        $('body').editable({
            selector: ".quantitymc0",
            type: "text",
            html: true,
            container: "body",
            pk: 0,
            url: "/Budget/SaveQuantityMutualCheck0",
            emptytext: "",
            params: function (p) {
                var post = tblDataRight.cell(this).index();
                var datarow = tblDataRight.row(post.row).data();
                p.pk = datarow.Id;
                p.ProjectMutualCheck0Id = $("#MutualCheck0Id").val();
                return p;

            },
            title: function (e) {
                var post = tblDataRight.cell(this).index();
                var datarow = tblDataRight.row(post.row).data();

                var _dtlist = "<dl class='dl-horizontal'>" +
			    "<dt>Volume Kontrak Awal</dt><dd>" + $.number(datarow.Quantity, ",", ".") + " " + datarow.UnitQuantity + "</dd></dt>";
                return _dtlist;
            },
            display: function (value) {



            },
            success: function (data, newvalue) {

                if (data.stat == 1) {
                    _loadContent();
                } else {
                    showNotificationSaveError(data, "Terjadi kesalahan");
                }

            }

        }); //end set the editable
    }

    var setHightlight = function () {


        if (tblDataRight !== null) {
            var thisChecked = $("#btnHighlight").is(":checked")
            console.log(thisChecked);
            var rows = tblDataRight.rows().nodes();
            tblDataRight.data().each(function (rowdata, index) {
                if (thisChecked) {
                    if (rowdata.Quantity != rowdata.QuantityMC0) {
                        $(rows[index]).find("td:eq(7)").addClass("highlight");
                    }

                } else {
                    $(rows[index]).find("td:eq(7)").removeClass("highlight");
                }
            });
        }
    }
</script>
<script type="text/javascript">



    $(function () {
        initEditable();
        initWrapper();
        initScrollbar();
        initTableData();
        _loadContent();


        $("#btnHighlight").change(function () {

            setHightlight();

        });

        $("#btnRefresh").click(function () {
            _loadContent();
        });

        $("#btnPrint").click(function () {

            var _ProjectId = $("#ProjectInfoId").val();
            window.location = "/ProjectManagement/Budget/PrintMutualCheck0/" + _ProjectId;
        });
        $("#btnSave").click(function () {
            showSavingNotification("Menyimpan data...");



            $.ajax({
                type: "POST",
                data: $("#frmApproval").serialize(),
                url: "/Budget/SaveMutualCheck0Header",
                success: function (data) {
                    $.notifyClose();
                    if (data.stat == 1) {
                        showNotification("Data Tersimpan");
                    } else {
                        showNotificationSaveError(data);
                    }
                },
                error: ajax_error_callback
            });
        });
    });
</script>
