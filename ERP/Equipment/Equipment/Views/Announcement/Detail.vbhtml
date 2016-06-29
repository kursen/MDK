@ModelType ERPBase.Announcement
@Code
    ViewData("Title") = "Detail"
End Code
@Using Html.BeginJUIBox("Pengumuman")
    
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <h2>@Model.Title</h2>
            <br />
            <strong>@Model.PublishDate.ToString("dd-MM-yyyy")</strong>
            <br />
            <br />
            <div>
                @Html.Raw(Model.TextContent)
            </div>
        </div>
    </div>
    
End Using
