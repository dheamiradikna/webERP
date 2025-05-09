<%@ Page Language="VB" MasterPageFile="~/wf/defAdmin.master" AutoEventWireup="false" CodeFile="mDomain.aspx.vb" Inherits="wf_admin_mDomain" title="Natawebsite.com :: Master :: Mall" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .row
        {
        	margin-bottom:7px;
        }
        .lCol
        {
        	width:100px;
        	float:left;
        	padding-top:4px;
        }
        .mCol
        {
        	width:10px;
        	float:left;
        }
        .rCol
        {
        	float:left;
        }
        .tHighlight{
            background-color:#bebe90;
            color:#FFF;
            cursor:default;
            font-size:11px;
        }
        
    </style>
    <script language="javascript" type="text/javascript">
        var _rbActive = '<%=rbActive%>';
        
        var updRowNo = -1;
        var updRef = -1;
        
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
        
        function doRefresh() {
            if (updRowNo != -1) {
                updateRow(updRef, updRowNo);
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
            document.getElementById("_mDomain").value = "1";
            document.forms[0].submit();
        }
        
        function doDelete(ref, keyword) {
            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_mDomainDelete").value = ref;
                document.forms[0].submit();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_mDomain" name="_mDomain" type="hidden" value="" />
    <input id="_mDomainDelete" name="_mDomainDelete" type="hidden" value="" />
    <div class="f2 bold bdrBottom ov">
        <div class="left">Master :: Mall</div>
        <div class="left mb5 ml10">
            <a href="javascript:showPopup(-1,-1,'<%=rootPath%>wf/admin/mDomainPopup.aspx?');"><img border="0" alt="input" src="<%=rootPath%>support/image/icon_add.png" height="20" /></a>
            
        </div>
    </div>
    <div id="divNotif" class="bdrAll mt5 pd5 fNotif" runat="server">
        Notification: For input new item, please click on icon + above.
    </div>
    <div class="mt15">
        <div class="row">
            <div class="lCol">Is Active</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input type="radio" name="rbActive" value="1" style="vertical-align:middle;" checked="checked"  />&nbsp;All&nbsp;&nbsp;&nbsp;
                <input type="radio" name="rbActive" value="2" style="vertical-align:middle;"/>&nbsp;Yes&nbsp;&nbsp;&nbsp;
                <input type="radio" name="rbActive" value="3" style="vertical-align:middle;"/>&nbsp;No
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Level</div>
            <div class="mCol"></div>
            <div class="rCol">
                <asp:Literal ID="ltrDomainLevel" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Keyword</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input name="txtKeyword" id="txtKeyword" type="text" style="width: 300px;" value="<%=txtKeyword%>" onkeypress="return submitenter(this,event)" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Sort By</div>
            <div class="mCol"></div>
            <div class="rCol">
                <asp:Literal ID="ltrSortBy" runat="server"></asp:Literal>
                <asp:Literal ID="ltrSortType" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol"></div>
            <div class="mCol"></div>
            <div class="rCol">
                <div class="linkBtn">
                    <a href="javascript:doSearch();">Search</a>
                    
                </div> 
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div id="divTable" class="mt10" runat="server">
        
    </div>
    <div id="divPaging" class="mt5 mb10" runat="server"></div>
    <script type="text/javascript" language="javascript">
        if (_rbActive != '') document.getElementsByName("rbActive")[_rbActive - 1].checked = true;
    </script>
</asp:Content>

