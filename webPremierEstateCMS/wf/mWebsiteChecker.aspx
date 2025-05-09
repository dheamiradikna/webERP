<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="mWebsiteChecker.aspx.vb" Inherits="wf_mWebsiteChecker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
        var updRowNo = -1;
        var updRef = -1;
        var _domainRef = '<%=Session("domainRef")%>';
        var _rootPath = '<%=rootPath%>';
        
        
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
            document.getElementById("_mWebsiteChecker").value = "1";
            document.forms[0].submit();
        }
        
        function doDelete(ref, keyword) {
            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_mWebsiteCheckerDelete").value = ref;
                document.forms[0].submit();
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_mWebsiteChecker" name="_mWebsiteChecker" type="hidden" value="" />
    <input id="_mWebsiteCheckerDelete" name="_mWebsiteCheckerDelete" type="hidden" value="" />

   <%-- <div class="f2 bold bdrBottom ov">
        <div class="left">Setting :: Tag Type</div>
        <div class="left mb5 ml10">
            <a href="javascript:showPopup(-1,-1,'<%=rootPath%>wf/sTagTypePopup.aspx?');"><img border="0" alt="input" src="<%=rootPath%>support/image/icon_add.png" height="20" /></a>
        </div>
    </div>
    <div id="divNotif" class="bdrAll mt5 pd5 fNotif" runat="server">
        Notification: For input new item, please click on icon + above.
    </div>
    <div id="divSubTitle" class="mt10 f3" runat="server">
        
    </div>
    <div class="clear"></div>
    <div class="mt15">
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
    </script>--%>

      
        <div class="left mr10">
            <asp:Literal ID="ltrDomain" runat="server"></asp:Literal>
        </div>

        <div class="row heading-row mt-50">
          <div class="col-md-12 text-center">
            <h2 class="heading uppercase"><small>Setting :: Website Checker</small> 
                <%--<a href="<%=rootPath%>wf/sTagTypeInput.aspx?;"><img border="0" alt="input" src="<%=rootPath%>support/image/icon_add.png" height="20" /></a>--%>
                <%--<a href="javascript:showPopup(-1,-1,'<%=rootPath%>wf/sTagTypePopup.aspx?');"><img border="0" alt="input" src="<%=rootPath%>support/image/icon_add.png" height="20" /></a>--%>
           </h2>
          </div>
        </div>
        <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
            <strong>Note :</strong> The Data Below is for Pagespeed Analysis.
        </div>

        <div id="divSubTitle" class="entry-meta" runat="server"></div>

        <!-- Forms -->
        		
		<div class="row">
			<div class="col-md-4">
                <h6>Keyword</h6>
			</div>
			<div class="col-md-8">
				<input name="txtKeyword" id="txtKeyword" type="text"  value="<%=txtKeyword%>" onkeypress="return submitenter(this,event);" />
			</div>    
        </div>
		
		
<%--		<div class="row">
			<div class="col-md-4">
                <h6>Sort by</h6>
			</div>
			<div class="col-md-4">
				<asp:Literal ID="ltrSortBy" runat="server"></asp:Literal>
			</div>  
			<div class="col-md-4">
                <asp:Literal ID="ltrSortType" runat="server"></asp:Literal>
			</div>			
        </div>--%>
		
		<div class="row" >
			<div class="col-md-8" style="float:right;">
                <a href="javascript:doSearch();" class="btn btn-md btn-dark"><span>Search</span></a>	
			</div>
        </div>

        <div id="divTable"  runat="server"></div>

		<div id="divPaging" class="pagination clear" runat="server"></div>	
			

          <!-- end col -->
  
</asp:Content>