Imports [class].clsWebGeneral
Imports [class].clsGeneral


Partial Class wf_admin_default
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtEmail As String = ""
    Protected txtPassword As String = ""

    Protected isEmail As String = "0"
    Protected isPassword As String = "0"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Dim _default As String = Request.Form("_default")

            If _default = "1" Then
                txtEmail = Request.Form("txtEmail")
                txtPassword = Request.Form("txtPassword")

                Dim Hashtable As New Hashtable

                Hashtable("email") = txtEmail
                Hashtable("password") = txtPassword
                Hashtable("isEmail") = "0"
                Hashtable("isPassword") = "0"

                If txtEmail = __superUser And txtPassword = __superPass Then
                    Session("isAdmin") = "1"
                    Session("userName") = txtEmail
                    Session("password") = txtPassword
                    Session("name") = "Administrator"

                    Response.Redirect(_rootPath + "wf/admin/index.aspx")

                ElseIf txtEmail = __superUser And txtPassword = __superPass Then
                    Session("isAdmin") = "1"
                    Session("userName") = txtEmail
                    Session("password") = txtPassword
                    Session("name") = "Administrator"

                    Response.Redirect(_rootPath + "wf/admin/index.aspx")
                Else
                    Hashtable("note") = "Wrong user and password"
                    Hashtable("isEmail") = "1"
                    Hashtable("isPassword") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "admin/default.aspx?", Hashtable))
                End If


            End If
        Else
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''

            Dim q_x As String = Request.Params("x")
            Dim q_note As String = ""
            Dim isRefresh As String = ""


            If Trim(q_x) <> "" Then
                Dim param As Hashtable = GetDecParam(q_x)
                q_note = param("note")
                txtEmail = param("email")
                txtPassword = param("password")
                isEmail = param("isEmail")
                isPassword = param("isPassword")

            End If

            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If
        End If
    End Sub

End Class
