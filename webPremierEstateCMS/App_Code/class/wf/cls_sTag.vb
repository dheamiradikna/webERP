Imports Microsoft.VisualBasic
Imports [class].clsWebGeneral
Imports System.Data.SqlClient


Namespace [class]
    Public Class cls_sTag
        Public Shared Function cekTagFile(ByVal domainRef As String, ByVal tagRef As String) As Boolean
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	tagPictureFile " + _
                                "from ms_tag " + _
                                "where	domainRef = @domainRef and tagRef = @tagRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                If Trim(sqlDr("tagPictureFile")) <> "" Then
                    result = True
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getTagRefParent(ByVal domainRef As String, ByVal tagRef As String) As Integer
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Integer = 0

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	tagRefParent " + _
                                "from	ms_tag " + _
                                "where domainRef = @domainRef and tagRef = @tagRef "


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

        Public Shared Sub rekTagParent(ByVal domainRef As String, ByVal tagRef As String, ByRef level As Integer)
            Dim tagRefParent As Integer = getTagRefParent(domainRef, tagRef)
            If tagRefParent <> 0 Then
                level = level + 1
                rekTagParent(domainRef, tagRefParent, level)
            End If
        End Sub

        Public Shared Function getTagLevel(ByVal domainRef As String, ByVal tagRef As String) As Integer
            Dim result As Integer = 0

            rekTagParent(domainRef, tagRef, result)

            Return result
        End Function

        Public Shared Sub rekTagChild(ByRef dtTree As DataTable, _
                                      ByVal domainRef As String, ByVal tagTypeRef As String, _
                                      ByVal tagRefParent As String, ByVal isActive As String, _
                                      ByVal keyword As String, _
                                        ByVal sortBy As String, ByVal sortType As String, _
                                        ByVal level As Integer)
            Dim dt As New DataTable
            Dim drTree As DataRow

            dt = getTagList(domainRef, tagTypeRef, tagRefParent, isActive, keyword, sortBy, sortType)
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
                    drTree("isSingleContent") = dt.Rows(i).Item("isSingleContent")
                    drTree("isOnlyParent") = dt.Rows(i).Item("isOnlyParent")
                    drTree("level") = level + 1
                    dtTree.Rows.Add(drTree)

                    rekTagChild(dtTree, domainRef, tagTypeRef, dt.Rows(i).Item("tagRef").ToString, isActive, keyword, sortBy, sortType, level + 1)
                Next
            End If
        End Sub

        Public Shared Function getTagTreeList(ByVal domainRef As String, ByVal tagTypeRef As String, _
                                              ByVal tagRefParent As String, ByVal isActive As String, _
                                              ByVal keyword As String, _
                                                ByVal sortBy As String, ByVal sortType As String) As DataTable
            Dim dt As New DataTable

            dt.Columns.Add("domainRef")
            dt.Columns.Add("tagRef")
            dt.Columns.Add("tagRefParent")
            dt.Columns.Add("tagTypeRef")
            dt.Columns.Add("tagTypeName")
            dt.Columns.Add("tagName")
            dt.Columns.Add("isActive")
            dt.Columns.Add("isSingleContent")
            dt.Columns.Add("isOnlyParent")
            dt.Columns.Add("level")

            rekTagChild(dt, domainRef, tagTypeRef, tagRefParent, isActive, keyword, sortBy, sortType, 0)

            Return dt
        End Function

        Public Shared Function getTagList(ByVal domainRef As String, ByVal tagTypeRef As String, _
                                                     ByVal tagRefParent As String, ByVal isActive As String, _
                                                     ByVal keyword As String, _
                                                     ByVal sortBy As String, ByVal sortType As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable
            Dim queryTagType As String = ""

            If Trim(tagTypeRef) <> "" And Trim(tagTypeRef) <> "All" Then
                queryTagType = " and t.tagTypeRef = @tagTypeRef "
            End If

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
            Dim field() As String = {"tagName", "tagTypeName", "keyword"}
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

            sqlCmd.CommandText = "select	t.domainRef, tagRef, tagRefParent, t.tagTypeRef, tt.tagTypeName, tagName, keyword, isActive, isSingleContent, isOnlyParent " + _
                                "from	    ms_tag t, ms_tagType tt " + _
                                "where	    t.domainRef = tt.domainRef and t.tagTypeRef = tt.tagTypeRef " + _
                                "           and t.domainRef = @domainRef " + _
                                queryTagType + _
                                "           and t.tagRefParent = @tagRefParent " + _
                                "           and (isActive = @isActive or 'All' = @isActive) " + _
                                whereSearch.ToString + strOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
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

        Public Shared Function getTagInfo(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	domainRef, tagRef, tagRefParent, tagTypeRef,contentDisplayRef, tagName, tagPicture, keyword " + _
                                "           , description, metaDescription, sortNo, isSingleContent, isOnlyParent, isTitle, isTitleDetail, isSynopsis, isContent " + _
                                "           , isThumbnail, isPicture,isVideo,isMap , isAttachment, isContentDate, isPictureHover, isImageSlideshow, isContentPubDate " + _
                                "           , isExpiredDate, isComment, isCommentPreApproval, isForum, isForumPreApproval " + _
                                "           , isPolling, isNeedApproval, isActive " + _
                                "           , isDisplay1, isDisplay2, isDisplay3, isDisplay4 " + _
                                "           , testLink, thumbImgW, thumbImgH " + _
                                "           , picImgW, picImgH, approvalUser, inputTime, inputUN, metaTitle, metaAuthor " + _
                                "from	    ms_tag t " + _
                                "where      domainRef = @domainRef and tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagInfoHTML(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.Columns.Add("domainRef")
            dt.Columns.Add("tagRef")
            dt.Columns.Add("tagRefParent")
            dt.Columns.Add("tagTypeRef")
            dt.Columns.Add("tagTypeName")
            dt.Columns.Add("tagName")
            dt.Columns.Add("isActive")
            dt.Columns.Add("level")

            sqlCmd.CommandText = "select	t.domainRef, tagRef, tagRefParent, t.tagTypeRef, tt.tagTypeName, tagName, isActive " + _
                                "from	    ms_tag t, ms_tagType tt " + _
                                "where	    t.domainRef = tt.domainRef and t.tagTypeRef = tt.tagTypeRef " + _
                                "           and t.domainRef = @domainRef " + _
                                "           and t.tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)


            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                Dim level As Integer = getTagLevel(domainRef, sqlDr("tagRef").ToString) + 1
                Dim tagName As String = ""
                For j = 1 To level - 1
                    tagName = tagName + "&nbsp;&nbsp;&nbsp;"
                Next
                tagName = tagName + sqlDr("tagName")

                dr = dt.NewRow
                dr("domainRef") = sqlDr("domainRef")
                dr("tagRef") = sqlDr("tagRef")
                dr("tagRefParent") = sqlDr("tagRefParent")
                dr("tagTypeRef") = sqlDr("tagTypeRef")
                dr("tagTypeName") = sqlDr("tagTypeName")
                dr("tagName") = tagName '+ sqlDr("tagRef").ToString
                dr("level") = level

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

         Public Shared Function getContentRefByTagRef(ByVal tagRef As String) As String
            Dim result As String = String.Empty

            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select contentRef " + _
                                 "from TR_contentTag " + _
                                 "where 1=1 and tagRef = @tagRef "
              sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlDr = sqlCmd.ExecuteReader()
            If sqlDr.Read() Then
                result = sqlDr("contentRef")
            End If
            
            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function deleteTag(ByVal domainRef As String, ByVal tagRef As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)
            Dim contentRef As String = ""

            contentRef = getContentRefByTagRef(tagRef)

            sqlCon.Open()

            Try
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	TR_contentImage " + _
                                     "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	TR_content " + _
                                     "where domainRef = @domainRef and contentRef = @contentRef"
                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
                sqlCmd.ExecuteNonQuery()

                sqlCmd.Parameters.Clear()
                sqlCmd.Prepare()
                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "delete	ms_tag " + _
                                     "where domainRef = @domainRef and tagRef = @tagRef"
                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
                
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertTag(ByVal domainRef As String, ByVal tagRefParent As String, ByVal tagTypeRef As String, ByVal contentDisplayRef As String, _
                                         ByVal tagName As String, ByVal keyword As String, ByVal description As String, ByVal metaDescription As String, _
                                         ByVal sortNo As String, ByVal isSingleContent As String, ByVal isOnlyParent As String, _
                                         ByVal isTitle As String, ByVal isTitleDetail As String, _
                                         ByVal isSynopsis As String, ByVal isContent As String, ByVal isThumbnail As String, _
                                         ByVal isPicture As String, ByVal isVideo As String,ByVal isMap As String , ByVal isAttachment As String, ByVal isContentDate As String, _
                                         ByVal isPictureHover As String, ByVal isImageSlideshow As String, _
                                         ByVal isContentPubDate As String, ByVal isExpiredDate As String, ByVal isComment As String, _
                                         ByVal isCommentPreApproval As String, ByVal isForum As String, ByVal isForumPreApproval As String, _
                                         ByVal isPolling As String, ByVal isNeedApproval As String, _
                                         ByVal isDisplay1 As String, ByVal isDisplay2 As String, ByVal isDisplay3 As String, ByVal isDisplay4 As String, _
                                         ByVal isActive As String, _
                                         ByVal testLink As String, ByVal thumbImgW As String, ByVal thumbImgH As String, _
                                         ByVal picImgW As String, ByVal picImgH As String, ByVal approvalUser As String, _
                                         ByVal inputUN As String, ByVal metaTitle As String, ByVal metaAuthor As String) As String

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
                sqlCmd.CommandText = "select	isnull(max(tagRef),0) + 1 as ref " + _
                                    "from ms_tag " + _
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
                sqlCmd.CommandText = "insert	into ms_tag " + _
                                    "           (domainRef, tagRef, tagRefParent, tagTypeRef,contentDisplayRef, tagName, keyword " + _
                                    "           , description, metaDescription, sortNo, isSingleContent, isOnlyParent, isTitle, isTitleDetail, isSynopsis, isContent " + _
                                    "           , isThumbnail, isPicture, isVideo,isMap , isAttachment, isContentDate, isPictureHover, isImageSlideshow, isContentPubDate " + _
                                    "           , isExpiredDate, isComment, isCommentPreApproval, isForum, isForumPreApproval " + _
                                    "           , isPolling, isNeedApproval " + _
                                    "           , isDisplay1, isDisplay2, isDisplay3, isDisplay4 " + _
                                    "           , isActive, testLink, thumbImgW, thumbImgH " + _
                                    "           , picImgW, picImgH, approvalUser, inputUN, metaTitle, metaAuthor) " + _
                                    "values	    (@domainRef, @tagRef, @tagRefParent, @tagTypeRef,@contentDisplayRef, @tagName, @keyword " + _
                                    "           , @description, @metaDescription, @sortNo, @isSingleContent, @isOnlyParent, @isTitle, @isTitleDetail, @isSynopsis, @isContent " + _
                                    "           , @isThumbnail, @isPicture,@isVideo,@isMap , @isAttachment, @isContentDate,@isPictureHover, @isImageSlideshow, @isContentPubDate " + _
                                    "           , @isExpiredDate, @isComment, @isCommentPreApproval, @isForum, @isForumPreApproval " + _
                                    "           , @isPolling, @isNeedApproval " + _
                                    "           , @isDisplay1, @isDisplay2, @isDisplay3, @isDisplay4 " + _
                                    "           , @isActive, @testLink, @thumbImgW, @thumbImgH " + _
                                    "           , @picImgW, @picImgH, @approvalUser, @inputUN, @metaTitle, @metaAuthor)"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@tagRef", result)
                sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)
                sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagTypeRef)
                sqlCmd.Parameters.AddWithValue("@contentDisplayRef", contentDisplayRef)
                sqlCmd.Parameters.AddWithValue("@tagName", tagName)
                sqlCmd.Parameters.AddWithValue("@keyword", keyword)
                sqlCmd.Parameters.AddWithValue("@description", description)
                sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)
                sqlCmd.Parameters.AddWithValue("@sortNo", sortNo)
                sqlCmd.Parameters.AddWithValue("@isSingleContent", isSingleContent)
                sqlCmd.Parameters.AddWithValue("@isOnlyParent", isOnlyParent)
                sqlCmd.Parameters.AddWithValue("@isTitle", isTitle)
                sqlCmd.Parameters.AddWithValue("@isTitleDetail", isTitleDetail)
                sqlCmd.Parameters.AddWithValue("@isSynopsis", isSynopsis)
                sqlCmd.Parameters.AddWithValue("@isContent", isContent)
                sqlCmd.Parameters.AddWithValue("@isThumbnail", isThumbnail)
                sqlCmd.Parameters.AddWithValue("@isPicture", isPicture)
                sqlCmd.Parameters.AddWithValue("@isVideo", isVideo)
                sqlCmd.Parameters.AddWithValue("@isMap", isMap)
                sqlCmd.Parameters.AddWithValue("@isAttachment", isAttachment)
                sqlCmd.Parameters.AddWithValue("@isContentDate", isContentDate)
                sqlCmd.Parameters.AddWithValue("@isPictureHover", isPictureHover)
                sqlCmd.Parameters.AddWithValue("@isImageSlideshow", isImageSlideshow)
                sqlCmd.Parameters.AddWithValue("@isContentPubDate", isContentPubDate)
                sqlCmd.Parameters.AddWithValue("@isExpiredDate", isExpiredDate)
                sqlCmd.Parameters.AddWithValue("@isComment", isComment)
                sqlCmd.Parameters.AddWithValue("@isCommentPreApproval", isCommentPreApproval)
                sqlCmd.Parameters.AddWithValue("@isForum", isForum)
                sqlCmd.Parameters.AddWithValue("@isForumPreApproval", isForumPreApproval)
                sqlCmd.Parameters.AddWithValue("@isPolling", isPolling)
                sqlCmd.Parameters.AddWithValue("@isNeedApproval", isNeedApproval)
                sqlCmd.Parameters.AddWithValue("@isDisplay1", isDisplay1)
                sqlCmd.Parameters.AddWithValue("@isDisplay2", isDisplay2)
                sqlCmd.Parameters.AddWithValue("@isDisplay3", isDisplay3)
                sqlCmd.Parameters.AddWithValue("@isDisplay4", isDisplay4)
                sqlCmd.Parameters.AddWithValue("@isActive", isActive)
                sqlCmd.Parameters.AddWithValue("@testLink", testLink)
                sqlCmd.Parameters.AddWithValue("@thumbImgW", thumbImgW)
                sqlCmd.Parameters.AddWithValue("@thumbImgH", thumbImgH)
                sqlCmd.Parameters.AddWithValue("@picImgW", picImgW)
                sqlCmd.Parameters.AddWithValue("@picImgH", picImgH)
                sqlCmd.Parameters.AddWithValue("@approvalUser", approvalUser)
                sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)
                sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
                sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)

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

        Public Shared Function updateTag(ByVal domainRef As String, ByVal tagRef As String, ByVal tagRefParent As String, ByVal tagTypeRef As String, ByVal contentDisplayRef As String, _
                                         ByVal tagName As String, ByVal keyword As String, ByVal description As String, ByVal metaDescription As String, _
                                         ByVal sortNo As String, ByVal isSingleContent As String, ByVal isOnlyParent As String, _
                                         ByVal isTitle As String, ByVal isTitleDetail As String, _
                                         ByVal isSynopsis As String, ByVal isContent As String, ByVal isThumbnail As String, _
                                         ByVal isPicture As String, ByVal isVideo As String,ByVal isMap As String, ByVal isAttachment As String, ByVal isContentDate As String, _
                                         ByVal isPictureHover As String, ByVal isImageSlideshow As String, _
                                         ByVal isContentPubDate As String, ByVal isExpiredDate As String, ByVal isComment As String, _
                                         ByVal isCommentPreApproval As String, ByVal isForum As String, ByVal isForumPreApproval As String, _
                                         ByVal isPolling As String, ByVal isNeedApproval As String, _
                                         ByVal isDisplay1 As String, ByVal isDisplay2 As String, ByVal isDisplay3 As String, ByVal isDisplay4 As String, _
                                         ByVal isActive As String, _
                                         ByVal testLink As String, ByVal thumbImgW As String, ByVal thumbImgH As String, _
                                         ByVal picImgW As String, ByVal picImgH As String, ByVal approvalUser As String, ByVal metaTitle As String, ByVal metaAuthor As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = tagRef
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "update	ms_tag " + _
                                "set	    tagRefParent = @tagRefParent, tagTypeRef = @tagTypeRef,contentDisplayRef =@contentDisplayRef, tagName = @tagName " + _
                                "           , keyword = @keyword, description = @description, metaDescription = @metaDescription " + _
                                "           , sortNo = @sortNo, isSingleContent = @isSingleContent, isOnlyParent = @isOnlyParent, isTitle = @isTitle, isTitleDetail = @isTitleDetail " + _
                                "           , isSynopsis = @isSynopsis, isContent = @isContent, isThumbnail = @isThumbnail " + _
                                "           , isPicture = @isPicture, isVideo = @isVideo,isMap =@isMap , isAttachment = @isAttachment, isContentDate = @isContentDate " + _
                                "           , isPictureHover=@isPictureHover, isImageSlideshow= @isImageSlideshow,isContentPubDate = @isContentPubDate, isExpiredDate = @isExpiredDate, isComment = @isComment " + _
                                "           , isCommentPreApproval = @isCommentPreApproval, isForum = @isForum, isForumPreApproval = @isForumPreApproval " + _
                                "           , isPolling = @isPolling, isNeedApproval = @isNeedApproval, isActive = @isActive " + _
                                "           , isDisplay1 = @isDisplay1, isDisplay2 = @isDisplay2, isDisplay3 = @isDisplay3, isDisplay4 = @isDisplay4 " + _
                                "           , testLink = @testLink, thumbImgW = @thumbImgW, thumbImgH = @thumbImgH " + _
                                "           , picImgW = @picImgW, picImgH = @picImgH, approvalUser = @approvalUser, metaTitle = @metaTitle, metaAuthor = @metaAuthor " + _
                                "where	    domainRef = @domainRef and tagRef = @tagRef "


            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)
            sqlCmd.Parameters.AddWithValue("@tagRefParent", tagRefParent)
            sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagTypeRef)
            sqlCmd.Parameters.AddWithValue("@contentDisplayRef", contentDisplayRef)
            sqlCmd.Parameters.AddWithValue("@tagName", tagName)
            sqlCmd.Parameters.AddWithValue("@keyword", keyword)
            sqlCmd.Parameters.AddWithValue("@description", description)
            sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)
            sqlCmd.Parameters.AddWithValue("@sortNo", sortNo)
            sqlCmd.Parameters.AddWithValue("@isSingleContent", isSingleContent)
            sqlCmd.Parameters.AddWithValue("@isOnlyParent", isOnlyParent)
            sqlCmd.Parameters.AddWithValue("@isTitle", isTitle)
            sqlCmd.Parameters.AddWithValue("@isTitleDetail", isTitleDetail)
            sqlCmd.Parameters.AddWithValue("@isSynopsis", isSynopsis)
            sqlCmd.Parameters.AddWithValue("@isContent", isContent)
            sqlCmd.Parameters.AddWithValue("@isThumbnail", isThumbnail)
            sqlCmd.Parameters.AddWithValue("@isPicture", isPicture)
            sqlCmd.Parameters.AddWithValue("@isVideo", isVideo)
            sqlCmd.Parameters.AddWithValue("@isMap", isMap)
            sqlCmd.Parameters.AddWithValue("@isAttachment", isAttachment)
            sqlCmd.Parameters.AddWithValue("@isContentDate", isContentDate)
            sqlCmd.Parameters.AddWithValue("@isPictureHover", isPictureHover)
            sqlCmd.Parameters.AddWithValue("@isImageSlideshow", isImageSlideshow)
            sqlCmd.Parameters.AddWithValue("@isContentPubDate", isContentPubDate)
            sqlCmd.Parameters.AddWithValue("@isExpiredDate", isExpiredDate)
            sqlCmd.Parameters.AddWithValue("@isComment", isComment)
            sqlCmd.Parameters.AddWithValue("@isCommentPreApproval", isCommentPreApproval)
            sqlCmd.Parameters.AddWithValue("@isForum", isForum)
            sqlCmd.Parameters.AddWithValue("@isForumPreApproval", isForumPreApproval)
            sqlCmd.Parameters.AddWithValue("@isPolling", isPolling)
            sqlCmd.Parameters.AddWithValue("@isNeedApproval", isNeedApproval)
            sqlCmd.Parameters.AddWithValue("@isDisplay1", isDisplay1)
            sqlCmd.Parameters.AddWithValue("@isDisplay2", isDisplay2)
            sqlCmd.Parameters.AddWithValue("@isDisplay3", isDisplay3)
            sqlCmd.Parameters.AddWithValue("@isDisplay4", isDisplay4)
            sqlCmd.Parameters.AddWithValue("@isActive", isActive)
            sqlCmd.Parameters.AddWithValue("@testLink", testLink)
            sqlCmd.Parameters.AddWithValue("@thumbImgW", thumbImgW)
            sqlCmd.Parameters.AddWithValue("@thumbImgH", thumbImgH)
            sqlCmd.Parameters.AddWithValue("@picImgW", picImgW)
            sqlCmd.Parameters.AddWithValue("@picImgH", picImgH)
            sqlCmd.Parameters.AddWithValue("@approvalUser", approvalUser)
            sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
            sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)

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