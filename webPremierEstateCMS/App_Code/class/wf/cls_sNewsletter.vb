Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports [class].clsWebGeneral

Namespace [class]
    Public Class cls_sNewsletter
        Public Shared Function getNewsletterList(ByVal domainRef As String, _
                                               ByVal keyword As String, _
                                               ByVal sortBy As String, ByVal sortType As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
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
            Else 
                strOrder = " order	by inputTime asc "
            End If
            ''''' order stuff end '''''
            ''''' order stuff end '''''
            ''''' order stuff end '''''

            ''''' any word method ''''
            ''''' any word method ''''
            ''''' any word method ''''
            Dim field() As String = {"Email"}
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

            sqlCmd.CommandText = "select	Email, inputTime " + _
                                "from	    TR_feedback " + _
                                whereSearch.ToString + strOrder

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function deleteNewsletter(ByVal domainRef As String, ByVal Email As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "delete	tr_subscribe " + _
                                 "where domainRef = @domainRef and Email = @Email"
            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@Email", Email)

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
