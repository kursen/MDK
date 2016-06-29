@modelType HRD.EmployeeOvertime
@Code
    ViewData("Title") = "Form Isian Lembur"
    Dim datedefaultOption = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                             .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
    Dim EmployeeName = ViewData("FullName").ToString()
    Dim OccupationName = ViewData("OccupationName").ToString()
End Code
@Functions
    Function WriteTimePicker(ByVal BeginId As String, BeginValue As DateTime?,
                             ByVal EndId As String, EndValue As DateTime?) As MvcHtmlString
        Dim strBeginValue = ""
        Dim strEndValue = ""
        If BeginValue.HasValue Then
            strBeginValue = BeginValue.Value.ToString("HH:mm")
            
        End If
        
        If EndValue.HasValue Then
            strEndValue = EndValue.Value.ToString("HH:mm")
        End If
            
        Dim sb As New StringBuilder
        
        sb.AppendLine("<div class='input-group-wrapper' style='display:table;position:relative'>")
        sb.AppendLine("<div class='input-group bootstrap-timepicker timepicker' style='margin-bottom:0px;'>")
        sb.AppendFormat("    <input name={0} id='{0}' type='text' value='{1}' class='form-control text-center'>", BeginId, strBeginValue)
        sb.AppendLine("    <span class='input-group-addon'><i class='fa fa-clock-o'></i></span>")
        sb.AppendLine("</div>")
        sb.AppendLine("    <span class='input-group-addon' style='padding-top:0px'>s/d</span>")
        sb.AppendLine("<div class='input-group bootstrap-timepicker timepicker' style='margin-bottom:0px;'>")
        sb.AppendFormat("    <input name={0} id='{0}' type='text' value='{1}' class='form-control text-center'>", EndId, strEndValue)
        sb.AppendLine("    <span class='input-group-addon'><i class='fa fa-clock-o'></i></span>")
        sb.AppendLine("</div>")
        sb.AppendLine("</div>")
        
        Return New MvcHtmlString(sb.ToString())
    End Function
    
  
End Functions
@Using Html.BeginJUIBox("Pencatatan Lembur")
    
    Using Html.BeginForm("SaveOvertime", "Overtime", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "off", .id = "frmOvertime"})
    @Html.HiddenFor(Function(m) m.Id, New With {.id = "overtimeid"})
    @Html.HiddenFor(Function(m) m.OfficeId)
    @Html.WriteFormDateInputFor(Function(m) m.ActivityDate, "Tanggal", datedefaultOption, smControlWidth:=4, lgControlWidth:=2)

    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Nama Pegawai</label>
        <div class="col-lg-4 col-sm-4">
            <input class="form-control" id="EmployeeName" name="EmployeeName" value="@EmployeeName" type="text">
            @Html.HiddenFor(Function(m) m.EmployeeId)
        </div>
    </div>

    
    @Html.WriteFormInput(Html.TextBox("EmployeeOccupation", OccupationName, New With {.class = "form-control", .readonly = "readonly"}), "Jabatan")

    @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Activity, New With {.class = "form-control"}), "Kegiatan")
    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Lembur 1 Pagi</label>
        <div class="col-lg-5 col-sm-8">
            @WriteTimePicker("BeginOvertime1Morning", Model.BeginOvertime1Morning, "EndOvertime1Morning", Model.EndOvertime1Morning)
            <p class="help-block">
                05:00 s/d 08:00</p>
        </div>
        <div class="col-lg-1 col-sm-2">
            <button class='btn btn-danger' type='button' id='btnClearLembur1Pagi'>
                <span class='fa fa-remove'></span>
            </button>
        </div>
    </div>
    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Lembur 1 Sore</label>
        <div class="col-lg-5 col-sm-8">
            @WriteTimePicker("BeginOvertime1Afternoon", Model.BeginOvertime1Afternoon, "EndOvertime1Afternoon", Model.EndOvertime1Afternoon)
            <p class="help-block">
                17:00 s/d 24:00</p>
        </div>
        <div class="col-lg-1 col-sm-2">
            <button class='btn btn-danger' type='button' id='btnClearLembur1Sore'>
                <span class='fa fa-remove'></span>
            </button>
        </div>
    </div>

    @<div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Lembur 2</label>
        <div class="col-lg-5 col-sm-8">
            @WriteTimePicker("BeginOvertime2", Model.BeginOvertime2, "EndOvertime2", Model.EndOvertime2)
            <p class="help-block">
                00:00 s/d 05:00</p>
        </div>
        <div class="col-lg-1 col-sm-2">
            <button class='btn btn-danger' type='button' id='btnClearLembur2'>
                <span class='fa fa-remove'></span>
            </button>
        </div>
    </div>
    
    @<div class="well">
        <div style="width: 50%" class="center-block">
            <button type='button' id="btnSubmit" class="btn btn-primary">
                Simpan</button>
            <button type='button' id="btnBack" class="btn btn-primary">
                Kembali</button>
        </div>
    </div>
    End Using
    
    
    
    
