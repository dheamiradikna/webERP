Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports [class].clsWebGeneral
Imports [class].clsGeneralSetting
Imports System.Net.Dns
Imports System.Net

Namespace [class]
    Public Class clsContentDB
        Public Shared Function GetCaptchaInfo(ByVal captcha As String, ByVal ipAddress As String) As DataTable
            Dim result As New DataTable
            Dim sqlCon As New SqlConnection(_conStrLDS)
            Dim sqlCmd As New SqlCommand



            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	captcha, ipAddress " + _
                                 "from " + _
                                 "   LG_captcha " + _
                                 "WHERE  captcha = @captcha and ipAddress = @ipAddress and useDate is null "

            
            sqlCmd.Parameters.AddWithValue("@captcha", captcha)
            sqlCmd.Parameters.AddWithValue("@ipAddress", ipAddress)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(result)

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        
        Public Shared Function insertRequestCallLeadV2(ByVal leadName As String, ByVal leadPhone As String, ByVal leadEmail As String, ByVal projectRef As String, ByVal responseRef As String, ByVal followUpAction As String, ByVal scheduleDesc As String) As String
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
                    sqlCmd.CommandText = "Insert into MS_lead(leadRef, leadName, leadPhone, leadEmail, projectRef, isAutoDistribute, realPhone, noted, sourceAds, inputUN) " + _
                                         "Values (@leadRef, @leadName, @leadPhone, @leadEmail, @projectRef, 1, @realPhone, @noted, @sourceAds, @inputUN)  "

                    sqlCmd.Parameters.AddWithValue("@leadRef", leadRef)
                    sqlCmd.Parameters.AddWithValue("@leadName", leadName)
                    sqlCmd.Parameters.AddWithValue("@leadPhone", newHp)
                    sqlCmd.Parameters.AddWithValue("@leadEmail", leadEmail)
                    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
                    sqlCmd.Parameters.AddWithValue("@realPhone", leadPhone)
                    sqlCmd.Parameters.AddWithValue("@noted", scheduleDesc)
                    sqlCmd.Parameters.AddWithValue("@sourceAds", _sourceAdsWebSEO)

                    'ini diisi sesuai domain web
                    sqlCmd.Parameters.AddWithValue("@inputUN", _domainName)
                    sqlCmd.ExecuteNonQuery()

                    sqlCmd.Parameters.Clear()
                    sqlCmd.Prepare()
                    sqlCmd.CommandType = CommandType.Text
                    sqlCmd.CommandText = "Insert into LG_MS_lead(leadRef, leadName, leadPhone, leadEmail, projectRef, isAutoDistribute, realPhone, noted, sourceAds, inputUN) " + _
                                         "Values (@leadRef, @leadName, @leadPhone, @leadEmail, @projectRef, 1, @realPhone, @noted, @sourceAds, @inputUN)  "

                    sqlCmd.Parameters.AddWithValue("@leadRef", leadRef)
                    sqlCmd.Parameters.AddWithValue("@leadName", leadName)
                    sqlCmd.Parameters.AddWithValue("@leadPhone", newHp)
                    sqlCmd.Parameters.AddWithValue("@leadEmail", leadEmail)
                    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
                    sqlCmd.Parameters.AddWithValue("@realPhone", leadPhone)
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
        Public Shared Function updateCaptcha(ByVal ipAddress As String, ByVal captcha As String) As String
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
                sqlCmd.CommandText = "update LG_captcha " + _
                                     " set useDate = getdate() " + _
                                     " where captcha = @captcha and ipAddress = @ipAddress "

                sqlCmd.Parameters.AddWithValue("@captcha", captcha)
                sqlCmd.Parameters.AddWithValue("@ipAddress", ipAddress)

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
        Public Shared Function getRandomDomain(ByVal top As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            Dim queryTop As String = ""
            If Trim(top) <> "" Then
                queryTop = " top " + top.ToString() + " "
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  " + queryTop + " domainRef,domainName,[description] " + _
                                 "From " + _
                                 "      MS_domain " + _
                                 "Where isActive =1 and isUpcomingMalls = 0 " + _
                                 "order by NEWID() "

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListImageRefByTag(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""
            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If
            
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select ci.imgRef, cg.contentRef, ct.contentRef,ct.synopsis,ct.title,ct.titleDetail, ct.content, ct.embedVideo, cg.tagRef, ct.publishDate " + _
                                 "From " + _
                                 "      TR_contentTag cg " + _
                                 "      INNER JOIN TR_Content ct on ct.contentRef = cg.contentRef and ct.domainRef = cg.domainRef " + _
                                 "      INNER JOIN TR_contentImage ci on ci.contentRef = cg.contentRef and ci.contentRef = cg.contentRef and ci.domainRef = cg.domainRef and ci.domainRef = ct.domainRef " + _
                                 "Where cg.tagRef = @tagRef	and cg.domainRef = @domainRef " + _
                                 queryPublishDate + queryExpiredDate 

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListImageRefByTagBannerERP360(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""
            connectionString = _conStrNC

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If
            
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select ci.imgRef, cg.contentRef, ct.contentRef,ct.synopsis,ct.title,ct.titleDetail, ct.content, ct.embedVideo, cg.tagRef, ct.publishDate " + _
                                 "From " + _
                                 "      TR_contentTag cg " + _
                                 "      INNER JOIN TR_Content ct on ct.contentRef = cg.contentRef and ct.domainRef = cg.domainRef " + _
                                 "      INNER JOIN TR_contentImage ci on ci.contentRef = cg.contentRef and ci.contentRef = cg.contentRef and ci.domainRef = cg.domainRef and ci.domainRef = ct.domainRef " + _
                                 "Where cg.tagRef = @tagRef	and cg.domainRef = @domainRef " + _
                                 queryPublishDate + queryExpiredDate 

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListWebMirror() As DataTable
            Dim connectionString As String = ""
            connectionString = _conStrNC

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select wm.webName, wm.webLink, wm.projectRef, 1 as sortNo " + _
                                 "From " + _
                                 "      MS_websiteMirror wm " + _
                                 "Where wm.isActive = 1 and wm.webName LIKE '%www%' " + _
                                 "union " + _
                                 "Select wm.webName, wm.webLink, wm.projectRef, 2 as sortNo " + _
                                 "From " + _
                                 "      MS_websiteMirror wm " + _
                                 "Where wm.isActive = 1 and wm.webName NOT LIKE '%www%' " + _
                                 "order by sortNo asc "
            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagRefByTagName(ByVal domainRef As String, ByVal tagName As String, ByVal isChildSite As Boolean) As Integer
            Dim result As Integer = 0
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select tagRef from ms_Tag " + _
                                 "where tagName like '%" + tagName.ToString() + "%' and isActive = 1 and domainRef=@domainRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("tagRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

         Public Shared Function getTagsWEB(ByVal contentRef As String) As DataTable
            Dim result As New DataTable
            Dim sqlCon As New SqlConnection(_constr)
            Dim sqlCmd As New SqlCommand

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            
            sqlCmd.CommandText = "Select k.* FROM TR_contentKeyword k " + _
                                 "INNER JOIN TR_content c On k.contentRef = c.contentRef " + _
                                 "where k.contentRef = @contentRef and k.keywordText <> '' "

            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(result)
            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getSingleContentFile(ByVal domainRef As String, ByVal imgRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

           If imgRef.Split(".").Length > 0 Then
                imgRef = imgRef.Split(".").First.ToString()
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select domainRef,imgRef,title,imgFileName,imgFile,description,keyword,imgW,imgH " + _
                                "from IMG_TR_image  " + _
                                "where	domainRef = @domainRef and imgRef = @imgRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getSingleContentFileBannerERP(ByVal domainRef As String, ByVal imgRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStrNC

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

           If imgRef.Split(".").Length > 0 Then
                imgRef = imgRef.Split(".").First.ToString()
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select domainRef,imgRef,title,imgFileName,imgFile,description,keyword,imgW,imgH " + _
                                "from IMG_TR_image  " + _
                                "where	domainRef = @domainRef and imgRef = @imgRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getFirstContentImage(ByVal domainRef As String, ByVal contentRef As String, ByVal imgType As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = "0"

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	top 1 imgRef from tr_contentImage where domainRef = @domainRef and contentRef = @contentRef and imgType = @imgType "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("imgRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getAllContentImage(ByVal domainRef As String, ByVal contentRef As String, ByVal imgType As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select imgRef from tr_contentImage where domainRef = @domainRef and contentRef = @contentRef and imgType = @imgType "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)


            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getParentTag(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select tagRefParent from ms_Tag where domainRef =@domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("tagRefParent")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()



            Return result
        End Function

        Public Shared Function getContentDisplay(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select contentDisplayRef from ms_Tag where domainRef =@domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("contentDisplayRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()



            Return result
        End Function

        Public Shared Function getListParentTag(ByVal domainRef As String, ByVal tagRefParent As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  tagRef, tagName, description " + _
                                 "From " + _
                                 "      MS_tag " + _
                                 "Where tagRefParent = @tagRefParent and domainRef = @domainRef order by sortNo asc "


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListParentTagByTagType(ByVal domainRef As String, ByVal tagTypeRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  tagRef,tagRef, tagName " + _
                                 "From " + _
                                 "      MS_tag " + _
                                 "Where tagTypeRef = @tagTypeRef and domainRef = @domainRef and isActive = 1 order by sortNo asc "


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagTypeRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getRandomContent(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean, ByVal top As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            Dim queryTop As String = ""
            If Trim(top) <> "" Then
                queryTop = " top " + top.ToString() + " "
            End If

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  " + queryTop + " ct.contentRef,ct.synopsis,ct.title " + _
                                 "From " + _
                                 "      TR_content ct " + _
                                 "      INNER JOIN TR_contentTag cg on cg.contentRef = ct.contentRef and cg.domainRef = ct.domainRef  " + _
                                 "Where cg.tagRef  = @tagRef and cg.domainRef = @domainRef " + _
                                 queryPublishDate + queryExpiredDate + _
                                 "order by NEWID() "


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagContentDisplay(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "SELECT contentDisplayRef, isSingleContent FROM MS_tag  " + _
                                "WHERE domainRef = @domainRef and tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagList() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select tagRef, tagName from MS_tag" + _
                                 " where domainRef = @domainRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagListByTagRefParent(ByVal tagRefParent As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select tagRef, tagName from MS_tag" + _
                                 " where domainRef = @domainRef and tagRefParent = @tagRefParent "

            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentList() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select contentRef, title from TR_Content" + _
                                 " where domainRef = @domainRef and title <> '' "

            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentListWebsite(ByVal contentRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select contentRef, title from TR_Content" + _
                                 " where domainRef = @domainRef and contentRef = @contentRef and title <> '' "

            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getProductList(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
            'Dim sqlCon As New SqlConnection(_naproConStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim queryClusterRef As String = ""
            If clusterRef <> "" And clusterRef <> "0" Then
                queryClusterRef = " and clusterRef = @clusterRef "
            End If

            ' sqlCon.Open()
            'sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select productRef, titleProduct from MS_dbMasterProjectProduct " + _
                                 " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isActive = 1 " + _
                                 queryClusterRef

            sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
            sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
            If clusterRef <> "" And clusterRef <> "0" Then
                sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
            End If

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            '   sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getCategoryList(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
            ' Dim sqlCon As New SqlConnection(_naproConStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            ' sqlCon.Open()
            ' sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select * from MS_dbMasterProjectCategory " + _
                                 " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

            sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
            sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            '   sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getClusterList(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String) As DataTable
            ' Dim sqlCon As New SqlConnection(_naproConStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            '  sqlCon.Open()
            ' sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select clusterRef, clusterDescription, facilities from MS_dbMasterProjectCluster " + _
                                 " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and categoryRef = @categoryRef "

            sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
            sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
            sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            ' sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContent(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean, Optional ByVal top As String = "", Optional ByVal alphabeticSort As String = "") As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim queryTop As String = ""
            Dim queryAlphabeticSort As String = ""
            Dim queryPublishDate As String = ""
            Dim queryExpiredDate As String = ""
            Dim queryOrderBy As String = ""


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

            'If tagref = _tagRefBackgroundSlider Or tagRef = _tagRefVisiMisi Then
            '    queryOrderBy = "order by  ct.contentRef asc "
            'Else
            '    queryOrderBy = "order by  ct.InputTime desc "
            'End If
            
            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  " + queryTop + "ct.contentRef,ct.synopsis,ct.title,ct.titleDetail, ct.content, ct.embedVideo, ct.publishDate, cg.tagRef " + _
                                 "From " + _
                                 "      TR_content ct " + _
                                 "      INNER JOIN TR_contentTag cg on cg.contentRef = ct.contentRef and cg.domainRef = ct.domainRef  " + _
                                 "Where cg.tagRef  = @tagRef and cg.domainRef = @domainRef " + _
                                 queryPublishDate + queryExpiredDate + queryAlphabeticSort.ToString + _
                                 queryOrderBy

            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt

        End Function

        Public Shared Function getTabImage(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr
            
            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            

            sqlCmd.CommandText = "select * "	+ _
                              "from       ms_tag " + _
                              "where	    domainRef = @domainRef and tagRef = @tagRef " 

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfoByTagTOP2(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
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



            sqlCmd.CommandText = "select	TOP 2 ct.tagRef, cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName " + _
                              "           , cn.title, cn.synopsis, cn.content, cn.titleDetail " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo, cn.metaDescription " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef and ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef and ct.tagRef = @tagRef " + _
                              queryPublishDate + queryExpiredDate + _
                              " order by   cn.inputTime DESC "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfoByTag(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            'Dim queryPublishDate As String = ""
            'If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
            '    queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
            'End If

            'Dim queryExpiredDate As String = ""
            'If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
            '    queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
            'End If

            'Dim queryOrder As String = ""
            'If tagRef = _tagRefArtikel Then
            '    queryOrder = " order by cn.publishDate DESC"
            'ElseIf tagRef = _tagRefMediaRelease Then
            '    queryOrder = " order by cn.publishDate DESC"
            'Elseif tagRef = _tagRefNews Then
            '    queryOrder = " order by cn.publishDate DESC"
            'End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select	ct.tagRef, cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName, tg.tagPicture " + _
                              "           , cn.title, cn.synopsis, cn.content, cn.titleDetail " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo, cn.metaDescription " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef and ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef and ct.tagRef = @tagRef " + _
                              "order by cn.sortNo asc"
            'queryPublishDate + queryExpiredDate + queryOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfoByTagHubungiDscImage(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
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

            'Dim queryOrder As String = ""
            'If tagRef = _tagRefArtikel Then
            '    queryOrder = " order by cn.publishDate DESC"
            'ElseIf tagRef = _tagRefMediaRelease Then
            '    queryOrder = " order by cn.publishDate DESC"
            'Elseif tagRef = _tagRefNews Then
            '    queryOrder = " order by cn.publishDate DESC"
            'End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select	cn.geolocation.Lat as latitude, cn.geolocation.Long as longitude, ct.tagRef, cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName, tg.tagPicture " + _
                              "           , cn.title, cn.synopsis, cn.content, cn.titleDetail " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo, cn.metaDescription " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef and ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef and ct.tagRef = @tagRef " + _
                              "order by newid() "

            'queryPublishDate + queryExpiredDate + queryOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function
        Public Shared Function getImageRefByContentDSC(ByVal psRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStrLDS

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim sqlDr As SqlDataReader
            Dim resultPsRef As String = ""

            If psRef.Split(".").Length > 0 Then
                psRef = psRef.Split(".").First.ToString()
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select p.imageProfile as imgFile, 'default.jpg' as imgFileName, p.* " + _
                                                      "From " + _
                                                      "ms_personal p " + _
                                                      "Where psRef = @psRef and imageProfile is not null "

            sqlCmd.Parameters.AddWithValue("@psRef", psRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function
        Public Shared Function getContentInfoByTagHubungiDscName(ByVal domainRef As String, ByVal tagRef As String, ByVal projectRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStrLDS

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
            End If

            'Dim queryOrder As String = ""
            'If tagRef = _tagRefArtikel Then
            '    queryOrder = " order by cn.publishDate DESC"
            'ElseIf tagRef = _tagRefMediaRelease Then
            '    queryOrder = " order by cn.publishDate DESC"
            'Elseif tagRef = _tagRefNews Then
            '    queryOrder = " order by cn.publishDate DESC"
            'End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select top 5 name, '62'+handPhone as handphone, psRef " + _
                              "from vPersonalProject p " + _
                              "where 1=1 and p.accountType IN (1, 2)" + _
                              "and  startLeadDistribution is not null and startLeadDistribution <= getdate() " + _
                              "and isActive = 1 and projectRef = @projectRef " + _
                              "order by newid() "

            'queryPublishDate + queryExpiredDate + queryOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfoByTagLvl3(ByVal domainRef As String, ByVal contentRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, contentRef, isChildSite) Then
                queryPublishDate = " And DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, contentRef, isChildSite) Then
                queryExpiredDate = " And DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
            End If

            Dim queryOrder As String = ""
            'If tagRef = _parentTagRefNewsEvents Then
            '    queryOrder = " order by cn.publishDate DESC"
            'End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select	ct.tagRef, cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName " + _
                              "           , cn.title, cn.synopsis, cn.content, cn.titleDetail " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo, cn.metaDescription, " + _
                              "           [geoLocation].[Lat] as [latitude],[geoLocation].[Long] as [longitude], cn.geoLocation   " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef And ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef And cn.contentRef = @contentRef " + _
                              queryPublishDate + queryExpiredDate

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfoByTagLvl3TheTeam(ByVal domainRef As String, ByVal contentRef As String, ByVal tagRefParent As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, contentRef, isChildSite) Then
                queryPublishDate = " And DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, contentRef, isChildSite) Then
                queryExpiredDate = " And DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
            End If

            Dim queryOrder As String = ""
            'If tagRef = _parentTagRefNewsEvents Then
            '    queryOrder = " order by cn.publishDate DESC"
            'End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select	ct.tagRef, cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName " + _
                              "           , cn.title, cn.synopsis, cn.content, cn.titleDetail " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo, cn.metaDescription " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef And ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef And cn.contentRef = @contentRef And tg.tagRefParent = @tagRefParent " + _
                              queryPublishDate + queryExpiredDate

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentRefByTag(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " And DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " And DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
            End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition " + _
                                "           , cn.title, cn.synopsis, cn.content " + _
                                "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate " + _
                                "from       tr_content cn " + _
                                "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef And ct.contentRef = cn.contentRef " + _
                                "where	    ct.domainRef = @domainRef And ct.tagRef = @tagRef " + _
                                queryPublishDate + queryExpiredDate

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("contentRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getEvent(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean, ByVal contentDate As SqlTypes.SqlDateTime) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim queryTop As String = ""
            Dim queryPublishDate As String = ""
            Dim queryExpiredDate As String = ""


            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " And DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " And DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select ct.contentRef,ct.synopsis,ct.title " + _
                                 "From " + _
                                 "      TR_content ct " + _
                                 "      INNER JOIN TR_contentTag cg on cg.contentRef = ct.contentRef And cg.domainRef = ct.domainRef  " + _
                                 "Where cg.tagRef  = @tagRef And cg.domainRef = @domainRef And ct.contentDate Is Not null And DATEDIFF(DAY,@contentDate,ct.contentDate)=0 " + _
                                 queryPublishDate + queryExpiredDate + _
                                 "order by ct.contentDate desc "

            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentDate", contentDate)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt

        End Function

        Public Shared Function getTotalEvent(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean, ByVal contentDate As SqlTypes.SqlDateTime) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim result As Boolean = False
            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim queryPublishDate As String = ""
            Dim queryExpiredDate As String = ""


            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " And DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " And DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select count(*) as total " + _
                                 "From " + _
                                 "      TR_content ct " + _
                                 "      INNER JOIN TR_contentTag cg on cg.contentRef = ct.contentRef And cg.domainRef = ct.domainRef  " + _
                                 "Where cg.tagRef  = @tagRef And cg.domainRef = @domainRef And ct.contentDate Is Not null And ct.contentDate = @contentDate " + _
                                 queryPublishDate + queryExpiredDate

            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentDate", contentDate)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                If sqlDr("total") > 0 Then
                    result = True
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getContentInfoByRef(ByVal domainRef As String, ByVal contentRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	contentRef, contentType, imgSetting, imgPosition " + _
                                "           , title, titleDetail, synopsis, content " + _
                                "           , contentDate, publishDate, expiredDate, approvedDate, embedVideo " + _
                                "from       tr_content " + _
                                "where	    domainRef = @domainRef And contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagInfoByContentRef(ByVal domainRef As String, ByVal contentRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	cg.domainRef, cg.contentRef,cg.tagRef, tg.tagRefParent,tg.contentDisplayRef,tg.tagName,tg.description " + _
                                "from       TR_contentTag cg " + _
                                "           INNER JOIN MS_tag tg on tg.tagRef = cg.tagRef And tg.domainRef = cg.domainRef " + _
                                "where	    cg.contentRef = @contentRef And cg.domainRef = @domainRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function updateDomainContentHit(ByVal domainRef As String, ByVal contentRef As String, ByVal isChildSite As Boolean) As String
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "UPDATE	TR_content " + _
                                            "SET hit = hit + 1 " + _
                                            "WHERE domainRef = @domainRef And contentRef = @contentRef "


                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

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


        Public Shared Function getDomainImage(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  imgFile,imgFileName " + _
                                 "From " + _
                                 "      MS_domainSetting " + _
                                 "Where domainRef=@domainRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainName(ByVal domainRef As String) As String
            Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlnaproCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlnaproCon.Open()
            sqlCmd.Connection = sqlnaproCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select domainName from MS_domain " + _
                                 "where domainRef = @domainRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("domainName")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlnaproCon.Close()

            Return result
        End Function

        Public Shared Function getDomainLogo(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select  logo as imgFile,logoFileName as imgFileName " + _
                                 "From " + _
                                 "      MS_domainSetting " + _
                                 "Where domainRef=@domainRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function


        Public Shared Function getSubTag(ByVal tagRefParent As String) As DataTable
            Dim connectionString As String = ""


            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select tagRef, tagName " + _
                                "from MS_tag  " + _
                                "where	tagRefParent = @tagRefParent"

            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getSubTagByClusterName(ByVal tagRefParent As String, ByVal clusterName As String) As DataTable
            Dim connectionString As String = ""


            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select tagRef, tagName " + _
                                "from MS_tag  " + _
                                "where	tagRefParent = @tagRefParent And tagName = @clusterName "

            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)
            sqlCmd.Parameters.AddWithValue("@clusterName", clusterName)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTag(ByVal tagRef As String) As DataTable
            Dim connectionString As String = ""


            connectionString = _conStr


            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select tagName, description " + _
                                "from MS_tag  " + _
                                "where	tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function


        Public Shared Function getTagTreeList(ByVal tagTypeRef As String, _
                                              ByVal tagRefParent As String, ByVal isActive As String) As DataTable
            Dim dt As New DataTable

            dt.Columns.Add("domainRef")
            dt.Columns.Add("tagRef")
            dt.Columns.Add("tagRefParent")
            dt.Columns.Add("tagTypeRef")
            dt.Columns.Add("tagTypeName")
            dt.Columns.Add("tagName")
            dt.Columns.Add("isActive")
            dt.Columns.Add("level")

            rekTagChild(dt, tagTypeRef, tagRefParent, isActive, 0)

            Return dt
        End Function

        Public Shared Sub rekTagChild(ByRef dtTree As DataTable, _
                                      ByVal tagTypeRef As String, _
                                      ByVal tagRefParent As String, ByVal isActive As String, _
                                      ByVal level As Integer)
            Dim dt As New DataTable
            Dim drTree As DataRow

            dt = getTagList(tagTypeRef, tagRefParent, isActive)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    drTree = dtTree.NewRow
                    drTree("domainRef") = dt.Rows(i).Item("domainRef")
                    drTree("tagRef") = dt.Rows(i).Item("tagRef")
                    drTree("tagRefParent") = dt.Rows(i).Item("tagRefParent")
                    drTree("tagTypeRef") = dt.Rows(i).Item("tagTypeRef")
                    drTree("tagTypeName") = dt.Rows(i).Item("tagTypeName")
                    drTree("tagName") = dt.Rows(i).Item("tagName")
                    drTree("isActive") = dt.Rows(i).Item("isActive")
                    drTree("level") = level + 1
                    dtTree.Rows.Add(drTree)

                    rekTagChild(dtTree, tagTypeRef, dt.Rows(i).Item("tagRef").ToString, isActive, level + 1)
                Next
            End If
        End Sub

        Public Shared Function getTagList(ByVal tagTypeRef As String, ByVal tagRefParent As String, _
                                          ByVal isActive As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim queryTagType As String = ""
            Dim tagType() As String = Split(tagTypeRef, ",")

            If Trim(tagTypeRef) <> "" And Trim(tagTypeRef) <> "All" Then
                queryTagType = " And ( "
                For i = 0 To tagType.Length - 1
                    queryTagType += " t.tagTypeRef = '" + tagType(i) + "' "
                    If i < tagType.Length - 1 Then queryTagType += " OR "
                Next
                queryTagType += " ) "
            End If

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	t.domainRef, tagRef, tagRefParent, t.tagTypeRef, tt.tagTypeName, tagName, isActive " + _
                                "from	    ms_tag t, ms_tagType tt " + _
                                "where	    t.domainRef = tt.domainRef and t.tagTypeRef = tt.tagTypeRef " + _
                                "           and t.domainRef = @domainRef " + _
                                queryTagType + _
                                "           and t.tagRefParent = @tagRefParent " + _
                                "           and (isActive = @isActive or 'All' = @isActive) " + _
                                "order  by t.sortNo "

            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            If Trim(tagTypeRef) <> "" And Trim(tagTypeRef) <> "All" Then
                sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagTypeRef)
            End If
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)
            sqlCmd.Parameters.AddWithValue("@isActive", isActive)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContactUsData(ByVal feedbackRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select * " + _
                                 "from TR_feedback " + _
                                 "where feedbackRef = @feedbackRef "



            sqlCmd.Parameters.AddWithValue("@feedbackRef", feedbackRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getImageRefByContent(ByVal contentRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select ci.imgRef " + _
                                                      "From " + _
                                                      "           TR_contentTag cg " + _
                                                      "           INNER JOIN TR_Content ct on ct.contentRef = cg.contentRef and ct.domainRef = cg.domainRef " + _
                                                      "           INNER JOIN TR_contentImage ci on ci.contentRef = cg.contentRef and ci.contentRef = cg.contentRef and ci.domainRef = cg.domainRef and ci.domainRef = ct.domainRef " + _
                                                      "Where ct.contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getImageRefByContentBannerERP360(ByVal contentRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStrNC

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select ci.imgRef " + _
                                                      "From " + _
                                                      "           TR_contentTag cg " + _
                                                      "           INNER JOIN TR_Content ct on ct.contentRef = cg.contentRef and ct.domainRef = cg.domainRef " + _
                                                      "           INNER JOIN TR_contentImage ci on ci.contentRef = cg.contentRef and ci.contentRef = cg.contentRef and ci.domainRef = cg.domainRef and ci.domainRef = ct.domainRef " + _
                                                      "Where ct.contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getLatestNewsList(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select	top 3 cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName " + _
                              "           , cn.title, cn.titleDetail, cn.synopsis, cn.content " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef and ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef and ct.tagRef = @tagRef " + _
                              "order by cn.inputTime desc "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentInfoMapByTag(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
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
                                "           ,  cn.embedVideo, [geoLocation].[Lat] as [latitude],[geoLocation].[Long] as [longitude], cn.title, cn.synopsis, cn.content " + _
                                "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.geoLocation " + _
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

        ' Public Shared Function getProjectSettingNaPro(ByVal projectCode As String) As DataTable
        '     Dim sqlNaproCon As New SqlConnection(_naproConStr)

        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text

        '     sqlCmd.CommandText = "Select dbMasterRef, projectRef " + _
        '                          "From " + _
        '                          "      SYS_dbMasterProjectSetting " + _
        '                          "Where settingValue = @projectCode "

        '     sqlCmd.Parameters.AddWithValue("@projectCode", projectCode)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt

        ' End Function

        ' Public Shared Function getFileDataCover(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = "select fileData As imgFile,fileName +'.'+ extension As imgFileName, dbMasterRef, projectRef " + _
        '                         " from MS_dbMasterProjectFile " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef != @projectRef and isHeaderImage = 1 "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt
        'End Function

        ' Public Shared Function ProjectSettingNaPro(ByVal projectCode As String) As DataTable
        '     Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable


        '     sqlNaProCon.Open()
        '     sqlCmd.Connection = sqlNaProCon

        '     sqlCmd.CommandText = CommandType.Text

        '     sqlCmd.CommandText = "Select dbMasterRef, projectRef " + _
        '                       "From" + _
        '                       " SYS_dbMasterProjectSetting " + _
        '                       "Where settingValue = @projectCode"
        '     sqlCmd.Parameters.AddWithValue("@projectCode", projectCode)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaProCon.Close()

        '     Return dt

        ' End Function

        ' Public Shared Function getProjectDescription(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '               Dim connectionString As String = ""

        '               connectionString = _naproConStr

        '               Dim sqlNaProCon As New SqlConnection(connectionString)
        '               Dim sqlCmd As New SqlCommand
        '               Dim dt As New DataTable

        '               sqlNaProCon.Open()
        '               sqlCmd.Connection = sqlNaProCon
        '               sqlCmd.CommandType = CommandType.Text

        '               sqlCmd.CommandText = "select projectDescription, projectName, projectSynopsis From " + _
        '                       "MS_dbMasterProject " + _
        '                       "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef"
        '               sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '               sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '               Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '               sqlDa.Fill(dt)

        '               sqlCmd = Nothing
        '               sqlNaProCon.Close()

        '               Return dt

        '       End Function

        ' Public Shared Function getProjectFile(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = "select fileData As imgFile,fileName +'.'+ extension As imgFileName, dbMasterRef, projectRef " + _
        '                         " from MS_dbMasterProjectFile " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef <> @projectRef and isHeaderImage = 1 "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt
        'End Function

        ' Public Shared Function getFileDataCoverDisplay(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = " select dbMasterRef,projectRef,fileData As imgFile,fileName +'.'+ extension As imgFileName " + _
        '                          " from MS_dbMasterProjectFile " + _
        '                          " where dbMasterRef = @dbMasterRef and projectRef = @projectRef And isHeaderImage =1 "


        '     '"Select fileData As imgFile,fileName +'.'+ extension As imgFileName " + _
        '     '                      " from MS_dbMasterProjectFile " + _
        '     '                      " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isHeaderImage = @ishead  "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt
        ' End Function

        ' Public Shared Function getProjectNameNapro(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '     Dim result As String = ""
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlnaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim sqlDr As SqlDataReader
        '     Dim dt As New DataTable

        '     sqlnaproCon.Open()
        '     sqlCmd.Connection = sqlnaproCon
        '     sqlCmd.CommandType = CommandType.Text

        '     sqlCmd.CommandText = " select projectName " + _
        '                            " from MS_dbMasterProject " + _
        '                          " where dbMasterRef= @dbMasterRef and projectRef=@projectRef "

        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)


        '     sqlDr = sqlCmd.ExecuteReader
        '     If sqlDr.Read Then
        '         result = sqlDr("projectName")
        '     End If
        '     sqlDr.Close()

        '     sqlCmd = Nothing
        '     sqlnaproCon.Close()

        '     Return result
        ' End Function

        ' Public Shared Function getContentHeadOffice(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = "select dbMasterRef, projectRef, hoAddress, hoProvinceName, hoPostCode, hoCountryName, hoPhone, hoEmail, projectSynopsis " + _
        '                         " from vDbMasterProject " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt
        'End Function

        ' Public Shared Function getCategoryAnotherProject(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable


        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = "select distinct a.projectCategoryType  ,b.projectCategoryTypeName " + _
        '                          "from MS_dbMasterProjectCategory a   inner join  LK_projectCategoryType b on a.projectCategoryType = b.projectCategoryType " + _
        '                          "where dbMasterRef  = @dbMasterRef and projectRef != @projectRef "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt

        ' End Function

        ' Public Shared Function getTitleBarCover(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '               Dim connectionString As String = ""

        '               connectionString = _naproConStr

        '               Dim sqlNaproCon As New SqlConnection(connectionString)
        '               Dim sqlCmd As New SqlCommand
        '               Dim dt As New DataTable

        '               sqlNaproCon.Open()
        '               sqlCmd.Connection = sqlNaproCon

        '               sqlCmd.CommandType = CommandType.Text
        '               'copyisheaderimage dr database
        '               'nambah imagefile untuk menyamakan database dr img file
        '               sqlCmd.CommandText = "select dbMasterRef, projectRef " + _
        '                                                       " from MS_dbMasterProjectFile " + _
        '                                                       " where dbMasterRef = @dbMasterRef and projectRef != @projectRef and isHeaderImage = 1 "
        '               'copyisheaderimage dr database
        '               sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '               sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '               Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '               sqlDa.Fill(dt)

        '               sqlCmd = Nothing
        '               sqlNaproCon.Close()

        '               Return dt
        '       End Function

        ' Public Shared Function getCategoryProject(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable


        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = "select distinct a.projectCategoryType  ,b.projectCategoryTypeName " + _
        '                          "from MS_dbMasterProjectCategory a   inner join  LK_projectCategoryType b on a.projectCategoryType = b.projectCategoryType " + _
        '                          "where dbMasterRef  = @dbMasterRef and projectRef != @projectRef "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt

        ' End Function

        ' Public Shared Function getProjectCategoryTypeFrAllProject(ByVal dbMasterRef As String) As DataTable


        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     sqlCmd.CommandText = "select distinct a.projectCategoryType  ,b.projectCategoryTypeName " + _
        '                          "from MS_dbMasterProjectCategory a   inner join  LK_projectCategoryType b on a.projectCategoryType = b.projectCategoryType " + _
        '                          "where dbMasterRef  = @dbMasterRef "

        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt

        ' End Function

        ' Public Shared Function getProjectRefByProjectCategoryType(ByVal projectCategoryType As String, ByVal dbMasterRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     Dim queryProjectCategoryType As String = ""
        '     If Trim(projectCategoryType) <> "" And Trim(projectCategoryType) <> "0" Then
        '         queryProjectCategoryType = "and pc.projectCategoryType = @projectCategoryType " 
        '     End If

        '     sqlCon.Open()
        '     sqlCmd.Connection = sqlCon

        '     sqlCmd.CommandType = CommandType.Text

        '     sqlCmd.CommandText = "select distinct TOP 10  pf.projectRef, projectCategoryType, p.projectName " + _
        '                          "from MS_dbMasterProjectFile pf " + _
        '                          "inner join MS_dbMasterProjectCategory pc on pc.dbMasterRef = pf.dbMasterRef and pc.projectRef = pf.projectRef " + _
        '                          "inner join MS_dbMasterProject p on p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                          "where  pf.dbMasterRef = @dbMasterRef " + _
        '                          queryProjectCategoryType + _
        '                          "order by p.projectName asc" 


        '   If Trim(projectCategoryType) <> "" And Trim(projectCategoryType) <> "0" Then                   
        '     sqlCmd.Parameters.AddWithValue("@projectCategoryType", projectCategoryType)
        '   End if
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)


        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlCon.Close()

        '     Return dt
        ' End Function

        ' Public Shared Function getProjectRefByProjectCategoryTypeAll(ByVal projectCategoryType As String, ByVal dbMasterRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     Dim queryProjectCategoryType As String = ""
        '     If Trim(projectCategoryType) <> "" And Trim(projectCategoryType) <> "0" Then
        '         queryProjectCategoryType = "and pc.projectCategoryType = @projectCategoryType " 
        '     End If

        '     sqlCon.Open()
        '     sqlCmd.Connection = sqlCon

        '     sqlCmd.CommandType = CommandType.Text

        '     sqlCmd.CommandText = "select distinct pf.projectRef, p.projectName " + _
        '                          "from MS_dbMasterProjectFile pf " + _
        '                          "inner join MS_dbMasterProjectCategory pc on pc.dbMasterRef = pf.dbMasterRef and pc.projectRef = pf.projectRef " + _
        '                          "inner join MS_dbMasterProject p on p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                          "where  pf.dbMasterRef = @dbMasterRef " + _
        '                          queryProjectCategoryType  + _
        '                          "order by p.projectName asc" 

        '     If Trim(projectCategoryType) <> "" And Trim(projectCategoryType) <> "0" Then                   
        '         sqlCmd.Parameters.AddWithValue("@projectCategoryType", projectCategoryType)
        '     End if
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)


        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlCon.Close()

        '     Return dt
        ' End Function

        ' Public Shared Function getProjectRefByProjectRef(ByVal projectCategoryType As String, ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     Dim queryProjectCategoryType As String = ""
        '     If Trim(projectCategoryType) <> "" And Trim(projectCategoryType) <> "0" Then
        '         queryProjectCategoryType = "and pc.projectCategoryType = @projectCategoryType " 
        '     End If

        '     sqlCon.Open()
        '     sqlCmd.Connection = sqlCon

        '     sqlCmd.CommandType = CommandType.Text

        '     sqlCmd.CommandText = "select distinct pf.projectRef, p.projectName " + _
        '                          "from MS_dbMasterProjectFile pf " + _
        '                          "inner join MS_dbMasterProjectCategory pc on pc.dbMasterRef = pf.dbMasterRef and pc.projectRef = pf.projectRef " + _
        '                          "inner join MS_dbMasterProject p on p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                          "where  pf.dbMasterRef = @dbMasterRef and pf.projectRef <> @projectRef " + _
        '                          queryProjectCategoryType + _
        '                          "order by p.projectName asc" 

        '     If Trim(projectCategoryType) <> "" And Trim(projectCategoryType) <> "0" Then                   
        '         sqlCmd.Parameters.AddWithValue("@projectCategoryType", projectCategoryType)
        '     End if
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)


        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlCon.Close()

        '     Return dt
        ' End Function

        ' Public Shared Function getProjectListNapro(ByVal dbMasterRef As String, ByVal settingRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandText = "select * " + _
        '                         " from SYS_dbMasterProjectSetting " + _
        '                         " where dbMasterRef = @dbMasterRef and settingRef = @settingRef "

        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@settingRef", settingRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt
        'End Function

        ' Public Shared Function getDbMasterProjectFile1(ByVal dbMasterRef As String, ByVal ProjectRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select dbMasterProjectFileRef,fileData as imgFile, fileName as imgFileName" + _
        '                 " From" + _
        '                 " MS_dbMasterProjectFile " + _
        '                 " Where dbMasterRef = @dbMasterRef and ProjectRef = @ProjectRef and isHeaderImage=0 "
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        ' End Function


        ' Public Shared Function getMapsDeveloper(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal isChildSite As Boolean) As DataTable
        '             Dim connectionString As String = ""
        '             connectionString = _naproConStr
        '             Dim queryPublishDate As String = ""
        '             'If isContentEnabledPublishDate(dbMasterRef, projectRef, isChildSite) Then
        '             '    queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),cn.publishDate)<=0 "
        '             'End If
        '             'Dim queryExpiredDate As String = ""
        '             'If isContentEnabledExpiredDate(dbMasterRef, projectRef, isChildSite) Then
        '             '    queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(cn.expiredDate,GETDATE()))>=0 "
        '             'End If
        '             Dim sqlCon As New SqlConnection(connectionString)
        '             Dim sqlCmd As New SqlCommand
        '             Dim dt As New DataTable
        '             sqlCon.Open()
        '             sqlCmd.Connection = sqlCon
        '             sqlCmd.CommandType = CommandType.Text
        '             sqlCmd.CommandText = "select [geoLocation].[Lat] as [latitude],[geoLocation].[Long] as [longitude], geoLocation " + _
        '                                 "from       vDbMasterProject " + _
        '                                 "where	    dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '             'queryPublishDate + queryExpiredDate

        '             sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '             sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '             Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '             sqlDa.Fill(dt)
        '             sqlCmd = Nothing
        '             sqlCon.Close()

        '             Return dt
        '         End Function


        ' Public Shared Function getWebUrl(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '     Dim result As String = ""
        '     Dim sqlCon As New SqlConnection(_naproConStr)
        '     Dim sqlCmd As New SqlCommand
        '     Dim sqlDr As SqlDataReader


        '     Dim sqlnaproCon As New SqlConnection(_naproConStr)
        '     Dim dt As New DataTable

        '     sqlnaproCon.Open()
        '     sqlCmd.Connection = sqlnaproCon
        '     sqlCmd.CommandType = CommandType.Text

        '     sqlCmd.CommandText = " select websiteURL " + _
        '                            " from MS_dbMasterProject " + _
        '                          " where dbMasterRef= @dbMasterRef and projectRef=@projectRef "

        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)


        '     sqlDr = sqlCmd.ExecuteReader
        '     If sqlDr.Read Then
        '         result = sqlDr("websiteURL")
        '     End If
        '     sqlDr.Close()

        '     sqlCmd = Nothing
        '     sqlnaproCon.Close()

        '     Return result
        ' End Function



        ' Public Shared Function getVideoUrl(ByVal dbMasterRef As String, ByVal projectRef As String) As String
        '             Dim result As String = ""
        '             Dim sqlCon As New SqlConnection(_naproConStr)
        '             Dim sqlCmd As New SqlCommand
        '             Dim sqlDr As SqlDataReader


        '             Dim sqlnaproCon As New SqlConnection(_naproConStr)
        '             Dim dt As New DataTable

        '             sqlnaproCon.Open()
        '             sqlCmd.Connection = sqlnaproCon
        '             sqlCmd.CommandType = CommandType.Text

        '             sqlCmd.CommandText = " select TOP 1 urlVideo " + _
        '                                    " from MS_dbMasterProjectFile " + _
        '                                  " where dbMasterRef= @dbMasterRef and projectRef=@projectRef " + _
        '                                  " order by dbMasterProjectFileRef desc"

        '             sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '             sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)


        '             sqlDr = sqlCmd.ExecuteReader
        '             If sqlDr.Read Then
        '                 result = sqlDr("urlVideo")
        '             End If
        '             sqlDr.Close()

        '             sqlCmd = Nothing
        '             sqlnaproCon.Close()

        '             Return result
        '         End Function

        ' Public Shared Function getprojectfilelist(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '             Dim connectionString As String = ""

        '             connectionString = _naproConStr

        '             Dim sqlNaproCon As New SqlConnection(_naproConStr)
        '             Dim sqlCmd As New SqlCommand
        '             Dim dt As New DataTable

        '             sqlNaproCon.Open()
        '             sqlCmd.Connection = sqlNaproCon
        '             sqlCmd.CommandType = CommandType.Text

        '             'copyisheaderimage dr database
        '             'nambah imagefile untuk menyamakan database dr img file
        '             sqlCmd.CommandText = "select fileName, extension, dbMasterProjectFileRef " + _
        '                                 " from MS_dbMasterProjectFile " + _
        '                                 " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isDownload = 1 and isPublic = 1"
        '             'copyisheaderimage dr database
        '             sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '             sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '             Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '             sqlDa.Fill(dt)

        '             sqlCmd = Nothing
        '             sqlNaproCon.Close()

        '             Return dt
        '         End Function

        ' Public Shared Function getDeveloperInfo(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '             Dim connectionString As String = ""

        '             connectionString = _naproConStr

        '             Dim sqlNaproCon As New SqlConnection(connectionString)
        '             Dim sqlCmd As New SqlCommand
        '             Dim dt As New DataTable

        '             sqlNaproCon.Open()
        '             sqlCmd.Connection = sqlNaproCon

        '             sqlCmd.CommandType = CommandType.Text
        '             'copyisheaderimage dr database
        '             'nambah imagefile untuk menyamakan database dr img file

        '             sqlCmd.CommandText = "select v.developerName, v.locationName, v.totalLandArea, v.developerRef, ms.developerDescription " + _
        '                                 " from vDbMasterProject v " + _
        '                                 " inner join MS_developer ms on ms.developerRef = v.developerRef " + _
        '                                 " where v.dbMasterRef = @dbMasterRef and projectRef = @projectRef "
        '             'copyisheaderimage dr database
        '             sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '             sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '             'sqlCmd.Parameters.AddWithValue("@developerRef", developerRef)

        '             Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '             sqlDa.Fill(dt)

        '             sqlCmd = Nothing
        '             sqlNaproCon.Close()

        '             Return dt

        '         End Function

        ' Public Shared Function getContentProjectLocation(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '     Dim connectionString As String = ""

        '     connectionString = _naproConStr

        '     Dim sqlNaproCon As New SqlConnection(connectionString)
        '     Dim sqlCmd As New SqlCommand
        '     Dim dt As New DataTable

        '     sqlNaproCon.Open()
        '     sqlCmd.Connection = sqlNaproCon

        '     sqlCmd.CommandType = CommandType.Text
        '     'copyisheaderimage dr database
        '     'nambah imagefile untuk menyamakan database dr img file
        '     sqlCmd.CommandText = "select dbMasterRef, projectRef, projectAddress, provinceName, projectPostCode, projectCountryName, projectPhone, projectEmail " + _
        '                         " from vDbMasterProject " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "
        '     'copyisheaderimage dr database
        '     sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '     sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '     Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '     sqlDa.Fill(dt)

        '     sqlCmd = Nothing
        '     sqlNaproCon.Close()

        '     Return dt
        'End Function

        ' Public Shared Function getContentMarketingOffice(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '             Dim connectionString As String = ""

        '             connectionString = _naproConStr

        '             Dim sqlNaproCon As New SqlConnection(connectionString)
        '             Dim sqlCmd As New SqlCommand
        '             Dim dt As New DataTable

        '             sqlNaproCon.Open()
        '             sqlCmd.Connection = sqlNaproCon

        '             sqlCmd.CommandType = CommandType.Text
        '             'copyisheaderimage dr database
        '             'nambah imagefile untuk menyamakan database dr img file
        '             sqlCmd.CommandText = "select dbMasterRef, projectRef, marketingAddress, marketingProvinceName, marketingPostCode, marketingCountryName, marketingPhone, marketingEmail " + _
        '                                 " from vDbMasterProject " + _
        '                                 " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "
        '             'copyisheaderimage dr database
        '             sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '             sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '             Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '             sqlDa.Fill(dt)

        '             sqlCmd = Nothing
        '             sqlNaproCon.Close()

        '             Return dt
        '        End Function

        ' Public Shared Function getFileDataCover1(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '                       Dim connectionString As String = ""

        '                       connectionString = _naproConStr

        '                       Dim sqlNaproCon As New SqlConnection(connectionString)
        '                       Dim sqlCmd As New SqlCommand
        '                       Dim dt As New DataTable

        '                       sqlNaproCon.Open()
        '                       sqlCmd.Connection = sqlNaproCon

        '                       sqlCmd.CommandType = CommandType.Text
        '                       'copyisheaderimage dr database
        '                       'nambah imagefile untuk menyamakan database dr img file
        '                       sqlCmd.CommandText = "select fileData As imgFile,fileName +'.'+ extension As imgFileName, dbMasterRef, projectRef " + _
        '                                                               " from MS_dbMasterProjectFile " + _
        '                                                               " where dbMasterRef = @dbMasterRef and ProjectRef != @ProjectRef and isHeaderImage = 1 " + _
        '                                                               " order by inputTime Desc"
        '                       'copyisheaderimage dr database
        '                       sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                       sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '                       Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '                       sqlDa.Fill(dt)

        '                       sqlCmd = Nothing
        '                       sqlNaproCon.Close()

        '                       Return dt
        '               End Function

        ' Public Shared Function getTitleBarCover1(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '                       Dim connectionString As String = ""

        '                       connectionString = _naproConStr

        '                       Dim sqlNaproCon As New SqlConnection(connectionString)
        '                       Dim sqlCmd As New SqlCommand
        '                       Dim dt As New DataTable

        '                       sqlNaproCon.Open()
        '                       sqlCmd.Connection = sqlNaproCon

        '                       sqlCmd.CommandType = CommandType.Text
        '                       'copyisheaderimage dr database
        '                       'nambah imagefile untuk menyamakan database dr img file
        '                       sqlCmd.CommandText = "select dbMasterRef, projectRef " + _
        '                                                               " from MS_dbMasterProjectFile " + _
        '                                                               " where dbMasterRef = @dbMasterRef and ProjectRef = @ProjectRef and isHeaderImage = 1 " + _
        '                                                               " order by inputTime Desc "

        '                       'copyisheaderimage dr database
        '                       sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                       sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '                       Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '                       sqlDa.Fill(dt)

        '                       sqlCmd = Nothing
        '                       sqlNaproCon.Close()

        '                       Return dt
        '               End Function

        Public Shared Function getNewsListByContentRef(ByVal contentRef As String) As DataTable
            Dim result As New DataTable
            Dim sqlCon As New SqlConnection(_constr)
            Dim sqlCmd As New SqlCommand

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select synopsis, title " + _
                                 "From TR_content " + _
                                 "WHERE contentRef = " + contentRef

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(result)
            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        'Public Shared Function getNewsListByContentRefNapro(ByVal contentRef As String) As DataTable
        '    Dim result As New DataTable
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text
        '    sqlCmd.CommandText = "Select synopsis, title " + _
        '                         "From TR_content " + _
        '                         "WHERE contentRef = " + contentRef

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(result)
        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getNewsEventNapro(ByVal dbMasterRef As String, ByVal tagRef As String, ByVal contentRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable
        '    Dim queryContentRef As String = ""
        '    If contentRef <> "" Then
        '        queryContentRef = "and c.contentRef = @contentRef "
        '    End If

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT ct.tagRef, c.* " + _
        '                         "FROM TR_content c " + _
        '                         "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                         "WHERE c.dbMasterRef = @dbMasterRef and c.contentPrivacyType = 0 " + queryContentRef + " " + _
        '                         "order by c.publishDate desc"

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
        '    If contentRef <> "" Then
        '       sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    End If

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getNewsArtikelEventNapro(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal tagRef As String, ByVal contentRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable
        '    Dim queryContentRef As String = ""
        '    If contentRef <> "" Then
        '        queryContentRef = "and c.contentRef = @contentRef "
        '    End If

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT ct.tagRef, c.* " + _
        '                         "FROM TR_content c " + _
        '                         "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                         "WHERE c.dbMasterRef = @dbMasterRef and c.projectRef = @projectRef and c.contentPrivacyType = 0 AND (ct.tagRef = @tagRef) " + queryContentRef + _
        '                         "order by c.publishDate desc"

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
        '    If contentRef <> "" Then
        '        sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    End If

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getImageRefByContentNapro(ByVal contentRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select imgRef " + _
        '                         "From " + _
        '                         "TR_contentImage " + _
        '                         "Where contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getURLNapro(ByVal contentRef As String) As String

        '    Dim result As String = ""

        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT url " + _
        '                            "from TR_content " + _
        '                            "where contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("url")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()


        '    Return result
        'End Function

        'Public Shared Function isContentHaveImageLatestNewsNapro(ByVal contentRef As String, ByVal isMicroSite As Boolean) As Boolean
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text
        '    sqlCmd.CommandText = "SELECT COUNT(contentRef) AS TOTAL FROM TR_contentImage " + _
        '                         "WHERE contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    If isMicroSite = False Then
        '        If dt.Rows(0).Item("TOTAL") > 0 Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    Else
        '        If dt.Rows(0).Item("TOTAL") = 1 Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    End If

        'End Function

        Public Shared Function isContentHaveImageLatest(ByVal contentRef As String, ByVal isMicroSite As Boolean) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "SELECT COUNT(contentRef) AS TOTAL FROM TR_contentImage " + _
                                 "WHERE contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            If isMicroSite = False Then
                If dt.Rows(0).Item("TOTAL") > 1 Then
                    Return True
                Else
                    Return False
                End If
            Else
                If dt.Rows(0).Item("TOTAL") = 1 Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        'Public Shared Function isUnitHave360(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String, ByVal productRef As String, ByVal isMicroSite As Boolean) As Boolean
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text
        '    sqlCmd.CommandText = "SELECT COUNT(dbMasterProjectProductThreesixtyRef) AS TOTAL FROM MS_dbMasterProjectProductThreesixty " + _
        '                         "WHERE dbMasterRef = @dbMasterRef and projectRef = @projectRef and clusterRef = @clusterRef and productRef = @productRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '    sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
        '    sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    If isMicroSite = False Then
        '        If dt.Rows(0).Item("TOTAL") > 0 Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    End If

        'End Function

        'Public Shared Function getContentImageProjectNaproByImgRef(ByVal imgRef As String, ByVal contentRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select imgRef, imgFile, title, description, imgFileName " + _
        '                         "From TR_contentImage " + _
        '                         "Where imgRef = @imgRef and contentRef = @contentRef and urlVideo = '' "

        '    sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)
        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        Public Shared Function getTitleByContentRef(ByVal contentRef As String) As String
             Dim result As String = ""
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlnaproCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As New DataTable

            sqlnaproCon.Open()
            sqlCmd.Connection = sqlnaproCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select title from TR_content " + _
                                 "where contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)


            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("title")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlnaproCon.Close()

            Return result
        End Function

        Public Shared Function getSynopsisByContentRef(ByVal contentRef As String) As String
            Dim result As String = String.Empty

            Dim sqlCon As New SqlConnection(_constr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "SELECT synopsis " + _
                                  "from TR_content  " + _
                                  "where  contentRef = @contentRef "
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlDr = sqlCmd.ExecuteReader()
            If sqlDr.Read() Then
                result = sqlDr("synopsis")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        'Public Shared Function getSynopsisByContentRefByNapro(ByVal dbMasterRef As String, ByVal tagRefNews As String, ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim queryContentRef As String = ""
        '    If contentRef <> "" Then
        '        queryContentRef = "and c.contentRef = @contentRef "
        '    End If

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT synopsis " + _
        '                         "FROM TR_content c " + _
        '                         "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                         "WHERE c.dbMasterRef = @dbMasterRef AND (ct.tagRef = @tagRefNews) " + queryContentRef 

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRefNews", tagRefNews)
        '    If contentRef <> "" Then
        '        sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    End If
        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("synopsis")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

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

        'Public Shared Function getNewsEventNaproByDbMasterRef(ByVal dbMasterRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT distinct(ct.tagRef) " + _
        '                            "FROM TR_content c " + _
        '                            "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                            "WHERE c.dbMasterRef = @dbMasterRef AND c.publishDate IS NOT NULL " + _
        '                            "order by ct.tagRef asc "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getNewsEventNaproByDbMasterRefTOP1(ByVal dbMasterRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT TOP 1 ct.tagRef " + _
        '                            "FROM TR_content c " + _
        '                            "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                            "WHERE c.dbMasterRef = @dbMasterRef AND c.publishDate IS NOT NULL " + _
        '                            "order by ct.tagRef asc "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getTagNameNaproByTagRef(ByVal tagRef As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "select tagName from MS_contentTag " + _
        '                         "where tagRef = @tagRef "

        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("tagName")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getTitleNaproByContentRef(ByVal contentRef As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "select title from TR_content " + _
        '                         "where contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("title")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getContentListNapro(ByVal tagRef As String) As DataTable
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    'sqlCmd.CommandText = "Select contentRef, title from TR_Content" 

        '    sqlCmd.CommandText = "SELECT ct.tagRef, c.* " + _
        '                         "FROM TR_content c " + _
        '                         "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                         "WHERE ct.tagRef = @tagRef " + _
        '                         "order by c.publishDate desc"

        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getTagListNapro() As DataTable
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select tagRef, tagName from MS_contentTag" 

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function GetContentDescriptions(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT description " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "
        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("description")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        ' Public Shared Function getTags(ByVal contentRef As String) As DataTable
        '    Dim result As New DataTable
        '    Dim sqlCon As New SqlConnection(_naproConstr)
        '    Dim sqlCmd As New SqlCommand

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select k.* FROM TR_contentKeyword k " + _
        '                         "INNER JOIN TR_content c On k.contentRef = c.contentRef " + _
        '                         "where k.contentRef = @contentRef and k.keywordText <> '' "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(result)
        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getClusterDescription(ByVal dbMasterRef As String, ByVal projectRef As String) As String
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

        '    sqlCmd.CommandText = " select clusterDescription,dbMasterRef,projectRef,clusterRef " + _
        '                           " from MS_dbMasterProjectCluster " + _
        '                         " where dbMasterRef= @dbMasterRef and projectRef=@projectRef   "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '    'sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)





        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("clusterDescription")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

        Public Shared Function getTagNameByTagRef(ByVal tagRef As String) As String
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

            sqlCmd.CommandText = "select tagName from MS_tag " + _
                                 "where tagRef = @tagRef "

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

       

        Public Shared Function getListProject(ByVal senderRef As String) As DataTable
            Dim connectionString As String = ""
            connectionString = _conStrLDS

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select mp.projectRef " + _
                                 "From " + _
                                 "      MS_areaSiteProject sp " + _
                                 "inner join LK_WAAutoSender s on s.senderRef = sp.senderRef " + _
                                 "INNER JOIN MS_Project mp on mp.projectRef = sp.projectRef " + _
                                 "INNER JOIN MS_subLocation sl on sl.subLocationRef = mp.subLocationRef " + _
                                 "WHERE mp.isLive = 1 and sp.senderRef = @senderRef " + _
                                 "group by mp.projectRef "

            sqlCmd.Parameters.AddWithValue("@senderRef", senderRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function
        Public Shared Function getListWebMirrorProject(ByVal projectRefList As String) As DataTable
            Dim connectionString As String = ""
            connectionString = _conStrNC

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select webName, webLink, projectRef, 1 as sortNo " + _
                                 "From " + _
                                 "      MS_websiteMirror " + _
                                 "Where projectRef in (" + projectRefList + ") and projectRef <> 0 and isActive = 1 and webName LIKE '%www%' " + _
                                 "union " + _
                                 "Select webName, webLink, projectRef, 2 as sortNo " + _
                                 "From " + _
                                 "      MS_websiteMirror " + _
                                 "Where projectRef in (" + projectRefList + ") and projectRef <> 0 and isActive = 1 and webName NOT LIKE '%www%' " + _
                                 "order by sortNo asc "

            sqlCmd.Parameters.AddWithValue("@projectRefList", projectRefList)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

#Region "contentImage"
        Public Shared Function getContentImageList(ByVal contentRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select * " + _
                                 "from TR_contentImage " + _
                                 "where domainRef = @domainRef and contentRef = @contentRef " + _
                                 "order by imgType desc"



            sqlCmd.Parameters.AddWithValue("@domainRef", _domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        '     Public Shared Function getCategoryDescription(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String) As String
        '         Dim result As String = ""
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlnaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim sqlDr As SqlDataReader
        '         Dim dt As New DataTable

        '         sqlnaproCon.Open()
        '         sqlCmd.Connection = sqlnaproCon
        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select categoryDescription,dbMasterRef,projectRef,categoryRef " + _
        '                                " from MS_dbMasterProjectCategory " + _
        '                              " where dbMasterRef= @dbMasterRef and projectRef=@projectRef and categoryRef = @categoryRef  "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)





        '         sqlDr = sqlCmd.ExecuteReader
        '         If sqlDr.Read Then
        '             result = sqlDr("categoryDescription")
        '         End If
        '         sqlDr.Close()

        '         sqlCmd = Nothing
        '         sqlnaproCon.Close()

        '         Return result
        '     End Function

        '     Public Shared Function getClusterDescription(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String, ByVal clusterRef As String) As String
        '         Dim result As String = ""
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlnaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim sqlDr As SqlDataReader
        '         Dim dt As New DataTable

        '         sqlnaproCon.Open()
        '         sqlCmd.Connection = sqlnaproCon
        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select clusterDescription,dbMasterRef,projectRef,clusterRef " + _
        '                              " from MS_dbMasterProjectCluster " + _
        '                              " where dbMasterRef= @dbMasterRef and projectRef=@projectRef and categoryRef = @categoryRef and clusterRef = @clusterRef " 

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         sqlDr = sqlCmd.ExecuteReader
        '         If sqlDr.Read Then
        '             result = sqlDr("clusterDescription")
        '         End If
        '         sqlDr.Close()

        '         sqlCmd = Nothing
        '         sqlnaproCon.Close()

        '         Return result
        '     End Function

        '     Public Shared Function getClusterDescriptionProject(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As String
        '         Dim result As String = ""
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlnaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim sqlDr As SqlDataReader
        '         Dim dt As New DataTable

        '         sqlnaproCon.Open()
        '         sqlCmd.Connection = sqlnaproCon
        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select clusterDescription,dbMasterRef,projectRef,clusterRef " + _
        '                              " from MS_dbMasterProjectCluster " + _
        '                              " where dbMasterRef= @dbMasterRef and projectRef=@projectRef  and clusterRef = @clusterRef " 

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         sqlDr = sqlCmd.ExecuteReader
        '         If sqlDr.Read Then
        '             result = sqlDr("clusterDescription")
        '         End If
        '         sqlDr.Close()

        '         sqlCmd = Nothing
        '         sqlnaproCon.Close()

        '         Return result
        '     End Function

        '     Public Shared Function getFileContentTower(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '                 Dim sqlNaproCon As New SqlConnection(connectionString)
        '                 Dim sqlCmd As New SqlCommand
        '                 Dim dt As New DataTable

        '                 sqlNaproCon.Open()
        '                 sqlCmd.Connection = sqlNaproCon

        '                 sqlCmd.CommandType = CommandType.Text
        '                 'copyisheaderimage dr database
        '                 'nambah imagefile untuk menyamakan database dr img file
        '                 sqlCmd.CommandText = "select TOP 3 fileData As imgFile, 'tower.jpg' As imgFileName ,dbMasterRef, projectRef, clusterRef, dbMasterProjectClusterFileRef  " + _
        '                                     " from MS_dbMasterProjectClusterFile " + _
        '                                     " where dbMasterRef = @dbMasterRef and projectRef = @projectRef   and fileData is not null " + _
        '                                     " order by sortNo asc"
        '                 'copyisheaderimage dr database
        '                 sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                 sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)




        '                 Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '                 sqlDa.Fill(dt)

        '                 sqlCmd = Nothing
        '                 sqlNaproCon.Close()

        '                 Return dt
        '            End Function

        '     Public Shared Function getFileDataTower(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text
        '         'copyisheaderimage dr database
        '         'nambah imagefile untuk menyamakan database dr img file
        '         sqlCmd.CommandText = "select fileData As imgFile, 'tower.jpg' As imgFileName ,dbMasterRef, projectRef, clusterRef  " + _
        '                             " from MS_dbMasterProjectClusterFile " + _
        '                             " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and clusterRef = @clusterRef and fileData is not null " + _
        '                             " order by sortNo asc"
        '         'copyisheaderimage dr database
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)




        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getContentProduct(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal productRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText =  "Select * " + _
        '                               "From" + _
        '                               " MS_dbMasterProjectProduct " + _
        '                               "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef and productRef = @productRef and isActive = 1 "
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageUnit(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select dbMasterRef, projectRef, fileData As imgFile " + _
        '                           "From" + _
        '                           " MS_dbMasterProjectProductFile " + _
        '                           "Where dbMasterRef = @dbMasterRef and @projectRef = projectRef " + _
        '                           "order by sortNo asc"
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageProduct(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "SELECT pf.dbMasterProjectProductFileRef, pf.dbMasterRef, pf.projectRef, p.clusterRef, pf.productRef, pf.fileData As imgFile " + _
        '                              "FROM MS_dbMasterProjectProductFile pf " + _
        '                              "INNER JOIN MS_dbMasterProjectProduct p on p.productRef = pf.productRef and p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                              "WHERE pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef and pf.isHeaderImage = 1 and p.isActive = 1 " + _
        '                              "order by pf.productRef, pf.sortNo asc "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageProductTOP1(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "SELECT TOP 1 pf.dbMasterProjectProductFileRef, pf.dbMasterRef, pf.projectRef, pf.productRef, pf.fileData As imgFile " + _
        '                              "FROM MS_dbMasterProjectProductFile pf " + _
        '                              "INNER JOIN MS_dbMasterProjectProduct p on p.productRef = pf.productRef and p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                              "WHERE pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef and pf.isHeaderImage = 1 and p.isActive =1 " + _
        '                              "order by pf.productRef, pf.sortNo asc "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageProductExcTOP1(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String, ByVal productRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "SELECT pf.dbMasterProjectProductFileRef, pf.dbMasterRef, pf.projectRef, pf.productRef, pf.fileData As imgFile " + _
        '                              "FROM MS_dbMasterProjectProductFile pf " + _
        '                              "INNER JOIN MS_dbMasterProjectProduct p on p.productRef = pf.productRef and p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                              "WHERE pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef and pf.isHeaderImage = 1 and pf.productRef <> @productRef and p.isActive = 1 " + _
        '                              "order by pf.productRef, pf.sortNo asc"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
        '         sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageBlockPlan(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text
        '         'copyisheaderimage dr database
        '         'nambah imagefile untuk menyamakan database dr img file
        '         ' sqlCmd.CommandText = "SELECT pf.dbMasterProjectProductFileRef, pf.dbMasterRef, pf.projectRef, pf.productRef, pf.fileData As imgFile " + _
        '         '                     "FROM MS_dbMasterProjectProductFile pf " + _
        '         '                     "INNER JOIN MS_dbMasterProjectProduct p on p.productRef = pf.productRef and p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '         '                     "WHERE pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef and pf.isHeaderImage = 1 " + _
        '         '                     "order by pf.productRef asc"

        '         'sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         'sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         'sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '          sqlCmd.CommandText = " select pf.dbMasterRef, pf.projectRef, pf.fileData As imgFile, pf.dbMasterProjectClusterBlockPlanFileRef as imgRef " + _
        '                               " from MS_dbMasterProjectClusterBlockPlanFile pf " + _
        '                               " INNER JOIN MS_dbMasterProjectCluster p on p.clusterRef = pf.clusterRef and p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                               " where pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef "

        '         'sqlCmd.CommandText = " select dbMasterRef,projectRef,fileData As imgFile, dbMasterProjectClusterBlockPlanFileRef as imgRef " + _
        '         '                     " from MS_dbMasterProjectClusterBlockPlanFile " + _
        '         '                     " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "


        '         '"Select fileData As imgFile,fileName +'.'+ extension As imgFileName " + _
        '         '                      " from MS_dbMasterProjectFile " + _
        '         '                      " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isHeaderImage = @ishead  "
        '         'copyisheaderimage dr database
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)


        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getImageProjectGalleryNaproLevel3(ByVal groupGalleryRef As String, ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '                   Dim connectionString As String = ""

        '                   connectionString = _naproConStr

        '                   Dim sqlCon As New SqlConnection(connectionString)
        '                   Dim sqlCmd As New SqlCommand
        '                   Dim dt As New DataTable
        '                   Dim queryGroupGalleryRef As String = ""
        '                   If Trim(groupGalleryRef) <> "" And Trim(groupGalleryRef) <> "0" Then
        '                           queryGroupGalleryRef = "and pg.groupGalleryRef = @groupGalleryRef "
        '                   End If

        '                   sqlCon.Open()
        '                   sqlCmd.Connection = sqlCon

        '                   sqlCmd.CommandType = CommandType.Text

        '                   sqlCmd.CommandText = "SELECT pg.* " + _
        '                                                             "FROM TR_projectGallery pg " + _
        '                                                             "inner join MS_projectGallery ms on ms.groupGalleryRef = pg.groupGalleryRef " + _
        '                                                             "where ms.dbMasterRef = @dbMasterRef and ms.projectRef = @projectRef " + _
        '                                                             queryGroupGalleryRef

        '                   sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                   sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '                   If Trim(groupGalleryRef) <> "" And Trim(groupGalleryRef) <> "0" Then
        '                           sqlCmd.Parameters.AddWithValue("@groupGalleryRef", groupGalleryRef)
        '                   End If

        '                   Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '                   sqlDa.Fill(dt)

        '                   sqlCmd = Nothing
        '                   sqlCon.Close()

        '                   Return dt
        '           End Function

        '     Public Shared Function getFileDataProduct(ByVal dbMasterProjectProductFileRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text
        '         'copyisheaderimage dr database
        '         'nambah imagefile untuk menyamakan database dr img file
        '         sqlCmd.CommandText = " select dbMasterRef,projectRef,fileData As imgFile,productRef, dbMasterProjectProductFileRef as imgRef " + _
        '                              " from MS_dbMasterProjectProductFile " + _
        '                              " where dbMasterProjectProductFileRef = @dbMasterProjectProductFileRef " + _
        '                              " order by sortNo asc"


        '         '"Select fileData As imgFile,fileName +'.'+ extension As imgFileName " + _
        '         '                      " from MS_dbMasterProjectFile " + _
        '         '                      " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isHeaderImage = @ishead  "
        '         'copyisheaderimage dr database
        '         sqlCmd.Parameters.AddWithValue("@dbMasterProjectProductFileRef", dbMasterProjectProductFileRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getTOP1CategoryData(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select TOP 1 * " + _
        '                              " from MS_dbMasterProjectCategory " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getCategoryData(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select * " + _
        '                              " from MS_dbMasterProjectCategory " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getClusterData(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String, ByVal clusterRef As String) As DataTable
        '         Dim connectionString As String = ""
        '         connectionString = _naproConStr

        '         Dim queryCategoryRef As String = ""
        '         If Trim(categoryRef) <> "" And Trim(categoryRef) <> "0" Then
        '             queryCategoryRef = "and categoryRef = @categoryRef "
        '         End If
        '         Dim queryClusterRef As String = ""
        '         If Trim(clusterRef) <> "" And Trim(clusterRef) <> "0" Then
        '             queryClusterRef = "and clusterRef = @clusterRef "
        '         End If

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select * " + _
        '                              " from MS_dbMasterProjectCluster " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef " + _
        '                              queryCategoryRef + queryClusterRef

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         If Trim(categoryRef) <> "" And Trim(categoryRef) <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)
        '         End If
        '         If Trim(clusterRef) <> "" And Trim(clusterRef) <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getClusterDataProject(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim connectionString As String = ""
        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select * " + _
        '                              " from MS_dbMasterProjectCluster " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getCategoryDataByCategoryRef(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select * " + _
        '                              " from MS_dbMasterProjectCategory " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and categoryRef = @categoryRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getClusterDataByClusterRef(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select * " + _
        '                              " from MS_dbMasterProjectCluster " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and clusterRef = @clusterRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getContentSearch(ByVal top As String, ByVal keyword As String, ByVal tagRefNapro As String, ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim result As New DataTable
        '         Dim sqlCon As New SqlConnection(_naproConStr)
        '         Dim sqlCmd As New SqlCommand

        '         ''''' any word method ''''
        '         ''''' any word method ''''
        '         ''''' any word method ''''
        '         Dim field() As String = {"d.developerName", "s.projectName", "c.title"}
        '         Dim whereSearch As New StringBuilder
        '         Dim i, f As Integer

        '         keyword = Replace(keyword, "'", "")
        '         keyword = Replace(keyword, """", "")

        '         If Trim(keyword) <> "" Then
        '             Dim temp() As String = keyword.Split(" ")

        '             whereSearch.Append(" and ( ")
        '             For i = 0 To temp.Length - 1
        '                 whereSearch.Append(" ( ")
        '                 For f = 0 To field.Length - 1
        '                     whereSearch.Append(" " + field(f) + " like '%" + temp(i) + "%' ")
        '                     If f < field.Length - 1 Then
        '                         whereSearch.Append(" or ")
        '                     End If
        '                 Next
        '                 whereSearch.Append(" ) ")

        '                 If i = temp.Length - 1 Then
        '                     whereSearch.Append(" ) ")
        '                 Else
        '                     whereSearch.Append(" or ")
        '                 End If
        '             Next
        '         End If
        '         ''''' any word method end ''''
        '         ''''' any word method end ''''
        '         ''''' any word method end ''''

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon
        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT distinct top " + top + " c.contentRef, c.*, d.developerRef, d.developerName, s.projectName FROM TR_content c " + _
        '                              "  INNER JOIN TR_contentTag ct on ct.contentRef = c.contentRef " + _
        '                              "  LEFT JOIN MS_dbMasterProject s on s.dbMasterRef = c.dbMasterRef and c.projectRef = s.projectRef " + _
        '                              "  LEFT JOIN MS_developer d on d.developerRef = s.developerRef  " + _
        '                              "  WHERE c.contentType = " + _newsContentType + " AND ct.tagRef = " + tagRefNapro + _
        '                              "  AND DATEDIFF(minute,GETDATE(),c.publishDate)<=0 " + _
        '                              "  AND DATEDIFF(DAY,GETDATE(),ISNULL(c.expiredDate,GETDATE()))>=0 " + _
        '                              "  AND c.dbMasterRef = @dbMasterRef and c.projectRef = @projectRef " + _
        '                              whereSearch.ToString + _
        '                              "  ORDER BY c.publishDate DESC"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(result)
        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return result
        '     End Function

        '     Public Shared Function getCountNewsEvent(ByVal contentRef As String) As Integer
        '         Dim result As Integer = 0
        '         Dim sqlCon As New SqlConnection(_naproConStr)
        '         Dim sqlCmd As New SqlCommand
        '         Dim sqlDr As SqlDataReader

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon
        '         sqlCmd.CommandType = CommandType.Text

        '         Dim queryContent As String = ""
        '         If Trim(contentRef) <> "" Then
        '             queryContent = " contentRef = @contentRef And "
        '         End If

        '         sqlCmd.CommandText = "Select count(contentRef) As contentRef  " + _
        '                               "from TR_content  "

        '         If Trim(contentRef) <> "" Then
        '             sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '         End If
        '         sqlDr = sqlCmd.ExecuteReader()
        '         If sqlDr.Read() Then
        '             result = sqlDr("contentRef")
        '         End If
        '         sqlDr.Close()

        '         sqlCmd = Nothing
        '         sqlCon.Close()
        '         Return result
        '     End Function

        '     Public Shared Function getTitle(ByVal contentRef As String) As String
        '         Dim result As String = String.Empty

        '         Dim sqlCon As New SqlConnection(_naproConStr)
        '         Dim sqlCmd As New SqlCommand
        '         Dim sqlDr As SqlDataReader

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon
        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "Select title " + _
        '                               "from tr_content  " + _
        '                               "where contentRef = @contentRef"
        '         sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '         sqlDr = sqlCmd.ExecuteReader()
        '         If sqlDr.Read() Then
        '             result = sqlDr("title")
        '         End If
        '         sqlDr.Close()

        '         sqlCmd = Nothing
        '         sqlCon.Close()


        '         Return result
        '     End Function

        '     Public Shared Function getContentImageListTop(ByVal contentRef As String) As DataTable
        '         Dim result As New DataTable
        '         Dim sqlCon As New SqlConnection(_naproConStr)
        '         Dim sqlCmd As New SqlCommand


        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon
        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "Select top 1 imgRef " + _
        '                              "from tr_contentImage  " + _
        '                              " where contentRef = @contentRef order by sortNo asc "

        '         sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '         sqlDa.Fill(result)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return result
        '     End Function

        '     Public Shared Function getProductData(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal productRef As String, ByVal clusterRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         Dim queryProjectRef As String = ""
        '         Dim queryClusterRef As String = ""
        '         If productRef <> "" And productRef <> "0" Then
        '             queryProjectRef = " and productRef = @productRef "
        '         End If
        '         If clusterRef <> "" And clusterRef <> "0" Then
        '             queryClusterRef = " and clusterRef = @clusterRef "
        '         End If

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select dbMasterRef, projectRef, clusterRef, productRef, titleProduct, productDescription, numOfBathrooms, numOfBedrooms, numOfAdditionalRooms, numOfAdditionalBathrooms, luas " + _
        '                           "From" + _
        '                           " MS_dbMasterProjectProduct " + _
        '                           "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isActive = 1 " + _
        '                           queryProjectRef + queryClusterRef

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         If productRef <> "" And productRef <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@productRef", productRef)
        '         End If
        '         If clusterRef <> "" And clusterRef <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageProductAll(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select dbMasterProjectProductFileRef, dbMasterRef, projectRef, productRef, fileData As imgFile " + _
        '                           "From" + _
        '                           " MS_dbMasterProjectProductFile " + _
        '                           "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef and isHeaderImage = 1 " + _
        '                           " order by productRef, sortNo asc"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageProductByProductRef(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal productRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select dbMasterProjectProductFileRef, dbMasterRef, projectRef, productRef, fileData As imgFile " + _
        '                           "From" + _
        '                           " MS_dbMasterProjectProductFile " + _
        '                           "Where dbMasterRef = @dbMasterRef and projectRef = @productRef and productRef = @projectRef and isHeaderImage = 1" + _
        '                           " order by sortNo asc"
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getNewsEventNapro(ByVal dbMasterRef As String, ByVal tagRefNews As String, ByVal tagRefEvent As String,ByVal contentRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         Dim queryContentRef As String = ""
        '         If contentRef <> "" Then
        '             queryContentRef = "and c.contentRef = @contentRef "
        '         End If

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT ct.tagRef, c.* " + _
        '                              "FROM TR_content c " + _
        '                              "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                              "WHERE c.dbMasterRef = @dbMasterRef AND (ct.tagRef = @tagRefNews or ct.tagRef = @tagRefEvent) and c.publishDate is not null " + queryContentRef + _
        '                              "order by c.publishDate desc"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@tagRefNews", tagRefNews)
        'sqlCmd.Parameters.AddWithValue("@tagRefEvent", tagRefEvent)
        '         If contentRef <> "" Then
        '             sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getImageProductSlider(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal productRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select dbMasterProjectProductFileRef, dbMasterRef, projectRef, productRef, fileData As imgFile " + _
        '                           "From" + _
        '                           " MS_dbMasterProjectProductFile " + _
        '                           "Where dbMasterRef = @dbMasterRef and projectRef = @projectRef and productRef = @productRef and isHeaderImage <> 1" + _
        '                           " order by sortNo asc"
        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     'productName

        '     Public Shared Function getClusterData(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String) As DataTable
        '         Dim connectionString As String = ""
        '         connectionString = _naproConStr

        '         'Dim queryOrder As String = ""
        '         'If categoryRef = _categoryResidence Then
        '         '    queryOrder = " order by clusterRef desc "
        '         'End If

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select * " + _
        '                              " from MS_dbMasterProjectCluster " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and categoryRef = @categoryRef " 

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getProductName(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal productRef As String) As String
        '         Dim result As String = ""
        '         Dim sqlCon As New SqlConnection(_naproConStr)
        '         Dim sqlCmd As New SqlCommand
        '         Dim sqlDr As SqlDataReader

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon
        '         sqlCmd.CommandType = CommandType.Text


        '         sqlCmd.CommandText = "SELECT titleProduct " + _
        '                              "FROM MS_dbMasterProjectProduct " + _
        '                              "WHERE dbMasterRef = @dbMasterRef and projectRef = @projectRef and productRef = @productRef and isActive = 1 "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '         sqlDr = sqlCmd.ExecuteReader()
        '         If sqlDr.Read() Then
        '             result = sqlDr("titleProduct")
        '         End If
        '         sqlDr.Close()

        '         sqlCmd = Nothing
        '         sqlCon.Close()
        '         Return result
        '     End Function

        '     Public Shared Function getNewsArtikelEventNaproTOP3(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal tagRef As String, ByVal contentRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         Dim queryContentRef As String = ""
        '         If contentRef <> "" Then
        '             queryContentRef = "and c.contentRef = @contentRef "
        '         End If

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT TOP 3 ct.tagRef, c.* " + _
        '                              "FROM TR_content c " + _
        '                              "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                              "WHERE c.dbMasterRef = @dbMasterRef and c.projectRef = @projectRef AND c.publishDate IS NOT NULL and c.contentPrivacyType = 0 AND (ct.tagRef = @tagRef) " + queryContentRef + _
        '                              "order by c.publishDate desc"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
        '         If contentRef <> "" Then
        '             sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getNewsEventNaproTOP3(ByVal dbMasterRef As String, ByVal tagRef As String, ByVal contentRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         Dim queryContentRef As String = ""
        '         If contentRef <> "" Then
        '             queryContentRef = "and c.contentRef = @contentRef "
        '         End If

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT TOP 3 ct.tagRef, c.* " + _
        '                              "FROM TR_content c " + _
        '                              "INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
        '                              "WHERE c.dbMasterRef = @dbMasterRef AND c.publishDate IS NOT NULL and c.contentPrivacyType = 0 AND (ct.tagRef = @tagRef) " + queryContentRef + _
        '                              "order by c.publishDate desc"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
        '         If contentRef <> "" Then
        '             sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getImageProjectGalleryNaproDisplay(ByVal groupGalleryRef As String, ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '                   Dim connectionString As String = ""

        '                   connectionString = _naproConStr

        '                   Dim sqlCon As New SqlConnection(connectionString)
        '                   Dim sqlCmd As New SqlCommand
        '                   Dim dt As New DataTable
        '                   Dim queryGroupGalleryRef As String = ""
        '                   If Trim(groupGalleryRef) <> "" And Trim(groupGalleryRef) <> "0" Then
        '                           queryGroupGalleryRef = "and pg.groupGalleryRef = @groupGalleryRef "
        '                   End If

        '                   sqlCon.Open()
        '                   sqlCmd.Connection = sqlCon

        '                   sqlCmd.CommandType = CommandType.Text

        '                   sqlCmd.CommandText = "SELECT pg.* " + _
        '                                                             "FROM TR_projectGallery pg " + _
        '                                                             "inner join MS_projectGallery ms on ms.groupGalleryRef = pg.groupGalleryRef " + _
        '                                                             "where ms.dbMasterRef = @dbMasterRef and ms.projectRef = @projectRef " + _
        '                                                             queryGroupGalleryRef

        '                   sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                   sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '                   If Trim(groupGalleryRef) <> "" And Trim(groupGalleryRef) <> "0" Then
        '                           sqlCmd.Parameters.AddWithValue("@groupGalleryRef", groupGalleryRef)
        '                   End If

        '                   Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '                   sqlDa.Fill(dt)

        '                   sqlCmd = Nothing
        '                   sqlCon.Close()

        '                   Return dt
        '           End Function

        '     Public Shared Function getImageProjectGalleryNaproByImgRef(ByVal imgGalleryRef As String, ByVal groupGalleryRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '                   Dim sqlCon As New SqlConnection(connectionString)
        '                   Dim sqlCmd As New SqlCommand
        '                   Dim dt As New DataTable

        '                   sqlCon.Open()
        '                   sqlCmd.Connection = sqlCon

        '                   sqlCmd.CommandType = CommandType.Text

        '                   sqlCmd.CommandText = "Select imgGalleryRef, groupGalleryRef, imgFile, title, description, fileName as imgFileName " + _
        '                                                             "From TR_projectGallery " + _
        '                                                             "Where imgGalleryRef = @imgGalleryRef and groupGalleryRef = @groupGalleryRef "

        '                   sqlCmd.Parameters.AddWithValue("@imgGalleryRef", imgGalleryRef)
        '                   sqlCmd.Parameters.AddWithValue("@groupGalleryRef", groupGalleryRef)

        '                   Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '                   sqlDa.Fill(dt)

        '                   sqlCmd = Nothing
        '                   sqlCon.Close()

        '                   Return dt
        '           End Function

        '     Public Shared Function getProjectGallery(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable


        '          Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "select groupGalleryName, groupGalleryRef " + _
        '                              "from MS_projectGallery " + _
        '                              "where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)


        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt


        '     End Function

        '     Public Shared Function getTRProjectGallery(ByVal groupGalleryRef As String) As DataTable


        '          Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "select * " + _
        '                              "from TR_projectGallery " + _
        '                              "where groupGalleryRef = @groupGalleryRef " + _
        '                              "order by sortNo asc "

        '         sqlCmd.Parameters.AddWithValue("@groupGalleryRef", groupGalleryRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt


        '     End Function

        '     Public Shared Function getTRProjectGalleryCluster(ByVal imgGalleryRef As String, ByVal clusterRef As String) As DataTable


        '          Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "select * " + _
        '                              "from TR_projectGalleryCluster " + _
        '                              "where imgGalleryRef = @imgGalleryRef and clusterRef = @clusterRef "

        '         sqlCmd.Parameters.AddWithValue("@imgGalleryRef", imgGalleryRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt


        '     End Function

        '     Public Shared Function getImageProjectGalleryproLevel3(ByVal groupGalleryRef As String, ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         Dim queryGroupGalleryRef As String = ""
        '         If Trim(groupGalleryRef) <> "" And Trim(groupGalleryRef) <> "0" Then
        '             queryGroupGalleryRef = "and pg.groupGalleryRef = @groupGalleryRef "
        '         End If

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT ms.groupGalleryRef, pg.* " + _
        '                              "FROM TR_projectGallery pg " + _
        '                              "inner join MS_projectGallery ms on ms.groupGalleryRef = pg.groupGalleryRef " + _
        '                              "where ms.dbMasterRef = @dbMasterRef and ms.projectRef = @projectRef " + _
        '                              queryGroupGalleryRef

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         If Trim(groupGalleryRef) <> "" And Trim(groupGalleryRef) <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@groupGalleryRef", groupGalleryRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getImageRefByCLusterRef(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT pgc.*, pg.groupGalleryRef, pg.title " + _
        '                              "FROM TR_projectGalleryCLuster pgc " + _
        '                              "inner join tr_projectGallery pg on pg.imgGalleryRef = pgc.imgGalleryRef " + _
        '                              "inner join ms_projectGallery mpg on mpg.groupGalleryRef = pg.groupGalleryRef " + _
        '                              "where clusterRef = @clusterRef and mpg.dbMasterRef = @dbMasterRef and mpg.projectRef = @projectRef " + _
        '                              "order by pg.sortNo asc "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getImageRefByCLusterRefProject(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT pgc.*, pg.groupGalleryRef, pg.title " + _
        '                              "FROM TR_projectGalleryCLuster pgc " + _
        '                              "inner join tr_projectGallery pg on pg.imgGalleryRef = pgc.imgGalleryRef " + _
        '                              "inner join ms_projectGallery mpg on mpg.groupGalleryRef = pg.groupGalleryRef " + _
        '                              "where clusterRef = @clusterRef and mpg.dbMasterRef = @dbMasterRef and mpg.projectRef = @projectRef " + _
        '                              "order by pg.sortNo asc "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        '      Public Shared Function getFeaturesList(ByVal dbMasterRef As String, ByVal projectRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlNaproCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = " select ms.projectFeature, lk.projectFeatureName " + _
        '                              " from MS_dbMasterProjectFeature ms " + _
        '                              " inner join LK_projectFeature lk on lk.projectFeature = ms.projectFeature " + _
        '                              " where dbMasterRef = @dbMasterRef and projectRef = @projectRef "

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function getContentAttachmentFileNaPro(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal attachRef As String) As DataTable
        '         Dim sqlNaproCon As New SqlConnection(_naproConStr)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         sqlNaproCon.Open()
        '         sqlCmd.Connection = sqlNaproCon
        '         sqlCmd.CommandType = CommandType.Text
        '         sqlCmd.CommandText = "select	fileName as attachFN, fileData as attachFile, extension " + _
        '                             "from MS_dbMasterProjectFile  " + _
        '                             "where	dbMasterRef = @dbMasterRef and projectRef = @projectRef and dbMasterProjectFileRef = @attachRef"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@attachRef", attachRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         'SqlConnection.ClearPool(sqlCon)
        '         sqlNaproCon.Close()

        '         Return dt
        '     End Function

        '     Public Shared Function GetListImageThreesixty(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String, ByVal productRef As String) As DataTable
        '                   Dim sqlCon As New SqlConnection(_naproConStr)
        '                   Dim sqlCmd As New SqlCommand
        '                   Dim dt As New DataTable

        '                   sqlCon.Open()
        '                   sqlCmd.Connection = sqlCon
        '                   sqlCmd.CommandType = CommandType.Text

        '                   sqlCmd.CommandText =  " Select dbMasterProjectProductThreesixtyRef, dbMasterRef, projectRef, clusterRef, productRef " + _
        '                                         " , title, description, isFirstScene, hfov, pitch, yaw, type, sortNo, inputTime, inputUN " + _
        '                                         " FROM MS_dbMasterProjectProductThreesixty " + _
        '                                         " WHERE dbMasterRef = @dbMasterRef and projectRef = @projectRef and clusterRef = @clusterRef and productRef = @productRef order by isFirstScene desc "


        '                   sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                   sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '                   sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
        '                   sqlCmd.Parameters.AddWithValue("@productRef", productRef)
        '                   Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '                   sqlDa.Fill(dt)

        '                   sqlCmd = Nothing
        '                   sqlCon.Close()

        '                   Return dt
        '           End Function

        '     Public Shared Function GetListLinkImageThreesixty(ByVal threesixtyRef As String, ByVal parent As String) As DataTable
        '                   Dim sqlCon As New SqlConnection(_naproConStr)
        '                   Dim sqlCmd As New SqlCommand
        '                   Dim dt As New DataTable

        '                   Dim queryParent As String = ""
        '                   Dim queryChild As String = ""
        '                   If Trim(parent) <> "" Then
        '                           queryParent = " and threesixtyLinkParentRef = @threesixtyLinkParentRef "
        '                   Else
        '                           queryChild = " and threesixtyLinkChildRef = @threesixtyLinkChildRef "
        '                   End If


        '                   sqlCon.Open()
        '                   sqlCmd.Connection = sqlCon
        '                   sqlCmd.CommandType = CommandType.Text

        '                   sqlCmd.CommandText = " Select * " + _
        '                                                             " FROM MS_dbMasterProjectProductThreesixtyLink " + _
        '                                                             " WHERE 1=1 " + queryParent + queryChild

        '                   If Trim(parent) <> "" Then
        '                           sqlCmd.Parameters.AddWithValue("@threesixtyLinkParentRef", threesixtyRef)
        '                   Else
        '                           sqlCmd.Parameters.AddWithValue("@threesixtyLinkChildRef", threesixtyRef)
        '                   End If
        '                   Dim sqlDa As New SqlDataAdapter(sqlCmd)

        '                   sqlDa.Fill(dt)

        '                   sqlCmd = Nothing
        '                   sqlCon.Close()

        '                   Return dt
        '           End Function

        '     Public Shared Function GetThreesixtyImage(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String, ByVal productRef As String, ByVal dbMasterProjectProductThreesixtyRef As String) As Byte()

        '             Dim sqlCon As New SqlConnection(_naproConStr)
        '                           Dim sqlCmd As New SqlCommand
        '                           Dim sqlDr As SqlDataReader
        '                           Dim dt As Byte() = {0}

        '                           If dbMasterProjectProductThreesixtyRef.Split(".").Length > 0 Then
        '                                   dbMasterProjectProductThreesixtyRef = dbMasterProjectProductThreesixtyRef.Split(".").First.ToString()
        '                           End If

        '                           sqlCon.Open()
        '                           sqlCmd.Connection = sqlCon
        '                           sqlCmd.CommandType = CommandType.Text
        '                           sqlCmd.CommandText = "select top 1 imageFile, dbMasterProjectProductThreesixtyRef " + _
        '                                                                           " from MS_dbMasterProjectProductThreesixty " + _
        '                                                                           "where 1=1 and dbMasterProjectProductThreesixtyRef = @dbMasterProjectProductThreesixtyRef " + _
        '                                                                           " and dbMasterRef = @dbMasterRef and projectRef = @projectRef and clusterRef = @clusterRef and productRef = @productRef "
        '                      sqlCmd.Parameters.AddWithValue("@dbMasterProjectProductThreesixtyRef", dbMasterProjectProductThreesixtyRef)
        '                           sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '                           sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '                           sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)
        '                           sqlCmd.Parameters.AddWithValue("@productRef", productRef)

        '                           sqlDr = sqlCmd.ExecuteReader
        '                           If sqlDr.Read Then
        '                                   dt = sqlDr("imageFile")
        '                           End If
        '                           sqlDr.Close()

        '                           sqlCmd = Nothing
        '                           'SqlConnection.ClearPool(sqlCon)
        '                           sqlCon.Close()

        '                           Return dt


        '                   End Function

        '     Public Shared Function getImageProductByCluster(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String) As DataTable
        '         Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable

        '         sqlNaProCon.Open()
        '         sqlCmd.Connection = sqlNaProCon

        '         sqlCmd.CommandText = CommandType.Text

        '         sqlCmd.CommandText = "Select pf.dbMasterProjectProductFileRef, pf.dbMasterRef, pf.projectRef, pf.productRef, p.clusterRef, pf.fileData As imgFile " + _
        '                           "From MS_dbMasterProjectProductFile pf " + _
        '                           "INNER JOIN MS_dbMasterProjectProduct p on p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef and p.productRef = pf.productRef " + _
        '                           "Where pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef and isHeaderImage = 1 and p.isActive = 1 " + _
        '                           " order by productRef, pf.sortNo asc"

        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlNaProCon.Close()

        '         Return dt

        '     End Function

        '     Public Shared Function getImageProductNapro(ByVal dbMasterProjectProductFileRef As String, ByVal dbMasterRef As String, ByVal projectRef As String, ByVal clusterRef As String, ByVal productRef As String) As DataTable
        '         Dim connectionString As String = ""

        '         connectionString = _naproConStr

        '         Dim sqlCon As New SqlConnection(connectionString)
        '         Dim sqlCmd As New SqlCommand
        '         Dim dt As New DataTable
        '         Dim querydbMasterProjectProductFileRef As String = ""
        '         Dim queryProductRef As String = ""
        '         If Trim(dbMasterProjectProductFileRef) <> "" And Trim(dbMasterProjectProductFileRef) <> "0" Then
        '             querydbMasterProjectProductFileRef = "and pf.dbMasterProjectProductFileRef = @dbMasterProjectProductFileRef "
        '         End If
        '         If Trim(productRef) <> "" And Trim(productRef) <> "0" Then
        '             queryProductRef = "and pf.productRef = @productRef "
        '         End If
        '         sqlCon.Open()
        '         sqlCmd.Connection = sqlCon

        '         sqlCmd.CommandType = CommandType.Text

        '         sqlCmd.CommandText = "SELECT pf.* " + _
        '                              "FROM MS_dbMasterProjectProductFile pf " + _
        '                              "inner join MS_dbMasterProjectProduct p on p.productRef = pf.productRef and p.dbMasterRef = pf.dbMasterRef and p.projectRef = pf.projectRef " + _
        '                              "where pf.dbMasterRef = @dbMasterRef and pf.projectRef = @projectRef and p.clusterRef = @clusterRef and pf.isHeaderImage = 1 and p.isActive = 1 " + _
        '                              querydbMasterProjectProductFileRef + _                     
        '                              queryProductRef + _                     
        '                              "order by pf.productRef, pf.sortNo asc" 


        '         sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '         sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '         If Trim(dbMasterProjectProductFileRef) <> "" And Trim(dbMasterProjectProductFileRef) <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@dbMasterProjectProductFileRef", dbMasterProjectProductFileRef)
        '         End If

        '         sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)

        '          If Trim(productRef) <> "" And Trim(productRef) <> "0" Then
        '             sqlCmd.Parameters.AddWithValue("@productRef", productRef)
        '         End If

        '         Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '         sqlDa.Fill(dt)

        '         sqlCmd = Nothing
        '         sqlCon.Close()

        '         Return dt
        '     End Function

        Public Shared Function getNewsletterTOP3(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim queryPublishDate As String = ""
            If isContentEnabledPublishDate(domainRef, tagRef, isChildSite) Then
                queryPublishDate = " AND DATEDIFF(DAY,GETDATE(),ct.publishDate)<=0 "
            End If

            Dim queryExpiredDate As String = ""
            If isContentEnabledExpiredDate(domainRef, tagRef, isChildSite) Then
                queryExpiredDate = " AND DATEDIFF(DAY,GETDATE(),ISNULL(ct.expiredDate,GETDATE()))>=0 "
            End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select TOP 3 ci.imgRef, cg.contentRef, ct.contentRef,ct.synopsis,ct.title,ct.titleDetail, ct.content, ct.embedVideo, cg.tagRef, ct.publishDate " + _
                                 "From " + _
                                 "      TR_contentTag cg " + _
                                 "      INNER JOIN TR_Content ct on ct.contentRef = cg.contentRef and ct.domainRef = cg.domainRef " + _
                                 "      INNER JOIN TR_contentImage ci on ci.contentRef = cg.contentRef and ci.contentRef = cg.contentRef and ci.domainRef = cg.domainRef and ci.domainRef = ct.domainRef " + _
                                 "Where cg.tagRef = @tagRef	and cg.domainRef = @domainRef " + _
                                 queryPublishDate + queryExpiredDate

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        'Public Shared Function getFacilitiesCluster(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal categoryRef As String, ByVal clusterRef As String) As String
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

        '    sqlCmd.CommandText = " select facilities " + _
        '                         " from MS_dbMasterProjectCluster " + _
        '                         " where dbMasterRef= @dbMasterRef and projectRef=@projectRef and categoryRef=@categoryRef and clusterRef=@clusterRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '    sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)
        '    sqlCmd.Parameters.AddWithValue("@clusterRef", clusterRef)


        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("facilities")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlnaproCon.Close()

        '    Return result
        'End Function

#End Region

#Region "Inquiries"
        'Public Shared Function getLKInquiries() As DataTable
        '    Dim sqlNaProCon As New SqlConnection(_naproConStr)

        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlNaProCon.Open()
        '    sqlCmd.Connection = sqlNaProCon

        '    sqlCmd.CommandText = CommandType.Text

        '    sqlCmd.CommandText =    "Select * " + _
        '                            "From LK_Adsinquiries "

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlNaProCon.Close()

        '    Return dt

        'End Function

        'Public Shared Function insertAdsLEAD(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal adsRef As String, ByVal name As String, _
        '                                     ByVal hp As String, ByVal email As String, ByVal inquiriesType As String, _
        '                                     ByVal inquiriesText As String, ByVal inputUN As String) As String

        '    Dim sqlCmd As New SqlCommand
        '    Dim result As String = ""
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlTrans As SqlTransaction
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlTrans = sqlCon.BeginTransaction

        '    sqlCmd.Transaction = sqlTrans
        '    sqlCmd.Connection = sqlCon

        '    Try
        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "select	isnull(max(leadRef),0) + 1 as ref " + _
        '                            "from TR_AdsLEAD "


        '        sqlDr = sqlCmd.ExecuteReader
        '        If sqlDr.Read Then
        '            result = sqlDr("ref")
        '        End If
        '        sqlDr.Close()


        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "insert	into TR_AdsLEAD " + _
        '                             "          (dbMasterRef, projectRef, adsRef, leadRef, name, hp, email, inquiriesType, inquiriesText, inputUN) " + _
        '                             "values	(@dbMasterRef, @projectRef, @adsRef, @leadRef, @name, @hp, @email, @inquiriesType, @inquiriesText, @inputUN) "

        '        sqlCmd.Parameters.AddWithValue("@leadRef", result)
        '        sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '        sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '        sqlCmd.Parameters.AddWithValue("@adsRef", adsRef)
        '        sqlCmd.Parameters.AddWithValue("@name", name)
        '        sqlCmd.Parameters.AddWithValue("@hp", hp)
        '        sqlCmd.Parameters.AddWithValue("@email", email)
        '        sqlCmd.Parameters.AddWithValue("@inquiriesType", inquiriesType)
        '        sqlCmd.Parameters.AddWithValue("@inquiriesText", inquiriesText)
        '        sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)

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

        'Public Shared Function getAds(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal adsRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlNaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlNaproCon.Open()
        '    sqlCmd.Connection = sqlNaproCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = " select * " + _
        '                         " from TR_ads " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and adsRef = @adsRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '    sqlCmd.Parameters.AddWithValue("@adsRef", adsRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlNaproCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function getAdsImage(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal adsRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlNaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlNaproCon.Open()
        '    sqlCmd.Connection = sqlNaproCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = " select * " + _
        '                         " from TR_adsImage " + _
        '                         " where dbMasterRef = @dbMasterRef and projectRef = @projectRef and adsRef = @adsRef "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '    sqlCmd.Parameters.AddWithValue("@adsRef", adsRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlNaproCon.Close()

        '    Return dt
        'End Function

        'Public Shared Function insertAdsView(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal adsRef As String, ByVal inputUN As String, ByVal sourceURL As String) As String

        '    Dim sqlCmd As New SqlCommand
        '    Dim result As String = ""
        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlTrans As SqlTransaction
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlTrans = sqlCon.BeginTransaction

        '    sqlCmd.Transaction = sqlTrans
        '    sqlCmd.Connection = sqlCon

        '    Try
        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "select	isnull(max(viewRef),0) + 1 as ref " + _
        '                             "from      TR_AdsView "


        '        sqlDr = sqlCmd.ExecuteReader
        '        If sqlDr.Read Then
        '            result = sqlDr("ref")
        '        End If
        '        sqlDr.Close()

        '        sqlCmd.Parameters.Clear()
        '        sqlCmd.Prepare()

        '        'Dim ipAddress() As System.Net.IPAddress = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())
        '        'Dim sourceURL As String = HttpContext.Current.Request.Url.AbsoluteUri
        '        Dim strHostName As String
        '        Dim strIPAddress As String
        '        strHostName = GetHostName()
        '        strIPAddress = GetHostByName(strHostName).AddressList(0).ToString()

        '        sqlCmd.CommandType = CommandType.Text
        '        sqlCmd.CommandText = "insert	into TR_AdsView " + _
        '                             "          (dbMasterRef, projectRef, adsRef, viewRef, ipAddress, timeAccess, sourceURL, inputUN) " + _
        '                             "values	(@dbMasterRef, @projectRef, @adsRef, @viewRef, @ipAddress, @timeAccess, @sourceURL, @inputUN) "

        '        sqlCmd.Parameters.AddWithValue("@viewRef", result)
        '        sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '        sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)
        '        sqlCmd.Parameters.AddWithValue("@adsRef", adsRef)
        '        'sqlCmd.Parameters.AddWithValue("@ipAddress", ipAddress(1).ToString)
        '        sqlCmd.Parameters.AddWithValue("@ipAddress", strIPAddress.ToString)
        '        sqlCmd.Parameters.AddWithValue("@timeAccess", Now())
        '        sqlCmd.Parameters.AddWithValue("@sourceURL", sourceURL)
        '        sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)

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

        'Public Shared Function getAdsRefByTitleAds(ByVal dbMasterRef As String, ByVal projectRef As String, ByVal titleAds As String) As String
        '    Dim result As String = ""
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader
        '    Dim dt As New DataTable

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "select adsRef from TR_ads " + _
        '                         "where dbMasterRef = @dbMasterRef and projectRef = @projectRef and titleAds like '%" + titleAds.ToString() + "%' "

        '    sqlCmd.Parameters.AddWithValue("@dbMasterRef", dbMasterRef)
        '    sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

        '    sqlDr = sqlCmd.ExecuteReader
        '    If sqlDr.Read Then
        '        result = sqlDr("adsRef")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getAdsImage(ByVal picRef As String) As DataTable
        '    Dim connectionString As String = ""

        '    connectionString = _naproConStr

        '    Dim sqlNaproCon As New SqlConnection(connectionString)
        '    Dim sqlCmd As New SqlCommand
        '    Dim dt As New DataTable

        '    sqlNaproCon.Open()
        '    sqlCmd.Connection = sqlNaproCon

        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = " select dbMasterRef,projectRef,picFile As imgFile,adsRef, picReF as imgRef, picName as imgFileName " + _
        '                         " from TR_adsImage " + _
        '                         " where picRef = @picRef "

        '    sqlCmd.Parameters.AddWithValue("@picRef", picRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(dt)

        '    sqlCmd = Nothing
        '    sqlNaproCon.Close()

        '    Return dt
        'End Function

#End Region

#Region "Meta Content Napro"
        'Public Shared Function getMetaTitleNapro(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT metaTitle " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "
        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metaTitle")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaAuthorNapro(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT metaAuthor " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "
        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metaAuthor")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getKeywordNapro(ByVal contentRef As String) As DataTable
        '    Dim result As New DataTable
        '    Dim sqlCon As New SqlConnection(_naproConstr)
        '    Dim sqlCmd As New SqlCommand

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "Select k.* FROM TR_contentKeyword k " + _
        '                         "INNER JOIN TR_content c On k.contentRef = c.contentRef " + _
        '                         "where k.contentRef = @contentRef and k.keywordText <> '' "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(result)
        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getDescriptionNapro(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_naproConStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText = "SELECT description " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "
        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("description")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

#End Region

#Region "Meta Content CMS Website"
        'Public Shared Function getMetaTitleContent(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT metaTitle " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metaTitle")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaAuthorContent(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT metaAuthor " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metaAuthor")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaKeywordContent(ByVal contentRef As String) As DataTable
        '    Dim result As New DataTable
        '    Dim sqlCon As New SqlConnection(_constr)
        '    Dim sqlCmd As New SqlCommand

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text
            
        '    sqlCmd.CommandText = "Select k.* FROM TR_contentKeyword k " + _
        '                         "INNER JOIN TR_content c On k.contentRef = c.contentRef " + _
        '                         "where k.contentRef = @contentRef and k.keywordText <> '' "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    Dim sqlDa As New SqlDataAdapter(sqlCmd)
        '    sqlDa.Fill(result)
        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaDescriptionContent(ByVal contentRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT metadescription " + _
        '                          "from TR_content  " + _
        '                          "where  contentRef = @contentRef "

        '    sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metadescription")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

#End Region

#Region "Meta Tag CMS Website"
        'Public Shared Function getMetaTitleTag(ByVal domainRef As String, ByVal tagRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT metaTitle " + _
        '                         "from MS_tag  " + _
        '                          "where  domainRef = @domainRef and tagRef = @tagRef "

        '    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metaTitle")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaAuthorTag(ByVal domainRef As String, ByVal tagRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT metaAuthor " + _
        '                          "from MS_tag  " + _
        '                          "where  domainRef = @domainRef and tagRef = @tagRef "

        '    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metaAuthor")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaKeywordTag(ByVal domainRef As String, ByVal tagRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT keyword " + _
        '                          "from MS_tag  " + _
        '                          "where  domainRef = @domainRef and tagRef = @tagRef "

        '    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("keyword")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function

        'Public Shared Function getMetaDescriptionTag(ByVal domainRef As String, ByVal tagRef As String) As String
        '    Dim result As String = String.Empty

        '    Dim sqlCon As New SqlConnection(_conStr)
        '    Dim sqlCmd As New SqlCommand
        '    Dim sqlDr As SqlDataReader

        '    sqlCon.Open()
        '    sqlCmd.Connection = sqlCon
        '    sqlCmd.CommandType = CommandType.Text

        '    sqlCmd.CommandText =  "SELECT metadescription " + _
        '                         "from MS_tag  " + _
        '                          "where  domainRef = @domainRef and tagRef = @tagRef "

        '    sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
        '    sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

        '    sqlDr = sqlCmd.ExecuteReader()
        '    If sqlDr.Read() Then
        '        result = sqlDr("metadescription")
        '    End If
        '    sqlDr.Close()

        '    sqlCmd = Nothing
        '    sqlCon.Close()

        '    Return result
        'End Function
        Public Shared Function getArtikelSearch(ByVal top As String, ByVal keyword As String, ByVal tagtypeRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable


            ''''' any word method ''''
            ''''' any word method ''''
            ''''' any word method ''''
            Dim field() As String = {"keywordText"}
            Dim whereSearch As New StringBuilder
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

            sqlCmd.CommandText = "SELECT top 12 ct.tagRef, mt.tagTypeRef, c.*, mt.tagName " + _
                                " FROM TR_content c " + _
                                " INNER JOIN TR_contentTag ct ON ct.contentRef = c.contentRef " + _
                                " inner join MS_tag mt on mt.tagRef = ct.tagRef  " + _
                                " inner join MS_tagType ctt on ctt.tagTypeRef = mt.tagTypeRef " + _
                                " where mt.tagTypeRef = " + tagtypeRef + " " + _
                                " and c.contentRef in (select distinct contentRef from tr_contentKeyword where 1=1 " + whereSearch.ToString + ") " + _
                                " order by c.publishDate desc "

            sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagtypeRef)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function
        Public Shared Function getImageRefByContentSearch(ByVal contentRef As String) As DataTable
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "Select ci.imgRef " + _
                                "From " + _
                                "           TR_contentTag cg " + _
                                "           INNER JOIN TR_Content ct on ct.contentRef = cg.contentRef " + _
                                "           INNER JOIN TR_contentImage ci on ci.contentRef = cg.contentRef and ci.contentRef = cg.contentRef " + _
                                "Where ct.contentRef = @contentRef "

            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        
        Public Shared Function getContentInfoByTag2(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As DataTable
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

            'Dim queryOrder As String = ""
            'If tagRef = _tagRefArtikel Then
            '    queryOrder = " order by cn.publishDate DESC"
            'ElseIf tagRef = _tagRefMediaRelease Then
            '    queryOrder = " order by cn.publishDate DESC"
            'Elseif tagRef = _tagRefNews Then
            '    queryOrder = " order by cn.publishDate DESC"
            'End If

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text



            sqlCmd.CommandText = "select	ct.tagRef, cn.contentRef, cn.contentType, cn.imgSetting, cn.imgPosition, tg.tagName, tg.tagPicture " + _
                              "           , cn.title, cn.synopsis, cn.content, cn.titleDetail " + _
                              "           , cn.contentDate, cn.publishDate, cn.expiredDate, cn.approvedDate, tg.description, cn.embedVideo, cn.metaDescription " + _
                              "from       tr_content cn " + _
                              "           INNER JOIN TR_contentTag ct on ct.domainRef = cn.domainRef and ct.contentRef = cn.contentRef " + _
                              "           INNER JOIN ms_tag tg on tg.tagRef = ct.tagRef " + _
                              "where	    ct.domainRef = @domainRef and ct.tagRef = @tagRef " + _
                              "order by cn.sortNo asc "
            'queryPublishDate + queryExpiredDate + queryOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function
#End Region

    End Class

End Namespace
