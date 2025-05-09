Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].cls_sTag


Partial Class wc_blockHeader
    Inherits System.Web.UI.UserControl

    Protected rootPath As String = _rootPath
    Protected domainName As String = ""

    Private Sub bindMenuTag(ByVal domainRef As String)
        Dim dt As New DataTable
        Dim result As New StringBuilder

        dt = getTagTypeListLookup(domainRef)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                result.Append("<li><a href=""#"">" + dt.Rows(i).Item("tagTypeName") + "</a>")
                result.Append(" <ul>")
                Dim dtT As New DataTable

                dtT = getTagTreeList(domainRef, dt.Rows(i).Item("tagTypeRef"), "0", "1", "", "-", "-")
                If dtT.Rows.Count > 0 Then

                    For t = 0 To dtT.Rows.Count - 1
                        If dtT.Rows(t).Item("isOnlyParent") = "1" Then
                            result.Append("  <li>" + "<a href=""#"">")
                        Else
                            If dtT.Rows(t).Item("isSingleContent") = "1" Then
                                result.Append("  <li>" + "<a href=""" + rootPath + "wf/contentTagSingle.aspx?tr=" + dtT.Rows(t).Item("tagRef") + """>")
                            Else
                                result.Append("  <li>" + "<a href=""" + rootPath + "wf/contentTag.aspx?tr=" + dtT.Rows(t).Item("tagRef") + """>")
                            End If
                        End If


                        For l = 1 To CInt(dtT.Rows(t).Item("level")) - 1
                            result.Append("&nbsp;&nbsp;&nbsp;")
                        Next
                        result.Append(dtT.Rows(t).Item("tagName") + "</a></li>")
                    Next
                End If

                result.Append(" </ul>")
                result.Append("</li>")
            Next

        End If

        ltrTag.Text = result.ToString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        bindMenuTag(Session("domainRef").ToString)

        domainName = "www." + Replace(Replace(Session("domain").ToString, "http://", ""), "www.", "")


    End Sub
End Class
