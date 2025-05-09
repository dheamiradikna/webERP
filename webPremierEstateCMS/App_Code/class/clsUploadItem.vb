Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports [class].clsWebGeneral


Namespace [class]
    Public Class clsUploadItem

        Public Shared Function getIMG_TR_imagePic(ByVal domainRef As String, ByVal imgRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	imgFile, imgFileName " + _
                                "from IMG_TR_image " + _
                                "where domainRef = @domainRef and imgRef = @imgRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function updateIMG_TR_image(ByVal domainRef As String, ByVal imgRef As String, _
                                              ByVal title As String, ByVal description As String, _
                                              ByVal keyword As String, ByVal imgW As Integer, _
                                              ByVal imgH As Integer, _
                                              ByVal imgFile As Byte(), ByVal imgFileName As String, _
                                              ByVal inputUN As String) As String

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
                sqlCmd.CommandText = "select	isnull(max(imgRef),0) + 1 as ref " + _
                                    "from IMG_TR_image " + _
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
                sqlCmd.CommandText = "update IMG_TR_image set title = @title, description = @description, keyword = @keyword, imgW = @imgW, imgH = @imgH, imgFile = @imgFile, imgFileName = @imgFileName where domainRef = @domainRef and imgRef = @imgRef "

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)
                sqlCmd.Parameters.AddWithValue("@title", title)
                sqlCmd.Parameters.AddWithValue("@description", description)
                sqlCmd.Parameters.AddWithValue("@keyword", keyword)
                sqlCmd.Parameters.AddWithValue("@imgW", imgW)
                sqlCmd.Parameters.AddWithValue("@imgH", imgH)
                sqlCmd.Parameters.AddWithValue("@imgFile", imgFile)
                sqlCmd.Parameters.AddWithValue("@imgFileName", imgFileName)
                sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)

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

        Public Shared Function insertContentImage(ByVal domainRef As String, ByVal contentRef As String, _
                                                   ByVal imgRef As String, ByVal imgType As String, ByVal sortNo As String) As String
            Dim sqlCmd As New SqlCommand
            Dim sqlCon As New SqlConnection(_conStr)
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "insert	into tr_contentImage " + _
                                "(domainRef, contentRef, imgRef, imgType, sortNo) " + _
                                "values(@domainRef, @contentRef, @imgRef, @imgType, @sortNo) "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgRef", imgRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)
            sqlCmd.Parameters.AddWithValue("@sortNo", sortNo)

            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertIMG_TR_image(ByVal domainRef As String, _
                                              ByVal title As String, ByVal description As String, _
                                              ByVal keyword As String, ByVal imgW As Integer, _
                                              ByVal imgH As Integer, _
                                              ByVal imgFile As Byte(), ByVal imgFileName As String, _
                                              ByVal inputUN As String) As String

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
                sqlCmd.CommandText = "select	isnull(max(imgRef),0) + 1 as ref " + _
                                    "from IMG_TR_image " + _
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
                sqlCmd.CommandText = "insert	into IMG_TR_image " + _
                                    "           (domainRef, imgRef, title, description, keyword, imgW, imgH, imgFile, imgFileName, inputUN) " + _
                                    "values	    (@domainRef, @imgRef, @title, @description, @keyword, @imgW, @imgH, @imgFile, @imgFileName, @inputUN)"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@imgRef", result)
                sqlCmd.Parameters.AddWithValue("@title", title)
                sqlCmd.Parameters.AddWithValue("@description", description)
                sqlCmd.Parameters.AddWithValue("@keyword", keyword)
                sqlCmd.Parameters.AddWithValue("@imgW", imgW)
                sqlCmd.Parameters.AddWithValue("@imgH", imgH)
                sqlCmd.Parameters.AddWithValue("@imgFile", imgFile)
                sqlCmd.Parameters.AddWithValue("@imgFileName", imgFileName)
                sqlCmd.Parameters.AddWithValue("@inputUN", inputUN)

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

        Public Shared Function uploadTagPicture(ByVal domainRef As String, ByVal tagRef As String, _
                                                   ByVal attachFile As Byte(), ByVal attachFileName As String) As String
            Dim sqlCmd As New SqlCommand
            Dim sqlCon As New SqlConnection(_conStr)
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "update ms_tag set tagPicture = @tagPicture, tagPictureFile = @tagPictureFile where domainRef = @domainRef and tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@tagPictureFile", attachFileName)
            sqlCmd.Parameters.AddWithValue("@tagPicture", attachFile)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)


            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getTagPicture(ByVal domainRef As String, ByVal tagRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	tagPicture as imgFile, tagPictureFile as imgFileName " + _
                                "from ms_tag " + _
                                "where domainRef = @domainRef and tagRef = @tagRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)


            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function uploadDomainLogo(ByVal domainRef As String, _
                                                ByVal attachFile As Byte(), ByVal attachFileName As String) As String
            Dim sqlCmd As New SqlCommand
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "update ms_domainSetting set logo = @logo,logoFileName = @logoFileName where domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@logo", attachFile)
            sqlCmd.Parameters.AddWithValue("@logoFileName", attachFileName)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getDomainLogo(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	logo as imgFile, logoFileName as imgFileName " + _
                                "from ms_domainSetting " + _
                                "where domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function uploadDomainImage(ByVal domainRef As String, _
                                                ByVal attachFile As Byte(), ByVal attachFileName As String) As String
            Dim sqlCmd As New SqlCommand
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "update ms_domainSetting set imgFile = @imgFile,imgFileName = @imgFileName where domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@imgFile", attachFile)
            sqlCmd.Parameters.AddWithValue("@imgFileName", attachFileName)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)


            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getDomainImage(ByVal domainRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStrSite)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	imgFile,imgFileName " + _
                                "from ms_domainSetting " + _
                                "where domainRef = @domainRef"

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

    End Class

End Namespace

