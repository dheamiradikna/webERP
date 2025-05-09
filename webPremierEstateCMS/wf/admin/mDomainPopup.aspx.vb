Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_mDomain
Imports [class].clsGeneralDB


Partial Class wf_admin_mDomainPopup
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected selDomainLevel As String = ""
    Protected txtDomainName As String = ""
    Protected txtIP As String = ""
    Protected txtCPName As String = ""
    Protected txtCPEmail As String = ""
    Protected txtCPHP As String = ""
    Protected txtCPPhone As String = ""
    Protected txtCPAddr As String = ""
    Protected ckActive As String = ""

    Protected selCountry As String = "All"
    Protected selProvince As String = "All"
    Protected selCity As String = "All"
    Protected txtDescription As String = ""
    Protected ckUpcoming As String = ""

    Private Function bindLogo(ByVal domainRef As String) As String
        Dim result As New StringBuilder

        If Trim(domainRef) = "" Then
            result.Append("<div class=""fNotif mt5 mb5"">After save, you can insert the logo.</div>")
        Else
            If cekDomainLogoFile(domainRef) Then
                result.Append("<div class=""ov mb5"">")
                result.Append("  <img src=""" + rootPath + "wf/support/displayImage.aspx?m=domainLogo&d=" + domainRef.ToString + "&w=400"" />")
                result.Append("</div>")
                result.Append("<div class=""mb5"">")
                result.Append("  <a href=""javascript:doDeleteLogo(" + domainRef.ToString + ");"">Delete Logo</a>")
                result.Append("</div>")
            Else
                result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=mDomainPopup.aspx&m=domainLogo&t=s&ir=0&r=" + domainRef.ToString + """>Click here</a> to upload.</div>")
            End If

        End If

        

        Return result.ToString
    End Function

    Private Function bindImage(ByVal domainRef As String) As String
        Dim result As New StringBuilder
        If Trim(domainRef) = "" Then
            result.Append("<div class=""fNotif mt5 mb5"">After save, you can insert the image.</div>")
        Else
            If cekDomainImageFile(domainRef) Then
                result.Append("<div class=""ov mb5"">")
                result.Append("  <img src=""" + rootPath + "wf/support/displayImage.aspx?m=domainImage&d=" + domainRef.ToString + "&w=400"" />")
                result.Append("</div>")
                result.Append("<div class=""mb5"">")
                result.Append("  <a href=""javascript:doDeleteImage(" + domainRef.ToString + ");"">Delete Image</a>")
                result.Append("</div>")
            Else
                result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=mDomainPopup.aspx&m=domainImage&t=s&ir=0&r=" + domainRef.ToString + """>Click here</a> to upload picture.</div>")
            End If

        End If

        Return result.ToString
    End Function

    Private Function bindSelDomainLevel(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getDomainLevelList()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selDomainLevel"" name=""selDomainLevel"" >")
            
            For i = 0 To dt.Rows.Count - 1
                If value = dt.Rows(i).Item("domainLevel") Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("domainLevel") + """>" + dt.Rows(i).Item("domainLevelName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("domainLevel") + """>" + dt.Rows(i).Item("domainLevelName") + "</option>")
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
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).Services.Add(New ServiceReference("~/wcf/wcfAddr.svc"))
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).CompositeScript.Scripts.Add(New ScriptReference("~/support/js/addrAll.js"))

        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""

        Dim q_ref As String = Request.QueryString("r")


        If IsPostBack Then
            Dim _save As String = Request.Form("_save")
            Dim _saveClose As String = Request.Form("_saveClose")
            Dim _delete As String = Request.Form("_delete")
            Dim _deleteImage As String = Request.Form("_deleteImage")
            Dim _deleteLogo As String = Request.Form("_deleteLogo")


            If Trim(_save) = "1" Or Trim(_saveClose) = "1" Then
                selDomainLevel = Request.Form("selDomainLevel")
                txtDomainName = Request.Form("txtDomainName")
                txtIP = Request.Form("txtIP")
                txtCPName = Request.Form("txtCPName")
                txtCPEmail = Request.Form("txtCPEmail")
                txtCPHP = Request.Form("txtCPHP")
                txtCPPhone = Request.Form("txtCPPhone")
                txtCPAddr = Request.Form("txtCPAddr")
                ckActive = IIf(Request.Form("ckActive") = "on", "1", "0")

                selCountry = Request.Form("selCountry")
                selProvince = Request.Form("selProvince")
                selCity = Request.Form("selCity")
                txtDescription = Request.Form("txtDescription")

                ckUpcoming = IIf(Request.Form("ckUpcoming") = "on", "1", "0")

                If Trim(selCountry) = "All" Then selCountry = "-"
                If Trim(selProvince) = "All" Then selProvince = "-"
                If Trim(selCity) = "All" Then selCity = "-"

                Dim temp As String = ""
                Dim Hashtable As New Hashtable

                If Trim(q_ref) <> "" Then
                    'update
                    temp = updateDomain(q_ref, selDomainLevel, txtDomainName, selCountry, selProvince, selCity, txtDescription, ckUpcoming, txtIP, txtCPName, txtCPEmail, txtCPHP, txtCPPhone, txtCPAddr, ckActive)
                Else
                    'insert
                    temp = insertDomain(selDomainLevel, txtDomainName, selCountry, selProvince, selCity, txtDescription, ckUpcoming, txtIP, txtCPName, txtCPEmail, txtCPHP, txtCPPhone, txtCPAddr, ckActive, inputUN)
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
                        Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mDomainPopup.aspx?r=" + temp + "&", Hashtable))

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

                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/notificationPopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_delete) = "1" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteDomain(q_ref)
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

            If Trim(_deleteImage) <> "" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteImage(_deleteImage)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "Delete image succeed."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mDomainPopup.aspx?r=" + q_ref + "&", Hashtable))

                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_deleteLogo) <> "" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteLogo(_deleteLogo)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "Delete logo succeed."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mDomainPopup.aspx?r=" + q_ref + "&", Hashtable))

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
                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Update Mall"

                Dim dt As New DataTable

                dt = getDomainInfo(q_ref)

                If dt.Rows.Count > 0 Then
                    ltrBtn.Text = "<div class=""linkBtn left mr5""> " + _
                                    "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("domainName")) + "');"">Delete</a> " + _
                                    "</div> " + _
                                    "<div class=""linkBtn left mr5""> " + _
                                    "  <a href=""" + rootPath + "wf/admin/mDomainPopup.aspx"">Insert New</a> " + _
                                    "</div> "

                    selDomainLevel = dt.Rows(0).Item("domainLevel")
                    txtDomainName = dt.Rows(0).Item("domainName")
                    txtIP = dt.Rows(0).Item("IP")
                    txtCPName = dt.Rows(0).Item("CPName")
                    txtCPEmail = dt.Rows(0).Item("CPEmail")
                    txtCPHP = dt.Rows(0).Item("CPHP")
                    txtCPPhone = dt.Rows(0).Item("CPPhone")
                    txtCPAddr = dt.Rows(0).Item("CPAddr")
                    ckActive = dt.Rows(0).Item("isActive")

                    selCountry = dt.Rows(0).Item("countryCode")
                    selProvince = dt.Rows(0).Item("provinceCode")
                    selCity = dt.Rows(0).Item("cityCode")
                    txtDescription = dt.Rows(0).Item("description")
                    ckUpcoming = dt.Rows(0).Item("isUpcomingMalls")
                End If

                ltrDomainLevel.Text = bindSelDomainLevel(selDomainLevel)

            Else

                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Insert Mall"

                ltrDomainLevel.Text = bindSelDomainLevel("")

            End If

            ltrDomainLogo.Text = bindLogo(q_ref)
            ltrDomainImage.Text = bindImage(q_ref)
        End If
    End Sub
End Class
