@ModelType  Purchasing.modelviewPriceComparison
@Code
    ViewData("Title") = "Detail Perbandingan Harga"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
    Dim ctx As New Purchasing.PurchasingEntities
    Dim vendorList = (From v In ctx.Vendor Select New With {.ID = v.Id, .Name = v.Name}).ToList
    Dim noUrut As Integer = 0
    Dim modelDetailPR As New List(Of Purchasing.DepartmentPRDetail)
    If Model IsNot Nothing Then
        modelDetailPR = (From dprd In ctx.DepartmentPRDetails Where dprd.DepartmentPurchaseRequisitionId = Model.PurchaseRequisitionID).ToList
    End If
End Code

@Using Html.BeginJUIBox("Detail Perbandingan Harga")
        @<div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    <button type="button" class="btn btn-danger btn-label-left" onclick="$('#editableData .editable') .editable('toggleDisabled')">
                        <span><i class="fa fa-edit"></i></span>Edit</button>
                    <button type="button" class="btn btn-danger btn-label-left" id="btnDelete">
                        <span><i class="fa fa-trash"></i></span>Hapus</button>
                </div>
            </div>
        </div>
        @<div id='editableData'>
            @Html.Hiddenfor(Function(m) m.PurchaseRequisitionID)
            @If (From pc In ctx.PriceComparison Where pc.PurchaseRequisitionID = Model.PurchaseRequisitionID Select pc.havePO).FirstOrDefault = False Then
                @<dl class="dl-horizontal">
                    <dt>Tanggal</dt>
                    <dd>@Html.EditableInputTextBox("CreateDate", Model.CreateDate.ToString("dd-MM-yyyy"), "date", "Tanggal Perbandingan", datapk:=Model.PurchaseRequisitionID, dataurl:="/PriceComparisson/SavePartial")</dd>
                    <dt>No Permintaan</dt>
                    <dd>@Model.NoRecord</dd>
                </dl>
            End If
            <div class="panel panel-primary">
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
                                @Html.EditableInputTextBox("VendorID1", (From v In vendorList Where v.ID = Model.VendorID1 Select v.Name).FirstOrDefault, "select", "Nama Pemasok", datapk:=Model.PCDetail1, sourcelist:="/PriceComparisson/GetVendorList", dataurl:="/PriceComparisson/SavePartial")
                                @Html.HiddenFor(Function(m) m.VendorID1, New With {.class = "vendorId"})
                            </th>
                            <th>
                                @Html.EditableInputTextBox("VendorID2", (From v In vendorList Where v.ID = Model.VendorID2 Select v.Name).FirstOrDefault, "select", "Nama Pemasok", datapk:=Model.PCDetail2, sourcelist:="/PriceComparisson/GetVendorList", dataurl:="/PriceComparisson/SavePartial")
                                @Html.HiddenFor(Function(m) m.VendorID2, New With {.class = "vendorId"})
                            </th>
                            <th>
                                @Html.EditableInputTextBox("VendorID3", (From v In vendorList Where v.ID = Model.VendorID3 Select v.Name).FirstOrDefault, "select", "Nama Pemasok", datapk:=Model.PCDetail3, sourcelist:="/PriceComparisson/GetVendorList", dataurl:="/PriceComparisson/SavePartial")
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
                                        @item.Currency @Html.EditableInputTextBox("Price1_" & noUrut,
                                                                   (From pcd In ctx.PriceComparisonDetail Where
                                                                    pcd.PriceComparisonID = Model.PCDetail1 AndAlso
                                                                    pcd.PRDetailID = item.ID Select pcd.Price).FirstOrDefault,
                                                                "number", "Harga Dasar", datapk:=(From pcd In ctx.PriceComparisonDetail Where
                                                                    pcd.PriceComparisonID = Model.PCDetail1 AndAlso
                                                                    pcd.PRDetailID = item.ID Select pcd.ID).FirstOrDefault,
                                                                dataurl:="/PriceComparisson/SavePartialPrice")
                                    </td>
                                    <td align="right">
                                        @item.Currency @Html.EditableInputTextBox("Price2_" & noUrut,
                                                                   (From pcd In ctx.PriceComparisonDetail Where
                                                                    pcd.PriceComparisonID = Model.PCDetail2 AndAlso
                                                                    pcd.PRDetailID = item.ID Select pcd.Price).FirstOrDefault,
                                                                "number", "Harga Dasar", datapk:=(From pcd In ctx.PriceComparisonDetail Where
                                                                    pcd.PriceComparisonID = Model.PCDetail2 AndAlso
                                                                    pcd.PRDetailID = item.ID Select pcd.ID).FirstOrDefault,
                                                                dataurl:="/PriceComparisson/SavePartialPrice")
                                    </td>
                                    <td align="right">
                                        @item.Currency @Html.EditableInputTextBox("Price3_" & noUrut,
                                                                   (From pcd In ctx.PriceComparisonDetail Where
                                                                    pcd.PriceComparisonID = Model.PCDetail3 AndAlso
                                                                    pcd.PRDetailID = item.ID Select pcd.Price).FirstOrDefault,
                                                                "number", "Harga Dasar", datapk:=(From pcd In ctx.PriceComparisonDetail Where
                                                                    pcd.PriceComparisonID = Model.PCDetail3 AndAlso
                                                                    pcd.PRDetailID = item.ID Select pcd.ID).FirstOrDefault,
                                                                dataurl:="/PriceComparisson/SavePartialPrice")
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
        </div>
