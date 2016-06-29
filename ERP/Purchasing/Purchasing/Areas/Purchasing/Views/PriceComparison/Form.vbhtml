@ModelType Purchasing.PriceComparison
@Code
    ViewData("Title") = "Price Comparison"
    Dim defaultDateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
    Dim NoRecord As String = String.Empty
    If Model.DepartmentPurchaseRequisition IsNot Nothing Then
        NoRecord = Model.DepartmentPurchaseRequisition.RecordNo
    End If
    
    Dim companyList = CType(ViewData("companyList"), List(Of Purchasing.Vendor))
    Dim vendor1 = companyList.Where(Function(m) m.Id = Model.VendorID1).SingleOrDefault()
    Dim vendor2 = companyList.Where(Function(m) m.Id = Model.VendorID2).SingleOrDefault()
    Dim vendor3 = companyList.Where(Function(m) m.Id = Model.VendorID3).SingleOrDefault()
    
    Dim v1Name As String="Pilih Vendor", v2Name As String="Pilih Vendor", v3Name As String="Pilih Vendor"
    If vendor1 IsNot Nothing Then
        v1Name = vendor1.Name
    End If
    If vendor2 IsNot Nothing Then
        v2Name = vendor2.Name
    End If
    If vendor3 IsNot Nothing Then
        v3Name = vendor3.Name
    End If
End Code
@Using Html.BeginJUIBox("Daftar Permintaan Pembelian")
    @<form name="formcomparison" id="frmcomparison" class="form-horizontal" method="post"
    action="" autocomplete="off">
    @Html.HiddenFor(Function(m) m.PurchaseRequisitionID, New With {.id = "spIDNoRecord"})
    @Html.HiddenFor(Function(m) m.VendorID1)
    @Html.HiddenFor(Function(m) m.VendorID2)
    @Html.HiddenFor(Function(m) m.VendorID3)
    @Html.HiddenFor(Function(m) m.ID, New With {.id = "PriceComparisonID"})
    <!--list arr send to controller -->
    <input type="hidden" name="listItemComparison" id="ItemComparisonSend" value="" />
    @Html.WriteFormDateInputFor(Function(m) m.CreateDate, "Tanggal", defaultDateFormat,
                                   smControlWidth:=2, lgControlWidth:=2)
    @Html.WriteFormInput(Html.TextBox("NoRecord",NoRecord, New With {.class = "form-control"}), "No Permintaan")
    <div class="panel panel-primary">
        <div class="panel-heading">
            Price Comparison
        </div>
        <table id="tblcomparison" class="table table-bordered">
            <colgroup>
                <col style='width: 40px' />
                <col style='width: auto' />
                <col style='width: 100px' />
                <col style='width: 100px' />
                <col style='width: 100px' />
                <col style='width: 100px' />
                <col style='width: 100px' />
                <col style='width: 100px' />
                <col style='width: 100px' />
                <col style='width: 100px' />
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
                        Satuan
                    </th>
                    <th colspan="2">
                        <a data-source="/Purchasing/PriceComparison/GetVendorList" data-title="Pilih Vendor" data-value="@Model.VendorID1"
                            data-type="select2" data-url="/Purchasing/PriceComparison/SavePartial" id="Vendor1"
                            data-id="49">@v1Name</a>
                    </th>
                    <th colspan="2">
                        <a data-source="/Purchasing/PriceComparison/GetVendorList" data-title="Pilih Vendor" data-value="@Model.VendorID2"
                            data-type="select2" data-url="/Purchasing/PriceComparison/SavePartial" id="Vendor2">
                            @v2Name</a>
                    </th>
                    <th colspan="2">
                        <a data-source="/Purchasing/PriceComparison/GetVendorList" data-title="Pilih Vendor" data-value="@Model.VendorID3"
                            data-placement="left" data-type="select2" data-url="/Purchasing/PriceComparison/SavePartial"
                            id="Vendor3">@v3Name</a>
                    </th>
                </tr>
                <tr>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Total
                    </th>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Total
                    </th>
                    <th>
                        Harga Dasar
                    </th>
                    <th>
                        Total
                    </th>
                </tr>
            </thead>
            <tfoot class="ftotal">
                <tr>
                    <td colspan="4" style="text-align: right">
                        Total:
                    </td>
                    <td style="text-align: right">
                    </td>
                    <td style="text-align: right">
                    </td>
                    <td style="text-align: right">
                    </td>
                    <td style="text-align: right">
                    </td>
                    <td style="text-align: right">
                    </td>
                    <td style="text-align: right">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    <div class="well cell-center">
        <div class="center-block" style="width: 200px">
            <button type="button" class="btn btn-primary" id="btnsave">
                Simpan</button>
            <a href="@Url.Action("Index", "PriceComparison")" class="btn btn-default" >
                Batal</a>
        </div>
    </div>
    </form>
