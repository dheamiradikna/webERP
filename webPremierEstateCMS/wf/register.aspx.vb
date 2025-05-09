Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].clsGeneral
Imports [class].cls_mUser

Partial Class wf_register
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtDomain As String = ""
    Protected txtEmail As String = ""
    Protected txtPassword As String = ""
    Protected txtName As String = ""
    Protected txtHP As String = ""

    Protected isDomain As String = "0"
    Protected isEmail As String = "0"
    Protected isPassword As String = "0"
    Protected isName As String = "0"
    Protected isHP As String = "0"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Dim _register As String = Request.Form("_register")

            If _register = "1" Then
                txtDomain = Request.Form("txtDomain")
                txtEmail = Request.Form("txtEmail")
                txtPassword = Request.Form("txtPassword")
                txtName = Request.Form("txtName")
                txtHP = Request.Form("txtHP")

                Dim Hashtable As New Hashtable

                Hashtable("domain") = txtDomain
                Hashtable("email") = txtEmail
                Hashtable("password") = txtPassword
                Hashtable("name") = txtName
                Hashtable("HP") = txtHP
                Hashtable("isDomain") = "0"
                Hashtable("isEmail") = "0"
                Hashtable("isPassword") = "0"
                Hashtable("isName") = "0"
                Hashtable("isHP") = "0"

                Dim domainRef As String = getDomainRefByName(txtDomain)

                If Not cekDomainValid(txtDomain) Then
                    Hashtable("note") = "Invalid domain"
                    Hashtable("isDomain") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
                End If

                If Not emailAddressCheck(txtEmail) Then
                    Hashtable("note") = "Invalid email"
                    Hashtable("isEmail") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
                End If

                If cekUsername(domainRef, txtEmail) Then
                    Hashtable("note") = "Email/Username already registered"
                    Hashtable("isEmail") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
                End If

                If Trim(txtPassword) = "" Then
                    Hashtable("note") = "Please fill password"
                    Hashtable("isPassword") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
                End If

                If Trim(txtName) = "" Then
                    Hashtable("note") = "Please fill name"
                    Hashtable("isName") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
                End If

                If Trim(txtHP) = "" Then
                    Hashtable("note") = "Please fill HP"
                    Hashtable("isHP") = "1"
                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
                End If

                Dim inputUN As String = Session("userName")
                If inputUN = String.Empty Then inputUN = ""

                Dim temp As String = insertUser(domainRef, txtEmail, txtPassword, txtName, txtHP, "", _defUserStatus, inputUN)

                If IsNumeric(temp) Then
                    Hashtable("domain") = ""
                    Hashtable("email") = ""
                    Hashtable("password") = ""
                    Hashtable("name") = ""
                    Hashtable("HP") = ""
                    Hashtable("note") = "Registration succeed, please wait for approval"

                    Response.Redirect(GetEncUrl(_rootPath + "wf/register.aspx?", Hashtable))
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
                txtDomain = param("domain")
                txtEmail = param("email")
                txtPassword = param("password")
                txtName = param("name")
                txtHP = param("HP")
                isDomain = param("isDomain")
                isEmail = param("isEmail")
                isPassword = param("isPassword")
                isName = param("isName")
                isHP = param("isHP")

            End If

            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If
        End If
    End Sub
End Class
