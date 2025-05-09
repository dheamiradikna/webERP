Imports [class].cls_wmContent
Imports [class].clsWebGeneral

Partial Class wf_support_displayAttachment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim q_domainRef As String = Request.QueryString("dr")
        Dim q_contentRef As String = Request.QueryString("cr")
        Dim q_attachRef As String = Request.QueryString("ar")

        Dim attach As Byte() = {0}
        Dim fileName As String = ""

        Dim dt As New DataTable
        Dim tagRef As String
        Dim offset As Integer = 0

        If Trim(q_attachRef) = "0" Then
            q_attachRef = getContentFirstAttachRef(q_domainRef, q_contentRef)
        End If

        dt = getContentAttachmentFile(q_domainRef, q_contentRef, q_attachRef)
        tagRef = getTagRefByContentRef(q_domainRef, q_contentRef)

        If dt.Rows.Count > 0 Then
            attach = dt.Rows(0).Item("attachFile")
            fileName = dt.Rows(0).Item("attachFN")

            Dim fileExt As String = ""
            Dim index As Integer = StrReverse(fileName).IndexOf(".")
            fileExt = Right(fileName, index)

            Try
                'iStream = New System.IO.MemoryStream(attach, offset, attach.Length)
                Select Case fileExt.ToLower
                    Case "doc"
                        Response.ContentType = "Application/ms-word"
                    Case "docx"
                        Response.ContentType = "Application/ms-word"
                    Case "xls"
                        Response.ContentType = "Application/ms-excel"
                    Case "xlsx"
                        Response.ContentType = "Application/ms-excel"
                    Case "pdf"
                        'If tagRef = _tagRefManualBook Then
                        '    Response.ContentType = "Application/" + fileExt
                        '    Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf")
                        '    Response.BufferOutput = True
                        '    Response.AddHeader("Content-Length", attach.Length.ToString())
                        '    Response.BinaryWrite(attach)
                        '    Response.End()
                        'Else
                            Response.ContentType = "Application/" + fileExt
                        'End If
                       
                    Case Else
                        Response.ContentType = "Application/" + fileExt
                End Select


                Response.AddHeader("Content-disposition", _
                  "attachment; filename=" & fileName)

                Response.OutputStream.Write(attach, 0, attach.Length)

                Response.End()
            Catch ex As Exception
                Response.Write("Error : " & ex.Message)
            Finally

            End Try

        End If
    End Sub

End Class
