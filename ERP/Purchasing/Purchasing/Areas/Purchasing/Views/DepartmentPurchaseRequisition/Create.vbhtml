@ModelType Purchasing.DepartmentPurchaseRequisition
@Code
    ViewData("Title") = "Data Baru Permintaan Pembelian " & ViewData("officeName")
    ViewData("ActivePath") = "/Purchasing/DepartmentPurchaseRequisition/Index"
    Dim BreadCrumb = New List(Of MvcHtmlString)
    BreadCrumb.Add(Html.ActionLink("Permintaan Pembelian", "Index", "ProjectPurchasingRequesition"))
    ViewData("Breadcrumb") = BreadCrumb
    Dim i As Integer = 0
End Code

@Using Html.BeginForm("Save", "DepartmentPurchaseRequisition", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmPurchase"})
    Using Html.BeginJUIBox("Data Baru Permintaan Pembelian " & ViewData("officeName"))
     @<input type='hidden' name='Id' id='documentId' value="@Model.Id" />
    @Html.HiddenFor(Function(m) m.OfficeID)
    
    @<div class="row">
        
        @Html.WriteFormDateInputFor(Function(m) m.RequestDate, "Tanggal",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                     smControlWidth:=4, lgControlWidth:=2)
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.RecordNo, New With {.class = "form-control"}), "No. Dokumen")
            
            @Html.WriteFormInput(Html.DropDownList("RequestTypeId", Nothing, Nothing, New With {.class = "form-control"}), "Tipe Permintaan")
            @Html.WriteFormDateInputFor(Function(m) m.DeliveryDate, "Tgl Kebutuhan",
                                New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                            .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                 smControlWidth:=4, lgControlWidth:=2)
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.DeliveryTo, New With {.class = "form-control"}), "Dikirim ke")
            @Html.WriteFormInput(Html.TextAreaFor(Function(m) m.DeliveryAddress, New With {.class = "form-control"}), "Alamat")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.RequestedBy_Name, New With {.class = "form-control person"}), "Diajukan Oleh")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.RequestedBy_Occupation, New With {.class = "form-control"}), "Jabatan")
            <input type="hidden" name="ItemPRDetail" id="PRDetailItems" value="" />
        
    </div>
    End Using
