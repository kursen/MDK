@ModelType ProjectManagement.ProjectInfo
@Code
    Dim PaymentDocument As New ProjectManagement.ProjectCostRecord
    PaymentDocument.TransactionDate = Today
    
    Dim PaymentItem As New ProjectManagement.ProjectCostRecordItem
    
    ViewData("Title") = "Keuangan Proyek"
    
End Code
@Html.Partial("ProjectPageMenu", Model)
@Using Html.BeginJUIBox("Rekapitulasi Pengeluaran Proyek")
    @<div id="divWeeklyList">
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class=" pull-right">
                    <button type="button" class="btn btn-danger btn-label-left btnAdd">
                        <span><i class="fa fa-plus"></i></span>Menambah</button>
                          <button  class="btn btn-danger btn-label-left" id="btnPrint">
                    <span><i class="fa fa-print"></i></span>Print</button>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 col-lg-2">
                Periode</div>
            <div class="col-sm-8 col-lg-10">
                <span id="dateStart"></span>- <span id="dateEnd"></span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 col-lg-2">
                Jumlah</div>
            <div class="col-sm-8 col-lg-10">
                <span id="AllTotal"></span>
            </div>
        </div>
		<div class="table-responsive">
        <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
            id="tblProjectCostWeeklyList">
            <colgroup>
                <col style="width: 50px;" />
                <col style="width: 100px;" />
                <col style="width: 100px;" />
                <col />
                <col style="width: 140px;" />
                <col style="width: 200px;" />
                <col style="width: 100px;" />
            </colgroup>
            <thead>
                <tr>
                    <th>
                        #
                    </th>
                    <th>
                        Tanggal
                    </th>
                    <th>
                        Posting
                    </th>
                    <th>
                        Uraian
                    </th>
                    <th>
                        Jumlah
                    </th>
                    <th>
                        Keterangan
                    </th>
                    <th>
                    </th>
                </tr>
            </thead>
        </table>
		</div>
    </div>
 
    
    @<div id="divForm" style="display: none">
        <div class="col-lg-12 col-sm-12">
            <div class=" pull-right">
                <button  class="btn btn-danger btn-label-left btnBack">
                    <span><i class="fa fa-arrow-left"></i></span>Kembali</button>
                   
            </div>
        </div>
        <h3 class="text-center">
            Pembayaran Cash</h3>
        @Using Html.BeginForm("SaveProjectCost", "Finance", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmProjectCost"})
            @Html.Hidden("ProjectInfoId", Model.Id)
            @<input type="hidden" name="Id" id="ProjectCostRecordId" value="0" />   
            @Html.WriteFormInput(Html.TextBox("TaskGroupTitle", PaymentDocument.TaskGroupTitle, New With {.class = "form-control"}), "Untuk Pekerjaan")
            @Html.WriteFormInput(Html.TextBox("PacketCode", PaymentDocument.PacketCode, New With {.class = "form-control"}), "Kode Paket")
            @Html.WriteFormInput(Html.DateInput("TransactionDate", PaymentDocument.TransactionDate,
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}), "Tanggal",
                                      smControlWidth:=4, lgControlWidth:=2)
            @<input type="hidden" name="items" id="PaymentItems" value="" />                        
            @<hr />
            @<div class="table-responsive">
			<table class="table table-bordered" id="tblProjectCostItem">
                <colgroup>
                    <col style="width: 50px" />
                    <col style="width: 80px" />
                    <col />
                    <col style="width: 150px" />
                    <col style="width: 150px" />
                    <col style="width: 100px" />
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            #
                        </th>
                        <th>
                            Posting
                        </th>
                        <th>
                            Uraian
                        </th>
                        <th>
                            Jumlah
                        </th>
                        <th>
                            Catatan
                        </th>
                        <th class="text-center">
                            <button type="button" class="btn btn-danger btn-label-left" id="btnAddCostItem">
                                <span><i class="fa fa-plus"></i></span>Tambah</button>
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td colspan="3" class="text-center">
                            TOTAL
                        </td>
                        <td class="tex-right">
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </tfoot>
            </table>
			</div>
            @<div class="well">
                <div class="row">
                    <div class="col-lg-offset-4 col-sm-offset-2">
                        <button type="button" class="btn btn-danger btn-label-left" id="btnSaveCashForm">
                            <span><i class="fa fa-arrow-up"></i></span> Post</button>
                               <button  type="button" class="btn btn-danger btn-label-left btnBack">
                    <span><i class="fa fa-arrow-left"></i></span>Kembali</button>
                    </div>
                </div>
            </div>
    End Using
    </div>
    @<div class="clearfix">
    </div>
    @<div id="divItemFormPlaceHolder" style="display: none">
        <div class="form-horizontal" id="divItem">
            @Using Html.BeginForm("SaveCostItems", "Finance", Nothing, FormMethod.Post, New With {.class = "form-horizontal",
                                                                                             .autocomplete = "Off", .id = "frmCostItem"})
                @<input type="hidden" name="id" id="ProjectCostRecordItemId" value="0" />
        
                @Html.WriteFormInput(Html.TextBox("PostCategory", "", New With {.class = "form-control", .maxlength = 250}), "Posting", smControlWidth:=6, lgControlWidth:=2)
                @Html.WriteFormInput(Html.TextBox("ItemDescription", "", New With {.class = "form-control", .maxlength = 250}), "Uraian")
                @Html.WriteFormInput(Html.DecimalInput("Value", 0), "Jumlah")
                @Html.WriteFormInput(Html.TextArea("Notes", "", New With {.class = "form-control"}), "Catatan")
                @<div class="col-lg-offset-4 col-sm-offset-2">
                    <button type="button" class="btn btn-primary btn-label-left" id="btnSimpanCostItem">
                        <span><i class="fa fa-plus"></i></span>Tambah</button>
                    <button type="reset" class="btn btn-primary" id="btnBatalCostItem">
                        Batal</button>
                </div>
    End Using
        </div>
    </div>
           
