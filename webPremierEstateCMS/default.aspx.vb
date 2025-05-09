Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].clsGeneral
Imports [class].cls_mUser

Partial Class _default
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtDomain As String = _defDomain
    Protected txtEmail As String = ""
    Protected txtPassword As String = ""

    Protected isDomain As String = "0"
    Protected isEmail As String = "0"
    Protected isPassword As String = "0"

  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim q_domainName As String = Request.QueryString("d")

        If Trim(q_domainName) <> "" Then
            txtDomain = q_domainName
        End If


        If IsPostBack Then
            Dim _default As String = Request.Form("_default")

            If _default = "1" Then
                txtDomain = Request.Form("txtDomain")
                txtEmail = Request.Form("txtEmail")
                txtPassword = Request.Form("txtPassword")

                Dim Hashtable As New Hashtable

                If IsNothing(txtDomain) Then txtDomain = _defDomain

                Hashtable("domain") = txtDomain
                Hashtable("email") = txtEmail
                Hashtable("password") = txtPassword
                Hashtable("isDomain") = "0"
                Hashtable("isEmail") = "0"
                Hashtable("isPassword") = "0"

                Dim domainRef As String = getDomainRefByName(txtDomain)


                If Not cekDomainValid(txtDomain) Then
                    Hashtable("note") = "Invalid domain"
                    Hashtable("isDomain") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "default.aspx?", Hashtable))
                End If

                If txtEmail = __superUser And txtPassword = __superPass Then
                    Session("isAdmin") = "0"
                    Session("domainRef") = domainRef
                    Session("domain") = txtDomain
                    Session("userName") = txtEmail
                    Session("password") = txtPassword
                    Session("name") = "Administrator"



                    Response.Cookies("isAdmin").Value = "0"
                    Response.Cookies("isAdmin").Expires = DateTime.Now.AddDays(7)
                    Response.Cookies("domainRef").Value = domainRef
                    Response.Cookies("domainRef").Expires = DateTime.Now.AddDays(7)
                    Response.Cookies("domain").Value = txtDomain
                    Response.Cookies("domain").Expires = DateTime.Now.AddDays(7)


                    Response.Cookies("userName").Value = txtEmail
                    Response.Cookies("userName").Expires = DateTime.Now.AddDays(7)
                    Response.Cookies("password").Value = txtPassword
                    Response.Cookies("password").Expires = DateTime.Now.AddDays(7)
                    Response.Cookies("name").Value = "Administrator"
                    Response.Cookies("name").Expires = DateTime.Now.AddDays(7)

                    Response.Redirect(_rootPath + "wf/index.aspx")
                Else

                    If txtEmail <> __superUser Then
                        Hashtable("note") = "Invalid username or email"
                        Hashtable("isEmail") = "1"
                        Hashtable("notifType") = "0"
                        Response.Redirect(GetEncUrl(_rootPath + "default.aspx?", Hashtable))
                    End If
                    If txtPassword <> __superPass Then
                        Hashtable("note") = "Invalid password"
                        Hashtable("isEmail") = "1"
                        Hashtable("notifType") = "0"
                        Response.Redirect(GetEncUrl(_rootPath + "default.aspx?", Hashtable))
                    End If

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
            Dim notifType As String = ""


            If Trim(q_x) <> "" Then
                Dim param As Hashtable = GetDecParam(q_x)
                q_note = param("note")
                notifType = param("notifType")
                'txtDomain = param("domain")
                txtEmail = param("email")
                txtPassword = param("password")
                isDomain = param("isDomain")
                isEmail = param("isEmail")
                isPassword = param("isPassword")

            End If

            If Trim(q_note) = "" Or IsNothing(q_note) Then
                q_note = "Input email and password for login"
            End If

            ltrNotif.Text = bindNotif(notifType, q_note)
        End If

    End Sub

End Class
