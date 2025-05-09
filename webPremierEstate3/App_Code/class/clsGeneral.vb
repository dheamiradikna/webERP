Imports System.IO
Imports System.Drawing
Imports System.Data
Imports Microsoft.VisualBasic
Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].clsGeneral
Imports [class].clsContentDB
Imports Aspose.Email.Mail
Imports Aspose.Email

Namespace [class]
    Public Class clsGeneral
        Public Shared sortTypeValue() As String = {"0", "1", "2"}
        Public Shared sortTypeText() As String = {"-", "Ascending", "Descending"}
        Public Shared sortTypeDB() As String = {"-", "asc", "desc"}

        Public Shared monthValue() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"}
        Public Shared monthName() As String = {"Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"}
        Public Shared monthNameShort() As String = {"Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"}

        Public Shared _keywordSplitter As String = ","

        Public Shared Function checkMobileDisplay() As Boolean
            Dim result As Boolean = False

            Dim u As String = HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT")
            Dim b As New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase)
            Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
            If b.IsMatch(u) Or v.IsMatch(Left(u, 4)) Then
                result = True
            End If

            Return result
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

        Public Shared Function bindPaging(ByVal pageCount As String, ByVal pageNo As Integer, ByVal numOfData As Integer, _
                                          ByVal url As String) As String
            Dim result As New StringBuilder
            Dim i As Integer

            If numOfData > 0 Then
                Dim totalPage As Int16 = Math.Ceiling(numOfData / pageCount)
                If pageNo > totalPage Then
                    pageNo = totalPage
                End If

                Dim startPage As Integer = (System.Math.Floor((pageNo - 1) / _numOfDisplayPage) * _numOfDisplayPage) + 1
                Dim endPage As Integer = System.Math.Ceiling(numOfData / pageCount)
                'result.Append("            <div class=""pagination text-center"">")
                'result.Append("              <nav class=""pagination__nav clearfix"">")
                'result.Append("                <a href=""#"" class=""pagination__page""><i class=""fa fa-angle-left""></i></a>")
                'result.Append("                <span class=""pagination__page active"">1</span>")
                'result.Append("                <a href=""#"" class=""pagination__page""><i class=""fa fa-angle-right""></i></a>")
                'result.Append("              </nav>")         
                'result.Append("            </div>")

                result.Append("<div class=""pagination text-center"">")
                result.Append("<nav class=""pagination__nav clearfix"">")
                If pageNo = 1 Then
                    ' result.Append(" <class=""pagination__page""><i class=""fa fa-angle-left""></i>")
                    result.Append("                <a href=""#"" class=""pagination__page""><div class=""sprite2 sprite-left-arrow""></div></a>")
                Else
                    result.Append("<a href=""" + Replace(url, "@p", (pageNo - 1).ToString) + """  class=""pagination__page"" ><div class=""sprite2 sprite-left-arrow""></div></a>")
                End If

                If startPage > _numOfDisplayPage Then
                    result.Append("<a href=""" + Replace(url, "@p", (startPage - 1).ToString) + """  class=""pagination__page"">...</a>")
                End If

                For i = startPage To startPage + _numOfDisplayPage
                    Dim p As String = i.ToString
                    If i = startPage + _numOfDisplayPage Then
                        p = "..."
                    End If
                    If i = pageNo Then
                        'Active Page
                        result.Append("<span class=""pagination__page active"">" + p + "</span>")
                    Else
                        result.Append("<a href=""" + Replace(url, "@p", i.ToString) + """  class=""pagination__page"">" + p + "</a>")
                    End If

                    If i >= endPage Then
                        Exit For
                    End If
                Next

                If pageNo = totalPage Then
                    ' result.Append("< class=""disabled""><span>&raquo;</span>")
                    result.Append("                <a href=""#"" class=""pagination__page""><div class=""sprite2 sprite-next""></div></a>")
                Else
                    result.Append("<a href=""" + Replace(url, "@p", (pageNo + 1).ToString) + """ class=""pagination__page"" ><div class=""sprite2 sprite-next""></div></a>")
                End If
                result.Append("</nav>")
                result.Append("</div>")
            End If

            Return result.ToString
        End Function

        ' Public Shared Function bindPaging(ByVal pageCount As String, ByVal pageNo As Integer, ByVal numOfData As Integer, _
        '                                  ByVal url As String) As String
        '    Dim result As New StringBuilder
        '    Dim i As Integer

        '    If numOfData > 0 Then
        '        Dim totalPage As Int16 = Math.Ceiling(numOfData / pageCount)
        '        If pageNo > totalPage Then
        '            pageNo = totalPage
        '        End If

        '        Dim startPage As Integer = (System.Math.Floor((pageNo - 1) / _numOfDisplayPage) * _numOfDisplayPage) + 1
        '        Dim endPage As Integer = System.Math.Ceiling(numOfData / pageCount)

        '        result.Append("<div class=""pagination-box"">")
        '        result.Append("<ul class=""pagination"">")
        '        If pageNo = 1 Then
        '            result.Append("<li class=""disabled""><span>&laquo;</span></li>")
        '        Else
        '            result.Append("<li><a href=""" + Replace(url, "@p", (pageNo - 1).ToString) + """>&laquo;</a></li>")
        '        End If

        '        If startPage > _numOfDisplayPage Then
        '            result.Append("<li><a href=""" + Replace(url, "@p", (startPage - 1).ToString) + """>...</a></li>")
        '        End If

        '        For i = startPage To startPage + _numOfDisplayPage
        '            Dim p As String = i.ToString
        '            If i = startPage + _numOfDisplayPage Then
        '                p = "..."
        '            End If
        '            If i = pageNo Then
        '                'Active Page
        '                result.Append("<li class=""active""><span>" + p + "</span></li>")
        '            Else
        '                result.Append("<li><a href=""" + Replace(url, "@p", i.ToString) + """>" + p + "</a></li>")
        '            End If

        '            If i >= endPage Then
        '                Exit For
        '            End If
        '        Next

        '        If pageNo = totalPage Then
        '            result.Append("<li class=""disabled""><span>&raquo;</span></li>")
        '        Else
        '            result.Append("<li><a href=""" + Replace(url, "@p", (pageNo + 1).ToString) + """>&raquo;</a></li>")
        '        End If
        '        result.Append("</ul>")
        '        result.Append("</div>")
        '    End If

        '    Return result.ToString
        'End Function
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

        Public Shared Function emailAddressCheck(ByVal emailAddress As String) As Boolean

            Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
            If emailAddressMatch.Success Then
                emailAddressCheck = True
            Else
                emailAddressCheck = False
            End If

        End Function

        'Public Shared Function filterFileName(ByVal fn As String) As String
        '    'Dim result As String = fn
        '    Dim result As New StringBuilder

        '    For i = 0 To fn.Count - 1
        '        If (fn.Chars(i) >= "0" And fn.Chars(i) <= "9") Or (fn.Chars(i) >= "a" And fn.Chars(i) <= "z") Or (fn.Chars(i) >= "A" And fn.Chars(i) <= "Z") Or fn.Chars(i) = "." Then
        '            result.Append(fn.Chars(i))
        '        End If
        '    Next

        '    Return result.ToString
        'End Function

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

            'canvas.DrawString(strWatermark, wmFont, New SolidBrush(Color.FromArgb(128, 0, 0, 0)), 2, 2)
            'canvas.DrawString(strWatermark, wmFont, New SolidBrush(Color.FromArgb(128, 255, 255, 255)), 0, 0)
            canvas.DrawString(strWatermark, wmFont, New SolidBrush(Color.FromArgb(128, 0, 0, 0)), (image.Width / 2) - (image.Width * ratioWidth / 2) + 2, (image.Height / 2) - (RequiredFontSize) + 2)
            canvas.DrawString(strWatermark, wmFont, New SolidBrush(Color.FromArgb(128, 255, 255, 255)), (image.Width / 2) - (image.Width * ratioWidth / 2), (image.Height / 2) - (RequiredFontSize))

            Dim msResult As New System.IO.MemoryStream

            'Select Case picFileExt.ToLower
            '    Case "bmp"
            '        image.Save(msResult, Imaging.ImageFormat.Png)
            '    Case Else
            '        image.Save(msResult, Imaging.ImageFormat.Jpeg)
            'End Select

            image.Save(msResult, Imaging.ImageFormat.Jpeg)

            Dim picFileDataNew(msResult.Length - 1) As Byte
            msResult.Position = 0
            msResult.Read(picFileDataNew, 0, msResult.Length)

            Return picFileDataNew
        End Function

        Public Shared Sub cekIsNotLoginWeb()
            If HttpContext.Current.Session("userName") Is Nothing Then
                HttpContext.Current.Response.Redirect(_rootPath + "Default.aspx")
            End If
        End Sub

        Public Shared Sub cekIsNotLoginWebPopup()
            If HttpContext.Current.Session("userName") Is Nothing Then
                Dim Hashtable As New Hashtable

                Hashtable("note") = "Sorry, your session expired, please re-login.<br/><br/>" + _
                                    "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                    "Thankyou."

                HttpContext.Current.Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))
            End If

        End Sub

        '===== HTML Tag remover =====

        Private Shared Function FoundOpener(ByVal text As String, ByVal Position As Long) As Long
            Dim CloserPosition As Long
            CloserPosition = InStr(Position + 1, text, ">", CompareMethod.Binary)
            If CloserPosition = 0 Then
                CloserPosition = Len(text)

            End If
            Return CloserPosition

        End Function

        Private Shared Function CalculateLength(ByVal start As Long, ByVal final As Long) As Long
            Return (final - start)
        End Function

        Public Shared Function HTMLTagRemover(ByVal srctext As String) As String
            Dim Counter As Long
            Dim CloserPosition As Long
            Dim length As Long
            Dim srcLength As Long = Len(srctext) - 1

            Do While Counter <= srcLength


                If srctext.Chars(Counter) = "<" Then
                    CloserPosition = FoundOpener(srctext, Counter)
                    length = CalculateLength(Counter, CloserPosition)
                    srctext = srctext.Remove(Counter, length)


                    srcLength = Len(srctext) - 1
                    Counter -= 1

                End If
                Counter += 1

            Loop

            Return srctext
        End Function

        '##### HTML Tag remover #####

        Public Shared Function convertStrToParam(ByVal str As String) As String
            'Dim temp As String = Replace(Replace(Trim(HTMLTagRemover(str.ToLower)), ".", ""), " ", ".")
            Dim replaceChar As String = "-"
            Dim temp As String = Replace(Trim(HTMLTagRemover(str.ToLower)), " ", replaceChar)
            Dim i As Integer
            Dim result As New StringBuilder
            Dim isDot As Boolean = False


            If Trim(temp) <> "" Then
                For i = 0 To temp.Length - 1
                    If (temp.Chars(i) >= "a" And temp.Chars(i) <= "z") Or (temp.Chars(i) >= "0" And temp.Chars(i) <= "9") Or temp.Chars(i) = replaceChar Then

                        If temp.Chars(i) = replaceChar Then
                            If Not isDot Then
                                result.Append(temp.Chars(i))
                            End If
                        Else
                            result.Append(temp.Chars(i))
                        End If

                        If temp.Chars(i) = replaceChar Then
                            isDot = True
                        Else
                            isDot = False
                        End If
                    Else
                        If Not isDot Then
                            result.Append(replaceChar)
                            isDot = True
                        End If
                        'result.Append("-")
                    End If
                Next
            Else
                result.Append("-")
            End If


            Dim resultStr As String = result.ToString
            If resultStr.Length >= 2 Then
                If resultStr.Chars(resultStr.Length - 1) = replaceChar Then
                    resultStr = Left(resultStr, Len(resultStr) - 1)
                End If
            End If

            Return resultStr

        End Function

        Public Shared Function convertStrToTitle(ByVal str As String) As String
            Dim replaceChar As String = "-"
            Dim temp As String = Replace(Trim(HTMLTagRemover(str.ToLower)), " ", replaceChar)
            Dim i As Integer
            Dim result As New StringBuilder
            Dim isDot As Boolean = False


            If Trim(temp) <> "" Then
                For i = 0 To temp.Length - 1
                    If (temp.Chars(i) >= "a" And temp.Chars(i) <= "z") Or (temp.Chars(i) >= "0" And temp.Chars(i) <= "9") Or temp.Chars(i) = ":" Or temp.Chars(i) = replaceChar Then

                        If temp.Chars(i) = replaceChar Then
                            If Not isDot Then
                                result.Append(temp.Chars(i))
                            End If
                        Else
                            result.Append(temp.Chars(i))
                        End If

                        If temp.Chars(i) = replaceChar Then
                            isDot = True
                        Else
                            isDot = False
                        End If
                    Else
                        If Not isDot Then
                            result.Append(replaceChar)
                            isDot = True
                        End If
                        'result.Append("-")
                    End If
                Next
                'Else
                '    result.Append("-")
            End If


            Dim resultStr As String = result.ToString
            If resultStr.Length >= 2 Then
                If resultStr.Chars(resultStr.Length - 1) = replaceChar Then
                    resultStr = Left(resultStr, Len(resultStr) - 1)
                End If
            End If

            resultStr = Replace(resultStr, replaceChar, " ")

            Return resultStr

        End Function

        Public Shared Function replaceEnterToBR(ByVal str As String) As String
            Return Replace(str, Chr(13) + Chr(10), "<br>")
        End Function

        Public Shared Function RenderControlToHtml(ByVal controlToRender As Control) As String
            Dim sb As New StringBuilder
            Dim strWrite As New StringWriter(sb)
            Dim htmlWriter As New HtmlTextWriter(strWrite)
            controlToRender.RenderControl(htmlWriter)
            Return sb.ToString
        End Function

        Public Shared Function SendEmail(ByVal emailType As EmailType, ByVal subject As String, ByVal mailTo() As String, ByVal mailCc() As String, ByVal mailBcc() As String, ByVal data As DataTable) As String
            Dim result As String = ""

            'Dim license As New Aspose.Email.License()
            'license.SetLicense(LicenseHelper.License.LStream)
            Dim imageFooter As String = _rootPath + "http://www.Kotasutera.com/support/img/logo-footer.png"
            Dim imageSideTop As String = _rootPath + "http://www.nataproperty.com/support/img/image-side-top.png"
            Dim imageSideProjectBottom As String = _rootPath + "http://www.Kotasutera.com/support/img/logomoci.png"
            Dim imageSideCenterIOS As String = _rootPath + "http://www.nataproperty.com/support/img/Icon-Apple.png"
            Dim imageSideCenterApps As String = _rootPath + "http://www.nataproperty.com/support/img/Icon-Android.png"
            Dim imageSideCenterWeb As String = _rootPath + "http://www.nataproperty.com/support/img/Icon-web.png"
            Dim imageSideCenterFb As String = _rootPath + "http://www.nataproperty.com/support/img/Icon-FB.png"
            Dim imageSideBottom As String = _rootPath + "http://www.nataproperty.com/support/img/Icon-Android.png"

            Try
                Dim msg As MailMessage = New MailMessage()
                msg.Subject = subject
                msg.From = New MailAddress(_emailWeb, _websiteProjectName)

                For Each emailTo As String In mailTo
                    If Trim(emailTo) <> "" Then
                        msg.To.Add(Trim(emailTo))
                    End If
                Next

                For Each emailCc As String In mailCc
                    If Trim(emailCc) <> "" Then
                        msg.CC.Add(emailCc)
                    End If
                Next

                For Each emailBcc As String In mailBcc
                    If Trim(emailBcc) <> "" Then
                        msg.Bcc.Add(emailBcc)
                    End If
                Next

                msg.IsBodyHtml = True
                msg.BodyEncoding = Encoding.UTF8
                Dim bodyHtml As String = ""

                Select Case emailType
                    Case EmailType.Subscribe
                        Dim introResult As New StringBuilder
                        Dim content As String = introResult.Append("<span style=""padding-top:10px;"">Terima kasih telah bergabung dengan " + _websiteProjectName + ". Kami akan memberikan update tentang " + _websiteProjectName + " kepada Anda.<br /><br /></span>").ToString
                        Dim subscribeResult As New StringBuilder
                        bodyHtml = File.ReadAllText(HttpContext.Current.Server.MapPath("support/template/templateEmail.html"))
                        subscribeResult.Append("<tr>")
                        subscribeResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Email</span> </th>")
                        subscribeResult.Append("</tr>")
                        subscribeResult.Append("<tr>")
                        subscribeResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("email") + "</td>")
                        subscribeResult.Append("</tr>")
                        subscribeResult.Append("<tr>")
                        subscribeResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Mobile Phone</span> </th>")
                        subscribeResult.Append("</tr>")
                        subscribeResult.Append("<tr>")
                        subscribeResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("hp").ToString + "</td>")
                        subscribeResult.Append("</tr>")
                        subscribeResult.Append("<tr>")
                        subscribeResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Name</span> </th>")
                        subscribeResult.Append("</tr>")
                        subscribeResult.Append("<tr>")
                        subscribeResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("name").ToString + "</td>")
                        subscribeResult.Append("</tr>")
                        bodyHtml = bodyHtml.Replace("@content", content)
                        bodyHtml = bodyHtml.Replace("@message", subscribeResult.ToString)
                    Case EmailType.ContactUs
                        Dim introResult As New StringBuilder
                        Dim content As String = introResult.Append("<span style=""padding-top:10px;"">Terima kasih telah menghubungi kami. Dibawah ini adalah pesan yang Anda kirimkan.<br /><br /></span>").ToString
                        Dim contactUsResult As New StringBuilder
                        bodyHtml = File.ReadAllText(HttpContext.Current.Server.MapPath("support/template/templateEmail.html"))
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Name</span> </th>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("name") + "</td>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Email</span> </th>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("email") + "</td>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Mobile Phone</span> </th>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("mobilePhone").ToString + "</td>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Message</span> </th>")
                        contactUsResult.Append("</tr>")
                        contactUsResult.Append("<tr>")
                        contactUsResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + replaceEnterToBR(data.Rows(0).Item("message")) + "</td>")
                        contactUsResult.Append("</tr>")
                        bodyHtml = bodyHtml.Replace("@content", content)
                        bodyHtml = bodyHtml.Replace("@message", contactUsResult.ToString)
                    Case EmailType.Inquiries
                        Dim introResult As New StringBuilder
                        Dim content As String = introResult.Append("<span style=""padding-top:10px;"">Terima kasih telah menghubungi kami. Dibawah ini adalah pesan yang Anda kirimkan.<br /><br /></span>").ToString
                        Dim inquiriesResult As New StringBuilder
                        bodyHtml = File.ReadAllText(HttpContext.Current.Server.MapPath("support/template/templateEmail.html"))
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Inquiries</span> </th>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("titleAds") + "</td>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Name</span> </th>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("name") + "</td>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Email</span> </th>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("email") + "</td>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Mobile Phone</span> </th>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("hp").ToString + "</td>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Type Inquiries</span> </th>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("inquiriesTypeName") + "</td>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Message</span> </th>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        inquiriesResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + replaceEnterToBR(data.Rows(0).Item("inquiriesText")) + "</td>")
                        inquiriesResult.Append("</tr>")
                        inquiriesResult.Append("<tr>")
                        bodyHtml = bodyHtml.Replace("@content", content)
                        bodyHtml = bodyHtml.Replace("@message", inquiriesResult.ToString)
                   Case EmailType.Career
                        Dim introResult As New StringBuilder
                        Dim content As String = introResult.Append("<span style=""padding-top:10px;"">Terima kasih telah menghubungi kami. Dibawah ini adalah Lamaran yang Anda kirimkan.<br /><br /></span>").ToString
                        Dim CareerResult As New StringBuilder
                        bodyHtml = File.ReadAllText(HttpContext.Current.Server.MapPath("support/template/templateEmail.html"))
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Position</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("Position") + "</td>")
                        CareerResult.Append("</tr>")

                        CareerResult.Append("<tr>")
                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>First Name</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("firstName") + "</td>")
                        CareerResult.Append("</tr>")

                        CareerResult.Append("<tr>")
                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Last Name</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("lastName") + "</td>")
                        CareerResult.Append("</tr>")

                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Email</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("email") + "</td>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Mobile Phone</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("mobilePhone").ToString + "</td>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Qualification & Expereince</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + replaceEnterToBR(data.Rows(0).Item("Experience")) + "</td>")
                        CareerResult.Append("</tr>")

                        CareerResult.Append("<tr>")
                        CareerResult.Append("<th style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;background-color: #f9f9f9;color: #f8931e;"" align=""left""><span>Resume</span> </th>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        CareerResult.Append("<td style=""width: 350px;padding-top: 5px;padding-bottom: 5px;padding-left: 5px;padding-bottom: 15px;"">" + data.Rows(0).Item("fileCv") + "</td>")
                        CareerResult.Append("</tr>")
                        CareerResult.Append("<tr>")
                        bodyHtml = bodyHtml.Replace("@content", content)
                        bodyHtml = bodyHtml.Replace("@message", CareerResult.ToString)
                End Select

                Dim rootPath As String = _rootPath
                bodyHtml = bodyHtml.Replace("@imageFooter", imageFooter)
                bodyHtml = bodyHtml.Replace("@imageSideTop", imageSideTop)
                bodyHtml = bodyHtml.Replace("@imageSideProjectBottom", imageSideProjectBottom)
                bodyHtml = bodyHtml.Replace("@imageSideCenterIOS", imageSideCenterIOS)
                bodyHtml = bodyHtml.Replace("@imageSideCenterApps", imageSideCenterApps)
                bodyHtml = bodyHtml.Replace("@imageSideCenterFb", imageSideCenterFb)
                bodyHtml = bodyHtml.Replace("@imageSideCenterWeb", imageSideCenterWeb)
                bodyHtml = bodyHtml.Replace("@imageSideBottom", imageSideBottom)
                bodyHtml = bodyHtml.Replace("@IOS", _linkIOS)
                bodyHtml = bodyHtml.Replace("@playStore", _linkPlayStore)
                bodyHtml = bodyHtml.Replace("@fb", _linkFb)
                bodyHtml = bodyHtml.Replace("@web", _rootPath)
                bodyHtml = bodyHtml.Replace("@sendDate", Format(Now.Date(), "dd/MM/yyyy").ToString())
                bodyHtml = bodyHtml.Replace("@emailSubject", subject)
                bodyHtml = bodyHtml.Replace("@emailWeb", _emailWeb)
                bodyHtml = bodyHtml.Replace("@websiteProjectName", _websiteProjectName)

                msg.HtmlBody = bodyHtml


                Dim client As New SmtpClient
                With client
                    .Host = _emailHost
                    .Port = CInt(_emailPort) 'port use by gmail to send mail
                    .Username = _emailUsername
                    .Password = _emailPassword
                    .SecurityOptions = SecurityOptions.SSLExplicit
                End With
                client.Send(msg)
            Catch ex As Exception
                result = ex.Message
            End Try

            Return result
        End Function

    End Class
End Namespace