@*    @<dl class="dl-horizontal" id='dProjectInfo'>
        <dt>Kode Proyek</dt>
        <dd>@Html.EditableInputTextBox("ProjectCode", Model.ProjectCode, "text", "Kode Proyek", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")</dd>
        <dt>Paket</dt>
        <dd>@Html.EditableInputTextBox("ProjectTitle", Model.ProjectTitle, "text", "Nama Proyek", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")</dd>
        <dt>Kontraktor</dt>
        <dd>@Html.EditableInputTextBox("CompanyInfoId", Model.CompanyInfo.Name, "select", "Nama Perusahaan", datapk:=Model.Id, sourcelist:="/ProjectInfo/GetCompanyList", dataurl:="/ProjectInfo/SavePartial")</dd>
        <dt>Nomor</dt>
        <dd>@Html.EditableInputTextBox("ContractNumber", Model.ContractNumber, "text", "No Kontrak", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")
            &nbsp;</dd>
        <dt>Nilai Proyek</dt>
        <dd>@Html.EditableInputTextBox("ContractValue", Model.ContractValue.ToString(), "text", "Nilai Kontrak", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")&nbsp;</dd>
        <dt>Tanggal Mulai</dt>
        <dd>@Html.EditableInputTextBox("DateStart", Model.DateStart.ToString("dd-MM-yyyy"), "date", "Tanggal Mulai", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")
            &nbsp;s/d @Model.DateStart.AddDays(Model.NumberOfDays - 1).ToString("dd-MM-yyyy")
        </dd>
        <dt>Lama Pekerjaan</dt>
        <dd>@Html.EditableInputTextBox("NumberOfDays", Model.NumberOfDays, "number", "Lama Pekerjaan", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")
            &nbsp;Hari Kalender &nbsp;</dd>
        <dt>Lokasi</dt>
        <dd>@Html.EditableInputTextBox("Location", Model.Location, "text", "Lokasi", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")&nbsp;</dd>
        <dt>Konsultan</dt>
        <dd>@Html.EditableInputTextBox("ConsultanName", Model.ConsultanName, "text", "Nama Konsultan", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")&nbsp;</dd>
    </dl>
End Using
@Using Html.BeginJUIBox("Dokumen")
    @<div id="divList">
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    <a href="javascript:void(0)" type="button" class="btn btn-danger btn-label-left"
                        id="btnUpload"><span><i class="fa fa-upload"></i></span>Upload </a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12">
				<div class="table-responsive">
                <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
                    id="DocList" width="100%">
                    <colgroup>
                        <col style="width: 50px;" />
                        <col />
                        <col />
                        <col style="width: 100px;" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Judul Dokumen
                            </th>
                            <th>
                                Deskripsi Isi
                            </th>
                            <th style="width: 100px !important;">
                            </th>
                        </tr>
                    </thead>
                </table>
				</div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div id='divForm' class="col-lg-12 col-sm-12" style='display: none'>
            @Html.Partial("UploadDoc")
        </div>
    </div>*@
    
End Using

<style type="text/css">
    .popover
    {
        z-index: 40000;
    }
</style>
@Section endScript
    <script type="text/javascript">
        $.fn.editabletypes.date.defaults.datepicker.language = "id";
        $.fn.editabletypes.date.defaults.viewformat = "dd-mm-yyyy";
        $.fn.editabletypes.date.defaults.format = "dd-mm-yyyy";

        var _savepartialresponse = function (data) {
            if ((data) && (data.stat == 1)) {

            } else {
                showNotification(data);
            };

        }
        $(function () {
            $("#btnDelete").click(function () {
                if (confirm("Hapus data perbandingan harga ini?")) {
                    $.ajax({
                        type: "POST",
                        data: { id: $("#PurchaseRequisitionID").val() },
                        url: "/Purchasing/PriceComparisson/Delete",
                        datatype: 'json',
                        success: function (data) {
                            alert('sukses');
                            if (data.stat == 1) {
                                window.location = "/Purchasing/PriceComparisson/Create";
                            }
                        }
                    });
                }
            });
        });
    </script>
    <script src="@Url.Content("~/plugins/jquery/jquery-migrate-1.2.1.min.js")" type="text/javascript"></script>
    <script src="../../../../plugins/datatables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../../../../plugins/datatables/dataTables.overrides_indo.js" type="text/javascript"></script>
    <script type="text/javascript" src="@Url.Content("~/js/CRUDHelpers.js")"></script>
End Section
