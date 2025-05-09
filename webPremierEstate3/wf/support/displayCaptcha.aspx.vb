
Imports System.Data
Imports System.Drawing
Imports [class].clsGeneralDB
Imports [class].clsWebGeneral

Partial Class wf_support_displayCaptcha
    Inherits System.Web.UI.Page

    Private Sub wf_support_displayCaptcha_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ipAddress As String = HttpContext.Current.Request.UserHostAddress

        Dim path As String = ""
        path = HttpContext.Current.Server.MapPath("~\support\img\base-captcha-2.png")
        Dim bmp = Bitmap.FromFile(path)
        Dim newImage = New Bitmap(bmp.Width - 80, bmp.Height - 20)

        Dim gr = Graphics.FromImage(newImage)
        gr.DrawImageUnscaled(bmp, 0, 0)


        Dim font As Font = New Font("Arial", 50)
        Dim strFont As String = ""

        Dim dtCaptcha As New DataTable
        dtCaptcha = GetCaptchaTop(Trim(ipAddress))

        If dtCaptcha.Rows.Count > 0
            Dim strTemp As String = dtCaptcha.Rows(0).Item("captcha")
            For i = 0 To strTemp.Length - 1
                strFont += strTemp.Substring(i, 1)

                If i < (strTemp.Length - 1)
                    strFont += " "
                End If
            Next
        Else
            strFont = RandomString()
            insertNewCaptcha(ipAddress, strFont, _domainName)
        End If
        

        gr.DrawString(strFont, font, Brushes.Black, New RectangleF(0, 5, bmp.Width, 0))

        'newImage.Save(HttpContext.Current.Server.MapPath("~\data\image\base-captcha-2.png"))

        Response.ContentType = "image/jpeg"
        Response.Cache.SetMaxAge(TimeSpan.Zero)
        Response.Cache.SetCacheability(HttpCacheability.Public)
        Response.Cache.SetSlidingExpiration(True)
        newImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
    End Sub

    Private Function RandomString() As String
        Dim r As Random = New Random
        Dim s As String = "ABCDEFGHJKLMNPQRSTUVWXYZ" + "23456789"

        Dim sb As New StringBuilder
        For i As Integer = 1 To 5
            Dim idx As Integer = r.Next(0, s.Length)
            sb.Append(s.Substring(idx, 1))

            If i < 5
                sb.Append(" ")
            End If
        Next
        Return sb.ToString()
    End Function

End Class
