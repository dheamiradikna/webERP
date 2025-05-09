
Imports System.Drawing
Imports System.Data
Imports System.DateTime
Imports [class].clsContentDB
Imports [class].clsGeneral
Imports [class].clsWebGeneral
Imports [class].clsGeneralDB

Partial Class wf_support_displayImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim q_module As String = Request.QueryString("m")
        Dim q_height As String = Request.QueryString("h") 'kanvas height
        Dim q_width As String = Request.QueryString("w") 'kanvas width
        Dim q_resizePct As String = Request.QueryString("pct")
        Dim q_isFullBackground As String = Request.QueryString("isBg")
       

        If Trim(q_height) = "" Then q_height = "0"
        If Trim(q_width) = "" Then q_width = "0"
        If Trim(q_resizePct) = "" Then q_resizePct = "0"

        Dim dt As New DataTable
        Dim dtPicture As New DataTable
        Dim dtNewsEvents As New DataTable
        Dim attach As Byte() = {0}

        Select Case q_module
            Case "domainLogo"
                Dim q_domainRef As String = Request.QueryString("d")
                dt = getDomainLogo(q_domainRef)
            Case "domainImage"
                Dim q_domainRef As String = Request.QueryString("d")
                dt = getDomainImage(q_domainRef)
            Case "content"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_imageRef As String = Request.QueryString("ir")
                dt = getSingleContentFile(q_domainRef, q_imageRef, False)
            Case "domainContent"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_imageRef As String = Request.QueryString("ir")
                dt = getSingleContentFile(q_domainRef, q_imageRef, True)
             Case "bannererp"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_imageRef As String = Request.QueryString("ir")
                dt = getSingleContentFileBannerERP(q_domainRef, q_imageRef, False)
            Case "tagPicture"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_tagRef As String = Request.QueryString("tr")
                dtPicture = getPicture(q_domainRef, q_tagRef)
            Case "newsEvents"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_imageRef As String = Request.QueryString("ir")
                Dim q_contentRef As String = Request.QueryString("cr")
                dt = getSingleContentFile(q_domainRef, q_imageRef, True)
            Case "covernapro"
                Dim q_dbMasterRef As String = Request.QueryString("d")
                Dim q_projectRef As String = Request.QueryString("pr")
                'dt = getFileDataCoverDisplay(q_dbMasterRef, q_projectRef)
            Case "newsEventNaPro"
	            Dim q_imageRef As String = Request.QueryString("ir")
	            Dim q_contentRef As String = Request.QueryString("cr")
                'dt = getContentImageProjectNaproByImgRef(q_imageRef, q_contentRef)
            Case "segar"
                Dim q_dbMasterRef As String = Request.QueryString("d")
                Dim q_projectRef As String = Request.QueryString("pr")
                Dim q_clusterRef As String = Request.QueryString("cr")
                'dt = getFileDataTower(q_dbMasterRef, q_projectRef, q_clusterRef)
            Case "imageBlockPlan"
                Dim q_dbMasterRef As String = Request.QueryString("d")
                Dim q_projectRef As String = Request.QueryString("pr")
                Dim q_clusterRef As String = Request.QueryString("cl")
                'dt = getImageBlockPlan(q_dbMasterRef, q_projectRef, q_clusterRef)
            Case "dataProduct"
                Dim q_dbMasterProjectProductFileRef As String = Request.QueryString("r")
                'dt = getFileDataProduct(q_dbMasterProjectProductFileRef)
            Case "gallerynapro"
                Dim q_imageGalleryRef As String = Request.QueryString("igr")
                Dim q_groupGalleryRef As String = Request.QueryString("ggr")
                'dt = getImageProjectGalleryNaproByImgRef(q_imageGalleryRef, q_groupGalleryRef)
            Case "threesixty"
                Dim q_dbMasterRef As String = Request.QueryString("d")
                Dim q_projectRef As String = Request.QueryString("pr")
                Dim q_clusterRef As String = Request.QueryString("cl")
                Dim q_productRef As String = Request.QueryString("pf")
                Dim q_ref As String = Request.QueryString("r")
                'attach = GetThreesixtyImage(q_dbMasterRef, q_projectRef, q_clusterRef, q_productRef, q_ref.Split(".")(0).ToString)
            Case "adsLEAD"
                Dim q_picRef As String = Request.QueryString("r")
                'dt = getAdsImage(q_picRef)
            Case "dsc"
                Dim q_psRef As String = Request.QueryString("ir")
                dt = getImageRefByContentDSC(q_psRef)
                If dt.Rows.Count = 0 Then
                    Response.Redirect(_rootPath + "Support/img/ICON-PRIME.png")
                End If
            Case Else
                Dim q_imgID As String = Request.QueryString("iid")
        End Select

      
        Dim fileName As String = ""
        
        If dt.Rows.Count > 0 Then
            Dim fileExt As String = ""
            attach = dt.Rows(0).Item("imgFile")
           
            If q_module = "newsEventNaPro" Then
                fileName = "img_Berita_" + dt.Rows(0).Item("imgRef").ToString + ".jpg"
            ElseIf q_module = "imageBlockPlan" Then
                fileName = "img_BlockPlan_" + dt.Rows(0).Item("imgRef").ToString + ".jpg"
            ElseIf q_module = "newsEventNaPro" Then
                fileName = "img_Berita_" + dt.Rows(0).Item("imgRef").ToString + ".jpg"
            ElseIf q_module = "dataProduct" Then
                fileName = "img_Unit_" + dt.Rows(0).Item("imgRef").ToString + ".jpg"
            ElseIf q_module = "content" Then
                fileName = dt.Rows(0).Item("imgFileName")
            Else
                If Trim(dt.Rows(0).Item("imgFileName")) <> "" Then
                    fileName = dt.Rows(0).Item("imgFileName")
                Else
                    fileName = Format(Now, "yyMMddHHmmss").ToString + ".jpg"
                End If
            End If
        ElseIf q_module = "threesixty" Then
            Dim q_ref As String = Request.QueryString("r")
            fileName = "img_" + q_ref
        End If


        If dtPicture.Rows.Count > 0 Then
            attach = dtPicture.Rows(0).Item("tagPicture")
            fileName = dtPicture.Rows(0).Item("tagPictureFile")
        End If

        If Trim(fileName) <> "" Then
            Dim fileExt As String = ""
            Dim index As Integer = StrReverse(fileName).IndexOf(".")
            fileExt = Right(fileName, index)

            Dim ms As New System.IO.MemoryStream(attach)
            Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(ms)

            Dim newW As Integer = image.Width
            Dim newH As Integer = image.Height
            Dim varBorder As Integer = 2

            Dim isWZero As Boolean = False
            Dim isHZero As Boolean = False

            If q_width = 0 Then
                q_width = newW + varBorder
                isWZero = True
            End If
            
            If q_height.Split(".").Length > 0 Then
                q_height = q_height.Split(".").First.ToString()
            End If     
            
            If q_height = 0 Then
                q_height = newH + varBorder
                isHZero = True
            End If

            If Trim(q_resizePct) = "0" Or Trim(q_resizePct) = "" Then
                If q_width <> 0 And q_height <> 0 Then

                    If image.Width > q_width - varBorder Then
                        Dim tempH As Integer = CSng(((q_width - varBorder) / image.Width)) * image.Height
                        If tempH <= q_height - varBorder Then
                            newW = q_width - varBorder
                            newH = tempH
                        Else
                            Dim tempW As Integer = CSng(((q_height - varBorder) / image.Height)) * image.Width
                            newH = q_height - varBorder
                            newW = tempW
                        End If

                    ElseIf image.Height > q_height - varBorder Then
                        'jika panjang gambar lebih lebar dari panjang kanvas
                        Dim tempW As Integer = CSng(((q_height - varBorder) / image.Height)) * image.Width
                        If tempW <= q_width - varBorder Then
                            newH = q_height - varBorder
                            newW = tempW
                        Else
                            Dim tempH As Integer = CSng(((q_width - varBorder) / image.Width)) * image.Height
                            newW = q_width - varBorder
                            newH = tempH
                        End If
                    Else
                    End If

                End If
            Else
                newW = (CSng(q_resizePct) / 100) * image.Width
                newH = (CSng(q_resizePct) / 100) * image.Height

                q_width = newW + 2
                q_height = newH + 2
            End If

            If isWZero Then
                q_width = newW + varBorder
            End If

            If isHZero Then
                q_height = newH + varBorder
            End If

            If newH <> image.Height And newW <> image.Width Then
                attach = resizePicture(attach, newW, newH, fileExt, newW, newH)

                If newH < q_height And newW < q_width Then
                    attach = combine(attach, q_width, q_height, newW, newH, fileExt)
                ElseIf newH < q_height Then
                    attach = combine(attach, newW, q_height, newW, newH, fileExt)
                ElseIf newW < q_width Then
                    attach = combine(attach, q_width, newH, newW, newH, fileExt)
                End If

                Dim msResize As New System.IO.MemoryStream(attach)
                image = System.Drawing.Image.FromStream(msResize)
            Else
                'tidak ada perubahan besar
                If q_isFullBackground = "1" Then
                    attach = resizePicture(attach, newW, newH, fileExt, newW, newH)
                    attach = combine(attach, q_width, q_height, newW, newH, fileExt)
                    Dim msResize As New System.IO.MemoryStream(attach)
                    image = System.Drawing.Image.FromStream(msResize)
                End If

            End If

            Select Case fileExt.ToLower
                Case "gif"
                    Response.ContentType = "image/gif"
                    Response.Cache.SetMaxAge(TimeSpan.Zero)
                    Response.Cache.SetCacheability(HttpCacheability.Public)
                    Response.Cache.SetSlidingExpiration(True)
                    image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif)
                Case "jpg"
                    Response.ContentType = "image/jpg"
                    Response.Cache.SetMaxAge(TimeSpan.Zero)
                    Response.Cache.SetExpires(DateTime.Now.AddDays(365))
                    Response.Cache.SetCacheability(HttpCacheability.Public)
                    Response.Cache.SetSlidingExpiration(True)
                    image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                Case "jpeg"
                    Response.ContentType = "image/jpeg"
                    Response.Cache.SetMaxAge(TimeSpan.Zero)
                    Response.Cache.SetExpires(DateTime.Now.AddDays(365))
                    Response.Cache.SetCacheability(HttpCacheability.Public)
                    Response.Cache.SetSlidingExpiration(True)
                    image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                Case Else
                    Response.ContentType = "image/png"
               Response.Cache.SetMaxAge(TimeSpan.Zero)
               Response.Cache.SetCacheability(HttpCacheability.Public)
               Response.Cache.SetSlidingExpiration(True)
               'image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png)

               Dim parameters As Imaging.EncoderParameters = New Imaging.EncoderParameters(3)
                   parameters.Param(0) = New Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L)
                   parameters.Param(1) = New Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, CType(Imaging.EncoderValue.ScanMethodInterlaced, Integer))
                   parameters.Param(2) = New Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, CType(Imaging.EncoderValue.RenderProgressive, Integer))
               Dim codecInfo As Imaging.ImageCodecInfo = findencoder(Imaging.ImageFormat.Png)
                   image.Save(Response.OutputStream, codecInfo, parameters)
                    End Select

        Else
            'no pic
            Dim imgBackGround As New Bitmap(CInt(q_width), CInt(q_height))
            For i = 0 To q_width - 1
                For j = 0 To q_height - 1
                    imgBackGround.SetPixel(i, j, Color.FromArgb(_imageBgColor.Split("|")(0), _imageBgColor.Split("|")(1), _imageBgColor.Split("|")(2)))
                Next
            Next

            Dim msBackGround As New System.IO.MemoryStream
            imgBackGround.Save(msBackGround, Imaging.ImageFormat.Jpeg)

            Dim picFileDataBackGround(msBackGround.Length - 1) As Byte
            msBackGround.Position = 0
            msBackGround.Read(picFileDataBackGround, 0, msBackGround.Length)

            'picFileDataBackGround = doWaterMark(picFileDataBackGround, "no thumbnail", 0.5)

            Dim msBackGroundOk As New System.IO.MemoryStream(picFileDataBackGround)
            Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(msBackGroundOk)

            Response.ContentType = "image/jpeg"
            Response.Cache.SetMaxAge(TimeSpan.Zero)
            Response.Cache.SetCacheability(HttpCacheability.Public)
            Response.Cache.SetSlidingExpiration(True)

            image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)

        End If

    End Sub
    Private Function findencoder(ByVal fmt As Imaging.ImageFormat) As Imaging.ImageCodecInfo
        Dim infoArray1 As Imaging.ImageCodecInfo() = Imaging.ImageCodecInfo.GetImageEncoders
        Dim infoArray2 As Imaging.ImageCodecInfo() = infoArray1
        Dim info1 As Imaging.ImageCodecInfo
        For i = 0 To infoArray2.Length
            info1 = infoArray2(i)
            If info1.FormatID.Equals(fmt.Guid) Then
                findencoder = info1
                Exit Function
            End If
        Next i
    End Function
End Class
