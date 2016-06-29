Imports System.Globalization
Namespace Equipment.Areas.Equipment.Controllers
    Public Class HomeController
        Inherits System.Web.Mvc.Controller
        Private _EqpEntities As EquipmentEntities
        Private currentUserProfile As ERPBase.ErpUserProfile

        Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Function CompletionUsageReports(Optional ByVal sundayDate As Date = Nothing) As ActionResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()
            If sundayDate = Nothing Then
                sundayDate = Date.Today.AddDays(Date.Today.DayOfWeek * -1)
            End If

            Dim VehicleList = _EqpEntities.ExecuteStoreQuery(Of SummaryCompletionItemReport)("EXEC [Eqp].[SummaryVehicleExistingReport] @WEEKDATESTART , @IDAREA",
                                                                                             New SqlClient.SqlParameter("@WEEKDATESTART", sundayDate),
                                                                                             New SqlClient.SqlParameter("@IDAREA", p.WorkUnitId))


            Dim HeavyEquipmentList = _EqpEntities.ExecuteStoreQuery(Of SummaryCompletionItemReport)("EXEC [Eqp].[SummaryHeavyEquipmentExistingReport] @WEEKDATESTART , @IDAREA",
                                                                                             New SqlClient.SqlParameter("@WEEKDATESTART", sundayDate),
                                                                                             New SqlClient.SqlParameter("@IDAREA", p.WorkUnitId))


            Dim TradoList = _EqpEntities.ExecuteStoreQuery(Of SummaryCompletionItemReport)("EXEC [Eqp].[SummaryTradoExistingReport] @WEEKDATESTART , @IDAREA",
                                                                                             New SqlClient.SqlParameter("@WEEKDATESTART", sundayDate),
                                                                                             New SqlClient.SqlParameter("@IDAREA", p.WorkUnitId))

            Return Json(New With {.thisWeekDayFirst = sundayDate.ToString("dd-MM-yyyy"),
                                  .thisWeekDayLast = sundayDate.AddDays(6).ToString("dd-MM-yyyy"),
                                  .prevWeekDay = sundayDate.AddDays(-7).ToString("dd-MM-yyyy"),
                                  .nextWeekDay = sundayDate.AddDays(7).ToString("dd-MM-yyyy"),
                                  VehicleList,
                                  HeavyEquipmentList,
                                  TradoList})
        End Function

        Public Function GetNumberOfVehicleNeedToMaintain() As ActionResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)

            Dim model = _EqpEntities.ExecuteStoreQuery(Of VehiclePeriodicMaintenanceListView)("EXEC eqp.GetScheduledVehicleMaintenancePlan  @startdate, @enddate, @areaid",
                                                                                              New SqlClient.SqlParameter("@startdate", startDate),
                                                                                              New SqlClient.SqlParameter("@enddate", enddate),
                                                                                              New SqlClient.SqlParameter("@areaid", currentUserProfile.WorkUnitId))

            Dim groupped = From m In model
                           Group m By m.VehicleId, m.Type, m.Code,
                           m.Species, m.PoliceNumber, m.Merk Into Vehicle = Group


            Return Json(New With {.NumberVehicleToMaintain = groupped.Count})
        End Function

        Public Function GetNumberOfHeavyEqpNeedToMaintain() As ActionResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)

            Dim model = _EqpEntities.ExecuteStoreQuery(Of HeavyEquipmentMaintenanceListView)("EXEC eqp.GetScheduledHeavyEqpMaintenancePlan  @startdate, @enddate, @areaid",
                                                                                              New SqlClient.SqlParameter("@startdate", startDate),
                                                                                              New SqlClient.SqlParameter("@enddate", enddate),
                                                                                              New SqlClient.SqlParameter("@areaid", currentUserProfile.WorkUnitId))

            Dim groupped = From m In model
                           Group m By m.HeavyEqpId, m.Type, m.Code,
                           m.Species, m.Merk Into HeavyEqp = Group


            Return Json(New With {.NumberHeavyEqpToMaintain = groupped.Count})
        End Function

        Public Function GetNumberOfMachineNeedToMaintain() As ActionResult

            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)

            Dim model = _EqpEntities.ExecuteStoreQuery(Of MachinePeriodicMaintenanceListView)("EXEC eqp.GetScheduledMachineMaintenancePlan  @startdate, @enddate, @areaid",
                                                                                              New SqlClient.SqlParameter("@startdate", startDate),
                                                                                              New SqlClient.SqlParameter("@enddate", enddate),
                                                                                              New SqlClient.SqlParameter("@areaid", currentUserProfile.WorkUnitId))

            Dim groupped = From m In model
                           Group m By m.MachineEqpId, m.Type, m.SerialNumber, m.Merk Into Machine = Group

            Return Json(New With {.NumberMachineToMaintain = groupped.Count})
        End Function

        Public Sub New()
            _EqpEntities = New EquipmentEntities



            currentUserProfile = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace
