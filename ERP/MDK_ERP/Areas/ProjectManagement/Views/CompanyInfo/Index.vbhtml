@Code
    ViewData("Title") = "Informasi Perusahaan"
    ViewBag.headIcon = "icon-building"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Project Management", "/ProjectManagement"}, _
                                            {"Informasi Perusahaan", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<div class="row">
    <div class="form form-horizontal">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-7">
        <div class="form-group">
            <label class="col-md-2 col-sm-2 col-xs-12 col-lg-3 text-left control-label">Nama Perusahaan</label>
            <div class="col-md-4 col-sm-4 col-xs-10">
                <input type='text' class="form-control" />
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-4 col-sm-4 col-xs-12 col-md-offset-2 col-sm-offset-2 col-xs-12 col-lg-offset-3">
                <a onclick="" id="filterBtn" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                <span class="loader hidden"></span>
            </div>
        </div>

        </div>
    </div>
</div>

<a class="btn btn-sm btn-success pull-right" href="@Url.Action("Create","CompanyInfo")" style="margin-right: 9px;margin-top: -50px;">
	<i class="icon-plus"></i> Tambah Baru
</a>
<div class="clear"></div>

@Using Html.RowBox("box-table")
    @<table id="tb_Data" class="display" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th>Nama</th>
                <th>Alias</th>
                <th>Kota</th>
                <th class="action"></th>
            </tr>
        </thead>
    </table>
End Using

@Section StyleSheet
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/CompanyInfo.js")"></script>
End Section
