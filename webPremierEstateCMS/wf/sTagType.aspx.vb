Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].cls_sTagType


Partial Class wf_sTagType
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtKeyword As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""

    Dim sortByValue() As String = {"0", "1", "2"}
    Dim sortByText() As String = {"-", "Ref", "Tag Type Name"}
    Dim sortByDB() As String = {"-", "tagTypeRef", "tagTypeName"}   

    Private Sub bindContent(ByVal domainRef As String, _
                            ByVal pageNo As Integer, _
                            ByVal keyword As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getTagTypeList(domainRef, keyword, sortBy, sortType)
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

            result.Append("<h6>" + dt.Rows.Count.ToString + " Rows Affected</h6>")

            result.Append("		   <div class=""row"">")
            result.Append("          <div class=""col-md-12"">")
            result.Append("            <div class=""table-wrap mb-30"">")
            result.Append("             <table class=""table table-bordered"" cellpadding=""4"" cellspacing=""1"">")
            result.Append("                 <tr class=""cart_item"">")
            result.Append("                     <th align=""center"">Tag Type Name</th>")
            result.Append("                     <th></th>")
            result.Append("                 </tr>")

            For i = 0 To dt.Rows.Count - 1

                If i + 1 >= firstRow Then
                    If i Mod 2 = 0 Then
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""cart_item"">")
                    Else
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""cart_item"">")
                    End If
                    result.Append("    <td valign=""top""><div id=""divTagTypeName" + i.ToString + """>" + dt.Rows(i).Item("tagTypeName") + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center"">")
                    result.Append("      <a title=""Update"" href=""javascript:showPopup(" + i.ToString + "," + dt.Rows(i).Item("tagTypeRef").ToString + ",'" + rootPath + "wf/sTagTypePopup.aspx?r=" + dt.Rows(i).Item("tagTypeRef").ToString + "');""><img alt=""Update"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_update.png"" /></a>&nbsp;")
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("tagTypeRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("tagTypeName")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
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
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).Services.Add(New ServiceReference("~/wcf/wf/wcf_sTagType.svc"))
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).CompositeScript.Scripts.Add(New ScriptReference("~/support/js/wf/js_sTagType.js"))



        Page.Title = "CMS :: " + Session("domain") + " :: Setting :: Tag Type"
        Dim q_page As String = Request.QueryString("p")

        If Trim(q_page) = "" Then q_page = "1"

        If IsPostBack Then
            Dim _sTagType As String = Request.Form("_sTagType")
            Dim _sTagTypeDelete As String = Request.Form("_sTagTypeDelete")

            txtKeyword = Request.Form("txtKeyword")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            Dim Hashtable As New Hashtable

            Hashtable("keyword") = txtKeyword
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("refresh") = 1

            If Trim(_sTagType) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/sTagType.aspx?p=1&", Hashtable))
            End If

            If Trim(_sTagTypeDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteTagType(Session("domainRef").ToString, _sTagTypeDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sTagType.aspx?p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sTagType.aspx?p=" + q_page + "&", Hashtable))
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
                selSortBy = param("sortBy")
                selSortType = param("sortType")
                isRefresh = param("refresh")
            End If

            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If

            If Trim(selSortBy) = "" Then selSortBy = "0"
            If Trim(selSortType) = "" Then selSortType = "0"

            ltrSortBy.Text = bindSelSortBy(sortByValue, sortByText, selSortBy)
            ltrSortType.Text = bindSelSortType(selSortType)

            If isRefresh = "1" Then

                Dim sortByParam As String = sortByDB(selSortBy)
                Dim sortTypeParam As String = sortTypeDB(selSortType)

                Dim numOfData As Integer = 0
                bindContent(Session("domainRef").ToString, q_page, txtKeyword, sortByParam, sortTypeParam, numOfData)

                Dim Hashtable As New Hashtable

                Hashtable("keyword") = txtKeyword
                Hashtable("sortBy") = selSortBy
                Hashtable("sortType") = selSortType
                Hashtable("refresh") = "1"

                Dim urlPaging As String = ""
                urlPaging = GetEncUrl(rootPath + "wf/sTagType.aspx?p=@p&", Hashtable)

                divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If




        End If



    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

End Class
