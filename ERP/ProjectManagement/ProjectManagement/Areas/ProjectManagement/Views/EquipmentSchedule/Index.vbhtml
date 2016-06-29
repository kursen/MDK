@ModelType ProjectManagement.Equipment
@Code
    ViewData("Title") = "Informasi Proyek"
End Code
@Html.Partial("ProjectPageMenu", ViewData("ProjectInfo"))
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <button class="btn btn-sm btn-success  btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</button>
        </div>
    </div>
</div>
<!--form nambah peralatan -->
@Using Html.BeginJUIBox("Data Peralatan", False, False, False, False, False, "fa fa-plus")
    @<div id="hideForm" style="display: block;">
    @Using Html.BeginForm("Create", "EquipmentSchedule", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
        @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
        @<div class="row">
              @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Equipments, New With {.class = "form-control"}), "Peralatan")
                         @Html.Hidden("ProjectInfoID",ViewData("ProjectInfo").id)
                 
              @Html.WriteFormIntegerInputFor(Function(m) m.Volume, "Volume", ".", smLabelWidth:=4, lgLabelWidth:=3, lgControlWidth:=4, smControlWidth:=3)

              @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Unit, New With {.class = "form-control", .value = "Unit"}), "Satuan")
                
              @Html.WriteFormDateInputFor(Function(m) m.BeginDate, "Tanggal Mulai",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    smControlWidth:=4, lgControlWidth:=2)

              @Html.WriteFormDateInputFor(Function(m) m.EndDate, "Tanggal Selesai",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    smControlWidth:=4, lgControlWidth:=2)
            </div>
        @<div class="form-actions form-actions-padding-sm">
            <div class="row">
                <div class="col-md-5 col-md-offset-5">
                    <button class="btn btn-primary" type="submit" id="btnSave">
                        <i class="fa fa-save"></i> Simpan</button>
                    <button class="btn" type="button" onclick="$('.btn-add').click()">
                        Batal</button>
                </div>
            </div>
        </div>
End Using
</div>
End Using
<!-- end form-->
@Using Html.BeginJUIBox("Daftar Peralatan")
    @Html.Hidden("IDProjectInfo", ViewData("ProjectInfo").id)
    @<div class="table-responsive">
	<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
        width="100%">
        <thead>
            <tr>
               
                <th>
                    Jenis Peralatan
                </th>
                <th>
                    Volume
                </th>
                <th>
                    Sat
                </th>
                <th>
                    Mulai
                </th>
                <th>
                    Selesai
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
	</div>

End Using

<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.overrides_indo.js")" type="text/javascript"></script>
<script src="@Url.Content("~/js/common.js")" type="text/javascript"></script>
<script type="text/javascript" src="@Url.Content("~/js/shared-function.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/EquipmentSchedule/index.js")"></script>