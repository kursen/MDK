@ModelType Purchasing.PurchaseOrder
@Code
    ViewData("Title") = "Purchase Order"
   
    Dim dateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left",
                               .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
   
End Code
@Using Html.BeginForm("Save", "PurchaseOrder", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmPurchase"})

    @<input type='hidden' name='Id' id='documentId' value="@Model.ID" />
    @<input type="hidden" name="ItemPODetail" id="PRDetailItems" value="" />
    @<input type="hidden" name="Vendor_Id" id="Vendor_Id" value="0" />
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            @Using Html.BeginJUIBox("Dasar Permintaan")
                    @Html.Hidden("POType", 1)
            @*    @Html.WriteFormInput(Html.DropDownList("POType", Nothing, Nothing, New With {.class = "form-control"}), "Tipe Permintaan", smControlWidth:=8)*@
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.RQNumber, New With {.class = "form-control"}), "No. Permintaan")      
    End Using
            @Using Html.BeginJUIBox("PO Information")
                @Html.WriteFormDateInputFor(Function(m) m.OrderDate, "Tanggal PO", dateFormat, smControlWidth:=4, lgControlWidth:=2)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.OrderNumber, New With {.class = "form-control"}), "No. PO", smControlWidth:=8)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.QuotationRef, New With {.class = "form-control"}), "Quotation Ref", smControlWidth:=8)
                @Html.WriteFormDateInputFor(Function(m) m.DeliveryDate, "Tgl Kebutuhan", dateFormat,
                                 smControlWidth:=4, lgControlWidth:=2)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.TermOfPayment, New With {.class = "form-control"}), "Term Of Payment", smControlWidth:=8)
                @Html.WriteFormInput(Html.DropDownList("Currency", Nothing, Nothing, New With {.class = "form-control"}), "Mata Uang", smControlWidth:=8)
    End Using
            @Using Html.BeginJUIBox("Vendor")
                @<div class="row">
                    <div class="col-lg-6 col-sm-12">
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Vendor_CompanyName,
                                                     New With {.class = "form-control"}), "Nama Perusahaan", lgControlWidth:=8, smControlWidth:=8)
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Vendor_ContactName,
                                                     New With {.class = "form-control"}), "Nama Kontak", lgControlWidth:=8, smControlWidth:=8)
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Vendor_Phone,
                                                     New With {.class = "form-control"}), "No. Telefon", lgControlWidth:=8, smControlWidth:=8)
                    </div>
                    <div class="col-lg-6 col-sm-12">
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Vendor_Address,
                                                     New With {.class = "form-control"}), "Alamat", lgControlWidth:=8, smControlWidth:=8)
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Vendor_City,
                                                     New With {.class = "form-control"}), "Kota", lgControlWidth:=8, smControlWidth:=8)
                        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Vendor_Province,
                                                     New With {.class = "form-control"}), "Provinsi", lgControlWidth:=8, smControlWidth:=8)
                    </div>
                </div>
                
    End Using
    @Using Html.BeginJUIBox("Tujuan Pengiriman")
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.DeliveryTo, New With {.class = "form-control"}), "Dikirim ke", smControlWidth:=8)
                @Html.WriteFormInput(Html.TextAreaFor(Function(m) m.DeliveryAddress, New With {.class = "form-control"}), "Alamat", smControlWidth:=8)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ContactPerson_Name, New With {.class = "form-control"}), "Atas Nama", smControlWidth:=8)
                @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ContactPerson_Phone, New With {.class = "form-control"}), "No. Telefon", smControlWidth:=8)
    End Using
        </div>
    </div>
    
