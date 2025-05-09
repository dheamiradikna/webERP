Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports System.IO
Imports Aspose.Pdf
Imports Aspose.Pdf.Devices
Imports Aspose.Pdf.Generator


Namespace [class]
    Public Class cls_wmContent

        Public Shared Function cekTagIsPublish(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim result As String = "0"
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select isContentPubDate from ms_tag where domainRef = @domainRef and tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("isContentPubDate")
            End If


            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        
        Public Shared Function saveContentAttachment(ByVal domainRef As String, ByVal contentRef As String, _
                                                     ByVal attachRef As String, _
                                                    ByVal attachFile As Byte(), ByVal attachFN As String, _
                                                    ByVal attachDesc As String, Optional ByVal tagRef As String = "0") As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text
            If Trim(attachRef) = "" Then
                'insert
                sqlCmd.CommandText = "insert into	tR_contentAttachment (domainRef, contentRef, attachRef, attachFN, attachFile, attachDesc) " + _
                                    " select    @domainRef, @contentRef, isnull(max(attachRef),0) + 1, @attachFN, @attachFile, @attachDesc " + _
                                    "from tR_contentAttachment " + _
                                    " where     domainRef = @domainRef and contentRef = @contentRef "
            Else
                'update
                sqlCmd.CommandText = "update	tR_contentAttachment " + _
                                    " set	    attachFN = @attachFN, attachFile = @attachFile, attachDesc = @attachDesc " + _
                                    " where     domainRef = @domainRef and contentRef = @contentRef " + _
                                    "           and attachRef = @attachRef "
                sqlCmd.Parameters.AddWithValue("@attachRef", attachRef)
            End If


            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@attachFN", attachFN)
            sqlCmd.Parameters.AddWithValue("@attachFile", attachFile)
            sqlCmd.Parameters.AddWithValue("@attachDesc", attachDesc)


            Try
                sqlCmd.ExecuteNonQuery()

                'If tagRef = _parentTagRefMenuBrochure Then


                '    Dim streamFile As New MemoryStream()
                '    streamFile.Write(attachFile, 0, attachFile.Length)

                '    File.WriteAllBytes(_ServerDecisionChange + "\data\presentation\data.pdf", attachFile)

                '    Dim license As New Aspose.Pdf.License()
                '    license.SetLicense(LicenseHelper.License.LStream)

                '    Dim pdfDocument As New Document(_ServerDecisionChange + "\data\presentation\data.pdf")

                '    If File.Exists(_ServerDecisionChange + "\data\presentation\data.pdf") Then
                '        File.Delete(_ServerDecisionChange + "\data\presentation\data.pdf")
                '    End If

                '    For pageCount As Integer = 1 To pdfDocument.Pages.Count
                '        If pageCount < 10 Then
                '            Using imageStream As New FileStream(_ServerDecisionChange + "\data\presentation\0" & pageCount & ".jpg", FileMode.Create)
                '                'Create Resolution object
                '                Dim resolution As New Resolution(300)
                '                'Create JPEG device with specified attributes (Width, Height, Resolution, Quality)
                '                'Quality [0-100], 100 is Maximum
                '                Dim jpegDevice As New JpegDevice(resolution, 75)

                '                'Convert a particular page and save the image to stream
                '                jpegDevice.Process(pdfDocument.Pages(pageCount), imageStream)

                '                'Close stream
                '                imageStream.Close()

                '            End Using
                '        Else
                '            Using imageStream As New FileStream(_ServerDecisionChange + "\data\presentation\" & pageCount & ".jpg", FileMode.Create)
                '                'Create Resolution object
                '                Dim resolution As New Resolution(300)
                '                'Create JPEG device with specified attributes (Width, Height, Resolution, Quality)
                '                'Quality [0-100], 100 is Maximum
                '                Dim jpegDevice As New JpegDevice(resolution, 75)

                '                'Convert a particular page and save the image to stream
                '                jpegDevice.Process(pdfDocument.Pages(pageCount), imageStream)

                '                'Close stream
                '                imageStream.Close()

                '            End Using
                '        End If
                '    Next pageCount
                'End If

            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getContentFirstAttachRef(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select 	top 1 i.attachRef " + _
                                "from tR_contentAttachment " + _
                                "where	domainRef = @domainRef and contentRef = @contentRef " + _
                                "order by inputtime asc"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("attachRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getContentAttachmentFile(ByVal domainRef As String, ByVal contentRef As String, ByVal attachRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	attachFN, attachFile, attachDesc " + _
                                "from tR_contentAttachment  " + _
                                "where	domainRef = @domainRef and contentRef = @contentRef and attachRef = @attachRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@attachRef", attachRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentAttachmentRef(ByVal domainRef As String, ByVal contentRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select 	attachRef, attachFN from tR_contentAttachment " + _
                                "where      domainRef = @domainRef And contentRef = @contentRef " + _
                                "order	    by inputtime"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentImage(ByVal domainRef As String, ByVal contentRef As String, _
                                               ByVal imgType As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select 	imgRef from tr_contentImage " + _
                                "where      domainRef = @domainRef And contentRef = @contentRef " + _
                                "           and imgType = @imgType " + _
                                "order	    by sortNo"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentTagRefStr(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""
            Dim strKeyword As New StringBuilder

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	ct.tagRef, t.tagName " + _
                                "from	    tr_contentTag ct, ms_tag t " + _
                                "where      ct.domainRef = t.domainRef And ct.tagRef = t.tagRef " + _
                                "           and ct.domainRef = @domainRef and ct.contentRef = @contentRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            While sqlDr.Read
                If Trim(sqlDr("tagRef")) <> "" Then
                    strKeyword.Append(Trim(sqlDr("tagRef")) + ",")
                End If
            End While
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            If strKeyword.ToString.Length > 0 Then
                result = Left(strKeyword.ToString, strKeyword.ToString.Length - 1)
            End If

            Return result
        End Function

        Public Shared Function getContentTagNameStr(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""
            Dim strKeyword As New StringBuilder

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	ct.tagRef, t.tagName " + _
                                "from	    tr_contentTag ct, ms_tag t " + _
                                "where      ct.domainRef = t.domainRef And ct.tagRef = t.tagRef " + _
                                "           and ct.domainRef = @domainRef and ct.contentRef = @contentRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            While sqlDr.Read
                If Trim(sqlDr("tagName")) <> "" Then
                    strKeyword.Append(Trim(sqlDr("tagName")) + ", ")
                End If
            End While
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            If strKeyword.ToString.Length > 0 Then
                result = Left(strKeyword.ToString, strKeyword.ToString.Length - 2)
            End If

            Return result
        End Function

        Public Shared Function cekIsThisTagContent(ByVal domainRef As String, _
                                                  ByVal contentRef As String, ByVal tagRef As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim result As Boolean = False
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select contentRef from tr_contentTag where domainRef = @domainRef and contentRef = @contentRef and tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = True
            End If

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getContentKeywordStr(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""
            Dim strKeyword As New StringBuilder

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	keywordRef, keywordText " + _
                                "from tr_contentKeyword " + _
                                "where	domainRef = @domainRef and contentRef = @contentRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            While sqlDr.Read
                If Trim(sqlDr("keywordText")) <> "" Then
                    strKeyword.Append(Trim(sqlDr("keywordText")) + _keywordSplitter + " ")
                End If
            End While
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            If strKeyword.ToString.Length > 0 Then
                result = Left(strKeyword.ToString, strKeyword.ToString.Length - 2)
            End If

            Return result
        End Function

        Public Shared Function getContentList(ByVal domainRef As String, ByVal contentType As String, _
                                                    ByVal imgSetting As String, ByVal tagRefList As String, _
                                                    ByVal contentDateFr As String, ByVal contentDateTo As String, _
                                                    ByVal isApproved As String, ByVal isPublish As String, ByVal isExpired As String, _
                                                    ByVal keyword As String, _
                                                    ByVal sortBy As String, ByVal sortType As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable


            ''''' is Approved '''''
            ''''' is Approved '''''
            ''''' is Approved '''''
            Dim queryIsApproved As String = ""
            If isApproved = "1" Then
                queryIsApproved = " and approvedDate is not null "
            ElseIf isApproved = "0" Then
                queryIsApproved = " and approvedDate is null "
            End If
            ''''' is Approved End '''''
            ''''' is Approved End '''''
            ''''' is Approved End '''''

            ''''' is Publish '''''
            ''''' is Publish '''''
            ''''' is Publish '''''
            Dim queryIsPublish As String = ""
            If isPublish = "1" Then
                queryIsPublish = " and publishDate is not null "
            ElseIf isPublish = "0" Then
                queryIsPublish = " and publishDate is null "
            End If
            ''''' is Publish End '''''
            ''''' is Publish End '''''
            ''''' is Publish End '''''

            ''''' is Expired '''''
            ''''' is Expired '''''
            ''''' is Expired '''''
            Dim queryIsExpired As String = ""
            If isExpired = "1" Then
                queryIsExpired = " and expiredDate is not null "
            ElseIf isExpired = "0" Then
                queryIsExpired = " and expiredDate is null "
            End If
            ''''' is Expired End '''''
            ''''' is Expired End '''''
            ''''' is Expired End '''''

            ''''' TAG '''''
            ''''' TAG '''''
            ''''' TAG '''''
            Dim queryTag As String = ""
            If Trim(tagRefList) <> "" Then
                queryTag = " and t.contentRef in (select contentRef from tr_contentTag where domainRef = @domainRef and tagRef in (" + tagRefList + ")) "
            End If
            ''''' TAG End '''''
            ''''' TAG End '''''
            ''''' TAG End '''''

            ''''' content start date '''''
            ''''' content start date '''''
            ''''' content start date '''''
            Dim queryContentStartDate As String = ""
            Dim queryContentEndDate As String = ""

            If Trim(contentDateFr) <> "" Then
                queryContentStartDate = " and datediff(d,'" + contentDateFr + "',contentDate) >= 0 "
            End If

            If Trim(contentDateTo) <> "" Then
                queryContentEndDate = " and datediff(d,'" + contentDateTo + "',contentDate) <= 0 "
            End If
            ''''' content start date end '''''
            ''''' content start date end '''''
            ''''' content start date end '''''

            ''''' order stuff '''''
            ''''' order stuff '''''
            ''''' order stuff '''''
            Dim strOrder As String = ""
            If Trim(sortBy) <> "-" Then
                strOrder = " order	by " + sortBy + " "
                If Trim(sortType) <> "-" Then
                    strOrder = strOrder + sortType + " "
                End If
            End If
            ''''' order stuff end '''''
            ''''' order stuff end '''''
            ''''' order stuff end '''''

            ''''' any word method ''''
            ''''' any word method ''''
            ''''' any word method ''''
            Dim field() As String = {"t.title", "t.titleDetail", "t.synopsis", "t.content", "ck.keywordText", "t.metadescription"}
            Dim whereSearch As New StringBuilder
            Dim whereSearchD As New StringBuilder
            Dim i, f As Integer

            keyword = Replace(keyword, "'", "")
            keyword = Replace(keyword, """", "")

            If Trim(keyword) <> "" Then
                Dim temp() As String = keyword.Split(" ")

                whereSearch.Append(" and ( ")
                For i = 0 To temp.Length - 1
                    whereSearch.Append(" ( ")
                    For f = 0 To field.Length - 1
                        whereSearch.Append(" " + field(f) + " like '%" + temp(i) + "%' ")
                        If f < field.Length - 1 Then
                            whereSearch.Append(" or ")
                        End If
                    Next
                    whereSearch.Append(" ) ")

                    If i = temp.Length - 1 Then
                        whereSearch.Append(" ) ")
                    Else
                        whereSearch.Append(" or ")
                    End If
                Next
            End If
            ''''' any word method end ''''
            ''''' any word method end ''''
            ''''' any word method end ''''


            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	t.domainRef, t.contentRef, t.contentType, ct.contentTypeName " + _
                                "           , t.imgSetting, cis.imgSettingName, t.title, t.titleDetail, t.synopsis " + _
                                "           , t.content, t.contentDate, t.publishDate, t.expiredDate, t.approvedDate " + _
                                "           , t.hit, t.inputTime, t.inputUN " + _
                                "from	    tr_content t " + _
                                "left join  lk_contentType ct on ct.contentType = t.contentType " + _
                                "left join  lk_contentImgSetting cis on cis.imgSetting = t.imgSetting " + _
                                "where	    t.domainRef = @domainRef " + _
                                "and        (t.contentType = @contentType or 'All' = @contentType) " + _
                                queryTag + _
                                queryContentStartDate + queryContentEndDate + _
                                queryIsApproved + queryIsExpired + queryIsPublish + _
                                whereSearch.ToString + _
                                whereSearchD.ToString + _
                                strOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentType", contentType)
            sqlCmd.Parameters.AddWithValue("@imgSetting", imgSetting)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfo(ByVal domainRef As String, ByVal contentRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	contentRef, contentType, imgSetting, imgPosition, geolocation.Lat as Latitude, geolocation.Long as Longitude " + _
                                "           , title, titleDetail, synopsis, content " + _
                                "           , contentDate, publishDate, expiredDate, approvedDate, embedVideo, metaTitle, metaAuthor, metaDescription " + _
                                "from       tr_content " + _
                                "where	    domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        'Public Shared Function getContentRelatedList(ByVal domainRef As String, ByVal contentRef As String) As DataTable
        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "select cr.contentRefRelated as contentRef, c.title " + _
        '                        "from	tr_content c " + _
        '                        "inner join tr_contentRelated cr on c.contentRef = cr.contentRefRelated " + _
        '                        "where domainRef = @domainRef and cr.contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        Public Shared Function getContentInfoHTML(ByVal domainRef As String, ByVal contentRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.Columns.Add("domainRef")
            dt.Columns.Add("contentRef")
            dt.Columns.Add("contentType")
            dt.Columns.Add("contentTypeName")
            dt.Columns.Add("imgSetting")
            dt.Columns.Add("imgSettingName")
            dt.Columns.Add("title")
            dt.Columns.Add("contentDate")
            dt.Columns.Add("publishDate")
            dt.Columns.Add("approvedDate")
            dt.Columns.Add("expiredDate")
            dt.Columns.Add("TAG")
            dt.Columns.Add("hit")

            sqlCmd.CommandText = "select	t.domainRef, t.contentRef, t.contentType, ct.contentTypeName " + _
                                "           , t.imgSetting, cis.imgSettingName, t.embedVideo, t.title, t.titleDetail, t.synopsis " + _
                                "           , t.content, t.contentDate, t.publishDate, t.expiredDate, t.approvedDate " + _
                                "           , t.hit, t.inputTime, t.inputUN " + _
                                "from	    tr_content t, lk_contentType ct, lk_contentImgSetting cis " + _
                                "where	    t.contentType = ct.contentType and t.imgSetting = cis.imgSetting " + _
                                 "          and domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                dr = dt.NewRow

                Dim strContentDate As String = ""
                Dim strPubDate As String = ""
                Dim strApprvDate As String = ""
                Dim strExpDate As String = ""

                If Not IsDBNull(sqlDr("contentDate")) Then
                    strContentDate = Format(sqlDr("contentDate"), "dd/MM/yyyy")
                End If

                If Not IsDBNull(sqlDr("publishDate")) Then
                    strPubDate = Format(sqlDr("publishDate"), "dd/MM/yyyy")
                End If

                If Not IsDBNull(sqlDr("approvedDate")) Then
                    strApprvDate = Format(sqlDr("approvedDate"), "dd/MM/yyyy")
                End If

                If Not IsDBNull(sqlDr("expiredDate")) Then
                    strExpDate = Format(sqlDr("expiredDate"), "dd/MM/yyyy")
                End If

                dr("domainRef") = sqlDr("domainRef")
                dr("contentRef") = sqlDr("contentRef")
                dr("contentType") = sqlDr("contentType")
                dr("contentTypeName") = sqlDr("contentTypeName")
                dr("imgSetting") = sqlDr("imgSetting")
                dr("imgSettingName") = sqlDr("imgSettingName")
                dr("title") = sqlDr("title")
                dr("contentDate") = strContentDate
                dr("publishDate") = strPubDate
                dr("approvedDate") = strApprvDate
                dr("expiredDate") = strExpDate
                dr("TAG") = getContentTagNameStr(sqlDr("domainRef").ToString, sqlDr("contentRef").ToString)
                dr("hit") = sqlDr("hit")


                dt.Rows.Add(dr)
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function


        Public Shared Function deleteContent(ByVal domainRef As String, ByVal contentRef As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	tr_content " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from tr_contentAttachment " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from tr_contentImage " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from tr_contentKeyword " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from tr_contentTag " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                'sqlCmd.Parameters.Clear()
                'sqlCmd.Prepare()
                'sqlCmd.CommandType = CommandType.Text
                'sqlCmd.CommandText = "delete from tr_contentRelated " + _
                '                 "where contentRef = @contentRef"
                'sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                'sqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function deleteImage(ByVal domainRef As String, ByVal imageRef As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	tr_contentImage " + _
                                 "where domainRef = @domainRef and imgRef = @imgRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@imgRef", imageRef)
                sqlCmd.ExecuteNonQuery()


                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from img_tr_image " + _
                                 "where domainRef = @domainRef and imgRef = @imgRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@imgRef", imageRef)
                sqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function removeImage(ByVal domainRef As String, ByVal contentRef As String, _
                                           ByVal imageRef As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	tr_contentImage " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef and imgRef = @imgRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.Parameters.AddWithValue("@imgRef", imageRef)
                sqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function deleteAttachment(ByVal domainRef As String, ByVal contentRef As String, _
                                                ByVal attachRef As String, Optional ByVal tagRef As String = "0") As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	tR_contentAttachment " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef and attachRef = @attachRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.Parameters.AddWithValue("@attachRef", attachRef)
                sqlCmd.ExecuteNonQuery()

                'If tagRef = _parentTagRefMenuBrochure Then
                '    DeleteDirectory(_ServerDecisionChange + "\data\presentation")
                'End If


                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Sub DeleteDirectory(ByVal path As String)
            If Directory.Exists(path) Then
                'Delete all files from the Directory
                For Each filepath As String In Directory.GetFiles(path)
                    File.Delete(filepath)
                Next
                'Delete all child Directories
                For Each dir As String In Directory.GetDirectories(path)
                    DeleteDirectory(dir)
                Next

            End If
        End Sub



        'Public Shared Function insertContent(ByVal domainRef As String, _
        '                                      ByVal contentType As String, ByVal imgSetting As String, _
        '                                      ByVal imgPosition As String, ByVal embedVideo As String, _
        '                                      ByVal title As String, ByVal titleDetail As String, _
        '                                      ByVal MapLatitude As String, ByVal MapLongitude As String, ByVal synopsis As String, ByVal content As String, _
        '                                      ByVal contentDate As string, _
        '                                      ByVal publishDate As System.Data.SqlTypes.SqlDateTime, _
        '                                      ByVal expiredDate As System.Data.SqlTypes.SqlDateTime, _
        '                                      ByVal approvedDate As System.Data.SqlTypes.SqlDateTime, _
        '                                      ByVal metaDescription As String, ByVal inputUN As String, _
        '                                      ByVal metaTitle As String, _
        '                                      ByVal metaAuthor As String, ByVal PICTUREFileName As string, ByVal PICTUREFile As Byte(), ByVal PICTURELength As Integer, _
        '                                      ByVal description As String, _
        '                                      ByVal keyword As String, ByVal imgW As Integer, _
        '                                      ByVal imgH As Integer,ByVal imgRef As String) As String

        '    Dim sqlCmd As New SqlCommand
        '    Dim result As String = ""
        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlTrans As SqlTransaction
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlTrans = sqlCon.BeginTransaction

        '    sqlCmd.Transaction = sqlTrans
        '    sqlCmd.Connection = sqlCon

        '    Try
        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "select	isnull(max(contentRef),0) + 1 as ref " + _
        '                            "from tr_content " + _
        '                            "where domainRef = @domainRef "

        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

        '        sqlDr = sqlCmd.ExecuteReader
        '        If sqlDr.Read Then
        '            result = sqlDr("ref")
        '        End If
        '        sqlDr.Close()

        '        Dim queryGeo As String
        '        If mapLatitude <> "" And mapLongitude <> "" Then
        '            queryGeo = " geography::Point(@mapLatitude, @mapLongitude, 4326) "
        '        Else
        '            queryGeo = " geography::[Null] "
        '        End If

        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "insert	into tr_content " + _
        '                            "           (domainRef, contentRef, contentType, imgSetting, imgPosition, embedVideo,geoLocation, title, titleDetail,    synopsis " + _
        '                            "           , content, contentDate, publishDate, expiredDate, approvedDate, metaDescription " + _
        '                            "           , inputUN, metaTitle, metaAuthor ) " + _
        '                            "values	    (@domainRef, @contentRef, @contentType, @imgSetting, @imgPosition, @embedVideo," + queryGeo + " , @title, @titleDetail, @synopsis " + _
        '                            "           , @content, @contentDate, @publishDate, @expiredDate, @approvedDate, @metaDescription " + _
        '                            "           , @inputUN, @metaTitle, @metaAuthor )"

        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '        sqlCmd.Parameters.AddWithValue("@contentRef", result)
        '        sqlCmd.Parameters.AddWithValue("@contentType", contentType)
        '        sqlCmd.Parameters.AddWithValue("@imgSetting", imgSetting)
        '        sqlCmd.Parameters.AddWithValue("@imgPosition", imgPosition)
        '        sqlCmd.Parameters.AddWithValue("@embedVideo", embedVideo)
        '        sqlCmd.Parameters.AddWithValue("@title", title)
        '        sqlCmd.Parameters.AddWithValue("@titleDetail", titleDetail)
        '        sqlCmd.Parameters.AddWithValue("@synopsis", synopsis)
        '        sqlCmd.Parameters.AddWithValue("@content", content)
        '        sqlCmd.Parameters.AddWithValue("@contentDate", contentDate)
        '        sqlCmd.Parameters.AddWithValue("@publishDate", publishDate)
        '        sqlCmd.Parameters.AddWithValue("@expiredDate", expiredDate)
        '        sqlCmd.Parameters.AddWithValue("@approvedDate", approvedDate)
        '        sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)
        '        sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)
        '        sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
        '        sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)
        '        If MapLatitude <> "" And MapLongitude <> "" Then
        '        sqlCmd.Parameters.AddWithValue("@mapLatitude", MapLatitude)
        '        sqlCmd.Parameters.AddWithValue("@mapLongitude", mapLongitude)
        '        End If


        '        If PICTURELength > 0 Then
        '                'If SIUPDate <> "" Then
        '                '    SIUPDate = DateTime.ParseExact(SIUPDate, "dd/MM/yyyy", cultureinfo)
        '                'End If


        '        sqlCmd.Parameters.Clear()
        '                sqlCmd.CommandType = CommandType.Text
        '                sqlCmd.CommandText = "select	isnull(max(imgRef),0) + 1 as ref " + _
        '                                        "from IMG_TR_image " + _
        '                                        "where domainRef = @domainRef "
        '        sqlCmd.Connection = sqlCon

        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '         sqlCmd.ExecuteNonQuery()       

        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '                sqlCmd.CommandType = CommandType.Text
        '                sqlCmd.CommandText = "insert	into IMG_TR_image " + _
        '                            "           (domainRef, imgRef, title, description, keyword, imgW, imgH, imgFile, imgFileName, inputUN) " + _
        '                            "values	    (@domainRef, @imgRef, @title, @description, @keyword, @imgW, @imgH, @imgFile, @imgFileName, @inputUN)"

        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '        sqlCmd.Parameters.AddWithValue("@imgRef", result)
        '        sqlCmd.Parameters.AddWithValue("@title", title)
        '            sqlCmd.Parameters.AddWithValue("@description", description)
        '            sqlCmd.Parameters.AddWithValue("@keyword", keyword)
        '            sqlCmd.Parameters.AddWithValue("@imgW", imgW)
        '            sqlCmd.Parameters.AddWithValue("@imgH", imgH)
        '            sqlCmd.Parameters.AddWithValue("@imgFile", PICTUREFile)
        '        sqlCmd.Parameters.AddWithValue("@imgFileName", PICTUREFileName)
        '        sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)

        '                sqlCmd.ExecuteNonQuery()
        '                'section SIUP  
        '            End If


        '        sqlCmd.ExecuteNonQuery()

        '        sqlTrans.Commit()
        '    Catch ex As Exception
        '        sqlTrans.Rollback()
        '        result = ex.Message
        '    End Try

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function


        Public Shared Function insertContent(ByVal domainRef As String, _
                                              ByVal contentType As String, ByVal imgSetting As String, _
                                              ByVal imgPosition As String, ByVal embedVideo As String, _
                                              ByVal title As String, ByVal titleDetail As String, _
                                              ByVal MapLatitude As String, ByVal MapLongitude As String, ByVal synopsis As String, ByVal content As String, _
                                              ByVal contentDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal publishDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal expiredDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal approvedDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal metaDescription As String, ByVal inputUN As String, _
                                              ByVal metaTitle As String, _
                                              ByVal metaAuthor As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "select	isnull(max(contentRef),0) + 1 as ref " + _
                                    "from tr_content " + _
                                    "where domainRef = @domainRef "

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

                sqlDr = sqlCmd.ExecuteReader
                If sqlDr.Read Then
                    result = sqlDr("ref")
                End If
                sqlDr.Close()

                Dim queryGeo As String
                If MapLatitude <> "" And MapLongitude <> "" Then
                    queryGeo = " geography::Point(@mapLatitude, @mapLongitude, 4326) "
                Else
                    queryGeo = " geography::[Null] "
                End If

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "insert	into tr_content " + _
                                    "           (domainRef, contentRef, contentType, imgSetting, imgPosition, embedVideo,geoLocation, title, titleDetail,    synopsis " + _
                                    "           , content, contentDate, publishDate, expiredDate, approvedDate, metaDescription " + _
                                    "           , inputUN, metaTitle, metaAuthor ) " + _
                                    "values	    (@domainRef, @contentRef, @contentType, @imgSetting, @imgPosition, @embedVideo," + queryGeo + " , @title, @titleDetail, @synopsis " + _
                                    "           , @content, @contentDate, @publishDate, @expiredDate, @approvedDate, @metaDescription " + _
                                    "           , @inputUN, @metaTitle, @metaAuthor )"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", result)
                sqlCmd.Parameters.AddWithValue("@contentType", contentType)
                sqlCmd.Parameters.AddWithValue("@imgSetting", imgSetting)
                sqlCmd.Parameters.AddWithValue("@imgPosition", imgPosition)
                sqlCmd.Parameters.AddWithValue("@embedVideo", embedVideo)
                sqlCmd.Parameters.AddWithValue("@title", title)
                sqlCmd.Parameters.AddWithValue("@titleDetail", titleDetail)
                sqlCmd.Parameters.AddWithValue("@synopsis", synopsis)
                sqlCmd.Parameters.AddWithValue("@content", content)
                sqlCmd.Parameters.AddWithValue("@contentDate", contentDate)
                sqlCmd.Parameters.AddWithValue("@publishDate", publishDate)
                sqlCmd.Parameters.AddWithValue("@expiredDate", expiredDate)
                sqlCmd.Parameters.AddWithValue("@approvedDate", approvedDate)
                sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)
                sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)
                sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
                sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)
                If MapLatitude <> "" And MapLongitude <> "" Then
                    sqlCmd.Parameters.AddWithValue("@mapLatitude", MapLatitude)
                    sqlCmd.Parameters.AddWithValue("@mapLongitude", MapLongitude)
                End If

                sqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function




             Public Shared Function updateContent(ByVal domainRef As String, ByVal contentRef As String, _
                                              ByVal contentType As String, ByVal imgSetting As String, _
                                              ByVal imgPosition As String, ByVal embedVideo As String, _
                                              ByVal title As String, ByVal titleDetail As String, _
                                              ByVal MapLatitude As String, ByVal MapLongitude As String, _
                                              ByVal synopsis As String, ByVal content As String, _
                                              ByVal contentDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal publishDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal expiredDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal approvedDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal metaDescription As String, _
                                              ByVal metaTitle As String, _
                                              ByVal metaAuthor As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = contentRef
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon


            Try

                Dim queryGeo As String
                If MapLatitude <> "" And MapLongitude <> "" Then
                    queryGeo = " geography::Point(@mapLatitude, @mapLongitude, 4326) "
                Else
                    queryGeo = " geography::[Null] "
                End If




                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "update	tr_content " + _
                                "set	    contentType = @contentType, imgSetting = @imgSetting, imgPosition = @imgPosition " + _
                                "           , embedVideo = @embedVideo, title = @title, titleDetail = @titleDetail, synopsis = @synopsis " + _
                                "           , content = @content, contentDate = @contentDate, publishDate = @publishDate " + _
                                "           , [GeoLocation] = " + queryGeo + " " + _
                                "           , expiredDate = @expiredDate, approvedDate = @approvedDate, metaDescription = @metaDescription " + _
                                "           , metaTitle = @metaTitle, metaAuthor = @metaAuthor " + _
                                "where	    domainRef = @domainRef and contentRef = @contentRef "


                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.Parameters.AddWithValue("@contentType", contentType)
                sqlCmd.Parameters.AddWithValue("@imgSetting", imgSetting)
                sqlCmd.Parameters.AddWithValue("@imgPosition", imgPosition)
                sqlCmd.Parameters.AddWithValue("@embedVideo", embedVideo)
                sqlCmd.Parameters.AddWithValue("@title", title)
                sqlCmd.Parameters.AddWithValue("@titleDetail", titleDetail)
                sqlCmd.Parameters.AddWithValue("@synopsis", synopsis)
                sqlCmd.Parameters.AddWithValue("@content", content)
                sqlCmd.Parameters.AddWithValue("@contentDate", contentDate)
                sqlCmd.Parameters.AddWithValue("@publishDate", publishDate)
                sqlCmd.Parameters.AddWithValue("@expiredDate", expiredDate)
                sqlCmd.Parameters.AddWithValue("@approvedDate", approvedDate)
                sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)
                sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
                sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)
                If MapLatitude <> "" And MapLongitude <> "" Then
                    sqlCmd.Parameters.AddWithValue("@mapLatitude", MapLatitude)
                    sqlCmd.Parameters.AddWithValue("@mapLongitude", MapLongitude)
                End If

                sqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertContentKeyword(ByVal domainRef As String, ByVal contentRef As String, _
                                                 ByVal keyword As String, ByVal keywordSplitter As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from tr_contentKeyword " + _
                                    "where	domainRef = @domainRef and contentRef = @contentRef"

                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

                sqlCmd.ExecuteNonQuery()

                Dim query As New StringBuilder
                For i = 0 To keyword.Split(keywordSplitter).Count - 1
                    If Trim(keyword.Split(keywordSplitter)(i)) <> "" Then
                        query.Append("insert	into tr_contentKeyword (domainRef, contentRef, keywordRef, keywordText) values (@domainRef, @contentRef, " + (i + 1).ToString + ", '" + Trim(keyword.Split(keywordSplitter)(i)) + "') ")
                    End If
                Next

                If Trim(query.ToString) <> "" Then
                    sqlCmd.Parameters.Clear()
                    sqlCmd.Prepare()

                    sqlCmd.CommandType = CommandType.Text
                    sqlCmd.CommandText = query.ToString

                    sqlCmd.Connection = sqlCon

                    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

                    sqlCmd.ExecuteNonQuery()
                End If



                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertContentTag(ByVal domainRef As String, ByVal contentRef As String, _
                                                 ByVal tagRefList As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete from tr_contentTag " + _
                                    "where	domainRef = @domainRef and contentRef = @contentRef"

                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

                sqlCmd.ExecuteNonQuery()

                Dim query As New StringBuilder
                For i = 0 To tagRefList.Split(",").Count - 1
                    If Trim(tagRefList.Split(",")(i)) <> "" Then
                        query.Append("insert	into tr_contentTag (domainRef, contentRef, tagRef) values (@domainRef, @contentRef, " + Trim(tagRefList.Split(",")(i)) + ") ")
                    End If
                Next

                If Trim(query.ToString) <> "" Then
                    sqlCmd.Parameters.Clear()
                    sqlCmd.Prepare()

                    sqlCmd.CommandType = CommandType.Text
                    sqlCmd.CommandText = query.ToString

                    sqlCmd.Connection = sqlCon

                    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

                    sqlCmd.ExecuteNonQuery()
                End If



                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getTagRefByContentRef(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select tagRef " + _
                                 "from TR_contentTag " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("tagRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()



            Return result
        End Function

        Public Shared Function getImage (ByVal domainRef As String, byval imgRef As string) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = " select * " + _
            " from IMG_TR_image " + _
            " where domainRef = @domainRef and imgRef = @imgRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)
            sqlCmd = Nothing
            sqlCon.Close()
            Return dt
        End Function


    End Class


End Namespace