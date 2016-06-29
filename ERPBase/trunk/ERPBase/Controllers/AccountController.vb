
Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Web.Routing
Imports System.Drawing.Drawing2D

Namespace Controllers


    Public Class AccountController
        Inherits System.Web.Mvc.Controller
        Private _MainEntities As MainEntities
        '
        ' GET: /Account/LogOn

        Public Function LogOn() As ActionResult

            Return View()
        End Function

        '
        ' POST: /Account/LogOn

        <HttpPost()> _
        Public Function LogOn(ByVal model As LogOnModel, ByVal returnUrl As String) As ActionResult
            If ModelState.IsValid Then
                If Membership.ValidateUser(model.UserName, model.Password) Then
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe)
                    If Url.IsLocalUrl(returnUrl) AndAlso returnUrl.Length > 1 AndAlso returnUrl.StartsWith("/") _
                       AndAlso Not returnUrl.StartsWith("//") AndAlso Not returnUrl.StartsWith("/\\") Then
                        ' Return Redirect(returnUrl)
                        Return Json(New With {.stat = 1, .url = returnUrl})
                    Else
                        'Return RedirectToAction("Index", "Home")
                        Return Json(New With {.stat = 1, .url = "/"})
                    End If
                Else

                    Dim theuser = Membership.GetUser(model.UserName)
                    If theuser Is Nothing Then
                        ModelState.AddModelError("username", "Username atau password tidak benar.")
                    Else
                        If theuser.IsApproved = False Then
                            ModelState.AddModelError("username", "Akun user tidak aktif")
                        ElseIf theuser.IsLockedOut Then
                            ModelState.AddModelError("username", "Akun user telah terkunci")
                        Else
                            ModelState.AddModelError("username", "Username atau password tidak benar.")
                        End If


                    End If


                End If
            End If

            ' If we got this far, something failed, redisplay form
            'Return View(model)
            Return Json(New With {.stat = 0, .errors = JsonErrorObject.Convert(ModelState)})
        End Function

        '
        ' GET: /Account/LogOff
        <Authorize()>
        Public Function LogOff() As ActionResult
            FormsAuthentication.SignOut()

            Return RedirectToAction("Index", "Home")
        End Function

        '
        ' GET: /Account/Register
        <Authorize(Roles:="Administrator,SystemAdmin")>
        Public Function Register() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Account/Register
        <Authorize(Roles:="Administrator,SystemAdmin")>
        <HttpPost()> _
        Public Function Register(ByVal model As RegisterModel) As ActionResult
            If ModelState.IsValid Then
                ' Attempt to register the user
                If model.Password.Equals(model.ConfirmPassword) = True Then
                    Dim createStatus As MembershipCreateStatus
                    Membership.CreateUser(model.UserName, model.Password, model.Email, Nothing, Nothing, False, Nothing, createStatus)

                    If createStatus = MembershipCreateStatus.Success Then
                        Return Json(New With {.stat = 1})
                    Else
                        ModelState.AddModelError("", ErrorCodeToString(createStatus))
                    End If
                End If

            End If

            ' If we got this far, something failed, redisplay form
            Return Json(New With {.stat = 0, .errors = JsonErrorObject.Convert(ModelState)})

        End Function

        '
        ' GET: /Account/ChangePassword

        <Authorize()> _
        Public Function ChangePassword() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Account/ChangePassword

        <Authorize()> _
        <HttpPost()> _
        Public Function ChangePassword(ByVal model As ChangePasswordModel) As ActionResult
            If ModelState.IsValid Then
                ' ChangePassword will throw an exception rather
                ' than return false in certain failure scenarios.
                Dim changePasswordSucceeded As Boolean

                Try
                    Dim currentUser As MembershipUser = Membership.GetUser(User.Identity.Name, True)
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword)
                Catch ex As Exception
                    changePasswordSucceeded = False
                End Try

                If changePasswordSucceeded Then
                    Return Json(New With {.stat = 1})
                Else
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.")
                End If
            End If

            ' If we got this far, something failed, redisplay form
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        '
        ' GET: /Account/ChangePasswordSuccess

        Public Function ChangePasswordSuccess() As ActionResult
            Return View()
        End Function


        <Authorize()>
        Public Function Profile() As ActionResult
            Dim currentUser = Membership.GetUser(User.Identity.Name)

            Return View(currentUser)
        End Function
        <Authorize(Roles:="Administrator, SystemAdmin")>
        Public Function UserList() As ActionResult
            Return View()
        End Function
        <Authorize(Roles:="Administrator, SystemAdmin")>
        <HttpPost()>
        Public Function GetUserList() As ActionResult
            Dim sqlSelect As String = <sql>
            SELECT u.userid,
                   u.username,
                   m.email,
                   m.isapproved,
                   m.islockedout,
                   m.lastlogindate
            FROM   dbo.ASPNET_USERS u
                   inner join dbo.ASPNET_MEMBERSHIP m
               ON u.userid = m.userid 
                                  </sql>.Value


            Dim rvalue = _MainEntities.ExecuteStoreQuery(Of UserAccountModel)(sqlSelect)
            Return Json(New With {.data = rvalue})
        End Function

        <Authorize(Roles:="Administrator, SystemAdmin")>
        <HttpPost()>
        Public Function LockAccount(ByVal userid As String, lockState As Boolean) As ActionResult
            Dim currentUser As MembershipUser = Membership.GetUser(userid)
            currentUser.IsApproved = lockState
            Membership.UpdateUser(currentUser)
            Return Json(New With {.stat = 1})
        End Function


        <Authorize(Roles:="Administrator, SystemAdmin")>
        <HttpGet()>
        Public Function UserRole(ByVal Id As String) As ActionResult
            Dim sqlQueryModulename As String = <sql>

                SELECT DISTINCT modulename as value , modulename as [text]
                FROM   dbo.ASPNET_ROLES 
                                           </sql>.Value

            Dim modulenames = _MainEntities.ExecuteStoreQuery(Of OptionItem)(sqlQueryModulename)
            Dim selectList = New SelectList(modulenames, "value", "text")
            ViewData("ModuleNames") = selectList
            ViewData("userid") = Id


            Return View()
        End Function
        <Authorize(Roles:="Administrator, SystemAdmin")>
        Public Function GetUserRole(ByVal Id As String, modulename As String) As ActionResult
            Dim currentUser = Membership.GetUser(Id)
            Dim sqlSelect As String = <sql>
            SELECT r.roleid,
                   r.rolename,
                   r.[description] AS modulename,
                   CASE
                     WHEN u.roleid IS NULL THEN 0
                     ELSE 1
                   END             AS hasrole
            FROM   dbo.ASPNET_ROLES r
                   LEFT JOIN ( SELECT ur.roleid
                               FROM   dbo.ASPNET_USERSINROLES ur
                                      LEFT JOIN dbo.ASPNET_USERS u
                                             ON ur.userid = u.userid
                               WHERE  username = @userid ) u
                          ON u.roleid = r.roleid
            WHERE  r.modulename = @modulename 
                                  </sql>.Value

            Dim roleForThisUser = _MainEntities.ExecuteStoreQuery(Of RolesOfUser)(sqlSelect,
                                                                                  New SqlClient.SqlParameter("@userid", Id),
                                                                                  New SqlClient.SqlParameter("@modulename", modulename))


            Return Json(New With {.data = roleForThisUser})
        End Function


        <Authorize(Roles:="Administrator, SystemAdmin")>
        <HttpPost()>
        Public Function SetUserRole(ByVal userid As String, rolename As String, lockstate As Boolean)
            If lockstate Then
                Roles.AddUserToRole(userid, rolename)
            Else
                Roles.RemoveUserFromRole(userid, rolename)
            End If
            Return Json(New With {.stat = 1})
        End Function


        <Authorize(Roles:="Administrator, SystemAdmin")>
        <HttpGet()>
        Public Function ResetPassword(ByVal userid As String) As ActionResult
            Return View()
        End Function



