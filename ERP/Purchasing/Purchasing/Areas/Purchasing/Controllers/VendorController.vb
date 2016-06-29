Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class VendorController
        Inherits System.Web.Mvc.Controller
        Dim ctx As New PurchasingEntities

        Function Index() As ActionResult
            Return View()
        End Function

        Function getList() As JsonResult
            Dim query = <sql>
                            SELECT *
                            FROM prc.Vendor v
                        </sql>.Value()
            Dim VendorItem = ctx.ExecuteStoreQuery(Of Vendor)(query)
            Dim model As List(Of Vendor) = VendorItem.ToList()

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Create() As ActionResult
            Dim ctx As New PurchasingEntities
            Dim model As New Vendor

            ' for category
            Dim category As New List(Of VendorCategory)
            category.Add(New VendorCategory With {.Id = 0, .Name = "-- Pilih --"})
            category.AddRange(ctx.VendorCategory.ToList())
            ViewData("CategoryId") = New SelectList(category, "Id", "Name", 0)

            ' for suspended status
            Dim Suspended As New Dictionary(Of Boolean, String)
            Suspended.Add(False, "Active")
            Suspended.Add(True, "Non Active")
            ViewData("Suspended") = New SelectList(Suspended, "Key", "Value", model.Suspended)

            ' for Vendor Number
            Dim QuoNo = (From a In ctx.Vendor
                         Select a.Number.Substring(2, 5)).Max()
            Dim No As Integer
            Dim Number As String
            If QuoNo Is Nothing Then
                No = 1
                Number = String.Format("{0:00000}", CDbl(No))
            Else
                No = CInt(QuoNo)
                Number = String.Format("{0:00000}", (CDbl(No) + 1))
            End If

            ViewData("QuoNumber") = "V" + Number

            Return View()
        End Function

        <HttpPost()>
        Function Create(ByVal model As Vendor) As ActionResult
            If ModelState.IsValid Then
                Try
                    Dim countVN As Integer = (From v In ctx.Vendor Where v.Number.ToUpper() = model.Number.ToUpper()).Count()

                    If countVN > 0 Then
                        ModelState.AddModelError("moreThanOnce", "Nomor Vendor telah digunakan")
                        Exit Try
                    End If

                    If Not IsNothing(model.Currency) Then model.Currency = model.Currency.ToUpper()
                    If model.Id = 0 Then model.Id = Nothing
                    ctx.Vendor.AddObject(model)
                    If model.Id <> 0 Then
                        ctx.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                    End If
                    ctx.SaveChanges()
                    If Not IsNothing(model.Id) Then
                        Return Json(New With {.id = model.Id, .stat = 1})
                    Else
                        Return Json(New With {.stat = 2})
                    End If
                Catch ex As Exception
                    ModelState.AddModelError("Error", ex.Message)
                End Try
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model As New Vendor
            model = (From a In ctx.Vendor
                         Where a.Id = id
                         Select a).FirstOrDefault()

            Dim VendorCategory = (From m In ctx.VendorCategory
                         Where m.Id = model.CategoryId
                         Select m.Name).FirstOrDefault()
            ViewData("CategoryId") = VendorCategory
            Return View(model)
        End Function

        <HttpPost()>
        Public Function GetVendorCategoryList() As ActionResult
            Dim VCList = From m In ctx.VendorCategory
                             Select New With {.value = m.Id, .text = m.Name}
            Return Json(VCList)
        End Function

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = ctx.Vendor.Where(Function(m) m.Id = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            'Select Case name
            '    Case "BalanceTax"
            '        Dim d As Double
            '        Decimal.TryParse(value, d)
            '        model.GetType.GetProperty(name).SetValue(model, d, Nothing)
            '    Case Else
            '        model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            'End Select
            model.GetType.GetProperty(name).SetValue(model, value, Nothing)

            ctx.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Function Delete(ByVal id As Integer) As ActionResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim model = (From a In ctx.Vendor Where a.Id = id).FirstOrDefault()
                If model IsNot Nothing Then ctx.Vendor.DeleteObject(model)
                ctx.SaveChanges()
                stat = 1
                msg = "Hapus data berhasil"
            Catch ex As Exception
                msgDesc = ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace
