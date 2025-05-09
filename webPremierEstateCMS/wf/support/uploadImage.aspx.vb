
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Newtonsoft.Json
Imports [class].clsWebGeneral

Partial Class uploadImage
    Inherits System.Web.UI.Page

    Private Sub uploadImage_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim picFileDataStream As Stream = Request.Files("image").InputStream
        Dim picFileLength As Integer = Request.Files("image").ContentLength
        Dim picFileData(picFileLength) As Byte
        Dim picFileType As String = Request.Files("image").ContentType
        Dim picFileExt As String
        Dim picFileName As String
        Dim tempFileName As String = Request.Files("image").FileName
        Dim index As Integer
        Dim temp As String = ""


        If picFileLength > 0 Then
            picFileDataStream.Read(picFileData, 0, picFileLength)
            Dim width As Integer = System.Drawing.Image.FromStream(picFileDataStream).PhysicalDimension.Width
            Dim height As Integer = System.Drawing.Image.FromStream(picFileDataStream).PhysicalDimension.Height

            index = StrReverse(tempFileName).IndexOf(".")
            picFileExt = Right(tempFileName, index)
            index = StrReverse(tempFileName).IndexOf("\")
            If index = -1 Then
                picFileName = tempFileName
            Else
                picFileName = Right(tempFileName, index)
            End If

            'batasi max 30 char
            picFileName = Right(picFileName, 50)

            Dim picFileDataResize() As Byte
            Dim newW As Integer = 0
            Dim newH As Integer = 0

            temp = InsertNicImage(picFileData, picFileName,picFileType , width, height)
            If IsNumeric(temp) Then
                Dim jsonObject = new JsonResult()
                jsonObject.Link = _rootPath + "wf/support/uploadImageReturn.aspx?id=" + temp.ToString()
                jsonObject.width = width
                Dim jsonString = JsonConvert.SerializeObject(jsonObject)

                Response.Write(jsonString.ToString())

            End If
        End If

    End Sub

    Private Function InsertNicImage(picFileDataResize As Byte(), picFileName As String, byval picFileType As string, width As Integer, height As Integer) As String
        Dim sqlCmd As New SqlCommand
        Dim result As String = ""
        Dim sqlCon As New SqlConnection(_constr)
        Dim sqlTrans As SqlTransaction
        Dim sqlDr As SqlDataReader

        sqlCon.Open()
        sqlTrans = sqlCon.BeginTransaction

        sqlCmd.Transaction = sqlTrans
        sqlCmd.Connection = sqlCon

        Try
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "Select	isnull(max(imageId),0) + 1 As ref " + _
                                 "FROM TR_nicImage "

            sqlDr = sqlCmd.ExecuteReader
            If sqlDr.Read Then
                result = sqlDr("ref")
            End If
            sqlDr.Close()


            sqlCmd.Parameters.Clear()
            sqlCmd.Prepare()

            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "INSERT INTO TR_nicImage " + _
                                "           (imageId,imageName,imageData,width,height,contentType) " + _
                                " VALUES	    (@imageId,@imageName,@imageData,@width,@height,@contentType) "

            sqlCmd.Parameters.AddWithValue("@imageId", result)
            sqlCmd.Parameters.AddWithValue("@imageName", picFileName)
            sqlCmd.Parameters.AddWithValue("@imageData", picFileDataResize)
            sqlCmd.Parameters.AddWithValue("@width", width)
            sqlCmd.Parameters.AddWithValue("@height", height)
            sqlCmd.Parameters.AddWithValue("@contentType", picFileType)


            sqlCmd.ExecuteNonQuery()


            sqlTrans.Commit()
        Catch mDeveloper As Exception
            sqlTrans.Rollback()
            result = mDeveloper.Message
        End Try

        sqlCmd = Nothing
        sqlCon.Close()

        Return result
    End Function

    Public Class JsonResult
        Public link As String
        Public width as Integer 
    End Class
End Class



