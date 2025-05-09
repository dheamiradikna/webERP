Imports [class].clsGeneral

Partial Class wf_notificationPopup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Notification"

        If Not Request.Params("x") Is Nothing Then
            Dim param As Hashtable = GetDecParam(Request.Params("x"))
            Dim q_note As String = param("note")

            If Trim(q_note) <> "" Then
                divNotif.InnerHtml = "Notification: " + q_note
            End If
        End If

    End Sub

End Class
