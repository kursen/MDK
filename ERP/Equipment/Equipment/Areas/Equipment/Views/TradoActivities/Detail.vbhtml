@ModelType Equipment.TradoActivity
@Code
    ViewData("Title") = "Detail Kegiatan Trado"
    
    Dim tableClass = "table table-bordered"
    
End Code
@helper WriteTime(ByVal t As TimeSpan?, Optional ByVal blILastOverTime As Boolean = False)

    If t.HasValue Then
        If blILastOverTime Then
            If t.Value.Ticks = 0 Then
    @Html.Raw("24:00")
                Return
            End If
        End If
        Dim d As Date = New Date(t.Value.Ticks)
    @Html.Raw(d.ToString("HH:mm"))
    Else
    @Html.Raw("--:--")
    End If

End Helper
@helper WriteDeltaTime(ByVal t1 As TimeSpan?, ByVal t2 As TimeSpan?, t3 As TimeSpan?, ByVal t4 As TimeSpan?)

    Dim totalTime1 As TimeSpan = Nothing
    Dim totalTime2 As TimeSpan = Nothing
    If t1.HasValue AndAlso t2.HasValue Then
        totalTime1 = t2.Value - t1.Value
    End If
    If t3.HasValue AndAlso t4.HasValue Then
        If t4.Value.Ticks < t3.Value.Ticks AndAlso t4.Value.Ticks = 0 Then
            t4 = New TimeSpan(24, 0, 0)
        End If
        totalTime2 += t4 - t3
    End If
    Dim h = totalTime1 + totalTime2
    
    @Html.Raw(h.TotalHours().ToString("N2"))
