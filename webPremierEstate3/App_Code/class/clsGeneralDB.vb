Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports [class].clsWebGeneral
Imports [class].clsContentDB
Imports [class].clsGeneral

Namespace [class]
    Public Class clsGeneralDB

        Public Shared Function GetCaptchaTop(ByVal ipAddress As String) As DataTable
            Dim result As New DataTable
            Dim sqlCon As New SqlConnection(_conStrLDS)
            Dim sqlCmd As New SqlCommand



            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select top 1 captcha, ipAddress " + _
                                 "from " + _
                                 "   LG_captcha " + _
                                 "WHERE  ipAddress = @ipAddress and useDate is null " + _
                                 "order by inputTime desc "

            sqlCmd.Parameters.AddWithValue("@ipAddress", ipAddress)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(result)

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        Public Shared Function insertNewCaptcha(ByVal ipAddress As String, ByVal captcha As String, ByVal inputUN As String) As String
            captcha = captcha.Replace(" ", "")
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStrLDS)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "insert	into LG_captcha " + _
                                     "           (captcha, ipAddress, inputUN) " + _
                                     "values	    (@captcha, @ipAddress, @inputUN) "

                sqlCmd.Parameters.AddWithValue("@captcha", captcha)
                sqlCmd.Parameters.AddWithValue("@ipAddress", ipAddress)
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
        Public Shared Function getTagName(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select tagName from ms_Tag where domainRef = @domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("tagName")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()



            Return result
        End Function
        Public Shared Function getContentListByTagRef(ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select ct.contentRef, ct.title from TR_Content ct" + _
                                 " INNER JOIN TR_contentTag cg on cg.contentRef = ct.contentRef" + _
                                 " where ct.domainRef = @domainRef and cg.tagRef = @tagRef  "

            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentRefByTagRefAndURL(ByVal contentName As String, ByVal rootPath As String, ByRef contentRef As String, ByRef tagRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = GetContentListByTagRef(tagRef)
            For i As Integer = 0 To dt.Rows.Count - 1
                If contentName.Contains(convertStrToParam(dt.Rows(i).Item("title").ToString.ToLower)) AndAlso dt.Rows(i).Item("title").ToString.ToLower.Length <> 1 Then
                    contentRef = dt.Rows(i).Item("contentRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        Public Shared Function getDescription(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select description from ms_Tag where domainRef =@domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("description")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getPicture(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            'Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select tagPicture, tagPictureFile from ms_Tag where domainRef =@domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt

        End Function

        'Public Shared Function getContent(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As String
        '    Dim connectionString As String = ""

        '    connectionString = _conStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim result As String = ""

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text
        '    sqlCmd.CommandText = "select contentDisplayRef from ms_Tag where domainRef =@domainRef and tagRef = @tagRef "

        '    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("contentDisplayRef")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        Public Shared Function cekURLTag(ByVal url As String, ByVal rootPath As String, _
                                         ByRef paramOut As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable

            dt = [class].clsContentDB.getTagTreeList(_tagTypeMenu, "0", "1")
            For i = 0 To dt.Rows.Count - 1
                If url.ToLower.Contains((rootPath + convertStrToParam(dt.Rows(i).Item("tagName").ToString.ToLower)).ToLower) Then
                    paramOut = dt.Rows(i).Item("tagRef").ToString + "|" + convertStrToParam(dt.Rows(i).Item("tagName").ToString.ToLower)
                    result = True
                    Exit For
                End If
            Next

            Return result
        End Function
        
        Public Shared Function getTagRefByURL(ByVal tagName As String, ByVal rootPath As String, ByRef tagRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getTagList()
            For i As Integer = 0 To dt.Rows.Count - 1
                If tagName.Contains(convertStrToParam(dt.Rows(i).Item("tagName").ToString.ToLower)) Then
                    tagRef = dt.Rows(i).Item("tagRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        Public Shared Function getTagRefByURLAndTagRefParent(ByVal tagName As String, ByRef tagRefParent As String, ByVal rootPath As String, ByRef tagRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getTagListByTagRefParent(tagRefParent)
            For i As Integer = 0 To dt.Rows.Count - 1
                If tagName.Contains(convertStrToParam(dt.Rows(i).Item("tagName").ToString.ToLower)) Then
                    tagRef = dt.Rows(i).Item("tagRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        'Public Shared Function getContentRefByURLNapro(ByVal contentName As String, ByVal tagRef As String, ByVal rootPath As String, ByRef contentRef As String) As Boolean
        '    Dim result As Boolean = False
        '    Dim dt As New DataTable
        '    dt = getContentListNapro(tagRef)
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If contentName.Contains(convertStrToParam(dt.Rows(i).Item("title").ToString.ToLower)) Then
        '            contentRef = dt.Rows(i).Item("contentRef").ToString
        '            result = True
        '            Exit For
        '        End If
        '    Next
        '    Return result
        'End Function

        'Public Shared Function getTagRefByURLNapro(ByVal tagName As String, ByVal rootPath As String, ByRef tagRef As String) As Boolean
        '    Dim result As Boolean = False
        '    Dim dt As New DataTable
        '    dt = getTagListNapro()
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If tagName.Contains(convertStrToParam(dt.Rows(i).Item("tagName").ToString.ToLower)) Then
        '            tagRef = dt.Rows(i).Item("tagRef").ToString
        '            result = True
        '            Exit For
        '        End If
        '    Next
        '    Return result
        'End Function

        Public Shared Function getContentRefByURL(ByVal contentName As String, ByVal rootPath As String, ByRef contentRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getContentList()
            For i As Integer = 0 To dt.Rows.Count - 1
               ' Dim title As String = convertStrToParam(dt.Rows(i).Item("title").ToString.ToLower)
                'If contentName.Contains(convertStrToParam(dt.Rows(i).Item("title").ToString.ToLower)) AndAlso dt.Rows(i).Item("title").ToString.ToLower.Length  <> 0  Then
                '    contentRef = dt.Rows(i).Item("contentRef").ToString
                '    result = True
                '    Exit For
                'End If
                If contentName.ToLower() = convertStrToParam(dt.Rows(i).Item("title").ToString.ToLower) Then 
                    contentRef = dt.Rows(i).Item("contentRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        Public Shared Function getContentRefByURLWebsite(ByVal contentRef As String, ByVal rootPath As String, ByRef contentName As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getContentListWebsite(contentRef)
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim title As String = convertStrToParam(dt.Rows(i).Item("title").ToString.ToLower)
                If contentName.Contains(title) Then
                    contentRef = dt.Rows(i).Item("contentRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        'Public Shared Function getProductRef(ByVal productName As String, ByVal rootPath As String, ByRef clusterRef As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef productRef As String) As Boolean
        '    Dim result As Boolean = False
        '    Dim dt As New DataTable
        '    dt = getProductList(dbMasterRef, projectRef, clusterRef)
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If productName.Equals(convertStrToParam(dt.Rows(i).Item("titleProduct").ToString.ToLower)) Then
        '            productRef = dt.Rows(i).Item("productRef").ToString
        '            result = True
        '            Exit For
        '        End If
        '    Next
        '    Return result
        'End Function

        'Public Shared Function getCategoryRef(ByVal categoryName As String, ByVal rootPath As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef categoryRef As String) As Boolean
        '    Dim result As Boolean = False
        '    Dim dt As New DataTable
        '    dt = getCategoryList(dbMasterRef, projectRef)
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If categoryName.Contains(convertStrToParam(dt.Rows(i).Item("categoryDescription").ToString.ToLower)) Then
        '            categoryRef = dt.Rows(i).Item("categoryRef").ToString
        '            result = True
        '            Exit For
        '        End If
        '    Next
        '    Return result
        'End Function

        'Public Shared Function getClusterRef(ByVal clusterName As String, ByVal rootPath As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef categoryRef As String, ByRef clusterRef As String) As Boolean
        '    Dim result As Boolean = False
        '    Dim dt As New DataTable
        '    dt = getClusterList(dbMasterRef, projectRef, categoryRef)
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If clusterName.Contains(convertStrToParam(dt.Rows(i).Item("clusterDescription").ToString.ToLower)) Then
        '            clusterRef = dt.Rows(i).Item("clusterRef").ToString
        '            result = True
        '            Exit For
        '        End If
        '    Next
        '    Return result
        'End Function
        Public Shared Function getProductRef(ByVal productName As String, ByVal rootPath As String, ByRef clusterRef As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef productRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getProductList(dbMasterRef, projectRef, clusterRef)
            For i As Integer = 0 To dt.Rows.Count - 1
                If productName.Equals(convertStrToParam(dt.Rows(i).Item("titleProduct").ToString.ToLower)) Then
                    productRef = dt.Rows(i).Item("productRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function
        Public Shared Function getCategoryRef(ByVal categoryName As String, ByVal rootPath As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef categoryRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getCategoryList(dbMasterRef, projectRef)
            For i As Integer = 0 To dt.Rows.Count - 1
                If categoryName.Contains(convertStrToParam(dt.Rows(i).Item("categoryDescription").ToString.ToLower)) Then
                    categoryRef = dt.Rows(i).Item("categoryRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function
        Public Shared Function getClusterRef(ByVal clusterName As String, ByVal rootPath As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef categoryRef As String, ByRef clusterRef As String) As Boolean
            Dim result As Boolean = False
            Dim dt As New DataTable
            dt = getClusterList(dbMasterRef, projectRef, categoryRef)
            For i As Integer = 0 To dt.Rows.Count - 1
                If clusterName.Contains(convertStrToParam(dt.Rows(i).Item("clusterDescription").ToString.ToLower)) Then
                    clusterRef = dt.Rows(i).Item("clusterRef").ToString
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        Public Shared Sub setMetaData(ByVal masterPage As MasterPage, ByVal tagRef As String, ByVal contentRef As String)

            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim metadesc As String = ""
            Dim keywords As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            If contentRef = "" Then
                sqlCmd.CommandText = "select tc.metadescription as metadesc " + _
                                     "from TR_content tc " + _
                                     "inner join TR_contentTag tt on tt.contentRef = tc.contentRef " + _
                                     "where tt.tagRef = @tagRef "

                sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            Else
                sqlCmd.CommandText = "select metadescription as metadesc " + _
                                     "from TR_content " + _
                                     "where contentRef = @contentRef "

                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            End If
            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                metadesc = sqlDr("metadesc")
            End If

            sqlDr.Close()

            sqlCmd.Parameters.Clear()

            If contentRef = "" Then
                sqlCmd.CommandText = "select tk.keywordText as keywords " + _
                                     "from TR_contentKeyword tk " + _
                                     "inner join TR_contentTag tt on tt.contentRef = tk.contentRef " + _
                                     "where tt.tagRef = @tagRef "

                sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            Else
                sqlCmd.CommandText = "select keywordText as keywords " + _
                                     "from TR_contentKeyword " + _
                                     "where contentRef = @contentRef "

                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            End If
            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                keywords = sqlDr("keywords")
            End If

            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            ''''''Set Metadata''''''
            'Similiarly we can add description Meta Tag to Head Section        
            Dim headContentPlaceHolder As ContentPlaceHolder = DirectCast(masterPage.FindControl("head"), ContentPlaceHolder)
            Dim htMeta As New HtmlMeta()
            htMeta.Attributes.Add("name", "description")
            htMeta.Attributes.Add("content", metadesc)
            'adding  Meta Tag to Head                
            headContentPlaceHolder.Controls.Add(htMeta)

            'Similiarly we can add keyword Meta Tag to Head Section        
            Dim htMeta1 As New HtmlMeta()
            htMeta1.Attributes.Add("name", "keywords")
            htMeta1.Attributes.Add("content", keywords)
            'adding the Meta Tag to Head
            headContentPlaceHolder.Controls.Add(htMeta1)
            ''''''End Set Metadata''''''

        End Sub

        'Public Shared Function getDbMasterRefByProjectCode(ByVal projectCode As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlnaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlnaproCon.Open()
        '    sqlCmd.Connection = sqlnaproCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select dbMasterRef " + _
        '                         "From " + _
        '                         "SYS_dbMasterProjectSetting " + _
        '                         "Where settingValue = @projectCode "

        '    sqlCmd.Parameters.AddWithValue("@projectCode", projectCode)


        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("dbMasterRef")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getProjectRefByProjectCode(ByVal projectCode As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlnaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlnaproCon.Open()
        '    sqlCmd.Connection = sqlnaproCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select projectRef " + _
        '                         "From " + _
        '                         "SYS_dbMasterProjectSetting " + _
        '                         "Where settingValue = @projectCode "

        '    sqlCmd.Parameters.AddWithValue("@projectCode", projectCode)
        '    'sqlCmd.Parameters.AddWithValue("@settingRef", _settingRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("projectRef")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getClusterListt(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select clusterRef, clusterDescription from MS_dbMasterProjectCluster " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getClusterReff(ByVal clusterName As String, ByVal rootPath As String, ByRef projectRef As String, ByRef dbMasterRef As String, ByRef clusterRef As String) As Boolean
        '    Dim result As Boolean = False
        '    Dim dt As New DataTable
        '    dt = getClusterListt(dbMasterRef, projectRef)
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If clusterName.Contains(convertStrToParam(dt.Rows(i).Item("clusterDescription").ToString.ToLower)) Then
        '            clusterRef = dt.Rows(i).Item("clusterRef").ToString
        '            result = True
        '            Exit For
        '        End If
        '    Next
        '    Return result
        'End Function

#Region "Subscribe"
        'Public Shared Function insertSubscribeNewsletter(ByVal email As String, ByVal hp As String, ByVal name As String, ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlTrans As SqlTransaction
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlTrans = sqlCon.BeginTransaction

        '    sqlCmd.Transaction = sqlTrans
        '    sqlCmd.Connection = sqlCon


        '    Try

        '        Dim newsletterSubscribeRef As Integer = 0

        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()
        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "select	isnull(max(newsletterSubscribeRef),0) + 1 as ref " + _
        '                                "from TR_newsletterSubscribe "

        '        sqlDr = sqlCmd.ExecuteReader
        '        If sqlDr.Read Then
        '            newsletterSubscribeRef = sqlDr("ref")
        '        End If
        '        sqlDr.Close()



        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()
        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "Insert into TR_newsletterSubscribe(newsletterSubscribeRef,email,hp,name,dbMasterRef,projectRef) " + _
        '                             "Values	                        (@newsletterSubscribeRef,@email,@hp,@name,@dbMasterRef,@projectRef)  "

        '        sqlCmd.Parameters.AddWithValue("@newsletterSubscribeRef", newsletterSubscribeRef)
        '        sqlCmd.Parameters.AddWithValue("@email", email)
        '        sqlCmd.Parameters.AddWithValue("@hp", hp)
        '        sqlCmd.Parameters.AddWithValue("@name", name)
        '        sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '        sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '        sqlCmd.ExecuteNonQuery()

        '        result = newsletterSubscribeRef

        '        sqlTrans.Commit()
        '    Catch ex As Exception
        '        sqlTrans.Rollback()
        '        result = ex.Message
        '    End Try

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function
#End Region

        'Public Shared Function getSubscribeEmailNotification(ByVal newsletterSubscribeRef As String) As DataTable
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dr As DataRow
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    dt.columns.add("newsletterSubscribeRef")
        '    dt.columns.add("email")
        '    dt.columns.add("hp")
        '    dt.columns.add("name")
        '    dt.columns.add("dbMasterRef")
        '    dt.columns.add("projectRef")

        '    sqlcmd.commandtext = "   select newsletterSubscribeRef,email,hp,name,dbMasterRef,projectRef " + _
        '                         "   from TR_newsletterSubscribe " + _
        '                         "   where newsletterSubscribeRef = @newsletterSubscribeRef "

        '    sqlcmd.parameters.addwithvalue("@newsletterSubscribeRef", newsletterSubscribeRef)

        '    sqldr = sqlcmd.executereader
        '    if sqldr.read then
        '        dr = dt.newrow
        '        dr("newsletterSubscribeRef") = sqldr("newsletterSubscribeRef")
        '        dr("email") = sqldr("email")
        '        dr("hp") = sqldr("hp")
        '        dr("name") = sqldr("name")
        '        dr("dbMasterRef") = sqldr("dbMasterRef")
        '        dr("projectRef") = sqldr("projectRef")
        '        dt.rows.add(dr)
        '    end if

        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()       

        '    Return dt
        'End Function

        Public Shared Function getContactUsNotification(ByVal feedbackRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.Columns.Add("feedbackRef")
            dt.Columns.Add("domainRef")
            dt.Columns.Add("name")
            dt.Columns.Add("email")
            dt.Columns.Add("mobilePhone")
            dt.Columns.Add("message")
            sqlCmd.CommandText = "   select feedbackRef, domainRef, name, email, mobilePhone, message " + _
                                 "   from TR_feedback " + _
                                 "   where feedbackRef = @feedbackRef "

            sqlCmd.Parameters.AddWithValue("@feedbackRef", feedbackRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                dr = dt.NewRow
                dr("feedbackRef") = sqlDr("feedbackRef")
                dr("domainRef") = sqlDr("domainRef")
                dr("name") = sqlDr("name")
                dr("email") = sqlDr("email")
                dr("mobilePhone") = sqlDr("mobilePhone")
                dr("message") = sqlDr("message")
                dt.Rows.Add(dr)
            End If

            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function
        'Notification career
        Public Shared Function getCareerNotification(ByVal jobAppRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.columns.add("jobAppRef")
            dt.columns.add("domainRef")
            dt.columns.add("Position")
            dt.columns.add("firstName")
            dt.columns.add("lastName")
            dt.columns.add("Email")
            dt.columns.add("mobilePhone")
            dt.columns.add("Experience")
            dt.columns.add("fileCv")
             
            sqlcmd.commandtext = "   select jobAppRef, domainRef, Position, firstName, lastName, Email, mobilePhone,Experience,fileCv " + _
                                 "   from TR_jobApplication " + _
                                 "   where jobAppRef = @jobAppRef "

            sqlcmd.parameters.addwithvalue("@jobAppRef", jobAppRef)

            sqldr = sqlcmd.executereader
            if sqldr.read then
                dr = dt.newrow
                dr("jobAppRef") = sqldr("jobAppRef")
                dr("domainRef") = sqldr("domainRef")
                dr("Position") = sqldr("Position")
                dr("firstName") = sqldr("firstName")
                dr("lastName") = sqldr("lastName")
                dr("Email") = sqldr("Email")
                dr("mobilePhone") = sqldr("mobilePhone")
                dr("Experience") = sqldr("Experience")
                dr("fileCv") = sqldr("fileCv")
                dt.rows.add(dr)
            end if

            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()       

            Return dt
        End Function

          Public Shared Function insertRequestCallLead(ByVal leadName As String, ByVal leadPhone As String, ByVal projectRef As String, ByVal scheduleDate As System.Data.SqlTypes.SqlDateTime, ByVal responseRef As String, ByVal followUpAction As String, ByVal scheduleDesc As String) As String
            Dim result As String = String.Empty

            Dim sqlCmd As New SqlCommand
            Dim sqlCon As New SqlConnection(_conStrLDS)
            Dim sqlTrans As SqlTransaction
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                Dim newHp As String = leadPhone

                If leadPhone.Trim() <> "" Then
                    Dim firstHp As String = leadPhone.Substring(0, 1)

                    If firstHp.Trim() = "0" Then
                        newHp = leadPhone.Remove(0, 1)
                    ElseIf firstHp.Trim() = "6" Then
                        If leadPhone.Substring(2, 1) = "0" Then
                            newHp = leadPhone.Remove(0, 3)
                        Else
                            newHp = leadPhone.Remove(0, 2)
                        End If
                    ElseIf firstHp.Trim() = "+" Then
                        newHp = leadPhone.Remove(0, 3)
                    End If

                    newHp = "62" + newHp
                End If

                Dim leadRef As Integer = 0

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "Select	leadRef " + _
                                            "from MS_lead where leadPhone = @leadPhone and projectRef = @projectRef "

                sqlCmd.Parameters.AddWithValue("@leadPhone", newHp)
                sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
                sqlDr = sqlCmd.ExecuteReader
                If sqlDr.Read Then
                    leadRef = sqlDr("leadRef")
                End If
                sqlDr.Close()

                If leadRef = 0 Then
                    sqlCmd.Parameters.Clear()
                    sqlCmd.Prepare()
                    sqlCmd.CommandType = CommandType.Text
                    sqlCmd.CommandText = "select	isnull(max(leadRef),0) + 1 as leadRef " + _
                                            "from MS_lead "

                    sqlDr = sqlCmd.ExecuteReader
                    If sqlDr.Read Then
                        leadRef = sqlDr("leadRef")
                    End If
                    sqlDr.Close()

                    sqlCmd.Parameters.Clear()
                    sqlCmd.Prepare()
                    sqlCmd.CommandType = CommandType.Text
                    sqlCmd.CommandText = "Insert into MS_lead(leadRef, leadName, leadPhone, projectRef, isAutoDistribute, realPhone, noted, sourceAds, inputUN) " + _
                                         "Values (@leadRef, @leadName, @leadPhone, @projectRef, 1, @realPhone, @noted, @sourceAds, @inputUN)  "

                    sqlCmd.Parameters.AddWithValue("@leadRef", leadRef)
                    sqlCmd.Parameters.AddWithValue("@leadName", leadName)
                    sqlCmd.Parameters.AddWithValue("@leadPhone", newHp)
                    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
                    sqlCmd.Parameters.AddWithValue("@realPhone", leadPhone)
                    If Not IsDBNull(scheduleDate) Then
                        Dim dt As DateTime = CType(scheduleDate, DateTime)
                        scheduleDesc = scheduleDesc + ", minta dihubungi tanggal " + Format(dt, "dd MMM yyyy HH:mm")
                    End If
                    sqlCmd.Parameters.AddWithValue("@noted", scheduleDesc)
                    sqlCmd.Parameters.AddWithValue("@sourceAds", _sourceAdsWebSEO)

                    'ini diisi sesuai domain web
                    sqlCmd.Parameters.AddWithValue("@inputUN", _domainName)
                    sqlCmd.ExecuteNonQuery()
                Else


                    Dim callCenterPsRef As Integer = 0
                    sqlCmd.Parameters.Clear()
                    sqlCmd.Prepare()
                    sqlCmd.CommandType = CommandType.Text
                    sqlCmd.CommandText = "select	top 1 psRef from " + _
                                            " tr_personalLead where leadRef = @leadRef and projectRef = @projectRef "

                    sqlCmd.Parameters.AddWithValue("@leadRef", leadRef)
                    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
                    sqlDr = sqlCmd.ExecuteReader
                    If sqlDr.Read Then
                        callCenterPsRef = sqlDr("psRef")
                    End If
                    sqlDr.Close()

                    If callCenterPsRef <> 0 Then
                        Dim FCMToken As String = ""
                        sqlCmd.Parameters.Clear()
                        sqlCmd.Prepare()
                        sqlCmd.CommandType = CommandType.Text
                        sqlCmd.CommandText = "select	FCMToken from " + _
                                                " LG_FCMToken where psRef = @psRef "

                        sqlCmd.Parameters.AddWithValue("@psRef", callCenterPsRef)
                        sqlDr = sqlCmd.ExecuteReader
                        If sqlDr.Read Then
                            FCMToken = sqlDr("FCMToken")
                        End If
                        sqlDr.Close()

                        Dim notificationRef As Integer = 0
                        sqlCmd.Parameters.Clear()
                        sqlCmd.Prepare()
                        sqlCmd.CommandType = CommandType.Text
                        sqlCmd.CommandText = "Select isnull(max(notificationRef),0) + 1 as ref from " + _
                                                " LG_notification "

                        sqlDr = sqlCmd.ExecuteReader
                        If sqlDr.Read Then
                            notificationRef = sqlDr("ref")
                        End If
                        sqlDr.Close()

                        Dim msg As String = "Hi, Anda mendapatkan lead baru."
                        sqlCmd.Parameters.Clear()
                        sqlCmd.Prepare()

                        sqlCmd.CommandType = CommandType.Text
                        sqlCmd.CommandText = "insert into " + _
                                                "  LG_notification(notificationRef,psRef,appsType,notificationType,title,synopsis,message,sendScheduleTime,FCMToken,inputUN) " + _
                                                " values(@notificationRef,@psRef,@appsType,@notificationType,@title,@synopsis,@message,DATEADD(MINUTE,-10,GETDATE()),@FCMToken,@inputUN) "

                        sqlCmd.Parameters.AddWithValue("@notificationRef", notificationRef)
                        sqlCmd.Parameters.AddWithValue("@psRef", callCenterPsRef)
                        sqlCmd.Parameters.AddWithValue("@appsType", "1")
                        sqlCmd.Parameters.AddWithValue("@FCMToken", FCMToken)
                        sqlCmd.Parameters.AddWithValue("@notificationType", "1")
                        sqlCmd.Parameters.AddWithValue("@title", "Lead Baru")
                        sqlCmd.Parameters.AddWithValue("@synopsis", msg)
                        sqlCmd.Parameters.AddWithValue("@message", msg)
                        sqlCmd.Parameters.AddWithValue("@inputUN", _domainName)
                        sqlCmd.ExecuteNonQuery()
                    End If

                    
                End If

                result = 1

                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        'Public Shared Function getInquiriesNotification(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal adsRef As String, ByVal leadRef As String) As DataTable
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dr As DataRow
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    dt.columns.add("titleAds")
        '    dt.columns.add("descriptionAds")
        '    dt.columns.add("emailAds")
        '    dt.columns.add("dbMasterRef")
        '    dt.columns.add("projectRef")
        '    dt.columns.add("adsRef")
        '    dt.columns.add("leadRef")
        '    dt.columns.add("name")
        '    dt.columns.add("hp")
        '    dt.columns.add("email")
        '    dt.columns.add("inquiriesType")
        '    dt.columns.add("inquiriesTypeName")
        '    dt.columns.add("inquiriesText")
        '    dt.columns.add("inputUN")

        '    sqlcmd.commandtext = "   select ad.titleAds, ad.descriptionAds, ad.email as emailAds, tr.dbMasterRef, tr.projectRef, tr.adsRef, tr.leadRef, tr.name, tr.hp, tr.email, tr.inquiriesType, tr.inquiriesText, tr.inputUN, lk.inquiriesTypeName " + _
        '                         "   from TR_Ads ad " + _
        '                         "   inner join TR_AdsLEAD tr on tr.dbMasterRef = ad.dbMasterRef and tr.dbMasterRef = ad.dbMasterRef and tr.projectRef = ad.projectRef and tr.adsRef = ad.adsRef  " + _
        '                         "   left join LK_adsInquiries lk on lk.inquiriesType = tr.inquiriesType  " + _
        '                         "   where tr.dbMasterRef=@dbMasterRef and tr.projectRef=@projectRef and tr.adsRef=@adsRef and tr.leadRef=@leadRef "

        '    sqlcmd.parameters.addwithvalue("@dbMasterRef", dbMasterRef)
        '    sqlcmd.parameters.addwithvalue("@projectRef", projectRef)
        '    sqlcmd.parameters.addwithvalue("@adsRef", adsRef)
        '    sqlcmd.parameters.addwithvalue("@leadRef", leadRef)

        '    sqldr = sqlcmd.executereader
        '    if sqldr.read then
        '        dr = dt.newrow
        '        dr("titleAds") = sqldr("titleAds")
        '        dr("descriptionAds") = sqldr("descriptionAds")
        '        dr("emailAds") = sqldr("emailAds")
        '        dr("dbMasterRef") = sqldr("dbMasterRef")
        '        dr("projectRef") = sqldr("projectRef")
        '        dr("adsRef") = sqldr("adsRef")
        '        dr("leadRef") = sqldr("leadRef")
        '        dr("name") = sqldr("name")
        '        dr("hp") = sqldr("hp")
        '        dr("email") = sqldr("email")
        '        dr("inquiriesType") = sqldr("inquiriesType")
        '        dr("inquiriesTypeName") = sqldr("inquiriesTypeName")
        '        dr("inquiriesText") = sqldr("inquiriesText")
        '        dr("inputUN") = sqldr("inputUN")
        '        dt.rows.add(dr)
        '    end if

        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()       

        '    Return dt
        'End Function

#Region "Meta From NataProperty"
        'Public Shared Function getMetaTitleProject(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlnaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlnaproCon.Open()
        '    sqlCmd.Connection = sqlnaproCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select metaTitle " + _
        '                         "From " + _
        '                         "MS_dbMasterProject " + _
        '                         "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("metaTitle")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaAuthorProject(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlnaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlnaproCon.Open()
        '    sqlCmd.Connection = sqlnaproCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select metaAuthor " + _
        '                         "From " + _
        '                         "MS_dbMasterProject " + _
        '                         "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("metaAuthor")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaKeywordProject(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlnaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlnaproCon.Open()
        '    sqlCmd.Connection = sqlnaproCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select metaKeyword " + _
        '                         "From " + _
        '                         "MS_dbMasterProject " + _
        '                         "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("metaKeyword")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaDescriptionProject(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlnaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlnaproCon.Open()
        '    sqlCmd.Connection = sqlnaproCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select metaDescription " + _
        '                         "From " + _
        '                         "MS_dbMasterProject " + _
        '                         "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("metaDescription")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

#End Region

#Region "Meta From CMS Website"
        Public Shared Function getMetaTitleContent(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaTitle " + _
                                 "From " + _
                                 "TR_content " + _
                                 "Where domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaTitle")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        Public Shared Function getMetaKeywordContent(ByVal domainRef As String, ByVal contentRef As String) As DataTable
            Dim result As New DataTable
            Dim sqlCon As New SqlConnection(_constr)
            Dim sqlCmd As New SqlCommand

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select k.* FROM TR_contentKeyword k " + _
                                 "INNER JOIN TR_content c On k.contentRef = c.contentRef " + _
                                 "where c.domainRef = @domainRef and k.contentRef = @contentRef and k.keywordText <> '' "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(result)
            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaAuthorContent(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaAuthor " + _
                                 "From " + _
                                 "TR_content " + _
                                 "Where domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaAuthor")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaDescriptionContent(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metadescription " + _
                                 "From " + _
                                 "TR_content " + _
                                 "Where domainRef = @domainRef and contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metadescription")
            End If
            sqlDr.Close()

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
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            If strKeyword.ToString.Length > 0 Then
                result = Left(strKeyword.ToString, strKeyword.ToString.Length - 2)
            End If

            Return result
        End Function
        Public Shared Function getMetaTitleTag(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaTitle, tagName " + _
                                 "From " + _
                                 "MS_tag " + _
                                 "Where domainRef = @domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaTitle")
                If Trim(result) = "" Then
                    result = "" + _websiteProjectName + " | " + sqlDr("tagName")
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaAuthorTag(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaAuthor " + _
                                 "From " + _
                                 "MS_tag " + _
                                 "Where domainRef = @domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaAuthor")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaKeywordTag(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select keyword " + _
                                 "From " + _
                                 "MS_tag " + _
                                 "Where domainRef = @domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("keyword")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaDescriptionTag(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaDescription " + _
                                 "From " + _
                                 "MS_tag " + _
                                 "Where domainRef = @domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaDescription")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        Public Shared Function getMetaTitle(ByVal domainRef As String, ByVal metaRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaTitle " + _
                                 "From " + _
                                 "MS_meta " + _
                                 "Where domainRef = @domainRef and metaRef = @metaRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaTitle")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        

        Public Shared Function getMetaAuthor(ByVal domainRef As String, ByVal metaRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaAuthor " + _
                                 "From " + _
                                 "MS_meta " + _
                                 "Where domainRef = @domainRef and metaRef = @metaRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaAuthor")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        
        Public Shared Function getMetaKeyword(ByVal domainRef As String, ByVal metaRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaKeyword " + _
                                 "From " + _
                                 "MS_meta " + _
                                 "Where domainRef = @domainRef and metaRef = @metaRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaKeyword")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaDescription(ByVal domainRef As String, ByVal metaRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select metaDescription " + _
                                 "From " + _
                                 "MS_meta " + _
                                 "Where domainRef = @domainRef and metaRef = @metaRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaDescription")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

#End Region

        #Region "LDS"
        Public Shared Function getProjectName(ByVal projectRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStrLDS

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select projectName " + _
                                 "From " + _
                                 "ms_project " + _
                                 "Where projectRef = @projectRef  "

            sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("projectName")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
#End Region
    End Class


End Namespace
