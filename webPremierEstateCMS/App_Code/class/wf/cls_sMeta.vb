Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports System.IO
Imports Aspose.Pdf
Imports Aspose.Pdf.Devices
Imports Aspose.Pdf.Generator


Namespace [class]
    Public Class cls_sMeta
     
        Public Shared Function getFirstMetaRef(ByVal domainRef As String) As String
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As String = ""

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select	top 1 metaRef from ms_meta where domainRef = @domainRef"


            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("metaRef").ToString
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function getMetaInfo(ByVal domainRef As String, ByVal metaRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select * " + _
                                 "from	ms_meta " + _
                                 "where      domainRef = @domainRef and metaRef = @metaRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function deleteMeta(ByVal domainRef As String, ByVal metaRef As String) As String

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
                sqlCmd.CommandText = "delete	ms_meta " + _
                                     "where domainRef = @domainRef and metaRef = @metaRef"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)

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

        Public Shared Function insertMeta(ByVal domainRef As String, ByVal metaTitle As String, ByVal metaAuthor As String, _
                                          ByVal metaKeyword As String, ByVal metaDescription As String, ByVal inputUN As String) As String

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
                sqlCmd.CommandText = "select	isnull(max(metaRef),0) + 1 as ref " + _
                                     "from ms_meta " + _
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
                sqlCmd.CommandText = "insert	into ms_meta " + _
                                    "           (domainRef, metaRef, metaTitle, metaAuthor, metaKeyword, metaDescription, inputUN ) " + _
                                    "values	    (@domainRef, @metaRef, @metaTitle, @metaAuthor, @metaKeyword, @metaDescription, @inputUN )"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@metaRef", result)
                sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
                sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)
                sqlCmd.Parameters.AddWithValue("@metaKeyword", metaKeyword)
                sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)
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
        
        Public Shared Function updateMeta(ByVal domainRef As String, ByVal metaRef As String, ByVal metaTitle As String, _
                                          ByVal metaAuthor As String, ByVal metaKeyword As String, ByVal metaDescription As String) As String

            Dim sqlCmd As New SqlCommand
            Dim result As String = metaRef
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlTrans As SqlTransaction

            sqlCon.Open()
            sqlTrans = sqlCon.BeginTransaction

            sqlCmd.Transaction = sqlTrans
            sqlCmd.Connection = sqlCon
                          
            Try

                sqlCmd.CommandType = CommandType.Text
                sqlCmd.CommandText = "update	ms_meta " + _
                                     "set	    metaTitle = @metaTitle, metaAuthor = @metaAuthor " + _
                                     "          , metaKeyword = @metaKeyword, metaDescription = @metaDescription " + _
                                     "where	    domainRef = @domainRef and metaRef = @metaRef "

                sqlCmd.Connection = sqlCon

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@metaRef", metaRef)
                sqlCmd.Parameters.AddWithValue("@metaTitle", metaTitle)
                sqlCmd.Parameters.AddWithValue("@metaAuthor", metaAuthor)
                sqlCmd.Parameters.AddWithValue("@metaKeyword", metaKeyword)
                sqlCmd.Parameters.AddWithValue("@metaDescription", metaDescription)

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

    End Class

End Namespace