End Helper
<div class="row">
    <div class="col-lg-12 col-sm-12">
        <div class="pull-right">
            <div class="button-group">
                <a   href="@Url.Action("Edit", "TradoActivities", New With {.id = Model.ID})"class="btn btn-danger btn-label-left" >
                    <span><i class="fa fa-edit"></i></span>Edit</a> <a href="@Url.Action("Index", "TradoActivities")" class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-arrow-left"></i></span>Kembali</a> <a href="@Url.Action("ReportTradoActivities", "Report", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                            <span><i class="fa fa-print"></i></span>Print</a>
            </div>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Detail Kegiatan Trado")
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <table class="@tableClass">
                <thead>
                    <tr>
                        <th>
                            HARI
                        </th>
                        <th>
                            TANGGAL
                        </th>
                        <th>
                            TIPE
                        </th>
                        <th>
                            MEREK 
                        </th>
                        <th>
                            NO. TRADO
                        </th>
                        <th>
                            NAMA SUPIR
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="text-center">
                        <td>@Model.Date.ToString("dddd")
                        </td>
                        <td>@Model.Date.ToString("dd-MM-yyyy")
                        </td>
                        <td>@Model.Category
                        </td>
                        <td>
                            @Model.Merk / @Model.Type
                        </td>
                        <td>@Model.Code
                        </td>
                        <td>@Model.Driver
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Operasional</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table  class="@tableClass" >
                        <colgroup>
                            @code
                                Dim colWidth = {"60px", "auto", "120px", "80px", "80px", "100px", "100px"}
    For Each item In colWidth
                                @<col style="width:@item" />
    Next
                            End Code
                        </colgroup>
                        <thead>
                            <tr>
                                <th rowspan='2'>
                                    Rit Ke
                                </th>
                                <th rowspan='2'>
                                    Jenis Muatan / Alat Berat
                                </th>
                                <th rowspan='2'>
                                Nama Operator
                                </th>
                                 <th colspan='2'>
                                Waktu
                               </th>
                                <th rowspan='2'>
                                    Lokasi Asal
                                </th>
                                <th rowspan='2'>
                                    Lokasi Tujuan
                                </th>
                                
                                <th rowspan='2'>
                                    KM Awal
                                </th>
                                <th rowspan='2'>
                                    KM Akhir
                                </th>
                                <th rowspan='2'>
                                    Jarak <br /> (KM)
                                </th>
                               
                            </tr>
                            <tr><th>Berangkat</th>
                            <th>Tiba</th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    Dim counter As Integer = 1
                            End Code
                            @For Each item In Model.TradoOperations
                                @<tr>
                                    <td>@counter
                                    </td>
                                    <td>@item.LoadType
                                    </td>
                                    <td>@item.Operator
                                    </td>
                                     <td class="text-center">@WriteTime(item.DepartureTime)
                                    </td>
                                    <td class="text-center">@WriteTime(item.ArrivalTime, True)
                                    </td>
                                    <td>@item.SourceLocation
                                    </td>
                                   
                                    <td>@item.Destination
                                    </td>
                                    <td class="text-right">@item.BeginKM
                                    </td>
                                    <td class="text-right">@item.EndKM
                                    </td>
                                     <td class="text-right">@item.Distance
                                    </td>
                                </tr>
    Next
                            @code 
                                If Model.TradoOperations.Count = 0 Then
                                @<tr>
                                    <td colspan="6">
                                        [Tidak ada data]
                                    </td>
                                </tr>
    End If
                            End Code
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
       
    </div>
    @<div class="row">
     <div class="col-lg-6 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    BBM</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                            <th>
                            No.
                            </th>
                            <th>
                                Jam Pengisian
                            </th>
                                <th>
                                    Liter
                                </th>
                                <th>
                                Nama / Lokasi SPBU
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                         @code
                             Dim number As Integer = 1
                            End Code
                            @For Each item In Model.FuelUsedTradoes
                                @<tr>
                                <td>
                                    @number
                                </td>
                                <td>
                                    @WriteTime(item.TimeFill)
                                </td>
                                    <td class="text-right">
                                        @item.AmountFuel
                                    </td>
                                    <td>
                                        @item.Location
                                    </td>
                                </tr>
                                number += 1
    Next
@code
        number = 1
End Code

                            @If Model.FuelUsedTradoes.Count = 0 Then
                                @<tr>
                                    <td>
                                        -
                                    </td>
                                </tr>
                                
    End If
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <div class="col-lg-6 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    BBM Diluar Operasi</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                            <th>
                            No.
                            </th>
                            <th>
                                Alokasi
                            </th>
                                <th>
                                    Liter
                                </th>
                                <th>
                                Nama Operator
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each item In Model.FuelUsedOutTradoes
                                @<tr>
                                <td>
                                    @number
                                </td>
                                <td>
                                    @item.Alocation
                                </td>
                                    <td class="text-right">
                                        @item.AmountFuelOut
                                    </td>
                                    <td>
                                        @item.Operator
                                    </td>
                                </tr>
                                number += 1
    Next

                            @If Model.FuelUsedTradoes.Count = 0 Then
                                @<tr>
                                    <td>
                                        -
                                    </td>
                                </tr>
    End If
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

    </div>
    @<div class="row">
        <div class="col-lg-6 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Non Operasional
                </div>
                <div class="panel-body">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th>
                                    Jenis Pekerjaan
                                </th>
                               
                                <th>
                                    Alasan Non Operasi
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    counter = 1
                            End Code
                            @For Each item In Model.TradoNonOperations
                                @<tr>
                                    <td>@counter.
                                    </td>
                                    <td>@item.NonOperationType
                                    </td>
                                    <td>@item.Reason
                                    </td>
                                </tr>
        counter += 1
    Next
                            @If Model.TradoNonOperations.Count = 0 Then
                                @<tr>
                                    <td colspan="5">
                                        [Tidak ada data]
                                    </td>
                                </tr>
    End If
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-lg-6 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Pemakaian Pelumas & Spare Part</div>
                <div class="panel-body">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th>
                                    Jenis Oli / Spare Part
                                </th>
                                <th>
                                    Jumlah
                                </th>
                                <th>
                                    Satuan
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    counter = 1
                                For Each item In Model.OilAndSparePartUsedTradoes
                                @<tr>
                                    <td>@counter.
                                    </td>
                                    <td>@item.OilOrSparePartType
                                    </td>
                                    <td class="text-right">@item.Amount
                                    </td>
                                    <td>@item.Unit
                                    </td>
                                </tr>
        counter += 1
    Next
                            If Model.OilAndSparePartUsedTradoes.Count = 0 Then
                                @<tr>
                                    <td colspan="3">
                                        [Tidak ada data]
                                    </td>
                                </tr>
    End If
                
                            End Code
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

End Using

   
