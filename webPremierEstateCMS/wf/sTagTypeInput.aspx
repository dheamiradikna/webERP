<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="sTagTypeInput.aspx.vb" Inherits="wf_sTagTypeInput" title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        var isTagTypeName = false;
        
        function checkEmpty(o,e) {
            if (o.value == '') {
                e.style.display = '';
                switch(o.id) {
                    case 'txtTagTypeName': isTagTypeName=false; break;
                }
            } else {
                    e.style.display = 'none';
                    switch(o.id) {
                        case 'txtTagTypeName': isTagTypeName=true; break;
                    }
                }
        }
        
        function doSave(tipe) {
            debugger;
            var errMsg = 'Please see again the error notification.';
            if (isTagTypeName == false) {
                alert(errMsg);
                document.getElementById("txtTagTypeName").focus();
            } else {
                if (tipe == 1) {
                    document.getElementById("_save").value = "1";
                } else {
                    document.getElementById("_saveClose").value = "1";
                }
                document.forms[0].submit();
            }
        }
        
        function doDelete(keyword) {            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_delete").value = "1";
                document.forms[0].submit();
            }
        }
        
        window.onload = function() {
            checkEmpty(document.getElementById("txtTagTypeName"),document.getElementById("eTagTypeName"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_saveClose" name="_saveClose" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>
    

   <%-- <section class="section-wrap checkout pt-0 pb-50">
      <div class="container">--%>

        <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
            Notification: Please fill the data below
        </div>

        <div class="row">
		    <div class="col-md-4">
                <h6>Tag Type Name</h6>
		    </div>
		    <div class="col-md-8">
			    <input name="txtTagTypeName" id="txtTagTypeName" type="text"  value="<%=txtTagTypeName%>" onkeypress="checkEmpty(this,document.getElementById('eTagTypeName'));" />
		         <div id="eTagTypeName" class="fNotif" style="display:none;"></div>
            </div>    
        </div>
        
        <div class="row">
            <div class="col-md-8">
             <%--   <div class="btn btn-md btn-dark">
                    <a href="javascript:doSave(2);">Save & Close</a>
                </div> 
                <div class="btn btn-md btn-dark">
                    <a href="javascript:doSave(1);">Save</a>
                </div> --%>
                <a href="javascript:doSave(2);" class="btn btn-md btn-dark"><span>Save & Close</span></a>	
                <a href="javascript:doSave(1);" class="btn btn-md btn-dark"><span>Save</span></a>	
                	
                <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
                <a href="<%=rootPath%>wf/sTagType.aspx?;" class="btn btn-md btn-dark"><span>Back to List</span></a>
                <%--<div class="btn btn-md btn-dark">
                    <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();">Close</a>
                </div> --%>
            </div>
        <div class="clear"></div>

         </div>

        <script type="text/javascript"language="javascript">
        </script>
 

</asp:Content>

