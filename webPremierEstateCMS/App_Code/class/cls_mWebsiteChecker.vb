Imports Microsoft.VisualBasic
Imports [class].clsWebGeneral
Imports System.Data.SqlClient

Namespace [class]

Public Class cls_mWebsiteChecker
            Public Shared Function getTagTypeListWebDev(ByVal userRef As String, _
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
            End If
            ''''' order stuff end '''''
            ''''' order stuff end '''''
            ''''' order stuff end '''''

            ''''' any word method ''''
            ''''' any word method ''''
            ''''' any word method ''''
            Dim field() As String = {"userName"}
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

            sqlCmd.CommandText = "select	userRef, userName, email, hp, company, urlWebsite, inputTime " + _
                                "from	    TR_websiteChecker " + _
                                "where 1=1 " + _
                                whereSearch.ToString + strOrder
                                '"where	    userRef = @userRef " + _
                                
            sqlCmd.Parameters.AddWithValue("@userRef", userRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

         Public Shared Function getTagTypeInfo(ByVal userRef As String, ByVal userName As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	userRef, userName " + _
                                "from	    TR_websiteChecker " + _
                                "where      userRef = @userRef "
                                '"where      domainRef = @domainRef and userRef = @userRef "

            sqlCmd.Parameters.AddWithValue("@userRef", userRef)
            'sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getTagTypeInfoHTML(ByVal domainRef As String, ByVal tagTypeRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.Columns.Add("domainRef")
            dt.Columns.Add("tagTypeRef")
            dt.Columns.Add("tagTypeName")

            sqlCmd.CommandText = "select	domainRef, tagTypeRef, tagTypeName " + _
                                "from	    ms_tagType " + _
                                "where      domainRef = @domainRef and tagTypeRef = @tagTypeRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagTypeRef)


            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                dr = dt.NewRow
                dr("domainRef") = sqlDr("domainRef")
                dr("tagTypeRef") = sqlDr("tagTypeRef")
                dr("tagTypeName") = sqlDr("tagTypeName")

                dt.Rows.Add(dr)
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function deleteTagType(ByVal domainRef As String, ByVal tagTypeRef As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "delete	ms_tagType " + _
                                 "where domainRef = @domainRef and tagTypeRef = @tagTypeRef"
            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@tagTypeRef", tagTypeRef)

            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertTagType(ByVal domainRef As String, _
                                              ByVal tagTypeName As String, _
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
                sqlCmd.CommandText = "select	isnull(max(tagTypeRef),0) + 1 as ref " + _
                                    "from ms_tagType " + _
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
                sqlCmd.CommandText = "insert	into ms_tagType " + _
                                    "           (domainRef, tagTypeRef, tagTypeName, inputUN) " + _
                                    "values	    (@domainRef, @tagTypeRef, @tagTypeName, @inputUN)"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@tagTypeRef", result)
                sqlCmd.Parameters.AddWithValue("@tagTypeName", tagTypeName)
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

        Public Shared Function updateTagType(ByVal userRef As String, ByVal userName As String, ByVal txtuserName As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = userName
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "update	TR_websiteChecker " + _
                                "set	userName = @userName " + _
                                "where	userRef = @userRef "
                                '"where	userRef = @userRef and tagTypeRef = @tagTypeRef "


            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@userRef", userRef)
            sqlCmd.Parameters.AddWithValue("@tagTypeRef", userName)
            'sqlCmd.Parameters.AddWithValue("@tagTypeName", tagTypeName)

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