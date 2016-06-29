@ModelType HRD.Master_Personal
@Code
    ViewData("Title") = "Detail"
End Code
@helper  WriteDetailItem(ByVal caption As String, value As String)
    @Html.WriteFormInput(New MvcHtmlString("<p class='form-control-static'>:&nbsp;" & value & "</p>"), caption, 
                          smControlWidth:=8, lgControlWidth:=7, lgLabelWidth:=5,smLabelWidth:=6)
End Helper
<style>
    .form-horizontal .form-control-static
    {
        padding-top: 4px;
    }
    #divBiodata .form-group
    {
        margin-top: 3px;
        margin-bottom: 3px;
    }
</style>
@Using Html.BeginJUIBox("Detail Karyawan")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="btn-group pull-right ">
                <a href='@Url.Action("Employee/" & Model.ID, "Report")' class="btn btn-danger">
                    Print</a>
                @Html.ActionLink("Edit", "EditEmployee", New With {.id = Model.ID}, New With {.class = "btn btn-primary ajax-link"})
                    @Html.ActionLink("Ubah Foto", "EditPhoto", New With {.id = Model.ID}, New With {.class = "btn btn-primary ajax-link"})
                @Html.ActionLink("Kembali", "Index", New With {.id = Model.OfficeId}, New With {.class = "btn btn-primary ajax-link"})
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-3 col-sm-5">
            <div class="well">
                <img alt="employeeimage" style="width: 100%" src="/Employee/EmployeePhoto/@Model.ID" />
            </div>
        </div>
        <div class="col-lg-8 col-sm-6">
            <div class="form-horizontal" id="divBiodata">
                <h3>
                    Biodata</h3>
                @WriteDetailItem("ID Karyawan", Model.EmployeeID)
                @WriteDetailItem("Nama Lengkap", Model.Title_Front & " " & Model.FullName & "," & Model.Title_Back)
                @WriteDetailItem("Nama Alias", Model.Alias)
                @WriteDetailItem("NIK",Model.NIK)
                @WriteDetailItem("Jenis Kelamin",Model.Gender)
                @WriteDetailItem("Agama ",Model.Religion)
                @WriteDetailItem("Tanggal Lahir", If(Model.DateOfBirth.HasValue, Model.DateOfBirth.Value.ToString("dd-MM-yyyy"), "??-??-????"))
                @WriteDetailItem("Tempat Lahir", Model.PlaceOfBirth)
                @WriteDetailItem("Status Perkawinan",Model.MaritalStatus)
                @WriteDetailItem("Golongan Darah",Model.BloodType)
                @WriteDetailItem("Alamat ", Model.HomeAddress_Street)
                @WriteDetailItem("Kota ", Model.HomeAddress_City)
                @WriteDetailItem("Kode Pos",Model.HomeAddress_PostCode)

                @WriteDetailItem("No. Telfon", Model.HomePhone)
                @WriteDetailItem("No. HP", Model.MobilePhone)
                @WriteDetailItem("No. Tel Darurat", Model.EmergencyCallNumber)
                @WriteDetailItem("Email ",Model.Email)
                
                

                <h3>
                    Jabatan</h3>
                @WriteDetailItem("Jabatan", Model.Occupation.Name)
                @WriteDetailItem("Kantor", Model.Office.Name)
                @WriteDetailItem("Status Pekerjaan",Model.WorkerStatus)
                @WriteDetailItem("Mulai Bekerja", If(Not IsNothing(Model.WorkStartDate), Model.WorkStartDate, "??-??-????"))

                @WriteDetailItem("Pembaharuan Data Terakhir",If(Not IsNothing(Model.LastUpdate),Model.LastUpdate,""))
            </div>
        </div>
    </div>
    
    
End Using
