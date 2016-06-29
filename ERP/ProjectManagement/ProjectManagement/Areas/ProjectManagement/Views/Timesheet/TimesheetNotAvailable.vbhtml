@ModelType ProjectManagement.ProjectInfo
@Code
    ViewData("Title") = "Informasi Proyek"
    
End Code
@Html.Partial("ProjectPageMenu", Model)
@Html.Hidden("ProjectInfoId", Model.Id)
@Using Html.BeginJUIBox("Timesheet")
    
    @<p>Rencana anggaran fisik belum lengkap. Timesheet tidak dapat ditampilkan.</p>
    @<p>
    @Html.Raw(ViewData("errorReason") )
    </p>
End Using