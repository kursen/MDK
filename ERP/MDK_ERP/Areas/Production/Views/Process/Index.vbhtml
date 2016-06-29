@Code
    ViewData("Title") = "Crusher"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Production", "/Production"}, _
                                             {"Crusher", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<form class="form-horizontal" role="form">
  <div class="form-group">
    <label class="col-sm-2 control-label">Tanggal Pelaksanaan</label>
    <div class="col-sm-2">
      <input type="text" class="form-control" id="dt_do">
    </div>
  </div>
  <div class="form-group">
    <label class="col-sm-2 control-label">Crusher</label>
    <div class="col-sm-2">
      <select class="form-control">
  <option>Crusher</option>
  <option>I</option>
  <option>II</option>
</select>
    </div>
  </div>
  <div class="form-group">
    <label for="" class="col-sm-2 control-label">Operator</label>
    <div class="col-sm-3">
      <input type="text" class="form-control" id="" />
    </div>
  </div>
   <div class="form-group">
    <label for="" class="col-sm-2 control-label">Shift</label>
    <div class="col-sm-2">
     <select class="form-control">
  <option>Shift</option>
  <option>Pagi</option>
  <option>Malam</option>
</select>
    </div>
  </div>
   <div class="form-group">
    <label for="" class="col-sm-2 control-label">Jumlah Grogol</label>
    <div class="col-sm-1">
      <input type="text" class="form-control" id="" />
    </div>
    <div class="col-sm-offset-1"><label class="control-label">Bucket</label></div>
  </div>
  <div class="form-group">
  <label class="col-sm-3 control-label">Output Grogol</label>
  </div>
   <div class="form-group">
    <label for="" class="col-sm-2 control-label">Abu</label>
    <div class="col-sm-1">
      <input type="text" class="form-control" id="">
    </div>
    <div class="col-sm-offset-1"><label class="control-label">Bucket</label></div>
  </div>
   <div class="form-group">
    <label for="" class="col-sm-2 control-label">Split 1,2</label>
    <div class="col-sm-1">
      <input type="text" class="form-control" id="">
    </div>
    <div class="col-sm-offset-1"><label class="control-label">Bucket</label></div>
  </div>
   <div class="form-group">
    <label for="" class="col-sm-2 control-label">Split 2,3</label>
    <div class="col-sm-1">
      <input type="text" class="form-control" id="">
    </div>
    <div class="col-sm-offset-1"><label class="control-label">Bucket</label></div>
  </div>
  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <button type="submit" class="btn btn-default">Simpan</button>
    </div>
  </div>
</form>
@section StyleSheet
<link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/js/moment.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/js/moment-with-locales.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/shared-function.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Scripts/CRUDHelpers.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Areas/Production/Scripts/crusher-pengolahan.js")"></script>
End Section
