@Code
    ViewData("Title") = "Perawatan Kendaraan"
    
    Dim defaultDateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
  
End Code
@Functions
    Function WriteSpanControl(ByVal value As String, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<span class='form-control-static'>" & value & "</span>")
    End Function
    
    Function WriteKmControl(ByVal value As Double, ByVal id As String) As MvcHtmlString
        Dim sb As New StringBuilder
        sb.AppendLine(" <div class='input-group'>")
        sb.AppendLine("<div class='input-group-addon'>Rata-rata</div>")
        sb.AppendFormat("<input type='text' class='form-control text-right' id='{0}' name='{0}' value={1} />", id, value)
        sb.AppendLine("<div class='input-group-addon'>KM</div>")
        sb.AppendLine("</div>")
    
    
        Dim inputKm = New MvcHtmlString(sb.ToString())
        Return inputKm
    
    End Function
    
End Functions
@Using Html.BeginJUIBox("Form Perawatan Kendaraan")
    
    @<div class="row">
        @Using Html.BeginForm("SaveMaintenancePlan", "PeriodicMaintenance", Nothing, FormMethod.Post, New With {.class = "form-horizontal",
                                                                                                               .autocomplete = "off"})
    
           
            @Html.WriteFormInput(Html.TextBox("Code", Nothing, New With {.class = "form-control"}), "Kode Kendaraan",lgControlWidth:=2)
            @Html.WriteFormInput(WriteSpanControl("DUMP TRUCK", "spCategory"), "Jenis")
            @Html.WriteFormInput(WriteSpanControl("BA 9999 QQ", "spPoliceNumber"), "No. Polisi",lgControlWidth:=2)
            @Html.WriteFormInput(WriteSpanControl("MITSUBISHI/L300", "spMerk"), "Merk/Type")
            @Html.WriteFormInput(WriteKmControl(0, "AverageKm"), "Pemakaian Harian")

            @<div class="col-lg-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Item Pekerjaan
                        <button class="btn btn-danger pull-right" type="button">
                            +</button>
                    </div>
                    <table class="table table-bordered">
                        <colgroup>
                            <col style="width: 60px">
                            <col style="width: auto">
                            <col style="width: 100px">
                        </colgroup>
                        <thead class="bg-default">
                            <tr>
                                <th rowspan="2">
                                    #
                                </th>
                                <th rowspan="2">
                                    Item
                                </th>
                                <th colspan="2" rowspan="2">
                                    Setiap
                                </th>
                                <th colspan="2">
                                    Perawatan Terakhir
                                </th>
                                <th colspan="2" rowspan="2"></th>
                            </tr>
                            <tr>
                                <th>Tanggal</th>
                                <th>Kilometer</th>

                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="text-right">
                                    1.
                                </td>
                                <td>
                                    Ganti Oli
                                </td>
                                <td class="text-right">
                                    1000
                                </td>
                                <td>
                                    KM
                                </td>
                                <td class="text-center">
                                    31-12-2015
                                </td>
                                <td>545034</td>
                                <td class="text-center">
                                    <a href="#">Ubah</a>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right">
                                    2.
                                </td>
                                <td>
                                    Perawatan Radiator (cuci radioator, ganti air, dll)
                                </td>
                                <td class="text-right">
                                    5
                                </td>
                                <td>
                                    Minggu
                                </td>
                                <td class="text-center">
                                    31-12-2015
                                </td>
                                <td>545034</td>
                                <td class="text-center">
                                    <a href="#">Ubah</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            
            @<div class="col-lg-12 col-sm-12">
                <div class="center-block" style="width: 200px">
                    <button type="button" class="btn btn-primary">
                        Simpan</button>
                </div>
            </div>
            
    End Using
    </div>
    
End Using
