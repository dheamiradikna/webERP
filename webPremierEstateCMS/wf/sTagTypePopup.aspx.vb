Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_sTagType
Imports [class].clsGeneralDB

Partial Class wf_sTagTypePopup
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtTagTypeName As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""

        Dim q_ref As String = Request.QueryString("r")

        If IsPostBack Then
            Dim _save As String = Request.Form("_save")
            Dim _saveClose As String = Request.Form("_saveClose")
            Dim _delete As String = Request.Form("_delete")

            If Trim(_save) = "1" Or Trim(_saveClose) = "1" Then
                txtTagTypeName = Request.Form("txtTagTypeName")

                Dim temp As String = ""
                Dim Hashtable As New Hashtable

                If Trim(q_ref) <> "" Then
                    'update
                    temp = updateTagType(Session("domainRef").ToString, q_ref, txtTagTypeName)
                Else
                    'insert
                    temp = insertTagType(Session("domainRef"), txtTagTypeName, inputUN)
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

                        'Response.Redirect(GetEncUrl(_rootPath + "wf/sTagTypePopup.aspx?r=" + temp + "&", Hashtable))
                        Response.Redirect(GetEncUrl(_rootPath + "wf/sTagTypeInput.aspx?r=" + temp + "&", Hashtable))

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

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_delete) = "1" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteTagType(Session("domainRef").ToString, q_ref)
                If Trim(temp) = "" Then
                    'benar
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; alert('Delete succeed, please click Refresh / Search button'); window.close();</script>")
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

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

            If Trim(q_ref) <> "" Then
                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Update Tag Type Name [" + Session("domain") + "]"

                Dim dt As New DataTable

                dt = getTagTypeInfo(Session("domainRef").ToString, q_ref)

                If dt.Rows.Count > 0 Then
                    ltrBtn.Text = "<div class=""btn btn-md btn-dark""> " + _
                                    "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("tagTypeName")) + "');"">Delete</a> " + _
                                    "</div> " + _
                                    "<div class=""btn btn-md btn-dark""> " + _
                                    "  <a href=""" + rootPath + "wf/sTagTypePopup.aspx"">Insert New</a> " + _
                                    "</div> "

                    txtTagTypeName = dt.Rows(0).Item("tagTypeName")
                End If

            Else

                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Insert Tag Type Name [" + Session("domain") + "]"


            End If
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWebPopup()
    End Sub

End Class
