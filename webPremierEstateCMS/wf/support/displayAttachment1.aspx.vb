'Imports [class].cls_GeneralInformation
Imports System.Data

Partial Class wf_support_displayAttachment1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imgSrc As String = Request.QueryString("is")

        Dim dt As Byte() = {0}
        Dim dt2 As New DataTable
        Dim dt3 As New DataTable
        Dim vendorFileRef As String = Request.QueryString("vfr")
        Dim dbRef As String = Request.QueryString("dr")
        Dim fileName As String = Request.QueryString("fn")
        Dim extension As String = Request.QueryString("ex")
        Dim DocumentTypePdf As String = ""
        Dim fileExt As String = ""
        Dim attach As Byte() = {0}
        If Trim(fileName) = "" Then
            fileName = "no-name"
        End If

        If Trim(extension) = "" Then
            extension = "jpg"
        End If

        'Select Case imgSrc
        '    Case "npwp"

        '        dt = getVendorNPWPImage(vendorFileRef)
        '        dt3 = getVendorNPWPFile(vendorFileRef)

        '        fileName = dt3.Rows(0).Item("vendorFileName")

        '        Dim ms As New System.IO.MemoryStream(dt)
        '        Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(ms)

        '        Response.ContentType = "image/jpeg"
        '        image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)

        '        Dim str() As String = Split(dt3.Rows(0).Item("vendorFileExt").ToString)
        '        Dim ext As String = str(0)

        '        If ext = "." Then
        '            fileExt = dt3.Rows(0).Item("vendorFileExt").ToString.Split(".")(1)
        '        Else
        '            fileExt = dt3.Rows(0).Item("vendorFileExt").ToString
        '        End If

        '    Case "taxDoc"

        '        dt = getVendorTaxImage(vendorFileRef)
        '        dt3 = getVendorTaxDoc(vendorFileRef)

        '        fileName = dt3.Rows(0).Item("vendorFileName")

        '        Dim ms As New System.IO.MemoryStream(dt)
        '        Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(ms)

        '        Response.ContentType = "image/jpeg"
        '        image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)

        '        Dim str() As String = Split(dt3.Rows(0).Item("vendorFileExt").ToString)
        '        Dim ext As String = str(0)

        '        If ext = "." Then
        '            fileExt = dt3.Rows(0).Item("vendorFileExt").ToString.Split(".")(1)
        '        Else
        '            fileExt = dt3.Rows(0).Item("vendorFileExt").ToString
        '        End If

        '    Case "officeDoc"

        '        dt = getVendorOfficeImage(vendorFileRef)
        '        dt3 = getVendorOfficeInfo(vendorFileRef)

        '        If dt3.Rows.Count > 0 Then

        '            attach = dt3.Rows(0).Item("attachFile")
                    
        '            If attach.Length > 0 Then
        '                fileName = dt3.Rows(0).Item("fileName")
        '                fileExt = dt3.Rows(0).Item("fileExt")


        '                Try
        '                        Select Case fileExt.ToLower
        '                            Case "doc"
        '                                Response.Clear()
        '                                Response.ContentType = "Application/ms-word"
        '                            Case "docx"
        '                                Response.Clear()
        '                                Response.ContentType = "Application/ms-word"
        '                            Case "xls"
        '                                Response.Clear() 
        '                                Response.ContentType = "Application/ms-excel"
        '                            Case "xlsx"
        '                                Response.Clear()
        '                                Response.ContentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        '                            Case "pdf"
        '                                Response.Clear()
        '                                Response.ContentType = "Application/" + fileExt
        '                            Case Else
        '                                Response.Clear()
        '                                Response.ContentType = "Application/" + fileExt
        '                        End Select


        '                        Response.AddHeader("Content-disposition", _
        '                        "attachment; filename=" & fileName + "." + fileExt)

        '                        Response.OutputStream.Write(attach, 0, attach.Length)
               
        '                        Response.End()
        '                Catch ex As Exception
        '                        Response.Write("Error : " & ex.Message)
        '                Finally

        '                End Try
        '            End If
                    
        '        End If
        '    Case ""
        '        Response.Clear()
        '        Response.ContentType = "Application/json"
        '        Response.Write("{}")
        '        Response.End
        'End Select

    End Sub

End Class
