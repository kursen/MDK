@ModelType ProjectManagement.ProjectInfo
@Code
    ViewData("Title") = "Project Info"
    
    
End Code
@Using Html.BeginJUIBox("Menambah data proyek", isCollapsible:=False,iconClass:="fa fa-user",isRemovable:=True)
    Using Html.BeginForm("SaveProjectInfo", "ProjectInfo", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmProjectInfo"})
        

    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ProjectCode, New With {.class = "form-control"}), "Kode Proyek", smControlWidth:=4, lgControlWidth:=2)
    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ProjectTitle, New With {.class = "form-control"}), "Nama Proyek", smControlWidth:=7, lgControlWidth:=7)
        
    @Html.WriteFormDateInputFor(Function(m) m.DateStart, "Tanggal Mulai",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    smControlWidth:=4, lgControlWidth:=2)
    
    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Lama Pekerjaan</label>
        <div class="col-lg-2 col-sm-4">
        <div class="input-group">
            @Html.IntegerInputFor(Function(m) m.NumberOfDays)
            <span class="input-group-addon">Hari Kerja</span>
        </div>
        
       
        
            @*note: for now just use "hari kalender"*@ @*<select name="DayUnit" id="DayUnit" class="form-control">
            <option value="Hari Kalender">Hari Kalender</option>
            <option value="Hari Kerja">Hari Kerja</option>
        </select>*@ 
            @Html.HiddenFor(Function(m) m.DayUnit)
        </div>
    </div>
    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Location, New With {.class = "form-control"}), "Lokasi")
    
    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ContractNumber, New With {.class = "form-control"}), "No. Kontrak/SPK")

 
    
    
    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Nilai Kontrak</label>
        <div class="col-lg-3 col-sm-4">
            <div class="input-group">
                <span class="input-group-addon">Rp.</span>
                @Html.DecimalInput("ContractValueMask", Model.ContractValue)
                @Html.HiddenFor(Function(m) m.ContractValue)
            </div>
        </div>
    </div>
    @Html.WriteFormInput(Html.DropDownList("CompanyInfoId",Nothing,Nothing,New With {.class="form-control"}),"Pelaksana Pekerjaan")
    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.ConsultanName, New With {.class = "form-control"}), "Nama Perusahaan Konsultan")
    @<div class="row">
        <div class="col-sm-offset-4 col-sm-2">
            <button type="button" id="btnSave" class="btn btn-primary ">
                <span><i class="fa fa-arrow-circle-o-right"></i></span> Submit
            </button>
            <button type="reset" class="btn">
                Reset</button>
        </div>
    </div>
    
    
    End Using
    
End Using
<script type="text/javascript">
    var btnSubmit = null;
    $(function () {

        $("#btnSave").click(function () {
            btnSubmit = $(this).button("menyimpan data...");
            showSavingNotification();
            
            var fmt = $.number($("#ContractValueMask").val(), 2, ",", "."); ;
            $("#ContractValue").val(fmt);

            var _data = $("#frmProjectInfo").serialize();
            var _url = $("#frmProjectInfo").attr("action");
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

    submitFormCallback = function (data) {
        if (data.stat == 1) {
            window.location = "/ProjectManagement/ProjectInfo/Detail/"+data.projectId;
            return;
        }
        btnSubmit.button("Submit");

        showNotificationSaveError(data);
    }
    
</script>
