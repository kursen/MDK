@ModelType ProjectManagement.ProjectInfo
@Code
    ViewData("Title") = "Informasi Proyek"
    
End Code
@Html.Partial("ProjectPageMenu", Model)
@Html.Hidden("ProjectInfoId", Model.Id)
@Using Html.BeginJUIBox("Mutual Check 0")
    
    @<p>Rencana anggaran fisik belum lengkap. Mutual Check0 tidak dapat ditampilkan.</p>
    @<p>
    @Html.Raw(ViewData("errorReason") )
    </p>
End Using