
Partial Class wf_test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim f As HttpPostedFile

        f = Request.Files.Get(0)
        'f.SaveAs(Server.MapPath("~") + "\test" + Format(Now, "HHmmssnnn") + ".jpg")
        f.SaveAs(Server.MapPath("~") + "\" + f.FileName)

    End Sub
End Class
