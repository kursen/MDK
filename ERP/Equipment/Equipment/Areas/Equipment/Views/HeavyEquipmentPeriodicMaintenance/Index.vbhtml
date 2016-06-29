@Code
    ViewData("Title") = "Perawatan Alat Berat"
    
    Dim p = ERPBase.ErpUserProfile.GetUserProfile()
    
    Dim context As New Equipment.EquipmentEntities
    Dim Hq = From m In context.HeavyEqps
                   Where m.IDArea = p.WorkUnitId
    
    Dim counter As Integer = 1
End Code
@Using Html.BeginJUIBox("Daftar Skema Perawatan Alat Berat")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <table class="table table-bordered" id="tblServiceSchedule">
                <thead>
                    <tr>
                        <th style="width: 60px">
                            #
                        </th>
                        <th style="width: 100px">
                            Kode
                        </th>
                         <th style="width: 200px">
                            Jenis
                        </th>
                      
                        <th>
                            Merk/Type
                        </th>
                       
                        <th style="width: 140px">
                        </th>
                    </tr>
                </thead>
                <tbody>
                   @For Each v In Hq
                           @<tr>
                           <td class="text-right">@counter</td>
                           <td>@v.Code</td>
                           <td>@v.Species</td>
                           <td>@v.Merk / @v.Type</td>
                           <td class="text-center">@Html.ActionLink("Lihat", "MaintenanceSchema", New With {.id = v.ID},New With{.class="btn btn-primary"})</td>
                           </tr>
                       counter += 1
                       Next
                </tbody>
            </table>
        </div>
    </div>
                   End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

    var tblServiceSchedule = null;
    $(function () {

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = null;
        datatableDefaultOptions.columnDefs = [{ "targets": [0, 4], "orderable": false}];
        datatableDefaultOptions.order = [[2, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        tblServiceSchedule = $("#tblServiceSchedule").DataTable(datatableDefaultOptions);
    }); //end init
</script>