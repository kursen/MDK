@ModelType MDK_ERP.AMPJournals

@Code
    ViewData("Title") = "Jurnal Hotmix"
    ViewBag.headIcon = "icon-cogs"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Daftar Jurnal Hotmix", "/Production/Process/AMP"}, _
                                             {"Form Jurnal hotmix", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-pencil'></i> Form Jurnal Hotmix</h4>
@Using Html.RowBox("", True)
    Using Html.BeginForm("EditAmp", "Process", Nothing, FormMethod.Post, New With {.name = "form-amp", .id = "form-amp", .class = "form-horizontal"})
    @Html.ValidationSummary(True, "Proses simpan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
        @Html.Hidden("ID")
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Tanggal Produksi</label>
        <div class="col-md-3 col-sm-4">
            <div class='input-group date' id='dtpDateUse'>
            @Html.TextBoxFor(Function(m) m.DateUse, New With {.class = "form-control", .Value = Now.Date.ToShortDateString})
                <span class="input-group-addon"><span class="icon-calendar"></span></span>
            </div>
            @Html.ValidationMessageFor(Function(m) m.DateUse)
        </div>  
    </div>
    
    @<div class="form-group">
    <div class="col-sm-8">
        <div class="form-group">
            <label class="col-sm-offset-1 col-md-offset-3 col-sm-3 col-lg-2 control-label">
                Jam Mulai</label>
            <div class="col-md-3 col-sm-3">
                <div class='input-group time' id="BeginTime">
                    @Html.TextBox("BeginProd", Now.ToShortTimeString, New With {.class = "form-control text-right"})
                    <span class="input-group-addon"><span class="icon-time"></span></span>
                </div>
            </div>
        </div>
         <div class="form-group">
            <label class="col-sm-offset-1 col-md-offset-3 col-sm-3 col-lg-2 control-label">
                Jam Selesai</label>
            <div class="col-md-3 col-sm-3">
                <div class='input-group time' id="EndTime">
                    @Html.TextBox("EndProd",  Now.ToShortTimeString, New With {.class = "form-control text-right"})
                    <span class="input-group-addon"><span class="icon-time"></span></span>
                </div>
            </div>
        </div>
    </div>
    </div>
    
     @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Operator</label>
        <div class="col-md-3 col-sm-4">
            @Html.TextBox("Operator", Nothing, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.Operator)
        </div>
    </div>
      @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Shift</label>
        <div class="col-sm-2">
             @Html.DropDownList("IdShift", Nothing, "Pilih Shift", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdShift)
        </div>
    </div>
    @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Mesin AMP</label>
        <div class="col-sm-2">
            @Html.DropDownList("IdMachine", Nothing, "Pilih AMP", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdMachine)
        </div>
    </div>
     @*@<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Project</label>
        <div class="col-sm-2">
            @Html.DropDownList("IDProject", Nothing, "Pilih No Project", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IDProject)
        </div>
    </div>*@
     @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Produk</label>
        <div class="col-sm-2">
            @*@Html.DropDownList("IDMaterial", New SelectList(DirectCast(ViewData("ListAsphal"), IEnumerable), "IDMaterial", "Name"), "Pilih Hot Mix", New With {.class = "form-control select2"})*@
            @Html.Hidden("IDMaterial")
            @Html.TextBox("MaterialName", ViewData("MaterialName"), New With {.class = "form-control", .disabled = True})
            @Html.ValidationMessageFor(Function(m) m.IDMaterial)
        </div>
    </div>
     @<div class="form-group">
        <div class="col-sm-8">
            @code
                Dim ctx As New MDK_ERP.ERPEntities
                Dim dataComp = ctx.CompositionProductList(Model.IDMaterial, 78).ToList() 'ctx.CompInUseJournal(Model.ID).ToList()
            End Code
            @For Each item In dataComp
                @<div class="form-group new">
                <input type="hidden" name="IDMaterial" value="@item.IDMaterial" />
                <input type="hidden" name="IDMeasurementUnit" value="@item.IdMeasurementUnit" />
                <label class="col-xs-offset-1 col-sm-offset-4 col-sm-3 col-lg-2 control-label">@item.MaterialComp</label>
                <div class="col-xs-offset-1 col-sm-offset-0 col-sm-3 col-lg-3 input-group">
                    <input type="text" class="form-control text-right amountcomposition" name="AmountComp" value="@item.AmountComp" readonly/>
                    <span class="input-group-addon">Ton</span>
                </div>
                </div>
            Next
        </div>
      </div>

   @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Keterangan</label>
        <div class="col-sm-3">
            @Html.TextArea("Description",New With{.class="form-control"})
            @Html.ValidationMessageFor(Function(m) m.Description)
        </div>
    </div>
    
    @<div class="form-actions form-actions-padding-sm">
    <div class="row">
        <div class="col-md-5 col-md-offset-2">
        <button type="submit" class="btn btn-primary"><i class="icon-save"></i> Simpan</button>
        &nbsp;
        <button class="btn" type="button" onclick="goBack();"> Kembali</button>
        </div>
    </div>
    </div>
    End Using
End Using

@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section
@Section jsScript
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
<script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/shared-function.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Process/EditAmp.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>

End Section

