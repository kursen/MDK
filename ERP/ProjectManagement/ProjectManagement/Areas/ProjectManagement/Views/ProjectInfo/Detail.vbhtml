@ModelType ProjectManagement.ProjectInfo
@Code
    ViewData("Title") = "Informasi Proyek"
    Html.SetEditableDefaultSettings(DisableOnload:=True)
End Code
@Html.Partial("ProjectPageMenu", Model)
@Using Html.BeginJUIBox("Proyek Detail")
    If Model.Archive = False Then
        @<div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    <button type="button" class="btn btn-danger btn-label-left" onclick="$('#dProjectInfo .editable') .editable('toggleDisabled')">
                        <span><i class="fa fa-edit"></i></span>Edit</button>
                </div>
            </div>
        </div>
    End If
    @<dl class="dl-horizontal" id='dProjectInfo'>
        <dt>Kode Proyek</dt>
        <dd>@Html.EditableInputTextBox("ProjectCode", Model.ProjectCode, "text", "Kode Proyek", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")</dd>
        <dt>Paket</dt>
        <dd>@Html.EditableInputTextBox("ProjectTitle", Model.ProjectTitle, "text", "Nama Proyek", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")</dd>
        <dt>Kontraktor</dt>
        <dd>@Html.EditableInputTextBox("CompanyInfoId", Model.CompanyInfo.Name, "select", "Nama Perusahaan", datapk:=Model.Id, sourcelist:="/ProjectInfo/GetCompanyList", dataurl:="/ProjectInfo/SavePartial")</dd>
        <dt>Nomor</dt>
        <dd>@Html.EditableInputTextBox("ContractNumber", Model.ContractNumber, "text", "No Kontrak", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")
            &nbsp;</dd>
        <dt>Nilai Proyek</dt>
        <dd>@Html.EditableInputTextBox("ContractValue", Model.ContractValue.ToString(), "text", "Nilai Kontrak", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")&nbsp;</dd>
        <dt>Tanggal Mulai</dt>
        <dd>@Html.EditableInputTextBox("DateStart", Model.DateStart.ToString("dd-MM-yyyy"), "date", "Tanggal Mulai", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")
            &nbsp;s/d @Model.DateStart.AddDays(Model.NumberOfDays - 1).ToString("dd-MM-yyyy")
        </dd>
        <dt>Lama Pekerjaan</dt>
        <dd>@Html.EditableInputTextBox("NumberOfDays", Model.NumberOfDays, "number", "Lama Pekerjaan", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")
            &nbsp;Hari Kalender &nbsp;</dd>
        <dt>Lokasi</dt>
        <dd>@Html.EditableInputTextBox("Location", Model.Location, "text", "Lokasi", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")&nbsp;</dd>
        <dt>Konsultan</dt>
        <dd>@Html.EditableInputTextBox("ConsultanName", Model.ConsultanName, "text", "Nama Konsultan", datapk:=Model.Id, dataurl:="/ProjectInfo/SavePartial")&nbsp;</dd>
    </dl>
End Using
@Using Html.BeginJUIBox("Dokumen")
    @<div id="divList">
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="pull-right">
                    <a href="javascript:void(0)" type="button" class="btn btn-danger btn-label-left"
                        id="btnUpload"><span><i class="fa fa-upload"></i></span>Upload </a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12">
				<div class="table-responsive">
                <table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
                    id="DocList" width="100%">
                    <colgroup>
                        <col style="width: 50px;" />
                        <col />
                        <col />
                        <col style="width: 100px;" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Judul Dokumen
                            </th>
                            <th>
                                Deskripsi Isi
                            </th>
                            <th style="width: 100px !important;">
                            </th>
                        </tr>
                    </thead>
                </table>
				</div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div id='divForm' class="col-lg-12 col-sm-12" style='display: none'>
            @Html.Partial("UploadDoc")
        </div>
    </div>
    
End Using

<style type="text/css">
    .popover
    {
        z-index: 40000;
    }
</style>
@Section endScript
    <script type="text/javascript">




        $.fn.editabletypes.date.defaults.datepicker.language = "id";
        $.fn.editabletypes.date.defaults.viewformat = "dd-mm-yyyy";
        $.fn.editabletypes.date.defaults.format = "dd-mm-yyyy";

        var _savepartialresponse = function (data) {
            if ((data) && (data.stat == 1)) {

            } else {
                showNotification(data);
            };

        }

        $(function () {


            $("#ContractValue").number($("#ContractValue").text(), 2, ",", ".");

        })
    </script>
    <script src="@Url.Content("~/plugins/jquery/jquery-migrate-1.2.1.min.js")" type="text/javascript"></script>
    <script src="../../../../plugins/datatables/jquery.dataTables.js" type="text/javascript"></script>
    <script src="../../../../plugins/datatables/dataTables.overrides_indo.js" type="text/javascript"></script>
    <script type="text/javascript" src="@Url.Content("~/js/CRUDHelpers.js")"></script>
    <script src="@Url.Content("~/Areas/ProjectManagement/Scripts/UploadProjectDoc.js")" type="text/javascript"></script>
End Section
