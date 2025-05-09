Imports Microsoft.VisualBasic
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].clsContentDB
Imports [class].clsWebGeneral
Imports System.Data


Public Class clsURLRewrite

    Implements IHttpModule

    Protected rootPath As String = Replace(Replace(ConfigurationManager.AppSettings.Item("rootPath").ToString(), "http://www.", ""), "http://", "")

    Public Sub Dispose() Implements System.Web.IHttpModule.Dispose

    End Sub

    Public Sub Init(ByVal context As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init
        AddHandler context.BeginRequest, AddressOf OnBeginRequest
    End Sub

    Private Function generatePath(ByVal url As String, ByVal keyword As String, ByRef param() As String) As String
        Dim x As Integer = url.IndexOf("?")
        If x <> -1 Then
            url = Left(url, x)
        End If

        url = Replace(Replace(url, "http://www.", ""), "http://", "")

        Dim qs As String = Right(url, StrReverse(url).ToLower.IndexOf(StrReverse(rootPath.ToLower + keyword.ToLower)))
        Dim path As String = ""
        Dim i As Integer
        Dim countPath() As String = qs.Split("/")

        qs = Replace(Replace(Trim(qs), "///", ""), "//", "")
        If Right(qs, 1) = "/" Then qs = Left(qs, Len(qs) - 1)

        If Trim(qs) <> "" Then
            If qs.IndexOf("/") <> -1 Then
                param = qs.Split("/")
            End If
        Else
            param = Split(qs, "")
        End If

        For i = 1 To countPath.Length + keyword.Split("/").Length - 2
            path = path + "../"
        Next

        Return path
    End Function

    Private Function generatePathV2(ByVal url As String, ByVal domain As String, ByRef param() As String) As String
        Dim qs As String = Microsoft.VisualBasic.Right(url, StrReverse(url).ToLower.IndexOf(StrReverse(domain.ToLower)))
        Dim path As String = ""
        Dim i As Integer

        param = qs.Split("/")
        For i = 1 To param.Length - 2
            path = path + "../"
        Next

        Return path
    End Function

    Private Sub OnBeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        Dim app As HttpApplication
        app = CType(sender, HttpApplication)
        Dim url As String = app.Request.Url.ToString

        Dim path As String = ""
        Dim param() As String
        Dim contentTag As String = ""

        If url.ToLower.Contains(rootPath.ToLower + "content.images") Then
            path = generatePath(url, "content.images", param) '0

            'http://../../../content.images/person/2/12/0/0/0/1


            Dim q_tagRef As String = ""
            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_domainRef As String = ""
            Dim q_module As String = ""

            If param.Length >= 5 And param(1) = "tagPicture" Then
                If param.Length >= 5 Then q_width = param(4)
                If param.Length >= 4 Then q_tagRef = param(3)
                If param.Length >= 3 Then q_domainRef = param(2)
                If param.Length >= 2 Then q_module = param(1)

                app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&d=" + q_domainRef + "&tr=" + q_tagRef + "&w=" + q_width)

            Else
                If param.Length >= 9 Then q_isFullBackground = param(8)
                If param.Length >= 8 Then q_resizePct = param(7)
                If param.Length >= 7 Then q_height = param(6)
                If param.Length >= 6 Then q_contentRef = param(5)
                If param.Length >= 5 Then q_width = param(4)
                If param.Length >= 4 Then q_imgRef = param(3)
                If param.Length >= 3 Then q_domainRef = param(2)
                If param.Length >= 2 Then q_module = param(1)

                app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&d=" + q_domainRef + "&ir=" + q_imgRef + "&w=" + q_width + "&cr=" + q_contentRef + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)

            End If

        ElseIf url.ToLower.Contains(rootPath.ToLower + "newsEvents.images") Then
            path = generatePath(url, "newsEvents.images", param) '0

            'http://../../../content.images/person/2/12/0/0/0/1


            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_domainRef As String = ""
            Dim q_module As String = ""


            If param.Length >= 9 Then q_isFullBackground = param(8)
            If param.Length >= 8 Then q_resizePct = param(7)
            If param.Length >= 7 Then q_height = param(6)
            If param.Length >= 6 Then q_contentRef = param(5)
            If param.Length >= 5 Then q_width = param(4)
            If param.Length >= 4 Then q_imgRef = param(3)
            If param.Length >= 3 Then q_domainRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&d=" + q_domainRef + "&ir=" + q_imgRef + "&w=" + q_width + "&cr=" + q_contentRef + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)


        ElseIf url.ToLower.Contains(rootPath.ToLower + "covernapro.images") Then
            path = generatePath(url, "covernapro.images", param) '0

            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_module As String = ""


            If param.Length >= 10 Then q_isFullBackground = param(9)
            If param.Length >= 9 Then q_resizePct = param(8)
            If param.Length >= 8 Then q_height = param(7)
            If param.Length >= 7 Then q_contentRef = param(6)
            If param.Length >= 6 Then q_width = param(5)
            If param.Length >= 5 Then q_imgRef = param(4)
            If param.Length >= 4 Then q_projectRef = param(3)
            If param.Length >= 3 Then q_dbMasterRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&d=" + q_dbMasterRef + "&pr=" + q_projectRef + "&ir=" + q_imgRef + "&w=" + q_width + "&cr=" + q_contentRef + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "covernaproproduct.images") Then
            path = generatePath(url, "covernaproproduct.images", param) '0

            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_productRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_module As String = ""


            If param.Length >= 11 Then q_isFullBackground = param(10)
            If param.Length >= 10 Then q_resizePct = param(9)
            If param.Length >= 9 Then q_height = param(8)
            If param.Length >= 8 Then q_contentRef = param(7)
            If param.Length >= 7 Then q_width = param(6)
            If param.Length >= 6 Then q_imgRef = param(5)
            If param.Length >= 5 Then q_productRef = param(4)
            If param.Length >= 4 Then q_projectRef = param(3)
            If param.Length >= 3 Then q_dbMasterRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&d=" + q_dbMasterRef + "&pr=" + q_projectRef + "&prod=" + q_productRef + "&ir=" + q_imgRef + "&w=" + q_width + "&cr=" + q_contentRef + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "covernaproproduct1.images") Then
            path = generatePath(url, "covernaproproduct1.images", param) '0

            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_productRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_module As String = ""


            If param.Length >= 9 Then q_isFullBackground = param(8)
            If param.Length >= 8 Then q_resizePct = param(7)
            If param.Length >= 7 Then q_height = param(6)
            If param.Length >= 6 Then q_contentRef = param(5)
            If param.Length >= 5 Then q_width = param(4)
            If param.Length >= 4 Then q_imgRef = param(3)
            If param.Length >= 3 Then q_productRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&prod=" + q_productRef + "&ir=" + q_imgRef + "&w=" + q_width + "&cr=" + q_contentRef + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "napro.images") Then
            path = generatePath(url, "napro.images", param) '0

            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_productRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_module As String = ""

            If param.Length >= 8 Then q_isFullBackground = param(7)
            If param.Length >= 7 Then q_resizePct = param(6)
            If param.Length >= 6 Then q_height = param(5)
            If param.Length >= 5 Then q_width = param(4)
            If param.Length >= 4 Then q_contentRef = param(3)
            If param.Length >= 3 Then q_imgRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&ir=" + q_imgRef + "&cr=" + q_contentRef + "&w=" + q_width + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "gallerynapro.images") Then
            path = generatePath(url, "gallerynapro.images", param) '0

            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_imgRef As String = ""
            Dim q_contentRef As String = ""
            Dim q_imgGalleryRef As String = ""
            Dim q_module As String = ""


            If param.Length >= 9 Then q_isFullBackground = param(8)
            If param.Length >= 8 Then q_resizePct = param(7)
            If param.Length >= 7 Then q_height = param(6)
            If param.Length >= 6 Then q_contentRef = param(5)
            If param.Length >= 5 Then q_width = param(4)
            If param.Length >= 4 Then q_imgRef = param(3)
            If param.Length >= 3 Then q_imgGalleryRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_module + "&igr=" + q_imgGalleryRef + "&ir=" + q_imgRef + "&w=" + q_width + "&cr=" + q_contentRef + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)


        ElseIf url.ToLower.Contains(rootPath.ToLower + "tag.images") Then
            path = generatePath(url, "tag.images", param) '0

            'http://../../../content.images/person/2/12/0/0/0/1

            Dim q_isFullBackground As String = ""
            Dim q_resizePct As String = ""
            Dim q_height As String = ""
            Dim q_width As String = ""
            Dim q_tagRef As String = ""
            Dim q_domainRef As String = ""
            Dim q_module As String = ""

            If param.Length >= 8 Then q_isFullBackground = param(7)
            If param.Length >= 7 Then q_resizePct = param(6)
            If param.Length >= 6 Then q_height = param(5)
            If param.Length >= 5 Then q_width = param(4)
            If param.Length >= 4 Then q_tagRef = param(3)
            If param.Length >= 3 Then q_domainRef = param(2)
            If param.Length >= 2 Then q_module = param(1)

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "d=" + q_domainRef + "&m=" + q_module + "&tr=" + q_tagRef + "&w=" + q_width + "&h=" + q_height + "&pct=" + q_resizePct + "&isBg=" + q_isFullBackground)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "download") Then
            path = generatePath(url, "download", param)
            Dim q_domainRef As String = ""
            Dim q_contentRef As String = ""

            If param.Length >= 3 Then q_contentRef = param(2)
            If param.Length >= 2 Then q_domainRef = param(1)

            app.Context.RewritePath(path + "wf/support/displayAttachmentDownload.aspx", "", "dr=" + q_domainRef + "&cr=" + q_contentRef)

            'ElseIf url.ToLower.Contains(rootPath.ToLower + "download") Then
            '    path = generatePath(url, "download", param)

            '    Dim q_extension As String = ""
            '    Dim q_dbMasterProjectFileRef As String = ""
            '    Dim q_projectRef As String = ""
            '    Dim q_dbMasterRef As String = ""

            '    If param.Length >= 5 Then q_extension = param(4)
            '    If param.Length >= 4 Then q_dbMasterProjectFileRef = param(3)
            '    If param.Length >= 3 Then q_projectRef = param(2)
            '    If param.Length >= 2 Then q_dbMasterRef = param(1)

            '    app.Context.RewritePath(path + "wf/support/displayAttachmentDownload.aspx", "", "d=" + q_dbMasterRef + "&pr=" + q_projectRef + "&fr=" + q_dbMasterProjectFileRef + "&ex=" + q_extension)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "unduh") Then
            path = generatePath(url, "unduh", param)

            Dim q_contentRef As String = ""
            Dim q_domainRef As String = ""

            If param.Length >= 3 Then q_contentRef = param(2)
            If param.Length >= 2 Then q_domainRef = param(1)

            app.Context.RewritePath(path + "wf/support/displayAttachmentDownload.aspx", "", "dr=" + q_domainRef + "&cr=" + q_contentRef)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "content/content") Then
            path = generatePath(url, "content/content", param)

            Dim q_clusterRef As String = ""
            Dim q_productRef As String = ""
            Dim q_cr As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_tagRef As String = ""
            Dim q_tagRefNapro As String = ""
            Dim clusterRef As String = ""

            Dim dtProjectSettingNapro As New DataTable

            'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)
            q_dbMasterRef = dtProjectSettingNapro.Rows(0).Item("dbMasterRef")
            q_projectRef = dtProjectSettingNapro.Rows(0).Item("projectRef")

            'If param.Length >= 5 And param(1) = "tower" Then
            '   clusterRef = getClusterRef(param(2), rootPath, q_projectRef, q_dbMasterRef, q_clusterRef)
            '    If param.Length >= 5 Then q_clusterRef = q_clusterRef
            '    If param.Length >= 4 Then q_projectRef = param(3)
            '    If param.Length >= 3 Then q_dbMasterRef = param(2)
            '    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
            'Else
            If param.Length >= 6 And param(1) = "what-s-on" Then
                If param.Length >= 6 Then q_cr = param(5)
                If param.Length >= 5 Then q_projectRef = param(4)
                If param.Length >= 4 Then q_dbMasterRef = param(3)
                'If param.Length >= 3 Then getTagRefByURLNapro(param(2), rootPath, q_tagRefNapro)
                If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                'If param.Length >= 5 Then q_cr = param(4)
                'If param.Length >= 4 Then q_projectRef = param(3)
                'If param.Length >= 3 Then q_dbMasterRef = param(2)
                'If param.Length >= 2 Then getTagRefByURLNapro(param(1), rootPath, q_tagRef)
                'getTagRefByURL(param(1), rootPath, q_r)
                'getTagRefByURLNapro(param(3), rootPath, q_tagRefNapro)
                'getContentRefByURLNapro(param(4), rootPath, q_cr)
            Else
                If param.Length >= 3 Then q_cr = param(3)
                If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
            End If


            app.Context.RewritePath(path + "wf/pContentLvl3.aspx", "", "t=" + q_tagRef + "&cr=" + q_cr + "&db=" + q_dbMasterRef + "&pr=" + q_projectRef + "&pf=" + q_productRef + "&cl=" + q_clusterRef + "&tn=" + q_tagRefNapro)

            'content/....
        ElseIf url.ToLower.Contains(rootPath.ToLower + "content") Then
            path = generatePath(url, "content", param)

            Dim q_categoryRef As String = ""
            Dim q_clusterRef As String = ""
            Dim q_productRef As String = ""
            Dim q_cr As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_tagRef As String = ""
            Dim q_tagRefLvl2 As String = ""
            Dim q_tagRefLvl3 As String = ""
            Dim q_tagRefLvl4 As String = ""
            Dim q_tagRefNapro As String = ""
            Dim clusterRef As String = ""
            Dim categoryRef As String = ""
            Dim q_developerRef As String = ""
            Dim q_clusterByCMSMetro As String = ""

            If param.Length = 2 Then
                getTagRefByURL(param(1), rootPath, q_tagRef)
                'ElseIf param.Length = 3 Then
                '    getContentRefByURL(param(2), rootPath, q_cr)
                '    getTagRefByURL(param(1), rootPath, q_tagRef)
            ElseIf param.Length = 4 Then
                getTagRefByURL(param(1), rootPath, q_tagRef)
                If param(2) = "project" Then
                    q_projectRef = param(3)
                ElseIf param(2) = "developer" Then
                    q_developerRef = param(3)
                End If
                getTagRefByURL(param(2), rootPath, q_tagRefLvl2)
                getTagRefByURL(param(1), rootPath, q_tagRef)
            ElseIf param.Length = 5 Then
                getTagRefByURL(param(1), rootPath, q_tagRef)
                getTagRefByURLAndTagRefParent(param(2), q_tagRef, rootPath, q_tagRefLvl2)
                getTagRefByURL(param(3), rootPath, q_tagRefLvl3)
                getTagRefByURLAndTagRefParent(param(4), q_tagRefLvl3, rootPath, q_tagRefLvl4)
            ElseIf param.Length = 6 And param(1) <> "what-s-on" Then
                Dim dtProjectSettingNapro As New DataTable
                'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)
                If dtProjectSettingNapro.Rows.Count > 0 Then
                    q_dbMasterRef = dtProjectSettingNapro.Rows(0).Item("dbMasterRef")
                    q_projectRef = dtProjectSettingNapro.Rows(0).Item("projectRef")
                End If
                getTagRefByURL(param(1), rootPath, q_tagRef)
                getTagRefByURLAndTagRefParent(param(2), q_tagRef, rootPath, q_tagRefLvl2)
                getTagRefByURL(param(3), rootPath, q_tagRefLvl3)
                getCategoryRef(param(4), rootPath, q_projectRef, q_dbMasterRef, q_categoryRef)
                getClusterRef(param(5), rootPath, q_projectRef, q_dbMasterRef, q_categoryRef, q_clusterRef)
            ElseIf param.Length >= 6 And param(1) = "what-s-on" Then
                'getTagRefByURLNapro(param(2), rootPath, q_tagRefNapro)
                If param.Length >= 6 Then q_projectRef = param(5)
                If param.Length >= 5 Then q_dbMasterRef = param(4)
                'If param.Length >= 4 Then getContentRefByURLNapro(param(3), q_tagRefNapro, rootPath, q_cr)
                If param.Length >= 3 Then q_tagRefNapro = q_tagRefNapro
                If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
            ElseIf param.Length >= 2 And param(1) = "news" Then
                getContentRefByURL(param(2), rootPath, q_cr)
                getTagRefByURL(param(1), rootPath, q_tagRef)
            ElseIf param.Length >= 2 And param(1) = "blog" Then
                getContentRefByURL(param(2), rootPath, q_cr)
                getTagRefByURL(param(1), rootPath, q_tagRef)
            ElseIf param.Length >= 2 And param(1) = "team" Then
                q_cr = param(2)
                getTagRefByURL(param(1), rootPath, q_tagRef)
                'ElseIf param(1) = "news" Then
                '    If param.Length >= 3 Then
                '        getContentRefByURL(param(2), rootPath, q_cr)
                '        getTagRefByURL(param(1), rootPath, q_tagRef)
                '    ElseIf param.Length >= 2 Then
                '        getTagRefByURL(param(1), rootPath, q_tagRef)
                '    End If
            ElseIf param.Length >= 6 And param(1) = "what-s-on" Then
                If param.Length >= 6 Then q_cr = param(5)
                If param.Length >= 5 Then q_projectRef = param(4)
                If param.Length >= 4 Then q_dbMasterRef = param(3)
                'If param.Length >= 3 Then getTagRefByURLNapro(param(2), rootPath, q_tagRefNapro)
                If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
            ElseIf param.Length >= 4 And param(1) = "about-us" Then
                getTagRefByURL(param(1), rootPath, q_tagRef)
                If param(2) = "project" Then
                    q_projectRef = param(3)
                ElseIf param(2) = "developer" Then
                    q_developerRef = param(3)
                End If
            ElseIf param.Length = 8 Then
                Dim dtProjectSettingNapro As New DataTable
                'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)
                If dtProjectSettingNapro.Rows.Count > 0 Then
                    q_dbMasterRef = dtProjectSettingNapro.Rows(0).Item("dbMasterRef")
                    q_projectRef = dtProjectSettingNapro.Rows(0).Item("projectRef")
                End If
                If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
                If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRefLvl2)
                If param.Length >= 4 Then getTagRefByURL(param(3), rootPath, q_tagRefLvl3)
                If param.Length >= 5 Then getTagRefByURL(param(4), rootPath, q_tagRefLvl4)
                If param.Length >= 6 Then getCategoryRef(param(5), rootPath, q_projectRef, q_dbMasterRef, q_categoryRef)
                If param.Length >= 7 Then getClusterRef(param(6), rootPath, q_projectRef, q_dbMasterRef, q_categoryRef, q_clusterRef)
                If param.Length >= 8 Then getProductRef(param(7), rootPath, q_clusterRef, q_projectRef, q_dbMasterRef, q_productRef)

            End If

            app.Context.RewritePath(path + "wf/pContentLvl3.aspx", "", "t=" + q_tagRef + "&tLvl2=" + q_tagRefLvl2 + "&tLvl3=" + q_tagRefLvl3 + "&tLvl4=" + q_tagRefLvl4 + "&cr=" + q_cr + "&db=" + q_dbMasterRef + "&pr=" + q_projectRef + "&pf=" + q_productRef + "&ct=" + q_categoryRef + "&cl=" + q_clusterRef + "&tn=" + q_tagRefNapro + "&d=" + q_developerRef + "&ccm=" + q_clusterByCMSMetro)

            'List
        ElseIf url.ToLower.Contains(rootPath.ToLower + "list") Then
            path = generatePath(url, "list", param) '0
            'ElseIf param(1) = "Support" then

            Dim q_page As String = ""
            Dim q_productRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_tagRef As String = ""
            Dim q_tagRef2 As String = ""
            Dim q_tagRefNapro As String = ""
            Dim q_cr As String = ""
            Dim dtProjectSettingNapro As New DataTable
            'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)
            'tamnbahan

            If param.Length >= 4 And param(1) <> "what-s-on" Then

                'If param.Length >= 4 Then q_page = param(3)
                'If param.Length >= 3 Then q_productRef = param(2)
                'If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
                If param(2) = "reiz-executive" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
                    'ElseIf param(1) = "produk" Then 
                    '    getTagRefByURL(param(2), rootPath, q_tagRef)
                    '    If param.Length >= 4 Then q_projectRef = param(3)
                    '    If param.Length >= 3 Then q_dbMasterRef = param(2)
                    '    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                ElseIf param(2) = "reiz-junior-site" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                ElseIf param(2) = "reiz-suite" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                ElseIf param(2) = "reiz-garden" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                ElseIf param(2) = "reiz-garden" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                ElseIf param(2) = "reiz-penthouse" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                ElseIf param(2) = "facilities" Then
                    If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                    If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

                Else
                    getTagRefByURL(param(2), rootPath, q_tagRef)
                    If param.Length >= 4 Then q_page = param(3)
                    If param.Length >= 3 Then q_cr = param(3)
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
                End If

            ElseIf param(1) = "unit" Then
                getTagRefByURL(param(1), rootPath, q_tagRef)
                'If param.Length >= 3 Then getContentRefByURLNapro(param(3), q_tagRef, rootPath, q_cr)
                If param.Length >= 2 Then q_tagRef = q_tagRef
            ElseIf param(1) = "what-s-on" Then
                If param(2) = "newsletter" Then
                    If param.Length >= 3 Then
                        getTagRefByURL(param(2), rootPath, q_tagRef)
                        getTagRefByURL(param(1), rootPath, q_tagRef2)
                    Else
                        q_page = param(3)
                    End If
                Else
                    ' Dim dtProjectSettingNapro As New DataTable
                    'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)
                    If dtProjectSettingNapro.Rows.Count > 0 Then
                        q_dbMasterRef = dtProjectSettingNapro.Rows(0).Item("dbMasterRef")
                        q_projectRef = dtProjectSettingNapro.Rows(0).Item("projectRef")
                    End If

                    'getTagRefByURLNapro(param(2), rootPath, q_tagRefNapro)

                    If param.Length >= 7 Then q_page = param(6)
                    'If param.Length >= 6 Then getContentRefByURLNapro(param(5), q_tagRefNapro, rootPath, q_cr)
                    If param.Length >= 5 Then q_projectRef = param(4)
                    If param.Length >= 4 Then q_dbMasterRef = param(3)
                    If param.Length >= 3 Then q_tagRefNapro = q_tagRefNapro
                    If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)
                End If

            ElseIf param(1) = "contact-us" Then
                If param.Length >= 3 Then
                    getContentRefByURL(param(2), rootPath, q_cr)
                    getTagRefByURL(param(1), rootPath, q_tagRef)
                ElseIf param.Length >= 2 Then
                    getTagRefByURL(param(1), rootPath, q_tagRef)
                End If
                'ElseIf param(1) = "support" then
                'ini kosong
            Else
                'If param.Length >= 5 Then q_page = param(4)
                'If param.Length >= 4 Then getContentRefByURL(param(3), rootPath, q_cr)
                'If param.Length >= 3 Then getTagRefByURL(param(2), rootPath, q_tagRef2)
                'If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRef)

            End If

            'app.Context.RewritePath(path + "wf/pContentLvl2.aspx", "", "t=" + q_tagRef + "&prod=" + q_productRef + "&p=" + q_page + "&tn=" + q_tagRefNapro + "&cr=" + q_cr + "&db=" + q_dbMasterRef + "&pr=" + q_projectRef + "&x=" + HttpContext.Current.Request.QueryString("x"))

            'Read More whats on
        ElseIf url.ToLower.Contains(rootPath.ToLower + "what-s-on") Then
            path = generatePath(url, "what-s-on", param) '0

            Dim q_page As String = ""
            Dim q_cr As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_tagRefNapro As String = ""
            Dim q_tagRefCarstensz As String = ""

            Dim dtProjectSettingNapro As New DataTable
            'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)

            'getTagRefByURLNapro(param(2), rootPath, q_tagRefNapro)


            If param.Length >= 7 Then q_page = param(6)
            'If param.Length >= 6 Then getContentRefByURLNapro(param(5), q_tagRefNapro, rootPath, q_cr)
            If param.Length >= 5 Then q_projectRef = param(4)
            If param.Length >= 4 Then q_dbMasterRef = param(3)
            If param.Length >= 3 Then q_tagRefNapro = q_tagRefNapro
            If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRefCarstensz)

            app.Context.RewritePath(path + "wf/pContentLvl2.aspx", "", "t=" + q_tagRefCarstensz + "&tn=" + q_tagRefNapro + "&cr=" + q_cr + "&db=" + q_dbMasterRef + "&pr=" + q_projectRef + "&p=" + q_page)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "subscribe") Then
            path = generatePath(url, "subscribe", param) '0

            Dim q_feedback As String = ""
            Dim q_tagRefCarstensz As String = ""

            If param.Length >= 3 Then q_feedback = param(2)
            If param.Length >= 2 Then getTagRefByURL(param(1), rootPath, q_tagRefCarstensz)

            app.Context.RewritePath(path + "wf/pContentLvl2.aspx", "", "t=" + q_tagRefCarstensz + "&f=" + q_feedback)

        ElseIf url.ToLower().Contains(rootPath.ToLower() + "search-all") Then
            path = generatePath(url, "search-all", param)



            app.Context.RewritePath(path + "wf/searchAll.aspx", "", "x=" + HttpContext.Current.Request.QueryString("x"))

        ElseIf url.ToLower().Contains(rootPath.ToLower() + "image-threesixty") Then
            path = generatePath(url, "image-threesixty", param)

            Dim q_ref As String = ""
            Dim q_productRef As String = ""
            Dim q_clusterRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim q_is As String = ""

            If param.Length >= 7 Then
                q_ref = param(6)
                q_productRef = param(5)
                q_clusterRef = param(4)
                q_projectRef = param(3)
                q_dbMasterRef = param(2)
                q_is = param(1)
            End If

            app.Context.RewritePath(path + "wf/support/displayImage.aspx", "", "m=" + q_is + "&d=" + q_dbMasterRef + "&pr=" + q_projectRef + "&cl=" + q_clusterRef + "&pf=" + q_productRef + "&r=" + q_ref)

        ElseIf url.ToLower.Contains(rootPath.ToLower + "threesixty") Then
            path = generatePath(url, "threesixty", param) '0

            Dim q_productRef As String = ""
            Dim q_categoryRef As String = ""
            Dim q_clusterRef As String = ""
            Dim q_projectRef As String = ""
            Dim q_dbMasterRef As String = ""
            Dim clusterRef As String = ""
            Dim productRef As String = ""
            Dim dtProjectSettingNapro As New DataTable

            If param.Length >= 7 Then
                'clusterRef = getClusterReff(param(1), rootPath, param(4), param(3), q_clusterRef)
                q_clusterRef = q_clusterRef

                If param.Length >= 7 Then q_productRef = param(6)
                If param.Length >= 6 Then q_clusterRef = param(5)
                If param.Length >= 5 Then q_projectRef = param(4)
                If param.Length >= 4 Then q_dbMasterRef = param(3)
                If param.Length >= 3 Then getProductRef(param(2), rootPath, q_clusterRef, param(4), param(3), q_productRef)
                If param.Length >= 2 Then q_clusterRef = q_clusterRef
            ElseIf param.Length >= 4 Then
                'dtProjectSettingNapro = ProjectSettingNaPro(_projectCode)
                q_dbMasterRef = dtProjectSettingNapro.Rows(0).Item("dbMasterRef")
                q_projectRef = dtProjectSettingNapro.Rows(0).Item("projectRef")
                getCategoryRef(param(1), rootPath, q_projectRef, q_dbMasterRef, q_categoryRef)
                getClusterRef(param(2), rootPath, q_projectRef, q_dbMasterRef, q_categoryRef, q_clusterRef)
                getProductRef(param(3), rootPath, q_clusterRef, q_projectRef, q_dbMasterRef, q_productRef)

                If param.Length >= 3 Then q_productRef = q_productRef
                If param.Length >= 2 Then q_clusterRef = q_clusterRef
            End If

            app.Context.RewritePath(path + "wf/threeSixtyView.aspx", "", "d=" + q_dbMasterRef + "&pr=" + q_projectRef + "&cl=" + q_clusterRef + "&pf=" + q_productRef)

        ElseIf url.ToLower().Contains(rootPath.ToLower() + "inquiries") Then
            path = generatePath(url, "inquiries", param)

            Dim q_title As String = ""
            Dim q_feedback As String = ""
            If param.Length >= 2 Then q_title = param(1)
            If param.Length >= 3 Then q_feedback = param(2)

            app.Context.RewritePath(path + "wf/inquiries.aspx", "", "tt=" + q_title + "&f=" + q_feedback)

        ElseIf Right(url, 1) = "/" Then
            app.Context.RewritePath("Default.aspx", "", "")
        Else

            Dim tagRefDefault As String = ""
            Dim isUrl As Boolean = False
            If url.ToLower().Contains(rootPath.ToLower() + convertStrToParam(getTagName(_domainRef, _tagRefBlog, False))) Then
                'Artikel
                path = generatePath(url, convertStrToParam(getTagName(_domainRef, _tagRefBlog, False)), param)
                tagRefDefault = _tagRefBlog

                isUrl = True

            End If

            If isUrl Then
                If param.Length >= 1 Then
                    Dim q_tagRef As String = tagRefDefault
                    Dim q_page As String = ""
                    If param.Length >= 3 Then q_page = param(2)
                    If param.Length = 2 Then
                        Dim q_tagRef2 As String = ""
                        Dim q_contentRef As String = param(1)
                        getTagRefByURL(param(1), rootPath, q_tagRef2)
                        getContentRefByTagRefAndURL(param(1), rootPath, q_contentRef, q_tagRef)

                        app.Context.RewritePath(path + "wf/pContentLvl3.aspx", "", "t=" + q_tagRef + "&cr=" + q_contentRef + HttpContext.Current.Request.QueryString("x"))

                    Else
                        'app.Context.RewritePath(path + "wf/pContentLvl2Bistro.aspx", "", "t=" + q_tagRef + HttpContext.Current.Request.QueryString("x"))
                        app.Context.RewritePath(path + "wf/pContentLvl2.aspx", "", "t=" + q_tagRef + "&p=" + q_page)
                    End If

                End If
            End If
        End If
    End Sub

End Class