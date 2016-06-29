@ModelType  Purchasing.PriceComparison
@Code
    ViewData("Title") = "Detail Perbandingan Harga"
    Dim NoUrut As Integer = 0
    Dim companyList = CType(ViewData("Companylist"), List(Of Purchasing.Vendor))
    Dim vendor1 = companyList.Where(Function(m) m.Id = Model.VendorID1).SingleOrDefault
    Dim vendor2 = companyList.Where(Function(m) m.Id = Model.VendorID2).SingleOrDefault
    Dim vendor3 = companyList.Where(Function(m) m.Id = Model.VendorID3).SingleOrDefault
End Code
@Using Html.BeginJUIBox("Detail Perbandingan Harga")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
             <a href="@Url.Action("Index", "PriceComparison")" class="btn btn-danger btn-label-left">
                    <span><i class="fa fa-arrow-left"></i></span>Kembali</a> 
                <a type="button" class="btn btn-danger btn-label-left" href="/Purchasing/PriceComparison/Edit/@Model.ID">
                    <span><i class="fa fa-edit"></i></span>Edit</a>
                <button type="button" class="btn btn-danger btn-label-left" id="btnDelete">
                    <span><i class="fa fa-trash"></i></span>Hapus</button>
            </div>
        </div>
    </div>
    @<div class="row">
        <dl class="dl-horizontal">
            <dt>Tanggal</dt>
            <dd>@Model.CreateDate.ToString("dd MMMM yyyy")</dd>
            <dt>No Permintaan</dt>
            <dd>@Model.DepartmentPurchaseRequisition.RecordNo</dd>
        </dl>
        <div class="panel panel-primary">
            <div class="panel-heading">
                Perbandingan Harga
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
                            @If Not vendor1 Is Nothing Then
                                @vendor1.Name
    End If
                        </th>
                        <th colspan="2">
                            @If Not vendor2 Is Nothing Then
                                @vendor2.Name     
    End If
                        </th>
                        <th colspan="2">
                            @If Not vendor3 Is Nothing Then
                                @vendor3.Name     
    End If
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Harga
                        </th>
                        <th>
                            Total
                        </th>
                        <th>
                            Harga
                        </th>
                        <th>
                            Total
                        </th>
                        <th>
                            Harga
                        </th>
                        <th>
                            Total
                        </th>
                    </tr>
                </thead>
                <tbody>
                    @code
    Dim gTotal1, gTotal2, gTotal3 As Decimal
                    
                
    For Each item In Model.PriceComparisonDetail
        NoUrut += 1
        Dim total1, total2, total3 As Double
                            
        total1 = item.Price1 * item.DepartmentPRDetail.Quantity
        total2 = item.Price2 * item.DepartmentPRDetail.Quantity
        total3 = item.Price3 * item.DepartmentPRDetail.Quantity
        gTotal1 += total1
        gTotal2 += total2
        gTotal3 += total3
                        @<tr>
                            <td class="text-right">
                                @NoUrut.
                            </td>
                            <td>
                                @item.DepartmentPRDetail.ItemName
                            </td>
                            <td class="text-right">
                                @item.DepartmentPRDetail.Quantity
                            </td>
                            <td class="text-left">
                                @item.DepartmentPRDetail.UnitQuantity
                            </td>
                            <td class="text-right">
                                @item.Price1.ToString("#,###.0#")
                            </td>
                            <td class="text-right">
                                @total1.ToString("#,###,0#")
                            </td>
                            <td class="text-right">
                                @item.Price2.ToString("#,###.0#")
                            </td>
                            <td class="text-right">@total2.ToString("#,###,0#")
                            </td>
                            <td class="text-right">
                                @item.Price3.ToString("#,###.0#")
                            </td>
                            <td class="text-right">
                                @total3.ToString("#,###,0#")
                            </td>
                        </tr>
    Next
                    End Code
                </tbody>
                <tfoot class="ftotal">
                    <tr>
                        <td colspan='4' class="text-right">
                            Total
                        </td>
                        <td>
                        </td>
                        <td class='text-right'>
                            @gTotal1.ToString("#,###.0#")
                        </td>
                        <td>
                        </td>
                        <td class='text-right'>
                            @gTotal2.ToString("#,###.0#")
                        </td>
                        <td>
                        </td>
                        <td class='text-right'>
                            @gTotal3.ToString("#,###.0#")
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    
                 
End Using
<style>
    .ftotal tr
    {
        border: 1px solid #c0c0c0;
    }
</style>
