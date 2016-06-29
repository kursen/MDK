@Code
    'Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Laporan</title>
</head>
<body>
    <h2>Html List in PDF</h2>
    <table width="100%">
        <tr>
            <td>First Name</td>
           
            <td>Last Name</td>
        </tr>
       @code
           Dim listdata = ViewData("report")
           For Each item In listdata
       End Code
            <tr>
                <td>@item.Name</td>
               
                <td>@Html.TextBox("value",Nothing,New With{.class="form-control",.style="width:30%;"})</td>
            </tr>
            @code
                Next
            End Code
           
    </table>
</body>
</html>
