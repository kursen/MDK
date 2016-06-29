Namespace HRD.Areas.HRD.Controllers
    Public Class OccupationController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /HRD/Occupation
        Dim hrd_Entities As HrdEntities
        Function Index() As ActionResult
            Dim model = From m In hrd_Entities.ManagementLevels
                        Order By m.Id
                        Select m.Id, m.LevelName

            ViewData("ManagementLevelId") = New SelectList(model, "Id", "LevelName")
            Return View()
        End Function

        <HttpPost()>
        Function GetOccupationList() As ActionResult
            Dim model = From m In hrd_Entities.Occupations
                        Order By m.Name
                        Select New With {m.Id, m.Name, .ManagementLevel = m.ManagementLevel.LevelName, .LevelId = m.ManagementLevelId}

            Return Json(New With {.data = model})
        End Function

        <HttpPost()>
        Public Function SaveOccupation(ByVal model As Occupation) As ActionResult
            If ModelState.IsValid Then

                hrd_Entities.Occupations.AddObject(model)
                If model.Id <> 0 Then
                    hrd_Entities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                hrd_Entities.SaveChanges()
                Return Json(New With {.stat = 1})

            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        <HttpPost()>
        Public Function DeleteOccupation(ByVal OccupationId As Integer) As ActionResult
            Dim model = hrd_Entities.Occupations.Where(Function(m) m.Id = OccupationId).SingleOrDefault()

            If model IsNot Nothing Then
                Dim NEmployee = model.Master_Personal.Count()
                If NEmployee > 0 Then
                    ModelState.AddModelError("General", "Jabatan tidak dapat dihapus karena masih ada pegawai dengan jabatan tersebut")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
            End If

            hrd_Entities.Occupations.DeleteObject(model)
            hrd_Entities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        Sub New()
            hrd_Entities = New HrdEntities
        End Sub
    End Class
End Namespace
