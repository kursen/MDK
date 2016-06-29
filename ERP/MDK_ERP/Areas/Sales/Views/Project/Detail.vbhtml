@imports MDK_ERP.HtmlHelpers

@Code
    
    ViewData("Title") = "Proyek Detail"
    ViewBag.headIcon = "icon-tasks"
End Code
@Section StyleSheet
    <style>
        div.form-group
        {
            margin-bottom: 0px;
        }
    </style>
End Section
<div class="row">
    <h1>
        Pengaspalan Jalan Padang-bukit tinggi km 20-30</h1>

</div>

<h2>Overview</h2>
<hr />
<form class="form-horizontal">

@Html.WriteStaticFormInput( "Kode Project", "001-0010-0001")
@Html.WriteStaticFormInput("Nama Project", "Pengerasan Jalan Padang Bukit Tinggi Km 20-30")
@Html.WriteStaticFormInput("No. SPK", "20344/3xx/3434/2015")
@Html.WriteStaticFormInput("Tanggal SPK", "02-01-2015")
@Html.WriteStaticFormInput("Tanggal Mulai", "20-05-2015")
@Html.WriteStaticFormInput("Tanggal Akhir", "30-07-2015")

<h2>Jadwal kebutuhan Aspal</h2>
<hr />
<table class="table table-bordered">
<thead>
    <tr>
        <th>#</th>
        <th>Tanggal</th>
        <th>Jenis</th>
        <th>Volume</th>
    </tr>
</thead>
</table>

</form>
