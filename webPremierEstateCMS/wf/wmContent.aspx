<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="wmContent.aspx.vb" Inherits="wf_wmContent" title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .mcol {
              margin-top:-20px;
              margin-bottom: 20px;
            }
        
    </style>
    <script language="javascript" type="text/javascript">
        var isContentDate = true;
        var updRowNo = -1;
        var updRef = -1;
        var _domainRef = '<%=Session("domainRef")%>';
        var _rootPath = '<%=rootPath%>';
        
        var _rbApproved = '<%=rbApproved%>';
        var _rbPublish = '<%=rbPublish%>';
        var _rbExpired = '<%=rbExpired%>';
        
        var _txtTagRef = '<%=txtTagRef%>';
        
        
        function submitenter(myfield,e) {
            var keycode;
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            else return true;

            if (keycode == 13) {
               doSearch();
               return false;
            }
            else return true;
        }
        
        function dateValidation(date) {
            var dateReg = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var regex = new RegExp(dateReg);
            return regex.test(date);
        }
        
        function cekDate(d, m, y, e) {
            var date = '';
            var selDay = document.getElementById(d);
            var selMonth = document.getElementById(m);
            var txtYear = document.getElementById(y);
            
            if (selDay.value == '-' && selMonth.value == '-' && txtYear.value == '') {
                isContentDate = true;
                e.style.display = 'none';
            } else {
            
                date = Right('00' + selDay.value,2) + '/' + Right('00' + selMonth.value,2) + '/' + Right('0000' + txtYear.value, 4);
                if (dateValidation(date) == true) {
                    isContentDate = true;
                    e.style.display = 'none';
                } else {
                    isContentDate = false;
                    e.style.display = '';
                }
            }
            
        }
        
        function doRefresh() {
            if (updRowNo != -1) {
                
                updateRow(_domainRef, updRef, updRowNo);
                document.getElementById("tr" + updRowNo).className = 'tHighlight';
            }
        }
        
        function showPopup(rowNo, ref, url) {
            var currentTime = new Date();
            
            if (rowNo == -1) {
                updRowNo = -1;
                updRef = -1; 
            } else {
                updRowNo = rowNo;
                updRef = ref; 
            }
            
            if (window.showModalDialog) { 
                url = url + '&y=' + converStrToParam(currentTime.getTime().toString());
                window.showModalDialog(url,"natawebsite.com","dialogWidth:600px;dialogHeight:400px");
                doRefresh();
            } else {
                window.open(url,"natawebsite","height=400,width=600,toolbar=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,modal=yes");
            }
        }
        
        function doSearch() {
            if (isContentDate == false) {
                alert('Content date still wrong, please check again');
            } else {
                document.getElementById("_wmContent").value = "1";
                document.forms[0].submit();
            }
        }
        
        function doDelete(ref, keyword) {
            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_wmContentDelete").value = ref;
                document.forms[0].submit();
            }
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
            
            divTagTree.style.display = "";
            divOpenTag.style.display = "none";
        }
        
        function doCloseTagTree() {
            var divOpenTag = document.getElementById("divOpenTag");
            var divTagTree = document.getElementById("divTagTree");
            
            divTagTree.style.display = "none";
            divOpenTag.style.display = "";
        }
        
        window.onload = function() {
            if (_txtTagRef == '') {
                //doOpenTagTree();
                doCloseTagTree();
            } else {
                doCloseTagTree();
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_wmContent" name="_wmContent" type="hidden" value="" />
    <input id="_wmContentDelete" name="_wmContentDelete" type="hidden" value="" />

    <div class="row heading-row mt-50">
        <div class="col-md-12 text-center">
        <h2 class="heading uppercase"><small>Web Module :: Content</small> 
            <a href="javascript:showPopup(-1,-1,'<%=rootPath%>wf/wmContentPopup.aspx?');"><img border="0" alt="input" src="<%=rootPath%>support/image/icon_add.png" height="20" /></a>
        </h2>
        </div>
    </div>

    <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
        Notification: For input new item, please click on icon + above.
    </div>
    <div id="divSubTitle" class="mt10 f3" runat="server">
        
    </div>
    <div class="clear"></div>
    <div class="mt15">
        <div class="row">
            <div class="col-md-4">
                <h6>Is Approved</h6>
			</div>
			<div class="col-md-8">
				<input id="rbApproved1" type="radio" class="input-radio" name="rbApproved"  value="1" checked="checked" />
				<label for="rbApproved1" >All</label>
				&nbsp;&nbsp;
				<input id="rbApproved2" type="radio" class="input-radio" name="rbApproved"  value="2" />
				<label for="rbApproved2">Yes</label>
				&nbsp;&nbsp;
				<input id="rbApproved3" type="radio" class="input-radio" name="rbApproved"  value="3" />
				<label for="rbApproved3">No</label>
			</div> 
        </div>

        <div class="row">
            <div class="col-md-4">
                <h6>Is Publish</h6>
			</div>
			<div class="col-md-8">
				<input id="rbPublish1" type="radio" class="input-radio" name="rbPublish"  value="1" checked="checked" />
				<label for="rbApproved1" >All</label>
				&nbsp;&nbsp;
				<input id="rbPublish2" type="radio" class="input-radio" name="rbPublish"  value="2" />
				<label for="rbApproved2">Yes</label>
				&nbsp;&nbsp;
				<input id="rbPublish3" type="radio" class="input-radio" name="rbPublish"  value="3" />
				<label for="rbApproved3">No</label>
			</div> 
        </div>
        <div class="row">
            <div class="col-md-4">
                <h6>Is Expired</h6>
			</div>
			<div class="col-md-8">
				<input id="rbExpired1" type="radio" class="input-radio" name="rbExpired"  value="1" checked="checked" />
				<label for="rbExpired1" >All</label>
				&nbsp;&nbsp;
				<input id="rbExpired2" type="radio" class="input-radio" name="rbExpired"  value="2" />
				<label for="rbExpired2">Yes</label>
				&nbsp;&nbsp;
				<input id="rbExpired3" type="radio" class="input-radio" name="rbExpired"  value="3" />
				<label for="rbExpired3">No</label>
			</div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h6>Content Type</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrContentType" runat="server"></asp:Literal>
            </div>
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
                <h6>Content Date</</h6>
			</div>
            <div class="col-md-8">
                <div>
                    <div class="left mr5"><span id="spanDayContentFr" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthContentFr" runat="server"></span></div>
                    <div class="left mr5">
                        <input value="<%=txtYearContentFr%>" id="txtYearContentFr" name="txtYearContentFr" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('selDayContentFr','selMonthContentFr','txtYearContentFr',document.getElementById('eDateContent'));" />
                        &nbsp;s/d&nbsp;
                    </div>
                    <div class="left mr5"><span id="spanDayContentTo" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthContentTo" runat="server"></span></div>
                    <div class="left mr5">
                        <input value="<%=txtYearContentTo%>" id="txtYearContentTo" name="txtYearContentTo" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('selDayContentTo','selMonthContentTo','txtYearContentTo',document.getElementById('eDateContent'));" />
                    </div>
                </div>
                <div id="eDateContent" class="fNotif" style="display:none;">* Tanggal masih salah</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4"><h6>Tag</h6></div>
            <div class="col-md-8">
                <div class="" style="width:390px;">
                    <input name="txtTagName" id="txtTagName" type="text" value="<%=txtTagName%>" style="width:377px; color:#c83232;" readonly="readonly" />
                    <input name="txtTagRef" id="txtTagRef" type="hidden" value="<%=txtTagRef%>" />
                    <div id="divOpenTag" class="mcol"><a href="javascript:doRemoveContentTagAll();" class="btn btn-sm btn-dark"><span>Remove All Tag</span></a>&nbsp;|&nbsp;<a href="javascript:doOpenTagTree();" class="btn btn-sm btn-dark"><span>Tag Tree</span> </a></div>
                    <div id="divTagTree" style="display:none;">
                        <asp:Literal ID="ltrTag" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <h6>Keyword</h6>
			</div>
			<div class="col-md-8">
                <input name="txtKeyword" id="txtKeyword" type="text" style="width: 300px;" value="<%=txtKeyword%>" onkeypress="return submitenter(this,event)" />
            </div>
        </div>
        <div class="row">
			<div class="col-md-4">
                <h6>Sort by</h6>
			</div>
			<div class="col-md-4">
				 <asp:Literal ID="ltrSortBy" runat="server"></asp:Literal>
			</div>  
			<div class="col-md-4">
				 <asp:Literal ID="ltrSortType" runat="server"></asp:Literal>
			</div>			
        </div>

        <div class="row">
            <div class="col-md-8" style="float:right;">
                <a href="javascript:doSearch();" class="btn btn-md btn-dark"><span>Search</span></a>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div id="divTable" class="mt10" runat="server">
        
    </div>
    <div id="divPaging" class="pagination clear" runat="server"></div>
    <script type="text/javascript" language="javascript">
    </script>
    <script type="text/javascript" language="javascript">
        if (_rbApproved != '') document.getElementsByName("rbApproved")[_rbApproved - 1].checked = true;
        if (_rbPublish != '') document.getElementsByName("rbPublish")[_rbPublish - 1].checked = true;
        if (_rbExpired != '') document.getElementsByName("rbExpired")[_rbExpired - 1].checked = true;
    </script>
</asp:Content>

