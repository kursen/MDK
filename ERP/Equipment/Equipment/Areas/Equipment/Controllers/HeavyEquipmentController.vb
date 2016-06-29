Namespace Equipment.Areas.Equipment.Controllers
    Public Class HeavyEquipmentController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Equipment/HeavyEquipment
        Dim ctx As New EquipmentEntities
        Function Index() As ActionResult

            Dim theTypes = ctx.ExecuteStoreQuery(Of String)("EXEC [dbo].[GetHeavyEquipmentTypes]")
            Dim optTypes = (From t In theTypes
                    Select New With {.value = t, .text = t}).ToList()

            optTypes.Insert(0, New With {.value = "", .text = ""})
            ViewData("filterAlatBerat") = New SelectList(optTypes, "value", "text")


            Return View()
        End Function

        Function Create() As ActionResult
            Dim m As New HeavyEqp
            m.BuiltYear = 1900
            m.Year = 1900
            m.Cost = 0D
         
            Return View(m)
        End Function

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = ctx.HeavyEqps.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data alat berat tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "DueDate"
                    If String.IsNullOrEmpty(value) Then
                        model.GetType.GetProperty(name).SetValue(model, Nothing, Nothing)
                    Else
                        Dim d As Date
                        Date.TryParse(value, d)
                        model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                    End If
                    
                Case "Cost"
                    Dim d As Decimal
                    Decimal.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "Year", "Weight", "IDArea"
                    Dim d As Integer
                    Integer.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "BuiltYear"
                    If String.IsNullOrWhiteSpace(value) Then
                        model.GetType.GetProperty(name).SetValue(model, Nothing, Nothing)
                    Else
                        Dim d As Integer
                        Integer.TryParse(value, d)
                        model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                    End If
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select

            ctx.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()> _
        Function SaveImage(ByVal form As FormCollection) As JsonResult
            Dim stat As Integer = 0
            Dim Message As String = ""

            Try



                Dim ID As Integer = form("ID")
                If (form.Item("deleteimage") = "on") Then
                    ctx.ExecuteStoreQuery(Of ERPBase.OptionItem)("update Eqp.HeavyEqp set Img = null where ID = @ID",
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
                    ctx.ExecuteStoreQuery(Of ERPBase.OptionItem)("update Eqp.HeavyEqp set Img = @InputImg where ID = @ID",
                                                                 New SqlClient.SqlParameter("@InputImg", inputImg), New SqlClient.SqlParameter("@ID", ID))
                Else


                End If
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                ModelState.AddModelError("General", ex.Message)
            End Try
            Return Json(New With {.stat = stat, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function HeavyEquipmentTypes(ByVal query As String) As ActionResult
            If String.IsNullOrWhiteSpace(query) Then query = ""
            query = query.ToUpper()
            Dim theTypes = ctx.ExecuteStoreQuery(Of String)("EXEC [dbo].[GetHeavyEquipmentTypes]")
            Dim a = New List(Of ERPBase.OptionItem)
            Dim filter = theTypes.OfType(Of String)().Where(Function(m) m.ToUpper().Contains(query))
            If filter IsNot Nothing Then
                For Each item In filter
                    a.Add(New ERPBase.OptionItem() With {.Text = item, .Value = item})
                Next
            End If

            Return Json(a, JsonRequestBehavior.AllowGet)
        End Function

        Function MerkHeavyEquipmentTypes(ByVal query As String) As ActionResult
            If String.IsNullOrWhiteSpace(query) Then query = ""
            query = query.ToUpper()
            Dim theTypes = ctx.ExecuteStoreQuery(Of String)("EXEC [dbo].[GetHeavyEquipmentMerkTypes]")
            Dim a = New List(Of ERPBase.OptionItem)
            Dim filter = theTypes.OfType(Of String)().Where(Function(m) m.ToUpper().Contains(query))
            If filter IsNot Nothing Then
                For Each item In filter
                    a.Add(New ERPBase.OptionItem() With {.Text = item, .Value = item})
                Next
            End If

            Return Json(a, JsonRequestBehavior.AllowGet)
        End Function

        Function getData() As JsonResult
            Dim model = (From e In ctx.HeavyEqps Select New With {e.ID, e.Species, e.Code, e.Year, e.Merk, e.Type, e.Cost}).ToList
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()> _
        Function Create(ByVal model As HeavyEqp) As ActionResult

            If ModelState.IsValid Then
                Try
                    ctx.HeavyEqps.AddObject(model)
                    If model.ID > 0 Then
                        ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
                    End If
                    ctx.SaveChanges()
                    Return Json(New With {.id = model.ID, .stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try

            End If
            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            'Dim model = ctx.HeavyEqps.Where(Function(m) m.ID = id).FirstOrDefault()

            Dim query As String = <sql>
				SELECT TOP 1
                  ID
                  ,IDArea
                  ,Code
                  ,Merk
                  ,Species
                  ,Type
                  ,Cost
                  ,Year
                  ,TaxNumber
                  ,DueDate
                  ,SerialNumber
                  ,Capacity
                  ,Remarks
                  ,Img
                  ,BuiltYear
                  ,IDOpr
                  ,OprName
	              ,o.Name as OfficeName
	              ,o.OfficeId
              FROM eqp.Getofficelist() o
	               RIGHT JOIN Eqp.HeavyEqp h
			               ON o.officeid = h.idarea
              WHERE h.ID=@ID
                                  </sql>.Value


            Dim HeavyEquipments = ctx.ExecuteStoreQuery(Of HeavyEqpView)(query,
                                                          New SqlClient.SqlParameter("@ID", id))



            Dim model As HeavyEqpView = HeavyEquipments.FirstOrDefault()
            Return View(model)
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.HeavyEqps Where pl.ID = id).FirstOrDefault()
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


        <HttpGet()>
        Function HeavyEqpPicture(ByVal Id As Integer) As ActionResult

            Dim model = ctx.HeavyEqps.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing OrElse model.Img Is Nothing OrElse model.Img.Length = 0 Then
                Return File(Server.MapPath("~/Areas/Equipment/images/noimage.gif"), "image/gif")
            End If
            Return File(model.Img, "image/png")
        End Function

        <HttpPost()>
        Public Function GetOfficeList() As ActionResult
            Dim _mainEntities As New ERPBase.MainEntities
            Dim officeList = From m In _mainEntities.Offices
                             Where m.Parent_ID = 0
                             Select New With {.value = m.Id, .text = m.Name}


            Return Json(officeList)
        End Function
    End Class
End Namespace