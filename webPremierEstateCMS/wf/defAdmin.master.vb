Imports [class].clsWebGeneral

Partial Class wf_defAdmin
    Inherits System.Web.UI.MasterPage

    Protected rootPath As String = _rootPath

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Session("isAdmin") = "1"
        Session("domainRef") = 1
        Session("domain") = "www.nataproptech.com"
        Session("userName") = "admin@admin.com"
        Session("password") = "admin"
        Session("name") = "Adhi Persada Properti"
    End Sub

    
End Class

