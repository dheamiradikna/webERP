Imports [class].clsWebGeneral
Imports [class].clsGeneralDB
Imports System.Data
Imports System.Web.Optimization
Imports System.Web

Partial Class wf_parent
    Inherits System.Web.UI.MasterPage

    Protected rootPath As String = _rootPath


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/bootstrap.min.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/owl.carousel.min.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/owl.theme.default.min.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/fonts/flaticon.css"" as=""fonts"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/boxicons.min.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/animate.min.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/magnific-popup.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/meanmenu.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/nice-select.min.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/style.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""stylesheet preload prefetch"" href=""" + rootPath + "Support/css/responsive.css"" as=""style"" media=""all"" />"))
          Page.Header.Controls.Add(New LiteralControl("<link rel=""icon"" href=""" + rootPath + "Support/img/favicon.png"" type=""image/png"" />"))

    End Sub

End Class

