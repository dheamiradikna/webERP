Imports [class].clsWebGeneral

Partial Class admin_default
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect(rootPath + "wf/admin/default.aspx")
    End Sub
End Class
