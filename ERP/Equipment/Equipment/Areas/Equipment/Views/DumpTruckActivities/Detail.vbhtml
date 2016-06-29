@ModelType Equipment.DumpTruckActivity
@Code
    ViewData("Title") = "Detail Kegiatan Dump Truck"
    
    Dim tableClass = "table table-bordered"
    Dim Roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(Roles, User)
    
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
            @If isProperUser Then
                @<a href="@Url.Action("Edit", "DumpTruckActivities", New With {.id = Model.ID})"class="btn btn-danger btn-label-left" >
                    <span><i class="fa fa-edit"></i></span>Edit</a> 
            End If
                
                    <a href="@Url.Action("Index", "DumpTruckActivities")" class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-arrow-left"></i></span>Kembali</a> 
                        
                        <a href="@Url.Action("ReportDTActivities", "Report", New With {.id = Model.ID})" class="btn btn-danger btn-label-left">
                            <span><i class="fa fa-print"></i></span>Print</a>
            </div>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Detail Kegiatan Alat Berat")
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
                            TIPE ALAT BERAT
                        </th>
                        <th>
                            MEREK ALAT BERAT
                        </th>
                        <th>
                            No. ALAT
                        </th>
                        <th>
                            NAMA OPERATOR
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
                        <td>@Model.Operator
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
                    JAM KERJA
                </div>
                <div class="panel-body">
                    <table  class="@tableClass">
                        <colgroup>
                            @code
    Dim w = 100 / 14
                            End Code
                            @For i As Integer = 0 To 13
                                @<col style='width:@(w)%' />
    Next
                        </colgroup>
                        <thead>
                            <tr>
                                <th colspan="2">
                                    Lembur 2
                                </th>
                                <th colspan="2">
                                    Pagi s/d Siang
                                </th>
                                <th colspan="2">
                                    Istirahat
                                </th>
                                <th colspan="2">
                                    Siang s/d Sore
                                </th>
                                <th colspan="2">
                                    Istirahat
                                </th>
                                <th colspan="4">
                                    Lembur (1)
                                </th>
                            </tr>
                            <tr>
                                <th>@WriteTime(Model.BeginSecondOverTime)
                                </th>
                                <th>@WriteTime(Model.EndSecondOverTime)
                                </th>
                                <th>@WriteTime(Model.BeginNormalI)
                                </th>
                                <th>@WriteTime(Model.EndNormalI)
                                </th>
                                <th>
                                    @WriteTime(Model.BeginBreakI)
                                </th>
                                <th>@WriteTime(Model.EndBreakI)
                                </th>
                                <th>@WriteTime(Model.BeginNormalII)
                                </th>
                                <th>@WriteTime(Model.EndNormalII)
                                </th>
                                <th>
                                    @WriteTime(Model.BeginBreakII)
                                </th>
                                <th>
                                    @WriteTime(Model.EndBreakII)
                                <th>
                                    @WriteTime(Model.BeginFirstOverTime1)
                                </th>
                                <th>
                                    @WriteTime(Model.EndFirstOverTime1)
                                <th>
                                    @WriteTime(Model.BeginFirstOverTime2)
                                </th>
                                <th>
                                    @WriteTime(Model.EndFirstOvertime2, True)
                            </tr>
                            <tr>
                                <td colspan="14">
                                    &nbsp;
                                </td>
                            </tr>
                        </thead>
                    </table>
                    <table  class="@tableClass">
                        <colgroup>
                            <col style="width: auto" />
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col style="width: 50px" />
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col style="width: 50px" />
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col style="width: 50px" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td class="text-bold">
                                    Jumlah Jam kerja
                                </td>
                                <td class="text-bold">
                                    Normal
                                </td>
                                <td class="text-center">
                                    @WriteDeltaTime(Model.BeginNormalI, Model.EndNormalI, Model.BeginNormalII, Model.EndNormalII)
                                </td>
                                <td class="text-bold">
                                    Jam
                                </td>
                                <td class="text-bold">
                                    Lembur (1)
                                </td>
                                <td class="text-center">
                                    @WriteDeltaTime(Model.BeginFirstOverTime1, Model.EndFirstOverTime1, Model.BeginFirstOverTime2, Model.EndFirstOvertime2)
                                </td>
                                <td class="text-bold">
                                    Jam
                                </td>
                                <td class="text-bold">
                                    Lembur (2)
                                </td>
                                <td class="text-center">
                                    @WriteDeltaTime(Model.BeginSecondOverTime, Model.EndSecondOverTime, Nothing, Nothing)
                                </td>
                                <td class="text-bold">
                                    Jam
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-10 col-sm-10">
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
                                    No.
                                </th>
                                <th rowspan='2'>
                                    Jenis Muatan
                                </th>
                                <th rowspan='2'>
                                    Lokasi Asal
                                </th>
                                <th colspan='2'>
                                    Waktu
                                </th>
                                <th rowspan='2'>
                                    Tujuan
                                </th>
                                <th rowspan='2'>
                                    Jarak <br /> (KM)
                                </th>
                                <th rowspan='2'>
                                    Nama Penerima
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
                            @For Each item In Model.DumpTruckOperations
                                @<tr>
                                    <td>@counter
                                    </td>
                                    <td>@item.LoadType
                                    </td>
                                    <td>@item.SourceLocation
                                    </td>
                                    <td class="text-center">@WriteTime(item.DepartureTime)
                                    </td>
                                    <td class="text-center">@WriteTime(item.ArrivalTime, True)
                                    </td>
                                    <td>@item.Destination
                                    </td>
                                    <td class="text-right">@item.Distance
                                    </td>
                                    <td>@item.ReceiverName
                                    </td>
                                </tr>
    Next
                            @code 
    If Model.DumpTruckOperations.Count = 0 Then
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
        <div class="col-lg-2 col-sm-2">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    BBM</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                                <th>
                                    Liter
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each item In Model.FuelUsedDumpTrucks
                                @<tr>
                                    <td>
                                        @item.AmountFuel
                                    </td>
                                </tr>
    Next
                            @If Model.FuelUsedDumpTrucks.Count = 0 Then
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
        <div class="col-lg-9 col-sm-8">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Non Operasional
                </div>
                <div class="panel-body">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                                <th rowspan="2">
                                    No.
                                </th>
                                <th rowspan="2">
                                    Jenis Pekerjaan
                                </th>
                                <th colspan="2">
                                    Waktu
                                </th>
                                <th rowspan="2">
                                    Alasan Non Operasi
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    Awal
                                </th>
                                <th>
                                    Akhir
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    counter = 1
                            End Code
                            @For Each item In Model.DumpTruckNonOperations
                                @<tr>
                                    <td>@counter.
                                    </td>
                                    <td>@item.NonOperationType
                                    </td>
                                    <td>@item.Begin
                                    </td>
                                    <td>@item.End
                                    </td>
                                    <td>@item.Reason
                                    </td>
                                </tr>
        counter += 1
    Next
                            @If Model.DumpTruckNonOperations.Count = 0 Then
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
        <div class="col-lg-3 col-sm-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Pemakaian Pelumas</div>
                <div class="panel-body">
                    <table class="@tableClass">
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th>
                                    Uraian
                                </th>
                                <th>
                                    Liter
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    counter = 1
    For Each item In Model.OilUsedDumpTrucks
                                @<tr>
                                    <td>@counter.
                                    </td>
                                    <td>@item.OilType
                                    </td>
                                    <td>@item.Amount
                                    </td>
                                </tr>
        counter += 1
    Next
    If Model.OilUsedDumpTrucks.Count = 0 Then
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
<link href="../../../../plugins/bootstrap-typeahead/typeahead.css" rel="stylesheet"
    type="text/css" />
<script src="../../../../plugins/bootstrap-typeahead/bloodhound.min.js" type="text/javascript"></script>
<script src="../../../../plugins/bootstrap-typeahead/typeahead.jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">

   
</script>
