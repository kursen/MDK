Namespace MenuBuilder


    Interface ISideMenu
        Function Render(ByVal Href As String) As String
    End Interface
    Public MustInherit Class SideMenu
        Implements ISideMenu
        Protected Friend Const cACTIVECLASS = " active-parent active"
        Property Id As Integer
        Property ParentId As Integer
        Property Icon As String
        Property Label As String
        Property Ordinal As Integer
        Property ActionLink As String
        Public MustOverride Function Render(ByVal href As String) As String Implements ISideMenu.Render
    End Class

    Public Class SideMenuLink
        Inherits SideMenu

        Overrides Function Render(ByVal ActiveLink As String) As String
            Dim activeClass = String.Empty
            If ActiveLink = ActionLink Then
                activeClass = cACTIVECLASS
            End If
            Dim sb As New StringBuilder
            sb.Append("<li>")
            sb.AppendFormat("<a href='{0}' class='ajax-link{1}'>", ActionLink, activeClass)
            If String.IsNullOrEmpty(Icon) = False Then
                sb.AppendFormat("<i class='fa {0}'></i> ", Icon)
            End If
            sb.Append(Label & "</a>")
            sb.AppendLine("</li>")
            Return sb.ToString()
        End Function
    End Class
    Public Class SideMenuGroup
        Inherits SideMenu

        Property Items As List(Of SideMenu)
        Public Sub New()
            Items = New List(Of SideMenu)
        End Sub
        Public Sub Add(item As SideMenu)
            Items.Add(item)
        End Sub
        Overrides Function Render(ByVal ActiveLink As String) As String

            Dim activeClass = String.Empty

            Dim found = Not Items.Where(Function(m) m.ActionLink = ActiveLink).SingleOrDefault() Is Nothing
            Dim display As String = "display:none"
            If found Then
                activeClass = cACTIVECLASS
                display = "display:block"
            End If
            Dim rvalue As New StringBuilder
            rvalue.Append("<li class='dropdown'>")
            rvalue.AppendFormat("<a href='#' class='dropdown-toggle{0}'>", activeClass)

            If Not String.IsNullOrEmpty(Icon) Then
                rvalue.AppendFormat("<i class='fa {0}'></i> ", Icon)
            End If
            rvalue.AppendFormat("<span class='hidden-xs'>{0}</span>", Label)
            rvalue.Append("</a>")
            rvalue.AppendLine()
            rvalue.AppendFormat("<ul class='dropdown-menu' style='{0}'>", display)
            For Each item In Items
                rvalue.Append(item.Render(ActiveLink))
            Next
            rvalue.AppendLine()
            rvalue.AppendLine("</ul>")
            Return rvalue.ToString()
        End Function
    End Class
    Public Class SideMenuBuilder
        Public Shared Function RenderMenu(ByVal activeLink As String, modulename As String, user As System.Security.Principal.IPrincipal) As String
            Dim rvalue As New List(Of ISideMenu)
            Dim menuEntities = New MainEntities
            Dim menus = menuEntities.SideBarMenus.ToList()
            Dim rootMenus = menus.Where(Function(m) m.ParentId = 0 AndAlso m.Module.Equals(modulename, StringComparison.OrdinalIgnoreCase)).OrderBy(Function(m) m.Ordinal)

            For Each item In rootMenus
                If item.ActionLink = "#" Then
                    Dim gItem = New SideMenuGroup With {.ActionLink = item.ActionLink,
                                                         .Icon = item.Icon,
                                                         .Label = item.Label,
                                                        .Id = item.ID,
                                                        .ParentId = item.ParentId}



                    gItem.Items = BuildSubMenu(menus, gItem.Id, user)
                    If Not gItem.Items.Count = 0 Then
                        rvalue.Add(gItem)
                    End If

                Else
                    Dim s = item.SideBarRoles.Select(Of String)(Function(m) m.RoleName).ToArray()
                    Dim allow As Boolean = False
                    If s.Count > 0 Then
                        For Each rolename In s
                            If user.IsInRole(rolename) Then
                                allow = True
                                Exit For
                            End If
                        Next
                    Else
                        allow = True
                    End If

                    If allow Then
                        rvalue.Add(New SideMenuLink With {.ActionLink = item.ActionLink, .Icon = item.Icon,
                                                      .Label = item.Label, .Id = item.ID, .ParentId = item.ParentId})
                    End If

                End If
            Next

            Dim sb As New StringBuilder

            For Each item In rvalue
                sb.AppendLine(item.Render(activeLink))
            Next
            Return sb.ToString
        End Function
        Private Shared Function BuildSubMenu(ByVal items As List(Of SideBarMenu),
                                             ByVal parentId As Integer,
                                              user As System.Security.Principal.IPrincipal) As List(Of SideMenu)
            Dim rvalue As New List(Of SideMenu)
            Dim theItems = items.Where(Function(m) m.ParentId = parentId).OrderBy(Function(m) m.Ordinal)
            For Each item In theItems
                If item.ActionLink = "#" Then
                    Dim gItem = New SideMenuGroup With {.ActionLink = item.ActionLink,
                                                         .Icon = item.Icon,
                                                         .Label = item.Label,
                                                        .Id = item.ID,
                                                        .ParentId = item.ParentId}



                    gItem.Items = BuildSubMenu(items, gItem.Id, user)
                    If Not gItem.Items.Count = 0 Then
                        rvalue.Add(gItem)
                    End If

                Else
                    Dim s = item.SideBarRoles.Select(Of String)(Function(m) m.RoleName).ToArray()
                    Dim allow As Boolean = False
                    If s.Count > 0 Then
                        For Each rolename In s
                            If user.IsInRole(rolename) Then
                                allow = True
                                Exit For
                            End If
                        Next
                    Else
                        allow = True
                    End If

                    If allow Then
                        rvalue.Add(New SideMenuLink With {.ActionLink = item.ActionLink, .Icon = item.Icon,
                                                      .Label = item.Label, .Id = item.ID, .ParentId = item.ParentId})
                    End If
                End If
            Next
            Return rvalue
        End Function
    End Class

    Public Class SideBarMenuHierarchy
        Inherits SideBarMenu
        Property Children As List(Of SideBarMenuHierarchy)
    End Class

End Namespace