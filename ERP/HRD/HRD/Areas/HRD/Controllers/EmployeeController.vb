Imports System.Drawing.Drawing2D

Namespace HRD.Areas.HRD.Controllers
    Public Class EmployeeController
        Inherits System.Web.Mvc.Controller
        Dim empEntities As HrdEntities
        Dim _mainEntities As ERPBase.MainEntities
        '
        ' GET: /HRD/Employee

        Function Index(Optional ByVal id As Integer = 1) As ActionResult
            Dim offices = (From o In _mainEntities.Offices
                           Order By o.Ordinal
                          Where o.Parent_ID = 0
                          Select New With {.Id = o.Id, .Name = o.Name}).ToList
            Dim selectList As New SelectList(offices, "Id", "Name", id)
            ViewData("OfficeList") = selectList
            Return View()
        End Function
        Function Detail(ByVal id As Integer) As ActionResult

            Dim model = empEntities.Master_Personal.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View(model)
        End Function

        Function EmployeeForm(Optional ByVal id As Integer = 0) As ActionResult
            Dim model = New Master_Personal
            model.OfficeId = id
            prepareEmployeeForm(model)
            Return View(model)
        End Function

        Function EditEmployee(ByVal id As Integer) As ActionResult
            Dim model = empEntities.Master_Personal.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            prepareEmployeeForm(model)
            Return View("EmployeeForm", model)
        End Function

        Private Sub prepareEmployeeForm(model As Master_Personal)
            Dim listOfOccupation = From o In empEntities.Occupations
                                   Order By o.Name
                                   Select o.Name, o.Id

            ViewData("OccupationId") = New SelectList(listOfOccupation, "Id", "Name", model.OccupationID)

            Dim listOffice = From o In _GetOfficeList() Where o.Id = model.OfficeId Order By o.Ordinal
                             Select o.Id, o.pathname

            ViewData("OfficeId") = New SelectList(listOffice, "Id", "pathname")
            Dim EmploymentStatus = empEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("SELECT Code AS Value, [Status] AS [Text] FROM HRD.EmployeeStatus ORDER BY Ordinal")
            ViewData("WorkerStatus") = New SelectList(EmploymentStatus, "Value", "Text", model.WorkerStatus)

            Dim Religions = empEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("SELECT Religion AS Value, Religion AS [Text] FROM HRD.Religion Order by Ordinal")
            ViewData("Religion") = New SelectList(Religions, "Value", "Text", model.Religion)

            Dim MaritalStatus = empEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("SELECT Code AS Value, [Status] AS [Text] " &
                                                                                     "FROM HRD.StatusMarital ORDER BY Ordinal")
            Dim LsGender As New List(Of ERPBase.OptionItem)
            LsGender.Add(New ERPBase.OptionItem With {.Text = "LAKI-LAKI", .Value = "LAKI-LAKI"})
            LsGender.Add(New ERPBase.OptionItem With {.Text = "PEREMPUAN", .Value = "PEREMPUAN"})

            ViewData("Gender") = New SelectList(LsGender, "Value", "Text", model.Gender)
            ViewData("MaritalStatus") = New SelectList(MaritalStatus, "Value", "Text", model.MaritalStatus)
        End Sub

        <HttpPost()>
        Public Function GetOfficeList() As ActionResult

            Return Json(_GetOfficeList())
        End Function

        Private Function _GetOfficeList() As List(Of ERPBase.OfficeHierarchyView)
            Dim arrOfficeId = From m In _mainEntities.Offices
                                         Where m.Parent_ID = 0
                                         Select m.Id



            Dim officeList As New List(Of ERPBase.OfficeHierarchyView)
            For Each item In arrOfficeId
                officeList.AddRange(ERPBase.OfficeHierarchyView.GetOfficeHierarchyView(item, _mainEntities))
            Next
            Return officeList
        End Function

        Function GetEmployeeList(ByVal OfficeId As Integer) As ActionResult
            Return Json(New With {.data = _GetEmployeeList(OfficeId)})
        End Function

        Private Function _GetEmployeeList(ByVal OfficeId As Integer) As EmployeeOfficeHierarchyView()




            Dim sql As String = <sql>
                            SELECT ovb.id                                           AS OfficeId,
                                   ovb.NAME                                         AS OfficeName,
                                   emp.id                                           AS Id,
                                   emp.fullname                                     AS Fullname,
                                   emp.EmployeeId                                   AS EmployeeId,
                                   emp.occupationname                               AS OccupationName,
                                   pathorder * 100 + Isnull( managementlevelid, 0 ) AS pathorder
                            FROM   ( SELECT mp.EmployeeId,
                                            mp.id,
                                            mp.fullname,
                                            mp.officeid,
                                            oc.NAME AS OccupationName,
                                            oc.managementlevelid
                                     FROM   hrd.MASTER_PERSONAL mp
                                            INNER JOIN hrd.OCCUPATION oc
                                                    ON mp.occupationid = oc.id ) AS emp
                                   RIGHT JOIN ( SELECT id,
                                                       NAME,
                                                       pathorder
                                                FROM   [dbo].[Officeviewbase] ( @officeId ) ) ovb
                                           ON ovb.id = emp.officeid
                            ORDER  BY pathorder 
                                </sql>.Value


            Dim employees = empEntities.ExecuteStoreQuery(Of EmployeeOfficeHierarchyView)(sql, New SqlClient.SqlParameter("@officeId", OfficeId)).ToArray
            Return employees
        End Function


        Function employeePhoto(ByVal Id As String) As ActionResult

            Dim fmodel = GetEmployeePhoto(Id)
            If fmodel IsNot Nothing AndAlso fmodel.Length > 0 Then
                Return File(fmodel, "image/jpg", "foto_" & Id & ".jpg")
            Else
                Return File(Server.MapPath("~/img/_default_male.png"), "image/jpg", "unknown.jpg")
            End If
        End Function

        <HttpGet()>
        Function EditPhoto(ByVal id As String) As ActionResult
            ViewData("id") = id
            Return View()
        End Function

        Private Function GetEmployeePhoto(ByVal Id As String) As Byte()
            Dim sql As String = "SELECT Photo FROM HRD.Master_personal WHERE id = @id"
            Dim p As New SqlClient.SqlParameter("@id", Id)
            Dim fmodel = empEntities.ExecuteStoreQuery(Of Byte())(sql, p).FirstOrDefault()
            Return fmodel
        End Function

        Public Function UploadPhoto() As ActionResult
            Dim f = Request.Files(0)
            If f IsNot Nothing Then

                Dim imgToSave As Drawing.Image
                Dim img = System.Drawing.Image.FromStream(f.InputStream)
                If img.Width > 500 Then
                    Dim ratio = 500 / img.Width
                    Dim newH = Convert.ToInt32(img.Height * ratio)
                    imgToSave = New Drawing.Bitmap(500, newH, img.PixelFormat)
                    Dim g = System.Drawing.Graphics.FromImage(imgToSave)
                    g.CompositingQuality = CompositingQuality.HighQuality
                    g.SmoothingMode = SmoothingMode.HighQuality
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.DrawImage(img, New Drawing.Rectangle(0, 0, 500, newH))
                    g.Flush()
                    g.Dispose()
                Else
                    imgToSave = CType(img.Clone(), Drawing.Image)
                End If
                Dim iomem As New IO.MemoryStream()
                imgToSave.Save(iomem, Drawing.Imaging.ImageFormat.Jpeg)


                Dim userId = Request.Params("userid")

                Dim foto() As Byte = iomem.ToArray()
                SavePhoto(foto, userId)
            End If

            Return Json(New With {.stat = 1})


        End Function

        Private Function SavePhoto(ByVal img As Byte(), ByVal id As String) As Integer
            Dim sql As String = "UPDATE HRD.Master_personal  SET  Photo =@photo WHERE id = @id"
            Dim p As New SqlClient.SqlParameter("@id", id)
            empEntities.ExecuteStoreCommand(sql, p, New SqlClient.SqlParameter("@photo", img))
            Return 1
        End Function
        <HttpPost()>
        Function CropPhoto(ByVal x As String, ByVal y As String, ByVal w As String, ByVal h As String, ByVal userid As String) As ActionResult
            Dim posx, posy, posw, posh As Double
            Dim ci As New System.Globalization.CultureInfo("en-GB")
            Double.TryParse(x, Globalization.NumberStyles.Any, ci, posx)
            Double.TryParse(y, Globalization.NumberStyles.Any, ci, posy)
            Double.TryParse(w, Globalization.NumberStyles.Any, ci, posw)
            Double.TryParse(h, Globalization.NumberStyles.Any, ci, posh)




            Dim arrimg As Byte() = GetEmployeePhoto(userid)

            If arrimg Is Nothing Then
                Return Json(New With {.stat = 0})
            End If


            Dim ioMem As New IO.MemoryStream(arrimg)
            Dim img = System.Drawing.Image.FromStream(ioMem)

            Dim newbitmap = New System.Drawing.Bitmap(150, 200, img.PixelFormat)

            Dim g = System.Drawing.Graphics.FromImage(newbitmap)
            g.CompositingQuality = CompositingQuality.HighQuality
            g.SmoothingMode = SmoothingMode.HighQuality
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            Dim src = New Drawing.Rectangle(CInt(posx), CInt(posy), CInt(posw), CInt(posh))
            Dim dest = New Drawing.Rectangle(0, 0, 150, 200)

            g.DrawImage(img, dest, src, Drawing.GraphicsUnit.Pixel)

            g.Flush()



            Dim foto() As Byte
            Dim ioMemnew As New IO.MemoryStream()
            newbitmap.Save(ioMemnew, Drawing.Imaging.ImageFormat.Jpeg)
            foto = ioMemnew.ToArray()
            SavePhoto(foto, userid)
            img.Dispose()
            g.Dispose()
            newbitmap.Dispose()

            Return RedirectToAction("EditPhoto", "Employee", New With {.id = userid})
        End Function


        Function SaveEmployee(ByVal model As Master_Personal) As ActionResult
            If ModelState.IsValid Then
                Try
                    empEntities.Master_Personal.AddObject(model)
                    If model.ID <> 0 Then
                        empEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                        Dim sql As String = "SELECT Photo FROM HRD.Master_personal WHERE id = @id"
                        Dim p As New SqlClient.SqlParameter("@id", model.ID)
                        Dim fmodel = empEntities.ExecuteStoreQuery(Of Byte())(sql, p).FirstOrDefault()
                        model.Photo = fmodel
                    End If
                    model.LastUpdate = Now
                    empEntities.SaveChanges()
                    Return Json(New With {.stat = 1, model.ID})
                Catch ex As Exception
                    Dim errMsg = ex.Message
                    If ex.InnerException IsNot Nothing Then
                        errMsg &= "<br/>" & ex.InnerException.Message
                    End If
                    ModelState.AddModelError("General", errMsg)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Public Sub New()
            empEntities = New HrdEntities
            _mainEntities = New ERPBase.MainEntities
        End Sub
    End Class
End Namespace
