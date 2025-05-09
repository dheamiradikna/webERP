Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_mUser
Imports [class].clsGeneralDB

Partial Class wf_admin_mUserPopup
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected selUserStatus As String = ""
    Protected txtEmail As String = ""
    Protected txtPassword As String = ""
    Protected txtName As String = ""
    Protected txtHP As String = ""
    Protected txtPhone As String = ""


    Private Function bindSelUserStatus(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getUserStatusListLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selUserStatus"" name=""selUserStatus"" >")

            For i = 0 To dt.Rows.Count - 1
                If value = dt.Rows(i).Item("userStatus") Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("userStatus") + """>" + dt.Rows(i).Item("userStatusName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("userStatus") + """>" + dt.Rows(i).Item("userStatusName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWebPopup()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""

        Dim q_domainRef As String = Request.QueryString("d")
        Dim q_ref As String = Request.QueryString("r")




        If IsPostBack Then
            Dim _save As String = Request.Form("_save")
            Dim _saveClose As String = Request.Form("_saveClose")
            Dim _delete As String = Request.Form("_delete")

            If Trim(_save) = "1" Or Trim(_saveClose) = "1" Then
                selUserStatus = Request.Form("selUserStatus")
                txtEmail = Request.Form("txtEmail")
                txtPassword = Request.Form("txtPassword")
                txtName = Request.Form("txtName")
                txtHP = Request.Form("txtHP")
                txtPhone = Request.Form("txtPhone")
                Dim ckEmailNotif As String = IIf(Request.Form("ckEmailNotif") = "on", "1", "0")

                Dim temp As String = ""
                Dim Hashtable As New Hashtable

                If Trim(q_ref) <> "" Then
                    'update
                    temp = updateUser(q_domainRef, q_ref, txtEmail, txtPassword, txtName, txtHP, txtPhone, selUserStatus)
                Else
                    'insert
                    temp = insertUser(q_domainRef, txtEmail, txtPassword, txtName, txtHP, txtPhone, selUserStatus, inputUN)
                End If

                If IsNumeric(temp) Then
                    If Trim(_save) = "1" Then
                        If Trim(q_ref) <> "" Then
                            'update
                            Hashtable("note") = "Update succeed."
                        Else
                            'insert
                            Hashtable("note") = "Insert succeed."
                        End If

                        If (ckEmailNotif = "1") Then
                            'send email nih
                        End If

                        Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mUserPopup.aspx?d=" + q_domainRef + "&r=" + temp + "&", Hashtable))

                    Else
                        If Trim(q_ref) <> "" Then
                            'update
                            Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; window.close();</script>")
                        Else
                            Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; alert('Insert succeed, please click Refresh / Search button'); window.close();</script>")
                        End If
                    End If

                Else
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/notificationpopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_delete) = "1" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteUser(q_domainRef, q_ref)
                If Trim(temp) = "" Then
                    'benar
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; alert('Delete succeed, please click Refresh / Search button'); window.close();</script>")
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/notificationpopup.aspx?", Hashtable))

                End If
            End If
        Else
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

            Dim domainName As String = getDomainNameByRef(q_domainRef)


            If Trim(q_ref) <> "" Then
                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Update User [" + domainName + "]"

                Dim dt As New DataTable

                dt = getUserInfo(q_domainRef, q_ref)

                If dt.Rows.Count > 0 Then
                    ltrBtn.Text = "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("email")) + "');"" class=""btn btn-md btn-dark""><span>Delete</span></a> " + _
                                  "  <a href=""" + rootPath + "wf/admin/mUserPopup.aspx?d=" + q_domainRef + """ class=""btn btn-md btn-dark""><span>Insert New</span></a> " 
                                   

                    selUserStatus = dt.Rows(0).Item("userStatus")
                    txtEmail = dt.Rows(0).Item("email")
                    txtPassword = dt.Rows(0).Item("password")
                    txtName = dt.Rows(0).Item("name")
                    txtHP = dt.Rows(0).Item("HP")
                    txtPhone = dt.Rows(0).Item("phone")

                End If

                ltrUserStatus.Text = bindSelUserStatus(selUserStatus)

            Else

                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Insert User [" + domainName + "]"

                ltrUserStatus.Text = bindSelUserStatus("")

            End If
        End If
    End Sub

End Class
