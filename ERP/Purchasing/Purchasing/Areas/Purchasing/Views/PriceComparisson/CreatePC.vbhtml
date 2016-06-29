@ModelType Purchasing.modelviewPriceComparison
@Code
    Dim ctx As New Purchasing.PurchasingEntities
    
    Dim noUrut As Integer = 0
    Dim modelDetailPR As New List(Of Purchasing.DepartmentPRDetail)
    If Model IsNot Nothing Then
        modelDetailPR = (From dprd In ctx.DepartmentPRDetails Where dprd.DepartmentPurchaseRequisitionId = Model.PurchaseRequisitionID).ToList
    End If
    ViewData("Title") = "Perbandingan Harga"
    Dim defaultDateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                      .orientation = "top left", .todayBtn = "linked",
                                      .language = "id", .daysOfWeekDisabled = {0}}
End Code
@Using Html.BeginJUIBox("Daftar Permintaan Pembelian")
    Using Html.BeginForm("Save", "PriceComparisson", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmPriceComparison"})
        @Html.Hiddenfor(Function(m) m.PurchaseRequisitionID)
        @Html.WriteFormDateInputFor(Function(m) m.CreateDate, "Tanggal", defaultDateFormat, smControlWidth:=2, lgControlWidth:=2)
        @Html.WriteFormInput(Html.TextBox("NoRecord", Model.NoRecord , New With {.class = "form-control"}), "No Permintaan")
        @<div class="panel panel-primary">
            <div class="panel-heading">
                Perbandingan Harga
            </div>
            <table id="tblcomparison" class="table table-bordered">
                <colgroup>
                    <col style='width: 30px' />
                    <col style='width: 200px' />
                    <col style='width: 1px' />
                    <col style='width: 100px' />
                    <col style='width: auto' />
                    <col style='width: auto' />
                    <col style='width: auto' />
                </colgroup>
                <thead class="bg-default">
                    <tr>
                        <th rowspan="2">
                            No.
                        </th>
                        <th rowspan="2">
                            Item
                        </th>
                        <th rowspan="2">
                            Kuantitas
                        </th>
                        <th rowspan="2">
                            Harga
                        </th>
                        <th>
                            <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor I</a>
                            @Html.HiddenFor(Function(m) m.VendorID1, New With {.class = "vendorId"})
                        </th>
                        <th>
                            <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor II</a>
                            @Html.HiddenFor(Function(m) m.VendorID2, New With {.class = "vendorId"})
                        </th>
                        <th>
                            <a class="btn btn-default vendorpopover" data-toggle="popover">Vendor III</a>
                            @Html.HiddenFor(Function(m) m.VendorID3, New With {.class = "vendorId"})
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Harga Dasar
                        </th>
                        <th>
                            Harga Dasar
                        </th>
                        <th>
                            Harga Dasar
                        </th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In modelDetailPR
                        noUrut += 1
                            @<tr>
                                <td align="right">
                                    <input type="hidden" value="@item.ID" name="IDDetail"/>
                                    @noUrut.
                                </td>
                                <td>
                                    @item.ItemName 
                                </td>
                                <td align="right">
                                    @item.Quantity @item.UnitQuantity 
                                </td>
                                <td align="right">
                                    @item.Currency @Format(item.EstUnitPrice, "#,##0")
                                </td>
                                <td align="right">
                                    <input type="hidden" value="0" name="lstPrice1"/>
                                    @item.Currency <a href='javascript:void(0)' data-toggle='popover' name="Price1" class='EstUnitPricePopover'>0</a>
                                </td>
                                <td align="right">
                                    <input type="hidden" value="0" name="lstPrice2"/> 
                                    @item.Currency <a href='javascript:void(0)' data-toggle='popover' name="Price2" class='EstUnitPricePopover'>0</a>
                                </td>
                                <td align="right">
                                    <input type="hidden" value="0" name="lstPrice3"/> 
                                    @item.Currency <a href='javascript:void(0)' data-toggle='popover' name="Price3" class='EstUnitPricePopover'>0</a>
                                </td>
                            </tr>
                    Next
                </tbody>
                @*<tfoot>
                    <tr>
                        <th colspan="5" style="text-align: right">
                            Total:
                        </th>
                        <th style="text-align: right">
                        </th>
                        <th colspan="2" style="text-align: right">
                        </th>
                        <th colspan="2" style="text-align: right">
                        </th>
                    </tr>
                </tfoot>*@
                @* <tfoot>
                    <tr>
                        <th>Total</th>
                        <th class="totalest1"></th>
                        <th class="totalest2"></th>
                        <th class="totalest3"></th>
                    </tr>
                </tfoot>*@
            </table>
        @* <table id="tbltotalestprice">
            <colgroup>
                <col style='width: 480px' />
                <col style='width: 200px' />
                <col style='width: 220px' />
            </colgroup>
            <tr>
                <td>
                    <strong>Total</strong>
                </td>
                <td id="totalprice1">
                    0
                </td>
                <td>
                    0
                </td>
                <td>
                    0   
                </td>
            </tr>
            <tfoot>
                
            </tfoot>
        </table>*@
        </div>
        @<div class="well cell-center">
            <div class="center-block" style="width: 200px">
                <button type="submit" class="btn btn-primary" id="btnsaveNew">
                    Simpan</button>
                <a href="@Url.Action("Index", "DepartmentPurchaseRequisition")" class="btn btn-default" >
                    Batal</a>
            </div>
        </div>
    End Using
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var beginEstUnitPrice = 0;
    var _initcostpopover = function () {
        $('.EstUnitPricePopover').popover({
            title: 'Harga Item',
            placement: 'left',
            html: true,
            container: "#ajax-content",
            content: '<input type="text" name="EstUnitPrice" \
            id="id_EstUnitPrice" class="form-control" valye="" />'
        }).on('shown.bs.popover', function () {
            $('#id_EstUnitPrice').val($(this).text());
        }).
        on('hide.bs.popover', function () {
            beginEstUnitPrice = parseInt($('.popover #id_EstUnitPrice').val());
        }).
        on('hidden.bs.popover', function () {
            $(this).closest('td').find('a').text(beginEstUnitPrice);
            var inputPrice = $(this).closest('td').find('input');
            inputPrice.val(beginEstUnitPrice);
        });
    }

    var contentpopover = '<input type="text" name="VendorVal" id="vendorinput" class="form-control vendorautocom" />';
    var tempvendor, idvendor;
    //autocomplete vendor
    var _initautocompleteVendor = function () {
        $('.vendorautocom').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Purchasing/PriceComparisson/autocompleteVendor',
                    type: 'POST',
                    data: {
                        term: $('.vendorautocom').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.name,
                                value: obj.name,
                                id: obj.id
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                tempvendor = ui.item.label;
                idvendor = ui.item.id;
            }
        });
    }

    var submitFormActivityCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return;
        }
        else if (data.stat == 2) {
            //data already there
            showNotificationSaveError(data);
        } else {
            showNotificationSaveError(data);
        }
    };


    $(document).ready(function () {
        if ($('#NoRecord').val() == null || $('#NoRecord').val() == '') {
        }
        else {
            $('#NoRecord').attr('readonly', true);
        }

        _initcostpopover();
        $('.vendorpopover').popover({
            title: 'Tambah Vendor',
            placement: 'left',
            html: true,
            content: contentpopover
        }).
        on('shown.bs.popover', function () {
            _initautocompleteVendor();
            $('.vendorautocom').val($(this).text());
            tempvendor = $('.vendorautocom').val();
        }).
        on('hidden.bs.popover', function () {
            $(this).text(tempvendor);
            $(this).next('.vendorId').val(idvendor);
        });

//        $('#btnsave').click(function (e) {
//            var _data = $('#frmcomparison').serializeArray();

//            $.ajax({
//                type: 'POST',
//                data: _data,
//                url: '/Purchasing/PriceComparisson/SaveItemPriceComparison',
//                dataType: 'json',
//                success: submitFormActivityCallback
//            });
//        });

    });

</script>
