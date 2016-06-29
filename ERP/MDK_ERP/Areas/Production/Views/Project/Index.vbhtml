@Code
    ViewData("Title") = "Daftar Proyek"
    ViewBag.headIcon = "icon-briefcase"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Daftar Proyek", Nothing}
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<a class="btn btn-sm btn-success pull-right" href="javascript:void(0)" data-toggle="modal" id="mdl" data-target="#modal-Data">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>
@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>
                    No. Proyek
                </th>
                <th>
                    Nama Paket
                </th>
                <th>
                    Tgl Mulai
                </th>
                <th>
                    Tgl Akhir
                </th>
                <th>
                    Nama Agen
                </th>
                <th>
                    Keterangan
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
End Using
<!-- Start Dummy Form -->
<section class="">
<!--** start modal **-->
    @Using Html.ModalForm("modal-Data", "Form Proyek", Url.Action("Save", "Project"), "", "POST", "form-Data", "form-horizontal")
        @<div class="form-group">
            <label class="col-md-3 control-label">No. Proyek</label>
            <div class="col-md-8">
                @Html.Hidden("Id")
                @Html.TextBox("NoProject", Nothing, New With {.class = "form-control"})
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Paket</label>
            <div class="col-md-8">
                @Html.TextBox("PackageName", Nothing, New With {.class = "form-control"})
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Tgl Mulai</label>
            <div class="col-md-5">
                <div class='input-group date' id='dtpStartDate'>
                    @Html.TextBox("StartDate", nothing, New With {.class = "form-control"})
                    <span class="input-group-addon"><span class="icon-calendar"></span></span>
                </div>
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Tgl Akhir</label>
            <div class="col-md-5">
                <div class='input-group date' id='dtpEndDate'>
                    @Html.TextBox("EndDate", Nothing, New With {.class = "form-control"})
                    <span class="input-group-addon"><span class="icon-calendar"></span></span>
                </div>
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Nama Agen</label>
            <div class="col-md-8">
                @Html.TextBox("AgentName", Nothing, New With {.class = "form-control"})
            </div>
        </div>
        @<div class="form-group">
            <label class="col-md-3 control-label">Keterangan</label>
            <div class="col-md-8">
                @Html.TextArea("Description", New With {.class = "form-control"})
            </div>
        </div>
        @<div class="row">
            <div class="col-md-12">
                @Using Html.WriteSummaryBox("Daftar Ruas Jalan Proyek", "icon-list")
                    @<a  id="addDetail"  class="btn" href="javascript:void(0)">
	                    <i class="icon-plus btn btn-success"></i> Tambah Detail
                     </a>
                    @<table id="tb_DetailProject" class="display" cellspacing="0" width="100%">
                        <thead>
                            <tr>
                                <th style="visibility: hidden">ID</th>
                                <th>Nama Ruas</th>
                                <th>Panjang (km)</th>
                                <th class="action"></th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                End Using
            </div>
        </div>
    End Using
<!-- end modal -->
</section>
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/Project/Project.js")"></script>
End Section
