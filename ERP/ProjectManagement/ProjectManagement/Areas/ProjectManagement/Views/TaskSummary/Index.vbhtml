@ModelType List(Of ProjectManagement.ProjectTaskDoneSummary)
    
@Code
    ViewData("Title") = "Index"
    
    Dim ProjectInfo = CType(ViewData("ProjectInfo"), ProjectManagement.ProjectInfo)
    
End Code

@Html.Partial("ProjectPageMenu", ProjectInfo)

@Using Html.BeginJUIBox("Ringkasan Penyelesaian Proyek")
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <table class="table table-bordered" id="tblSummary">
                <colgroup>
                    <col style="width: 100px;" />
                    <col style="width: auto;" />
                    <col style="width: 100px" />
                    <col style="width: 40px" />
                    <col style="width: 100px" />
                    <col style="width: 40px" />
                    <col style="width: 100px" />
                    <col style="width: 40px" />
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            No. Item
                        </th>
                        <th>
                            Uraian
                        </th>
                        <th colspan="2">
                            Volume
                        </th>
                        <th colspan="2">
                            Selesai
                        </th>
                        <th colspan="2">
                            Sisa
                        </th>
                        <th>
                            Persentase
                        </th>
                        <th>
                            Detail
                        </th>
                    </tr>
                    
                </thead>
                <tbody>
                
                        
                @Code
                    Dim sm = From m In Model
                 Order By m.ord
                 Group By m.DivisionNumber, m.DivisionTitle Into g = Group
                Select New With {DivisionNumber, DivisionTitle, g}

                    For Each Division In sm
                        @<tr>
                        <td> </td>
                        <td colspan="8">@Division.DivisionNumber. @Division.DivisionTitle</td>
                        </tr>
                        For Each taskItem In Division.g
                        @<tr>
                        <td class="text-center">@taskItem.PaymentNumber</td>
                        <td>@taskItem.TaskTitle</td>
                        <td class="text-right">@taskItem.TargetQuantity.ToString("N2")</td>
                        <td>@taskItem.UnitQuantity</td>
                        <td class="text-right">@taskItem.Volume.ToString("N2")</td>
                        <td>@taskItem.UnitQuantity</td>
                        <td class="text-right">@taskItem.LeftOver.ToString("N2")</td>
                        <td>@taskItem.UnitQuantity</td>
                        <td class="text-right">@taskItem.Percentage.ToString("N2") %</td>
                        <td class="text-center">
                        @Html.ActionLink("Detail", "DetailSummary", New With {.taskId= taskItem.TaskId})
                        </td>
                        </tr>    
                        Next
                    Next
                    
                End Code
                        
                
                
                </tbody>
            </table>
        </div>
    </div>
    
    
    
    
                End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

    var tblSummary = null;
    $(function () { //init
        datatableDefaultOptions.searching = false;


       // tblSummary = $("#tblSummary").DataTable(datatableDefaultOptions);


    });  //end init

</script>
