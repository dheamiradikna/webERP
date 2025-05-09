Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].cls_sTag

Partial Class wf_sTag
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected rbActive As String = ""
    Protected selTagType As String = ""
    Protected selParentTag As String = ""
    Protected txtKeyword As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""

    Dim sortByValue() As String = {"0", "1", "2", "3", "4"}
    Dim sortByText() As String = {"-", "Ref", "Tag Type Name", "Tag Name", "Is Active"}
    Dim sortByDB() As String = {"sortNo", "tagRef", "tagTypeName", "tagName", "isActive"}

    Private Function bindSelTagTypeAll(ByVal domainRef As String, ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getTagTypeListLookup(domainRef)
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selTagType"" name=""selTagType"" >")
            If value = "All" Then
                result.Append("<option selected value=""All"">All</option>")
            Else
                result.Append("<option value=""All"">All</option>")
            End If
            For i = 0 To dt.Rows.Count - 1
                
                If value = dt.Rows(i).Item("tagTypeRef").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("tagTypeRef").ToString + """>" + dt.Rows(i).Item("tagTypeName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("tagTypeRef").ToString + """>" + dt.Rows(i).Item("tagTypeName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Function bindSelParentAll(ByVal domainRef As String, ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        result.Append("<select id=""selParentTag"" name=""selParentTag"" >")
        If value = "0" Then
            result.Append("<option selected value=""0"">All</option>")
        Else
            result.Append("<option value=""0"">All</option>")
        End If

        dt = getTagTreeList(domainRef, "", "0", "1", "", "-", "-")
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim tagName As String = ""
                For j = 1 To CInt(dt.Rows(i).Item("level")) - 1
                    tagName = tagName + "&nbsp;&nbsp;&nbsp;"
                Next
                tagName = tagName + dt.Rows(i).Item("tagName")

                If value = dt.Rows(i).Item("tagRef").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("tagRef").ToString + """>" + tagName + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("tagRef").ToString + """>" + tagName + "</option>")
                End If
            Next
        End If

        result.Append("</select> ")

        Return result.ToString
    End Function

    Private Sub bindContent(ByVal domainRef As String, ByVal tagTypeRef As String, _
                            ByVal parentTag As String, ByVal isActive As String, _
                            ByVal pageNo As Integer, _
                            ByVal keyword As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getTagTreeList(domainRef, tagTypeRef, parentTag, isActive, keyword, sortBy, sortType)
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
            'result.Append("<div class=""fNotif mb5"">" + dt.Rows.Count.ToString + " rows affected</div>")
            result.Append("	<section class=""section shopping-cart pt-0"">")
            result.Append("      <div class=""container relative"">")

            result.Append("     <h6>" + dt.Rows.Count.ToString + " Rows Affected</h6>")
            result.Append("		   <div class=""row"">")
            result.Append("          <div class=""col-md-12"">")
            result.Append("            <div class=""table-wrap mb-30"">")
            result.Append("              <table class=""table table-bordered"" cellpadding=""4"" cellspacing=""1"">")
            result.Append("                 <tr class=""cart_item"">")
            result.Append("                     <td align=""center"">Tag Type Name</td>")
            result.Append("                     <td align=""center"">Tag Name</td>")
            result.Append("                     <td align=""center"">Is Active</td>")
            result.Append("                     <td align=""center"">Level</td>")
            result.Append("                     <td></td>")
            result.Append("                 </tr>")

            For i = 0 To dt.Rows.Count - 1

                If i + 1 >= firstRow Then
                    Dim strActive As String = ""
                    If dt.Rows(i).Item("isActive") = "1" Then
                        strActive = "<img height=""13"" src=""" + rootPath + "support/image/icon_yes.png"" />"
                    Else
                        strActive = "<img height=""13"" src=""" + rootPath + "support/image/icon_no.png"" />"
                    End If

                    Dim tagName As String = ""
                    For j = 1 To CInt(dt.Rows(i).Item("level")) - 1
                        tagName = tagName + "&nbsp;&nbsp;&nbsp;"
                    Next
                    tagName = tagName + dt.Rows(i).Item("tagName")

                    If i Mod 2 = 0 Then
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""cart_item"">")
                    Else
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""cart_item"">")
                    End If
                    result.Append("    <td valign=""top""><div id=""divTagTypeName" + i.ToString + """>" + dt.Rows(i).Item("tagTypeName") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divTagName" + i.ToString + """>" + tagName + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divActive" + i.ToString + """>" + strActive + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divLevel" + i.ToString + """>" + dt.Rows(i).Item("level").ToString + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center"">")
                    result.Append("      <a title=""Update"" href=""javascript:showPopup(" + i.ToString + "," + dt.Rows(i).Item("tagRef").ToString + ",'" + rootPath + "wf/sTagPopup.aspx?r=" + dt.Rows(i).Item("tagRef").ToString + "');""><img alt=""Update"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_update.png"" /></a>&nbsp;")
                    'result.Append("      <a title=""Update"" href=""" + rootPath + "wf/sTagInput.aspx?r=" + dt.Rows(i).Item("tagRef").ToString + """><img alt=""Update"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_update.png"" /></a>&nbsp;")
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("tagRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("tagName")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
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
            result.Append("           </table>")
            result.Append("          </div> ")
            result.Append("        </div> ")

            result.Append("	</div>")
            result.Append("</section>")
        Else
            result.Append("<div class=""fNotif"">* No data available</div>")
        End If
        divTable.InnerHtml = result.ToString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).Services.Add(New ServiceReference("~/wcf/wf/wcf_sTag.svc"))
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).CompositeScript.Scripts.Add(New ScriptReference("~/support/js/wf/js_sTag.js"))



        Page.Title = "CMS :: " + Session("domain") + " :: Setting :: Tag"
        Dim q_page As String = Request.QueryString("p")

        If Trim(q_page) = "" Then q_page = "1"

        If IsPostBack Then
            Dim _sTag As String = Request.Form("_sTag")
            Dim _sTagDelete As String = Request.Form("_sTagDelete")

            rbActive = Request.Form("rbActive")
            selTagType = Request.Form("selTagType")
            selParentTag = Request.Form("selParentTag")
            txtKeyword = Request.Form("txtKeyword")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            Dim Hashtable As New Hashtable

            Hashtable("active") = rbActive
            Hashtable("tagType") = selTagType
            Hashtable("parentTag") = selParentTag
            Hashtable("keyword") = txtKeyword
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("refresh") = 1

            If Trim(_sTag) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/sTag.aspx?p=1&", Hashtable))
            End If

            If Trim(_sTagDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteTag(Session("domainRef").ToString, _sTagDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sTag.aspx?p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sTag.aspx?p=" + q_page + "&", Hashtable))
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
                rbActive = param("active")
                selTagType = param("tagType")
                selParentTag = param("parentTag")
                txtKeyword = param("keyword")
                selSortBy = param("sortBy")
                selSortType = param("sortType")
                isRefresh = param("refresh")
            End If

            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If

            If Trim(selSortBy) = "" Then selSortBy = "0"
            If Trim(selSortType) = "" Then selSortType = "0"
            If Trim(selTagType) = "" Then selTagType = "All"
            If Trim(selParentTag) = "" Then selParentTag = "0"


            ltrTagType.Text = bindSelTagTypeAll(Session("domainRef").ToString, selTagType)
            ltrParentTag.Text = bindSelParentAll(Session("domainRef").ToString, selParentTag)
            ltrSortBy.Text = bindSelSortBy(sortByValue, sortByText, selSortBy)
            ltrSortType.Text = bindSelSortType(selSortType)

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
                bindContent(Session("domainRef").ToString, selTagType, selParentTag, isActiveParam, q_page, txtKeyword, sortByParam, sortTypeParam, numOfData)

                Dim Hashtable As New Hashtable

                Hashtable("active") = rbActive
                Hashtable("tagType") = selTagType
                Hashtable("parentTag") = selParentTag
                Hashtable("keyword") = txtKeyword
                Hashtable("sortBy") = selSortBy
                Hashtable("sortType") = selSortType
                Hashtable("refresh") = "1"

                Dim urlPaging As String = ""
                urlPaging = GetEncUrl(rootPath + "wf/sTag.aspx?p=@p&", Hashtable)

                divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If




        End If



    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

End Class
