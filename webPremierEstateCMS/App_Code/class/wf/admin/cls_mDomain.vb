Imports Microsoft.VisualBasic
Imports [class].clsWebGeneral
Imports System.Data.SqlClient


Namespace [class]


    Public Class cls_mDomain
        Public Shared Function cekDomainLogoFile(ByVal domainRef As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	logo " + _
                                "from ms_domainSetting " + _
                                "where	domainRef = @domainRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                If Not IsDBNull(sqlDr("logo")) Then
                    result = True
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function cekDomainImageFile(ByVal domainRef As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	imgFile " + _
                                "from ms_domainSetting " + _
                                "where	domainRef = @domainRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                If Not IsDBNull(sqlDr("imgFile")) Then
                    result = True
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
        Public Shared Function getDomainList(ByVal domainLevel As String, _
                                             ByVal isActive As String, ByVal keyword As String, _
                                             ByVal sortBy As String, ByVal sortType As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

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
            Dim field() As String = {"domainLevelName", "domainName", "IP", "CPName", "CPEmail", "CPHP"}
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

            sqlCmd.CommandText = "select	d.domainRef, d.domainLevel, dl.domainLevelName, d.domainName " + _
                                "           , d.IP, d.CPName, d.CPEmail, d.CPHP, d.CPPhone, d.CPAddr " + _
                                "           , d.isActive, d.inputTime, d.inputUN " + _
                                "from	    ms_domain d, lk_domainLevel dl " + _
                                "where	    d.domainLevel = dl.domainLevel " + _
                                "           and (isActive = @isActive or 'All' = @isActive) " + _
                                "           and (d.domainLevel = @domainLevel or 'All' = @domainLevel)  " + _
                                whereSearch.ToString + strOrder

            sqlCmd.Parameters.AddWithValue("@isActive", isActive)
            sqlCmd.Parameters.AddWithValue("@domainLevel", domainLevel)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainInfo(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	d.domainRef, d.domainLevel, dl.domainLevelName, d.domainName " + _
                                "           ,d.countryCode, d.provinceCode, d.cityCode,d.description,d.isUpcomingMalls, d.IP, d.CPName, d.CPEmail, d.CPHP, d.CPPhone, d.CPAddr " + _
                                "           , d.isActive, d.inputTime, d.inputUN " + _
                                "from	    ms_domain d, lk_domainLevel dl " + _
                                "where      d.domainLevel = dl.domainLevel " + _
                                "           and d.domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainInfoHTML(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.Columns.Add("domainRef")
            dt.Columns.Add("domainLevel")
            dt.Columns.Add("domainLevelName")
            dt.Columns.Add("domainName")
            dt.Columns.Add("IP")
            dt.Columns.Add("CPName")
            dt.Columns.Add("CPEmail")
            dt.Columns.Add("CPHP")
            dt.Columns.Add("CPPhone")
            dt.Columns.Add("isActive")

            sqlCmd.CommandText = "select	d.domainRef, d.domainLevel, dl.domainLevelName, d.domainName " + _
                                "           , d.IP, d.CPName, d.CPEmail, d.CPHP, d.CPPhone, d.CPAddr " + _
                                "           , d.isActive, d.inputTime, d.inputUN " + _
                                "from	    ms_domain d, lk_domainLevel dl " + _
                                "where      d.domainLevel = dl.domainLevel " + _
                                "           and d.domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                dr = dt.NewRow
                dr("domainRef") = sqlDr("domainRef")
                dr("domainLevel") = sqlDr("domainLevel")
                dr("domainLevelName") = sqlDr("domainLevelName")
                dr("domainName") = sqlDr("domainName")
                dr("IP") = sqlDr("IP")
                dr("CPName") = sqlDr("CPName")
                dr("CPEmail") = sqlDr("CPEmail")
                dr("CPHP") = sqlDr("CPHP")
                dr("CPPhone") = sqlDr("CPPhone")

                If sqlDr("isActive") = "1" Then
                    dr("isActive") = "<img height=""13"" src=""" + _rootPath + "support/image/icon_yes.png"" />"
                Else
                    dr("isActive") = "<img height=""13"" src=""" + _rootPath + "support/image/icon_no.png"" />"
                End If


                dt.Rows.Add(dr)
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function deleteDomain(ByVal domainRef As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStrSite)

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            

            Try
                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	ms_domain " + _
                                     "where domainRef = @domainRef"


                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.ExecuteNonQuery()


                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	ms_domainSetting " + _
                                     "where domainRef = @domainRef"
                
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	ms_tag " + _
                                     "where domainRef = @domainRef"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	ms_tagType " + _
                                     "where domainRef = @domainRef"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function deleteImage(ByVal domainRef As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "update	ms_domainSetting set imgFile = NULL, imgFileName = '' " + _
                                 "where domainRef = @domainRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
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

        Public Shared Function deleteLogo(ByVal domainRef As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "update	ms_domainSetting set logo = NULL, logoFileName = '' " + _
                                 "where domainRef = @domainRef"
                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
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

        Public Shared Function insertDomain(ByVal domainLevel As String, ByVal domainName As String, _
                                            ByVal countryCode As String, ByVal provinceCode As String, ByVal cityCode As String, ByVal description As String, ByVal isUpcoming As String, _
                                            ByVal IP As String, ByVal CPName As String, ByVal CPEmail As String, _
                                            ByVal CPHP As String, ByVal CPPhone As String, _
                                            ByVal CPAddr As String, ByVal isActive As String, _
                                            ByVal inputUN As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlTrans As SqlTransaction
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "select	isnull(max(domainRef),0) + 1 as ref " + _
                                    "from ms_domain "

                sqlDr = sqlCmd.ExecuteReader
                If sqlDr.Read Then
                    result = sqlDr("ref")
                End If
                sqlDr.Close()


                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "insert	into ms_domain " + _
                                    "           (domainRef, domainLevel, domainName,countryCode,provinceCode,cityCode,description,isUpcomingMalls, IP, CPName, CPEmail, CPHP, CPPhone, CPAddr, isActive, inputUN) " + _
                                    "select 	@domainRef, @domainLevel, @domainName,@countryCode,@provinceCode,@cityCode,@description,@isUpComing, @IP, @CPName " + _
                                    "           , @CPEmail, @CPHP, @CPPhone, @CPAddr, @isActive, @inputUN "

                sqlCmd.Parameters.AddWithValue("@domainRef", result)
                sqlCmd.Parameters.AddWithValue("@domainLevel", domainLevel)
                sqlCmd.Parameters.AddWithValue("@domainName", domainName)
                sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)
                sqlCmd.Parameters.AddWithValue("@provinceCode", provinceCode)
                sqlCmd.Parameters.AddWithValue("@cityCode", cityCode)
                sqlCmd.Parameters.AddWithValue("@description", description)
                sqlCmd.Parameters.AddWithValue("@isUpComing", isUpcoming)
                sqlCmd.Parameters.AddWithValue("@IP", IP)
                sqlCmd.Parameters.AddWithValue("@CPName", CPName)
                sqlCmd.Parameters.AddWithValue("@CPEmail", CPEmail)
                sqlCmd.Parameters.AddWithValue("@CPHP", CPHP)
                sqlCmd.Parameters.AddWithValue("@CPPhone", CPPhone)
                sqlCmd.Parameters.AddWithValue("@CPAddr", CPAddr)
                sqlCmd.Parameters.AddWithValue("@isActive", isActive)
                sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)

                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "insert	into ms_domainSetting " + _
                                    "           (domainRef, imageBgColor) " + _
                                    "values 	(@domainRef,@imageBgColor) "

                sqlCmd.Parameters.AddWithValue("@domainRef", result)
                sqlCmd.Parameters.AddWithValue("@imageBgColor", _imageBgColor)

                sqlCmd.ExecuteNonQuery()


                Dim queryScriptDefaultMenu As New StringBuilder

                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tagType] ([domainRef], [tagTypeRef], [tagTypeName], [inputTime], [inputUN]) VALUES (@domainRef, 1, N'Menu', GetDate(), N'" + inputUN + "');")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tagType] ([domainRef], [tagTypeRef], [tagTypeName], [inputTime], [inputUN]) VALUES (@domainRef, 2, N'Content', GetDate(), N'" + inputUN + "');")



                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 1, 0, 1, 5, N'Stores', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 2, 0, 1, 1, N'F&B', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 3, 0, 1, 6, N'Mall Map', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'0', N'0', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 4, 0, 1, 7, N'Events', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'1', N'1', N'0', N'1', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 5, 0, 1, 1, N'Promotions/Deals at Tenants', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 6, 0, 1, 1, N'Movies', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'1', N'1', N'0', N'1', N'1', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 7, 0, 1, 6, N'Directions', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 8, 0, 1, 2, N'Services', 0x, N'', N'', N'', 0, N'0', N'0', N'1', N'0', N'1', N'1', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 9, 0, 1, 4, N'Contact Us', 0x, N'', N'', N'', 0, N'0', N'1', N'0', N'0', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")
                queryScriptDefaultMenu.Append("INSERT [dbo].[MS_tag] ([domainRef], [tagRef], [tagRefParent], [tagTypeRef], [contentDisplayRef], [tagName], [tagPicture], [tagPictureFile], [keyword], [description], [sortNo], [isOnlyParent], [isSingleContent], [isTitle], [isTitleDetail], [isSynopsis], [isContent], [isThumbnail], [isPicture], [isAttachment], [isContentDate], [isContentPubDate], [isExpiredDate], [isComment], [isCommentPreApproval], [isForum], [isForumPreApproval], [isPolling], [isNeedApproval], [isDisplay1], [isDisplay2], [isDisplay3], [isDisplay4], [isActive], [testLink], [thumbImgW], [thumbImgH], [picImgW], [picImgH], [approvalUser], [inputTime], [inputUN]) VALUES (@domainRef, 10, 0, 2, 1, N'Sliding Banner Home', 0x, N'', N'', N'', 0, N'0', N'0', N'0', N'1', N'0', N'0', N'0', N'1', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'0', N'1', N'', 0, 0, 0, 0, N'', GETDATE(), N'" + inputUN + "')")

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = queryScriptDefaultMenu.ToString()

                sqlCmd.Parameters.AddWithValue("@domainRef", result)

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

        Public Shared Function updateDomain(ByVal domainRef As String, _
                                            ByVal domainLevel As String, ByVal domainName As String, _
                                            ByVal countryCode As String, ByVal provinceCode As String, ByVal cityCode As String, ByVal description As String, ByVal isUpcoming As String, _
                                            ByVal IP As String, ByVal CPName As String, ByVal CPEmail As String, _
                                            ByVal CPHP As String, ByVal CPPhone As String, _
                                            ByVal CPAddr As String, ByVal isActive As Boolean) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = domainRef
            Dim sqlCon As New SqlConnection(_conStrSite)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "update	ms_domain " + _
                                "set	domainLevel = @domainLevel, domainName = @domainName, countryCode=@countryCode, provinceCode=@provinceCode, cityCode=@cityCode, description=@description, isUpcomingMalls=@isUpComing, IP = @IP " + _
                                ", CPName = @CPName, CPEmail = @CPEmail, CPHP = @CPHP, CPPhone = @CPPhone,CPAddr = @CPAddr " + _
                                ", isActive = @isActive " + _
                                "where	domainRef = @domainRef "



            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@domainLevel", domainLevel)
            sqlCmd.Parameters.AddWithValue("@domainName", domainName)
            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)
            sqlCmd.Parameters.AddWithValue("@provinceCode", provinceCode)
            sqlCmd.Parameters.AddWithValue("@cityCode", cityCode)
            sqlCmd.Parameters.AddWithValue("@description", description)
            sqlCmd.Parameters.AddWithValue("@isUpComing", isUpcoming)
            sqlCmd.Parameters.AddWithValue("@IP", IP)
            sqlCmd.Parameters.AddWithValue("@CPName", CPName)
            sqlCmd.Parameters.AddWithValue("@CPEmail", CPEmail)
            sqlCmd.Parameters.AddWithValue("@CPHP", CPHP)
            sqlCmd.Parameters.AddWithValue("@CPPhone", CPPhone)
            sqlCmd.Parameters.AddWithValue("@CPAddr", CPAddr)
            sqlCmd.Parameters.AddWithValue("@isActive", isActive)

            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function
    End Class

End Namespace
