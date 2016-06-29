@code
    ViewData("Title") = "Beranda"
    ViewData("showHeader") = False
End Code
@Imports MDK_ERP.HtmlHelpers
<div class="row">
    <div class="col-md-offset-1 col-md-7">
        <div class="box-">
            <div class="box-content box-no-bg">
                <div class="panel panel-minimal">
                    <!-- Media list feed -->
                    <ul class="media-list media-list-feed nm">
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-cogs bgcolor-success"></i>
                            </div>
                            <div class="media-body">
                                <a href="@Url.Action("Index", "Dashboard", New With {.area = "Production"})" class="media-heading">
                                    Production</a>
                                <p class="media-text">
                                    departemen yang mengolah suatu produk dengan mengubah bentuk atau sifat suatu bahan
                                    atau merakit suku cadang menjadi produk selesai.</p>
                            </div>
                        </li>
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-suitcase bgcolor-success"></i>
                            </div>
                            <div class="media-body">
                                <a href="@Url.Action("Index", "CompanyInfo", New With {.area = "ProjectManagement"})" class="media-heading">
                                    Project Management</a>
                                <p class="media-text">
                                    proses planning, organising dan managing task atau tugas untuk mencapai tujuan yang ditetapkan.</p>
                            </div>
                        </li>
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-tags bgcolor-success"></i>
                            </div>
                            <div class="media-body">
                                 <a href="@Url.Action("Index", "SalesHome", New With {.area = "Sales"})" class="media-heading">
                                    Project and Sales</a>
                                <p class="media-text">
                                    memiliki tugas utama administrasi project dan penjualan</p>
                            </div>
                        </li>   
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-group bgcolor-muted"></i>
                            </div>
                            <div class="media-body">
                                <p class="media-heading text-muted">
                                    HRD</p>
                                <p class="media-text">
                                    memiliki tugas utama untuk mengelola sumber daya manusia di dalam perusahaan</p>
                            </div>
                        </li>
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-money bgcolor-muted"></i>
                            </div>
                            <div class="media-body">
                                <p class="media-heading text-muted">
                                    Purchasing</p>
                                <p class="media-text">
                                    Melakukan proses pembelian barang agar tersedianya barang sesuai dengan permintaan
                                    kebutuhan setiap departemen.</p>
                            </div>
                        </li>
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-dollar bgcolor-muted"></i>
                            </div>
                            <div class="media-body">
                                <p class="media-heading text-muted">
                                    Accounting</p>
                                <p class="media-text">
                                    Merencanakan, mengembangkan, dan mengontrol fungsi keuangan dan akuntansi di perusahaan
                                    dalam memberikan informasi keuangan secara komprehensif dan tepat waktu untuk membantu
                                    perusahaan dalam proses pengambilan keputusan yang mendukung pencapaian target financial
                                    perusahaan.</p>
                            </div>
                        </li>
                        <li class="media">
                            <div class="media-object pull-left">
                                <i class="icon-user-md bgcolor-success"></i>
                            </div>
                            <div class="media-body">
                                <a href="@Url.Action("Index", "Usermanagement")" class="media-heading">User Management</a>
                                <p class="media-text">
                                    Menambah User, memberikan hak akses</p>
                            </div>
                        </li>
                    </ul>
                    <!--/ Media list feed -->
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-7">
    </div>
</div>
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/css/uielement.css")" />
End Section
@Section jsScript
End Section
