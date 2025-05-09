Imports [class].clsWebGeneral

Partial Class wf_logout
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.RemoveAll()
        Session.RemoveAll()

        Response.Redirect(rootPath + "default.aspx")
    End Sub
End Class
