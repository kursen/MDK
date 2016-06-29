Imports System.Data.SqlClient
Imports MDK_ERP.MDK_ERP


<Authorize()> _
Public Class UserManagementController
    Inherits BaseController

    '
    ' GET: /UserManagement

    Function Index() As ActionResult
        Return View()
    End Function

    Function getUserData() As JsonResult
        Dim allUsers = Membership.GetAllUsers

        Dim model = (From p As MembershipUser In allUsers
                    Select New ModelUserInformation With {
                        .ID = p.ProviderUserKey, _
                        .UserName = p.UserName, _
                        .IsLock = p.IsLockedOut}).ToList()

        Return Json(New With {.Data = model}, JsonRequestBehavior.AllowGet)
    End Function

    '
    ' GET: /UserManagement/Create

    Function Create(Optional ByVal ID As String = "") As ActionResult
        ViewData("allRoles") = Roles.GetAllRoles

        Dim model As New NewUser
        If String.IsNullOrEmpty(ID) Then
            Return View(model)
        Else
            Dim editUser = Membership.GetUser(Guid.Parse(ID))
            'model.UserKey = editUser.ProviderUserKey
            model.ID = editUser.ProviderUserKey
            model.UserName = editUser.UserName
            model.Password = ""
            model.PasswordConfirm = model.Password
            model.UserRole = Roles.GetRolesForUser(model.UserName)
            Return View(model)
        End If
    End Function

    <HttpPost()> _
    Function Create(ByVal model As NewUser) As ActionResult
        Dim dataUser = Membership.GetUser(model.ID)
        'MsgBox(model.UserKey.ToString())
        If ModelState.IsValid Then
            Try
                If IsNothing(dataUser) Then
                    Dim createUser As MembershipUser = Membership.CreateUser(model.UserName, model.Password)
                    Roles.AddUserToRoles(model.UserName, model.UserRole)
                Else
                    Dim resetPass = dataUser.ResetPassword()
                    Membership.Provider.ChangePassword(model.UserName, resetPass, model.Password)

                    Dim defaultRoles = Roles.GetRolesForUser(model.UserName)
                    If defaultRoles.Length > 0 Then
                        Roles.RemoveUserFromRoles(model.UserName, defaultRoles)
                    End If
                    Roles.AddUserToRoles(model.UserName, model.UserRole)
                End If

                Return RedirectToAction("Index", "UserManagement")
            Catch ex As MembershipCreateUserException
                ModelState.AddModelError("Error", GetErrorMessage(ex.StatusCode))
                'ModelState.AddModelError("", ex.Message)
            End Try
        Else
            ModelState.AddModelError("", "Seluruh Isian data wajib diisi")
        End If


        ' If we got this far, something failed, redisplay form
        'model.UserKey = dataUser.ProviderUserKey
        'model.UserName = dataUser.UserName
        'model.Password = ""
        'model.PasswordConfirm = model.Password
        ViewData("allRoles") = Roles.GetAllRoles
        Return View(model)
    End Function

    Function ActivateToggle(ByVal ID As String) As ActionResult
        Dim stat As Byte = 0
        Dim msg As String = ""
        Dim userData As MembershipUser
        Dim g As New Guid(ID)
        userData = Membership.GetUser(g)
        Try
            If Not userData.UserName.ToLower() = User.Identity.Name.ToLower() Then
                Dim _provinder As MembershipProvider = Membership.Provider
                Dim aspUser = _provinder.GetUser(userData.UserName, False)

                If userData.IsLockedOut = True Then
                    aspUser.IsApproved = True
                    aspUser.UnlockUser()
                Else
                    aspUser.IsApproved = False
                    ctx.ExecuteStoreCommand("UPDATE aspnet_Membership SET IsLockedOut=@Status WHERE UserId=@UserId",
                                            New SqlParameter("@Status", True), New SqlParameter("@UserId", g))
                End If
                Membership.UpdateUser(aspUser)
                stat = 1
            Else
                msg = "Error :" + "User saat ini sedang digunakan."
            End If
        Catch ex As Exception
            msg = "Error :" + ex.Message
        End Try
        Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
    End Function

    Function Delete(ByVal ID As String) As ActionResult
        Dim stat As Byte = 0
        Dim msg As String = ""
        Try
            Dim editUser = Membership.GetUser(Guid.Parse(ID))
            'Dim editUser = Membership.GetUser(userID)

            If Not IsNothing(editUser) AndAlso Not editUser.UserName.ToLower() = User.Identity.Name.ToLower() Then
                Dim defaultRoles = Roles.GetRolesForUser(editUser.UserName)
                If defaultRoles.Length > 0 Then
                    Roles.RemoveUserFromRoles(editUser.UserName, defaultRoles)
                    Membership.DeleteUser(editUser.UserName)
                End If
                stat = 1
            End If
        Catch ex As Exception
            msg = "Error :" + ex.Message
        End Try
        Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
    End Function

    Public Function GetErrorMessage(ByVal status As MembershipCreateStatus) As String

        Select Case status
            Case MembershipCreateStatus.DuplicateUserName
                Return "Nama user ini sudah ada"

            Case MembershipCreateStatus.DuplicateEmail
                Return "A username for that e-mail address already exists. Please enter a different e-mail address."

            Case MembershipCreateStatus.InvalidPassword
                Return "Password invalid"

            Case MembershipCreateStatus.InvalidEmail
                Return "The e-mail address provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidAnswer
                Return "The password retrieval answer provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidQuestion
                Return "The password retrieval question provided is invalid. Please check the value and try again."

            Case MembershipCreateStatus.InvalidUserName
                Return "Nama user harus diisi"

            Case MembershipCreateStatus.ProviderError
                Return "The authentication provider Returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            Case MembershipCreateStatus.UserRejected
                Return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator."

            Case Else
                Return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator."
        End Select
    End Function

End Class
