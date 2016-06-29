@ModelType HRD.Master_Personal
@Code
    ViewData("Title") = "EmployeeForm"
    Dim defaultClass = New With {.class = "form-control"}
    Dim datedefaultOption = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
End Code
@Using Html.BeginJUIBox("Edit Data Karyawan")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="btn-group pull-right ">
            @If Model.ID = 0 Then
                    @Html.ActionLink("Kembali", "Index", Nothing, New With {.class = "btn btn-primary ajax-link"})
                Else
                @Html.ActionLink("Kembali", "Detail", New With {.id = Model.ID}, New With {.class = "btn btn-primary ajax-link"})    
                End If
                
            </div>
        </div>
    </div>
    
    @<div class="row">
        @Using Html.BeginForm("SaveEmployee", "Employee", Nothing, FormMethod.Post, New With {.id = "frmEmployee", .class = "form-horizontal"})
            @Html.HiddenFor(Function(m) m.ID)
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.EmployeeID, defaultClass), "Nomor ID Karyawan")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.FullName, defaultClass), "Nama Lengkap")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Alias, defaultClass), "Alias")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Title_Front, defaultClass), "Gelar depan")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Title_Back, defaultClass), "Gelar belakang")

            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.NIK, defaultClass), "No. Induk Kependudukan")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.PlaceOfBirth, defaultClass), "Tempat Lahir")
            @Html.WriteFormInput(Html.DateInputFor(Function(m) m.DateOfBirth, datedefaultOption), "Tanggal Lahir", smControlWidth:=4, lgControlWidth:=2)
            
            @Html.WriteFormInput(Html.DropDownList("Religion", Nothing, defaultClass), "Agama")
@Html.WriteFormInput(Html.DropDownList("Gender", Nothing, defaultClass), "Gender")
            
            @Html.WriteFormInput(Html.DropDownList("MaritalStatus", Nothing, defaultClass), "Status Perkawinan")
            @<hr />
            
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.HomeAddress_Street, defaultClass), "Alamat Rumah")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.HomeAddress_City, defaultClass), "Kota/Kabupaten")
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.HomeAddress_PostCode, defaultClass), "Kode Post", smControlWidth:=4, lgControlWidth:=2)
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.HomePhone, defaultClass), "No. Telfon Rumah") 
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.MobilePhone, defaultClass), "No. Telfon Seluler") 
            @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.EmergencyCallNumber, defaultClass), "No. Telfon Darurat") 
            @<hr />
            
            
            
            @Html.WriteFormInput(Html.DateInputFor(Function(m) m.WorkStartDate, datedefaultOption), "Tanggal Mulai Bekerja", smControlWidth:=4, lgControlWidth:=2)
            @Html.WriteFormInput(Html.DropDownList("OccupationId", Nothing, defaultClass), "Jabatan")
            @Html.WriteFormInput(Html.DropDownList("OfficeId", Nothing, New With {.class = "form-control"}), "Kantor / Unit Kerja")

            @<hr />
            @Html.WriteFormInput(Html.DropDownList("WorkerStatus", Nothing, defaultClass), "Status Karyawan")
            @<div class="row">
                <div class="col-lg-12 col-sm-12">
                    <div class="well">
                        <div class="col-lg-offset-4 col-sm-offset-2">
                            <button class="btn btn-primary" type="button" id="btnSave">
                                Simpan</button>
                        </div>
                    </div>
                </div>
            </div>
    End Using
    </div>
End Using
<script type="text/javascript">
    var btnSubmit = null;
    var arroffice = [];
    submitFormCallback = function (data) {
        if (data.stat == 1) {

            window.location = "/HRD/Employee/Detail/" + data.ID;
            return;
        }
        btnSubmit.button("Simpan");

        showNotificationSaveError(data);
    }

    $(function () {

        //load array office;
        $.ajax({
            type: 'POST',
            url: "/HRD/Employee/GetOfficeList",

            success: function (data) {
                arroffice = data;
            },
            error: ajax_error_callback,
            dataType: 'json'
        });

        $("#OfficeId").click(function () {
            //fill the items
            $("#OfficeId").empty();
            $(arroffice).each(function (idx, a) {
                var _sparator = "&nbsp;";
                for (var i = 0; i < a.lvl; i++)
                    _sparator += _sparator;

                $("#OfficeId").append("<option value='" + a.Id + "' data-path='" + escape(a.pathname) + "'>" + _sparator + a.Name + "</option>");
            });

        });
        $("#OfficeId").change(function () {
            var selected = $(this).val();
            var text = $("#OfficeId option:selected").data("path");
            $(this).empty();
            $("#OfficeId").append("<option value='" + selected + "'>" + unescape(text) + "</option>");
        });

        $("#btnSave").click(function () {
            btnSubmit = $(this).button("menyimpan data...");
            showSavingNotification();


            var _data = $("#frmEmployee").serialize();
            var _url = $("#frmEmployee").attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: submitFormCallback,
                error: ajax_error_callback,
                dataType: 'json'
            });


        });


    });
</script>
