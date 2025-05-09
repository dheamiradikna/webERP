<%@ Page Title="" Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="sBooking.aspx.vb" Inherits="wf_sBooking" %>

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
            document.getElementById("_sBooking").value = "1";
            document.forms[0].submit();
        }

        function doDelete(ref, keyword) {
            if (confirm("Are you sure to delete this Booking \"" + keyword + "\" ?")) {
                document.getElementById("_sBookingDelete").value = ref;
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
    <input id="_sBooking" name="_sBooking" type="hidden" value="" />
    <input id="_sBookingDelete" name="_sBookingDelete" type="hidden" value="" />
    <div class="f2 bold bdrBottom ov">
        <div class="left">Setting :: Booking Order</div>
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
    <div id="divPaging" class="mt5 mb10" runat="server"></div>
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>

