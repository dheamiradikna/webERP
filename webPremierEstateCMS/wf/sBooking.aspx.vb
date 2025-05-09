Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_sBooking

Partial Class wf_sBooking
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtKeyword As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""

    Dim sortByValue() As String = {"0", "1", "2"}
    Dim sortByText() As String = {"-", "Email", "Name"}
    Dim sortByDB() As String = {"inputTime", "email", "name"}

    Private Sub bindContent(ByVal domainRef As String, _
                            ByVal pageNo As Integer, _
                            ByVal keyword As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getBookingList(domainRef, keyword, sortBy, sortType)
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
            result.Append("    <td></td>")
            result.Append("    <td></td>")
            result.Append("    <td align=""center"">Name</td>")
            result.Append("    <td align=""center"">Contact Person</td>")
            result.Append("    <td align=""center"">Email</td>")
            result.Append("    <td align=""center"">Address</td>")
            result.Append("    <td align=""center"">Order Date</td>")
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
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("bookingRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("email") + "-" + dt.Rows(i).Item("name")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
                    result.Append("    </td>")
                    result.Append("    <td valign=""top""><div id=""divName" + i.ToString + """>" + dt.Rows(i).Item("Name") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divContact" + i.ToString + """>" + dt.Rows(i).Item("contactPerson") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divEmail" + i.ToString + """>" + dt.Rows(i).Item("email") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divAddress" + i.ToString + """>" + dt.Rows(i).Item("address") + "</div></td>")
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
                    result.Append("    <td colspan=""5"">")

                    result.Append(bindMessage(dt.Rows(i).Item("contentRef").ToString, dt.Rows(i).Item("qty").ToString, dt.Rows(i).Item("price").ToString, dt.Rows(i).Item("shippingType").ToString, dt.Rows(i).Item("time").ToString, dt.Rows(i).Item("moment").ToString, dt.Rows(i).Item("unitMansion").ToString, dt.Rows(i).Item("unitBlok").ToString, dt.Rows(i).Item("unitNo").ToString, dt.Rows(i).Item("decName").ToString, dt.Rows(i).Item("note").ToString))

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


            result.Append("</table>")

        Else

            result.Append("<div class=""fNotif"">* No data available</div>")


        End If

        divTable.InnerHtml = result.ToString


    End Sub

    Private Function bindMessage(ByVal contentRef As String, ByVal qty As String, ByVal price As String, ByVal shippingType As String, ByVal time As String, ByVal moment As String, _
                                 ByVal unitMansion As String, ByVal unitBlok As String, ByVal unitNo As String, ByVal decName As String, ByVal note As String) As String
        Dim result As New StringBuilder
        Dim dt As New DataTable
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Item Name : </span><span style=""margin-left:5px;"">" + getItemName(contentRef.ToString()) + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Qty : </span><span style=""margin-left:5px;"">" + qty.ToString() + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">price : </span><span style=""margin-left:5px;"">" + price.ToString() + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Shipping Type : </span><span style=""margin-left:5px;"">" + getShippingName(shippingType.ToString()) + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Time : </span><span style=""margin-left:5px;"">" + time.ToString() + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Moment : </span><span style=""margin-left:5px;"">" + getMomentName(moment.ToString()) + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Unit : </span><span style=""margin-left:5px;"">" + unitMansion.ToString() + " Blok " + unitBlok.ToString() + " - " + unitNo.ToString() + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Deceased Name : </span><span style=""margin-left:5px;"">" + decName.ToString() + "</span>")
        result.Append("</div>")
        result.Append("<div class=""mb10"">")
        result.Append("<span style=""font-size:12px;font-weight:bold"">Message : </span><br/>")
        result.Append("<div style=""margin-top:5px;margin-left:10px;max-width:500px;"">" + note.ToString() + "</div>")
        result.Append("</div>")
        Return result.ToString()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = "CMS :: " + Session("domain") + " :: Setting :: Booking"
        Dim q_page As String = Request.QueryString("p")

        If Trim(q_page) = "" Then q_page = "1"

        If IsPostBack Then
            Dim _sBooking As String = Request.Form("_sBooking")
            Dim _sBookingDelete As String = Request.Form("_sBookingDelete")

            txtKeyword = Request.Form("txtKeyword")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            Dim Hashtable As New Hashtable

            Hashtable("keyword") = txtKeyword
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("refresh") = 1

            If Trim(_sBooking) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/sBooking.aspx?p=1&", Hashtable))
            End If

            If Trim(_sBookingDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteBooking(Session("domainRef").ToString, _sBookingDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sBooking.aspx?p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sBooking.aspx?p=" + q_page + "&", Hashtable))
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
                urlPaging = GetEncUrl(rootPath + "wf/sBooking.aspx?p=@p&", Hashtable)

                divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

End Class
