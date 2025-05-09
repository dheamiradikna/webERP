Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_sTag
Imports [class].clsGeneralDB
Imports System.Data

Partial Class wf_sTagPopup
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected selTagType As String = ""
    Protected selParentTag As String = ""
    Protected txtTagName As String = ""
    Protected txtMetaTitle As String = ""
    Protected txtMetaAuthor As String = ""
    Protected txtKeyword As String = ""
    Protected txtDescription As String = ""
    Protected txtMetaDescription As String = ""
    Protected txtSortNo As String = "0"
    Protected txtTestLink As String = ""
    Protected txtThumbW As String = "0"
    Protected txtThumbH As String = "0"
    Protected txtPicW As String = "0"
    Protected txtPicH As String = "0"
    Protected txtApprovalUser As String = ""
    Protected ckActive As String = ""

    Protected ckIsSingleContent As String = ""
    Protected ckIsOnlyParent As String = ""
    Protected ckIsTitle As String = ""
    Protected ckIsTitleDetail As String = ""
    Protected ckIsSynopsis As String = ""
    Protected ckIsContent As String = ""
    Protected ckIsThumbnail As String = ""
    Protected ckIsPicture As String = ""
    Protected ckIsVideo As String = ""
    Protected ckIsMap As String = ""
    Protected ckIsAttachment As String = ""
    Protected ckIsDate As String = ""

    Protected ckIsImageSlideShow As String = ""
    Protected ckIsPictureHover As String = ""

    Protected ckIsPublishDate As String = ""
    Protected ckIsExpiredDate As String = ""
    Protected ckIsComment As String = ""
    Protected ckIsCommentPreApproval As String = ""
    Protected ckIsNeedApproval As String = ""
    Protected ckIsPolling As String = ""
    Protected ckIsForum As String = ""
    Protected ckIsForumPreApproval As String = ""

    Protected ckIsDisplay1 As String = ""
    Protected ckIsDisplay2 As String = ""
    Protected ckIsDisplay3 As String = ""
    Protected ckIsDisplay4 As String = ""

    Protected selContentDisplay As String = "0"


    Private Function bindSelTagType(ByVal domainRef As String, ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getTagTypeListLookup(domainRef)
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selTagType"" name=""selTagType"" >")
            
            For i = 0 To dt.Rows.Count - 1
                If value = dt.Rows(i).Item("tagTypeRef").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("tagTypeRef").ToString + """>" + dt.Rows(i).Item("tagTypeName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("tagTypeRef").ToString + """>" + dt.Rows(i).Item("tagTypeName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Function bindSelParent(ByVal domainRef As String, ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        result.Append("<select id=""selParentTag"" name=""selParentTag"" >")
        result.Append("<option value=""0"">No Parent</option>")

        dt = getTagTreeList(domainRef, "", "0", "1", "", "-", "-")
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim tagName As String = ""
                For j = 1 To CInt(dt.Rows(i).Item("level")) - 1
                    tagName = tagName + "&nbsp;&nbsp;&nbsp;"
                Next
                tagName = tagName + dt.Rows(i).Item("tagName")

                If value = dt.Rows(i).Item("tagRef").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("tagRef").ToString + """>" + tagName + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("tagRef").ToString + """>" + tagName + "</option>")
                End If
            Next
        End If

        result.Append("</select> ")

        Return result.ToString
    End Function

    Private Function bindContentDisplay(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getContentDisplayListLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selContentDisplay"" name=""selContentDisplay"" >")

            For i = 0 To dt.Rows.Count - 1
                If value = dt.Rows(i).Item("contentDisplayRef").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("contentDisplayRef").ToString + """>" + dt.Rows(i).Item("contentDisplayName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("contentDisplayRef").ToString + """>" + dt.Rows(i).Item("contentDisplayName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Function bindPicture(ByVal domainRef As String, ByVal tagRef As String) As String
        Dim result As New StringBuilder

        If Trim(tagRef) = "" Then
            result.Append("<div class=""alert alert-info fade in alert-dismissible"">After save, you can insert the picture.</div>")
        Else
            If cekTagFile(domainRef, tagRef) Then
                result.Append("<div><img src=""" + rootPath + "wf/support/displayImage.aspx?m=tag&d=" + domainRef + "&r=" + tagRef + "&w=400""></div>")
                result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?m=tag&t=s&ir=0&r=" + tagRef + """>Click here</a> to change the picture.</div>")
                If tagRef = _contentTagRefBackgroundSlider Then
                    result.Append("<div style=""font-size: 10px;font-weight: bold"">*Max size 500KB</div>")
                Else
                    result.Append("<div style=""font-size: 10px;font-weight: bold"">*Max size 250KB</div>")
                End If
            Else
                result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?m=tag&t=s&ir=0&r=" + tagRef + """>Click here</a> to upload the picture.</div>")
                If tagRef = _contentTagRefBackgroundSlider Then
                    result.Append("<div style=""font-size: 10px;font-weight: bold"">*Max size 500KB</div>")
                Else
                    result.Append("<div style=""font-size: 10px;font-weight: bold"">*Max size 250KB</div>")
                End If
            End If

        End If

        Return result.ToString
    End Function
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""

        Dim q_ref As String = Request.QueryString("r")

        If IsPostBack Then
            Dim _save As String = Request.Form("_save")
            Dim _saveClose As String = Request.Form("_saveClose")
            Dim _delete As String = Request.Form("_delete")

            If Trim(_save) = "1" Or Trim(_saveClose) = "1" Then
                selTagType = Request.Form("selTagType")
                selParentTag = Request.Form("selParentTag")
                txtTagName = Request.Form("txtTagName")
                txtMetaTitle = Request.Form("txtMetaTitle")
                txtMetaAuthor = Request.Form("txtMetaAuthor")
                txtKeyword = Request.Form("txtKeyword")
                txtMetaDescription = Request.Form("txtMetaDescription")
                txtDescription = Request.Form("txtDescription")
                txtSortNo = Request.Form("txtSortNo")

                txtTestLink = Request.Form("txtTestLink")
                txtThumbW = Request.Form("txtThumbW")
                txtThumbH = Request.Form("txtThumbH")
                txtPicW = Request.Form("txtPicW")
                txtPicH = Request.Form("txtPicH")
                txtApprovalUser = Request.Form("txtApprovalUser")
                ckActive = IIf(Request.Form("ckActive") = "on", "1", "0")

                ckIsSingleContent = IIf(Request.Form("ckIsSingleContent") = "on", "1", "0")
                ckIsOnlyParent = IIf(Request.Form("ckIsOnlyParent") = "on", "1", "0")
                ckIsTitle = IIf(Request.Form("ckIsTitle") = "on", "1", "0")
                ckIsTitleDetail = IIf(Request.Form("ckIsTitleDetail") = "on", "1", "0")
                ckIsSynopsis = IIf(Request.Form("ckIsSynopsis") = "on", "1", "0")
                ckIsContent = IIf(Request.Form("ckIsContent") = "on", "1", "0")
                ckIsThumbnail = IIf(Request.Form("ckIsThumbnail") = "on", "1", "0")
                ckIsPicture = IIf(Request.Form("ckIsPicture") = "on", "1", "0")
                ckIsVideo = IIf(Request.Form("ckIsVideo") = "on", "1", "0")
                ckIsMap = IIf(Request.Form("ckIsMap") = "on", "1", "0")
                ckIsAttachment = IIf(Request.Form("ckIsAttachment") = "on", "1", "0")
                ckIsDate = IIf(Request.Form("ckIsDate") = "on", "1", "0")

                ckIsPictureHover = IIf(Request.Form("ckIsPictureHover") = "on", "1", "0")
                ckIsImageSlideShow = IIf(Request.Form("ckIsImageSlideShow") = "on", "1", "0")

                ckIsPublishDate = IIf(Request.Form("ckIsPublishDate") = "on", "1", "0")
                ckIsExpiredDate = IIf(Request.Form("ckIsExpiredDate") = "on", "1", "0")
                ckIsComment = IIf(Request.Form("ckIsComment") = "on", "1", "0")
                ckIsCommentPreApproval = IIf(Request.Form("ckIsCommentPreApproval") = "on", "1", "0")
                ckIsNeedApproval = IIf(Request.Form("ckIsNeedApproval") = "on", "1", "0")
                ckIsPolling = IIf(Request.Form("ckIsPolling") = "on", "1", "0")
                ckIsForum = IIf(Request.Form("ckIsForum") = "on", "1", "0")
                ckIsForumPreApproval = IIf(Request.Form("ckIsForumPreApproval") = "on", "1", "0")

                ckIsDisplay1 = IIf(Request.Form("ckIsDisplay1") = "on", "1", "0")
                ckIsDisplay2 = IIf(Request.Form("ckIsDisplay2") = "on", "1", "0")
                ckIsDisplay3 = IIf(Request.Form("ckIsDisplay3") = "on", "1", "0")
                ckIsDisplay4 = IIf(Request.Form("ckIsDisplay4") = "on", "1", "0")

                selContentDisplay = Request.Form("selContentDisplay")

                If Trim(txtSortNo) = "" Then txtSortNo = "0"
                If Trim(txtPicH) = "" Then txtPicH = "0"
                If Trim(txtPicW) = "" Then txtPicW = "0"
                If Trim(txtThumbH) = "" Then txtThumbH = "0"
                If Trim(txtThumbW) = "" Then txtThumbW = "0"

                Dim temp As String = ""
                Dim Hashtable As New Hashtable

                If Trim(q_ref) <> "" Then
                    'update
                    temp = updateTag(Session("domainRef").ToString, q_ref, selParentTag, selTagType, selContentDisplay, txtTagName, txtKeyword, txtDescription, txtMetaDescription, _
                                     txtSortNo, ckIsSingleContent, ckIsOnlyParent, ckIsTitle, ckIsTitleDetail, ckIsSynopsis, ckIsContent, ckIsThumbnail, ckIsPicture, ckIsVideo,ckIsMap ,ckIsAttachment, ckIsDate, _
                                     ckIsPictureHover, ckIsImageSlideShow, ckIsPublishDate, ckIsExpiredDate, ckIsComment, ckIsCommentPreApproval, ckIsForum, _
                                     ckIsForumPreApproval, ckIsPolling, ckIsNeedApproval, ckIsDisplay1, ckIsDisplay2, ckIsDisplay3, ckIsDisplay4, ckActive, txtTestLink, txtThumbW, txtThumbH, _
                                     txtPicW, txtPicH, txtApprovalUser, txtMetaTitle, txtMetaAuthor)
                Else
                    'insert
                    temp = insertTag(Session("domainRef").ToString, selParentTag, selTagType, selContentDisplay, txtTagName, txtKeyword, txtDescription, txtMetaDescription, _
                                     txtSortNo, ckIsSingleContent, ckIsOnlyParent, ckIsTitle, ckIsTitleDetail, ckIsSynopsis, ckIsContent, ckIsThumbnail, ckIsPicture, ckIsVideo,ckIsMap ,ckIsAttachment, ckIsDate, _
                                     ckIsPictureHover, ckIsImageSlideShow, ckIsPublishDate, ckIsExpiredDate, ckIsComment, ckIsCommentPreApproval, ckIsForum, _
                                     ckIsForumPreApproval, ckIsPolling, ckIsNeedApproval, ckIsDisplay1, ckIsDisplay2, ckIsDisplay3, ckIsDisplay4, ckActive, txtTestLink, txtThumbW, txtThumbH, _
                                     txtPicW, txtPicH, txtApprovalUser, inputUN, txtMetaTitle, txtMetaAuthor)
                End If

                If IsNumeric(temp) Then
                    If Trim(_save) = "1" Then
                        If Trim(q_ref) <> "" Then
                            'update
                            Hashtable("note") = "Update succeed."
                        Else
                            'insert
                            Hashtable("note") = "Insert succeed."
                        End If

                        Response.Redirect(GetEncUrl(_rootPath + "wf/sTagPopup.aspx?r=" + temp + "&", Hashtable))

                    Else
                        If Trim(q_ref) <> "" Then
                            'update
                            Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; window.close();</script>")
                        Else
                            Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; alert('Insert succeed, please click Refresh / Search button'); window.close();</script>")
                        End If
                    End If

                Else
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_delete) = "1" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteTag(Session("domainRef").ToString, q_ref)
                If Trim(temp) = "" Then
                    'benar
                    Page.ClientScript.RegisterStartupScript(Me.GetType, "closePopup", "<script language='javascript'>try{window.opener.doRefresh();}catch(e){}; alert('Delete succeed, please click Refresh / Search button'); window.close();</script>")
                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

                End If
            End If
        Else
            ''''' not postback '''''
            ''''' not postback '''''
            ''''' not postback '''''

            Dim q_tagType As String = ""
            Dim q_parentTag As String = ""

            If Not Request.Params("x") Is Nothing Then
                Dim param As Hashtable = GetDecParam(Request.Params("x"))
                Dim q_note As String = param("note")
                q_tagType = param("tagType")
                q_parentTag = param("parentTag")
                selContentDisplay = param("selContentDisplay")

                If Trim(q_note) <> "" Then
                    divNotif.InnerHtml = "Notification: " + q_note
                End If
            End If

            If Trim(q_ref) <> "" Then
                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Update Tag [" + Session("domain") + "]"

                Dim dt As New DataTable

                dt = getTagInfo(Session("domainRef").ToString, q_ref)

                
                If dt.Rows.Count > 0 Then
                    Dim Hashtable As New Hashtable
                    Hashtable("tagType") = dt.Rows(0).Item("tagTypeRef")
                    Hashtable("parentTag") = dt.Rows(0).Item("tagRefParent")
                    Hashtable("selContentDisplay") = dt.Rows(0).Item("contentDisplayRef")

                    ltrBtn.Text =  "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("tagName")) + "');"" class=""btn btn-md btn-dark""><span>Delete</span></a> " + _
                                    "  <a href=""" + GetEncUrl(_rootPath + "wf/sTagPopup.aspx?", Hashtable) + """ class=""btn btn-md btn-dark""><span>Insert New</span></a> " 
                                    


                    selTagType = dt.Rows(0).Item("tagTypeRef")
                    selParentTag = dt.Rows(0).Item("tagRefParent")
                    txtTagName = dt.Rows(0).Item("tagName")
                    txtMetaTitle = dt.Rows(0).Item("metaTitle")
                    txtMetaAuthor = dt.Rows(0).Item("metaAuthor")
                    txtKeyword = dt.Rows(0).Item("keyword")
                    txtDescription = dt.Rows(0).Item("description")
                    txtMetaDescription = dt.Rows(0).Item("metaDescription")
                    txtSortNo = dt.Rows(0).Item("sortNo")
                    txtTestLink = dt.Rows(0).Item("testLink")
                    txtThumbW = dt.Rows(0).Item("thumbImgW")
                    txtThumbH = dt.Rows(0).Item("thumbImgH")
                    txtPicW = dt.Rows(0).Item("picImgW")
                    txtPicH = dt.Rows(0).Item("picImgH")
                    txtApprovalUser = dt.Rows(0).Item("approvalUser")
                    ckActive = dt.Rows(0).Item("isActive")

                    ckIsSingleContent = dt.Rows(0).Item("isSingleContent")
                    ckIsOnlyParent = dt.Rows(0).Item("isOnlyParent")
                    ckIsTitle = dt.Rows(0).Item("isTitle")
                    ckIsTitleDetail = dt.Rows(0).Item("isTitleDetail")
                    ckIsSynopsis = dt.Rows(0).Item("isSynopsis")
                    ckIsContent = dt.Rows(0).Item("isContent")
                    ckIsThumbnail = dt.Rows(0).Item("isThumbnail")
                    ckIsPicture = dt.Rows(0).Item("isPicture")
                    ckIsVideo = dt.Rows(0).Item("isVideo")
                    ckIsMap = dt.Rows(0).Item("isMap")
                    ckIsAttachment = dt.Rows(0).Item("isAttachment")
                    ckIsDate = dt.Rows(0).Item("isContentDate")

                    ckIsPictureHover = dt.Rows(0).Item("IsPictureHover")
                    ckIsImageSlideShow = dt.Rows(0).Item("IsImageSlideShow")

                    ckIsPublishDate = dt.Rows(0).Item("isContentPubDate")
                    ckIsExpiredDate = dt.Rows(0).Item("isExpiredDate")
                    ckIsComment = dt.Rows(0).Item("isComment")
                    ckIsCommentPreApproval = dt.Rows(0).Item("isCommentPreApproval")
                    ckIsNeedApproval = dt.Rows(0).Item("isNeedApproval")
                    ckIsPolling = dt.Rows(0).Item("isPolling")
                    ckIsForum = dt.Rows(0).Item("isForum")
                    ckIsForumPreApproval = dt.Rows(0).Item("isForumPreApproval")
                    ckIsDisplay1 = dt.Rows(0).Item("isDisplay1")
                    ckIsDisplay2 = dt.Rows(0).Item("isDisplay2")
                    ckIsDisplay3 = dt.Rows(0).Item("isDisplay3")
                    ckIsDisplay4 = dt.Rows(0).Item("isDisplay4")
                    selContentDisplay = dt.Rows(0).Item("contentDisplayRef")

                End If

                ltrTagType.Text = bindSelTagType(Session("domainRef").ToString, selTagType)
                ltrParentTag.Text = bindSelParent(Session("domainRef").ToString, selParentTag)
                ltrContentDisplay.Text = bindContentDisplay(selContentDisplay)
            Else

                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Insert Tag [" + Session("domain") + "]"

                ltrTagType.Text = bindSelTagType(Session("domainRef").ToString, q_tagType)
                ltrParentTag.Text = bindSelParent(Session("domainRef").ToString, q_parentTag)
                ltrContentDisplay.Text = bindContentDisplay(selContentDisplay)
            End If

            ltrPicture.Text = bindPicture(Session("domainRef").ToString, q_ref)
            
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWebPopup()
    End Sub

End Class
