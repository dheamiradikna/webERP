Imports System.Data
Imports [class].cls_wmContent
Imports [class].clsGeneral
Imports [class].clsGeneralDB
Imports [class].clsWebGeneral
Imports [class].clsContentDB


Partial Class wf_displayAttachmentDownload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim q_domainRef As String = Request.QueryString("dr")
        Dim q_contentRef As String = Request.QueryString("cr")
        Dim q_dbMasterRef As String = Request.QueryString("d")
        Dim q_projectRef As String = Request.QueryString("pr")
        Dim q_fileRef As String = Request.QueryString("fr")
        Dim q_attachRef As String = Request.QueryString("ar")
        Dim q_tagRef As String = Request.QueryString("t")
        Dim q_x As String = Request.Params("x")
        Dim q_extension As String = Request.QueryString("ex")

        Dim result As New StringBuilder

        Dim attach As Byte() = {0}
        Dim fileName As String = ""
        Dim ext As String = ""

        Dim dt As New DataTable
        Dim offset As Integer = 0

        If Trim(q_attachRef) <> "" Or q_attachRef Is Nothing Then
            If q_dbMasterRef = "" Then
                q_attachRef = getContentFirstAttachRef(q_domainRef, q_contentRef)
            End If
        End If

        Dim logID As String = ""
        Dim Email As String = ""
        Dim dtLog As New DataTable
        If Trim(q_x) <> "" Then
            Dim param As Hashtable = GetDecParam(q_x)
            logID = param("id")
            Email = param("email")
        End If

        If Trim(logID) <> "" Then

        Else
            dt = getContentAttachmentFile(q_domainRef, q_contentRef, q_attachRef)
        End If
        
            If dt.Rows.Count > 0 Then
                attach = dt.Rows(0).Item("attachFile")
                fileName = dt.Rows(0).Item("attachFN")

            Dim fileExt As String = ""
                Dim index As Integer
            If q_dbMasterRef <> "" Then
                fileExt = Split(ext, ".")(1).ToString
                ext = dt.Rows(0).Item("extension")
            Else
                index = StrReverse(fileName + q_extension).IndexOf(".")
                fileExt = Right(fileName, index)
            End If



            Try
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
                            Response.ContentType = "Application/" + fileExt
                            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf")
                            Response.BufferOutput = True
                            Response.AddHeader("Content-Length", attach.Length.ToString())
                            Response.BinaryWrite(attach)
                            Response.End()
                        Case "mp4"
                            Response.ContentType = "Application/video"
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
