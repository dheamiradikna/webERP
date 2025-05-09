Imports Microsoft.VisualBasic
Imports [class].clsWebGeneral
Imports System.Data.SqlClient


Namespace [class]



    Public Class cls_mUser
        Public Shared Function getUserList(ByVal domainRef As String, _
                                                 ByVal userStatus As String, ByVal keyword As String, _
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
            Dim field() As String = {"email", "name", "hp", "phone", "userStatusName"}
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

            sqlCmd.CommandText = "select	u.domainRef, u.userRef, u.email, u.password, u.name, u.hp, u.phone, u.inputTime, u.inputUN " + _
                                "           , u.userStatus, l.userStatusName " + _
                                "from	    ms_user u, lk_userStatus l " + _
                                "where	    u.userStatus = l.userStatus " + _
                                "           and domainRef = @domainRef " + _
                                "           and u.userStatus = @userStatus  " + _
                                whereSearch.ToString + strOrder

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@userStatus", userStatus)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getUserInfo(ByVal domainRef As String, ByVal userRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "select	u.domainRef, u.userRef, u.email, u.password, u.name, u.hp, u.phone, u.inputTime, u.inputUN " + _
                                "           , u.userStatus, l.userStatusName " + _
                                "from	    ms_user u, lk_userStatus l " + _
                                "where      u.userStatus = l.userStatus " + _
                                "           and u.domainRef = @domainRef and u.userRef = @userRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@userRef", userRef)

            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dt)

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function getUserInfoHTML(ByVal domainRef As String, ByVal userRef As String) As DataTable
            Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dr As DataRow
            Dim dt As New DataTable

            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text

            dt.Columns.Add("domainRef")
            dt.Columns.Add("userRef")
            dt.Columns.Add("email")
            dt.Columns.Add("password")
            dt.Columns.Add("name")
            dt.Columns.Add("HP")
            dt.Columns.Add("phone")
            dt.Columns.Add("userStatusName")

            sqlCmd.CommandText = "select	u.domainRef, u.userRef, u.email, u.password, u.name, u.hp, u.phone, u.inputTime, u.inputUN " + _
                                "           , u.userStatus, l.userStatusName " + _
                                "from	    ms_user u, lk_userStatus l " + _
                                "where      u.userStatus = l.userStatus " + _
                                "           and u.domainRef = @domainRef and u.userRef = @userRef "

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@userRef", userRef)


            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                dr = dt.NewRow
                dr("domainRef") = sqlDr("domainRef")
                dr("userRef") = sqlDr("userRef")
                dr("email") = sqlDr("email")
                dr("password") = sqlDr("password")
                dr("name") = sqlDr("name")
                dr("hp") = sqlDr("HP")
                dr("phone") = sqlDr("phone")
                dr("userStatusName") = sqlDr("userStatusName")

                dt.Rows.Add(dr)
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
        End Function

        Public Shared Function deleteUser(ByVal domainRef As String, ByVal userRef As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = ""
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "delete	ms_user " + _
                                 "where domainRef = @domainRef and userRef = @userRef"
            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@userRef", userRef)

            Try
                sqlCmd.ExecuteNonQuery()
            Catch ex As Exception
                result = ex.Message
            End Try

            sqlCmd = Nothing
            sqlCon.Close()

            Return result
        End Function

        Public Shared Function insertUser(ByVal domainRef As String, _
                                              ByVal email As String, ByVal password As String, _
                                              ByVal name As String, ByVal hp As String, _
                                              ByVal phone As String, ByVal userStatus As String, _
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
                sqlCmd.CommandText = "select	isnull(max(userRef),0) + 1 as ref " + _
                                    "from ms_user " + _
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
                sqlCmd.CommandText = "insert	into ms_user " + _
                                    "           (domainRef, userRef, email, password, name, hp, phone, userStatus, inputUN) " + _
                                    "values	    (@domainRef, @userRef, @email, @password, @name, @hp, @phone, @userStatus, @inputUN)"

                sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
                sqlCmd.Parameters.AddWithValue("@userRef", result)
                sqlCmd.Parameters.AddWithValue("@email", email)
                sqlCmd.Parameters.AddWithValue("@password", password)
                sqlCmd.Parameters.AddWithValue("@name", name)
                sqlCmd.Parameters.AddWithValue("@hp", hp)
                sqlCmd.Parameters.AddWithValue("@phone", phone)
                sqlCmd.Parameters.AddWithValue("@userStatus", userStatus)
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

        Public Shared Function updateUser(ByVal domainRef As String, ByVal userRef As String, _
                                              ByVal email As String, ByVal password As String, _
                                              ByVal name As String, ByVal hp As String, _
                                              ByVal phone As String, ByVal userStatus As String) As String
            Dim sqlCmd As New SqlCommand
            Dim result As String = userRef
            Dim sqlCon As New SqlConnection(_conStr)

            sqlCon.Open()

            sqlCmd.CommandType = CommandType.Text

            sqlCmd.CommandText = "update	ms_user " + _
                                "set	email = @email, password = @password, name = @name " + _
                                ", hp = @hp, phone = @phone, userStatus = @userStatus " + _
                                "where	domainRef = @domainRef and userRef = @userRef "



            sqlCmd.Connection = sqlCon

            sqlCmd.Parameters.AddWithValue("@domainRef", domainRef)
            sqlCmd.Parameters.AddWithValue("@userRef", userRef)
            sqlCmd.Parameters.AddWithValue("@email", email)
            sqlCmd.Parameters.AddWithValue("@password", password)
            sqlCmd.Parameters.AddWithValue("@name", name)
            sqlCmd.Parameters.AddWithValue("@hp", hp)
            sqlCmd.Parameters.AddWithValue("@phone", phone)
            sqlCmd.Parameters.AddWithValue("@userStatus", userStatus)

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