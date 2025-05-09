Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].cls_wmContent

Partial Class wf_wmContent
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected rbApproved As String = ""
    Protected rbPublish As String = ""
    Protected rbExpired As String = ""

    Protected selContentType As String = ""
    Protected selImageSetting As String = ""

    Protected selDayContentFr As String = ""
    Protected selMonthContentFr As String = ""
    Protected txtYearContentFr As String = ""
    Protected selDayContentTo As String = ""
    Protected selMonthContentTo As String = ""
    Protected txtYearContentTo As String = ""

    Protected txtKeyword As String = ""
    Protected txtMetaDescription As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""

    Protected txtTagName As String = ""
    Protected txtTagRef As String = ""

    Dim sortByValue() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
    Dim sortByText() As String = {"-", "Ref", "Title", "Content Type", "Image Setting", "Content Date", "Publish Date", "Approved Date", "Expired Date", "Hit / View"}
    Dim sortByDB() As String = {"-", "contentRef", "title", "contentTypeName", "imgSettingName", "contentDate", "publishDate", "approvedDate", "expiredDate", "hit"}

    Public Sub rekContentTagChild(ByVal domainRef As String, ByVal contentRef As String, _
                                 ByRef strTree As StringBuilder, ByVal tagRef As String, _
                                  ByVal parentName As String, ByRef allTag As String, ByVal isAllTagAdd As Boolean, _
                                  ByVal level As Integer)
        Dim dt As New DataTable

        If Trim(contentRef) = "" Then contentRef = "0"

        If tagRef <> "0" Then
            If isAllTagAdd Then
                If allTag = "" Then
                    allTag = tagRef
                Else
                    allTag = allTag + "-" + tagRef
                End If
            End If
        End If

        dt = getTagListByParentLookup(domainRef, tagRef)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim tab As Integer = 20 * level
                Dim includeAllTag As Boolean = False

                strTree.Append("<div style=""margin-left:" + tab.ToString + "px; margin-top:3px;"" id=""divH" + dt.Rows(i).Item("tagRef").ToString + """>")

                ''''' semua bisa di add, gak peduli punya child atau tidak '''''
                If cekIsTagHaveChild(domainRef, dt.Rows(i).Item("tagRef").ToString) Then
                    strTree.Append(" <span><img src=""" + rootPath + "support/image/icon_folderOpen.png"" style=""vertical-align:middle"" /></span>&nbsp;")
                    strTree.Append(dt.Rows(i).Item("tagName") + " <span class=""fNotif"">[" + dt.Rows(i).Item("tagTypeName") + "]</span>")
                Else
                    strTree.Append(" <span><img src=""" + rootPath + "support/image/icon_folder.png"" style=""vertical-align:middle"" /></span>&nbsp;")
                    strTree.Append(dt.Rows(i).Item("tagName") + " <span class=""fNotif"">[" + dt.Rows(i).Item("tagTypeName") + "]</span>")
                End If
                includeAllTag = True
                strTree.Append(" <span id=""spIsG" + dt.Rows(i).Item("tagRef").ToString + """>")
                If cekIsThisTagContent(domainRef, contentRef, dt.Rows(i).Item("tagRef").ToString) Then
                    strTree.Append("&nbsp;<img height=""16"" src=""" + rootPath + "support/image/icon_yes.png"" style=""vertical-align:middle"" />")
                    strTree.Append("</span>")
                    'strTree.Append(" &nbsp;&nbsp;<span id=""spRemAdd" + dt.Rows(i).Item("tagRef").ToString + """><a href=""javascript:doRemoveContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">Remove</a></span>")
                    strTree.Append("&nbsp;&nbsp;<span id=""spRemAdd" + dt.Rows(i).Item("tagRef").ToString + """><a href=""javascript:doAddContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ + ]</a>&nbsp;<a href=""javascript:doRemoveContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ - ]</a></span>")
                Else
                    strTree.Append("</span>")
                    'strTree.Append(" &nbsp;<span id=""spRemAdd" + dt.Rows(i).Item("tagRef").ToString + """><a href=""javascript:doAddContentTag('" + domainRef + "'," + contentRef + "," + dt.Rows(i).Item("tagRef").ToString + ");"">Add</a></span>")
                    strTree.Append("&nbsp;&nbsp;<span id=""spRemAdd" + dt.Rows(i).Item("tagRef").ToString + """><a href=""javascript:doAddContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ + ]</a>&nbsp;<a href=""javascript:doRemoveContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ - ]</a></span>")
                End If
                ''''' semua bisa di add, gak peduli punya child atau tidak '''''
                strTree.Append("</div>")
                strTree.Append("<div>")
                rekContentTagChild(domainRef, contentRef, strTree, dt.Rows(i).Item("tagRef"), IIf(Trim(parentName) = "", dt.Rows(i).Item("tagName"), parentName + " :: " + dt.Rows(i).Item("tagName")), allTag, includeAllTag, level + 1)
                strTree.Append("</div>")
            Next
        End If
    End Sub

    Private Function bindTag(ByVal domainRef As String, ByVal contentRef As String) As String
        Dim result As New StringBuilder
        Dim allGroup As String = ""

        rekContentTagChild(domainRef, contentRef, result, "0", "", allGroup, False, 0)

        result.Append("<div class=""mcol""><a href=""javascript:doRemoveContentTagAll()""class=""btn btn-sm btn-dark""><span>Remove All Tag</span></a>&nbsp;|&nbsp;<a href=""javascript:doCloseTagTree()"" class=""btn btn-sm btn-dark""><span>Close Tag Tree</span></a></div>")

        Return result.ToString
    End Function

    Private Function bindDay(ByVal id As String, ByVal value As String, ByVal notifID As String) As String
        Dim result As New StringBuilder
        Dim i As Integer

        result.Append("<select id=""selDay" + id + """ name=""selDay" + id + """ onchange=""cekDate('selDay" + id + "','selMonth" + id + "','txtYear" + id + "',document.getElementById('" + notifID + "'));"">")
        result.Append("<option value=""-"">-</option>")
        For i = 1 To 31
            If value = i.ToString Then
                result.Append("<option selected=""selected"" value=""" + i.ToString + """>" + i.ToString + "</option>")
            Else
                result.Append("<option value=""" + i.ToString + """>" + i.ToString + "</option>")
            End If

        Next
        result.Append("</select>")

        Return result.ToString

    End Function

    Private Function bindMonth(ByVal id As String, ByVal value As String, ByVal notifID As String) As String
        Dim result As New StringBuilder
        Dim i As Integer

        result.Append("<select id=""selMonth" + id + """ name=""selMonth" + id + """ onchange=""cekDate('selDay" + id + "','selMonth" + id + "','txtYear" + id + "',document.getElementById('" + notifID + "'));"">")
        result.Append("<option value=""-"">-</option>")
        For i = 0 To monthValue.Count - 1
            If value = (i + 1).ToString Then
                result.Append(" <option selected=""selected"" value=""" + monthValue(i) + """>" + monthName(i) + "</option>")
            Else
                result.Append(" <option value=""" + monthValue(i) + """>" + monthName(i) + "</option>")
            End If

        Next

        result.Append("</select>")

        Return result.ToString


    End Function

    Private Function bindSelContentTypeAll(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getContentTypeListLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selContentType"" name=""selContentType"" >")
            If value = "All" Then
                result.Append("<option selected value=""All"">All</option>")
            Else
                result.Append("<option value=""All"">All</option>")
            End If
            For i = 0 To dt.Rows.Count - 1

                If value = dt.Rows(i).Item("contentType").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("contentType").ToString + """>" + dt.Rows(i).Item("contentTypeName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("contentType").ToString + """>" + dt.Rows(i).Item("contentTypeName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Function bindSelImageSettingAll(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getContentImgSettingLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selImageSetting"" name=""selImageSetting"" >")
            If value = "All" Then
                result.Append("<option selected value=""All"">All</option>")
            Else
                result.Append("<option value=""All"">All</option>")
            End If
            For i = 0 To dt.Rows.Count - 1

                If value = dt.Rows(i).Item("imgSetting").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("imgSetting").ToString + """>" + dt.Rows(i).Item("imgSettingName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("imgSetting").ToString + """>" + dt.Rows(i).Item("imgSettingName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Sub bindContent(ByVal domainRef As String, ByVal contentType As String, _
                            ByVal imgSetting As String, ByVal tagRefList As String, _
                            ByVal contentDateFr As String, ByVal contentDateTo As String, _
                            ByVal isApproved As String, ByVal isPublish As String, ByVal isExpired As String, _
                            ByVal pageNo As Integer, _
                            ByVal keyword As String, ByVal metaDescription As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getContentList(domainRef, contentType, imgSetting, tagRefList, contentDateFr, _
                            contentDateTo, isApproved, isPublish, isExpired, keyword, sortBy, sortType)
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

            Dim URLPreviewContent As String = getDomainURLPreviewContent(Session("domainRef").ToString)

            'result.Append("<div class=""fNotif mb5"">" + dt.Rows.Count.ToString + " rows affected</div>")
            result.Append("	<section class=""section shopping-cart pt-0"">")
            result.Append("      <div class=""container relative"">")

            result.Append("     <h6>" + dt.Rows.Count.ToString + " Rows Affected</h6>")
            result.Append("		   <div class=""row"">")
            result.Append("          <div class=""col-md-12"">")
            result.Append("            <div class=""table-wrap mb-30"">")

            result.Append("<table class=""table table-bordered"" cellpadding=""4"" cellspacing=""1"">")
            result.Append("  <tr class=""cart_item"">")
            result.Append("    <td align=""center"">Type</td>")
            result.Append("    <td align=""center"">Title</td>")
            result.Append("    <td align=""center"">Content Date</td>")
            result.Append("    <td align=""center"">Pub Date</td>")
            result.Append("    <td align=""center"">Apprv Date</td>")
            result.Append("    <td align=""center"">Exp Date</td>")
            result.Append("    <td align=""center"">TAG</td>")
            result.Append("    <td align=""center"">Hit</td>")
            result.Append("    <td></td>")
            result.Append("  </tr>")

            For i = 0 To dt.Rows.Count - 1

                If i + 1 >= firstRow Then
                    Dim strContentDate As String = ""
                    Dim strPubDate As String = ""
                    Dim strApprvDate As String = ""
                    Dim strExpDate As String = ""

                    If Not IsDBNull(dt.Rows(i).Item("contentDate")) Then
                        strContentDate = Format(dt.Rows(i).Item("contentDate"), "dd/MM/yyyy")
                    End If

                    If Not IsDBNull(dt.Rows(i).Item("publishDate")) Then
                        strPubDate = Format(dt.Rows(i).Item("publishDate"), "dd/MM/yyyy")
                    End If

                    If Not IsDBNull(dt.Rows(i).Item("approvedDate")) Then
                        strApprvDate = Format(dt.Rows(i).Item("approvedDate"), "dd/MM/yyyy")
                    End If

                    If Not IsDBNull(dt.Rows(i).Item("expiredDate")) Then
                        strExpDate = Format(dt.Rows(i).Item("expiredDate"), "dd/MM/yyyy")
                    End If

                    If i Mod 2 = 0 Then
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow1"">")
                    Else
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow2"">")
                    End If
                    result.Append("    <td valign=""top""><div id=""divType" + i.ToString + """>" + dt.Rows(i).Item("contentTypeName") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divTitle" + i.ToString + """>" + dt.Rows(i).Item("title") + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divContentDate" + i.ToString + """>" + strContentDate + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divPubDate" + i.ToString + """>" + strPubDate + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divApprvDate" + i.ToString + """>" + strApprvDate + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divExpDate" + i.ToString + """>" + strExpDate + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divTAG" + i.ToString + """>" + getContentTagNameStr(Session("domainRef").ToString, dt.Rows(i).Item("contentRef").ToString) + "</div></td>")
                    result.Append("    <td valign=""top"" align=""right""><div id=""divHit" + i.ToString + """>" + dt.Rows(i).Item("hit").ToString + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center"">")
                    result.Append("      <a title=""Update"" href=""javascript:showPopup(" + i.ToString + "," + dt.Rows(i).Item("contentRef").ToString + ",'" + rootPath + "wf/wmContentPopup.aspx?r=" + dt.Rows(i).Item("contentRef").ToString + "');""><img alt=""Update"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_update.png"" /></a>&nbsp;")
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("contentRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("title")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
                    If Trim(URLPreviewContent) <> "" Then
                        URLPreviewContent = Replace(URLPreviewContent, "@ref", dt.Rows(i).Item("contentRef").ToString)
                        result.Append("      <a title=""View"" target=""_blank"" href=""" + URLPreviewContent + """><img alt=""view"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_view.png"" /></a>")
                    End If
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
            result.Append("          </div> ")
            result.Append("        </div> ")

            result.Append("	</div>")
            result.Append("</section>")
        Else
            result.Append("<div class=""fNotif"">* No data available</div>")
        End If
        divTable.InnerHtml = result.ToString

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).Services.Add(New ServiceReference("~/wcf/wf/wcf_wmContent.svc"))
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).CompositeScript.Scripts.Add(New ScriptReference("~/support/js/wf/js_wmContent.js"))



        Page.Title = "CMS :: " + Session("domain") + " :: Web Module :: Content"
        Dim q_page As String = Request.QueryString("p")

        If Trim(q_page) = "" Then q_page = "1"

        If IsPostBack Then
            Dim _wmContent As String = Request.Form("_wmContent")
            Dim _wmContentDelete As String = Request.Form("_wmContentDelete")

            rbApproved = Request.Form("rbApproved")
            rbPublish = Request.Form("rbPublish")
            rbExpired = Request.Form("rbExpired")
            selContentType = Request.Form("selContentType")
            selImageSetting = Request.Form("selImageSetting")

            selDayContentFr = Request.Form("selDayContentFr")
            selMonthContentFr = Request.Form("selMonthContentFr")
            txtYearContentFr = Request.Form("txtYearContentFr")

            selDayContentTo = Request.Form("selDayContentTo")
            selMonthContentTo = Request.Form("selMonthContentTo")
            txtYearContentTo = Request.Form("txtYearContentTo")

            txtKeyword = Request.Form("txtKeyword")
            txtMetaDescription = Request.Form("txtMetaDescription")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            txtTagName = Request.Form("txtTagName")
            txtTagRef = Request.Form("txtTagRef")

            Dim Hashtable As New Hashtable

            Hashtable("approved") = rbApproved
            Hashtable("publish") = rbPublish
            Hashtable("expired") = rbExpired
            Hashtable("contentType") = selContentType
            Hashtable("imageSetting") = selImageSetting
            Hashtable("dCFr") = selDayContentFr
            Hashtable("mCFr") = selMonthContentFr
            Hashtable("yCFr") = txtYearContentFr
            Hashtable("dCTo") = selDayContentTo
            Hashtable("mCTo") = selMonthContentTo
            Hashtable("yCTo") = txtYearContentTo
            Hashtable("keyword") = txtKeyword
            Hashtable("metaDescription") = txtMetaDescription
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("tagName") = txtTagName
            Hashtable("tagRef") = txtTagRef
            Hashtable("refresh") = 1

            If Trim(_wmContent) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/wmContent.aspx?p=1&", Hashtable))
            End If

            If Trim(_wmContentDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteContent(Session("domainRef").ToString, _wmContentDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/wmContent.aspx?p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/wmContent.aspx?p=" + q_page + "&", Hashtable))
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

                rbApproved = param("approved")
                rbPublish = param("publish")
                rbExpired = param("expired")
                selContentType = param("contentType")
                selImageSetting = param("imageSetting")

                selDayContentFr = param("dCFr")
                selMonthContentFr = param("mCFr")
                txtYearContentFr = param("yCFr")

                selDayContentTo = param("dCTo")
                selMonthContentTo = param("mCTo")
                txtYearContentTo = param("yCTo")
                
                txtKeyword = param("keyword")
                txtMetaDescription = param("metaDescription")
                selSortBy = param("sortBy")
                selSortType = param("sortType")
                isRefresh = param("refresh")

                txtTagName = param("tagName")
                txtTagRef = param("tagRef")
            End If



            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If

            If Trim(selSortBy) = "" Then selSortBy = "0"
            If Trim(selSortType) = "" Then selSortType = "0"
            If Trim(selContentType) = "" Then selContentType = "All"
            If Trim(selImageSetting) = "" Then selImageSetting = "All"

            If Trim(selDayContentFr) = "" Then selDayContentFr = "-"
            If Trim(selMonthContentFr) = "" Then selMonthContentFr = "-"
            If Trim(selDayContentTo) = "" Then selDayContentTo = "-"
            If Trim(selMonthContentTo) = "" Then selMonthContentTo = "-"

            ltrContentType.Text = bindSelContentTypeAll(selContentType)
            ltrImageSetting.Text = bindSelImageSettingAll(selImageSetting)
            spanDayContentFr.InnerHtml = bindDay("ContentFr", selDayContentFr, "eDateContent")
            spanMonthContentFr.InnerHtml = bindMonth("ContentFr", selMonthContentFr, "eDateContent")
            spanDayContentTo.InnerHtml = bindDay("ContentTo", selDayContentTo, "eDateContent")
            spanMonthContentTo.InnerHtml = bindMonth("ContentTo", selMonthContentTo, "eDateContent")
            ltrSortBy.Text = bindSelSortBy(sortByValue, sortByText, selSortBy)
            ltrSortType.Text = bindSelSortType(selSortType)

            If isRefresh = "1" Then
                Dim isApprovedParam As String = "All"
                Select Case rbApproved
                    Case "1" 'all
                        isApprovedParam = "All"
                    Case "2" 'yes
                        isApprovedParam = "1"
                    Case "3" 'no
                        isApprovedParam = "0"
                End Select

                Dim isPublishParam As String = "All"
                Select Case rbPublish
                    Case "1" 'all
                        isPublishParam = "All"
                    Case "2" 'yes
                        isPublishParam = "1"
                    Case "3" 'no
                        isPublishParam = "0"
                End Select

                Dim isExpiredParam As String = "All"
                Select Case rbExpired
                    Case "1" 'all
                        isExpiredParam = "All"
                    Case "2" 'yes
                        isExpiredParam = "1"
                    Case "3" 'no
                        isExpiredParam = "0"
                End Select

                Dim sortByParam As String = sortByDB(selSortBy)
                Dim sortTypeParam As String = sortTypeDB(selSortType)

                Dim contentDateFrParam As String = ""
                Dim contentDateToParam As String = ""

                If Trim(txtYearContentFr) <> "" Then
                    contentDateFrParam = txtYearContentFr + "/" + selMonthContentFr + "/" + selDayContentFr
                End If

                If Trim(txtYearContentTo) <> "" Then
                    contentDateToParam = txtYearContentTo + "/" + selMonthContentTo + "/" + selDayContentTo
                End If

                Dim numOfData As Integer = 0
                bindContent(Session("domainRef").ToString, selContentType, selImageSetting, txtTagRef, contentDateFrParam, contentDateToParam, isApprovedParam, isPublishParam, isExpiredParam, q_page, txtKeyword, txtMetaDescription, sortByParam, sortTypeParam, numOfData)

                Dim Hashtable As New Hashtable

                Hashtable("approved") = rbApproved
                Hashtable("publish") = rbPublish
                Hashtable("expired") = rbExpired
                Hashtable("contentType") = selContentType
                Hashtable("imageSetting") = selImageSetting
                Hashtable("dCFr") = selDayContentFr
                Hashtable("mCFr") = selMonthContentFr
                Hashtable("yCFr") = txtYearContentFr
                Hashtable("dCTo") = selDayContentTo
                Hashtable("mCTo") = selMonthContentTo
                Hashtable("yCTo") = txtYearContentTo
                Hashtable("keyword") = txtKeyword
                Hashtable("metaDescription") = txtMetaDescription
                Hashtable("sortBy") = selSortBy
                Hashtable("sortType") = selSortType
                Hashtable("refresh") = 1
                Hashtable("tagName") = txtTagName
                Hashtable("tagRef") = txtTagRef

                Dim urlPaging As String = ""
                urlPaging = GetEncUrl(rootPath + "wf/wmContent.aspx?p=@p&", Hashtable)

                divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If


            ltrTag.Text = bindTag(Session("domainRef").ToString, "")

        End If



    End Sub

End Class
