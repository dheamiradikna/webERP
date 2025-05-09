<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="contentTagSingle.aspx.vb" Inherits="wf_contentTagSingle" title="" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
       .mcol {
              margin-top:-20px;
              margin-bottom: 20px;
            }
        .nicEdit-panel {background-color: #fff !important;}
        .nicEdit-button {background-color: #fff !important;}
        
    </style>
    <script language="javascript" type="text/javascript" src="<%=rootPath%>plugin/editor/nicEdit.js"></script>
    <script language="javascript" type="text/javascript">
        var isContentDate = true;
        var isPublishDate = true;
        var isExpiredDate = true;

        var _rootPath = '<%=rootPath%>';
        var _isUpdate = '<%=isUpdate%>';
        var _isVideo = '<%=isVideo%>';
        
        var _isPictureHover = '<%=isPictureHover%>';
        var _isImageSlideshow = '<%=isImageSlideshow%>';
        
        var _isTitle = '<%=isTitle%>';       
        var _isTitleDetail = '<%=isTitleDetail%>';
        var _isMapLatitude = '<%=isMapLatitude%>';
        var _isMapLongitude = '<%=isMapLongitude%>';
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
                    if (_isDate == "1") isContentDate = false;
                    e.style.display = '';
                } else if (ctrl == "Publish") {
                    if (_isPublishDate == "1") isPublishDate = true;
                    e.style.display = 'none';
                } else if (ctrl == "Expired") {
                    if (_isExpiredDate == "1") isExpiredDate = true;
                    e.style.display = 'none';
                }

            } else {
                date = Right('00' + selDay.value, 2) + '/' + Right('00' + selMonth.value, 2) + '/' + Right('0000' + txtYear.value, 4);
                if (dateValidation(date) == true) {

                    if (ctrl == "Content") {
                        if (_isDate == "1") isContentDate = true;
                    } else if (ctrl == "Publish") {
                        if (_isPublishDate == "1") isPublishDate = true;
                    } else if (ctrl == "Expired") {
                        if (_isExpiredDate == "1") isExpiredDate = true;
                    }
                    e.style.display = 'none';
                } else {
                    if (ctrl == "Content") {
                        if (_isDate == "1") isContentDate = false;
                    } else if (ctrl == "Publish") {
                        if (_isPublishDate == "1") isPublishDate = false;
                    } else if (ctrl == "Expired") {
                        if (_isExpiredDate == "1") isExpiredDate = false;
                    }
                    e.style.display = '';
                }
            }

        }


        function doSave() {
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
                document.getElementById("_save").value = "1";
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
        

        window.onload = function() {
            cekDate("Content", document.getElementById("eDateContent"));
            cekDate("Publish", document.getElementById("eDatePublish"));
            cekDate("Expired", document.getElementById("eDateExpired"));
            countLengthTextArea(document.getElementById('txtSynopsis'), document.getElementById('divSynopsisLimit'), 1000);

            var iconPath = _rootPath + "plugin/editor/nicEditorIcons.gif"
            var txtContent = new nicEditor({ iconsPath: iconPath, fullPanel: true }).panelInstance('txtContent');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>
    <input id="_deleteImage" name="_deleteImage" type="hidden" value=""/>
    <input id="_deleteAttachment" name="_deleteAttachment" type="hidden" value=""/>
    <input id="_removeImage" name="_removeImage" type="hidden" value=""/>
    
    <div class="f2 bold bdrBottom ov">
        <div class="alert alert-warning fade in alert-dismissible" id="divTitleTop" runat="server"></div>
        
    </div>


    <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
        Notification: -
    </div>
    
    
    <div id="divForm" class="mt10">
        <%--<div class="row">
            <div class="lCol"></div>
            <div class="mCol"></div>
            <div class="rCol">
               <div class="linkBtn left mr5">
                    <a href="javascript:doSave();">Save</a>
                </div> 
                <asp:Literal ID="ltrBtnTop" runat="server"></asp:Literal>
                
            </div>
            <div class="clear"></div>
        </div>--%>
        <div class="row">
            <div class="col-md-4">
				<h6>Content Type</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrContentType" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4">
				<h6>Image Setting</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImageSetting" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
				<h6>Image Position</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImagePosition" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
		        <h6>Meta Title</h6>
		    </div>
            <div class="col-md-8">
                <input  name="txtMetaTitle" id="txtMetaTitle" type="text" value="<%=txtMetaTitle%>" onkeyup="countLengthTextArea(this,document.getElementById('divTitleLimit'),100);" />
                <div id="divTitleLimit" class="mcol">100 char left</div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
	            <h6>Meta Author</h6>
	        </div>
            <div class="col-md-8">
                <input name="txtMetaAuthor" id="txtMetaAuthor" type="text" value="<%=txtMetaAuthor%>" onkeyup="countLengthTextArea(this,document.getElementById('divAuthorLimit'),100);" />
                <div id="divAuthorLimit" class="mcol">100 char left</div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
			    <h6>Meta Keyword</h6>
		     </div>
            <div class="col-md-8">
                <input name="txtKeyword" id="txtKeyword" type="text" value="<%=txtKeyword%>" onkeyup="countLengthKeyword(this,document.getElementById('divKeywordLimit'),10);" />
                <div id="divKeywordLimit" class="mcol">10 keyword left, Separate by "," (coma)</div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
			    <h6>Meta Description</h6>
		     </div>
            <div class="col-md-8">
                <textarea  id="txtMetaDescription" name="txtMetaDescription" cols="" rows="" onkeyup="countLengthTextArea(this,document.getElementById('divMetaDescLimit'),160);"><%=txtMetaDescription%></textarea>
                <div id="divMetaDescLimit" class="mcol">160 char left</div>
            </div>
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
             <div class="col-md-4"><h6>Picture</h6></div>
             <div class="col-md-8">
                <asp:Literal ID="ltrPicture" runat="server"></asp:Literal>
            </div>
        </div>
        
        <div class="row" id="divPictureHover" style="display:none;">
            <div class="lCol">Picture Hover</div>
            <div class="mCol"></div>
            <div class="rCol ov">
                <asp:Literal ID="ltrPictureHover" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        
        <div class="row" id="divImageSlideshow" style="display:none;">
            <div class="col-md-4">
				<h6>Image Slideshow</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImageSlideshow" runat="server"></asp:Literal>
            </div>
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
            <div class="col-md-4">
			    <h6>Video</h6>
		    </div>
            <div class="col-md-4">
                 <label >https://www.youtube.com/embed/</label>   
            </div>
            <div class="col-md-4">
                <input name="txtVideo" id="txtVideo" type="text" placeholder="Insert your embed address video here" value="<%=txtVideo%>" />
                <div class="mcol">ex: https://www.youtube.com/embed/GeMqI519bH8 </div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divTitle" style="display:none;">
            <div class="col-md-4">
			    <h6>Title</h6>
		    </div>
            <div class="col-md-8">
                <input name="txtTitle" id="txtTitle" type="text" value="<%=txtTitle%>" />
            </div>       
        </div>
        <div class="row" id="divTitleDetail" style="display:none;">
            <div class="col-md-4">
			    <h6>Title Detail</h6>
		    </div>
            <div class="col-md-8">
                <input  name="txtTitleDetail" id="txtTitleDetail" type="text" value="<%=txtTitleDetail%>" />
            </div>
        </div>

         <div class="row" id="divMapLatitude" style="display:none;">
            <div class="col-md-4"> 
                <h6>Map Latitude</h6>
            </div>
            <div class="col-md-8">
                <input name="txtMapLatitude" id="txtMapLatitude" type="text" value="<%=txtMapLatitude%>" />
            </div>
            <div class="clear"></div>
        </div>

         <div class="row" id="divMapLongitude" style="display:none;">
            <div class="lCol">Map Longitude</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtMapLongitude" id="txtMapLongitude" type="text" value="<%=txtMapLongitude%>" />
            </div>
            <div class="clear"></div>
            </div>
        <div class="row" id="divSynopsis" style="display:none;">
            <div class="col-md-4">
                <h6>Synopsis</h6>  
            </div>
            <div class="col-md-8">
                <textarea id="txtSynopsis" name="txtSynopsis" onkeyup="countLengthTextArea(this,document.getElementById('divSynopsisLimit'),300);"><%=txtSynopsis%></textarea>
                <div id="divSynopsisLimit" class="mcol">300 char left</div>
            </div>
        </div>
        <div class="row" id="divContent" style="display:none;">
            <div class="col-md-4">
                <h6>Content</h6>
            </div>
            <div class="col-md-8">
                <textarea id="txtContent" name="txtContent" style="height: 300px;"><%=txtContent%></textarea>
            </div>
        </div>

        <div class="row" id="divContentDate" style="display:none;">
            <div class="col-md-4"><h6>Content Date</h6></div>
            <div class="col-md-8">
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
            <div class="col-md-4"><h6>Publish Date</h6></div>
            <div class="col-md-8">
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
            <div class="col-md-4"><h6>Expired Date</h6></div>
            <div class="col-md-8"></div>
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
            <div class="col-md-4"><h6>Hit / View</h6></div>
            <div class="col-md-8">
                <div id="divHitView" runat="server" class=""></div>
            </div>
            <div class="clear"></div>
        </div>

        <d iv class="row">
            <div class="col-md-12">
                <a href="javascript:doSave();" class="btn btn-md btn-dark"><span>Save</span></a> 
                <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>  
            </div>
            <div class="clear"></div>
        </d>
    </div>
    
    <script type="text/javascript"language="javascript">
        var divThumbnail = document.getElementById("divThumbnail")
        var divPicture = document.getElementById("divPicture")
        var divAttachment = document.getElementById("divAttachment")
        var divVideo = document.getElementById("divVideo")
        var divTitle = document.getElementById("divTitle")      
        var divTitleDetail = document.getElementById("divTitleDetail")
        var divMapLatitude = document.getElementById("divMapLatitude")
        var divMapLongitude = document.getElementById("divMapLongitude")
        var divSynopsis = document.getElementById("divSynopsis")
        var divContent = document.getElementById("divContent")
        var divContentDate = document.getElementById("divContentDate")
        var divPublishDate = document.getElementById("divPublishDate")
        var divExpiredDate = document.getElementById("divExpiredDate")
        var divHit = document.getElementById("divHit")

        var divPictureHover = document.getElementById("divPictureHover")
        var divImageSlideshow = document.getElementById("divImageSlideshow")

        var _isVideo = '<%=isVideo%>';
        var _isPictureHover = '<%=isPictureHover%>';
        var _isImageSlideshow = '<%=isImageSlideshow%>';
        
        
        var _isTitle = '<%=isTitle%>';
        var _isTitleDetail = '<%=isTitleDetail%>';
        var _isMap = '<%=isMap%>';
        var _isMapLatitude = '<%=isMapLatitude%>';
        var _isMapLongitude = '<%=isMapLongitude%>';
        var _isSynopsis = '<%=isSynopsis%>';
        var _isContent = '<%=isContent%>';
        var _isThumbnail = '<%=isThumbnail%>';
        var _isPicture = '<%=isPicture%>';
        var _isAttachment = '<%=isAttachment%>';
        var _isDate = '<%=isDate%>';
        var _isPublishDate = '<%=isPublishDate%>';
        var _isExpiredDate = '<%=isExpiredDate%>';

        if (_isVideo == '1') divVideo.style.display = ''; else divVideo.style.display = 'none';

        if (_isPictureHover == '1') divPictureHover.style.display = ''; else divPictureHover.style.display = 'none';
        if (_isImageSlideshow == '1') divImageSlideshow.style.display = ''; else divImageSlideshow.style.display = 'none';
        
        if (_isTitle == '1') divTitle.style.display = ''; else divTitle.style.display = 'none';
        if (_isTitleDetail == '1') divTitleDetail.style.display = ''; else divTitleDetail.style.display = 'none';
        if (_isMap == '1') {
            divMapLatitude.style.display = '';
            divMapLongitude.style.display = '';
        } else {
            divMapLatitude.style.display = 'none';
            divMapLongitude.style.display = 'none';
        }
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