End Using
@Using Html.BeginJUIBox("Detail Permintaan Pembelian", True, False, False, False, False, "")
    @<div class="table-responsive">
	<table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
        id="tblPRDetail">
         <colgroup>
                <col style="width: 60px" />
                <col style="width: auto" />
                <col style="width: auto" />
                <col style="width: 60" />
                <col style="width: 60" />
                <col style="width: 60" />
                <col style="width: 200" />
                <col style="width: 60" />
                <col style="width: 200" />
                <col style="width: 120" />
            </colgroup>
            <thead>
                <tr>
                    <th>
                        No.
                    </th>
                    <th style="width: 300px">
                        Nama Item
                    </th>
                    <th>
                        Alokasi
                    </th>
                    <th colspan="2">
                        Jumlah
                    </th>
                    <th colspan="2">
                        Harga
                    </th>
                    <th colspan="2">
                        Total Harga
                    </th>
                    <th>
                        Catatan
                    </th>
                    <th>
                        <button type="button" class="btn btn-danger btn-label-left" id="btnPRDetail">
                            <span><i class="fa fa-plus"></i></span>Tambah</button>
                    </th>
                </tr>
            </thead>
    </table>
	</div>
    @<div id="formPRDetail" style="display: none;">

                @Using Html.BeginForm("DepartmentPRDetailValidation", "DepartmentPurchaseRequisition", Nothing, FormMethod.Post,
                            New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmPRDetail"})
                    
            @Html.Hidden("DepartmentPurchaseRequisitionId", 0)
            @<input type='hidden' name='Id' id='ItemId' value='0' />
            @Html.Hidden("rowIdx", -1)

            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Nama Item
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("ItemName", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Alokasi
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("Allocation", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Jumlah
                </label>
                <div class="col-lg-2 col-sm-3">
                    @Html.DecimalInput("Quantity", 0, ".", ",", 2)
                </div>
                <div class="col-lg-2 col-sm-3">
                    @Html.TextBox("UnitQuantity", "Unit", New With {.class = "form-control"})
                </div>
            </div>

            

            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Perkiraan Harga/Satuan
                </label>
                <div class="col-lg-1 col-sm-2">Rp.
                    @Html.Hidden("Currency","Rp")
                </div>
                <div class="col-lg-2 col-sm-4">
                    @Html.DecimalInput("EstUnitPrice", 0, ".", ",", 2)
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Total Harga
                </label>
                <div class="col-lg-2 col-sm-4">
                    @Html.DecimalInput("TotalEstPrice", 0, ".", ",", 2)
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Catatan
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("Remarks", "-", New With {.class = "form-control"})
                </div>
            </div>
          
            @<div class="form-group">
                <div class="col-sm-offset-3 col-sm-10">
                    <button type="button" class="btn btn-primary" id="btnSavePRDetail">
                        Tambahkan</button>
                    <button type="button" class="btn btn-default" onclick="$('#btnPRDetail').click()">
                        Tutup</button>
                </div>
            </div>
    End Using
    </div>
End Using
<form action="" onsubmit="return false" id='frmApproval' autocomplete="off">
<div class="row">
    <div class="col-lg-6 col-sm-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                DiKetahui Oleh
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Nama
                        </label>
                        <div class="col-lg-6 col-sm-8">
                            @Html.TextBoxFor(Function(m) m.KnownBy_Name, New With {.class = "form-control person"})
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Jabatan
                        </label>
                        <div class="col-lg-6 col-sm-8">
                            @Html.TextBoxFor(Function(m) m.KnownBy_Occupation, New With {.class = "form-control"})
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-6 col-sm-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Disetujui Oleh
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Nama
                        </label>
                        <div class="col-lg-6 col-sm-8">
                            @Html.TextBoxFor(Function(m) m.ApprovedBy_Name, New With {.class = "form-control person"})
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Jabatan
                        </label>
                        <div class="col-lg-6 col-sm-8">
                            @Html.TextBoxFor(Function(m) m.ApprovedBy_Occupation, New With {.class = "form-control"})
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</form>
<div class="row">
    <div class="col-sm-12 col-lg-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Status Dokumen
            </div>
            <div class="panel-body">
            @If Model.DocState > 1 Then
                Dim arrDocstate = Purchasing.GlobalArray.PurchaseRequisitionDocState()
                @Html.Raw(arrDocstate(Model.DocState - 1))
            Else
                @<div class="radio-inline">
                    <label>
                        @Html.RadioButton("DocState", 0, Model.DocState = 0, New With {.autocomplete = "off"})
                        @Purchasing.GlobalArray.PurchaseRequisitionDocState(0) <i class="fa fa-circle-o"></i>
                    </label>
                </div>
                @<div class="radio-inline">
                    <label>
                        @Html.RadioButton("DocState", 1, Model.DocState = 1, New With {.autocomplete = "off"})
                        @Purchasing.GlobalArray.PurchaseRequisitionDocState(1)<i class="fa fa-circle-o"></i>
                    </label>
                </div>
            End If
                
            </div>
        </div>
    </div>
</div>
<div class="well cell-center">
    
           <div class="center-block" style="width: 200px">
        <button type="button" class="btn btn-primary" id="btnSavePurchase">
            Simpan</button>
        <a href="@Url.Action("Index", "DepartmentPurchaseRequisition")" class="btn btn-default" >
            Batal</a>
    </div>
    
</div>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/datatables/dataTables.bootstrap.css")" />
<link href="../../../../plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css"
    rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.bundle.min.js")" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.css")" />
<style>
    .popover
    {
        min-width: 300px;
        min-height: 200px;
    }
</style>


<script type="text/javascript">
    var tblPRDetail = null;
    var _validatePRDetailCallback = function (data) {
        if (data.stat == 1) {
            var rowIdx = data.rowIdx;

            var arrData = tblPRDetail.data();
            if (rowIdx == -1) {
                arrData.splice(arrData.length, 0, data.model); //tblPRDetail.row.add(data.model);
            } else {
                //this is editing;
                arrData.splice(rowIdx, 1, data.model);
            }
                tblPRDetail.clear();
                tblPRDetail.rows.add(arrData);

            tblPRDetail.draw();
            $('#frmPRDetail').trigger("reset");
            $("#rowIdx").val(-1);
            $("#ItemId").val(0);
            $("#ItemName").focus();
            $(".form-group").removeClass("has-error");
            $("#btnSavePRDetail").text("Tambah");
        } else {
            showNotificationSaveError(data, "Penambahan gagal.");
        }
    }


    var _loadItems = function () {
        var _id = $("#documentId").val();
        if (parseInt(_id) == 0) {
            return;
        }
        $.ajax({
            type: "POST",
            data: { id: _id },
            url: "/Purchasing/DepartmentPurchaseRequisition/GetRequestItems",
            datatype: 'json',
            success: function (data) {
                tblPRDetail.clear();
                tblPRDetail.rows.add(data.data).draw();
            }
        });

    }

    /*table Operation*/
    var _renderEditColumnPRDetail = function (data, type, row) {
        if (type == "display") {
            return "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-default btn-xs btnEditPRDItem' ><i class='fa fa-edit'></i></button>" +
                            "<button type='button' class='btn btn-default btn-xs btnRemovePRDItem'><i class='fa fa-remove'></i></button>" +
                            "</div>";
        }
        return data;
    }

    var _renderRemarks = function (data, type, row) {
        if (type == 'display') {
            data = data == null ? "" : data;
            var content = "<a role='button'  data-trigger='focus' tabindex='0'  class='btn btn-lg btn-danger'  data-toggle='popover' title='Catatan' " +
                        "data-content='" + data + "' ><span class='fa fa-align-left'></span> </a>";
            return content;
        }
        return data;
    }
    var arrPRDetail = [
              { "data": "DepartmentPurchaseRequisitionId", "sClass": "text-right" }, //0
              {"data": "ItemName" }, //1
               {"data": "Allocation" }, //2
              {"data": "Quantity", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //3
              {"data": "UnitQuantity" }, //4
              {"data": "Currency", "sClass": "text-right" }, //5
              {"data": "EstUnitPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //6
               {"data": "Currency", "sClass": "text-right" }, //7
              {"data": "TotalEstPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //8
              {"data": "Remarks", "sClass": "text-center", "mRender": _renderRemarks }, //9
              {"data": "ID", "mRender": _renderEditColumnPRDetail, "sClass": "text-center"}//10
    ];
    var _GeneralTable = function (arrColumns) {
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
            $('[data-toggle="popover"]').popover({
                placement: "left",
                trigger: "manual",
                container: 'body'
            }).click(function (e) {
                $('.popover').not(this).popover('hide');
                $(this).popover('toggle');
            });
        };

    }

    //Submit Purchase
    $('#btnSavePurchase').click(function (e) {

        var _data1 = $('#frmApproval').serializeArray();


        showSavingNotification("Menyimpan data");
        //Operation
        var _dataItemsPRDetail = tblPRDetail.data();
        var _dataPRDetailend = []
        $(_dataItemsPRDetail).each(function (d, e) {
            _dataPRDetailend.push(e);
        });
        $('#PRDetailItems').val(JSON.stringify(_dataPRDetailend));

        var _data = $('#frmPurchase').serializeArray();
        _data.push({ name: "ApprovedBy_Name", value: $("#ApprovedBy_Name").val() });
        _data.push({ name: "ApprovedBy_Occupation", value: $("#ApprovedBy_Occupation").val() });
        _data.push({ name: "KnownBy_Name", value: $("#KnownBy_Name").val() });
        _data.push({ name: "KnownBy_Occupation", value: $("#KnownBy_Occupation").val() });
        if ($('input[name=DocState]:checked').length > 0)
            _data.push({ name: "DocState", value: $('input[name=DocState]:checked').val() });
        var _url = $("#frmPurchase").attr('action');
        $.ajax({
            type: "POST",
            data: $.param(_data),
            url: _url,
            datatype: 'json',
            success: submitPurchaseCallback
        });
    });
    var submitPurchaseCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else {
            window.location = "/Purchasing/DepartmentPurchaseRequisition/Detail/" + data.id;
        }
    }


    //
    var _initUnitQuantity = function () {

        var unitNames = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/Purchasing/Common/UnitQuantityName",
                prepare: function (query, settings) {
                    settings.type = "POST";
                    //settings.contentType = "application/json; charset=UTF-8";
                    settings.data = { query: query };
                    return settings;
                }
            }

        });
        unitNames.initialize();
        $("#UnitQuantity").typeahead(null, {
            name: "UnitQuantity",
            source: unitNames
        });

    };    //end _initQuantity


    $(document).ready(function () {
        $('#btnPRDetail').click(function () {
            $('#frmPRDetail').trigger("reset");
            $("#rowIdx").val(-1);
            $("#ItemId").val(0);

            $('#frmPRDetail,.form-group').removeClass('has-error');
            if ($('#formPRDetail').is(":hidden")) {
                $(this).hide();
            } else {
                $(this).show();
            }
            $('#formPRDetail').slideToggle();
            $("#btnSavePRDetail").text("Tambah");
        });

        $("#ProjectCode").change(function () {
            $("#ProjectTitle").val($(this).find(':selected').data('title'));

        });

        _GeneralTable(arrPRDetail);
        tblPRDetail = $("#tblPRDetail").DataTable(datatableDefaultOptions)
        .on("click", ".btnRemovePRDItem", function (d) {
            if ($('#formPRDetail').is(':visible')) {
                $('#formPRDetail').hide();
                $('#btnPRDetail').show('fast');
            }

            if (confirm("Hapus item ini ?") == false) {
                return;
            }

            var tr = $(this).closest('tr');
            var row = tblPRDetail.row(tr);

            if (row.data().Id == 0) {
                row.remove().draw()
                return;
            }
            else {
                //DeleteRequestItem
                $.ajax({
                    type: "POST",
                    data: { id: row.data().ID },
                    url: "/Purchasing/DepartmentPurchaseRequisition/DeleteRequestItem",
                    datatype: 'json',
                    success: function (data) {
                        if (data.stat == 1) {
                            row.remove().draw()
                            return;
                        } else {
                            shownotificationerror(data, "Penghapusan gagal");
                        }
                    }
                });
            }
        })
        .on("click", ".btnEditPRDItem", function (d) {
            var tr = $(this).closest('tr');
            var row = tblPRDetail.row(tr);
            var dataItem = row.data();
            $("#rowIdx").val(row.index());
            $("#ItemId").val(dataItem.ID);
            $("#ItemName").val(dataItem.ItemName);
            $("#Allocation").val(dataItem.Allocation);
            $("#Quantity").val(dataItem.Quantity);
            $("#UnitQuantity").val(dataItem.UnitQuantity);
            $("#Currency").val(dataItem.Currency);
            $("#EstUnitPrice").val(dataItem.EstUnitPrice);
            $("#TotalEstPrice").val(dataItem.TotalEstPrice);
            $("#Remarks").val(dataItem.Remarks);
            $("#formPRDetail").show("slow");
            $("#ItemName").focus();
            $("#btnSavePRDetail").text("Update");
        });


        $('#btnSavePRDetail').click(function () {
            var _data = $("#frmPRDetail").serialize();
            var _url = $("#frmPRDetail").attr('action');
            $('input[Docstate]:checked').val();
            $.ajax({
                type: "POST",
                data: _data,
                url: _url,
                success: _validatePRDetailCallback
            });
        });


        $('#EstUnitPrice, #Quantity').blur(function () {
            $('#TotalEstPrice').val($('#EstUnitPrice').val() * $('#Quantity').val());
        });
        $("#ItemName").autocomplete({
            source: "/Purchasing/DepartmentPurchaseRequisition/GetProposedGoodList/",
            data: { term: $("#ItemName").val() },
            minLength: 1,
            select: function (event, ui) {
                $("#ItemName").val(ui.item.value);
                return false;
            }
        })
        .data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.value + "</a>")
                .appendTo(ul);
        };

        $(".person").each(function () {
            $(this).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: '/Purchasing/Common/GlobalEmployeeNames',
                        data: request,
                        dataType: 'json',
                        success: function (data) {
                            response($.map(data, function (obj) {
                                return {
                                    label: obj.Value,
                                    value: obj.Text
                                }
                            }));
                        }
                    });
                },
                change: function (event, ui) {
                    var id = $(this).attr("id");
                    if (ui.item != null) {
                        $(this).val(ui.item.value);
                        switch (id) {
                            case "RequestedBy_Name":
                                $('#RequestedBy_Occupation').val(ui.item.label);
                                break;
                            case "KnownBy_Name":
                                $('#KnownBy_Occupation').val(ui.item.label);
                                break;
                            case "ApprovedBy_Name":
                                $('#ApprovedBy_Occupation').val(ui.item.label);
                                break;
                        }
                    } else {
                        switch (id) {
                            case "RequestedBy_Name":
                                $('#RequestedBy_Occupation').val('');
                                break;
                            case "KnownBy_Name":
                                $('#KnownBy_Occupation').val('');
                                break;
                            case "ApprovedBy_Name":
                                $('#ApprovedBy_Occupation').val('');
                                break;
                        }
                    }



                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.value + ", " + item.label + "</a>")
                .appendTo(ul);
            };


        }); //end init autocomplete


        _initUnitQuantity()
        _loadItems();
    });                            //end init;
</script>