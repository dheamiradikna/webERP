Imports System.IO
Imports System.Data
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Drawing
Imports [class].clsWebGeneral


Namespace [class]

    Public Class clsGeneralDB
        Public Shared Function getFirstContentRefInTag(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	top 1 contentRef from tr_contentTag where domainRef = @domainRef and tagRef = @tagRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("contentRef").ToString
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getTagTypeNameByTagRef(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	tagTypeName " + _
                                "from	ms_tag t, ms_tagType tt " + _
                                "where  t.domainRef = tt.domainRef And t.tagTypeRef = tt.tagTypeRef " + _
                                "and t.domainRef = @domainRef and t.tagRef = @tagRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("tagTypeName")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getTagName(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	tagName " + _
                                "from	ms_tag " + _
                                "where domainRef = @domainRef and tagRef = @tagRef "


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

        Public Shared Function getDomainURLPreviewContent(ByVal domainRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select URLPreviewContent from ms_domain where domainRef = @domainRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("URLPreviewContent")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getContentFirstTag(ByVal domainRef As String, ByVal contentRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	top 1 tagRef " + _
                                "from       tr_contentTag " + _
                                "where      domainRef = @domainRef And contentRef = @contentRef"


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

        Public Shared Function getTagImageSetting(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	thumbImgW, thumbImgH, picImgW, picImgH " + _
                                "from ms_tag " + _
                                "where	domainRef = @domainRef and tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagPermissionList(ByVal domainRef As String, ByVal tagRefList As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	domainRef, tagRef, isTitle, isTitleDetail, isSynopsis, isContent " + _
                                "           , isThumbnail, isPicture,isVideo,isMap , isAttachment, isContentDate, isPictureHover, isImageSlideshow, isContentPubDate " + _
                                "           , isExpiredDate, isComment, isCommentPreApproval, isForum, isForumPreApproval " + _
                                "           , isPolling, isNeedApproval, isActive, testLink, thumbImgW, thumbImgH " + _
                                "           , picImgW, picImgH, approvalUser " + _
                                "from	    ms_tag t " + _
                                "where      domainRef = @domainRef and tagRef in (" + tagRefList + ")"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function cekIsTagHaveChild(ByVal domainRef As String, ByVal tagRef As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim result As Boolean = False
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	tagRef " + _
                                "from ms_tag " + _
                                "where domainRef = @domainRef and tagRefParent = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = True
            End If


            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getTagListByParentLookup(ByVal domainRef As String, ByVal tagRefParent As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable


            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	t.domainRef, tagRef, tagRefParent, t.tagTypeRef, tt.tagTypeName, tagName, isActive " + _
                                "from	    ms_tag t, ms_tagType tt " + _
                                "where	    t.domainRef = tt.domainRef and t.tagTypeRef = tt.tagTypeRef and t.domainRef = @domainRef and tagRefParent = @tagRefParent " + _
                                "order by   sortNo "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentTypeListLookup() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	contentType, contentTypeName " + _
                                "from lk_ContentType " + _
                                "order by sortNo"


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentTypeListLookupContentRelated() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select * from tr_content "


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentImgSettingLookup() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	imgSetting, imgSettingName " + _
                                "from lk_contentImgSetting " + _
                                "order by sortNo"


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentImgPositionLookup() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	imgPosition, imgPositionName " + _
                                "from lk_imagePosition " + _
                                "order by sortNo"


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainSetting(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	imageBgColor " + _
                                "from ms_domainSetting " + _
                                "where domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function


        Public Shared Function cekDomainValid(ByVal domainName As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	domainRef " + _
                                "from ms_domain " + _
                                "where	domainName = @domainName and isActive = '1'"


            sqlCmd.Parameters.AddWithValue("@domainName", domainName)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = True
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function cekUsername(ByVal domainRef As String, ByVal email As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	userRef " + _
                                "from ms_user " + _
                                "where	domainRef = @domainRef and email = @email"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@email", email)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = True
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function cekLogin(ByVal domainRef As String, ByVal email As String, _
                                        ByVal password As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	userRef " + _
                                "from ms_user " + _
                                "where	domainRef = @domainRef and email = @email and password = @password"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@email", email)
            sqlCmd.Parameters.AddWithValue("@password", password)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = True
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function cekLoginRetRef(ByVal domainRef As String, ByVal email As String, _
                                        ByVal password As String) As Integer
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Integer = 0

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	userRef " + _
                                "from ms_user " + _
                                "where	domainRef = @domainRef and email = @email and password = @password"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@email", email)
            sqlCmd.Parameters.AddWithValue("@password", password)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("userRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function countUserByStatus(ByVal domainRef As String, ByVal userStatus As String) As Integer
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Integer = 0

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	count(userRef) as jml " + _
                                "from ms_user " + _
                                "where	domainRef = @domainRef and userStatus = @userStatus"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@userStatus", userStatus)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("jml")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getUserStatusListLookup() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	userStatus, userStatusName " + _
                                "from lk_userStatus " + _
                                "order	by sort"


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainRefByName(ByVal domainName As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	top 1 domainRef " + _
                                "from	ms_domain " + _
                                "where domainName = @domainName and isActive = '1' "


            sqlCmd.Parameters.AddWithValue("@domainName", domainName)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("domainRef")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getDomainNameByRef(ByVal domainRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	domainName " + _
                                "from	ms_domain " + _
                                "where domainRef = @domainRef "


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("domainName")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getDomainListLookup() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	d.domainRef, d.domainLevel, d.domainName " + _
                                "from	    ms_domain d " + _
                                "where	    isActive = '1' " + _
                                "order by   domainName "


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainListLookupMall() As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	d.domainRef, d.domainLevel, d.domainName " + _
                                "from	    ms_domain d " + _
                                "where	    isActive = '1' " + _
                                "order by   domainName "


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagTypeListLookup(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	tagTypeRef, tagTypeName " + _
                                "from ms_tagType " + _
                                "where	domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getContentDisplayListLookup() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	contentDisplayRef, contentDisplayName " + _
                                "from lk_contentDisplay " + _
                                "order by sortNo asc "

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getDomainLevelList() As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	domainLevel, domainLevelName " + _
                                "from lk_domainLevel " + _
                                "order 	by sort"

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getCountryList() As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	countryCode, countryName " + _
                                "from       lk_addrCountry " + _
                                "order	by  sortNo"

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListProvince(ByVal countryCode As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	provinceCode, provinceName " + _
                                "from       lk_addrProvince " + _
                                "where	    countryCode = @countryCode " + _
                                "order	    by sortNo"

            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListCity(ByVal countryCode As String, ByVal provinceCode As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	cityCode, cityName " + _
                                "from       lk_addrCity " + _
                                "where	    countryCode = @countryCode " + _
                                "           and provinceCode = @provinceCode " + _
                                "order by   sortNo"

            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)
            sqlCmd.Parameters.AddWithValue("@provinceCode", provinceCode)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function GetAllProjectList(ByVal keyword As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            ''''' any word method ''''
            ''''' any word method ''''
            ''''' any word method ''''
            Dim field() As String = {"t.title"}
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
            sqlCmd.CommandText = "select	t.contentRef, t.title " + _
                                "from       tr_content t where 1=1 " + _
                                whereSearch.ToString


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getListProductType(ByVal categoryRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	* " + _
                                "from       ms_productType " + _
                                " where categoryRef = @categoryRef "

            sqlCmd.Parameters.AddWithValue("@categoryRef", categoryRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)


            sqlDa.Fill(dt)

            sqlCmd = Nothing
            'SqlConnection.ClearPool(sqlCon)
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function InsertListContent(ByVal contentRef As String, ByVal count As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim dt As New DataTable
            Dim sqlTrans As SqlTransaction
            Dim sqlDr As SqlDataReader
            
            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "select	isnull(max(likeRef),0) + 1 as ref " + _
                                    "from TR_likeContentImage "

                sqlDr = sqlCmd.ExecuteReader
                If sqlDr.Read Then
                    result = sqlDr("ref")
                End If
                sqlDr.Close()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText =  " insert into  TR_likeContentImage " + _
                                      " (likeRef, contentRef, count) " + _
                                      " values (@likeRef, @contentRef, @count) "
                               
                              
                sqlCmd.Parameters.AddWithValue("@likeRef", result)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.Parameters.AddWithValue("@count", count)

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

        Public Shared Function InsertListContentProject(ByVal projectRef As String, ByVal count As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim dt As New DataTable
            Dim sqlTrans As SqlTransaction
            Dim sqlDr As SqlDataReader
            
            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "select	isnull(max(logRef),0) + 1 as ref " + _
                                    "from LOG_projectLike "

                sqlDr = sqlCmd.ExecuteReader
                If sqlDr.Read Then
                    result = sqlDr("ref")
                End If
                sqlDr.Close()

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = " insert into  LOG_projectLike " + _
                                      " (logRef, projectRef) " + _
                                      " values (@logRef, @projectRef) "


                sqlCmd.Parameters.AddWithValue("@logRef", result)
                sqlCmd.Parameters.AddWithValue("@projectRef", projectRef)

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

        Public Shared Function getProvinceList(ByVal countryCode As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	provinceCode, provinceName " + _
                                "from       lk_addrProvince " + _
                                "where	    countryCode = @countryCode " + _
                                "order	    by sortNo"

            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getCityList(ByVal countryCode As String, ByVal provinceCode As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	cityCode, cityName " + _
                                "from       lk_addrCity " + _
                                "where	    countryCode = @countryCode " + _
                                "           and provinceCode = @provinceCode " + _
                                "order by   sortNo"

            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)
            sqlCmd.Parameters.AddWithValue("@provinceCode", provinceCode)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getCountryListAll() As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select 'All' as countryCode, 'All' as countryName, 0 as sortUnion, 0 as sortNo " + _
                                "union " + _
                                "select	countryCode, countryName, 1 as sortUnion, sortNo " + _
                                "from lk_addrCountry " + _
                                "order	by sortUnion, sortNo "

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getProvinceListAll(ByVal countryCode As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select 'All' as provinceCode, 'All' as provinceName, 0 as sortUnion, 0 as sortNo " + _
                                "union " + _
                                "select	provinceCode, provinceName, 1 as sortUnion, sortNo " + _
                                "from lk_addrProvince " + _
                                "where	countryCode = @countryCode " + _
                                "order	by sortUnion, sortNo"

            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getCityListAll(ByVal countryCode As String, ByVal provinceCode As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select 'All' as cityCode, 'All' as cityName, 0 as sortUnion, 0 as sortNo " + _
                                "union " + _
                                "select	cityCode, cityName, 1 as sortUnion, sortNo " + _
                                "from lk_addrCity " + _
                                "where	countryCode = @countryCode " + _
                                "and provinceCode = @provinceCode " + _
                                "order by sortUnion, sortNo "

            sqlCmd.Parameters.AddWithValue("@countryCode", countryCode)
            sqlCmd.Parameters.AddWithValue("@provinceCode", provinceCode)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        #Region "GENERATED"
        Public Shared Function SaveFileToFolder(ByVal domainRef As String, ByVal fileRef As String, ByVal fileName As String, ByVal fileExtension As String, ByVal fileData As Byte(), ByVal docType As String) As String
            Dim url As String = ""

            Try
                If Not System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~") + "\data\" + domainRef + "\" + docType + "") Then
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + "\data\" + domainRef + "\" + docType + "")
                End If

                Dim path As String = ""
                path = HttpContext.Current.Server.MapPath("~\data\" + domainRef + "\" + docType + "\")

                File.WriteAllBytes(path + fileName + "_" + fileRef + "." + fileExtension, fileData)

                url = _rootPath + "data/" + domainRef + "/" + docType + "/" + fileName + "_" + fileRef + "." + fileExtension

            Catch ex As Exception
                url = ex.Message
            End Try

            Return url
        End Function
#End Region
          




    End Class

End Namespace