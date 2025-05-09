Imports [class].clsGeneral

Partial Class wf_admin_index
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWebAdmin()
    End Sub
End Class
