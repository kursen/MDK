Namespace Controllers
    Public Class SystemSettingController
        Inherits System.Web.Mvc.Controller
        Private _MainEntities As MainEntities


#Region "system menu"



        '
        ' GET: /SystemSetting

        Function Index() As ActionResult
            Dim sqlQueryModulename As String = <sql>

                SELECT DISTINCT modulename as value , modulename as [text]
                FROM   dbo.ASPNET_ROLES 
                                           </sql>.Value

            Dim modulenames = _MainEntities.ExecuteStoreQuery(Of OptionItem)(sqlQueryModulename).ToList()
            modulenames.Insert(0, New OptionItem() With {.Text = "Home", .Value = "Home"})
            Dim selectList = New SelectList(modulenames, "value", "text")

            ViewData("ModuleNames") = selectList
            Dim model = New SideBarMenu
            model.ID = 0
            model.Label = "New Menu"
            model.Icon = "fa-dashboard"
            Return View(model)
        End Function


        <HttpPost()>
        Function GetSideMenus(ByVal Modulename As String) As ActionResult
            Dim sqlquery As String = <sql>
                                         SELECT * FROM SideBarMenu
                                         WHERE module=@modulename

                                     </sql>.Value
            Dim sideMenus = _MainEntities.ExecuteStoreQuery(Of MenuBuilder.SideBarMenuHierarchy)(sqlquery, New SqlClient.SqlParameter("@modulename", Modulename)).ToList()

            Dim hMenu = New List(Of MenuBuilder.SideBarMenuHierarchy)
            Dim Level1Menus = sideMenus.Where(Function(m) m.ParentId = 0).OrderBy(Function(m) m.Ordinal)
            For Each item In Level1Menus

                setChildren(item, sideMenus)
            Next

            Return Json(Level1Menus)
        End Function
        Private Sub setChildren(ByVal menuitem As MenuBuilder.SideBarMenuHierarchy, ByVal arrayData As List(Of MenuBuilder.SideBarMenuHierarchy))

            menuitem.Children = New List(Of MenuBuilder.SideBarMenuHierarchy)
            menuitem.Children.AddRange(arrayData.Where(Function(m) m.ParentId = menuitem.ID).OrderBy(Function(m) m.Ordinal))
            For Each item In menuitem.Children
                setChildren(item, arrayData)
            Next
        End Sub

        <HttpPost()>
        Public Function reOrder(ByVal obj As String) As ActionResult
            Dim result = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of MenuItemJson))(obj)

            _reorder(0, result)

            Return Json(New With {.stat = 1})

        End Function
        Private Function _reorder(ByVal pareintId As Integer, children As List(Of MenuItemJson)) As Boolean
            Dim ordinal As Integer = 0
            For Each item In children
                Dim itemId = item.id
                Dim query As String = "UPDATE SideBarMenu SET parentId = @parentId, Ordinal=@ordinal WHERE ID=@id"
                'has this item children?



                If (item.children IsNot Nothing) Then
                    query = "UPDATE SideBarMenu SET parentId = @parentId, Ordinal=@ordinal, ActionLink='#' WHERE ID=@id"
                    _reorder(itemId, item.children)
                End If

                _MainEntities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@parentId", pareintId),
                                                                New SqlClient.SqlParameter("@id", itemId),
                                                                New SqlClient.SqlParameter("@ordinal", ordinal))
                ordinal += 1
            Next
            Return True
        End Function

        <Authorize()>
        <HttpPost()>
        Public Function SaveMenu(ByVal theMenu As SideBarMenu) As ActionResult
            Dim menuType = Request.Form("menutype")

            _MainEntities.SideBarMenus.AddObject(theMenu)
            If theMenu.ID > 0 Then
                _MainEntities.ObjectStateManager.ChangeObjectState(theMenu, EntityState.Modified)
            End If
            _MainEntities.SaveChanges()


            Return Json(New With {.stat = 1})
        End Function

#End Region
        Public Sub New()
            _MainEntities = New MainEntities
        End Sub
    End Class
    Public Class MenuItemJson
        Property id As Integer
        Property children As List(Of MenuItemJson)
    End Class
End Namespace
