Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class VendorCategoryController
        Inherits System.Web.Mvc.Controller
        Dim ctx As New PurchasingEntities

        '
        ' GET: /Purchasing/VendorCategory

        Function Index() As ActionResult
            Return View()
        End Function

        'untuk medapatkan list Item
        <HttpPost()>
        Public Function GetCategoryItems() As JsonResult
            Dim models = From m In ctx.VendorCategory
                         Select New With {.Id = m.Id, .Name = m.Name}

            Return Json(New With {.data = models})
        End Function

        <HttpPost()>
        Function ValidateItemCategory(ByVal model As VendorCategory, ByVal itemCategoryRowIdx As Integer) As JsonResult
            If String.IsNullOrWhiteSpace(model.Name) Then
                ModelState.Clear()
                ModelState.AddModelError("ItemToMaintainName", "Item tidak boleh kosong")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Dim rvalue = New With {.id = model.Id, .name = model.Name}
            Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = itemCategoryRowIdx})
        End Function

        Function Save(ByVal sVendorCategoryItem As String) As ActionResult

            If ModelState.IsValid Then
                Dim thisVendorCategoryCheck = (From m In ctx.VendorCategory)
                Dim VendorCategoryItems = New List(Of VendorCategory)

                Newtonsoft.Json.JsonConvert.PopulateObject(sVendorCategoryItem, VendorCategoryItems)
                If VendorCategoryItems.Count > 0 Then
                    For Each item In VendorCategoryItems
                        If item.Id = 0 Then
                            If thisVendorCategoryCheck IsNot Nothing Then
                                ''find the name first
                                Dim theName = item.Name
                                Dim found = (From m In thisVendorCategoryCheck
                                            Where m.Name.Equals(theName, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault()
                                If found Is Nothing Then
                                    Dim newCheckItem = New VendorCategory
                                    newCheckItem.Name = item.Name
                                    ctx.VendorCategory.AddObject(newCheckItem)
                                    ctx.SaveChanges()
                                    item.Id = newCheckItem.Id
                                Else
                                    item.Id = found.Id
                                End If
                            End If
                        End If
                    Next
                Else
                    ModelState.AddModelError("General", "Item kategori vendor harus diisi")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If

                Return Json(New With {.stat = 1, .ListItem = VendorCategoryItems})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        <HttpPost()>
        Function DeleteItemCategory(ByVal Id As Integer) As JsonResult
            Dim model = ctx.VendorCategory.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model IsNot Nothing Then
                Try
                    ctx.DeleteObject(model)
                    ctx.SaveChanges()
                Catch ex As Exception
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
                End Try

            End If
            Return Json(New With {.stat = 1})
        End Function
    End Class
End Namespace
