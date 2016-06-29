Namespace Equipment.Areas.Equipment.Controllers
    Public Class VehicleAreaController
        Inherits System.Web.Mvc.Controller

        Private eqp_entities As EquipmentEntities
        '
        ' GET: /Equipment/VehicleArea

        Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Function GetHeavyEquipmentList() As ActionResult
            Dim query As String = <sql>
				SELECT h.ID,
					   h.Code,
					   h.Merk +'/'+ h.[Type] BrandType,
					   h.species as category,
					   o.Name as OfficeName,
					   o.officeId,
                       h.IDOpr,
                       h.OprName
				FROM   eqp.Getofficelist( ) o
					   RIGHT JOIN eqp.HEAVYEQP h
							   ON o.officeid = h.idarea 
                                  </sql>.Value


            Dim HeavyEquipments = eqp_entities.ExecuteStoreQuery(Of VehicleLocationView)(query)

            Dim queryVehicle As String = <sql>
                 SELECT h.ID,
					   h.Code,
					   h.Merk +'/'+ h.[Type] BrandType,
					   h.species as category,
                       h.PoliceNumber,
					   o.Name as OfficeName,
					   o.officeId,
                        h.IDDriver as IDOpr,
                        h.DriverName as OprName
				FROM   eqp.Getofficelist( ) o
					   RIGHT JOIN eqp.Vehicle h
							   ON o.officeid = h.idarea 

                                         </sql>.Value

            Dim Vehicles = eqp_entities.ExecuteStoreQuery(Of VehicleLocationView)(queryVehicle)
            Dim ent2 = New ERPBase.MainEntities
            Dim officelist = (From m In ent2.Offices
                             Where m.Parent_ID = 0
                             Select New With {.value = m.Id, .text = m.Name, .key = m.Code}).ToList()

            Return Json(New With {.heavyequipment = HeavyEquipments, .vehicles = Vehicles, .officelist = officelist})
        End Function

        <HttpPost()>
        Function SaveHeavyEquipmentToNewLocation(ByVal pk As Integer, value As Integer) As ActionResult
            Dim query = <sql>
                            UPDATE eqp.HEAVYEQP
                            SET idarea = @idarea
                            WHERE ID=@ID

                        </sql>.Value()

            Dim result = eqp_entities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@idarea", value),
                                                          New SqlClient.SqlParameter("@ID", pk))

            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Function SaveVehicleToNewLocation(ByVal pk As Integer, value As Integer) As ActionResult
            Dim query = <sql>
                            UPDATE eqp.Vehicle
                            SET idarea = @idarea
                            WHERE ID=@ID

                        </sql>.Value()

            Dim result = eqp_entities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@idarea", value),
                                                          New SqlClient.SqlParameter("@ID", pk))

            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Function SaveDefaultOperator(ByVal pk As Integer, value As Integer?) As ActionResult
            If value.HasValue Then
                Dim query = <sql>
                            UPDATE e
                            SET    e.idopr = t.id,
                                   e.oprname = t.fullname
                            FROM   eqp.HEAVYEQP e
                                   , eqp.Getemployees( ) t
                            WHERE  t.id = @idopr AND e.id=@ID



                        </sql>.Value()

                Dim result = eqp_entities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@idopr", value.Value),
                                                              New SqlClient.SqlParameter("@ID", pk))
            Else
                ''set it to null

                Dim query = <sql>
                            UPDATE eqp.HEAVYEQP
                            SET    idopr =null,
                                   oprname = null
                            WHERE  id=@ID



                        </sql>.Value()

                Dim result = eqp_entities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@ID", pk))
            End If


            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Function GetOperators(ByVal OfficeId As Integer, ByVal query As String) As ActionResult

            Dim a = eqp_entities.ExecuteStoreQuery(Of ERPBase.OptionItem) _
                    ("SELECT CAST(ID AS VARCHAR(MAX)) AS Value, Fullname as Text FROM EQP.GetOperators(@officeid, @term)",
                                                                          New SqlClient.SqlParameter("@OfficeId", OfficeId),
                                                                          New SqlClient.SqlParameter("@term", query))

            Return Json(a)
        End Function

        <HttpPost()>
        Function SaveDefaultDriver(ByVal pk As Integer, value As Integer?) As ActionResult

            If value.HasValue Then
                Dim query = <sql>
                            UPDATE e
                            SET    e.iddriver = t.id,
                                   e.drivername = t.fullname
                            FROM   eqp.VEHICLE e
                                   , eqp.Getemployees( ) t
                            WHERE  t.id = @idopr AND e.id=@ID
                        </sql>.Value()
                Dim result = eqp_entities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@idopr", value.Value),
                                                       New SqlClient.SqlParameter("@ID", pk))
            Else
                Dim query = <sql>
                            UPDATE eqp.VEHICLE
                            SET    iddriver =null,
                                   drivername = null
                            WHERE  id=@ID
                        </sql>.Value()

                Dim result = eqp_entities.ExecuteStoreCommand(query, New SqlClient.SqlParameter("@ID", pk))
            End If

         
         

            Return Json(New With {.stat = 1})
        End Function
        <HttpPost()>
        Function GetDrivers(ByVal OfficeId As Integer, ByVal query As String) As ActionResult
            Dim a = eqp_entities.ExecuteStoreQuery(Of ERPBase.OptionItem) _
                    ("SELECT CAST(ID AS VARCHAR(MAX)) AS Value, Fullname as Text FROM EQP.GetDrivers(@officeid, @term)",
                                                                          New SqlClient.SqlParameter("@OfficeId", OfficeId),
                                                                          New SqlClient.SqlParameter("@term", query))
            Return Json(a)
        End Function

        Public Sub New()
            eqp_entities = New EquipmentEntities
        End Sub
    End Class
End Namespace
