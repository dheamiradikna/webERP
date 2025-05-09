Imports System.IO
Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].clsUploadItem
Imports [class].cls_wmContent

Partial Class wf_support_postedFile
    Inherits System.Web.UI.Page

    Protected status As String = ""
    Protected message As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim q_module As String = Request.QueryString("m")
        Dim q_isResize As String = Request.QueryString("ir")
        Dim q_ref As String = Request.QueryString("r") 'content ref
        Dim q_resizeW As String = Request.QueryString("rw")
        Dim q_resizeH As String = Request.QueryString("rh")
        Dim q_tagRef As String = Request.QueryString("tr")
        Dim q_imageRef As String = Request.QueryString("im")
        Dim q_imageRefHover As String = Request.QueryString("imh")
        Dim q_source As String = Request.QueryString("src")

        If Trim(q_resizeW) = "" Then q_resizeW = "0"
        If Trim(q_resizeH) = "" Then q_resizeH = "0"

        Dim f As HttpPostedFile
        f = Request.Files.Get(0)

        If Not f Is Nothing Then

            Dim attachFileDataStream As Stream = f.InputStream
            Dim attachFileLength As Integer = f.ContentLength
            Dim attachFileData(attachFileLength) As Byte
            Dim attachFileType As String = f.ContentType
            Dim attachFileExt As String = ""
            Dim attachFileName As String = ""
            Dim tempFileName As String = filterFileName(f.FileName)
            Dim index As Integer = 0

            Dim attachFileString As String = ""
            Dim size As Double
            Dim strSize As Integer 
            Dim fileSize As String = ""

            attachFileString = GetFileImageSize(attachFileData)

           If attachFileString <> "" Then
                size = Split(attachFileString, " ")(0)
                strSize = Math.Round(size)
                fileSize = Split(attachFileString, " ")(1)
            End If

            Dim maxWidthPic As Integer = CInt(q_resizeW)
            Dim maxHeightPic As Integer = CInt(q_resizeH)

            Select Case q_module
                Case "tag"
                    'cek maxwidth/maxheight
                Case "contentPic"
                Case "contentThumb"

            End Select

            Try
                attachFileDataStream.Read(attachFileData, 0, attachFileLength)

                index = StrReverse(tempFileName).IndexOf(".")
                attachFileExt = Right(tempFileName, index)
                index = StrReverse(tempFileName).IndexOf("\")
                If index = -1 Then
                    attachFileName = tempFileName
                Else
                    attachFileName = Right(tempFileName, index)
                End If

                'batasi max 30 char
                attachFileName = Right(attachFileName, 100)


                If q_module = "tag" Or q_module = "contentPic" Or q_module = "contentPicChange" Or q_module = "contentPicHoverChange" Or q_module = "contentThumb" Or q_module = "contentPicHover" Or q_module = "contentImageSlideshow" Or q_module = "domainLogo" Or q_module = "domainImage" Then
                    ''''' image stufff '''''
                    ''''' image stufff '''''
                    ''''' image stufff '''''

                    Dim width As Integer = System.Drawing.Image.FromStream(attachFileDataStream).PhysicalDimension.Width
                    Dim height As Integer = System.Drawing.Image.FromStream(attachFileDataStream).PhysicalDimension.Height

                    'lock by width
                    'If width > maxWidthPic Then width = maxWidthPic

                    Dim picFileDataResize() As Byte
                    Dim newW As Integer = 0
                    Dim newH As Integer = 0

                    If q_isResize = "0" Then
                        maxWidthPic = width
                        maxHeightPic = height
                    End If

                    If maxWidthPic = 0 Then maxWidthPic = width
                    If maxHeightPic = 0 Then maxHeightPic = height

                    'If width <> maxWidthPic And height <> maxHeightPic Then
                    '    If width <= maxWidthPic And height <= maxHeightPic Then
                    '        picFileDataResize = combine(attachFileData, maxWidthPic, maxHeightPic, width, height, attachFileExt)
                    '    ElseIf width <= maxWidthPic Then
                    '        picFileDataResize = combine(resizePicture(attachFileData, 0, maxHeightPic - 2, attachFileExt, newW, newH), maxWidthPic, maxHeightPic, newW, newH, attachFileExt)
                    '    ElseIf height <= maxHeightPic Then
                    '        picFileDataResize = combine(resizePicture(attachFileData, maxWidthPic - 2, 0, attachFileExt, newW, newH), maxWidthPic, maxHeightPic, newW, newH, attachFileExt)
                    '    ElseIf width > maxWidthPic And height > maxHeightPic Then
                    '        If CDbl(width) / maxWidthPic > CDbl(height) / maxHeightPic Then
                    '            picFileDataResize = combine(resizePicture(attachFileData, maxWidthPic - 2, 0, attachFileExt, newW, newH), maxWidthPic, maxHeightPic, newW, newH, attachFileExt)
                    '        Else
                    '            picFileDataResize = combine(resizePicture(attachFileData, 0, maxHeightPic - 2, attachFileExt, newW, newH), maxWidthPic, maxHeightPic, newW, newH, attachFileExt)
                    '        End If
                    '    Else
                    '        picFileDataResize = combine(resizePicture(attachFileData, maxWidthPic - 2, maxHeightPic - 2, attachFileExt, newW, newH), maxWidthPic, maxHeightPic, newW, newH, attachFileExt)
                    '    End If
                    'Else
                        picFileDataResize = attachFileData
                    'End If

                    Dim inputUN As String = Session("userName")
                    If inputUN = String.Empty Then inputUN = ""

                    Select Case q_module
                        Case "domainLogo"
                            Dim temp As String = ""

                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = uploadDomainLogo(q_ref, picFileDataResize, "logo_" + q_ref + "." + attachFileExt)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = uploadDomainLogo(q_ref, picFileDataResize, "logo_" + q_ref + "." + attachFileExt)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = uploadDomainLogo(q_ref, picFileDataResize, "logo_" + q_ref + "." + attachFileExt)
                                    End If
                                End If
                            End If

                            If Trim(temp) = "" Then
                                status = "success"
                                message = "Upload succeed"
                            Else
                                status = "error"
                                message = "Upload error, please try again"
                            End If

                        Case "domainImage"
                            Dim temp As String = ""

                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = uploadDomainImage(q_ref, picFileDataResize, "image_" + q_ref + "." + attachFileExt)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = uploadDomainImage(q_ref, picFileDataResize, "image_" + q_ref + "." + attachFileExt)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = uploadDomainImage(q_ref, picFileDataResize, "image_" + q_ref + "." + attachFileExt)
                                    End If
                                End If
                            End If

                            If Trim(temp) = "" Then
                                status = "success"
                                message = "Upload succeed"
                            Else
                                status = "error"
                                message = "Upload error, please try again"
                            End If

                        Case "tag"
                            Dim temp As String = ""

                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = uploadTagPicture(Session("domainRef").ToString, q_ref, picFileDataResize, Session("domain") + "_tag_" + q_ref + "." + attachFileExt)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = uploadTagPicture(Session("domainRef").ToString, q_ref, picFileDataResize, Session("domain") + "_tag_" + q_ref + "." + attachFileExt)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = uploadTagPicture(Session("domainRef").ToString, q_ref, picFileDataResize, Session("domain") + "_tag_" + q_ref + "." + attachFileExt)
                                    End If
                                End If
                            End If

                            If Trim(temp) = "" Then
                                status = "success"
                                message = "Upload succeed"
                            Else
                                status = "error"
                                message = "Upload error, please try again"
                            End If

                        Case "contentPic"

                            Dim temp As String = ""
                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                End If
                            End If

                            'status = "success"
                            'message = "Upload succeed"
                            'Response.Write("<div id=""status"">" + status + "</div>")
                            'Response.Write("<div id=""message"">" + message + "</div>")
                            'Exit Sub

                            If IsNumeric(temp) Then
                                temp = insertContentImage(Session("domainRef").ToString, q_ref, temp, "P", 0)

                                If Trim(temp) = "" Then
                                    status = "success"
                                    message = "Upload succeed"
                                Else
                                    status = "error"
                                    message = "Upload error, please try again"
                                End If
                            Else
                                status = "error"
                                message = "Upload to img_tr_image error, please try again"
                            End If

                        Case "contentPicChange"

                            Dim temp As String = ""
                          
                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = deleteImage(Session("domainRef").ToString, q_imageRef)
                                    temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = deleteImage(Session("domainRef").ToString, q_imageRef)
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = deleteImage(Session("domainRef").ToString, q_imageRef)
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                End If
                            End If

                            If IsNumeric(temp) Then
                                temp = insertContentImage(Session("domainRef").ToString, q_ref, temp, "P", 0)

                                If Trim(temp) = "" Then
                                    status = "success"
                                    message = "Upload succeed"
                                Else
                                    status = "error"
                                    message = "Upload error, please try again"
                                End If
                            Else
                                status = "error"
                                message = "Upload to img_tr_image error, please try again"
                            End If

                        Case "contentPicHoverChange"

                            Dim temp As String = ""
                            
                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = deleteImage(Session("domainRef").ToString, q_imageRefHover)
                                    temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = deleteImage(Session("domainRef").ToString, q_imageRefHover)
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = deleteImage(Session("domainRef").ToString, q_imageRefHover)
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                End If
                            End If

                            If IsNumeric(temp) Then
                                temp = insertContentImage(Session("domainRef").ToString, q_ref, temp, "H", 0)

                                If Trim(temp) = "" Then
                                    status = "success"
                                    message = "Upload succeed"
                                Else
                                    status = "error"
                                    message = "Upload error, please try again"
                                End If
                            Else
                                status = "error"
                                message = "Upload to img_tr_image error, please try again"
                            End If

                        Case "contentThumb"

                            Dim temp As String = ""

                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                End If
                            End If

                            If IsNumeric(temp) Then
                                temp = insertContentImage(Session("domainRef").ToString, q_ref, temp, "T", 0)

                                If Trim(temp) = "" Then
                                    status = "success"
                                    message = "Upload succeed"
                                Else
                                    status = "error"
                                    message = "Upload error, please try again"
                                End If
                            Else
                                status = "error"
                                message = "Upload to img_tr_image error, please try again"
                            End If

                            ''Tambahan 18/08/2015
                        Case "contentPicHover"

                            Dim temp As String = ""

                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                End If
                            End If

                            If IsNumeric(temp) Then
                                temp = insertContentImage(Session("domainRef").ToString, q_ref, temp, "H", 0)

                                If Trim(temp) = "" Then
                                    status = "success"
                                    message = "Upload succeed"
                                Else
                                    status = "error"
                                    message = "Upload error, please try again"
                                End If
                            Else
                                status = "error"
                                message = "Upload to img_tr_image error, please try again"
                            End If

                        Case "contentImageSlideshow"

                            Dim temp As String = ""

                            If attachFileExt = "pdf" Then
                                If fileSize = "TB" Or fileSize = "GB" Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf fileSize = "MB" And strSize > 20 Then
                                    Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                                ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                                    temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                End If
                            Else
                                If q_tagRef = _contentTagRefBackgroundSlider Then
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 500 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                Else
                                    If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf fileSize = "KB" And strSize > 250 Then
                                        Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                                    ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                        temp = insertIMG_TR_image(Session("domainRef").ToString, "", "", "", newW, newH, picFileDataResize, Session("domain") + "_content_" + q_ref + "." + attachFileExt, inputUN)
                                    End If
                                End If
                            End If

                            If IsNumeric(temp) Then
                                temp = insertContentImage(Session("domainRef").ToString, q_ref, temp, "I", 0)

                                If Trim(temp) = "" Then
                                    status = "success"
                                    message = "Upload succeed"
                                Else
                                    status = "error"
                                    message = "Upload error, please try again"
                                End If
                            Else
                                status = "error"
                                message = "Upload to img_tr_image error, please try again"
                            End If

                    End Select

                    ''''' image stufff end '''''
                    ''''' image stufff end '''''
                    ''''' image stufff end '''''

                ElseIf q_module = "contentAttach" Then

                    ''''' attachment stuff '''''
                    ''''' attachment stuff '''''
                    ''''' attachment stuff '''''

                    Dim temp As String = ""

                    If attachFileExt = "pdf" Then
                        If fileSize = "TB" Or fileSize = "GB" Then
                            Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                        ElseIf fileSize = "MB" And strSize > 20 Then
                            Response.Write("<script language=""javascript"">alert('Max size 20MB !!!');</script>")
                        ElseIf (fileSize = "MB" And strSize <= 20) Or fileSize = "KB" Or fileSize = "bytes" Then
                            temp = saveContentAttachment(Session("domainRef").ToString, q_ref, "", attachFileData, attachFileName, "", q_tagRef)
                        End If
                    Else
                        If q_tagRef = _contentTagRefBackgroundSlider Then
                            If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                            ElseIf fileSize = "KB" And strSize > 500 Then
                                Response.Write("<script language=""javascript"">alert('Max size 500KB !!!');</script>")
                            ElseIf (fileSize = "KB" And strSize < 500) Or fileSize = "bytes" Then
                                temp = saveContentAttachment(Session("domainRef").ToString, q_ref, "", attachFileData, attachFileName, "", q_tagRef)
                            End If
                        Else
                            If fileSize = "TB" Or fileSize = "GB" Or fileSize = "MB" Then
                                Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                            ElseIf fileSize = "KB" And strSize > 250 Then
                                Response.Write("<script language=""javascript"">alert('Max size 250KB !!!');</script>")
                            ElseIf (fileSize = "KB" And strSize < 250) Or fileSize = "bytes" Then
                                temp = saveContentAttachment(Session("domainRef").ToString, q_ref, "", attachFileData, attachFileName, "", q_tagRef)
                            End If
                        End If
                    End If

                    If Trim(temp) = "" Then
                        status = "success"
                        message = "Upload succeed"
                    Else
                        status = "error"
                        message = "Upload error, please try again"
                    End If

                    ''''' attachment stuff end '''''
                    ''''' attachment stuff end '''''
                    ''''' attachment stuff end '''''

                End If

            Catch ex As Exception

                status = "error"
                message = "Error processing file"

                Select Case q_module
                    Case "tag"
                    Case "contentPic"
                    Case "contentThumb"
                End Select

            End Try

        End If

        Response.Write("<div id=""status"">" + status + "</div>")
        'Response.Write("<div id=""message"">" + message + " and <a href=""" + _rootPath + "wf/contentTagPopup.aspx?tr=" + q_tagRef + "&r=" + q_ref + " "">Click Here</a> to go back</div>")
        Response.Write("<div id=""message"">" + message + " and <a href=""" + _rootPath + "wf/" + q_source + "?tr=" + q_tagRef + "&r=" + q_ref + " "">Click Here</a> to go back</div>")

    End Sub
End Class
