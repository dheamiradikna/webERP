Imports Microsoft.VisualBasic
Imports [class].clsWebGeneral
Imports System.Data.SqlClient

Namespace [class]
    Public Class clsSecurityDB
        Public Shared Function updateUserPassword(ByVal userRef As String, ByVal domainRef As String, ByVal password As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "update	ms_user " + _
                                "set password = @password " + _
                                "where userRef = @userRef and domainRef = @domainRef "

            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@userRef", userRef)
            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@password", password)

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

