<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="sTagInput.aspx.vb" Inherits="wf_sTagInput" title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
      .mcol {
              margin-top:-20px;
              margin-bottom: 20px;
            }
    </style>
    <script type="text/javascript" language="javascript">
        var isTagName = false;
        var _ckActive = '<%=ckActive%>';
        var _rootpath = '<%=rootPath%>';
        var _ckIsTitle = '<%=ckIsTitle%>';
        var _ckIsSingleContent = '<%=ckIsSingleContent%>';
        var _ckIsOnlyParent = '<%=ckIsOnlyParent%>';
        var _ckIsTitleDetail = '<%=ckIsTitleDetail%>';
        var _ckIsSynopsis = '<%=ckIsSynopsis%>';
        var _ckIsContent = '<%=ckIsContent%>';
        var _ckIsThumbnail = '<%=ckIsThumbnail%>';
        var _ckIsPicture = '<%=ckIsPicture%>';
        var _ckIsVideo = '<%=ckIsVideo%>';
        var _ckIsMap = '<%=ckIsMap%>';
        var _ckIsAttachment = '<%=ckIsAttachment%>';
        var _ckIsDate = '<%=ckIsDate%>';
        var _ckIsImageSlideShow = '<%=ckIsImageSlideShow%>';
        var _ckIsPictureHover = '<%=ckIsPictureHover%>';
        var _ckIsPublishDate = '<%=ckIsPublishDate%>';
        var _ckIsExpiredDate = '<%=ckIsExpiredDate%>';
        var _ckIsComment = '<%=ckIsComment%>';
        var _ckIsCommentPreApproval = '<%=ckIsCommentPreApproval%>';
        var _ckIsNeedApproval = '<%=ckIsNeedApproval%>';
        var _ckIsPolling = '<%=ckIsPolling%>';
        var _ckIsForum = '<%=ckIsForum%>';
        var _ckIsForumPreApproval = '<%=ckIsForumPreApproval%>';
        var _ckIsDisplay1 = '<%=ckIsDisplay1%>';
        var _ckIsDisplay2 = '<%=ckIsDisplay2%>';
        var _ckIsDisplay3 = '<%=ckIsDisplay3%>';
        var _ckIsDisplay4 = '<%=ckIsDisplay4%>';
        
        function checkEmpty(o,e) {
            if (o.value == '') {
                e.style.display = '';
                switch(o.id) {
                    case 'txtTagName': isTagName=false; break;
                }
            } else {
                    e.style.display = 'none';
                    switch(o.id) {
                        case 'txtTagName': isTagName=true; break;
                    }
                }
        }
        
        function doSave(tipe) {
            var errMsg = 'Please see again the error notification.';
            if (isTagName == false) {
                alert(errMsg);
                document.getElementById("txtTagName").focus();
            } else {
                if (tipe == 1) {
                    document.getElementById("_save").value = "1";
                } else {
                    document.getElementById("_saveClose").value = "1";
                }
                document.forms[0].submit();
            }
        }
        
        function doDelete(keyword) {
            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_delete").value = "1";
                document.forms[0].submit();
            }
        }

        function doClose(keyword) {
   
            window.location = _rootpath + "wf/sTag.aspx";
        }
        
        window.onload = function() {
            checkEmpty(document.getElementById("txtTagName"),document.getElementById("eTagName"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_saveClose" name="_saveClose" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>

    <%--<div id="divNotif" class="bdrAll mt5 pd5 fNotif mb10" runat="server">
        Notification: Please fill the data below
    </div>
    <div class="row">
        <div class="lCol">Picture</div>
        <div class="mCol"></div>
        <div class="rCol">
            <asp:Literal ID="ltrPicture" runat="server"></asp:Literal>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Tag Type</div>
        <div class="mCol"></div>
        <div class="rCol">
            <asp:Literal ID="ltrTagType" runat="server"></asp:Literal>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Parent Tag</div>
        <div class="mCol"></div>
        <div class="rCol">
            <asp:Literal ID="ltrParentTag" runat="server"></asp:Literal>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Tag Name</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtTagName" id="txtTagName" type="text" value="<%=txtTagName%>" onkeyup="checkEmpty(this,document.getElementById('eTagName'));" />
            <div id="eTagName" class="fNotif" style="display:none;">* Must be filled ...</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Meta Title</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtMetaTitle" id="txtMetaTitle" type="text" value="<%=txtMetaTitle%>" onkeyup="countLengthTextArea(this,document.getElementById('divTitleLimit'),100);" />
            <div id="divTitleLimit" class="fNotif">100 char left</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Meta Author</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtMetaAuthor" id="txtMetaAuthor" type="text" value="<%=txtMetaAuthor%>" onkeyup="countLengthTextArea(this,document.getElementById('divAuthorLimit'),100);" />
            <div id="divAuthorLimit" class="fNotif">100 char left</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Meta Keyword</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtKeyword" id="txtKeyword" type="text" value="<%=txtKeyword%>" onkeyup="countLengthKeyword(this,document.getElementById('divKeywordLimit'),10);" />
            <div id="divKeywordLimit" class="fNotif">10 keyword left, Separate by "," (coma)</div>
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
    <div class="row">
        <div class="lCol">Description</div>
        <div class="mCol"></div>
        <div class="rCol">
            <textarea class="ta" id="txtDescription" name="txtDescription" cols="" rows="" ><%=txtDescription%></textarea>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Sort No</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtSortNo" id="txtSortNo" type="text" value="<%=txtSortNo%>" onkeyup="toNumeric(this);" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Test Link</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtTestLink" id="txtTestLink" type="text" value="<%=txtTestLink%>" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Thumb Width</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtThumbW" id="txtThumbW" type="text" value="<%=txtThumbW%>" onkeyup="toNumeric(this);" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Thumb Height</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtThumbH" id="txtThumbH" type="text" value="<%=txtThumbH%>" onkeyup="toNumeric(this);" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Pic Width</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtPicW" id="txtPicW" type="text" value="<%=txtPicW%>" onkeyup="toNumeric(this);" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Pic Height</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtPicH" id="txtPicH" type="text" value="<%=txtPicH%>" onkeyup="toNumeric(this);" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Approval User</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtApprovalUser" id="txtApprovalUser" type="text" value="<%=txtApprovalUser%>" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Is Active</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input id="ckActive" name="ckActive" type="checkbox" checked="checked" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row bdrBottom ov pd5">
        <div class="f2 right">Setting</div>    
    </div>
    <div class="clear"></div>
    <div class="row">
        <table class="tbl" cellpadding="5" cellspacing="1">
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsSingleContent" name="ckIsSingleContent" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Single Content</div></td>
                <td><div class="left mr5"><input id="ckIsOnlyParent" name="ckIsOnlyParent" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Only Parent</div></td>
                <td><div class="left mr5"><input id="ckIsVideo" name="ckIsVideo" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Video</div></td>
                <td><div class="left mr5"><input id="ckIsMap" name="ckIsMap" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Map</div></td>
            </tr>
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsTitle" name="ckIsTitle" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Title</div></td>
                <td><div class="left mr5"><input id="ckIsTitleDetail" name="ckIsTitleDetail" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Title Detail</div></td>
                <td><div class="left mr5"><input id="ckIsSynopsis" name="ckIsSynopsis" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Synopsis</div></td>
                <td><div class="left mr5"><input id="ckIsContent" name="ckIsContent" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Content</div></td>
            </tr>
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsThumbnail" name="ckIsThumbnail" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Thumbnail</div></td>
                <td><div class="left mr5"><input id="ckIsPicture" name="ckIsPicture" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Picture</div></td>
                <td><div class="left mr5"><input id="ckIsAttachment" name="ckIsAttachment" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Attachment</div></td>
                <td><div class="left mr5"><input id="ckIsDate" name="ckIsDate" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Date</div></td>
            </tr>
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsPictureHover" name="ckIsPictureHover" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Picture Hover</div></td>
                <td><div class="left mr5"><input id="ckIsImageSlideShow" name="ckIsImageSlideShow" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Image Slideshow</div></td>
                <td><div class="left mr5"><input id="ckIsComment" name="ckIsComment" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Comment</div></td>
                <td><div class="left mr5"><input id="ckIsCommentPreApproval" name="ckIsCommentPreApproval" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Comment Pre Approval</div></td>
            </tr>
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsPublishDate" name="ckIsPublishDate" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Publish Date</div></td>
                <td><div class="left mr5"><input id="ckIsExpiredDate" name="ckIsExpiredDate" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Expired Date</div></td>
                <td><div class="left mr5"><input id="ckIsForum" name="ckIsForum" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Forum</div></td>
                <td><div class="left mr5"><input id="ckIsForumPreApproval" name="ckIsForumPreApproval" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Forum Pre Approval</div></td>
            </tr>
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsNeedApproval" name="ckIsNeedApproval" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Need Approval</div></td>
                <td><div class="left mr5"><input id="ckIsPolling" name="ckIsPolling" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Is Polling</div></td>
                <td><div class="left mr5"><input id="ckIsDisplay2" name="ckIsDisplay2" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Display #2</div></td>
                <td><div class="left mr5"><input id="ckIsDisplay3" name="ckIsDisplay3" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Display #3</div></td>
            </tr>
            <tr class="tblRow1">
                <td><div class="left mr5"><input id="ckIsDisplay4" name="ckIsDisplay4" type="checkbox" style="vertical-align:middle;" /></div><div class="mt5 left">Display #4</div></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            
        </table>
    </div>
    <div class="row bdrBottom ov"></div>
    <div class="clear mb10"></div>
    <div class="row">
        <div class="lCol">Content Display</div>
        <div class="mCol"></div>
        <div class="rCol">
            <asp:Literal ID="ltrContentDisplay" runat="server"></asp:Literal>
        </div>
        <div class="clear"></div>
    </div>
    
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
            <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
            <div class="linkBtn left">
                <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();">Close</a>
            </div> 
            
        </div>
        <div class="clear"></div>
    </div>--%>
    	<%--<section class="section-wrap checkout pt-0 pb-50">
        <div class="container">--%>

		<div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
            <strong>Notification:</strong> For input new item, please click on icon + above.
        </div>

			<!-- Forms -->
			<div class="contact-name mb-30">
				<div class="row">
					<div class="col-md-4">
						<h6>Picture</h6>
					</div>
					<div class="col-md-8">
						<%--<div id="divUploadFileSIUP">
							<input name="fileSIUP" id="fileSIUP" type="file" class="file" data-show-upload="false"  data-preview-file-type="text"/> 
							<p id="eFileSIUP" style="margin-bottom:20px; display:none; font-size:12px; color:red;" ></p>
						</div>--%>
                        <asp:Literal ID="ltrPicture" runat="server"></asp:Literal>
					</div>    
				</div>
			</div> <!-- end col- -->
			
			<div class="row">
				<div class="col-md-4">
					<h6>Tag Type</h6>
				</div>
				<div class="col-md-8">
					<asp:Literal ID="ltrTagType" runat="server"></asp:Literal>
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Parent Tag</h6>
				</div>
				<div class="col-md-8">
					 <asp:Literal ID="ltrParentTag" runat="server"></asp:Literal>
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Tag Name</h6>
				</div>
				<div class="col-md-8">
					<input name="txtTagName" id="txtTagName" type="text" value="<%=txtTagName%>" onkeyup="checkEmpty(this,document.getElementById('eTagName'));" />
                    <div id="eTagName" class="mcol" style="display:none;">* Must be filled ...</div>
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Meta Title</h6>
				</div>
				<div class="col-md-8">
					<input  name="txtMetaTitle" id="txtMetaTitle" type="text" value="<%=txtMetaTitle%>" onkeyup="countLengthTextArea(this,document.getElementById('divTitleLimit'),100);" />
                    <div id="divTitleLimit" class="fNotif">100 char left</div>
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
					<input  name="txtKeyword" id="txtKeyword" type="text" value="<%=txtKeyword%>" onkeyup="countLengthKeyword(this,document.getElementById('divKeywordLimit'),10);" />
                    <div id="divKeywordLimit" class="mcol">10 keyword left, Separate by "," (coma)</div>
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Meta Description</h6>
				</div>
				<div class="col-md-8">
					<textarea id="txtMetaDescription" name="txtMetaDescription" cols="" rows="" onkeyup="countLengthTextArea(this,document.getElementById('divMetaDescLimit'),160);"><%=txtMetaDescription%></textarea>
                    <div id="divMetaDescLimit" class="mcol">160 char left</div>
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Description</h6>
				</div>
				<div class="col-md-8">
					<textarea class="ta" id="txtDescription" name="txtDescription" cols="" rows="" ><%=txtDescription%></textarea>
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Sort No</h6>
				</div>
				<div class="col-md-8">
					<input name="txtSortNo" id="txtSortNo" type="text" value="<%=txtSortNo%>" onkeyup="toNumeric(this);" />
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Test Link</h6>
				</div>
				<div class="col-md-8">
					 <input name="txtTestLink" id="txtTestLink" type="text" value="<%=txtTestLink%>" />
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Thumb Width</h6>
				</div>
				<div class="col-md-8">
					 <input name="txtThumbW" id="txtThumbW" type="text" value="<%=txtThumbW%>" onkeyup="toNumeric(this);" />
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Thumb Height</h6>
				</div>
				<div class="col-md-8">
					 <input name="txtThumbH" id="txtThumbH" type="text" value="<%=txtThumbH%>" onkeyup="toNumeric(this);" />
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Pic Width</h6>
				</div>
				<div class="col-md-8">
					<input class="tb" name="txtPicW" id="txtPicW" type="text" value="<%=txtPicW%>" onkeyup="toNumeric(this);" />
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Pic Height</h6>
				</div>
				<div class="col-md-8">
					 <input class="tb" name="txtPicH" id="txtPicH" type="text" value="<%=txtPicH%>" onkeyup="toNumeric(this);" />
				</div>    
			</div>
			
			<div class="row">
				<div class="col-md-4">
					<h6>Approval User</h6>
				</div>
				<div class="col-md-8">
					<input class="tb" name="txtApprovalUser" id="txtApprovalUser" type="text" value="<%=txtApprovalUser%>" />
				</div>    
			</div>
			
			<div class="contact-name mb-30">
				<div class="row">
					<div class="col-md-4">
						<h6>Is Active</h6>
					</div>
					<div class="col-md-8">
						 <input id="ckActive" class="input-checkbox" name="ckActive" type="checkbox" checked="checked" />
                        <label for="ckActive"></label>
					</div>    
				</div>
			</div>
		<div class="row">
          <div class="col-md-12">
            <div class="table-wrap mb-30">
              <table class="table table-bordered">
                <tr  class="cart_item">
                    <td><input id="ckIsSingleContent" name="ckIsSingleContent" type="checkbox" /><label for="ckIsSingleContent">Is Single Content</label></td>
                    <td><input id="ckIsOnlyParent" name="ckIsOnlyParent" type="checkbox"  /><label for="ckIsOnlyParent">Is Only Parent</label></td>
                    <td><input id="ckIsVideo" name="ckIsVideo" type="checkbox"  /><label for="ckIsVideo">Is Video</label></td>
                    <td><input id="ckIsMap" name="ckIsMap" type="checkbox"  /><label for="ckIsMap">Is Map</label></td>
               </tr>
			   <tr  class="cart_item">
                    <td><input id="ckIsTitle" name="ckIsTitle" type="checkbox"  /><label for="ckIsTitle">Is Title</label></td>
                    <td><input id="ckIsTitleDetail" name="ckIsTitleDetail" type="checkbox"  /><label for="ckIsTitleDetail">Is Title Detail</label></td>
                    <td><input id="ckIsSynopsis" name="ckIsSynopsis" type="checkbox"  /><label for="ckIsSynopsis">Is Synopsis</label></td>
                    <td><input id="ckIsContent" name="ckIsContent" type="checkbox"  /><label for="ckIsContent">Is Content</label></td>
               </tr>
			   <tr  class="cart_item">
                    <td><input id="ckIsThumbnail" name="ckIsThumbnail" type="checkbox"  /><label for="ckIsThumbnail">Is Thumbnail</label></td>
                    <td><input id="ckIsPicture" name="ckIsPicture" type="checkbox"  /><label for="ckIsPicture">Is Picture</label></td>
                    <td><input id="ckIsAttachment" name="ckIsAttachment" type="checkbox"  /><label for="ckIsAttachment">Is Attachment</label></td>
                    <td><input id="ckIsDate" name="ckIsDate" type="checkbox"  /><label for="ckIsDate">Is Date</label></td>
               </tr>
			   <tr  class="cart_item">
                    <td><input id="ckIsPictureHover" name="ckIsPictureHover" type="checkbox"  /><label for="ckIsPictureHover">Is Picture Hover</label></td>
                    <td><input id="ckIsImageSlideShow" name="ckIsImageSlideShow" type="checkbox"  /><label for="ckIsImageSlideShow">Is Image Slideshow</label></td>
                    <td><input id="ckIsComment" name="ckIsComment" type="checkbox"  /><label for="ckIsComment">Is Comment</label></td>
                    <td><input id="ckIsCommentPreApproval" name="ckIsCommentPreApproval" type="checkbox"  /><label for="ckIsCommentPreApproval">Is Comment Pre Approval</label></td>
               </tr>
			   <tr  class="cart_item">
                    <td><input id="ckIsPublishDate" name="ckIsPublishDate" type="checkbox" /><label for="ckIsPublishDate">Is Publish Date</label></td>
                    <td><input id="ckIsExpiredDate" name="ckIsExpiredDate" type="checkbox" /><label for="ckIsExpiredDate">Is Expired Date</label></td>
                    <td><input id="ckIsForum" name="ckIsForum" type="checkbox"  /><label for="ckIsForum">Is Forum</label></td>
                    <td><input id="ckIsForumPreApproval" name="ckIsForumPreApproval" type="checkbox" /><label for="ckIsForumPreApproval">Is Forum Pre Approval</label></td>
               </tr>
			   <tr  class="cart_item">
                    <td><input id="ckIsNeedApproval" name="ckIsNeedApproval" type="checkbox"  /><label for="ckIsNeedApproval">Is Need Approval</label></td>
                    <td><input id="ckIsPolling" name="ckIsPolling" type="checkbox"  /><label for="ckIsPolling">Is Polling</label></td>
                    <td><input id="ckIsDisplay2" name="ckIsDisplay2" type="checkbox"  /><label for="ckIsDisplay2">Display #2</label></td>
                    <td><input id="ckIsDisplay3" name="ckIsDisplay3" type="checkbox"  /><label for="ckIsDisplay3">Display #3</label></td>
               </tr>
			   <tr  class="cart_item">
                    <td><input id="ckIsDisplay4" name="ckIsDisplay4" type="checkbox" style="vertical-align:middle;" /><label for="ckIsDisplay4">Display #4</label></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
             </table>
            </div>
          </div> <!-- end col -->
        </div> <!-- end row -->

        <div class="row">
			<div class="col-md-4">
				<h6>Content Display</h6>
			</div>
			<div class="col-md-8">
				<asp:Literal ID="ltrContentDisplay" runat="server"></asp:Literal>
			</div>    
		</div>
		
		
		<div class="row" >
			<div class="col-md-12" style="text-align:center;">
               
                <a href="javascript:doSave(2);" class="btn btn-md btn-dark"><span>Save & Close</span></a>
                <a href="javascript:doSave(1);" class="btn btn-md btn-dark"><span>Save</span></a>
                <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
                <a href="javascript:doClose();" class="btn btn-md btn-dark"><span>Close</span></a>
			</div>
        </div>
		
		
	  <%--</div>
	</section>--%>








    <script type="text/javascript"language="javascript">
        if (_ckActive == '1') document.getElementById("ckActive").checked = true; else document.getElementById("ckActive").checked = false;
        if (_ckIsSingleContent == '1') document.getElementById("ckIsSingleContent").checked = true; else document.getElementById("ckIsSingleContent").checked = false;
        if (_ckIsOnlyParent == '1') document.getElementById("ckIsOnlyParent").checked = true; else document.getElementById("ckIsOnlyParent").checked = false;
        if (_ckIsTitle == '1') document.getElementById("ckIsTitle").checked = true; else document.getElementById("ckIsTitle").checked = false;
        if (_ckIsTitleDetail == '1') document.getElementById("ckIsTitleDetail").checked = true; else document.getElementById("ckIsTitleDetail").checked = false;
        if (_ckIsSynopsis == '1') document.getElementById("ckIsSynopsis").checked = true; else document.getElementById("ckIsSynopsis").checked = false;
        if (_ckIsContent == '1') document.getElementById("ckIsContent").checked = true; else document.getElementById("ckIsContent").checked = false;
        if (_ckIsThumbnail == '1') document.getElementById("ckIsThumbnail").checked = true; else document.getElementById("ckIsThumbnail").checked = false;
        if (_ckIsPicture == '1') document.getElementById("ckIsPicture").checked = true; else document.getElementById("ckIsPicture").checked = false;
        if (_ckIsVideo == '1') document.getElementById("ckIsVideo").checked = true; else document.getElementById("ckIsVideo").checked = false;
        if (_ckIsMap == '1') document.getElementById("ckIsMap").checked = true; else document.getElementById("ckIsMap").checked = false;
        if (_ckIsAttachment == '1') document.getElementById("ckIsAttachment").checked = true; else document.getElementById("ckIsAttachment").checked = false;
        if (_ckIsDate == '1') document.getElementById("ckIsDate").checked = true; else document.getElementById("ckIsDate").checked = false;
        if (_ckIsAttachment == '1') document.getElementById("ckIsAttachment").checked = true; else document.getElementById("ckIsAttachment").checked = false;
        if (_ckIsPictureHover == '1') document.getElementById("ckIsPictureHover").checked = true; else document.getElementById("ckIsPictureHover").checked = false;
        if (_ckIsImageSlideShow == '1') document.getElementById("ckIsImageSlideShow").checked = true; else document.getElementById("ckIsImageSlideShow").checked = false;
        if (_ckIsExpiredDate == '1') document.getElementById("ckIsExpiredDate").checked = true; else document.getElementById("ckIsExpiredDate").checked = false;
        if (_ckIsComment == '1') document.getElementById("ckIsComment").checked = true; else document.getElementById("ckIsComment").checked = false;
        if (_ckIsCommentPreApproval == '1') document.getElementById("ckIsCommentPreApproval").checked = true; else document.getElementById("ckIsCommentPreApproval").checked = false;
        if (_ckIsNeedApproval == '1') document.getElementById("ckIsNeedApproval").checked = true; else document.getElementById("ckIsNeedApproval").checked = false;
        if (_ckIsPolling == '1') document.getElementById("ckIsPolling").checked = true; else document.getElementById("ckIsPolling").checked = false;
        if (_ckIsForum == '1') document.getElementById("ckIsForum").checked = true; else document.getElementById("ckIsForum").checked = false;
        if (_ckIsForumPreApproval == '1') document.getElementById("ckIsForumPreApproval").checked = true; else document.getElementById("ckIsForumPreApproval").checked = false;
        //if (_ckIsDisplay1 == '1') document.getElementById("ckIsDisplay1").checked = true; else document.getElementById("ckIsDisplay1").checked = false;
        if (_ckIsDisplay2 == '1') document.getElementById("ckIsDisplay2").checked = true; else document.getElementById("ckIsDisplay2").checked = false;
        if (_ckIsDisplay3 == '1') document.getElementById("ckIsDisplay3").checked = true; else document.getElementById("ckIsDisplay3").checked = false;
        if (_ckIsDisplay4 == '1') document.getElementById("ckIsDisplay4").checked = true; else document.getElementById("ckIsDisplay4").checked = false;
    </script>
</asp:Content>

