Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports [class].clsWebGeneral
Imports [class].clsGeneralSetting
Imports [class].clsContentDB
Imports [class].clsGeneral

Namespace [class]
    Public Class clsIntContentDb

        Public Shared Function getSubmenu(ByVal tagRefParent As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select   tagname " + _
                                 "From " + _
                                 "      MS_tag " + _
                                 "Where isActive =1 and tagRefParent = @tagRefParent "
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()
            Return dt
        End Function

        Public Shared Function getContentChangeImage(ByVal domainRef As String, ByVal contentRef As String, ByVal imgType As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = "0"

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            Dim dt As New DataTable
            sqlCmd.CommandText = "select	top 2 imgRef from tr_contentImage where domainRef = @domainRef and contentRef = @contentRef and imgType = @imgType "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()
            Return dt
        End Function


        Public Shared Function getContentMessage(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean, Optional ByVal top As String = "", Optional ByVal alphabeticSort As String = "") As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim queryTop As String = ""
            Dim queryAlphabeticSort As String = ""
            Dim queryPublishDate As String = ""
            Dim queryExpiredDate As String = ""


            If Trim(top) <> "" Then
                queryTop = " TOP " + top + " "
            End If

            If Trim(alphabeticSort) <> "All" And Trim(alphabeticSort) <> "" Then
                If Trim(alphabeticSort) = "NA" Then
                    queryAlphabeticSort = " and ct.title NOT  LIKE '[A-Z]%' "
                Else
                    queryAlphabeticSort = " and ct.title Like '" + alphabeticSort.ToString() + "%' "
                End If
            End If

            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            
            sqlCmd.CommandText = "Select  " + queryTop + "ct.contentRef,ct.synopsis,ct.title,ct.titleDetail, ct.content " + _
                                 "From " + _
                                 "      TR_content ct " + _
                                 "      INNER JOIN TR_contentTag cg on cg.contentRef = ct.contentRef and cg.domainRef = ct.domainRef  " + _
                                 "Where cg.tagRef  = @tagRef and cg.domainRef = @domainRef " + _
                                 queryPublishDate + queryExpiredDate + queryAlphabeticSort.ToString + _
                                 "order by  ct.contentRef asc "

            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt

        End Function


        Public Shared Function getContentInfoVideoByTag(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
            End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition " + _
                                "           ,  cn.embedVideo, cn.title, cn.synopsis, cn.content " + _
                                "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description " + _
                                "from       tr_content cn " + _
                                "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef and ct.contentRef = cn.contentRef " + _
                                "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                                "where	    ct.domainRef = @domainRef and ct.tagRef = @tagRef " + _
                                queryPublishDate + queryExpiredDate + _
                                " order by   cn.contentDate DESC "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function insertFeedBack(ByVal domainRef As String, ByVal name As String, ByVal email As String, _
                                               ByVal mobilePhone As String, ByVal message As String) As String
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
                sqlCmd.CommandText = "select	isnull(max(feedbackRef),0) + 1 as ref " + _
                                    "from " + _NSMainDB + "..TR_feedback "


                sqlDr = sqlCmd.ExecuteReader
                If sqlDr.Read Then
                    result = sqlDr("ref")
                End If
                sqlDr.Close()


                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "insert	into " + _NSMainDB + "..TR_feedback " + _
                                     "           (feedbackRef, domainRef, name, email, mobilePhone, message) " + _
                                     "values	    (@feedbackRef, @domainRef,  @name, @email, @mobilePhone, @message) "

                sqlCmd.Parameters.AddWithValue("@feedbackRef", result)
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@name", name)
                sqlCmd.Parameters.AddWithValue("@email", email)
                sqlCmd.Parameters.AddWithValue("@mobilePhone", mobilePhone)
                sqlCmd.Parameters.AddWithValue("@message", message)

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
        'Job Application
        'Public Shared Function insertJobApplication(ByVal domainRef As String, ByVal Position As String, ByVal firstName As String, ByVal lastName As String, _
        '                                      ByVal Email As String, ByVal mobilePhone As String, ByVal Experience As String, ByVal fileCV As String) As String
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
        '        sqlCmd.CommandText = "select	isnull(max(jobAppRef),0) + 1 as ref " + _
        '                            "from " + _NSMainDB + "..TR_JobApplication "


        '        sqlDr = sqlCmd.ExecuteReader
        '        If sqlDr.Read Then
        '            result = sqlDr("ref")
        '        End If
        '        sqlDr.Close()


        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "insert	into " + _NSMainDB + "..TR_JobApplication " + _
        '                             "           (jobAppRef, domainRef, Position, firstName, lastName, Email, mobilePhone,Experience,fileCv) " + _
        '                             "values    (@jobAppRef, @domainRef, @Position, @firstName, @lastName, @Email, @mobilePhone, @Experience, @fileCv) "

        '        sqlCmd.Parameters.AddWithValue("@jobAppRef", result)
        '        sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '        sqlCmd.Parameters.AddWithValue("@Position", Position)
        '        sqlCmd.Parameters.AddWithValue("@firstName", firstName)
        '        sqlCmd.Parameters.AddWithValue("@lastName", lastName)
        '        sqlCmd.Parameters.AddWithValue("@Email", Email)
        '        sqlCmd.Parameters.AddWithValue("@mobilePhone", mobilePhone)
        '        sqlCmd.Parameters.AddWithValue("@Experience", Experience)
        '        sqlCmd.Parameters.AddWithValue("@fileCv", fileCv)

        '        sqlCmd.ExecuteNonQuery()


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
        Public Shared Function getTitleContent(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon


            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select   tagname, description " + _
                                 "From " + _
                                 "      MS_tag " + _
                                 "Where domainRef =@domainRef and tagRef = @tagRef "
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()
            Return dt
        End Function

        Public Shared Function getTagRefwithExceptionByParentRef(ByVal domainRef As String, ByVal tagRefParent As String, ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select   tagRef " + _
                                 "From " + _
                                 "      MS_tag " + _
                                 "Where domainRef =@domainRef and tagRefParent = @tagRefParent and not tagRef=@tagRef"
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()
            Return dt
        End Function

        Public Shared Function getContentRefOurProduct(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select   c.contentRef " + _
                                 "From " + _
                                 "      TR_Content c" + _
                                 " Inner Join tr_contentTag ct on ct.contentRef = c.contentRef " + _
                                 "Where ct.domainRef =@domainRef and ct.tagRef=@tagRef"
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()
            Return dt
        End Function

        Public Shared Function getImageRefByContentRef(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select imgRef from TR_contentImage where domainRef =@domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("imgRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()



            Return result
        End Function
    End Class
End Namespace

