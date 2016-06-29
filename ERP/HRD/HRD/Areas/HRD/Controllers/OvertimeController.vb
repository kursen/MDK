Namespace HRD.Areas.HRD.Controllers
    Public Class OvertimeController
        Inherits System.Web.Mvc.Controller
        Private _hrdEntities As HrdEntities
        '
        ' GET: /HRD/Overtime

        Private cUserProfile As ERPBase.ErpUserProfile

        Function Index() As ActionResult
            Return View()
        End Function


        Public Function GetOvertimeList(ByVal overtimedate As Date) As JsonResult
            Dim sql = <sql>
                          SELECT ov.*,
                                   mp.fullname AS EmployeeName
                            FROM   [HRD].[employeeovertime] ov
                                   INNER JOIN hrd.master_personal mp
                                           ON ov.employeeid = mp.id
                            WHERE  ov.activitydate = @overtimedate
                                   AND ov.officeid = @officeId
                      </sql>.Value()
            Dim model = _hrdEntities.ExecuteStoreQuery(Of EmployeeOvertimeView)(sql, New SqlClient.SqlParameter("@overtimedate", overtimedate),
                                                                                New SqlClient.SqlParameter("@OfficeId", cUserProfile.WorkUnitId))

            Return Json(New With {.data = model})
        End Function

        Public Function Create() As ActionResult
            Dim model As New EmployeeOvertime
            model.ActivityDate = Date.Today
            ViewData("Fullname") = ""
            ViewData("OccupationName") = ""
            Return View("OvertimeForm", model)
        End Function
        Public Function Edit(ByVal Id As Integer) As ActionResult
            Dim model = _hrdEntities.EmployeeOvertimes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim employee = (From m In _hrdEntities.Master_Personal
                           Where m.ID = model.EmployeeId
                           Select New With {m.FullName, .OccupationName = m.Occupation.Name}).SingleOrDefault()
            If employee IsNot Nothing Then
                ViewData("Fullname") = employee.FullName
                ViewData("OccupationName") = employee.OccupationName
            End If
            Return View("OvertimeForm", model)
        End Function

        Public Function SaveOvertime(ByVal model As EmployeeOvertime) As JsonResult
            If ModelState.IsValid Then
                Dim overtimeOk As Boolean = False

                If model.BeginOvertime1Afternoon.HasValue AndAlso model.EndOvertime1Afternoon.HasValue Then
                    If model.EndOvertime1Afternoon.Value.Hour = 0 Then
                        model.EndOvertime1Afternoon = model.ActivityDate.AddDays(1)
                    Else
                        model.EndOvertime1Afternoon = model.ActivityDate.AddHours(model.EndOvertime1Afternoon.Value.Hour).AddMinutes(model.EndOvertime1Afternoon.Value.Minute)
                    End If
                    model.BeginOvertime1Afternoon = model.ActivityDate.AddHours(model.BeginOvertime1Afternoon.Value.Hour).AddMinutes(model.BeginOvertime1Afternoon.Value.Minute)

                    If Date.Compare(model.BeginOvertime1Afternoon.Value, model.EndOvertime1Afternoon.Value) > 0 Then
                        ModelState.AddModelError("BeginOvertime1Afternoon", "Jam masuk Lembur 1 Sore harus lebih dulu dari jam keluar")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    Else
                        overtimeOk = True
                    End If

                End If
                If model.BeginOvertime1Morning.HasValue AndAlso model.EndOvertime1Morning.HasValue Then
                    model.BeginOvertime1Morning = model.ActivityDate.AddHours(model.BeginOvertime1Morning.Value.Hour).AddMinutes(model.BeginOvertime1Morning.Value.Minute)
                    model.EndOvertime1Morning = model.ActivityDate.AddHours(model.EndOvertime1Morning.Value.Hour).AddMinutes(model.EndOvertime1Morning.Value.Minute)
                    If Date.Compare(model.BeginOvertime1Morning.Value, model.EndOvertime1Morning.Value) > 0 Then
                        ModelState.AddModelError("BeginOvertime1Morning", "Jam masuk Lembur 1 Pagi harus lebih dulu dari jam keluar")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    Else
                        overtimeOk = True
                    End If
                    
                End If
                If model.BeginOvertime2.HasValue AndAlso model.EndOvertime2.HasValue Then

                    model.BeginOvertime2 = model.ActivityDate.AddHours(model.BeginOvertime2.Value.Hour).AddMinutes(model.BeginOvertime2.Value.Minute)
                    model.EndOvertime2 = model.ActivityDate.AddHours(model.EndOvertime2.Value.Hour).AddMinutes(model.EndOvertime2.Value.Minute)
                    If Date.Compare(model.BeginOvertime2.Value, model.EndOvertime2.Value) > 0 Then
                        ModelState.AddModelError("BeginOvertime2", "Jam masuk Lembur 2  harus lebih dulu dari jam keluar")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    Else
                        overtimeOk = True
                    End If
                End If
                If overtimeOk Then
                    _hrdEntities.EmployeeOvertimes.AddObject(model)
                    If model.Id > 0 Then
                        _hrdEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                    End If
                    _hrdEntities.SaveChanges()
                    Return Json(New With {.stat = 1, model, .bg1 = model.BeginOvertime1Afternoon.ToString()})
                Else
                    ModelState.AddModelError("General", "data lembur belum dimasukkan")
                End If

            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpPost()>
        Public Function Delete(ByVal Id As Integer) As JsonResult
            Dim model = _hrdEntities.EmployeeOvertimes.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model IsNot Nothing Then
                _hrdEntities.DeleteObject(model)
                _hrdEntities.SaveChanges()
            End If
            Return Json(New With {.stat = 1})
        End Function

        Public Sub New()
            _hrdEntities = New HrdEntities
            cUserProfile = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace
