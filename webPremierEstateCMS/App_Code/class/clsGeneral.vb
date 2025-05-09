Imports Microsoft.VisualBasic
Imports System.Drawing
Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].cls_sTag

Namespace [class]
    Public Class clsGeneral

        Public Shared sortTypeValue() As String = {"0", "1", "2"}
        Public Shared sortTypeText() As String = {"-", "Ascending", "Descending"}
        Public Shared sortTypeDB() As String = {"-", "asc", "desc"}

        Public Shared monthValue() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"}
        Public Shared monthName() As String = {"Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"}
        Public Shared monthNameShort() As String = {"Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"}

        Public Shared _keywordSplitter As String = ","

        Public Shared Function getTagParentNameRekursif(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim result As String = ""

            While tagRef > 0
                result = getTagName(domainRef, tagRef) + " :: " + result
                tagRef = getTagRefParent(domainRef, tagRef)
            End While


            Return Left(result, Len(result) - 4)
        End Function

        Public Shared Function bindDomainLoad(ByVal value As String, ByRef firstDomainRef As String) As String
            Dim result As New StringBuilder
            Dim i As Integer
            Dim dt As New DataTable

            dt = getDomainListLookup()
            If dt.Rows.Count > 0 Then
                firstDomainRef = dt.Rows(0).Item("domainRef").ToString
                result.Append("<select class=""pd0"" style=""height:20px;"" id=""selDomain"" name=""selDomain"" onchange=""doLoadDomain()"" >")

                For i = 0 To dt.Rows.Count - 1
                    If value = dt.Rows(i).Item("domainRef").ToString Then
                        result.Append("<option selected value=""" + dt.Rows(i).Item("domainRef").ToString + """>" + dt.Rows(i).Item("domainName") + "</option>")
                    Else
                        result.Append("<option value=""" + dt.Rows(i).Item("domainRef").ToString + """>" + dt.Rows(i).Item("domainName") + "</option>")
                    End If
                Next
                result.Append("</select> ")
            End If
            Return result.ToString
        End Function

        Public Shared Function bindDomainLoadMall(ByVal value As String, ByRef firstDomainRef As String) As String
            Dim result As New StringBuilder
            Dim i As Integer
            Dim dt As New DataTable

            dt = getDomainListLookupMall()
            If dt.Rows.Count > 0 Then
                firstDomainRef = dt.Rows(0).Item("domainRef").ToString
                result.Append("<select class=""pd0"" style=""height:20px;"" id=""selDomain"" name=""selDomain"" onchange=""doLoadDomain()"" >")

                For i = 0 To dt.Rows.Count - 1
                    If value = dt.Rows(i).Item("domainRef").ToString Then
                        result.Append("<option selected value=""" + dt.Rows(i).Item("domainRef").ToString + """>" + dt.Rows(i).Item("domainName") + "</option>")
                    Else
                        result.Append("<option value=""" + dt.Rows(i).Item("domainRef").ToString + """>" + dt.Rows(i).Item("domainName") + "</option>")
                    End If
                Next
                result.Append("</select> ")
            End If
            Return result.ToString
        End Function

        Public Shared Function bindSelSortType(ByVal value As String) As String
            Dim result As New StringBuilder
            Dim i As Integer

            result.Append("<select id=""selSortType"" name=""selSortType"" >")
            For i = 0 To sortTypeValue.Length - 1
                If value = sortTypeValue(i) Then
                    result.Append("<option selected value=""" + sortTypeValue(i) + """>" + sortTypeText(i) + "</option>")
                Else
                    result.Append("<option value=""" + sortTypeValue(i) + """>" + sortTypeText(i) + "</option>")
                End If
            Next
            result.Append("</select> ")

            Return result.ToString
        End Function

        Public Shared Function bindSelSortBy(ByVal sortByValue() As String, ByVal sortByText() As String, _
                                       ByVal value As String) As String
            Dim result As New StringBuilder
            Dim i As Integer

            result.Append("<select id=""selSortBy"" name=""selSortBy"" >")
            For i = 0 To sortByValue.Length - 1
                If value = sortByValue(i) Then
                    result.Append("<option selected value=""" + sortByValue(i) + """>" + sortByText(i) + "</option>")
                Else
                    result.Append("<option value=""" + sortByValue(i) + """>" + sortByText(i) + "</option>")
                End If
            Next
            result.Append("</select> ")

            Return result.ToString
        End Function

        Public Shared Function GetDecParam(ByVal param As String) As Hashtable
            Dim paramEnc As String = New UTF8Encoding().GetString(Convert.FromBase64String(param))
            Dim pValue As String() = paramEnc.Split("&"c)
            Dim encParams As New Hashtable
            Dim i As Integer
            For i = 0 To pValue.Length - 1
                Dim nameP As String() = pValue(i).Split("="c)
                If nameP.Length = 2 Then
                    encParams.Add(nameP(0), HttpUtility.UrlDecode(nameP(1)).Trim())
                End If
            Next i
            Return encParams
        End Function 'GetDecParam

        Public Shared Function GetEncUrl(ByVal pageName As String, ByVal parameters As Hashtable) As String
            Dim queryStr As New StringBuilder
            Dim isBegin As Boolean = True
            Dim temp As String
            Dim key As String

            For Each key In parameters.Keys
                If isBegin Then
                    isBegin = False
                Else
                    queryStr.Append("&"c)
                End If
                If parameters(key) Is Nothing Then
                    temp = ""
                Else
                    temp = parameters(key).ToString()
                End If
                queryStr.Append(key + "=" + HttpUtility.UrlEncode(temp)) 'ToDo: Unsupported feature: conditional (?) operator.
            Next key

            Return pageName + "x=" + Convert.ToBase64String(New UTF8Encoding().GetBytes(queryStr.ToString))
            'Return pageName + "?x=" + Convert.ToBase64String(Convert.ToByt(queryStr.ToString))
        End Function 'GetEncUrl

        ''''''' about JSON ''''''
        ''''''' about JSON ''''''
        ''''''' about JSON ''''''
        ''''''' about JSON ''''''
        ''''''' about JSON ''''''

        Public Shared Function MyURLEncode(ByVal text As String) As String
            Dim huruf() As String = {" ", """", "#", "$", "%", "&", "+", ",", "/", ":", ";", "<", _
                        "=", ">", "?", "@", "[", "\", "]", "^", "`", _
                        "{", "|", "}", "~", Chr(13), Chr(10), "'"}
            Dim hurufEncode() As String = {"%20", "%22", "%23", "%24", "%25", "%26", "%2b", "%2c", "%2f", "%3a", "%3b", "%3c", _
                        "%3d", "%3e", "%3f", "%40", "%5b", "%5c", "%5d", "%5e", "%60", _
                        "%7b", "%7c", "%7d", "%7e", "%0d", "%0a", "%27"}


            Dim i As Integer
            Dim j As Integer
            Dim result As String = ""

            If Trim(text) <> "" Then

                For i = 0 To text.Length - 1
                    Dim hurufHasil As String = ""
                    For j = 0 To huruf.Length - 1
                        If text(i) = huruf(j) Then

                            hurufHasil = hurufEncode(j)
                            Exit For
                        End If
                        If Asc(text(i)) = 13 Then
                            hurufHasil = ""
                        End If
                    Next
                    If hurufHasil = "" Then
                        result = result + text(i)
                    Else
                        result = result + hurufHasil
                    End If
                Next

            End If


            Return result
        End Function

        Public Shared Function convertDStoJSON(ByVal ds As DataSet) As String
            Dim result As New StringBuilder
            Dim i As Integer, j As Integer, d As Integer

            result.Append("{")
            For d = 0 To ds.Tables.Count - 1
                result.Append(ds.Tables(d).TableName + ":[")
                For i = 0 To ds.Tables(d).Rows.Count - 1
                    result.Append("{")
                    For j = 0 To ds.Tables(d).Columns.Count - 1
                        result.Append("""" + ds.Tables(d).Columns(j).ColumnName + """:""" + _
                                MyURLEncode(ds.Tables(d).Rows(i).Item(j).ToString) + """")
                        If j < ds.Tables(d).Columns.Count - 1 Then
                            result.Append(",")
                        End If
                    Next
                    result.Append("}")
                    If i < ds.Tables(d).Rows.Count - 1 Then
                        result.Append(",")
                    End If
                Next
                result.Append("]")
                If d < ds.Tables.Count - 1 Then
                    result.Append(",")
                End If
            Next
            result.Append("}")


            Return result.ToString
        End Function

        Public Shared Function convertDTtoJSON(ByVal DTName As String, ByVal dt As DataTable) As String
            Dim result As New StringBuilder
            Dim i As Integer, j As Integer

            result.Append("{" + DTName + ":[")
            For i = 0 To dt.Rows.Count - 1
                result.Append("{")
                For j = 0 To dt.Columns.Count - 1
                    result.Append("""" + dt.Columns(j).ColumnName + """:""" + _
                            MyURLEncode(dt.Rows(i).Item(j).ToString) + """")
                    If j < dt.Columns.Count - 1 Then
                        result.Append(",")
                    End If
                Next
                result.Append("}")
                If i < dt.Rows.Count - 1 Then
                    result.Append(",")
                End If
            Next
            result.Append("]}")

            Return result.ToString
        End Function

        ''''''' end of about JSON ''''''
        ''''''' end of about JSON ''''''
        ''''''' end of about JSON ''''''
        ''''''' end of about JSON ''''''
        ''''''' end of about JSON ''''''


        Public Shared Function bindPaging(ByVal pageCount As String, ByVal pageNo As Integer, ByVal numOfData As Integer, _
                           ByVal url As String, Optional ByVal selectColor As String = "") As String
            Dim result As New StringBuilder
            Dim i As Integer

            If numOfData > 0 Then
                Dim totalPage As Int16 = Math.Ceiling(numOfData / pageCount)
                If pageNo > totalPage Then
                    pageNo = totalPage
                End If


                Dim startPage As Integer = (System.Math.Floor((pageNo - 1) / _numOfDisplayPage) * _numOfDisplayPage) + 1
                Dim endPage As Integer = System.Math.Ceiling(numOfData / pageCount)

                result.Append("Page:&nbsp;&nbsp;")

                If startPage > _numOfDisplayPage Then
                    result.Append("<a href=""" + Replace(url, "@p", (startPage - 1).ToString) + """>...</a> | ")
                End If

                For i = startPage To startPage + _numOfDisplayPage
                    Dim p As String = i.ToString
                    If i = startPage + _numOfDisplayPage Then
                        p = "..."
                    End If
                    If i = pageNo Then
                        Dim textColor As String = ""
                        If Trim(selectColor) <> "" Then
                            textColor = "color:" + selectColor + ";"
                        End If
                        result.Append("<a href=""" + Replace(url, "@p", i.ToString) + """><font style=""font-size:100%; " + textColor + """><strong>" + p + "</strong></font></a>")
                    Else
                        result.Append("<a href=""" + Replace(url, "@p", i.ToString) + """>" + p + "</a>")
                    End If

                    If i >= endPage Then
                        Exit For
                    End If

                    If i < startPage + _numOfDisplayPage Then
                        result.Append(" | ")
                    End If
                Next
            End If

            Return result.ToString

        End Function

        Public Shared Function emailAddressCheck(ByVal emailAddress As String) As Boolean

            Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
            If emailAddressMatch.Success Then
                emailAddressCheck = True
            Else
                emailAddressCheck = False
            End If

        End Function

        Public Shared Function filterFileName(ByVal fn As String) As String
            'Dim result As String = fn
            Dim result As New StringBuilder

            For i = 0 To fn.Count - 1
                If (fn.Chars(i) >= "0" And fn.Chars(i) <= "9") Or (fn.Chars(i) >= "a" And fn.Chars(i) <= "z") Or (fn.Chars(i) >= "A" And fn.Chars(i) <= "Z") Or fn.Chars(i) = "." Then
                    result.Append(fn.Chars(i))
                End If
            Next

            Return result.ToString
        End Function

        Public Shared Function combine(ByVal picFileData As Byte(), ByVal bgW As Integer, ByVal bgH As Integer, _
                                       ByVal w As Integer, ByVal h As Integer, ByVal picFileExt As String) As Byte()
            Dim ms As New System.IO.MemoryStream(picFileData)
            Dim i, j As Integer

            Dim imgFront As New Bitmap(ms)

            Dim imgCombine As New Bitmap(bgW, bgH)
            For i = 0 To bgW - 1
                For j = 0 To bgH - 1
                    imgCombine.SetPixel(i, j, Color.FromArgb(CInt(_imageBgColor.Split("|")(0)), CInt(_imageBgColor.Split("|")(1)), CInt(_imageBgColor.Split("|")(2))))
                Next
            Next

            Dim g As System.Drawing.Graphics = Graphics.FromImage(imgCombine)
            g.DrawImage(imgFront, New Point(((bgW - w) / 2), (bgH - h) / 2))

            g.Dispose()
            g = Nothing

            Dim msCombine As New System.IO.MemoryStream
            Select Case picFileExt.ToLower
                Case "gif"
                    imgCombine.Save(msCombine, Imaging.ImageFormat.Gif)
                Case Else
                    imgCombine.Save(msCombine, Imaging.ImageFormat.Jpeg)
            End Select

            Dim picFileDataCombine(msCombine.Length - 1) As Byte
            msCombine.Position = 0
            msCombine.Read(picFileDataCombine, 0, msCombine.Length)

            Return picFileDataCombine
        End Function

        Public Shared Function MakeTransparentGif(ByVal bitmap As Bitmap, ByVal color As Color) As Bitmap


            Dim R As Byte = color.R
            Dim G As Byte = color.G
            Dim B As Byte = color.B

            Dim fin As New System.IO.MemoryStream()
            bitmap.Save(fin, System.Drawing.Imaging.ImageFormat.Gif)

            Dim fout As New System.IO.MemoryStream(CInt(fin.Length))
            Dim count As Integer = 0
            Dim buf As Byte() = New Byte(255) {}
            Dim transparentIdx As Byte = 0
            fin.Seek(0, System.IO.SeekOrigin.Begin)
            'header
            count = fin.Read(buf, 0, 13)
            If (buf(0) <> 71) OrElse (buf(1) <> 73) OrElse (buf(2) <> 70) Then
                Return Nothing
            End If
            'GIF
            fout.Write(buf, 0, 13)

            Dim i As Integer = 0
            If (buf(10) And &H80) > 0 Then
                i = If(1 << ((buf(10) And 7) + 1) = 256, 256, 0)
            End If

            While i <> 0
                fin.Read(buf, 0, 3)
                If (buf(0) = R) AndAlso (buf(1) = G) AndAlso (buf(2) = B) Then
                    transparentIdx = CByte((256 - i))
                End If
                fout.Write(buf, 0, 3)
                i -= 1
            End While

            Dim gcePresent As Boolean = False
            While True
                fin.Read(buf, 0, 1)
                fout.Write(buf, 0, 1)
                If buf(0) <> &H21 Then
                    Exit While
                End If
                fin.Read(buf, 0, 1)
                fout.Write(buf, 0, 1)
                gcePresent = (buf(0) = &HF9)
                While True
                    fin.Read(buf, 0, 1)
                    fout.Write(buf, 0, 1)
                    If buf(0) = 0 Then
                        Exit While
                    End If
                    count = buf(0)
                    If fin.Read(buf, 0, count) <> count Then
                        Return Nothing
                    End If
                    If gcePresent Then
                        If count = 4 Then
                            buf(0) = buf(0) Or &H1
                            buf(3) = transparentIdx
                        End If
                    End If
                    fout.Write(buf, 0, count)
                End While
            End While
            While count > 0
                count = fin.Read(buf, 0, 1)
                fout.Write(buf, 0, 1)
            End While
            fin.Close()
            fout.Flush()

            Return New Bitmap(fout)
        End Function

        Public Shared Function resizePicture(ByVal picFileData As Byte(), ByVal w As Integer, ByVal h As Integer, _
                                  ByVal picFileExt As String, ByRef newW As Integer, ByRef newH As Integer) As Byte()

            Dim MS As New System.IO.MemoryStream(picFileData)
            Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(MS)

            MS = Nothing

            Dim newWidth As Integer
            Dim newHeight As Integer

            If w <> 0 And h <> 0 Then
                'jika keduanya tidak NOL
                newWidth = w
                newHeight = h

            ElseIf w <> 0 Then
                newWidth = w
                newHeight = (CSng(w) / image.Width) * image.Height
            ElseIf h <> 0 Then
                newHeight = h
                newWidth = (CSng(h) / image.Height) * image.Width
            End If

            newW = newWidth
            newH = newHeight

            Dim thumbnail As System.Drawing.Image = New Bitmap(newWidth, newHeight)
            Dim graphic As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(thumbnail)

            graphic.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphic.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graphic.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            graphic.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

            ''in case ada gif, ini bisa digunakan untuk merubah transparan jadi RGB tertentu, dimana entar pada saat load diganti jadi transparant argbnya, jadi gak item
            If picFileExt.ToLower = "gif" Then
                Dim sb = New SolidBrush(System.Drawing.Color.FromArgb(255, 255, 204))
                graphic.FillRectangle(sb, 0, 0, newWidth, newHeight)
            End If


            graphic.DrawImage(image, 0, 0, newWidth, newHeight)

            Dim msResize As New System.IO.MemoryStream
            Select Case picFileExt.ToLower
                Case "gif"
                    thumbnail = MakeTransparentGif(thumbnail, Color.FromArgb(255, 255, 204))
                    thumbnail.Save(msResize, Imaging.ImageFormat.Gif)
                Case Else
                    thumbnail.Save(msResize, Imaging.ImageFormat.Jpeg)
            End Select
            Dim picFileDataResize(msResize.Length - 1) As Byte
            msResize.Position = 0
            msResize.Read(picFileDataResize, 0, msResize.Length)

            Return picFileDataResize
        End Function

        Public Shared Function doWaterMark(ByVal picFileData As Byte(), ByVal strWatermark As String, ByVal ratioWidth As Single) As Byte()
            Dim MS As New System.IO.MemoryStream(picFileData)
            Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(MS)

            Dim canvas As Graphics = Graphics.FromImage(image)
            Dim StringSizeF As SizeF
            Dim DesiredWidth As Single
            Dim wmFont As Font
            Dim RequiredFontSize As Single
            Dim Ratio As Single

            wmFont = New Font("Tahoma", 14, FontStyle.Bold)
            DesiredWidth = image.Width * ratioWidth
            StringSizeF = canvas.MeasureString(strWatermark, wmFont)
            Ratio = StringSizeF.Width / wmFont.SizeInPoints
            RequiredFontSize = DesiredWidth / Ratio

            wmFont = New Font("Tahoma", RequiredFontSize, FontStyle.Bold)

            canvas.DrawString(strWatermark, wmFont, New SolidBrush(Color.FromArgb(128, 0, 0, 0)), (image.Width / 2) - (image.Width * ratioWidth / 2) + 2, (image.Height / 2) - (RequiredFontSize) + 2)
            canvas.DrawString(strWatermark, wmFont, New SolidBrush(Color.FromArgb(128, 255, 255, 255)), (image.Width / 2) - (image.Width * ratioWidth / 2), (image.Height / 2) - (RequiredFontSize))

            Dim msResult As New System.IO.MemoryStream

            image.Save(msResult, Imaging.ImageFormat.Jpeg)

            Dim picFileDataNew(msResult.Length - 1) As Byte
            msResult.Position = 0
            msResult.Read(picFileDataNew, 0, msResult.Length)

            Return picFileDataNew
        End Function

        Public Shared Sub cekIsNotLoginWebAdmin()
            If HttpContext.Current.Session("isAdmin") Is Nothing Then
                HttpContext.Current.Response.Redirect(_rootPath + "admin")
            End If
        End Sub

        Public Shared Sub cekIsNotLoginWebAdminPopup()
            If HttpContext.Current.Session("isAdmin") Is Nothing Then
                Dim Hashtable As New Hashtable

                Hashtable("note") = "Sorry, your session expired, please re-login.<br/><br/>" + _
                                    "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                    "Thankyou."

                HttpContext.Current.Response.Redirect(GetEncUrl(_rootPath + "wf/admin/notificationpopup.aspx?", Hashtable))
            End If

        End Sub

        Public Shared Sub cekIsNotLoginWeb()
            If HttpContext.Current.Session("domainRef") Is Nothing Then
                HttpContext.Current.Response.Redirect(_rootPath + "default.aspx")
            End If
        End Sub

        Public Shared Sub cekIsNotLoginWebPopup()
            If HttpContext.Current.Session("domainRef") Is Nothing Then
                Dim Hashtable As New Hashtable

                Hashtable("note") = "Sorry, your session expired, please re-login.<br/><br/>" + _
                                    "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                    "Thankyou."

                HttpContext.Current.Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))
            End If
            
        End Sub

        Public Shared Function GetFileImageSize(ByVal TheFile As Byte()) As String
            Dim DoubleBytes As Double

            'If TheFile.Length = 0 Then Return ""
            
            ''If Not System.IO.File.Exists(TheFile) Then Return ""
            ''---
            ''Dim TheSize As ULong = My.Computer.FileSystem.GetFileInfo(TheFile).Length
            'Dim decodedBytes As Byte()
            'decodedBytes = Convert.FromBase64String(TheFile)

            Dim TheSize As ULong = TheFile.Length
            Dim SizeType As String = ""
            '---

            Try
                Select Case TheSize
                    Case Is >= 1099511627776
                        DoubleBytes = CDbl(TheSize / 1099511627776) 'TB
                        Return FormatNumber(DoubleBytes) & " TB"
                    Case 1073741824 To 1099511627775
                        DoubleBytes = CDbl(TheSize / 1073741824) 'GB
                        Return FormatNumber(DoubleBytes) & " GB"
                    Case 1048576 To 1073741823
                        DoubleBytes = CDbl(TheSize / 1048576) 'MB
                        Return FormatNumber(DoubleBytes) & " MB"
                    Case 1024 To 1048575
                        DoubleBytes = CDbl(TheSize / 1024) 'KB
                        Return FormatNumber(DoubleBytes) & " KB"
                    Case 0 To 1023
                        DoubleBytes = TheSize ' bytes
                        Return FormatNumber(DoubleBytes) & " bytes"
                    Case Else
                        Return ""
                End Select
            Catch
                Return ""
            End Try
        End Function
        Public Shared Function bindNotif(ByVal notifType As String, ByVal note As String) As String
            Dim result As New StringBuilder

            ''notif type sukses = 1
            ''notif type error = 0

            If notifType = "1" Then
                result.Append("     <div class=""alert alert-success fade in alert-dismissible"" role=""alert"">")
                result.Append("         <strong>Notification!</strong> " + note + "")
                result.Append("     </div>")
            ElseIf notifType = "0" Then
                result.Append("     <div class=""alert alert-danger fade in alert-dismissible"" role=""alert"">")
                result.Append("         <strong>Notification!</strong> " + note + "")
                result.Append("     </div>")
            Else
                If note = "" Then
                    note = "For input new item, please click on icon + above."
                End If

                result.Append("     <div class=""alert alert-info fade in alert-dismissible"" role=""alert"">")
                result.Append("         <strong>Notification!</strong> " + note + "")
                result.Append("     </div>")
            End If

            Return result.ToString
        End Function
       
    End Class
End Namespace

