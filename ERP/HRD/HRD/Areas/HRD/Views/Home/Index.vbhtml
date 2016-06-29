@Code
    ViewData("Title") = "Dashboard"
    Dim _hrdEntities = New HRD.HrdEntities
    Dim employees = (From m In _hrdEntities.Master_Personal
                     Select m.OfficeId, m.ID)
End Code
<div class="row">
    <div class="col-lg-6 col-sm-6">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Jumlah Karyawan</div>
            <div class="panel-body">
                Jumlah Pegawai: @employees.Count
            </div>
        </div>
    </div>
</div>
