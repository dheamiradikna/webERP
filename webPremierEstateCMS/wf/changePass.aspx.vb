Imports [class].clsWebGeneral
Imports [class].clsSecurityDB
Imports [class].clsGeneral
Imports [class].clsGeneralDB
'Imports [class].cls_sUser

Partial Class wf_changePass
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath
    Protected txtOldPassword As String = ""
    Protected txtNewPassword As String = ""
    Protected txtRetype As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = "Home :: Change Password"

        If IsPostBack Then
            Dim _save As String = Request.Form("_save")

            If Trim(_save) = "1" Then
                txtOldPassword = Request.Form("txtOldPassword")
                txtNewPassword = Request.Form("txtNewPassword")
                txtRetype = Request.Form("txtRetype")

                Dim Hashtable As New Hashtable
                Dim userRef As Integer = 0
                userRef = cekLoginRetRef(Session("domainRef").ToString, Session("userName").ToString, txtOldPassword)
                If userRef = 0 Then
                    Hashtable("note") = "Wrong old password"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/changePass.aspx?", Hashtable))
                End If

                If txtNewPassword <> txtRetype Then
                    Hashtable("note") = """New password"" different than ""Retype"""
                    Response.Redirect(GetEncUrl(_rootPath + "wf/changePass.aspx?", Hashtable))
                End If


                Dim temp As String = ""
                temp = updateUserPassword(userRef, Session("domainRef").ToString, txtNewPassword)

                If Trim(temp) = "" Then
                    Hashtable("note") = "Update succeed."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/changePass.aspx?", Hashtable))
                Else
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:history.go(-1);"">Click here</a> to go back.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notification.aspx?", Hashtable))
                End If
            End If
        Else
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''

            If Not Request.Params("x") Is Nothing Then
                Dim param As Hashtable = GetDecParam(Request.Params("x"))
                Dim q_note As String = param("note")

                If Trim(q_note) <> "" Then
                    divNotif.InnerHtml = "Notification: " + q_note
                End If
            End If


        End If

    End Sub

End Class
