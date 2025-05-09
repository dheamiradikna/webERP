<%@ Page Language="VB" MasterPageFile="~/wf/defPopup.master" AutoEventWireup="false" CodeFile="wmContentPopup.aspx.vb" Inherits="wf_wmContentPopup" title="" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .tb
        {
        	width: 400px;
        }
        .ta
        {
        	width: 400px;
        	height: 100px;
        }
        .nicEdit-panel {background-color: #fff !important;}
        .nicEdit-button {background-color: #fff !important;}
        
        
    </style>
    <script language="javascript" type="text/javascript" src="<%=rootPath%>plugin/editor/nicEdit.js"></script>
    <script type="text/javascript" language="javascript">
        var isContentDate = true;
        var isPublishDate = true;
        var isExpiredDate = true;
        
        var _rootPath = '<%=rootPath%>';
        var _isForm = '<%=isForm%>';
        var _isUpdate = '<%=isUpdate%>';
        var _txtTagRef = '<%=txtTagRef%>';
        var _isTitle = '<%=isTitle%>';
        var _isTitleDetail = '<%=isTitleDetail%>';
        var _isVideo = '<%=isVideo%>';
        var _isSynopsis = '<%=isSynopsis%>';
        var _isContent = '<%=isContent%>';
        var _isThumbnail = '<%=isThumbnail%>';
        var _isPicture = '<%=isPicture%>';
        var _isAttachment = '<%=isAttachment%>';
        var _isDate = '<%=isDate%>';
        var _isPublishDate = '<%=isPublishDate%>';
        var _isExpiredDate = '<%=isExpiredDate%>';
        
        function dateValidation(date) {
            var dateReg = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var regex = new RegExp(dateReg);
            return regex.test(date);
        }
        
        function cekDate(ctrl, e) {
            var date = '';
            var selDay = document.getElementById("selDay" + ctrl);
            var selMonth = document.getElementById("selMonth" + ctrl);
            var txtYear = document.getElementById("txtYear" + ctrl);
            
            if (selDay.value == '-' && selMonth.value == '-' && txtYear.value == '') {
            
                if (ctrl == "Content") {
                    if (_isDate=="1") isContentDate = false;
                    e.style.display = '';
                } else if (ctrl == "Publish") {
                    if (_isPublishDate=="1") isPublishDate = true;
                    e.style.display = 'none';
                } else if (ctrl == "Expired") {
                    if (_isExpiredDate=="1") isExpiredDate = true;
                    e.style.display = 'none';
                }
                
            } else {
                date = Right('00' + selDay.value,2) + '/' + Right('00' + selMonth.value,2) + '/' + Right('0000' + txtYear.value, 4);
                if (dateValidation(date) == true) {
                    
                    if (ctrl == "Content") {
                        if (_isDate=="1") isContentDate = true; 
                    } else if (ctrl == "Publish") {
                        if (_isPublishDate=="1") isPublishDate = true;
                    } else if (ctrl == "Expired") {
                        if (_isExpiredDate=="1") isExpiredDate = true;
                    }
                    e.style.display = 'none';
                } else {
                    if (ctrl == "Content") {
                        if (_isDate=="1") isContentDate = false;
                    } else if (ctrl == "Publish") {
                        if (_isPublishDate=="1") isPublishDate = false;
                    } else if (ctrl == "Expired") {
                        if (_isExpiredDate=="1") isExpiredDate = false;
                    }
                    e.style.display = '';
                }
            }
            
        }
        
        
        function doSave(tipe) {
            //var errMsg = 'Please see again the error notification.';
            if (isContentDate == false) {
                alert("\"Content Date\" still wrong ");
                document.getElementById("txtYearContent").focus();
            } if (isPublishDate == false) {
                alert("\"Publish Date\" still wrong ");
                document.getElementById("txtYearPublish").focus();
            } if (isExpiredDate == false) {
                alert("\"Expired Date\" still wrong ");
                document.getElementById("txtYearExpired").focus();
            } else {
                if (tipe == 1) {
                    document.getElementById("_save").value = "1";
                } else {
                    document.getElementById("_saveClose").value = "1";
                }
                document.getElementById('txtContent').value = nicEditors.findEditor('txtContent').getContent();
                document.forms[0].submit();
            }
        }
        
        function doDelete(keyword) {
            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_delete").value = "1";
                document.forms[0].submit();
            }
        }
        
        function doDeleteImage(imgRef) {
            document.getElementById("_deleteImage").value = imgRef;
            document.forms[0].submit();
        }

        function doDeleteAttachment(attachRef) {
            document.getElementById("_deleteAttachment").value = attachRef;
            document.forms[0].submit();
        }
        
        function doRemoveImage(imgRef) {
            document.getElementById("_removeImage").value = imgRef;
            document.forms[0].submit();
        }
        
        function doAddContentTag(name, ref) {
            var txtTagName = document.getElementById("txtTagName");
            var txtTagRef = document.getElementById("txtTagRef");
            var isAdd = true;
            
            if (txtTagRef.value != '') {
                for (i = 0; i < txtTagRef.value.split(',').length  ; i++) {
                    if (txtTagRef.value.split(',')[i] != "") {
                        if (txtTagRef.value.split(',')[i] == ref) {
                            isAdd = false;
                            break;
                        }
                    }
                }
            }
            
            if (isAdd) {
                if (txtTagName.value != '') txtTagName.value = txtTagName.value + ', ';
                txtTagName.value = txtTagName.value + name;
                
                if (txtTagRef.value != '') txtTagRef.value = txtTagRef.value + ',';
                txtTagRef.value = txtTagRef.value + ref;
            } else {
                alert("Tag \"" + name + "\" already in the list");
            }
        }
        
        function doRemoveContentTag(name, ref) {
            var txtTagName = document.getElementById("txtTagName");
            var txtTagRef = document.getElementById("txtTagRef");
            var isRemove = false;
            var newTagName = "";
            var newTagRef = "";
            
            if (txtTagRef.value != '') {
                for (i = 0; i < txtTagRef.value.split(',').length  ; i++) {
                    if (txtTagRef.value.split(',')[i] != ref) {
                        if (newTagRef != '') newTagRef = newTagRef + ',';
                        newTagRef = newTagRef + txtTagRef.value.split(',')[i];
                        
                        if (newTagName != '') newTagName = newTagName + ', ';
                        newTagName = newTagName + txtTagName.value.split(', ')[i];
                    } else {
                        isRemove = true;
                    }
                }
                if (!isRemove) {
                    alert('No tag to remove');
                }
            } else {
                alert('No tag to remove');
            }
            
            txtTagName.value = newTagName;
            txtTagRef.value = newTagRef;
        }
        
        function doRemoveContentTagAll() {
            var txtTagName = document.getElementById("txtTagName");
            var txtTagRef = document.getElementById("txtTagRef");
            
            txtTagName.value = "";
            txtTagRef.value = "";
        }
        
        function doOpenTagTree() {
            var divOpenTag = document.getElementById("divOpenTag");
            var divTagTree = document.getElementById("divTagTree");
            var divForm = document.getElementById("divForm");
            
            divTagTree.style.display = "";
            divOpenTag.style.display = "none";
            divForm.style.display = "none";
        }
        
        function doCloseTagTree() {
            var divOpenTag = document.getElementById("divOpenTag");
            var divTagTree = document.getElementById("divTagTree");
            
            divTagTree.style.display = "none";
            divOpenTag.style.display = "";
        }
        
        function doGenerateForm() {
            var txtTagRef = document.getElementById("txtTagRef");
            
            if (txtTagRef.value == '') {
                alert('Please choose tag first');
            } else {
                document.getElementById("_generate").value = "1";
                document.forms[0].submit();
            }
        }
        
        window.onload = function() {
            //checkEmpty(document.getElementById("txtTagName"),document.getElementById("eTagName"));
            cekDate("Content", document.getElementById("eDateContent"));
            cekDate("Publish", document.getElementById("eDatePublish"));
            cekDate("Expired", document.getElementById("eDateExpired"));
            countLengthTextArea(document.getElementById('txtSynopsis'),document.getElementById('divSynopsisLimit'),1000);
            
            if (_txtTagRef == '') {
                doOpenTagTree();
            } else {
                doCloseTagTree();
            }
            
            
            
            var divForm = document.getElementById("divForm");
            if (_isForm == "1") {
                divForm.style.display = "";
            } else {
                divForm.style.display = "none";
            }
            
            var iconPath = _rootPath + "plugin/editor/nicEditorIcons.gif"
            var txtContent = new nicEditor({iconsPath: iconPath, fullPanel : true }).panelInstance('txtContent');
            
            document.getElementById("divLoadingForm").style.display = "none";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_generate" name="_generate" type="hidden" value="0"/>
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_saveClose" name="_saveClose" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>
    <input id="_deleteImage" name="_deleteImage" type="hidden" value=""/>
    <input id="_deleteAttachment" name="_deleteAttachment" type="hidden" value=""/>
    <input id="_removeImage" name="_removeImage" type="hidden" value=""/>
    <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
        Notification: Please choose tag and klik "Generate Form"
    </div>
    <div class="row">
        <div class="col-md-4"><h6>Tag</h6></div>
        <div class="col-md-8">
            <div class="" style="width:390px;">
                <input name="txtTagName" id="txtTagName" type="text" value="<%=txtTagName%>" style="width:377px; color:#c83232;" readonly="readonly" />
                <input name="txtTagRef" id="txtTagRef" type="hidden" value="<%=txtTagRef%>" />
                <div id="divOpenTag"><a href="javascript:doOpenTagTree();"class="btn btn-sm btn-dark"><span>Open Tag Tree</span></a></div>
                <div id="divTagTree" style="display:none;">
                    <asp:Literal ID="ltrTag" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row" style="margin-top:20px;">
        <div class="col-md-4"></div>
        <div class="col-md-8">
                <a href="javascript:doGenerateForm();" class="btn btn-md btn-dark"><span>Generate Form</span></a> 
        </div>
        <div class="clear"></div>
    </div>
    <div class="row" id="divLoadingForm">
        <div>Loading form ...</div>
        <div class="clear"></div>
    </div>
    <div id="divForm" style="display:none;" class="mt10">
        <div class="row">
            <div class="lCol"></div>
            <div class="mCol"></div>
            <div class="rCol">
                <div class="linkBtn left mr5">
                    <a href="javascript:doSave(2);">Save & Close</a>
                </div> 
                <div class="linkBtn left mr5">
                    <a href="javascript:doSave(1);">Save</a>
                </div> 
                <asp:Literal ID="ltrBtnTop" runat="server"></asp:Literal>
                <div class="linkBtn left">
                    <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();">Close</a>
                </div> 
                
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4"><h6>Content Type</h6></div>
            <div class="col-md-8">
                <asp:Literal ID="ltrContentType" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4"><h6>Image Setting</h6></div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImageSetting" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4"><h6>Image Position</h6></div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImagePosition" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
        <div class="col-md-4"><h6>Meta Title</h6></div>
            <div class="col-md-8">
                <input name="txtMetaTitle" id="txtMetaTitle" type="text" value="<%=txtMetaTitle%>" onkeyup="countLengthTextArea(this,document.getElementById('divTitleLimit'),100);" />
                <div id="divTitleLimit" class="mcol">100 char left</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4"><h6></h6>Meta Author</div>>
            <div class="col-md-8">
                <input  name="txtMetaAuthor" id="txtMetaAuthor" type="text" value="<%=txtMetaAuthor%>" onkeyup="countLengthTextArea(this,document.getElementById('divAuthorLimit'),100);" />
                <div id="divAuthorLimit" class="mcol">100 char left</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4"><h6>Meta Keyword</h6></div>
            <div class="col-md-8">
                <input  name="txtKeyword" id="txtKeyword" type="text" value="<%=txtKeyword%>" onkeyup="countLengthKeyword(this,document.getElementById('divKeywordLimit'),10);" />
                <div id="divKeywordLimit" class="mcol">10 keyword left, Separate by "," (coma)</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Meta Description</div>
            <div class="mCol"></div>
            <div class="rCol">
                <textarea class="ta" id="txtMetaDescription" name="txtMetaDescription" cols="" rows="" onkeyup="countLengthTextArea(this,document.getElementById('divMetaDescLimit'),160);"><%=txtMetaDescription%></textarea>
                <div id="divMetaDescLimit" class="fNotif">160 char left</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divThumbnail" style="display:none;">
            <div class="lCol">Thumbnail</div>
            <div class="mCol"></div>
            <div class="rCol">
                <asp:Literal ID="ltrThumbnail" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divPicture" style="display:none;">
            <div class="lCol">Picture</div>
            <div class="mCol"></div>
            <div class="rCol ov">
                <asp:Literal ID="ltrPicture" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divAttachment" style="display:none;">
            <div class="lCol">Attachment</div>
            <div class="mCol"></div>
            <div class="rCol">
                <asp:Literal ID="ltrAttachment" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divVideo" style="display:none;">
            <div class="lCol">Video</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtVideo" id="txtVideo" type="text" placeholder="Insert your embed address video here" value="<%=txtVideo%>" />
                <label style="float: left;margin-right: 3px;margin-top: 5px;">https://www.youtube.com/embed/</label>
                <div style="margin-top: 4px; margin-left:182px">ex: https://www.youtube.com/embed/GeMqI519bH8 </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divTitle" style="display:none;">
            <div class="lCol">Title</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtTitle" id="txtTitle" type="text" value="<%=txtTitle%>" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divTitleDetail" style="display:none;">
            <div class="lCol">Title Detail</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtTitleDetail" id="txtTitleDetail" type="text" value="<%=txtTitleDetail%>" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divSynopsis" style="display:none;">
            <div class="lCol">
                Synopsis
                <div id="divSynopsisLimit" class="fNotif ov mt5">1000 char left</div>
            </div>
            <div class="mCol"></div>
            <div class="rCol">
                <textarea class="ta" id="txtSynopsis" name="txtSynopsis" onkeyup="countLengthTextArea(this,document.getElementById('divSynopsisLimit'),1000);"><%=txtSynopsis%></textarea>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divContent" style="display:none;">
            <div class="mb5">Content</div>
            <div>
                <textarea id="txtContent" name="txtContent" style="width: 530px; height: 300px;"><%=txtContent%></textarea>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divContentDate" style="display:none;">
            <div class="lCol">Content Date</div>
            <div class="mCol"></div>
            <div class="rCol">
                <div>
                    <div class="left mr5"><span id="spanDayContent" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthContent" runat="server"></span></div>
                    <div class="left mr5"><input id="txtYearContent" name="txtYearContent" value="<%=txtYearContent%>" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('Content', document.getElementById('eDateContent'));" /></div>
                </div>
                <div>
                    <div id="eDateContent" class="fNotif" style="display: none;">* "Content Date" still wrong</div>            
                </div>
                
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divPublishDate" style="display:none;">
            <div class="lCol">Publish Date</div>
            <div class="mCol"></div>
            <div class="rCol">
                <div>
                    <div class="left mr5"><span id="spanDayPublish" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthPublish" runat="server"></span></div>
                    <div class="left mr5"><input id="txtYearPublish" name="txtYearPublish" value="<%=txtYearPublish%>" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('Publish', document.getElementById('eDatePublish'));" /></div>
                </div>
                <div>
                    <div id="eDatePublish" class="fNotif" style="display: none;">* "Publish Date" still wrong</div>            
                </div>               
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divExpiredDate" style="display:none;">
            <div class="lCol">Expired Date</div>
            <div class="mCol"></div>
            <div class="rCol">
                <div>
                    <div class="left mr5"><span id="spanDayExpired" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthExpired" runat="server"></span></div>
                    <div class="left mr5"><input id="txtYearExpired" name="txtYearExpired" value="<%=txtYearExpired%>" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('Expired', document.getElementById('eDateExpired'));" /></div>
                </div>
                <div>
                    <div id="eDateExpired" class="fNotif" style="display: none;">* "Expired Date" still wrong</div>            
                </div>
                
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divSortNo" style="margin-top:20px;">
            <div class="col-md-4">
			    <h6 >Sort No</h6>
		    </div>
            <div class="col-md-8">
                <input name="txtSortNo" id="txtSortNo" type="text" value="<%=txtSortNo%>" />
            </div>
        </div>
        <div class="row" id="divHit" style="display:none;">
            <div class="lCol">Hit / View</div>
            <div class="mCol"></div>
            <div class="rCol">
                <div id="divHitView" runat="server" class="bold mb10 mt3"></div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <a href="javascript:doSave(2);" class="btn btn-md btn-dark"><span>Save & Close</span></a>
                <a href="javascript:doSave(1);" class="btn btn-md btn-dark"><span>Save</span></a>
                <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
                <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();" class="btn btn-md btn-dark"<span>Close</span>></a>
                
            </div>
            <div class="clear"></div>
        </div>
    </div>
    
    <script type="text/javascript"language="javascript">
        var divThumbnail = document.getElementById("divThumbnail")
        var divPicture = document.getElementById("divPicture")
        var divAttachment = document.getElementById("divAttachment")
        var divTitle = document.getElementById("divTitle")
        var divVideo = document.getElementById("divVideo")
        var divTitleDetail = document.getElementById("divTitleDetail")
        var divSynopsis = document.getElementById("divSynopsis")
        var divContent = document.getElementById("divContent")
        var divContentDate = document.getElementById("divContentDate")
        var divPublishDate = document.getElementById("divPublishDate")
        var divExpiredDate = document.getElementById("divExpiredDate")
        var divHit = document.getElementById("divHit")
    
        var _isTitle = '<%=isTitle%>';
        var _isVideo = '<%=isVideo%>';
        var _isTitleDetail = '<%=isTitleDetail%>';
        var _isSynopsis = '<%=isSynopsis%>';
        var _isContent = '<%=isContent%>';
        var _isThumbnail = '<%=isThumbnail%>';
        var _isPicture = '<%=isPicture%>';
        var _isAttachment = '<%=isAttachment%>';
        var _isDate = '<%=isDate%>';
        var _isPublishDate = '<%=isPublishDate%>';
        var _isExpiredDate = '<%=isExpiredDate%>';
    
        if (_isTitle == '1') divTitle.style.display = ''; else divTitle.style.display = 'none';
        if (_isTitleDetail == '1') divTitleDetail.style.display = ''; else divTitleDetail.style.display = 'none';
        if (_isVideo == '1') divVideo.style.display = ''; else divVideo.style.display = 'none';
        if (_isSynopsis == '1') divSynopsis.style.display = ''; else divSynopsis.style.display = 'none';
        if (_isContent == '1') divContent.style.display = ''; else divContent.style.display = 'none';
        if (_isThumbnail == '1') divThumbnail.style.display = ''; else divThumbnail.style.display = 'none';
        if (_isPicture == '1') divPicture.style.display = ''; else divPicture.style.display = 'none';
        if (_isAttachment == '1') divAttachment.style.display = ''; else divAttachment.style.display = 'none';
        if (_isDate == '1') divContentDate.style.display = ''; else divContentDate.style.display = 'none';
        if (_isPublishDate == '1') divPublishDate.style.display = ''; else divPublishDate.style.display = 'none';
        if (_isExpiredDate == '1') divExpiredDate.style.display = ''; else divExpiredDate.style.display = 'none';
        if (_isUpdate == '1') divHit.style.display = ''; else divHit.style.display = 'none';
        
    </script>
</asp:Content>

