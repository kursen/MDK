
@Code
    ViewData("Title") = "Konfigurasi"
    ViewBag.headIcon = "icon-wrench"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                             {"Home", "/"}, _
                                             {"Konfigurasi", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<div class="bs-glyphicons box-double-padding">
    <ul class="bs-glyphicons-list">
        <li>
            <a href="@Url.Action("Material","DataMaster")">
                <span class="glyphicon icon-book" aria-hidden="true"></span>
                <span class="glyphicon-class">Daftar Material</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("MaterialComposition", "DataMaster")">
                <span class="glyphicon icon-stackexchange" aria-hidden="true"></span>
                <span class="glyphicon-class">Komposisi Produk Material</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("MaterialUnit", "DataMaster")">
                <span class="glyphicon icon-bookmark" aria-hidden="true"></span>
                <span class="glyphicon-class">Unit Material</span>
            </a>
        </li>
        @*<li>
            <a href="@Url.Action("MaterialWeightRatio", "DataMaster")">
                <span class="glyphicon icon-retweet" aria-hidden="true"></span>
                <span class="glyphicon-class">Rasio Bobot Material</span>
            </a>
        </li>*@
        @*<li>
            <a href="@Url.Action("InventoryStatus", "DataMaster")">
                <span class="glyphicon icon-check" aria-hidden="true"></span>
                <span class="glyphicon-class">Status Inventaris</span>
            </a>
        </li>*@
        <li>
            <a href="@Url.Action("MachineType", "DataMaster")">
                <span class="glyphicon icon-tasks" aria-hidden="true"></span>
                <span class="glyphicon-class">Jenis Mesin</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("MaterialMachine", "DataMaster")">
                <span class="glyphicon icon-cog" aria-hidden="true"></span>
                <span class="glyphicon-class">Inventaris Mesin</span>
            </a>
        </li>
        @*<li>
            <a href="@Url.Action("MaterialVehicle", "DataMaster")">
                <span class="glyphicon icon-truck" aria-hidden="true"></span>
                <span class="glyphicon-class">Inventaris Kendaraan</span>
            </a>
        </li>
        <li>
            <a href="@Url.Action("MaterialWeigher", "DataMaster")">
                <span class="glyphicon icon-exchange" aria-hidden="true"></span>
                <span class="glyphicon-class">Inventaris Timbangan</span>
            </a>
        </li>*@
    </ul>
    </div>

@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/css/docs.min.css")" />
End Section

@Section jsScript
End Section
