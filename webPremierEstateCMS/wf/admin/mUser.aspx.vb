Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].cls_mUser

Partial Class wf_admin_mUser
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtKeyword As String = ""
    Protected selSortBy As String = ""
    Protected selSortType As String = ""
    Protected q_domainRef As String = ""

    Dim sortByValue() As String = {"0", "1", "2", "3", "4", "5"}
    Dim sortByText() As String = {"-", "Ref", "Email", "Name", "HP", "Phone"}
    Dim sortByDB() As String = {"-", "userRef", "email", "name", "HP", "phone"}

    Private Sub bindSubTitle(ByVal domainRef As String, ByVal userStatus As String)
        Dim result As New StringBuilder
        Dim dt As New DataTable

        dt = getUserStatusListLookup()
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim strUserCount As String = ""

                If dt.Rows(i).Item("userStatus") = _defUserStatus Then
                    strUserCount = " (" + countUserByStatus(domainRef, dt.Rows(i).Item("userStatus")).ToString + ")"
                End If
                If i > 0 Then
                    result.Append("&nbsp;&nbsp;|+|&nbsp;&nbsp;")
                End If
                If userStatus = dt.Rows(i).Item("userStatus") Then
                    result.Append("<a href=""" + rootPath + "wf/admin/mUser.aspx?d=" + domainRef + "&s=" + dt.Rows(i).Item("userStatus") + """><span class=""bdrLink"">" + dt.Rows(i).Item("userStatusName") + strUserCount + "</span></a>")
                Else
                    result.Append("<a href=""" + rootPath + "wf/admin/mUser.aspx?d=" + domainRef + "&s=" + dt.Rows(i).Item("userStatus") + """>" + dt.Rows(i).Item("userStatusName") + strUserCount + "</a>")
                End If
            Next
        End If

        divSubTitle.InnerHtml = result.ToString
    End Sub

    Private Sub bindContent(ByVal domainRef As String, ByVal userStatus As String, _
                            ByVal pageNo As Integer, _
                            ByVal keyword As String, _
                            ByVal sortBy As String, ByVal sortType As String, ByRef numOfData As Integer)
        Dim dt As New DataTable
        Dim result As New StringBuilder
        Dim i As Integer
        Dim rowCountPage As Integer = _rowCountDefault


        dt = getUserList(domainRef, userStatus, keyword, sortBy, sortType)
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
            result.Append("	<section class=""section-wrap shopping-cart pt-0"">")
            result.Append("      <div class=""container relative"">")

            result.Append("<div class=""alert alert-success fade in alert-dismissible"">" + dt.Rows.Count.ToString + " rows affected</div>")

            result.Append("		   <div class=""row"">")
            result.Append("          <div class=""col-md-12"">")
            result.Append("            <div class=""table-wrap mb-30"">")
            result.Append("              <table class=""table table-bordered"" cellpadding=""4"" cellspacing=""1"">")
            result.Append("                 <tr class=""tblRowHeader"">")
            result.Append("                     <td align=""center"">Email / Username</td>")
            result.Append("                     <td align=""center"">Password</td>")
            result.Append("                     <td align=""center"">Name</td>")
            result.Append("                     <td align=""center"">HP</td>")
            result.Append("                     <td align=""center"">Phone</td>")
            result.Append("                      <td></td>")
            result.Append("                  </tr>")

            For i = 0 To dt.Rows.Count - 1

                If i + 1 >= firstRow Then
                    If i Mod 2 = 0 Then
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow1"">")
                    Else
                        result.Append("  <tr id=""tr" + i.ToString + """ class=""tblRow2"">")
                    End If
                    result.Append("    <td valign=""top""><div id=""divEmail" + i.ToString + """>" + dt.Rows(i).Item("email") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divPassword" + i.ToString + """>" + dt.Rows(i).Item("password") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divName" + i.ToString + """>" + dt.Rows(i).Item("name") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divHP" + i.ToString + """>" + dt.Rows(i).Item("HP") + "</div></td>")
                    result.Append("    <td valign=""top""><div id=""divPhone" + i.ToString + """>" + dt.Rows(i).Item("phone") + "</div></td>")
                    result.Append("    <td valign=""top"" align=""center"">")
                    result.Append("      <a title=""Update"" href=""javascript:showPopup(" + i.ToString + "," + dt.Rows(i).Item("userRef").ToString + ",'" + rootPath + "wf/admin/mUserPopup.aspx?d=" + domainRef + "&r=" + dt.Rows(i).Item("userRef").ToString + "');""><img alt=""Update"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_update.png"" /></a>&nbsp;")
                    result.Append("      <a title=""Delete"" href=""javascript:doDelete('" + dt.Rows(i).Item("userRef").ToString + "','" + MyURLEncode(dt.Rows(i).Item("email")) + "');""><img alt=""Delete"" height=""13"" border=""0"" src=""" + rootPath + "support/image/icon_delete.png"" /></a>")
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).Services.Add(New ServiceReference("~/wcf/wf/admin/wcf_mUser.svc"))
        CType(Master.FindControl("SMMain"), AjaxControlToolkit.ToolkitScriptManager).CompositeScript.Scripts.Add(New ScriptReference("~/support/js/wf/admin/js_mUser.js"))

        Dim q_page As String = Request.QueryString("p")
        q_domainRef = Request.QueryString("d")
        Dim q_userStatus As String = Request.QueryString("s")

        If Trim(q_page) = "" Then q_page = "1"
        If Trim(q_domainRef) = "" Then q_domainRef = getDomainRefByName(_defDomain)
        If Trim(q_userStatus) = "" Then q_userStatus = _defUserStatus

        If IsPostBack Then
            Dim _mUser As String = Request.Form("_mUser")
            Dim _mUserDelete As String = Request.Form("_mUserDelete")

            txtKeyword = Request.Form("txtKeyword")
            selSortBy = Request.Form("selSortBy")
            selSortType = Request.Form("selSortType")

            Dim Hashtable As New Hashtable

            Hashtable("keyword") = txtKeyword
            Hashtable("sortBy") = selSortBy
            Hashtable("sortType") = selSortType
            Hashtable("refresh") = 1

            If Trim(_mUser) = "1" Then
                Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mUser.aspx?d=" + q_domainRef + "&s=" + q_userStatus + "&p=1&", Hashtable))
            End If

            If Trim(_mUserDelete) <> "" Then
                Dim temp As String = ""

                temp = deleteUser(q_domainRef, _mUserDelete)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "1 record deleted."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mUser.aspx?d=" + q_domainRef + "&s=" + q_userStatus + "&p=" + q_page + "&", Hashtable))
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error, please try again."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/admin/mUser.aspx?d=" + q_domainRef + "&s=" + q_userStatus + "&p=" + q_page + "&", Hashtable))
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

            bindSubTitle(q_domainRef, q_userStatus)
            Dim firstDomainRef As String = ""
            ltrDomain.Text = bindDomainLoad(q_domainRef, firstDomainRef)
            ltrSortBy.Text = bindSelSortBy(sortByValue, sortByText, selSortBy)
            ltrSortType.Text = bindSelSortType(selSortType)

            If Trim(q_domainRef) = "" Then q_domainRef = firstDomainRef

            If isRefresh = "1" Then

                Dim sortByParam As String = sortByDB(selSortBy)
                Dim sortTypeParam As String = sortTypeDB(selSortType)

                Dim numOfData As Integer = 0
                bindContent(q_domainRef, q_userStatus, q_page, txtKeyword, sortByParam, sortTypeParam, numOfData)

                Dim Hashtable As New Hashtable

                Hashtable("keyword") = txtKeyword
                Hashtable("sortBy") = selSortBy
                Hashtable("sortType") = selSortType
                Hashtable("refresh") = "1"

                Dim urlPaging As String = ""
                urlPaging = GetEncUrl(rootPath + "wf/admin/mUser.aspx?d=" + q_domainRef + "&s=" + q_userStatus + "&p=@p&", Hashtable)

               divPaging.InnerHtml = bindPaging(_rowCountDefault, q_page, numOfData, urlPaging)
            End If




        End If


        
    End Sub
End Class
