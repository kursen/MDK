@Code
    ViewData("Title") = "Dashboard"
    ViewData("HeaderIcon") = "fa-dashboard"
    Dim _mainEntities = New MainEntities
    
    Dim _announcements = From m In _mainEntities.Announcements
                         Where m.IsPublished = True
                         
    Dim users = Membership.GetAllUsers()
   
   
  
            
End Code
@helper WriteAreaMenu()
    Dim allAreas = (From r As Route In RouteTable.Routes
            Where r.DataTokens IsNot Nothing AndAlso r.DataTokens.ContainsKey("area")
            Select r.DataTokens("area")).ToArray()
            

    
    For Each item As String In allAreas
    @Html.ActionLink(item, "Home", item, Nothing, New With {.class = "btn btn-primary"})
    Next
End Helper
<div class="row">
    <div class="col-lg-6 col-sm-6">
        @If _announcements.Count > 0 Then
            @<div class="panel panel-primary">
                <div class="panel-heading">
                    Pengumuman</div>
                <div class="panel-body">
                    @For Each item In _announcements
                    
                        @<h3>@item.Title</h3>
                        @<br />
                        @<div>
                            <strong>@item.PublishDate.ToString("dd MMMM yyyy")</strong></div>
                        @Html.Raw(item.TextContent)
                        @<hr />
                   
            Next
                </div>
            </div>
                
                
                
        End If
    </div>
    <div class="col-lg-6 col-sm-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Users</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <table class="table table-bordered table-hover">
                            <tbody>
                                @For Each u As MembershipUser In users
                                    ' Dim p = ProfileBase.Create(u.UserName)
                                    @<tr>
                                        <td>
                                            @u.UserName
                                        </td>
                                        <td class="text-center">
                                            @Html.Raw(IIf(u.IsOnline, "<span style='color:red' class='fa fa-sun-o'></span>", "<span class='fa fa-moon-o'></span>"))
                                        </td>
                                    </tr>
            
                                Next
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class='col-lg-12 col-sm-12'>
                        User online: @Membership.GetNumberOfUsersOnline()
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

  
</script>
