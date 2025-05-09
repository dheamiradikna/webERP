Imports System.Drawing
Imports [class].clsUploadItem
Imports [class].clsGeneral
Imports [class].clsWebGeneral

Partial Class wf_support_displayImage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim q_module As String = Request.QueryString("m")
        Dim q_height As String = Request.QueryString("h") 'kanvas height
        Dim q_width As String = Request.QueryString("w") 'kanvas width
        Dim q_resizePct As String = Request.QueryString("pct")
        Dim q_isFullBackground As String = Request.QueryString("isBg")

        If Trim(q_height) = "" Then q_height = "0"
        If Trim(q_width) = "" Then q_width = "0"
        If Trim(q_resizePct) = "" Then q_resizePct = "0"

        Dim dt As New DataTable

        Select Case q_module
            Case "domainLogo"
                Dim q_domainRef As String = Request.QueryString("d")
                dt = getDomainLogo(q_domainRef)
            Case "domainImage"
                Dim q_domainRef As String = Request.QueryString("d")
                dt = getDomainImage(q_domainRef)
            Case "tag"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_ref As String = Request.QueryString("r")
                dt = getTagPicture(q_domainRef, q_ref)
            Case "content"
                Dim q_domainRef As String = Request.QueryString("d")
                Dim q_imageRef As String = Request.QueryString("ir")
                dt = getIMG_TR_imagePic(q_domainRef, q_imageRef)
            Case Else
                Dim q_imgID As String = Request.QueryString("iid")
        End Select

        Dim attach As Byte() = {0}
        Dim fileName As String = ""

        If dt.Rows.Count > 0 Then
            attach = dt.Rows(0).Item("imgFile")
            fileName = dt.Rows(0).Item("imgFileName")
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
                'attach = resizePicture(attach, newW, newH, fileExt, newW, newH)

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
                    'attach = resizePicture(attach, newW, newH, fileExt, newW, newH)
                    attach = combine(attach, q_width, q_height, newW, newH, fileExt)
                    Dim msResize As New System.IO.MemoryStream(attach)
                    image = System.Drawing.Image.FromStream(msResize)
                End If

            End If

            Select Case fileExt.ToLower
                Case "gif"
                    Response.ContentType = "image/gif"
                    image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif)
                Case Else
                    Response.ContentType = "image/jpeg"
                    image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
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

            picFileDataBackGround = doWaterMark(picFileDataBackGround, "no thumbnail", 0.5)

            Dim msBackGroundOk As New System.IO.MemoryStream(picFileDataBackGround)
            Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(msBackGroundOk)

            Response.ContentType = "image/jpeg"
            image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)

        End If


        
    End Sub
End Class