End Using
@Using Html.BeginJUIBox("Items")
    @<div class="table-responsive">
        <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
            id="tblPODetail">
            <colgroup>
                <col style="width: 60px" />
                <col style="width: auto" />
                <col style="width: 60" />
                <col style="width: 60" />
                <col style="width: 60" />
                <col style="width: 200" />
                <col style="width: 60" />
                <col style="width: 60" />
                <col style="width: 80" />
            </colgroup>
            <thead>
                <tr>
                    <th>
                        No.
                    </th>
                    <th style="width: 300px">
                        Nama Item
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
                        <button type="button" class="btn btn-danger btn-label-left" id="btnPRDetail">
                            <span><i class="fa fa-plus"></i></span>Tambah</button>
                    </th>
                </tr>
            </thead>
            <tfoot>
                <tr style="border-top: 1px solid #000;">
                    <td>
                    </td>
                    <td colspan="6">
                        TOTAL
                    </td>
                    <td class="text-right">
                        0,00
                    </td>
                    <td>
                    </td>
                </tr>
                <tr style="border-top: 1px solid #000;" class='ppn-row'>
                    <td>
                    </td>
                    <td colspan="6">
                        PPn 10%
                    </td>
                    <td class="text-right" id='ppn10'>
                        0,00
                    </td>
                    <td>
                    </td>
                </tr>
                <tr style="border-top: 1px solid #000;" class='ppn-row'>
                    <td>
                    </td>
                    <td colspan="6">
                        TOTAL Setelah PPn
                    </td>
                    <td class="text-right" id='ppnafter'>
                        0,00
                    </td>
                    <td>
                    </td>
                </tr>
            </tfoot>
        </table>
        <div class='checkbox-inline'>
        <label >
            <input type='checkbox' value='true' name='PPnAdded' id='PPnAdded' @iif(Model.PPnAdded,"checked='checked'","")  autocomplete='off' />
            PPn 10%  <i class="fa fa-square-o"></i></label>
        </div>
    </div>
    @<div id="formPRDetail" style="display: none;">
        @Using Html.BeginForm("PODetailValidation", "PurchaseOrder", Nothing, FormMethod.Post,
                            New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmPRDetail"})
                    
            @Html.Hidden("PurchasingOrderId", 0)
            @Html.Hidden("PRDetailId", 0)
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
                    Harga/Satuan
                </label>
                <div class="col-lg-1 col-sm-2">
                    <span id="spCurrency"></span>
                </div>
                <div class="col-lg-2 col-sm-4">
                    @Html.DecimalInput("UnitPrice", 0, ".", ",", 2)
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Total Harga
                </label>
                <div class="col-lg-2 col-sm-4">
                    @Html.DecimalInput("TotalPrice", 0, ".", ",", 2)
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
@Using Html.BeginJUIBox("Catatan/Remarks")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            @Html.TextAreaFor(Function(m) m.Remarks, New With {.autocomplete = "off"})
        </div>
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
                    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.PreparedBy_Name, New With {.class = "form-control person"}), "Nama", smControlWidth:=8)
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
                    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ApprovedBy_Name, New With {.class = "form-control person"}), "Nama", smControlWidth:=8)
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
                            @Purchasing.GlobalArray.PurchaseOrderDocState(0) <i class="fa fa-circle-o"></i>
                        </label>
                    </div>
                    @<div class="radio-inline">
                        <label>
                            @Html.RadioButton("DocState", 1, Model.DocState = 1, New With {.autocomplete = "off"})
                            @Purchasing.GlobalArray.PurchaseOrderDocState(1)<i class="fa fa-circle-o"></i>
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
        <a href="@Url.Action("Index", "PurchaseOrder")" class="btn btn-default" >Batal</a>
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
<script src="../../../../plugins/tinymce/tinymce.min.js" type="text/javascript"></script>
<style>
    .ui-autocomplete
    {
        height: 200px;
        overflow-y: scroll;
        overflow-x: hidden;
    }