End Using
<style type="text/css">
    .tr_weekgroup
    {
        font-weight: bold;
    }
    .tr_paymentgroup td
    {
        font-weight: bold;
    }
</style>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/datatables/dataTables.bootstrap.css")" />
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.bundle.min.js")" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.css")" />
<script type="text/javascript">

    var tblProjectCostWeeklyList = null;
    var tblProjectCostItem = null;

    var editedProjectCostItemIndex = -1;

    var _validateCostItemCallback = function (data) {
        if (data.stat == 1) {
            $("#divItem").appendTo($("#divItemFormPlaceHolder"));

            var ItemCostId = $("#ProjectCostRecordItemId").val();
            var oItem = {
                Id: ItemCostId,
                ProjectCostRecordId: $("#ProjectCostRecordId").val(),
                PostCategory: $("#PostCategory").val(),
                ItemDescription: $("#ItemDescription").val(),
                Value: $("#Value").val(),
                Notes: $("#Notes").val()
            };
            if (editedProjectCostItemIndex == -1) {

                tblProjectCostItem.row.add(oItem);
            } else {
                //this is editing;
                var arrData = tblProjectCostItem.data();
                arrData.splice(editedProjectCostItemIndex, 1, oItem);
                tblProjectCostItem.clear();
                tblProjectCostItem.rows.add(arrData);

            }


            $("#btnBatalCostItem").trigger("click");
        } else {
            showNotificationSaveError(data, "Penambahan gagal.");
        }

    }

    var _loadData = function () {
        var projectId = $("#ProjectInfoId").val();
        $.ajax({
            type: "POST",
            data: { id: projectId },
            url: "/Finance/GetCostList",
            success: _loadDataCallback
        });
    }
    var _loadDataCallback = function (d) {
        tblProjectCostWeeklyList.clear();
        if (d.data.length > 0) {
            tblProjectCostWeeklyList.rows.add(d.data).draw();
            var dtStart = new Date(parseInt(d.dateStart.substr(6)));
            $("#dateStart").html($.datepicker.formatDate('dd-mm-yy', dtStart));
            var dtEnd = new Date(parseInt(d.dateEnd.substr(6)));
            $("#dateEnd").html($.datepicker.formatDate('dd-mm-yy', dtEnd));
            $("#AllTotal").html("Rp. " + $.number(d.TotalAmount, 2, ",", "."));
        }


    }

    $(function () {  //init script

        var PostingTypes = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/Finance/PostingTypes/",
                replace: function (url, uriEncodedQuery) {
                    var q = $("#PostCategory").val();
                    return url + "?query=" + q;

                }
            }

        });
        PostingTypes.initialize();

        $("#PostCategory").typeahead(null, {
            name: "PostCategory",
            source: PostingTypes
        });



        var taskTypes = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/Finance/TaskTypes/",
                replace: function (url, uriEncodedQuery) {
                    var q = $("#TaskGroupTitle").val();
                    return url + "?query=" + q;

                }
            }

        });
        taskTypes.initialize();
        $("#TaskGroupTitle").typeahead(null, {
            name: "TaskGroupTitle",
            source: taskTypes
        });




        $(".btnBack").click(function () {
            $("#divWeeklyList").show("slow");

            $("#divForm").hide();
            $("#divItem").appendTo($("#divItemFormPlaceHolder"));
            $("#form_placeholder").remove();
            tblProjectCostItem.clear().draw();
            $("#btnAddCostItem").prop("disabled", false);
            $("#frmProjectCost").trigger("reset");
            $("#frmCostItem").trigger("reset");
            $(window).unbind('beforeunload'); ;

        });
        $(".btnAdd").click(function () {
            $("#divWeeklyList").hide();

            $("#divForm").show("slow");
            $(window).bind('beforeunload', function () {
                return 'Do you really want to leave?';
            });
        });

        $("#btnAddCostItem").click(function () {
            $("#tblProjectCostItem").append("<tr id='form_placeholder' style='display:none'><td colspan='6'></td></tr>");
            $("#divItem").appendTo($("#form_placeholder td:first"));
            $("#form_placeholder").show("slow");
            $("#PostCategory").focus();
            $("#btnAddCostItem").prop("disabled", true);
            $(".btnEditCostItem").prop("disabled", true);
            $(".btnRemoveCostItem").prop("disabled", true);

            $('html, body').animate({
                scrollTop: $("#form_placeholder").offset().top - 150
            }, 0);
        });

        $("#btnSimpanCostItem").click(function () {
            var _data = $("#frmCostItem").serialize();
            var _url = "/Finance/ValidateCostItem";
            $.ajax({
                type: "POST",
                data: _data,
                url: _url,
                success: _validateCostItemCallback
            });
        });



        $("#btnBatalCostItem").click(function () {
            $("#frmCostItem").trigger("reset");
            $("#divItem").appendTo($("#divItemFormPlaceHolder"));
            $("#form_placeholder").remove();
            tblProjectCostItem.draw();
            $("#btnAddCostItem").prop("disabled", false);
            $(".btnEditCostItem").prop("disabled", false);
            $(".btnRemoveCostItem").prop("disabled", false);
            $("#btnAddCostItem").focus();
            editedProjectCostItemIndex = -1
        });

        $("#btnSaveCashForm").click(function () {
            showSavingNotification("Menyimpan data pembayaran");

            var _dataItems = tblProjectCostItem.data();
            var _dataToSend = [];
            $(_dataItems).each(function (d, e) {
                e.ProjectCostRecordId = 0;
                _dataToSend.push(e);
            });
            $("#PaymentItems").val(JSON.stringify(_dataToSend));
            $.ajax({
                type: "POST",
                data: $("#frmProjectCost").serialize(),
                url: "/Finance/SaveCashForm",
                success: function (data) {
                    $.notifyClose();
                    if (data.stat == 1) {
                        $(".btnBack").trigger("click");
                        _loadData();
                    } else {
                        showNotificationSaveError(data);
                    }
                }
            });

        });


        $("#btnPrint").click(function () {
            var projectId = $("#ProjectInfoId").val();
            window.location = "/ProjectManagement/Finance/PrintProjectCost/" + projectId;
        });

        //datatable form


        var _renderEditColumn = function (data, type, row) {
            if (type == "display") {
                return "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-default btn-xs btnEditCostItem' ><i class='fa fa-edit'></i></button>" +
                            "<button type='button' class='btn btn-default btn-xs btnRemoveCostItem'><i class='fa fa-remove'></i></button>" +
                            "</div>";
            }
            return data;
        }

        var arrColumns = [
            { "data": "Id", "sClass": "text-center" }, //
              {"data": "PostCategory", "sClass": "text-center" }, //
              {"data": "ItemDescription" }, //
              {"data": "Value", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //
              {"data": "Notes", "sClass": "text-center" }, //
              {"data": "Id", "sClass": "text-center", "mRender": _renderEditColumn}//
        ];
        var _coldefs = [];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {

            //show row number
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }

            //calculate total
            var api = this.api();
            var total = api.column(3, { page: 'current' }).data().sum();
            total = $.number(total, 2, ",", ".");
            $(api.table().footer()).find("tr:first td").eq(1).html(total);



        };


        tblProjectCostItem = $("#tblProjectCostItem").DataTable(datatableDefaultOptions)
        .on("click", ".btnRemoveCostItem", function (d) {

            if (confirm("Hapus item ini ?") == false) {
                return;
            }

            var tr = $(this).closest('tr');
            var row = tblProjectCostItem.row(tr);
            if (row.data().Id == 0) {
                row.remove().draw()
                return;
            }
            //else
            $.ajax({
                type: "POST",
                data: { id: row.data().Id },
                url: "/Finance/DeleteCostItem",
                success: function (data) {
                    row.remove().draw();
                }
            });


        })
        .on("click", ".btnEditCostItem", function (d) {
            var tr = $(this).closest('tr');
            var row = tblProjectCostItem.row(tr);
            $("<tr id='form_placeholder' style='display:none'><td colspan='6'></td></tr>").insertAfter(tr);
            $("#divItem").appendTo($("#form_placeholder td:first"));
            $("#form_placeholder").show("slow");

            $("#PostCategory").focus();
            $("#btnAddCostItem").prop("disabled", true);
            var dataItem = row.data();
            editedProjectCostItemIndex = row.index();
            $("#PostCategory").typeahead('val', dataItem.PostCategory);
            $("#ItemDescription").val(dataItem.ItemDescription);
            $("#Value").val(dataItem.Value);
            $("#Notes").val(dataItem.Notes);
            $("#ProjectCostRecordItemId").val(dataItem.Id);
        });


        //tblProjectCostWeeklyList
        var _arrCostFields = [
              { "data": "ProjectCostRecordId", "sClass": "text-center" }, //
              {"data": "TransactionDate", "sClass": "text-center", "mRender": _fnRenderNetDate }, //
              {"data": "PostCategory", "sClass": "text-center" }, //
              {"data": "ItemDescription" }, //
              {"data": "Value", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //
              {"data": "Notes" }, //
              {"data": "Id", "sClass": "text-center", "mRender": function (data, type, row) { return ""; } }, //

        ]
        datatableDefaultOptions.aoColumns = _arrCostFields;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }
            var api = this.api();

            if (parseInt(api.data().length, 0) > 0) {
                var datastart = api.row(0).data();
                var dtStart = new Date(parseInt(datastart.WeekDateStart.substr(6)));
                var dtEnd = new Date(parseInt(datastart.WeekDateStart.substr(6)));
                dtEnd.setDate(dtEnd.getDate() + 6);
                var currentdate = null;
                var PaymentId = -1;
                var totalCounter = -1;
                var arrPeriodTotal = [0];
                var PeriodTotal = 0;
                var rows = api.rows({ page: 'current' }).nodes();

                api.column(6, {
                    page: 'current'
                }).data().each(function (group, i) {
                    var datarow = api.row(i).data();

                    currentdate = new Date(parseInt(datarow.WeekDateStart.substr(6)));



                    if (dtStart <= currentdate && currentdate <= dtEnd) {

                        if (totalCounter >= 0) {
                            $(rows).eq(i).before("<tr><td colspan='7'>&nbsp;</td></tr>");
                        }
                        var _htmlTr = "<tr class='tr_weekgroup'><td colspan='4'>";
                        _htmlTr += "Periode " + $.datepicker.formatDate('dd-mm-yy', dtStart);
                        _htmlTr += " s/d " + $.datepicker.formatDate('dd-mm-yy', dtEnd);
                        _htmlTr += "</td><td class='text-right' id='totalcounter" + (totalCounter + 1) + "' colspan='1'>0000</td><td></td><td></td></tr>";

                        $(rows).eq(i).before(_htmlTr);
                        dtStart.setDate(dtStart.getDate() + 7);
                        dtEnd.setDate(dtEnd.getDate() + 7);

                        totalCounter++;
                        arrPeriodTotal[totalCounter] = 0;
                    }

                    arrPeriodTotal[totalCounter] += datarow.Value;

                    if (PaymentId != datarow.ProjectCostRecordId) {
                        var _htmlTr = "<tr class='tr_paymentgroup'><td style='padding-left:50px;' colspan='6'>";
                        _htmlTr += datarow.TaskGroupTitle;
                        _htmlTr += "</td><td class='text-center'>" +
                        "<div class='btn-group' role='group'>" +
                        "<button type='button' class='btn btn-default btn-xs btnEditCost' data-id='" + datarow.ProjectCostRecordId + "' ><i class='fa fa-edit'></i></button>" +
                        "<button type='button' class='btn btn-default btn-xs btnDeleteCost' data-id='" + datarow.ProjectCostRecordId + "' ><i class='fa fa-remove'></i></button>" +
                        "</div>" +
                        "</td></tr>";
                        $(rows).eq(i).before(_htmlTr);
                        PaymentId = datarow.ProjectCostRecordId;
                    }



                });



                for (var i = 0; i < arrPeriodTotal.length; i++) {
                    $("#totalcounter" + (i)).text($.number(arrPeriodTotal[i], 2, ",", "."));

                }

            }



        }

        tblProjectCostWeeklyList = $("#tblProjectCostWeeklyList").DataTable(datatableDefaultOptions)
         .on("click", ".btnDeleteCost", function () {
             if (confirm("Hapus item ini ?")) {
                 $.ajax({
                     type: "POST",
                     data: { id: $(this).data("id") },
                     url: "/Finance/DeleteProjectCost",
                     success: function (data) {
                         if (data.stat == 1) {
                             _loadData();
                         }
                     }
                 });

             }
         })
        .on("click", ".btnEditCost", function () {
            var itemData = new Array();

            var ProjectCostId = $(this).data("id");
            tblProjectCostItem.clear();
            var headerData = null;
            tblProjectCostWeeklyList.data().each(function (d) {
                if (d.ProjectCostRecordId == ProjectCostId) {
                    var oItem = {
                        Id: d.Id,
                        ProjectCostRecordId: d.ProjectCostRecordId,
                        PostCategory: d.PostCategory,
                        ItemDescription: d.ItemDescription,
                        Value: d.Value,
                        Notes: d.Notes
                    };
                    tblProjectCostItem.row.add(oItem);
                    if (headerData == null) {
                        headerData = d;
                    }
                }
            })

            ;


            $(".btnAdd").trigger("click");
            $("#ProjectInfoId").val(headerData.ProjectInfoId);
            $("#ProjectCostRecordId").val(headerData.ProjectCostRecordId);
            $("#TaskGroupTitle").typeahead('val', headerData.TaskGroupTitle);
            $("#PacketCode").val(headerData.PacketCode);
            var dtStart = new Date(parseInt(headerData.TransactionDate.substr(6)));

            var dtStartWrapper = $.datepicker.formatDate('dd-mm-yy', dtStart);
            //$("#TransactionDate").val(dtStartWrapper);
            $('#dtpk_TransactionDate').datepicker('update', dtStart);
            tblProjectCostItem.draw();

        });
        _loadData();




    });                                                                         //end init scritp;
</script>