#Region "upload photo profile"
        Public Function EditPhoto() As ActionResult
            Return View()
        End Function

        Public Function UploadPhotoProfile() As ActionResult
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
                Dim p = ProfileBase.Create(userId)
                Dim foto() As Byte = iomem.ToArray()
                p.SetPropertyValue("PhotoProfile", foto)
                p.Save()
                
            End If

            Return Json(New With {.stat = 1})

            Return View()
        End Function


        <HttpPost()>
        Function CropPhoto(ByVal x As String, ByVal y As String, ByVal w As String, ByVal h As String, ByVal userid As String) As ActionResult
            Dim posx, posy, posw, posh As Double
            Dim ci As New System.Globalization.CultureInfo("en-GB")
            Double.TryParse(x, Globalization.NumberStyles.Any, ci, posx)
            Double.TryParse(y, Globalization.NumberStyles.Any, ci, posy)
            Double.TryParse(w, Globalization.NumberStyles.Any, ci, posw)
            Double.TryParse(h, Globalization.NumberStyles.Any, ci, posh)



            Dim p = ProfileBase.Create(userid)
            Dim arrimg As Byte() = CType(p.GetPropertyValue("PhotoProfile"), Byte())

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
            p.SetPropertyValue("PhotoProfile", foto)
            p.Save()
            img.Dispose()
            g.Dispose()
            newbitmap.Dispose()

            Return RedirectToAction("EditPhoto", "Account", New With {.userid = userid})
        End Function
        Public Function UserPhoto(ByVal userId As String) As ActionResult
            Dim p = ProfileBase.Create(User.Identity.Name)
            Dim img = CType(p.GetPropertyValue("PhotoProfile"), Byte())
            If img Is Nothing Then
                Return File(Server.MapPath("~/img/_default_male.png"), "image/png", "unknown.png")
            Else
                Return File(img, "image/jpg", "foto_" & userId & ".jpg")
            End If

        End Function
#End Region

#Region "Status Code"
        Public Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
            ' See http://go.microsoft.com/fwlink/?LinkID=177550 for
            ' a full list of status codes.
            Select Case createStatus
                Case MembershipCreateStatus.DuplicateUserName
                    Return "User name already exists. Please enter a different user name."

                Case MembershipCreateStatus.DuplicateEmail
                    Return "A user name for that e-mail address already exists. Please enter a different e-mail address."

                Case MembershipCreateStatus.InvalidPassword
                    Return "The password provided is invalid. Please enter a valid password value."

                Case MembershipCreateStatus.InvalidEmail
                    Return "The e-mail address provided is invalid. Please check the value and try again."

                Case MembershipCreateStatus.InvalidAnswer
                    Return "The password retrieval answer provided is invalid. Please check the value and try again."

                Case MembershipCreateStatus.InvalidQuestion
                    Return "The password retrieval question provided is invalid. Please check the value and try again."

                Case MembershipCreateStatus.InvalidUserName
                    Return "The user name provided is invalid. Please check the value and try again."

                Case MembershipCreateStatus.ProviderError
                    Return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator."

                Case MembershipCreateStatus.UserRejected
                    Return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."

                Case Else
                    Return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator."
            End Select
        End Function
#End Region

        Public Sub New()
            _MainEntities = New MainEntities
        End Sub
    End Class
End Namespace