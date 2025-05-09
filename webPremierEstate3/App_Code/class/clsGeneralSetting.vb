Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports [class].clsWebGeneral

Namespace [class]
    Public Class clsGeneralSetting


        Public Shared Function isContentEnabledPublishDate(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "SELECT isContentPubDate from ms_tag " + _
                                 "WHERE domainRef = @domainRef " + _
                                 "AND tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                If sqlDr("isContentPubDate") Then
                    result = True
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()
            Return result
        End Function

        Public Shared Function isContentEnabledExpiredDate(ByVal domainRef As String, ByVal tagRef As String, ByVal isChildSite As Boolean) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim result As Boolean = False

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "SELECT isExpiredDate from ms_tag " + _
                                 "WHERE domainRef = @domainRef " + _
                                 "AND tagRef = @tagRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagRef", tagRef)

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                If sqlDr("isExpiredDate") Then
                    result = True
                End If
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()
            Return result
        End Function

        Public Shared Function isContentHaveImage(ByVal domainRef As String, ByVal contentRef As String, ByVal imgType As String, ByVal isMicroSite As Boolean) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "SELECT COUNT(contentRef) AS TOTAL FROM TR_contentImage A " + _
                                 "WHERE A.domainRef = @domainRef " + _
                                 "AND A.contentRef = @contentRef " + _
                                 "AND imgType = @imgType "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            If dt.Rows(0).Item("TOTAL") > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function isContentHaveVideo(ByVal domainRef As String, ByVal contentRef As String, ByVal isMicroSite As Boolean) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "SELECT COUNT(contentRef) AS TOTAL FROM TR_content " + _
                                 "WHERE domainRef = @domainRef " + _
                                 "AND contentRef = @contentRef " + _
                                 "AND embedVideo <> '' "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)
            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            If dt.Rows(0).Item("TOTAL") > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function isContentHaveImageLatestNews(ByVal domainRef As String, ByVal contentRef As String, ByVal imgType As String, ByVal isMicroSite As Boolean) As Boolean
            Dim connectionString As String = ""

            connectionString = _conStr

            Dim sqlCon As New SqlConnection(connectionString)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "SELECT COUNT(contentRef) AS TOTAL FROM TR_contentImage A " + _
                                 "WHERE A.domainRef = @domainRef " + _
                                 "AND A.contentRef = @contentRef " + _
                                 "AND imgType = @imgType "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@contentRef", contentRef)
            sqlCmd.Parameters.AddWithValue("@imgType", imgType)

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

    End Class
End Namespace