</style>
<script type="text/javascript">
    var tblPODetail = null;


    var _loadItems = function () {
        var _id = $("#documentId").val();
        if (parseInt(_id) == 0) {
            return;
        }
        $.ajax({
            type: "POST",
            data: { id: _id },
            url: "/Purchasing/PurchaseOrder/GetPOItems",
            datatype: 'json',
            success: function (data) {
                tblPODetail.clear();
                tblPODetail.rows.add(data.data).draw();
            }
        });


    }

    var _loadRequestItems = function () {
        var _id = $("#RQNumber").val();
        if (parseInt(_id) == 0) {
            return;
        }
        $.ajax({
            type: "POST",
            data: { id: _id },
            url: "/Purchasing/PurchaseOrder/GetRequestItems",
            datatype: 'json',
            success: function (data) {
                tblPODetail.clear();
                tblPODetail.rows.add(data.data).draw();
            }
        });


    }

    var _initTblPODetail = function () {
        /*table Operation*/
        var _renderEditColumnPRDetail = function (data, type, row) {
            if (type == "display") {
                return "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-default btn-xs btnEditPRDItem' ><i class='fa fa-edit'></i></button>" +
                            "<button type='button' class='btn btn-default btn-xs btnRemovePRDItem'><i class='fa fa-remove'></i></button>" +
                            "</div>";
            }
            return data;
        };
        var arrColumns = [
              { "data": "PurchaseOrderId", "sClass": "text-right" }, //0
              {"data": "ItemName" }, //1
              {"data": "Quantity", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //2
              {"data": "UnitQuantity" }, //3
              {"data": "Currency", "sClass": "text-right" }, //4
              {"data": "UnitPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //5
               {"data": "Currency", "sClass": "text-right" }, //6
              {"data": "TotalPrice", "sClass": "text-right", "mRender": _fnRender2DigitDecimal }, //7
              {"data": "Id", "mRender": _renderEditColumnPRDetail, "sClass": "text-center"}//8
        ];

        var _coldefs = [];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.rowId = "DT_RowId";
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            //show row number
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }

            //            calculate total
            var api = this.api();
            var total = api.column(7, { page: 'current' }).data().sum();
            var stotal = $.number(total, 2, ",", ".");
            $(api.table().footer()).find("tr:first td").eq(2).html(stotal);

            $("#ppn10").html($.number(total * 0.1, 2, ",", "."));
            $("#ppnafter").html($.number(total + (total * 0.1), 2, ",", "."));

        };

        tblPODetail = $("#tblPODetail").DataTable(datatableDefaultOptions)
        .on("click", ".btnRemovePRDItem", function (d) {
            if ($('#formPRDetail').is(':visible')) {
                $('#formPRDetail').hide();
                $('#btnPRDetail').show('fast');
            }

            if (confirm("Hapus item ini ?") == false) {
                return;
            }

            var tr = $(this).closest('tr');
            var row = tblPODetail.row(tr);

            if (row.data().Id == 0) {
                row.remove().draw()
                return;
            }
            else {
                //DeleteRequestItem
                $.ajax({
                    type: "POST",
                    data: { id: row.data().ID },
                    url: "/Purchasing/PurchaseOrder/DeleteRequestItem",
                    datatype: 'json',
                    success: function (data) {
                        if (data.stat == 1) {
                            row.remove().draw()
                            return;
                        }
                    }
                });
            }
        })
        .on("click", ".btnEditPRDItem", function (d) {

            var tr = $(this).closest('tr');
            var row = tblPODetail.row(tr);
            var dataItem = row.data();
            $("#rowIdx").val(row.index());
            $("#ItemId").val(dataItem.Id);
            $("#ItemName").val(dataItem.ItemName);
            $("#Quantity").val(dataItem.Quantity);
            $("#UnitQuantity").val(dataItem.UnitQuantity);
            $("#UnitPrice").val(dataItem.UnitPrice);
            $("#TotalPrice").val(dataItem.TotalPrice);
            $("#spCurrency").val($("#Currency").val());
            $("#PRDetailId").val(dataItem.PRDetailId);
            $("#formPRDetail").show("slow");
            $("#ItemName").focus();
            $("#btnSavePRDetail").text("Update");
        });


    }



    var _initTextEditor = function () {
        tinymce.init({
            selector: '#Remarks',
            valid_elements: "p,br,b,i,strong,em,ul,li",
            height: 200,
            toolbar: "bold italic numlist",
            menubar: "false",
            image_advtab: false,
            content_css: "/Areas/Purchasing/css/contentTinyMCE.css"
        });
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

    _loadVendorPrice = function () {
        if (parseInt($("#Vendor_Id").val()) > 0) {
            //GetVendorPriceComparison


            $.ajax({
                type: "POST",
                data: { vendorId: $("#Vendor_Id").val(), rqnumber: $("#RQNumber").val() },
                url: "/Purchasing/PurchaseOrder/GetVendorPriceComparison",
                datatype: 'json',
                error: ajax_error_callback,
                success: function (data) {

                    $(data).each(function (i, d) {
                        //PRDetailID
                        var rowdata = tblPODetail.row("#row_" + d.PRDetailID).data();
                        console.log(rowdata);
                        rowdata.UnitPrice = d.Price;
                        rowdata.TotalPrice = d.Price * rowdata.Quantity;
                        tblPODetail.row("#row_" + d.PRDetailID).invalidate();


                    });
                    tblPODetail.draw();
                    var _tdata = tblPODetail.data();
                    console.log(_tdata);
                }
            });

        };

    }             ///_loadVendorPrice

    $(document).ready(function () {

        $("#Currency").change(function () {
            $("#spCurrency").text($(this).val());

            var arrData = tblPODetail.data();
            for (var i = 0; i < arrData.length; i++) {
                arrData[i].Currency = $("#Currency").val();
            }

            tblPODetail.clear();
            tblPODetail.rows.add(arrData);
            tblPODetail.draw();

        });

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

        //Submit Purchase
        $('#btnSavePurchase').click(function (e) {

            var _data = $('#frmApproval').serializeArray();


            showSavingNotification("Menyimpan data");
            //Operation
            var _dataItemsPRDetail = tblPODetail.data();
            var _dataPRDetailend = []
            $(_dataItemsPRDetail).each(function (d, e) {
                _dataPRDetailend.push(e);
            });
            $('#PRDetailItems').val(JSON.stringify(_dataPRDetailend));

            var _data = $('#frmPurchase').serializeArray();
            _data.push({ name: "ApprovedBy_Name", value: $("#ApprovedBy_Name").val() });
            _data.push({ name: "PreparedBy_Name", value: $("#PreparedBy_Name").val() });
            _data.push({ name: "PPnAdded", value: $("#PPnAdded").is(":checked") });
            tinyMCE.triggerSave();
            _data.push({ name: "Remarks", value: $("#Remarks").val() });
            if ($('input[name=DocState]:checked').length > 0)
                _data.push({ name: "DocState", value: $('input[name=DocState]:checked').val() });
            var _url = $("#frmPurchase").attr('action');
            $.ajax({
                type: "POST",
                data: $.param(_data),
                url: _url,
                datatype: 'json',
                success: function (data) {
                    if (data.stat == 0) {
                        showNotificationSaveError(data);
                        return
                    } else {
                        window.location = "/Purchasing/PurchaseOrder/Detail/" + data.id;
                    }
                }
            });
        });





        $('#btnSavePRDetail').click(function () {
            var _data = $("#frmPRDetail").serializeArray();
            var _url = $("#frmPRDetail").attr('action');
            _data.push({ name: "Currency", value: $("#Currency").val() });
            $.ajax({
                type: "POST",
                data: $.param(_data),
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var rowIdx = data.rowIdx;

                        if (rowIdx == -1) {
                            tblPODetail.row.add(data.model);
                        } else {
                            //this is editing;
                            var arrData = tblPODetail.data();
                            arrData.splice(rowIdx, 1, data.model);
                            tblPODetail.clear();
                            tblPODetail.rows.add(arrData);
                        }
                        tblPODetail.draw();
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
            });
        });


        $('#UnitPrice, #Quantity').blur(function () {
            $('#TotalPrice').val($('#UnitPrice').val() * $('#Quantity').val());
        });

        $("#Vendor_CompanyName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: '/Purchasing/PurchaseOrder/GetVendorInfo',
                    data: { term: $("#Vendor_CompanyName").val() },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Name,
                                value: obj.Name,
                                ID: obj.Id,
                                ContactName: obj.ContactName,
                                Address: obj.Address,
                                City: obj.City,
                                Province: obj.Province,
                                Phone: obj.Phone
                            }
                        }));
                    }
                });
            },
            select: function (event, ui) {
                $("#Vendor_CompanyName").val(ui.item.value);
                return false;
            },
            change: function (event, ui) {
                if (ui.item != null) {
                    $("#Vendor_ContactName").val(ui.item.ContactName);
                    $("#Vendor_Address").val(ui.item.Address);
                    $("#Vendor_City").val(ui.item.City);
                    $("#Vendor_Province").val(ui.item.Province);
                    $("#Vendor_Phone").val(ui.item.Phone);
                    $("#Vendor_Id").val(ui.item.ID);

                    _loadVendorPrice();
                } else {
                    $("#Vendor_Id").val(0);
                }
            }
        })
        .data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.value + "</a>")
                .appendTo(ul);
        };
        $("#ItemName").autocomplete({
            source: "/Purchasing/PurchaseOrder/GetProposedGoodList/",
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


        $("#RQNumber").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: '/Purchasing/PurchaseOrder/SearchRequisition',
                    data: { term: $("#RQNumber").val(), "ReqType": $("#POType").val() },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.RecordNo,
                                value: obj.RecordNo,
                                rqId: obj.ID,
                                requestedByName: obj.RequestedBy_Name,
                                deliveryTo: obj.DeliveryTo,
                                deliveryDate: obj.DeliveryDate,
                                deliveryAddress: obj.DeliveryAddress,
                                Description: obj.Description
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item != null) {
                    //get requisition item;
                    $("#DeliveryTo").val(ui.item.deliveryTo);
                    $("#dtpk_DeliveryDate").datepicker("update", moment(ui.item.deliveryDate).toDate());

                    $("#DeliveryAddress").val(ui.item.deliveryAddress);
                    $("#ContactPerson_Name").val(ui.item.requestedByName);

                    $.ajax({
                        type: "POST",
                        url: '/Purchasing/PurchaseOrder/GetRequisitionItem',
                        data: { "ReqNo": ui.item.value, "ReqType": $("#POType").val() },
                        dataType: 'json',
                        success: function (data) {
                            for (var i = 0; i < data.length; i++) {
                                data[i].PurchasingOrderId = 0;
                                data[i].Currency = $("#Currency").val();
                                tblPODetail.row.add(data[i]);
                            }
                            tblPODetail.draw();
                        }
                    });
                }
            }

        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.label + "<label style='display:block'>" + item.Description + "</label></a>")
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
                    }
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .append("<a>" + item.value + ", " + item.label + "</a>")
                .appendTo(ul);
            };


        }); //end init autocomplete




        _initTblPODetail();
        _initUnitQuantity()
        _loadItems();
        _initTextEditor();
        $("#PPnAdded").change(function () {
            if ($(this).is(":checked")) {
                $(".ppn-row").show();

            } else {
                $(".ppn-row").hide();
            }

        });
        $("#Currency").trigger("change");
        $("#PPnAdded").trigger("change");
    });                                                                     //end init;
</script>