End Using
<style>
.ui-autocomplete { height: 200px; overflow-y: scroll; overflow-x: hidden;}
</style>
<link href="@Url.Content("/Areas/HRD/js/bootstrap-timepicker/css/bootstrap-timepicker.min.css")" rel="stylesheet"
    type="text/css" />
<script src="@Url.Content("/Areas/HRD/js/bootstrap-timepicker/js/bootstrap-timepicker.js")" type="text/javascript"></script>
<script type="text/javascript">


    var _initTimePicker = function () {
        var timePickerDefOption = {
            minuteStep: 15,
            //maxHours: 9,
            //minHours:5,
            template: 'dropdown',
            showSeconds: false,
            showMeridian: false,
            showInputs: false,
            explicitMode: true,
            defaultTime: false,
            icons: { up: "fa fa-caret-up", down: "fa fa-caret-down" }
        };

        timePickerDefOption.minHours = 5;
        timePickerDefOption.maxHours = 7;
        $('#BeginOvertime1Morning').timepicker(timePickerDefOption);
        timePickerDefOption.minHours = 6;
        timePickerDefOption.maxHours = 8;
        $('#EndOvertime1Morning').timepicker(timePickerDefOption);

        timePickerDefOption.minHours = 17;
        timePickerDefOption.maxHours = 23;
        $('#BeginOvertime1Afternoon').timepicker(timePickerDefOption);
        timePickerDefOption.minHours = 18;
        timePickerDefOption.maxHours = 24;
        $('#EndOvertime1Afternoon').timepicker(timePickerDefOption);

        timePickerDefOption.minHours = 0;
        timePickerDefOption.maxHours = 4;
        $('#BeginOvertime2').timepicker(timePickerDefOption);
        timePickerDefOption.minHours = 1;
        timePickerDefOption.maxHours = 5;
        $('#EndOvertime2').timepicker(timePickerDefOption);
        $("#btnClearLembur1Pagi").click(function () {
            $("#BeginOvertime1Morning").val("");
            $("#EndOvertime1Morning").val("");
        });
        $("#btnClearLembur1Sore").click(function () {
            $("#BeginOvertime1Afternoon").val("");
            $("#EndOvertime1Afternoon").val("");
        });
        $("#btnClearLembur2").click(function () {
            $("#BeginOvertime2").val("");
            $("#EndOvertime2").val("");
        });
    };

    $(function () {
        _initTimePicker();
        $("#btnBack").click(function () {
            window.location = "/HRD/Overtime/Index";
        });

        //init autocomplete
        $('#EmployeeName').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/HRD/GlobalArray/AutoCompleteEmployeeForOffice',
                    data: {
                        term: $('#EmployeeName').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.OccupationName,
                                value: obj.Fullname,
                                id: obj.Id,
                                officeId:obj.OfficeId
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $('#EmployeeId').val(0)
                    $('#EmployeeId').parent().parent().addClass('has-error');
                    $("#EmployeeOccupation").val("");
                    $("#OfficeId").val("0");
                } else {
                    $('#EmployeeId').val(ui.item.id)
                    $('#EmployeeId').parent().parent().removeClass('has-error');
                    $("#EmployeeOccupation").val(ui.item.label);
                    $("#OfficeId").val(ui.item.officeId);
                }
            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            return ($('<li>').append('<a><strong>' + item.value + '</strong>, <i>' +
            item.label + '</i></a>').appendTo(ul));
        };

        ; //end autocomplete


        $("#btnSubmit").click(function (e) {
            e.preventDefault();
            btnSubmit = $(this).button("menyimpan data...");
            showSavingNotification();


            var _data = $("#frmOvertime").serialize();
            var _url = $("#frmOvertime").attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: function (data) {
                    if (data.stat == 1) {
                         $("#btnBack").trigger("click");
                        return;
                    }
                    btnSubmit.button("Simpan");
                    showNotificationSaveError(data);

                },
                error: ajax_error_callback,
                dataType: 'json'
            });
        });

    });                      //init
</script>
