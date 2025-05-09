<%@ Page Title="" Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="sNewsletter.aspx.vb" Inherits="wf_sNewsletter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script type="text/javascript">

        function submitenter(myfield, e) {
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

        function doSearch() {
            document.getElementById("_sNewsletter").value = "1";
            document.forms[0].submit();
        }

        function doDelete(ref, keyword) {
            if (confirm("Are you sure to delete this feedback \"" + keyword + "\" ?")) {
                document.getElementById("_sNewsletterDelete").value = ref;
                document.forms[0].submit();
            }
        }

        function openDetail(i) {
            document.getElementById("tr" + i).style.display = "";
            document.getElementById("divDetailPanel" + i).innerHTML = "<a href='javascript:closeDetail(" + i + ");'>close</a>";
        }

        function closeDetail(i) {
            document.getElementById("tr" + i).style.display = "none";
            document.getElementById("divDetailPanel" + i).innerHTML = "<a href='javascript:openDetail(" + i + ");'>detail</a>";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_sNewsletter" name="_sNewsletter" type="hidden" value="" />
    <input id="_sNewsletterDelete" name="_sNewsletterDelete" type="hidden" value="" />

<%--    <div class="f2 bold bdrBottom ov">
        <div class="left">Setting :: Feedback</div>
    </div>
    <div id="divNotif" class="bdrAll mt5 pd5 fNotif" runat="server">
        Notification: -
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
    <div id="divTable" class="mt10" runat="server"></div>
    <div id="divPaging" class="mt5 mb10" runat="server"></div>--%>

      <section class="section-wrap checkout pt-0 pb-50">
      <div class="container">
        <div class="left mr10">
            <asp:Literal ID="ltrDomain" runat="server"></asp:Literal>
        </div>

        <div class="row heading-row mt-50">
          <div class="col-md-12 text-center">
            <h2 class="heading uppercase"><small>Setting :: Newsletter</small></h2>
          </div>
        </div>
        <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
            <strong>Notification: -</strong> 
        </div>

        <div id="divSubTitle" class="entry-meta" runat="server"></div>

        <!-- Forms -->
        		
		<div class="row">
			<div class="col-md-4">
                <h6>Keyword</h6>
			</div>
			<div class="col-md-8">
				<input name="txtKeyword" id="txtKeyword" type="text"  value="<%=txtKeyword%>" onkeypress="return submitenter(this,event)" />
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
		
		<div class="row" >
			<div class="col-md-8" style="float:right;">
                <a href="javascript:doSearch();" class="btn btn-md btn-dark"><span>Search</span></a>	
			</div>
        </div>

        <div id="divTable" class="mt10" runat="server"></div>

		<div id="divPaging" class="pagination clear" runat="server"></div>	
			

          <!-- end col -->
        </div> <!-- end row -->
	</section>
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>

