@Code
    ViewData("Title") = "Halaman Error"
    ViewBag.showHeader = False
End Code

<h1>@ViewData("errTitle")</h1>
<hr class="hr-normal" />
<div id="defaults-change-alert" class="alert alert-warning" role="alert">
  <strong>Description : </strong> @ViewData("Exception")
</div>

<div class="pull-right">
    <a class="btn btn-default" href="javascript:void(0)" onclick="goBack();">Kembali ke halaman sebelumnya</a>
    <a class="btn btn-success" href="@Url.RouteUrl("Production_default", New With {.controller = "Dashboard", .Action = "Index"})">Dashboard</a>
</div>
