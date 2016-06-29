Namespace Equipment.Areas.Equipment.Controllers
    Public Class VehiclesController
        Inherits System.Web.Mvc.Controller
        Dim ctx As New EquipmentEntities
        '
        ' GET: /Equipment/Vehicles

        Function Index() As ActionResult
            Dim theTypes = ctx.ExecuteStoreQuery(Of String)("EXEC [dbo].[GetVehiclesTypes]")
            Dim optTypes = (From t In theTypes
                    Select New With {.value = t, .text = t}).ToList()

            optTypes.Insert(0, New With {.value = "", .text = ""})
            ViewData("filterKendaraan") = New SelectList(optTypes, "value", "text")
            Return View()
        End Function

        Function getData() As JsonResult
            Dim model = From m In ctx.Vehicles
                       Select New With {m.ID, m.PoliceNumber, m.Code, m.Merk, m.Type, m.Cost, m.Year, m.DueDate, m.Species}
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model = ctx.Vehicles.Where(Function(m) m.ID = id).FirstOrDefault
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim officeId = model.IDArea
            Dim _mainEntities = New ERPBase.MainEntities
            Dim office = (From m In _mainEntities.Offices
                         Where m.Id = officeId
                         Select m.Name).FirstOrDefault

            ViewData("office") = office

            Return View(model)
        End Function

        Function Create() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Public Function GetOfficeList() As ActionResult
            Dim _mainEntities As New ERPBase.MainEntities
            Dim officeList = From m In _mainEntities.Offices
                             Where m.Parent_ID = 0
                             Select New With {.value = m.Id, .text = m.Name}

            Return Json(officeList)
        End Function
        '
        <HttpGet()>
        Function VehiclePicture(ByVal Id As Integer) As ActionResult

            Dim model = ctx.Vehicles.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing OrElse model.Img Is Nothing OrElse model.Img.Length = 0 Then
                Return File(Server.MapPath("~/Areas/Equipment/images/noimage.gif"), "image/gif")
            End If
            Return File(model.Img, "image/png")
        End Function

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = ctx.Vehicles.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data project tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "KeurDueDate", "DueDate"
                    Dim d As Date
                    Date.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "Cost"
                    Dim d As Decimal
                    Decimal.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "PoliceNumber"
                    Dim theNumber As Integer
                    If Integer.TryParse(Request.Form("value[bk2]"), theNumber) = False Then
                        ModelState.AddModelError("General", "Format No.Pol tidak benar")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If

                    Dim pnumber = String.Format("{0} {1} {2}", Request.Form("value[bk1]").ToUpper(), Request.Form("value[bk2]"), Request.Form("value[bk3]").ToUpper())
                    model.GetType.GetProperty(name).SetValue(model, pnumber, Nothing)
                Case "IDArea"
                    Dim i As Integer
                    If Integer.TryParse(value, i) = False Then
                        ModelState.AddModelError("General", "Wilayah Kerja harus dipilih.")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If
                    model.GetType.GetProperty(name).SetValue(model, i, Nothing)
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select

            ctx.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()> _
        Function Create(ByVal model As Vehicle, ByVal form As FormCollection) As ActionResult
            If ModelState.IsValid Then
                Try
                    ctx.Vehicles.AddObject(model)
                    If model.ID > 0 Then
                        ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
                    End If
                    ctx.SaveChanges()
                    Return Json(New With {.id = model.ID, .stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpPost()> _
        Function SaveImage(ByVal form As FormCollection) As JsonResult
            Dim stat As Integer = 0
            Dim Message As String = ""

            Try
                Dim ID As Integer = form("ID")
                If (form.Item("deleteimage") = "on") Then
                    ctx.ExecuteStoreQuery(Of ERPBase.OptionItem)("update Eqp.Vehicle set Img = null where ID = @ID",
                                                                                    New SqlClient.SqlParameter("@ID", ID))

                    Return Json(New With {.stat = 1})
                End If
                Dim contenttypes = {"gif", "png", "jpg", "jpeg"}
                Dim image = Request.Files("Img")
                If contenttypes.Any(Function(m) image.ContentType.Contains(m)) = False Then
                    ModelState.AddModelError("General", "File yang diupload bukan file gambar.")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
                Dim imglength = image.ContentLength
                If imglength > 0 Then
                    Dim inputImg(imglength) As Byte
                    image.InputStream.Read(inputImg, 0, imglength)
                    ctx.ExecuteStoreQuery(Of ERPBase.OptionItem)("update Eqp.Vehicle set Img = @InputImg where ID = @ID",
                                                                 New SqlClient.SqlParameter("@InputImg", inputImg), New SqlClient.SqlParameter("@ID", ID))
                Else


                End If
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                ModelState.AddModelError("General", ex.Message)
            End Try
            Return Json(New With {.stat = stat, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        
        '
        Function VehiclesTypes(ByVal query As String) As ActionResult
            If String.IsNullOrWhiteSpace(query) Then query = ""
            query = query.ToUpper()
            Dim theTypes = ctx.ExecuteStoreQuery(Of String)("EXEC [dbo].[GetVehicleTypes]")
            Dim a = New List(Of ERPBase.OptionItem)
            Dim filter = theTypes.OfType(Of String)().Where(Function(m) m.ToUpper().Contains(query))
            If filter IsNot Nothing Then
                For Each item In filter
                    a.Add(New ERPBase.OptionItem() With {.Text = item, .Value = item})
                Next
            End If

            Return Json(a, JsonRequestBehavior.AllowGet)
        End Function

        Function MerkVehiclesTypes(ByVal query As String) As ActionResult
            If String.IsNullOrWhiteSpace(query) Then query = ""
            query = query.ToUpper()
            Dim theTypes = ctx.ExecuteStoreQuery(Of String)("EXEC [dbo].[GetVehicleMerkTypes]")
            Dim a = New List(Of ERPBase.OptionItem)
            Dim filter = theTypes.OfType(Of String)().Where(Function(m) m.ToUpper().Contains(query))
            If filter IsNot Nothing Then
                For Each item In filter
                    a.Add(New ERPBase.OptionItem() With {.Text = item, .Value = item})
                Next
            End If

            Return Json(a, JsonRequestBehavior.AllowGet)
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.Vehicles Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Got Exception"
                msgDesc = ex.InnerException.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace