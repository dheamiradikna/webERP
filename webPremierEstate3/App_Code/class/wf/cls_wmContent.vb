Imports System.Data
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports [class].clsWebGeneral
Imports [class].clsGeneral


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
            sqlDr.Close()

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function saveContentAttachment(ByVal domainRef As String, ByVal contentRef As String, _
                                                     ByVal attachRef As String, _
                                                    ByVal attachFile As Byte(), ByVal attachFN As String, _
                                                    ByVal attachDesc As String) As String
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
                'result = ""
            Catch ex As Exception
                result = ex.Message
            End Try


            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
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
            sqlCmd.CommandText = "select 	top 1 attachRef " + _
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
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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

            sqlDr.Close()
            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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
                                                    ByVal keyword As String, ByVal metaDescription As String, _
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

            metaDescription = Replace(metaDescription, "'", "")
            metaDescription = Replace(metaDescription, """", "")

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

            If Trim(metaDescription) <> "" Then
                Dim temp() As String = metaDescription.Split(" ")

                whereSearch.Append(" and ( ")
                For i = 0 To temp.Length - 1
                    whereSearchD.Append(" ( ")
                    For f = 0 To field.Length - 1
                        whereSearchD.Append(" " + field(f) + " like '%" + temp(i) + "%' ")
                        If f < field.Length - 1 Then
                            whereSearchD.Append(" or ")
                        End If
                    Next
                    whereSearchD.Append(" ) ")

                    If i = temp.Length - 1 Then
                        whereSearchD.Append(" ) ")
                    Else
                        whereSearchD.Append(" or ")
                    End If
                Next
            End If
            ''''' any word method end ''''


            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            'sqlCmd.CommandText = "select	domainRef, contentRef, contentType, imgSetting, title, titleDetail, synopsis " + _
            '                    ", content, contentDate, publishDate, expiredDate, approvedDate " + _
            '                    ", hit, inputTime, inputUN " + _
            '                    "from	tr_content " + _
            sqlCmd.CommandText = "select	t.domainRef, t.contentRef, t.contentType, ct.contentTypeName, ck.keywordText " + _
                                "           , t.imgSetting, cis.imgSettingName, t.title, t.titleDetail, t.synopsis " + _
                                "           , t.content, t.contentDate, t.publishDate, t.expiredDate, t.approvedDate, t.metadescription " + _
                                "           , t.hit, t.inputTime, t.inputUN " + _
                                "from	    tr_content t " + _
                                "left join  tr_contentKeyword ck on ck.contentRef = t.contentRef " + _
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
            'SqlConnection.ClearPool(sqlCon)
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

            sqlCmd.CommandText = "select	domainRef, contentRef, contentType, imgSetting, imgPosition, title, titleDetail, synopsis " + _
                                ", content, contentDate, publishDate, expiredDate, approvedDate,metaDescription " + _
                                ", hit, inputTime, inputUN " + _
                                "from	tr_content " + _
                                "where      domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return dt
        End Function

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
                                "           , t.imgSetting, cis.imgSettingName, t.title, t.titleDetail, t.synopsis " + _
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
            'SqlConnection.ClearPool(sqlCon)
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

                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
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
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function deleteAttachment(ByVal domainRef As String, ByVal contentRef As String, _
                                                ByVal attachRef As String) As String

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


                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertContent(ByVal domainRef As String, _
                                              ByVal contentType As String, ByVal imgSetting As String, _
                                              ByVal imgPosition As String, _
                                              ByVal title As String, ByVal titleDetail As String, _
                                              ByVal synopsis As String, ByVal content As String, _
                                              ByVal contentDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal publishDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal expiredDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal approvedDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal metaDescription As String, ByVal inputUN As String) As String

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


                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "insert	into tr_content " + _
                                    "           (domainRef, contentRef, contentType, imgSetting, imgPosition, title, titleDetail, synopsis " + _
                                    "           , content, contentDate, publishDate, expiredDate, approvedDate,metaDescription " + _
                                    "           , inputUN) " + _
                                    "values	    (@domainRef, @contentRef, @contentType, @imgSetting, @imgPosition, @title, @titleDetail, @synopsis " + _
                                    "           , @content, @contentDate, @publishDate, @expiredDate, @approvedDate,@metaDescription " + _
                                    "           , @inputUN)"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", result)
                sqlCmd.Parameters.AddWithValue("@contentType", contentType)
                sqlCmd.Parameters.AddWithValue("@imgSetting", imgSetting)
                sqlCmd.Parameters.AddWithValue("@imgPosition", imgPosition)
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

                sqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function updateContent(ByVal domainRef As String, ByVal contentRef As String, _
                                              ByVal contentType As String, ByVal imgSetting As String, _
                                              ByVal imgPosition As String, _
                                              ByVal title As String, ByVal titleDetail As String, _
                                              ByVal synopsis As String, ByVal content As String, _
                                              ByVal contentDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal publishDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal expiredDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal approvedDate As System.Data.SqlTypes.SqlDateTime, _
                                              ByVal metaDescription As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = contentRef
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "update	tr_content " + _
                                "set	    contentType = @contentType, imgSetting = @imgSetting, imgPosition = @imgPosition " + _
                                "           , title = @title, titleDetail = @titleDetail, synopsis = @synopsis " + _
                                "           , content = @content, contentDate = @contentDate, publishDate = @publishDate " + _
                                "           , expiredDate = @expiredDate, approvedDate = @approvedDate,metaDescription = @metaDescription " + _
                                "where	    domainRef = @domainRef and contentRef = @contentRef "


                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.Parameters.AddWithValue("@contentType", contentType)
                sqlCmd.Parameters.AddWithValue("@imgSetting", imgSetting)
                sqlCmd.Parameters.AddWithValue("@imgPosition", imgPosition)
                sqlCmd.Parameters.AddWithValue("@title", title)
                sqlCmd.Parameters.AddWithValue("@titleDetail", titleDetail)
                sqlCmd.Parameters.AddWithValue("@synopsis", synopsis)
                sqlCmd.Parameters.AddWithValue("@content", content)
                sqlCmd.Parameters.AddWithValue("@contentDate", contentDate)
                sqlCmd.Parameters.AddWithValue("@publishDate", publishDate)
                sqlCmd.Parameters.AddWithValue("@expiredDate", expiredDate)
                sqlCmd.Parameters.AddWithValue("@approvedDate", approvedDate)
                sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)

                sqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return result
        End Function

        'Public Shared Function insertContentKeyword(ByVal domainRef As String, ByVal contentRef As String, _
        '                                         ByVal keyword As String, ByVal keywordSplitter As String) As String
        '    Dim sqlCmd As New SqlCommand
        '    Dim result As String = ""
        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlTrans As SqlTransaction

        '    sqlCon.Open()
        '    sqlTrans = sqlCon.BeginTransaction

        '    sqlCmd.Transaction = sqlTrans
        '    sqlCmd.Connection = sqlCon

        '    Try
        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "delete from tr_contentKeyword " + _
        '                            "where	domainRef = @domainRef and contentRef = @contentRef"

        '        sqlCmd.Connection = sqlCon

        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '        sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '        sqlCmd.ExecuteNonQuery()

        '        Dim query As New StringBuilder
        '        For i = 0 To keyword.Split(keywordSplitter).Count - 1
        '            If Trim(keyword.Split(keywordSplitter)(i)) <> "" Then
        '                query.Append("insert	into tr_contentKeyword (domainRef, contentRef, keywordRef, keywordText) values (@domainRef, @contentRef, " + (i + 1).ToString + ", '" + Trim(keyword.Split(keywordSplitter)(i)) + "') ")
        '            End If
        '        Next

        '        If Trim(query.ToString) <> "" Then
        '            sqlCmd.Parameters.Clear()
        '            sqlCmd.Prepare()

        '            sqlCmd.CommandType = CommandType.Text
        '            sqlCmd.CommandText = query.ToString

        '            sqlCmd.Connection = sqlCon

        '            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '            sqlCmd.ExecuteNonQuery()
        '        End If



        '        sqlTrans.Commit()
        '    Catch ex As Exception
        '        sqlTrans.Rollback()
        '        result = ex.Message
        '    End Try

        '    sqlCmd = Nothing
        '    'SqlConnection.ClearPool(sqlCon)
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function insertContentTag(ByVal domainRef As String, ByVal contentRef As String, _
        '                                         ByVal tagRefList As String) As String
        '    Dim sqlCmd As New SqlCommand
        '    Dim result As String = ""
        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlTrans As SqlTransaction

        '    sqlCon.Open()
        '    sqlTrans = sqlCon.BeginTransaction

        '    sqlCmd.Transaction = sqlTrans
        '    sqlCmd.Connection = sqlCon

        '    Try
        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "delete from tr_contentTag " + _
        '                            "where	domainRef = @domainRef and contentRef = @contentRef"

        '        sqlCmd.Connection = sqlCon

        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '        sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '        sqlCmd.ExecuteNonQuery()

        '        Dim query As New StringBuilder
        '        For i = 0 To tagRefList.Split(",").Count - 1
        '            If Trim(tagRefList.Split(",")(i)) <> "" Then
        '                query.Append("insert	into tr_contentTag (domainRef, contentRef, tagRef) values (@domainRef, @contentRef, " + Trim(tagRefList.Split(",")(i)) + ") ")
        '            End If
        '        Next

        '        If Trim(query.ToString) <> "" Then
        '            sqlCmd.Parameters.Clear()
        '            sqlCmd.Prepare()

        '            sqlCmd.CommandType = CommandType.Text
        '            sqlCmd.CommandText = query.ToString

        '            sqlCmd.Connection = sqlCon

        '            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '            sqlCmd.ExecuteNonQuery()
        '        End If



        '        sqlTrans.Commit()
        '    Catch ex As Exception
        '        sqlTrans.Rollback()
        '        result = ex.Message
        '    End Try

        '    sqlCmd = Nothing
        '    'SqlConnection.ClearPool(sqlCon)
        '    sqlCon.Close()

        '    Return result
        'End Function

    End Class

End Namespace