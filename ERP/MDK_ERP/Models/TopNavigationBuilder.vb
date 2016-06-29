Public Interface ITopNavigation
    Function Render(ByVal ActiveLink As String) As String
End Interface
Public MustInherit Class TopNavigation
    Implements ITopNavigation
    Protected Friend Const cACTIVECLASS = " active" ' " active-parent active"
    Property Id As Integer
    Property ParentId As Integer
    Property Label As String
    Property Ordinal As Integer
    Property Link As String
    Property ModuleName As String
    Public MustOverride Function Render(ByVal ActiveLink As String) As String Implements ITopNavigation.Render

End Class

Public Class TopNavigationLink
    Inherits TopNavigation

    Public Overrides Function Render(ActiveLink As String) As String
        Dim activeClass = String.Empty
        If ActiveLink = Link Then
            activeClass = cACTIVECLASS
        End If
        Dim sb As New StringBuilder
        sb.AppendFormat("<li class='{0}' >", activeClass)
        sb.AppendFormat("<a href='{0}' class='{1}'>", Link, activeClass)
        sb.Append(Label & "</a>")
        sb.AppendLine("</li>")
        Return sb.ToString()
    End Function
End Class
Public Class TopNavigationDropDownLink
    Inherits TopNavigation
    Property Items As List(Of TopNavigation)


    Public Sub New()
        Items = New List(Of TopNavigation)
    End Sub
    Public Overrides Function Render(ActiveLink As String) As String
        Dim activeClass = String.Empty

        Dim found = Not Items.Where(Function(m) m.Link = ActiveLink).SingleOrDefault() Is Nothing
        Dim itemSb = New StringBuilder
        For Each item In Items
            itemSb.Append(item.Render(ActiveLink))
        Next

        If itemSb.ToString().Contains("active") Then
            activeClass = cACTIVECLASS
        End If

        Dim rvalue As New StringBuilder
        rvalue.AppendFormat("   <li class='dropdown {0}'>", activeClass)
        rvalue.AppendFormat("     <a href='#' class='dropdown-toggle{0}' data-toggle='dropdown'>", activeClass)

        rvalue.AppendFormat("{0} <span class='caret'></span>", Label)
        rvalue.Append("</a>")
        rvalue.AppendLine()
        rvalue.AppendLine("     <ul class='dropdown-menu' role='menu'>")
        rvalue.Append(itemSb)
        rvalue.AppendLine()
        rvalue.AppendLine("</ul>")
        Return rvalue.ToString()
    End Function
End Class
Public Class TopNavigationDevider
    Inherits TopNavigation

    Public Overrides Function Render(ActiveLink As String) As String
        Return "<li class='divider'></li>"
    End Function
End Class
Public Class TopNavigationBuilder
    Private Shared _theUser As System.Security.Principal.IPrincipal
    Private Shared _Items As List(Of TopNavigationMenu)
    Private Shared _activeLink As String
    Public Shared Function Render(ByVal activeLink As String, ByVal ModuleName As String, ByVal user As System.Security.Principal.IPrincipal) As String
        _theUser = user
        Dim rvalue As New List(Of ITopNavigation)
        Dim entities = New ERPEntities
        _Items = entities.TopNavigationMenu.Where(Function(m) m.ModuleName = ModuleName).ToList()

        rvalue.AddRange(BuildSubMenu(0))
        Dim sb As New StringBuilder

        For Each item In rvalue
            sb.AppendLine(item.Render(activeLink))
        Next
        Return sb.ToString

    End Function
    Private Shared Function BuildSubMenu(ByVal parentId As Integer) As List(Of TopNavigation)
        Dim rvalue As New List(Of TopNavigation)
        Dim theItems = _Items.Where(Function(m) m.ParentId = parentId).OrderBy(Function(m) m.Ordinal)
        For Each item In theItems
            If item.Link = "#" Then
                Dim gItem = New TopNavigationDropDownLink With {.Link = item.Link,
                                                    .Label = item.Label,
                                                    .Id = item.Id,
                                                    .ParentId = item.ParentId}
                gItem.Items = BuildSubMenu(gItem.Id)
                If Not gItem.Items.Count = 0 Then
                    rvalue.Add(gItem)
                End If

            ElseIf item.Link = "-" Then
                Dim theDivider = New TopNavigationDevider
                rvalue.Add(theDivider)

            Else


                'Dim s = item.SideBarRoles.Select(Of String)(Function(m) m.RoleName).ToArray()
                Dim allow As Boolean = True
                'If s.Count > 0 Then
                '    For Each rolename In s
                '        If user.IsInRole(rolename) Then
                '            allow = True
                '            Exit For
                '        End If
                '    Next
                'Else
                '    allow = True
                'End If

                If allow Then
                    rvalue.Add(New TopNavigationLink With {.Link = item.Link,
                                                  .Label = item.Label, .Id = item.Id, .ParentId = item.ParentId})
                End If
            End If
        Next
        Return rvalue
    End Function
End Class
