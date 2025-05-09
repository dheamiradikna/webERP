Imports [class].clsWebGeneral
Imports [class].clsUploadItem
Imports [class].cls_wmContent
Imports System.IO
Partial Class wf_support_EditForm
    Inherits System.Web.UI.Page
    Protected rootPath As String = _rootPath
    Protected type As String = ""
    Protected action As String = ""
    Protected tagRef As String = ""

    Private Function bindPicture(ByVal tagRef As String, ByVal domainRef As String, ByVal imageRef As String) As String
        Dim result As New StringBuilder

        'If Trim(contentRef) = "" Then
        '    result.Append("<div class=""alert alert-info fade in alert-dismissible"">After save, you can insert the picture.</div>")
        'Else
        'Dim dt As New DataTable
        'Dim dt2 As New DataTable
        'Dim dtImg As New DataTable

        'dtImg = getTagImageSetting(domainRef, getContentFirstTag(domainRef, contentRef))

        'dt = getImage(domainRef, imageRef)

        'dt.Merge(dt2)
        ''dt.DefaultView.Sort = ""
        'dt = dt.DefaultView.ToTable()

        'If dt.Rows.Count > 0 Then
        '    For i = 0 To dt.Rows.Count - 1
        '        result.Append("<div class=""ov mb5"">")
        '        result.Append("  <img src=""" + rootPath + "wf/support/displayImage.aspx?m=content&d=" + domainRef + "&ir=" + dt.Rows(i).Item("imgRef").ToString + "&w=0"" style=""width:100%"" />")
        '        result.Append("</div>")
        '        result.Append("<div class=""mb5"">")
        '        result.Append("  <a href=""javascript:doDeleteImage(" + dt.Rows(i).Item("imgRef").ToString + ");"">Delete Image</a>")
        '        result.Append("</div>")
        '        result.Append("<div class=""mb5"">")
        '        result.Append("  <a href=""" + rootPath + "wf/support/editForm.aspx?src=contentTagPopup.aspx&m=contentPic&tr=" + tagRef + "&t=m&ir=0&r=" + contentRef + """);"">Edit Image</a>")
        '        result.Append("</div>")
        '    Next

        'Else
        '    'no image
        'End If

        'result.Append("<img id = ""image"" src=""http: //1.bp.blogspot.com/-sGL3240zgpg/UQ0XTX386HI/AAAAAAAAAU4/gqjeoO9JHCo/s1600/blank!.png"" alt=""Picture"">")
        result.Append("<img id = ""image"" src=""" + rootPath + "wf/support/displayImage.aspx?m=content&d=" + domainRef + "&ir=" + imageRef + "&w=0"" alt=""Picture"">")
        'End If


        Return result.ToString
    End Function

    Private Sub wf_support_editForm_Load(sender As Object, e As EventArgs) Handles Me.Load
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

        Dim attachFileExt As String = Request.Form("isExt")
        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""
        Dim newW As Integer = CInt(Request.Form("dataWidth"))
        Dim newH As Integer = CInt(Request.Form("dataHeight"))

        tagRef = q_tagRef

        Select Case q_type
            Case "m"
                type = "multiple"
            Case "s"
                type = ""
            Case Else
                type = ""
        End Select

        ''cropper

        Dim save_image As String = Request.Form("save_image")
        If Trim(save_image) = "1" Then
            Dim image_value As String = Request.Form("image_value")

            Dim decodedBytes As Byte()
            decodedBytes = Convert.FromBase64String(image_value)

            Dim decodedText As String
            decodedText = Encoding.UTF8.GetString(decodedBytes)

            Dim resultImage As String = ""
            'resultImage = insertImageProperty(decodedBytes, decodedText.Length)
            resultImage = updateIMG_TR_image(Session("domainRef").ToString, q_imageRef, "", "", "", newW, newH, decodedBytes, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)

            If IsNumeric(resultImage) Then
                resultImage = insertContentImage(Session("domainRef").ToString, q_ref, resultImage, "P", 0)

            Else

            End If
        End If

        ltrPicture.Text = bindPicture(q_tagRef, Session("domainRef").ToString, q_imageRef)

        'Select Case q_module
        '    Case "domainLogo"
        '        Me.Title = "Upload Domain Logo [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Domain Logo [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/admin/" + q_source + "?r=" + q_ref + """>Click Here</a> to go back"

        '    Case "domainImage"
        '        Me.Title = "Upload Domain Image [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Domain Image [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/admin/" + q_source + "?r=" + q_ref + """>Click Here</a> to go back"

        '    Case "tag"
        '        Me.Title = "Upload Tag Picture [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Tag Picture [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/sTagPopup.aspx?r=" + q_ref + """>Click Here</a> to go back"

        '    Case "contentPic"
        '        Me.Title = "Upload Content Picture [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Content Picture [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

        '    Case "contentPicChange"
        '        Me.Title = "Change Content Picture [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Change Content Picture [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + "&im=" + q_imageRef + """>Click Here</a> to go back"

        '    Case "contentPicHoverChange"
        '        Me.Title = "Change Content Picture [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Change Content Picture [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + "&imh=" + q_imageRefHover + """>Click Here</a> to go back"

        '    Case "contentThumb"
        '        Me.Title = "Upload Content Thumbnail [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Content Thumbnail [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

        '    Case "contentAttach"
        '        Me.Title = "Upload Attachment [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Content Attachment [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

        '        ''Tambahan 18/08/2015
        '    Case "contentPicHover"
        '        Me.Title = "Upload Content Picture Hover [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Content Picture Hover [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

        '    Case "contentImageSlideshow"
        '        Me.Title = "Upload Content Image Slideshow [" + Session("domain") + "]"
        '        titlePopup.InnerHtml = "Upload Content Image Slideshow [" + Session("domain") + "]"
        '        divNotif.InnerHtml = "<a href=""" + rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + """>Click Here</a> to go back"

        'End Select

        ' action = "postedFile.aspx?m=" + q_module + "&r=" + q_ref + "&ir=" + q_isResize + "&rw=" + q_resizeW + "&rh=" + q_resizeH + "&tr=" + q_tagRef + "&im=" + q_imageRef + "&imh=" + q_imageRefHover

    End Sub
End Class
