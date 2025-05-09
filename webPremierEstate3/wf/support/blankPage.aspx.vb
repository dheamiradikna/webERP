Partial Class wf_support_blankPage
    Inherits System.Web.UI.Page

    Private Sub bindBlankPage()
        Dim result As New StringBuilder()
        Dim context As HttpContext = HttpContext.Current
        Dim bca As HttpBrowserCapabilities = context.Request.Browser

        If bca.IsMobileDevice Then
            result.Append("     <div>")
            result.Append("         <img src=""../../support/img/mociphone.png"" style=""width: 102%;"" />")
            result.Append("     </div>")
        Else
            result.Append("     <div>")
            result.Append("         <img src=""../../support/img/moci.png"" />")
            result.Append("     </div>")
        End If

        ltrBlankPage.Text = result.ToString

    End Sub

    Private Sub wf_support_blankPage_Load(sender As Object, e As EventArgs) Handles Me.Load

        If IsPostBack Then

        Else
            bindBlankPage()
        End If

    End Sub
End Class