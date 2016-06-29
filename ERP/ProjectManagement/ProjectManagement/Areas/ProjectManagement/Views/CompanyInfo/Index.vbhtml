@Code
    ViewData("Title") = "Informasi Kontraktor"
    ViewData("HeaderIcon") = "fa-building"
  
End Code

<div class="row">
    <div class="form form-horizontal">
        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-7">
            <div class="form-group">
                <label class="col-md-2 col-sm-2 col-xs-12 col-lg-3 text-left control-label">
                    Nama Perusahaan</label>
                <div class="col-md-4 col-sm-4 col-xs-10 no-side-padding">
                    <input type='text' class="form-control" id="fltName" />
                </div>
                <div class="col-md-4 col-sm-4 col-xs-10">
                    <a onclick="" id="filterBtn" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                    <span class="loader fa fa-spinner fa-spin hidden"></span>
                </div>
            </div>
           @* <div class="form-group">
                <div class="col-md-4 col-sm-4 col-xs-12 col-md-offset-2 col-sm-offset-2 col-xs-12 col-lg-offset-3">
                    <a onclick="" id="filterBtn" href="javascript:void(0)" class="btn btn-primary">Filter</a>
                    <span class="loader fa fa-spinner fa-spin hidden"></span>
                </div>
            </div>*@
        </div>
    </div>
</div>
<div class="clear">
</div>
@Using Html.BeginJUIBox("Daftar Perusahaan")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <a class="btn btn-sm btn-success btn-label-left" href="@Url.Action("Create", "CompanyInfo","ProjectManagement_default")">
                    <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
            </div>
        </div>
    </div>
	
    @<div class="table-responsive">
	<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer" width="100%">
        <colgroup>
            <col style="width: 50px;" />
            <col />
            <col style="width: 130px;" />
            <col style="width: 180px;" />
            <col style="width: 120px;" />
        </colgroup>
        <thead>
            <tr>
                <th>
                    #
                </th>
                <th>
                    Nama
                </th>
                <th>
                    Alias
                </th>
                <th>
                    Kota
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>
	</div>
End Using

<script src="../../../../plugins/datatables/jquery.dataTables.js" type="text/javascript"></script>
<script src="../../../../plugins/datatables/dataTables.overrides_indo.js" type="text/javascript"></script>

<script type="text/javascript" src="@Url.Content("~/js/CRUDHelpers.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Areas/ProjectManagement/Scripts/CompanyInfo.js")"></script>
