Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports [class].cls_wmContent
Imports [class].clsGeneralDB
Imports Newtonsoft.Json

Partial Class wf_wmContentPopup
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath

    Protected txtTagRef As String = ""
    Protected txtTagName As String = ""
    Protected selContentType As String = ""
    Protected selImageSetting As String = ""
    Protected selImagePosition As String = ""
    Protected txtMetaTitle As String = ""
    Protected txtMetaAuthor As String = ""
    Protected txtKeyword As String = ""
    Protected txtMetaDescription As String = ""
    Protected txtTitle As String = ""
    Protected txtVideo As String = ""
    Protected txtTitleDetail As String = ""
    Protected txtSynopsis As String = ""
    Protected txtContent As String = ""

    Protected _fileImgName As String = ""
    Protected ImgBytes As Byte() = {0}

    Protected txtSortNo As String = ""

    Protected txtMapLongitude As String = ""
    Protected txtMapLatitude As String = ""
    Protected selDayContent As String = ""
    Protected selMonthContent As String = ""
    Protected txtYearContent As String = ""

    Protected selDayPublish As String = ""
    Protected selMonthPublish As String = ""
    Protected txtYearPublish As String = ""

    Protected selDayExpired As String = ""
    Protected selMonthExpired As String = ""
    Protected txtYearExpired As String = ""


    Protected isMapLongitude As String = ""
    Protected isMapLatitude As String = ""
    Protected isMap As String = ""
    Protected isUpdate As String = ""
    Protected isForm As String = ""
    Protected isTitle As String = ""
    Protected isVideo As String = ""
    Protected isTitleDetail As String = ""
    Protected isSynopsis As String = ""
    Protected isContent As String = ""
    Protected isThumbnail As String = ""
    Protected isPicture As String = ""
    Protected isAttachment As String = ""
    Protected isDate As String = ""
    Protected isPublishDate As String = ""
    Protected isExpiredDate As String = ""

    Protected _jsonContentRelated As String = "[]"

    Public Sub New()

    End Sub

    Private Function bindDay(ByVal ctrlName As String, ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer

        result.Append("<select id=""selDay" + ctrlName + """ name=""selDay" + ctrlName + """ onchange=""cekDate('" + ctrlName + "', document.getElementById('eDate" + ctrlName + "'));"">")
        result.Append("<option value=""-"">-</option>")
        For i = 1 To 31
            If value = i.ToString Then
                result.Append("<option selected=""selected"" value=""" + i.ToString + """>" + i.ToString + "</option>")
            Else
                result.Append("<option value=""" + i.ToString + """>" + i.ToString + "</option>")
            End If
        Next
        result.Append("</select>")

        Return result.ToString
    End Function

    Private Function bindMonth(ByVal ctrlName As String, ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer

        result.Append("<select id=""selMonth" + ctrlName + """ name=""selMonth" + ctrlName + """ onchange=""cekDate('" + ctrlName + "', document.getElementById('eDate" + ctrlName + "'));"">")
        result.Append("<option value=""-"">-</option>")
        For i = 0 To monthValue.Count - 1
            If value = (i + 1).ToString Then
                result.Append(" <option selected=""selected"" value=""" + monthValue(i) + """>" + monthName(i) + "</option>")
            Else
                result.Append(" <option value=""" + monthValue(i) + """>" + monthName(i) + "</option>")
            End If
        Next

        result.Append("</select>")

        Return result.ToString
    End Function

    Private Function bindSelContentType(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getContentTypeListLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selContentType"" name=""selContentType"" >")
            For i = 0 To dt.Rows.Count - 1

                If value = dt.Rows(i).Item("contentType").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("contentType").ToString + """>" + dt.Rows(i).Item("contentTypeName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("contentType").ToString + """>" + dt.Rows(i).Item("contentTypeName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Function bindSelImageSetting(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getContentImgSettingLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selImageSetting"" name=""selImageSetting"" >")
            For i = 0 To dt.Rows.Count - 1
                If value = dt.Rows(i).Item("imgSetting").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("imgSetting").ToString + """>" + dt.Rows(i).Item("imgSettingName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("imgSetting").ToString + """>" + dt.Rows(i).Item("imgSettingName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Private Function bindSelImagePosition(ByVal value As String) As String
        Dim result As New StringBuilder
        Dim i As Integer
        Dim dt As New DataTable

        dt = getContentImgPositionLookup()
        If dt.Rows.Count > 0 Then
            result.Append("<select id=""selImagePosition"" name=""selImagePosition"" >")
            For i = 0 To dt.Rows.Count - 1
                If value = dt.Rows(i).Item("imgPosition").ToString Then
                    result.Append("<option selected value=""" + dt.Rows(i).Item("imgPosition").ToString + """>" + dt.Rows(i).Item("imgPositionName") + "</option>")
                Else
                    result.Append("<option value=""" + dt.Rows(i).Item("imgPosition").ToString + """>" + dt.Rows(i).Item("imgPositionName") + "</option>")
                End If
            Next
            result.Append("</select> ")
        End If

        Return result.ToString
    End Function

    Public Sub rekContentTagChild(ByVal domainRef As String, ByVal contentRef As String, _
                                 ByRef strTree As StringBuilder, ByVal tagRef As String, _
                                  ByVal parentName As String, ByRef allTag As String, ByVal isAllTagAdd As Boolean, _
                                  ByVal level As Integer)
        Dim dt As New DataTable

        If Trim(contentRef) = "" Then contentRef = "0"

        If tagRef <> "0" Then
            If isAllTagAdd Then
                If allTag = "" Then
                    allTag = tagRef
                Else
                    allTag = allTag + "-" + tagRef
                End If
            End If
        End If

        dt = getTagListByParentLookup(domainRef, tagRef)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                Dim tab As Integer = 20 * level
                Dim includeAllTag As Boolean = False

                strTree.Append("<div style=""margin-left:" + tab.ToString + "px; margin-top:3px;"" id=""divH" + dt.Rows(i).Item("tagRef").ToString + """>")

                ''''' semua bisa di add, gak peduli punya child atau tidak '''''
                If cekIsTagHaveChild(domainRef, dt.Rows(i).Item("tagRef").ToString) Then
                    strTree.Append(" <span><img src=""" + rootPath + "support/image/icon_folderOpen.png"" style=""vertical-align:middle"" /></span>&nbsp;")
                    strTree.Append(dt.Rows(i).Item("tagName") + " <span class=""fNotif"">[" + dt.Rows(i).Item("tagTypeName") + "]</span>")
                Else
                    strTree.Append(" <span><img src=""" + rootPath + "support/image/icon_folder.png"" style=""vertical-align:middle"" /></span>&nbsp;")
                    strTree.Append(dt.Rows(i).Item("tagName") + " <span class=""fNotif"">[" + dt.Rows(i).Item("tagTypeName") + "]</span>")
                End If
                includeAllTag = True
                strTree.Append(" <span id=""spIsG" + dt.Rows(i).Item("tagRef").ToString + """>")
                If cekIsThisTagContent(domainRef, contentRef, dt.Rows(i).Item("tagRef").ToString) Then
                    strTree.Append("&nbsp;<img height=""16"" src=""" + rootPath + "support/image/icon_yes.png"" style=""vertical-align:middle"" />")
                    strTree.Append("</span>")
                    strTree.Append("&nbsp;&nbsp;<span id=""spRemAdd" + dt.Rows(i).Item("tagRef").ToString + """><a href=""javascript:doAddContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ + ]</a>&nbsp;<a href=""javascript:doRemoveContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ - ]</a></span>")
                Else
                    strTree.Append("</span>")
                    strTree.Append("&nbsp;&nbsp;<span id=""spRemAdd" + dt.Rows(i).Item("tagRef").ToString + """><a href=""javascript:doAddContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ + ]</a>&nbsp;<a href=""javascript:doRemoveContentTag('" + dt.Rows(i).Item("tagName") + "', '" + dt.Rows(i).Item("tagRef").ToString + "');"">[ - ]</a></span>")
                End If
                ''''' semua bisa di add, gak peduli punya child atau tidak '''''
                strTree.Append("</div>")
                strTree.Append("<div>")
                rekContentTagChild(domainRef, contentRef, strTree, dt.Rows(i).Item("tagRef"), IIf(Trim(parentName) = "", dt.Rows(i).Item("tagName"), parentName + " :: " + dt.Rows(i).Item("tagName")), allTag, includeAllTag, level + 1)
                strTree.Append("</div>")
            Next
        End If
    End Sub

    Private Function bindTag(ByVal domainRef As String, ByVal contentRef As String) As String
        Dim result As New StringBuilder
        Dim allGroup As String = ""

        rekContentTagChild(domainRef, contentRef, result, "0", "", allGroup, False, 0)
        result.Append("<a href=""javascript:doRemoveContentTagAll()"" class=""btn btn-sm btn-dark""><span>Remove All Tag</span></a>&nbsp;|&nbsp;<a href=""javascript:doCloseTagTree()"" class=""btn btn-sm btn-dark""><span>Close Tag Tree</span></a></div>")

        Return result.ToString
    End Function

    Private Function bindThumbnail(ByVal domainRef As String, ByVal contentRef As String) As String
        Dim result As New StringBuilder

        If Trim(contentRef) = "" Then
            result.Append("<div class=""fNotif mt5 mb5"">After save, you can insert the thumbnail.</div>")
        Else
            Dim dt As New DataTable
            Dim dtImg As New DataTable

            dtImg = getTagImageSetting(domainRef, getContentFirstTag(domainRef, contentRef))

            dt = getContentImage(domainRef, contentRef, "T")
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    result.Append("<div class=""ov mb5"">")
                    result.Append("  <img src=""" + rootPath + "wf/support/displayImage.aspx?m=content&d=" + domainRef + "&ir=" + dt.Rows(i).Item("imgRef").ToString + "&w=400"" />")
                    result.Append("</div>")
                    result.Append("<div class=""mb5"">")
                    result.Append("  <a href=""javascript:doDeleteImage(" + dt.Rows(i).Item("imgRef").ToString + ");"">Delete Image</a>")
                    result.Append("  &nbsp;|&nbsp;&nbsp;<a href=""javascript:doRemoveImage(" + dt.Rows(i).Item("imgRef").ToString + ");"">Remove Image From This Content</a>")
                    result.Append("</div>")
                Next
            Else
                'no image
            End If
            result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=wmContentPopup.aspx&m=contentThumb&t=s&ir=0&r=" + contentRef + """>Click here</a> to upload.</div>")

            If dtImg.Rows.Count > 0 Then
                If dtImg.Rows(0).Item("thumbImgW") <> 0 And dtImg.Rows(0).Item("thumbImgH") <> 0 Then
                    result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=wmContentPopup.aspx&m=contentThumb&t=s&ir=1&rw=" + dtImg.Rows(0).Item("thumbImgW").ToString + "&rh=" + dtImg.Rows(0).Item("thumbImgH").ToString + "&r=" + contentRef + """>Click here</a> to upload, auto resize to " + dtImg.Rows(0).Item("thumbImgW").ToString + "(w) x " + dtImg.Rows(0).Item("thumbImgH").ToString + "(h) if bigger than.</div>")
                End If
            End If

        End If

        Return result.ToString
    End Function

    Private Function bindPicture(ByVal domainRef As String, ByVal contentRef As String) As String
        Dim result As New StringBuilder

        If Trim(contentRef) = "" Then
            result.Append("<div class=""fNotif mt5 mb5"">After save, you can insert the picture.</div>")
        Else
            Dim dt As New DataTable
            Dim dtImg As New DataTable

            dtImg = getTagImageSetting(domainRef, getContentFirstTag(domainRef, contentRef))

            dt = getContentImage(domainRef, contentRef, "P")
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    result.Append("<div class=""ov mb5"">")
                    result.Append("  <img src=""" + rootPath + "wf/support/displayImage.aspx?m=content&d=" + domainRef + "&ir=" + dt.Rows(i).Item("imgRef").ToString + "&w=400"" />")
                    result.Append("</div>")
                    result.Append("<div class=""mb5"">")
                    result.Append("  <a href=""javascript:doDeleteImage(" + dt.Rows(i).Item("imgRef").ToString + ");"">Delete Image</a>")
                    result.Append("  &nbsp;|&nbsp;&nbsp;<a href=""javascript:doRemoveImage(" + dt.Rows(i).Item("imgRef").ToString + ");"">Remove Image From This Content</a>")
                    result.Append("</div>")
                Next

            Else
                'no image
            End If
            result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=wmContentPopup.aspx&m=contentPic&t=m&ir=0&r=" + contentRef + """>Click here</a> to upload.</div>")

            If dtImg.Rows.Count > 0 Then
                If dtImg.Rows(0).Item("picImgW") <> 0 And dtImg.Rows(0).Item("picImgH") <> 0 Then
                    result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=wmContentPopup.aspx&m=contentPic&t=m&ir=1&rw=" + dtImg.Rows(0).Item("picImgW").ToString + "&rh=" + dtImg.Rows(0).Item("picImgH").ToString + "&r=" + contentRef + """>Click here</a> to upload, auto resize to " + dtImg.Rows(0).Item("picImgW").ToString + "(w) x " + dtImg.Rows(0).Item("picImgH").ToString + "(h) if bigger than.</div>")
                End If
            End If


        End If

        Return result.ToString
    End Function

    Private Function bindAttachment(ByVal domainRef As String, ByVal contentRef As String) As String
        Dim result As New StringBuilder

        If Trim(contentRef) = "" Then
            result.Append("<div class=""fNotif mt5 mb5"">After save, you can upload the attachment.</div>")
        Else
            Dim dt As New DataTable


            dt = getContentAttachmentRef(domainRef, contentRef)

            If dt.Rows.Count > 0 Then

                For i = 0 To dt.Rows.Count - 1
                    result.Append("<div class=""ov mb5"">")
                    Select Case Right(dt.Rows(i).Item("attachFN"), 4).ToLower
                        Case ".pdf"
                            result.Append("<a target=""_blank"" href=""" + rootPath + "wf/support/displayAttachment.aspx?dr=" + domainRef + "&cr=" + contentRef + "&ar=" + dt.Rows(i).Item("attachRef").ToString + """><img border=""0"" src=""" + rootPath + "support/image/icon_pdf.jpg"" /></a>")
                        Case ".doc"
                            result.Append("<a target=""_blank"" href=""" + rootPath + "wf/support/displayAttachment.aspx?dr=" + domainRef + "&cr=" + contentRef + "&ar=" + dt.Rows(i).Item("attachRef").ToString + """><img border=""0"" src=""" + rootPath + "support/image/icon_word.jpg"" /></a>")
                        Case "docx"
                            result.Append("<a target=""_blank"" href=""" + rootPath + "wf/support/displayAttachment.aspx?dr=" + domainRef + "&cr=" + contentRef + "&ar=" + dt.Rows(i).Item("attachRef").ToString + """><img border=""0"" src=""" + rootPath + "support/image/icon_word.jpg"" /></a>")
                        Case ".xls"
                            result.Append("<a target=""_blank"" href=""" + rootPath + "wf/support/displayAttachment.aspx?dr=" + domainRef + "&cr=" + contentRef + "&ar=" + dt.Rows(i).Item("attachRef").ToString + """><img border=""0"" src=""" + rootPath + "support/image/icon_excel.jpg"" /></a>")
                        Case "xlsx"
                            result.Append("<a target=""_blank"" href=""" + rootPath + "wf/support/displayAttachment.aspx?dr=" + domainRef + "&cr=" + contentRef + "&ar=" + dt.Rows(i).Item("attachRef").ToString + """><img border=""0"" src=""" + rootPath + "support/image/icon_excel.jpg"" /></a>")
                        Case Else
                            result.Append("<a target=""_blank"" href=""" + rootPath + "wf/support/displayAttachment.aspx?dr=" + domainRef + "&cr=" + contentRef + "&ar=" + dt.Rows(i).Item("attachRef").ToString + """><img border=""0"" src=""" + rootPath + "support/image/icon_file.png"" /></a>")
                    End Select
                    result.Append("</div>")
                    result.Append("<div class=""mb5"">")
                    result.Append("  <a href=""javascript:doDeleteAttachment(" + dt.Rows(i).Item("attachRef").ToString + ");"">Delete Attachment</a>")
                    result.Append("</div>")
                Next

            Else
                'no attachment
            End If
            result.Append("<div class=""fNotif mt5 mb5""><a href=""" + rootPath + "wf/support/uploadForm.aspx?src=wmContentPopup.aspx&m=contentAttach&t=m&r=" + contentRef + """>Click here</a> to upload.</div>")


        End If

        Return result.ToString
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim inputUN As String = Session("userName")
        If inputUN = String.Empty Then inputUN = ""

        Dim q_ref As String = Request.QueryString("r")

        If IsPostBack Then
            Dim _generate As String = Request.Form("_generate")
            Dim _save As String = Request.Form("_save")
            Dim _saveClose As String = Request.Form("_saveClose")
            Dim _delete As String = Request.Form("_delete")
            Dim _deleteImage As String = Request.Form("_deleteImage")
            Dim _deleteAttachment As String = Request.Form("_deleteAttachment")
            Dim _removeImage As String = Request.Form("_removeImage")
            Dim ImgLength As Integer = Encoding.UTF8.GetString(ImgBytes).Length

            If Trim(_generate) = "1" Then
                Dim Hashtable As New Hashtable

                txtTagName = Request.Form("txtTagName")
                txtTagRef = Request.Form("txtTagRef")

                selContentType = Request.Form("selContentType")
                selImageSetting = Request.Form("selImageSetting")
                selImagePosition = Request.Form("selImagePosition")

                selDayContent = Request.Form("selDayContent")
                selMonthContent = Request.Form("selMonthContent")
                txtYearContent = Request.Form("txtYearContent")

                selDayPublish = Request.Form("selDayPublish")
                selMonthPublish = Request.Form("selMonthPublish")
                txtYearPublish = Request.Form("txtYearPublish")

                selDayExpired = Request.Form("selDayExpired")
                selMonthExpired = Request.Form("selMonthExpired")
                txtYearExpired = Request.Form("txtYearExpired")

                txtSortNo = Request.Form("txtSortNo")

                Hashtable("contentType") = selContentType
                Hashtable("imageSetting") = selImageSetting
                Hashtable("ip") = selImagePosition
                Hashtable("tagName") = txtTagName
                Hashtable("tagRef") = txtTagRef
                Hashtable("isForm") = "1"
                Hashtable("note") = "Please fill all data below than click ""save"""
                Hashtable("dc") = selDayContent
                Hashtable("mc") = selMonthContent
                Hashtable("yc") = txtYearContent
                Hashtable("dp") = selDayPublish
                Hashtable("mp") = selMonthPublish
                Hashtable("yp") = txtYearPublish
                Hashtable("de") = selDayExpired
                Hashtable("me") = selMonthExpired
                Hashtable("ye") = txtYearExpired


                Response.Redirect(GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?r=" + q_ref + "&", Hashtable))
            End If

            If Trim(_save) = "1" Or Trim(_saveClose) = "1" Then
                txtTagRef = Request.Form("txtTagRef")
                txtTagName = Request.Form("txtTagName")
                selContentType = Request.Form("selContentType")
                selImageSetting = Request.Form("selImageSetting")
                selImagePosition = Request.Form("selImagePosition")
                txtMetaTitle = Request.Form("txtMetaTitle")
                txtMetaAuthor = Request.Form("txtMetaAuthor")
                txtKeyword = Request.Form("txtKeyword")
                txtMetaDescription = Request.Form("txtMetaDescription")
                txtVideo = Request.Form("txtVideo")
                txtTitle = Request.Form("txtTitle")
                txtTitleDetail = Request.Form("txtTitleDetail")
                txtSynopsis = Request.Form("txtSynopsis")
                txtContent = Request.Form("txtContent")

                txtMapLongitude = Request.Form("txtMapLongitude")
                txtMapLatitude = Request.Form("txtMapLatitude")


                selDayContent = Request.Form("selDayContent")
                selMonthContent = Request.Form("selMonthContent")
                txtYearContent = Request.Form("txtYearContent")

                selDayPublish = Request.Form("selDayPublish")
                selMonthPublish = Request.Form("selMonthPublish")
                txtYearPublish = Request.Form("txtYearPublish")

                selDayExpired = Request.Form("selDayExpired")
                selMonthExpired = Request.Form("selMonthExpired")
                txtYearExpired = Request.Form("txtYearExpired")

                txtSortNo = Request.Form("txtSortNo")

                Dim contentDate As System.Data.SqlTypes.SqlDateTime
                contentDate = System.Data.SqlTypes.SqlDateTime.Null
                If selDayContent <> "-" And selMonthContent <> "-" And txtYearContent <> "" Then
                    contentDate = New System.Data.SqlTypes.SqlDateTime(txtYearContent, selMonthContent, selDayContent)
                End If

                Dim publishDate As System.Data.SqlTypes.SqlDateTime
                publishDate = System.Data.SqlTypes.SqlDateTime.Null
                If selDayPublish <> "-" And selMonthPublish <> "-" And txtYearPublish <> "" Then
                    publishDate = New System.Data.SqlTypes.SqlDateTime(txtYearPublish, selMonthPublish, selDayPublish)
                ElseIf cekTagIsPublish(Session("domainRef").ToString, txtTagRef.Split(",")(0)) = "0" Then
                    publishDate = New System.Data.SqlTypes.SqlDateTime(Now.Year, Now.Month, Now.Day)
                End If

                Dim expiredDate As System.Data.SqlTypes.SqlDateTime
                expiredDate = System.Data.SqlTypes.SqlDateTime.Null
                If selDayExpired <> "-" And selMonthExpired <> "-" And txtYearExpired <> "" Then
                    expiredDate = New System.Data.SqlTypes.SqlDateTime(txtYearExpired, selMonthExpired, selDayExpired)
                End If

                'ini musti cek approval belum nih
                Dim approvedDate As System.Data.SqlTypes.SqlDateTime
                approvedDate = New System.Data.SqlTypes.SqlDateTime(Now)


                Dim temp As String = ""
                Dim Hashtable As New Hashtable

                Dim _listContentRelated As String = Request.Form("_listContentRelated")
                Dim dtContentRelated As New DataTable
                If Trim(_listContentRelated) <> "" Then
                    dtContentRelated = JsonConvert.DeserializeObject(Of DataTable)(_listContentRelated)
                End If

                If Trim(q_ref) <> "" Then
                    'update
                    temp = updateContent(Session("domainRef").ToString, q_ref, selContentType, selImageSetting, selImagePosition, txtVideo, txtTitle, txtTitleDetail, txtMapLongitude, txtMapLatitude, txtSynopsis, _
                                         txtContent, contentDate, publishDate, expiredDate, approvedDate, txtMetaDescription, txtMetaTitle, txtMetaAuthor)
                Else
                    'insert
                    temp = insertContent(Session("domainRef").ToString, selContentType, selImageSetting, selImagePosition, txtVideo, txtTitle, txtTitleDetail, txtSynopsis, txtContent, _
                                         txtMapLongitude, txtMapLatitude, contentDate, publishDate, expiredDate, approvedDate, txtMetaDescription, inputUN, txtMetaTitle, txtMetaAuthor)
                End If

                If IsNumeric(temp) Then
                    insertContentKeyword(Session("domainRef").ToString, temp, txtKeyword, _keywordSplitter)
                    insertContentTag(Session("domainRef").ToString, temp, txtTagRef)

                    If Trim(_save) = "1" Then
                        If Trim(q_ref) <> "" Then
                            'update
                            Hashtable("note") = "Update succeed."
                        Else
                            'insert
                            Hashtable("note") = "Insert succeed."
                        End If

                        Response.Redirect(GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?r=" + temp + "&", Hashtable))

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


                temp = deleteContent(Session("domainRef").ToString, q_ref)
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

            If Trim(_deleteImage) <> "" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteImage(Session("domainRef").ToString, _deleteImage)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "Delete image succeed."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?r=" + q_ref + "&", Hashtable))

                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_removeImage) <> "" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = removeImage(Session("domainRef").ToString, q_ref, _removeImage)
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "Remove image from this content succeed."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?r=" + q_ref + "&", Hashtable))

                Else
                    'salah
                    Hashtable("note") = "Sorry, there is an error.<br>" + _
                                "Error Message: " + temp + "<br><br>" + _
                                "<a href=""javascript:window.close();"">Click here</a> to close this popup.<br>" + _
                                "Thankyou."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/notificationpopup.aspx?", Hashtable))

                End If
            End If

            If Trim(_deleteAttachment) <> "" Then
                Dim temp As String = ""
                Dim Hashtable As New Hashtable


                temp = deleteAttachment(Session("domainRef").ToString, q_ref, _deleteAttachment, )
                If Trim(temp) = "" Then
                    'benar
                    Hashtable("note") = "Delete attachment succeed."

                    Response.Redirect(GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?r=" + q_ref + "&", Hashtable))

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

            'Dim q_tagType As String = ""
            'Dim q_parentTag As String = ""

            If Not Request.Params("x") Is Nothing Then
                Dim param As Hashtable = GetDecParam(Request.Params("x"))
                Dim q_note As String = param("note")

                selContentType = param("contentType")
                selImageSetting = param("imageSetting")
                selImagePosition = param("ip")
                txtTagName = param("tagName")
                txtTagRef = param("tagRef")
                isForm = param("isForm")
                selDayContent = param("dc")
                selMonthContent = param("mc")
                txtYearContent = param("yc")
                selDayPublish = param("dp")
                selMonthPublish = param("mp")
                txtYearPublish = param("yp")
                selDayExpired = param("de")
                selMonthExpired = param("me")
                txtYearExpired = param("ye")

                If Trim(q_note) <> "" Then
                    divNotif.InnerHtml = "Notification: " + q_note
                Else
                    If isForm = "1" Then
                        divNotif.InnerHtml = "Notification: Please fill all data below than click ""save"""
                    End If
                End If
            End If



            If Trim(q_ref) <> "" Then
                isUpdate = "1"

                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Update Content [" + Session("domain") + "]"
                Page.Title = "CMS :: " + Session("domain") + " :: Update Content"

                Dim dt As New DataTable

                dt = getContentInfo(Session("domainRef").ToString, q_ref)


                If dt.Rows.Count > 0 Then

                    Dim dtContentRelated As New DataTable
                    'dtContentRelated = getContentRelatedList(Session("domainRef").ToString, q_ref)
                    _jsonContentRelated = JsonConvert.SerializeObject(dtContentRelated, Formatting.None)

                    selContentType = dt.Rows(0).Item("contentType")
                    selImageSetting = dt.Rows(0).Item("imgSetting")
                    selImagePosition = dt.Rows(0).Item("imgPosition")
                    txtMetaTitle = dt.Rows(0).Item("metaTitle")
                    txtMetaAuthor = dt.Rows(0).Item("metaAuthor")
                    txtKeyword = getContentKeywordStr(Session("domainRef").ToString, q_ref)
                    txtMetaDescription = dt.Rows(0).Item("metadescription")
                    txtVideo = dt.Rows(0).Item("embedVideo")
                    txtTitle = dt.Rows(0).Item("title")
                    txtTitleDetail = dt.Rows(0).Item("titleDetail")


                    'If Not IsDBNull(dt.Rows(0).Item("latitude")) Then
                    '    txtMapLatitude = dt.Rows(0).Item("Latitude")
                    'End If

                    'If Not IsDBNull(dt.Rows(0).Item("Longitude")) Then
                    '    txtMapLongitude = dt.Rows(0).Item("Longitude")
                    'End If


                    'txtSynopsis = dt.Rows(0).Item("synopsis")
                    'txtContent = dt.Rows(0).Item("content")

                    'If Not IsDBNull(dt.Rows(0).Item("contentDate")) Then
                    '    selDayContent = Day(dt.Rows(0).Item("contentDate"))
                    '    selMonthContent = Month(dt.Rows(0).Item("contentDate"))
                    '    txtYearContent = Year(dt.Rows(0).Item("contentDate"))
                    'End If

                    'If Not IsDBNull(dt.Rows(0).Item("latitude")) Then
                    '    txtMapLatitude = dt.Rows(0).Item("Latitude")
                    'End If

                    'If Not IsDBNull(dt.Rows(0).Item("Longitude")) Then
                    '    txtMapLongitude = dt.Rows(0).Item("Longitude")
                    'End If

                    'If Not IsDBNull(dt.Rows(0).Item("publishDate")) Then
                    '    selDayPublish = Day(dt.Rows(0).Item("publishDate"))
                    '    selMonthPublish = Month(dt.Rows(0).Item("publishDate"))
                    '    txtYearPublish = Year(dt.Rows(0).Item("publishDate"))
                    'End If

                    'If Not IsDBNull(dt.Rows(0).Item("expiredDate")) Then
                    '    selDayExpired = Day(dt.Rows(0).Item("expiredDate"))
                    '    selMonthExpired = Month(dt.Rows(0).Item("expiredDate"))
                    '    txtYearExpired = Year(dt.Rows(0).Item("expiredDate"))
                    'End If

                    '' mungkin perlu param tag list
                    If Trim(txtTagRef) = "" Then
                        txtTagRef = getContentTagRefStr(Session("domainRef").ToString, q_ref)
                        txtTagName = getContentTagNameStr(Session("domainRef").ToString, q_ref)
                    End If

                    'txtSortNo = dt.Rows(0).Item("sortNo")

                    'divHitView.InnerHtml = dt.Rows(0).Item("hit").ToString
                    'isForm = "1"

                    Dim Hashtable As New Hashtable
                    Hashtable("contentType") = dt.Rows(0).Item("contentType")
                    Hashtable("imageSetting") = dt.Rows(0).Item("imgSetting")
                    Hashtable("tagRef") = txtTagRef
                    Hashtable("tagName") = txtTagName
                    Hashtable("dc") = selDayContent
                    Hashtable("mc") = selMonthContent
                    Hashtable("yc") = txtYearContent
                    Hashtable("dp") = selDayPublish
                    Hashtable("mp") = selMonthPublish
                    Hashtable("yp") = txtYearPublish
                    Hashtable("de") = selDayExpired
                    Hashtable("me") = selMonthExpired
                    Hashtable("ye") = txtYearExpired
                    Hashtable("isForm") = 1

                    ltrBtn.Text = "<div class=""linkBtn left mr5""> " + _
                                    "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("title")) + "');"">Delete</a> " + _
                                    "</div> " + _
                                    "<div class=""linkBtn left mr5""> " + _
                                    "  <a href=""" + GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?", Hashtable) + """>Insert New</a> " + _
                                    "</div> "

                    ltrBtnTop.Text = "<div class=""linkBtn left mr5""> " + _
                                    "  <a href=""javascript:doDelete('" + MyURLEncode(dt.Rows(0).Item("title")) + "');"">Delete</a> " + _
                                    "</div> " + _
                                    "<div class=""linkBtn left mr5""> " + _
                                    "  <a href=""" + GetEncUrl(_rootPath + "wf/wmContentPopup.aspx?", Hashtable) + """>Insert New</a> " + _
                                    "</div> "
                End If

            Else



                CType(Master.FindControl("titlePopup"), Web.UI.HtmlControls.HtmlGenericControl).InnerHtml = "Insert Content [" + Session("domain") + "]"

                Page.Title = "CMS :: " + Session("domain") + " :: Insert Content"
            End If



            ''''' set form permission '''''
            ''''' set form permission '''''
            ''''' set form permission '''''
            If isForm = "1" Then
                If Trim(txtTagRef) <> "" Then
                    Dim dtTag As New DataTable

                    dtTag = getTagPermissionList(Session("domainRef").ToString, txtTagRef)
                    If dtTag.Rows.Count > 0 Then
                        For i = 0 To dtTag.Rows.Count - 1
                            If dtTag.Rows(i).Item("isVideo") = "1" Then isVideo = "1"
                            If dtTag.Rows(i).Item("isTitle") = "1" Then isTitle = "1"
                            If dtTag.Rows(i).Item("isTitleDetail") = "1" Then isTitleDetail = "1"
                            If dtTag.Rows(i).Item("isSynopsis") = "1" Then isSynopsis = "1"
                            If dtTag.Rows(i).Item("isMap") = "1" Then isMap = "1"

                            If dtTag.Rows(i).Item("isContent") = "1" Then isContent = "1"
                            If dtTag.Rows(i).Item("isThumbnail") = "1" Then isThumbnail = "1"
                            If dtTag.Rows(i).Item("isPicture") = "1" Then isPicture = "1"
                            If dtTag.Rows(i).Item("isAttachment") = "1" Then isAttachment = "1"
                            If dtTag.Rows(i).Item("isContentDate") = "1" Then isDate = "1"
                            If dtTag.Rows(i).Item("isContentPubDate") = "1" Then isPublishDate = "1"
                            If dtTag.Rows(i).Item("isExpiredDate") = "1" Then isExpiredDate = "1"
                        Next
                    End If

                    If Trim(q_ref) = "" Then
                        If isDate <> "1" Then
                            selDayContent = "-"
                            selMonthContent = "-"
                            txtYearContent = ""
                        Else
                            If Trim(selDayContent) = "-" Then selDayContent = Day(Now)
                            If Trim(selMonthContent) = "-" Then selMonthContent = Month(Now)
                            If Trim(txtYearContent) = "" Then txtYearContent = Year(Now)
                        End If

                        If isPublishDate <> "1" Then
                            selDayPublish = "-"
                            selMonthPublish = "-"
                            txtYearPublish = ""
                        Else
                            If Trim(selDayPublish) = "-" Then selDayPublish = Day(Now)
                            If Trim(selMonthPublish) = "-" Then selMonthPublish = Month(Now)
                            If Trim(txtYearPublish) = "" Then txtYearPublish = Year(Now)
                        End If
                    End If
                End If
            End If
            ''''' set form permission end '''''
            ''''' set form permission end '''''
            ''''' set form permission end '''''

            ltrContentType.Text = bindSelContentType(selContentType)
            ltrImageSetting.Text = bindSelImageSetting(selImageSetting)
            ltrImagePosition.Text = bindSelImagePosition(selImagePosition)

            spanDayContent.InnerHtml = bindDay("Content", selDayContent)
            spanMonthContent.InnerHtml = bindMonth("Content", selMonthContent)

            spanDayPublish.InnerHtml = bindDay("Publish", selDayPublish)
            spanMonthPublish.InnerHtml = bindMonth("Publish", selMonthPublish)

            spanDayExpired.InnerHtml = bindDay("Expired", selDayExpired)
            spanMonthExpired.InnerHtml = bindMonth("Expired", selMonthExpired)

            ltrThumbnail.Text = bindThumbnail(Session("domainRef").ToString, q_ref)
            ltrPicture.Text = bindPicture(Session("domainRef").ToString, q_ref)
            ltrTag.Text = bindTag(Session("domainRef").ToString, q_ref)
            ltrAttachment.Text = bindAttachment(Session("domainRef").ToString, q_ref)
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        cekIsNotLoginWebPopup()
    End Sub
End Class