End Using
<style>
    .select2-container
    {
        width: 300px;
    }
    .ftotal tr
    {
        border: 1px solid #c0c0c0;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tablecomparison = null;
    var dataFound = false;

    loadItemToCompare = function () {
        var _id = parseInt($('#spIDNoRecord').val());

        if (_id == 0 || isNaN(_id))
        { return; }

        $.ajax({
            url: '/Purchasing/PriceComparison/GetItemToPriceComparison',
            data: { id: _id },
            type: 'POST',
            success: function (d) {

                if (parseInt($("#PriceComparisonID").val()) == 0 && d.model.existing == 1) {
                    alert("Perbandingan harga untuk permintaan ini telah ada.");
                    $("#NoRecord").val("");
                    $('#spIDNoRecord').val(0);
                    return;
                }
                $("#PriceComparisonID").val(d.model.ID);
                $("#spIDNoRecord").val(d.model.PurchaseRequisitionID);
                $("#Vendor1").val(d.model.Vendor1);
                $("#Vendor2").val(d.model.Vendor2);
                $("#Vendor2").val(d.model.Vendor2);
                tablecomparison.clear();
                tablecomparison.rows.add(d.data).draw();
            },
            datatype: 'json'
        });
        dataFound = false;
    };

    var _initTblItem = function () {
        var _rendertotalEstprice = function (data, type, row) {

            if (type == 'display') {
                return $.number(data, 2, ",", ".");
            }
            return data;

        }

        var _renderpopovercost = function (data, type, row) {
            if (type == 'display') {
                return "<a data-type='text'  data-pk='" + row.ID + "' class='EstUnitPricePopover'>" + data + "</a>";
            }
            return data;
        }

        var arrColumnsOp = [
            { 'data': 'ID', 'sClass': 'text-right' }, //0
            {'data': 'ItemName' }, //1
            {'data': 'Quantity', 'sClass': 'text-right' }, //2
            {'data': 'UnitQuantity', 'sClass': 'text-left' }, //3
            {'data': 'Price1', 'sClass': 'text-right Price1', 'mRender': _renderpopovercost }, //4
            {'data': 'TotalEstPrice1', 'sClass': 'text-right', 'mRender': _rendertotalEstprice }, //5
            {'data': 'Price2', 'sClass': 'text-right Price2', 'mRender': _renderpopovercost }, //6
            {'data': 'TotalEstPrice2', 'sClass': 'text-right', 'mRender': _rendertotalEstprice }, //7
            {'data': 'Price3', 'sClass': 'text-right Price3', 'mRender': _renderpopovercost }, //8
            {'data': 'TotalEstPrice3', 'sClass': 'text-right', 'mRender': _rendertotalEstprice}//9
        ];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.bProcessing = true,
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.aoColumns = arrColumnsOp;
        datatableDefaultOptions.fnDrawCallback = function (oSettings, data) {
            $(".EstUnitPricePopover").editable({
                url: '/Purchasing/PriceComparison/ValidatePrice',
                params: function (params) {
                    if ($(this).closest("td").hasClass("Price1")) {
                        params.name = "Price1";
                    } else if ($(this).closest("td").hasClass("Price2")) {
                        params.name = "Price2";
                    } else params.name = "Price3";
                    params.rowindex = tablecomparison.cell($(this).closest("td")).index().row;
                    params.colindex = tablecomparison.cell($(this).closest("td")).index().column;
                    return params;
                },
                tpl: '<input type="text" id ="inputprice" class="numericinput form-control text-right" autocomplete="off" style="padding-right: 24px;">',
                display: function (value, sourceData) {
                    $(this).text($.number(value, 2, ",", "."));
                },
                success: function (d) {
                    tablecomparison.cell(d.rowindex, d.colindex).data(d.value).draw();
                    var _q = tablecomparison.cell(d.rowindex, 2).data(); //quantity
                    tablecomparison.cell(d.rowindex, d.colindex + 1).data(d.value * _q).draw();
                }

            }).on('shown', function (e, editable) {
                $(".numericinput").number(true, 2, ",", ".");
            });
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }
        };

        datatableDefaultOptions.footerCallback = function (tfoot, data, start, end, display) {
            var api = this.api();
            $(api.column(5).footer()).html($.number(api.column(5, { page: 'current' }).data().sum(), 2, ",", ".")); //Total Price Vendor I
            $(api.column(7).footer()).html($.number(api.column(7, { page: 'current' }).data().sum(), 2, ",", ".")); //Total Price Vendor II
            $(api.column(9).footer()).html($.number(api.column(9, { page: 'current' }).data().sum(), 2, ",", ".")); //Total Price Vendor III

        };

        tablecomparison = $('#tblcomparison').DataTable(datatableDefaultOptions);

    }  //end _initTblItem


    var _initautocompletenoRecord = function () {
        dataFound = false;
        $('#NoRecord').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Purchasing/PriceComparison/autocompleteRequestNumber',
                    type: 'POST',
                    data: {
                        term: $('#NoRecord').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Norecord,
                                value: obj.Norecord,
                                ID: obj.ID
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    //alert("No. Permintaan Tidak ditemukan");
                    $(".vendorpopover").each(function (e) {
                        $(this).attr("disabled", "disabled");
                    });
                    return;
                }
                $('#spIDNoRecord').val(ui.item.ID);
                $(".vendorpopover").each(function (e) {
                    $(this).removeAttr("disabled");
                });

                loadItemToCompare();
                dataFound = true;
            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            if (item.ID == 0)
                return ($('<li>').append(item.label).appendTo(ul));
            return ($('<li>').append('<a><strong>' + item.label + '</strong></a>').appendTo(ul));
        };
    }    //_initautocompletenoRecord



    var submitFormActivityCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return;
        }
        else if (data.stat == 2) {
            //data already there
            showNotificationSaveError(data);
        } else {
            window.location.href = '/Purchasing/PriceComparison/Detail/' + data.id;
        }
    }; //submitFormActivityCallback


    $(document).ready(function () {
        dataFound = false;
        //_initTblItem();
        if ($('#NoRecord').val() == null || $('#NoRecord').val() == '') {
            //initialize table item
            _initTblItem();
            _initautocompletenoRecord();
        }
        else {
            $('#NoRecord').attr('readonly', true);
            _initTblItem();

        }

        //hide error sedang memproses datatable
        $('.dataTables_processing').hide();

        $('#btnsave').click(function (e) {
            var _v1 = $("#Vendor1").editable('getValue').Vendor1;
            _v1 = isNaN(_v1) ? 0 : _v1;
            $("#VendorID1").val(_v1);
            var _v2 = $("#Vendor2").editable('getValue').Vendor2;
            _v2 = isNaN(_v2) ? 0 : _v2;
            $("#VendorID2").val(_v2);
            var _v3 = $("#Vendor3").editable('getValue').Vendor3;
            _v3 = isNaN(_v3) ? 0 : _v3;
            $("#VendorID3").val(_v3);

            var _dataItemPriceComparison = tablecomparison.data();
            var _dataItemPriceComparisonSend = [];

            $(_dataItemPriceComparison).each(function (d, e) {

                _dataItemPriceComparisonSend.push(e);
            });

            $('#ItemComparisonSend').val(JSON.stringify(_dataItemPriceComparisonSend));

            var _data = $('#frmcomparison').serializeArray();
            $.ajax({
                type: 'POST',
                data: _data,
                url: '/Purchasing/PriceComparison/SaveItemPriceComparison',
                dataType: 'json',
                error: ajax_error_callback,
                success: submitFormActivityCallback
            });
        });


        if ($("#spIDNoRecord").val() > 0) {
            loadItemToCompare();
        }

        $('#NoRecord').blur(function () {
            if ($('#NoRecord').val() == '') return;
            if (dataFound) { dataFound = false; return; }
            $.ajax({
                url: '/Purchasing/PriceComparison/GetRequestNumber',
                type: 'POST',
                data: {
                    NoRecord: $('#NoRecord').val()
                },
                dataType: 'json',
                success: function (data) {
                    tablecomparison.clear().draw();
                    if (data == 0) {
                        alert('No Permintaan tidak ditemukan');
                        return;
                    }
                    if (data == -1) {
                        alert('No Permintaan ini masih berstatus draft');
                        return;
                    }
                    if (data == -2) {
                        alert('No Permintaan ini belum disetujui');
                        return;
                    }
                    if (data == -3) {
                        alert('No Permintaan sudah memiliki data perbandingan harga');
                        return;
                    }
                    $('#spIDNoRecord').val(data);
                    $(".vendorpopover").each(function (e) {
                        $(this).removeAttr("disabled");
                    });

                    loadItemToCompare();
                }
            });
        });
    });              //end init

</script>
<link href='/plugins/bootstrap-editable/bootstrap-editable.css' rel='stylesheet' />
<script src="/plugins/bootstrap-editable/bootstrap-editable.js" type="text/javascript"></script>
<script src="../../../../plugins/select2/select2.min.js" type="text/javascript"></script>
<script type="text/javascript">$(function(){

	
	    $('#Vendor1, #Vendor2, #Vendor3').editable({
        select2: {
                placeholder: 'Pilih Vendor',
                allowClear: true,
                placement:'bottom',

                id: function (item) {
                    return item.value;
                },
                ajax: {
                    type: 'POST',
                    url: '/Purchasing/PriceComparison/GetVendorList',
                    dataType: 'json',
                    data: function (term, page) {
                        return { query: term };
                    },
                    results: function (data, page) {
                        return { results: data };
                    }
                },
                formatResult: function (item) {
                    return item.text;
                },
                formatSelection: function (item) {
                    return item.text;
                }
            }
    });
});
 <!--init script end-->
</script>
