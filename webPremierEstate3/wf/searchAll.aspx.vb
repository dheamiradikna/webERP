Imports System.Data
Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].clsContentDB
Imports [class].clsGeneral
Imports System.IO
Imports Aspose.Email.Mail
Imports Aspose.Email
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports [class]
Partial Class wf_searchAll
    Inherits System.Web.UI.Page
    Protected rootPath As String = _rootPath
    Protected rootPathCMS As String = _rootPathCMS

    Protected keyword As String = ""
    Protected topSearch As String = "5"
    Protected tagTypeRefContent As String = _tagTypeRefContent

    Private Sub wf_searchAll_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim q_x As String = Request.Params("x")
        Dim q_tagTypeRef As String = tagTypeRefContent
        Dim q_contentRef As String = "2"
        Dim q_ref As String = Request.QueryString("r")
        Dim q_page As String = Request.QueryString("p")

        If Trim(q_x) <> "" Then
            Dim param As Hashtable = GetDecParam(q_x)
            keyword = param("keyword")
        End If

        If IsPostBack Then


        Else

            'notpostback
            ltrContent.Text = bindSearchArticle(q_page, q_tagTypeRef)

        End If


    End Sub

    Private Function bindSearchArticle(ByVal pageNo As String, ByVal tagtypeRef As String) As String

        Dim result As New StringBuilder
        Dim dtImage As New DataTable
        Dim dtArtikelLvl3 As New DataTable

        dtArtikelLvl3 = getArtikelSearch(topSearch, keyword, tagtypeRef)

        result.Append("        <section style=""padding-top: 0px !important; padding-bottom: 0px !important;"" class=""services padding-v100 top-posts"">")
        result.Append("             <div class=""container"">")
        result.Append("                  <h3 style=""text-transform: capitalize;"">Hasil Pencarian <i>Keyword " + keyword + "</i></h3>")
        'result.Append("                  <div class=""row"" style=""margin-right: 45%;margin-left: 45%;margin-bottom: 5%;margin-top: -3%;width: 10%;border-bottom: 5px solid #9cc0e5;""></div>")
        result.Append("                       <div class=""row"">")
        result.Append("                            <div class=""col-xs-12 text-center"">")

        If dtArtikelLvl3.Rows.Count > 0 Then

            For j As Integer = 0 To dtArtikelLvl3.Rows.Count - 1
                result.Append("                        <div class=""single-item"" style=""display: inline-block;"">")

                dtImage = getImageRefByContentSearch(dtArtikelLvl3.Rows(j).Item("contentRef").ToString)
                If dtImage.Rows.Count > 0 Then
                    result.Append("                            <a href=""" + _rootPath + convertStrToParam(dtArtikelLvl3.Rows(j).Item("tagName")).ToString + "/" + convertStrToParam(getTitleByContentRef(dtArtikelLvl3.Rows(j).Item("contentRef"))) + """>")
                    result.Append("                                 <img class=""lazyload"" src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/100/0/67"+".jpg"""" data-src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/360/0/240"+".jpg"""" alt=""search blog"">")
                    result.Append("                            </a>")
                End If
                If dtArtikelLvl3.Rows.Count > 0 Then
                    result.Append("                            <div style=""background-color: #ffffff !important;"" class=""single-item-body"">")
                    result.Append("                                        <p style=""margin: -34px 0 6px !important;color: #f5abcb;"" class=""post-category"">" + dtArtikelLvl3.Rows(j).Item("tagName") + "</p>")
                    result.Append("                                        <h3 style=""min-height: 63px !important;display: -webkit-box;-webkit-box-orient: vertical;-webkit-line-clamp: 3;overflow: hidden; margin: 0px 0 50px !important; font-size: 20px !important;"">")

                    'untuk meng-highlight keyword yang di cari di search pada frontend
                    Dim title As String = dtArtikelLvl3.Rows(j).Item("title")

                    For x As Integer = 0 To keyword.Split(" ").Count - 1
                        title = title.ToLower().Replace(keyword.Split(" ")(x), "<span style=""background-color:yellow;"">" + keyword.Split(" ")(x) + "</span>")
                    Next

                    result.Append("                                    <a href=""" + _rootPath + convertStrToParam(dtArtikelLvl3.Rows(j).Item("tagName")).ToString + "/" + convertStrToParam(getTitleByContentRef(dtArtikelLvl3.Rows(j).Item("contentRef"))) + """ style=""text-transform:capitalize;"">" + title + "</a>")
                    result.Append("                                </h3>")
                    result.Append("                            </div>")
                    result.Append("                        </div>")
                End If
            Next
        End If

        result.Append("                    </div>")
        result.Append("                </div>")
        result.Append("            </div>")
        result.Append("        </section>")


        Return result.ToString

    End Function

    Private Function bindContent(ByVal pageNo As String, ByVal tagTypeRef As String, ByVal tagRef As String, ByVal contentRef As String, ByVal projectRef As String) As String 
        Dim result As New StringBuilder

            result.Append(bindSearchArticle(pageNo, tagtypeRef))

        Return result.ToString()
    End Function


End Class
