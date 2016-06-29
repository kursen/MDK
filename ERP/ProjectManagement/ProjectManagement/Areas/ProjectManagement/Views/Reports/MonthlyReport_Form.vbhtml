@ModelType ProjectManagement.MonthlyReport 
<style type="text/css">
    tr.tbHead {
        background:none !important;
    }
    tr.tbHead th {
        height:30px;
        text-align:left !important;
    }
</style>

<div class="row" id="form_Monthly">
    <div class="col-sm-12">
    @Using Html.BeginForm("SaveMonthlyReport", "Reports", FormMethod.Post, New With {.autocomplete = "Off", .class = "form-horizontal", .id = "frmMonthly"})
        @Html.Hidden("ID")
        @Html.Hidden("ProjectInfoId")
        @Html.Hidden("MonthNumber")
        
        @Html.WriteFormInput(Html.TextBox("MingguKe", Nothing, New With {.class = "form-control", .disabled = "disabled"}), "Bulan Ke-", smControlWidth:=5, lgControlWidth:=3)
        
        @Html.WriteFormInput(Html.TextBox("ImplementingActivities", Nothing, New With {.class = "form-control"}), "Pelaksana Kegiatan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Place", Nothing, New With {.class = "form-control"}), "Tempat Laporan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormDateInputFor(Function(m) m.DateApproval, "Tanggal Laporan",
                                        New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                                  .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfMonthDisabled = {0}},
                                        smControlWidth:=4, lgControlWidth:=2)
        
        @<div class="form-group no-margin">
            <h4 class="col-lg-3 col-sm-4">Diajukan Oleh</h4>
        </div>
        @<hr class='no-margin' />
        @Html.WriteFormInput(Html.TextBox("Approval3Occupation", Nothing, New With {.class = "form-control"}), "Jabatan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval3Company", Nothing, New With {.class = "form-control"}), "Nama Perusahaan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval3Name", Nothing, New With {.class = "form-control"}), "Nama", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval3Identity", Nothing, New With {.class = "form-control"}), "ID", smControlWidth:=5, lgControlWidth:=3)

        @<div class="form-group no-margin">
            <h4 class="col-lg-3 col-sm-4">Diperiksa Oleh</h4>
        </div>
        @<hr class='no-margin' />
        @Html.WriteFormInput(Html.TextBox("Approval2Occupation", Nothing, New With {.class = "form-control"}), "Jabatan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval2Company", Nothing, New With {.class = "form-control"}), "Nama Perusahaan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval2Name", Nothing, New With {.class = "form-control"}), "Nama", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval2Identity", Nothing, New With {.class = "form-control"}), "ID", smControlWidth:=5, lgControlWidth:=3)

        @<div class="form-group no-margin">
            <h4 class="col-lg-3 col-sm-4">Disetujui Oleh</h4>
        </div>
        @<hr class='no-margin' />
        @Html.WriteFormInput(Html.TextBox("Approval1Occupation", Nothing, New With {.class = "form-control"}), "Jabatan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval1Company", Nothing, New With {.class = "form-control"}), "Nama Perusahaan", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval1Name", Nothing, New With {.class = "form-control"}), "Nama", smControlWidth:=5, lgControlWidth:=3)
        @Html.WriteFormInput(Html.TextBox("Approval1Identity", Nothing, New With {.class = "form-control"}), "ID", smControlWidth:=5, lgControlWidth:=3)
        @<br />
        @<div class="col-lg-offset-3 col-sm-offset-3">
            <button type="button" class="btn btn-info" id="btnSavePrint"><i class="fa fa-print"></i> 
                Simpan dan Print</button>
            &nbsp;
            <button type="submit" class="btn btn-primary" id="btnSave"><i class="fa fa-save"></i> 
                Simpan</button>
            <button type="button" class="btn" id="btnBatal" onclick="">
                Batal</button>
        </div>
    End Using 
    </div>
</div>    
<script type="text/javascript">
    var isAnyError = false;
    var attr_Det;

    submitFormCallback = function (data) {
        if (data.stat == 1) {
            showNotification("Data telah berhasil disimpan");
            GenTable.ajax.reload();
            $("#btnBatal").click();
            return false;
        }
        showNotificationSaveError(data);
    }
</script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#frmMonthly").submit(function (e) {
            e.stopPropagation();
            e.preventDefault();
            showSavingNotification();
            var _data = $(this).serialize();
            var _url = $(this).attr("action");

            var formData = new FormData(this);
            $.ajax({
                type: 'POST',
                url: _url,
                data: formData,
                success: submitFormCallback,
                error: ajax_error_callback,
                async: false,
                contentType: false,
                processData: false,
                dataType: 'json'
            });
        });

        $('#btnSavePrint').click(function () {
            var frm = $("#frmMonthly");

            showSavingNotification();
            var _data = $(frm).serialize();
            var _url = $(frm).attr("action");

            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data telah berhasil disimpan");
                        window.location.href = "/ProjectManagement/Reports/MonthlyReportPrint?Id=" + data.MnID + "&forMC0=@IIf(ViewData("ForMC0") = False OrElse IsNothing(ViewData("ForMC0")), "", "true")";
                        return false;
                    }
                    showNotificationSaveError(data);
                },
                error: ajax_error_callback,
                dataType: 'json'
            });
        });
    });
</script>
