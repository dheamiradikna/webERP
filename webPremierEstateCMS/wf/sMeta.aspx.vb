Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_sMeta
Imports [class].clsGeneralDB

Partial Class wf_sMeta
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected isUpdate As String = ""
    
    Protected txtMetaTitle As String = ""
    Protected txtMetaAuthor As String = ""
    Protected txtMetaKeyword As String = ""
    Protected txtMetaDescription As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""

        Dim q_ref As String = Request.QueryString("r")
        Dim q_hardcodeRef As String = Request.QueryString("hr")
         If Trim(q_ref) = "" Then
            q_ref = getFirstMetaRef(Session("domainRef").ToString)
        End If
        If IsPostBack Then
            Dim _save As String = Request.Form("_save")
            Dim _delete As String = Request.Form("_delete")

            If Trim(_save) = "1" Then
                txtMetaTitle = Request.Form("txtMetaTitle")
                txtMetaAuthor = Request.Form("txtMetaAuthor")
                txtMetaKeyword = Request.Form("txtMetaKeyword")
                txtMetaDescription = Request.Form("txtMetaDescription")
               
                Dim temp As String = ""
                Dim Hashtable As New Hashtable

                If Trim(q_ref) <> "" Then
                    'update
                    temp = updateMeta(Session("domainRef").ToString, q_ref, txtMetaTitle, txtMetaAuthor, txtMetaKeyword, txtMetaDescription)
                Else
                    'insert
                    temp = insertMeta(Session("domainRef").ToString, txtMetaTitle, txtMetaAuthor, txtMetaKeyword, txtMetaDescription, inputUN)
                End If
                
                
                If Trim(q_ref) <> "" Then
                    'update
                    Hashtable("note") = "Update succeed."
                Else
                    'insert
                    Hashtable("note") = "Insert succeed."
                End If

                Response.Redirect(GetEncUrl(_rootPath + "wf/sMeta.aspx?hr= " + q_hardcodeRef + "&r=" + temp + "&", Hashtable))

            End If

            If Trim(_delete) = "1" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteMeta(Session("domainRef").ToString, q_ref)
                If Trim(temp) = "" Then
                    'benar
                    
                    Hashtable("note") = "Delete succeed."
                    Response.Redirect(GetEncUrl(_rootPath + "wf/sMeta.aspx?hr= " + q_hardcodeRef + "&r=" + temp + "&", Hashtable))

                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:history.go(-1);"">Click here</a> to go back.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notification.aspx?", Hashtable))

                End If
            End If
            
        Else
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''

            'Dim q_tagType As String = ""
            'Dim q_parentTag As String = ""

            If Not Request.Params("x") Is Nothing Then
                Dim param As Hashtable = GetDecParam(Request.Params("x"))
                Dim q_note As String = param("note")

                If Trim(q_note) <> "" Then
                    divNotif.InnerHtml =  "Info: Page Meta ini untuk setting meta secara general (di home)" + _
                                          "<br />" + _
                                          "Notification: " + q_note
                Else
                    divNotif.InnerHtml =  "Info: Page Meta ini untuk setting meta secara general (di home)" + _
                                          "<br />" + _
                                          "Notification: Please fill all data below than click ""save"""
                End If
            End If



            If Trim(q_ref) <> "" Then
                isUpdate = "1"

                divTitleTop.InnerHtml =  " Setting :: Meta [" + Session("domain") + "]"
                Page.Title = "CMS :: " + Session("domain") + " :: Meta"

                Dim dt As New DataTable

                dt = getMetaInfo(Session("domainRef").ToString, q_ref)


                If dt.Rows.Count > 0 Then

                    txtMetaTitle = dt.Rows(0).Item("metaTitle")
                    txtMetaAuthor = dt.Rows(0).Item("metaAuthor")
                    txtMetaKeyword = dt.Rows(0).Item("metaKeyword")
                    txtMetaDescription = dt.Rows(0).Item("metaDescription")
                    
                    Dim Hashtable As New Hashtable
                    Hashtable("isForm") = 1

                    ltrBtn.Text = "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("metaTitle")) + "');"" class=""btn btn-md btn-dark""><span>Delete</span></a> " 
                                                       
                    'ltrBtnTop.Text = "<div class=""linkBtn left mr5""> " + _
                    '                "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("metaTitle")) + "');"">Delete</a> " + _
                    '                "</div> "
                End If

            Else

                divTitleTop.InnerHtml =  " Setting :: Meta [" + Session("domain") + "]"

                Page.Title = "CMS :: " + Session("domain") + " :: Meta"
            End If

        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWeb()
    End Sub

End Class
