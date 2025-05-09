
Imports System.Data
Imports System.Data.SqlClient
Imports [class].clsWebGeneral

Partial Class uploadImageReturn
    Inherits System.Web.UI.Page

    Private Sub uploadImageReturn_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim id = Request.QueryString("id")


        Dim dt As Byte() = {0}
        Dim attach As Byte() = {0}

        dt = getNicImage(id)

        Dim ms As New System.IO.MemoryStream(dt)
        Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(ms)

        Response.ContentType = "image/jpeg"
        image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
    End Sub

    Private Function getNicImage(s As String) As Byte()
         Dim sqlCon As New SqlConnection(_conStr)
            Dim sqlCmd As New SqlCommand
            Dim sqlDr As SqlDataReader
            Dim dt As Byte() = {0}


            sqlCon.Open()
            sqlCmd.Connection = sqlCon
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.CommandText = "select imageData " + _
                                 "from TR_nicImage " + _
                                 "where imageId = @imageId "


            sqlCmd.Parameters.AddWithValue("@imageId", s)

            sqlDr = sqlCmd.ExecuteReader

            If sqlDr.Read Then
                dt = sqlDr("imageData")
            End If
            sqlDr.Close()

            sqlCmd = Nothing
            sqlCon.Close()

            Return dt
    End Function
End Class
