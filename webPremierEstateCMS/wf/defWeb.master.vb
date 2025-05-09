Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports [class].clsGeneral

Partial Class wf_defWeb
    Inherits System.Web.UI.MasterPage

    Protected rootPath As String = _rootPath

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
       
        If HttpContext.Current.Session("domainRef") Is Nothing Then
            'jika tidak ada session cek cookisnya
            If HttpContext.Current.Request.Browser.Cookies Then
                'jika browsernya acccept cookies
                If Not HttpContext.Current.Request.Cookies("domainRef") Is Nothing Then
                    'Response.Write(HttpContext.Current.Response.Cookies("projectRef").Value + "x")
                    'Exit Sub

                    Session("userName") = Request.Cookies("userName").Value  'cookies.Item("userName")
                    Session("password") = Request.Cookies("password").Value  'cookies.Item("password")
                    Session("name") = Request.Cookies("name").Value  'cookies.Item("name")
                    Session("domainRef") = Request.Cookies("domainRef").Value  'cookies.Item("domainRef")

                End If
            End If
        Else
            '' ini harusnya set cookies, tapi kalo ditaruh sini, nanti terus2 an diinisiate jadi berat
        End If

        cekIsNotLoginWeb()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As New DataTable
        dt = getDomainSetting(Session("domainRef").ToString)
        If dt.Rows.Count > 0 Then
            _imageBgColor = dt.Rows(0).Item("imageBgColor")
        End If
    End Sub
End Class

