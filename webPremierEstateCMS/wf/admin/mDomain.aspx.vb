Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].cls_mDomain

Partial Class wf_admin_mDomain
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtKeyword As String = ""
    Protected rbActive As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""
    Protected selDomainLevel As String = ""

    Dim sortByValue() As String = {"0", "1", "2", "3", "4", "5", "6"}
    Dim sortByText() As String = {"-", "Ref", "Level", "Name", "IP", "CP Name", "Is Active"}
    Dim sortByDB() As String = {"-", "domainRef", "domainLevelName", "domainName", "IP", "CPName", "isActive"}

    Private Function bindSelDomainLevelAll(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getDomainLevelList()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selDomainLevel"" name=""selDomainLevel"" >")
            If value = "All" Then
                result.Append("<option selected value=""All"">All</option>")
            Else
                result.Append("<option value=""All"">All</option>")
            End If
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

    Private Sub bindContent(ByVal pageNo As Integer, ByVal isActive As String, _
                            ByVal domainLevel As String, ByVal keyword As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getDomainList(domainLevel, isActive, keyword, sortBy, sortType)
        numOfData = dt.Rows.Count


        If dt.Rows.Count > 0 Then

            '''' setting pagging ''''
            Dim totalPage As Int16 = Math.Ceiling(dt.Rows.Count / rowCountPage)
            If pageNo > totalPage Then
                pageNo = totalPage
            End If

            Dim firstRow As Int16 = ((pageNo - 1) * rowCountPage) + 1
            Dim countPage As Int16 = 0
            ''''' end setting pagging '''' 

            result.Append("<div class=""fNotif mb5"">" + dt.Rows.Count.ToString + " rows affected</div>")

            result.Append("<table class=""tbl"" cellpadding=""4"" cellspacing=""1"">")
            result.Append("  <tr class=""tblRowHeader"">")
            result.Append("    <td align=""center"">Level</td>")
            result.Append("    <td align=""center"">Name</td>")
            result.Append("    <td align=""center"">IP</td>")
            result.Append("    <td align=""center"">CP Name</td>")
            result.Append("    <td align=""center"">CP Email</td>")
            result.Append("    <td align=""center"">CP HP</td>")
            result.Append("    <td align=""center"">Is Active</td>")
            result.Append("    <td></td>")
            result.Append("  </tr>")

            For i = 0 To dt.Rows.Count - 1

                If i + 1 >= firstRow Then
                    Dim strActive As String = ""

                    If dt.Rows(i).Item("isActive") = "1" Then
                        strActive = "<img height=""13"" src=""" + rootPath + "support/image/icon_yes.png"" />"
                    Else
                        strActive = "<img height=""13"" src=""" + rootPath + "support/image/icon_no.png"" />"
                    End If

                    If i Mod 2 = 0 Then
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow1"">")
                    Else
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow2"">")
                    End If
                    result.Append("    <td valign=""top""><div id=""divDomainLevelName" + i.ToString + """>" + dt.Rows(i).Item("domainLevelName") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divDomainName" + i.ToString + """>" + dt.Rows(i).Item("domainName") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divIP" + i.ToString + """>" + dt.Rows(i).Item("IP") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divCPName" + i.ToString + """>" + dt.Rows(i).Item("CPName") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divCPEmail" + i.ToString + """>" + dt.Rows(i).Item("CPEmail") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divCPHP" + i.ToString + """>" + dt.Rows(i).Item("CPHP") + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divActive" + i.ToString + """>" + strActive + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center"">")
                    result.Append("      <a title=""Update"" href=""javascript:showPopup(" + i.ToString + "," + dt.Rows(i).Item("domainRef").ToString + ",'" + rootPath + "wf/admin/mDomainPopup.aspx?r=" + dt.Rows(i).Item("domainRef").ToString + "');""><img alt=""Update"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_update.png"" /></a>&nbsp;")
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("domainRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("domainName")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
                    result.Append("    </td>")
                    result.Append("  </tr>")


                    '''' to break after all row in a page done ''''
                    countPage = countPage + 1
                    If countPage >= rowCountPage Then
                        Exit For
                    End If
                    '''' end break ''''
                End If


            Next


            result.Append("</table>")
        Else

            result.Append("<div class=""fNotif"">* No data available</div>")


        End If

        divTable.InnerHtml = result.ToString


    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).Services.Add(New ServiceReference("~/wcf/wf/admin/wcf_mDomain.svc"))
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).CompositeScript.Scripts.Add(New ScriptReference("~/support/js/wf/admin/js_mDomain.js"))



        Dim q_page As String = Request.QueryString("p")
        If Trim(q_page) = "" Then q_page = "1"

        If IsPostBack Then
            Dim _mDomain As String = Request.Form("_mDomain")
            Dim _mDomainDelete As String = Request.Form("_mDomainDelete")

            rbActive = Request.Form("rbActive")
            selDomainLevel = Request.Form("selDomainLevel")
            txtKeyword = Request.Form("txtKeyword")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            Dim Hashtable As New Hashtable

            Hashtable("active") = rbActive
            Hashtable("domainLevel") = selDomainLevel
            Hashtable("keyword") = txtKeyword
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("refresh") = 1

            If Trim(_mDomain) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mDomain.aspx?p=1&", Hashtable))
            End If

            If Trim(_mDomainDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteDomain(_mDomainDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mDomain.aspx?p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mDomain.aspx?p=" + q_page + "&", Hashtable))
                End If
            End If



        Else
            'not postback

            Dim q_x As String = Request.Params("x")
            Dim q_note As String = ""
            Dim isRefresh As String = ""


            If Trim(q_x) <> "" Then
                Dim param As Hashtable = GetDecParam(q_x)
                q_note = param("note")
                txtKeyword = param("keyword")
                rbActive = param("active")
                selDomainLevel = param("domainLevel")
                selSortBy = param("sortBy")
                selSortType = param("sortType")
                isRefresh = param("refresh")
            End If

            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If

            If Trim(selSortBy) = "" Then selSortBy = "0"
            If Trim(selSortType) = "" Then selSortType = "0"
            If Trim(selDomainLevel) = "" Then selDomainLevel = "All"

            ltrSortBy.Text = bindSelSortBy(sortByValue, sortByText, selSortBy)
            ltrSortType.Text = bindSelSortType(selSortType)
            ltrDomainLevel.Text = bindSelDomainLevelAll(selDomainLevel)

            If isRefresh = "1" Then

                Dim isActiveParam As String = "All"
                Select Case rbActive
                    Case "1" 'all
                        isActiveParam = "All"
                    Case "2" 'yes
                        isActiveParam = "1"
                    Case "3" 'no
                        isActiveParam = "0"
                End Select

                Dim sortByParam As String = sortByDB(selSortBy)
                Dim sortTypeParam As String = sortTypeDB(selSortType)

                Dim numOfData As Integer = 0
                bindContent(q_page, isActiveParam, selDomainLevel, txtKeyword, sortByParam, sortTypeParam, numOfData)

                Dim Hashtable As New Hashtable

                Hashtable("active") = rbActive
                Hashtable("domainLevel") = selDomainLevel
                Hashtable("keyword") = txtKeyword
                Hashtable("sortBy") = selSortBy
                Hashtable("sortType") = selSortType
                Hashtable("refresh") = "1"

                Dim urlPaging As String = ""
                urlPaging = GetEncUrl(rootPath + "wf/admin/mDomain.aspx?p=@p&", Hashtable)

                divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If




        End If
    End Sub
End Class
