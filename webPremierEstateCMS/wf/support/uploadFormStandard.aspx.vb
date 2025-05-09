Imports [class].clsWebGeneral
Imports [class].clsGeneral
Partial Class wf_support_uploadFormStandard
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath
    Protected type As String = ""
    Protected action As String = ""
    Protected tagRef As String = ""

    Private Sub wf_support_uploadFormStandard_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim q_source As String = Request.QueryString("src")
        Dim q_module As String = Request.QueryString("m")
        Dim q_type As String = Request.QueryString("t")
        Dim q_isResize As String = Request.QueryString("ir")
        Dim q_ref As String = Request.QueryString("r")
        Dim q_resizeW As String = Request.QueryString("rw")
        Dim q_resizeH As String = Request.QueryString("rh")
        Dim q_tagRef As String = Request.QueryString("tr")
        Dim q_imageRef As String = Request.QueryString("im")
        Dim q_imageRefHover As String = Request.QueryString("imh")

        If Trim(q_isResize) = "" Then q_isResize = "0"
        If Trim(q_resizeW) = "" Then q_resizeW = "0"
        If Trim(q_resizeH) = "" Then q_resizeH = "0"

        tagRef = q_tagRef

        Select Case q_type
            Case "m"
                type = "multiple"
            Case "s"
                type = ""
            Case Else
                type = ""
        End Select

        Select Case q_module
            Case "domainLogo"
                Me.Title = "Upload Domain Logo [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Domain Logo [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/admin/" + q_source + "?r=" + q_ref + """>Click Here</a> to go back"

            Case "domainImage"
                Me.Title = "Upload Domain Image [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Domain Image [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/admin/" + q_source + "?r=" + q_ref + """>Click Here</a> to go back"

            Case "tag"
                Me.Title = "Upload Tag Picture [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Tag Picture [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/sTagPopup.aspx?r=" + q_ref + """>Click Here</a> to go back"

            Case "contentPic"
                Me.Title = "Upload Content Picture [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Content Picture [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

            Case "contentPicChange"
                Me.Title = "Change Content Picture [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Change Content Picture [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + "&im=" + q_imageRef + """>Click Here</a> to go back"

            Case "contentPicHoverChange"
                Me.Title = "Change Content Picture [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Change Content Picture [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + "&imh=" + q_imageRefHover + """>Click Here</a> to go back"

            Case "contentThumb"
                Me.Title = "Upload Content Thumbnail [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Content Thumbnail [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

            Case "contentAttach"
                Me.Title = "Upload Attachment [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Content Attachment [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

                ''Tambahan 18/08/2015
            Case "contentPicHover"
                Me.Title = "Upload Content Picture Hover [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Content Picture Hover [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

            Case "contentImageSlideshow"
                Me.Title = "Upload Content Image Slideshow [" + Session("domain") + "]"
                titlePopup.InnerHtml = "Upload Content Image Slideshow [" + Session("domain") + "]"
                divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

        End Select
        Dim Hashtable As New Hashtable
       ' Response.Redirect(GetEncUrl(_rootPath + "wf/contentTagPopup.aspx?tr=" + q_tagRef, Hashtable))

        action = "postedFile.aspx?m=" + q_module + "&r=" + q_ref + "&ir=" + q_isResize + "&rw=" + q_resizeW + "&rh=" + q_resizeH + "&tr=" + q_tagRef + "&im=" + q_imageRef + "&imh=" + q_imageRefHover + "&src=" + q_source
        
    End Sub
End Class
