Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_sFeedback

Partial Class wf_sFeedback
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtKeyword As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""

    Dim sortByValue() As String = {"0", "1", "2"}
    Dim sortByText() As String = {"-", "Email", "Subject"}
    Dim sortByDB() As String = {"-", "email", "subject"}

    Private Sub bindContent(ByVal domainRef As String, _
                            ByVal pageNo As Integer, _
                            ByVal keyword As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getFeedbackList(domainRef, keyword, sortBy, sortType)
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

            result.Append("<table class=""table table-bordered"" cellpadding=""4"" cellspacing=""1"" style=""width:98%;margin-top: 20px;"">")
            result.Append("  <tr class=""tblRowHeader"">")
            result.Append("    <td></td>")
            result.Append("    <td></td>")
            result.Append("    <td align=""center"">Name</td>")
            result.Append("    <td align=""center"">Email</td>")
            result.Append("    <td align=""center"">Phone</td>")
            result.Append("    <td align=""center"">Message</td>")
            result.Append("    <td align=""center"">Input Time</td>")
            result.Append("  </tr>")

            For i = 0 To dt.Rows.Count - 1

                If i + 1 >= firstRow Then
                    Dim strRowClass As String = ""

                    If i Mod 2 = 0 Then
                        strRowClass = "tblRow1"
                    Else
                        strRowClass = "tblRow2"
                    End If

                    result.Append("<tr class=""" + strRowClass + """>")
                    result.Append("    <td valign=""top"" align=""center""><div id=""divDetailPanel" + i.ToString + """><a href=""javascript:openDetail(" + i.ToString + ");"">Message</a></div></td>")
                    result.Append("    <td>")
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("feedbackRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("name")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
                    result.Append("    </td>")
                    result.Append("    <td valign=""top""><div id=""divName" + i.ToString + """>" + dt.Rows(i).Item("Name") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divEmail" + i.ToString + """>" + dt.Rows(i).Item("Email") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divmobilePhone" + i.ToString + """>" + dt.Rows(i).Item("mobilePhone").ToString + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divMessage" + i.ToString + """>" + dt.Rows(i).Item("message") + "</div></td>")
                  
                    result.Append("    <td valign=""top""><div id=""divReceivedDate" + i.ToString + """>" + Format(dt.Rows(i).Item("InputTime"), "dd MMM yyyy hh:mm") + "</div></td>")
                    result.Append("  </tr>")

                    '''''''''''''' Bind Detail ''''''''''''''
                    '''''''''''''' Bind Detail ''''''''''''''
                    '''''''''''''' Bind Detail ''''''''''''''
                    '''''''''''''' Bind Detail ''''''''''''''

                    If i Mod 2 = 0 Then
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow1"" style=""display:none;"">")
                    Else
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow2"" style=""display:none;"">")
                    End If
                    result.Append("    <td></td>")
                    result.Append("    <td></td>")
                    result.Append("    <td colspan=""4"">")

                    result.Append(bindMessage(dt.Rows(i).Item("message").ToString))

                    result.Append("    </td>")
                    result.Append("  </tr>")
                    '''''''''''''' Bind Detail END ''''''''''''''
                    '''''''''''''' Bind Detail END ''''''''''''''
                    '''''''''''''' Bind Detail END ''''''''''''''
                    '''''''''''''' Bind Detail END ''''''''''''''

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

    Private Function bindMessage(ByVal message As String) As String
        Dim result As New StringBuilder
        Dim dt As New DataTable
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Message : </span><br/>")
        result.Append("<div style=""margin-top:5px;margin-left:10px;max-width:500px;"">" + message.ToString() + "</div>")
        result.Append("</div>")
        Return result.ToString()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = "CMS :: " + Session("domain") + " :: Setting :: Feedback"
        Dim q_page As String = Request.QueryString("p")

        If Trim(q_page) = "" Then q_page = "1"

        If IsPostBack Then
            Dim _sFeedback As String = Request.Form("_sFeedback")
            Dim _sFeedbackDelete As String = Request.Form("_sFeedbackDelete")

            txtKeyword = Request.Form("txtKeyword")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            Dim Hashtable As New Hashtable

            Hashtable("keyword") = txtKeyword
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("refresh") = 1

            If Trim(_sFeedback) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/sFeedback.aspx?p=1&", Hashtable))
            End If

            If Trim(_sFeedbackDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteFeedback(Session("domainRef").ToString, _sFeedbackDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sFeedback.aspx?p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sFeedback.aspx?p=" + q_page + "&", Hashtable))
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
                urlPaging = GetEncUrl(rootPath + "wf/sFeedback.aspx?p=@p&", Hashtable)

                divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

End Class
