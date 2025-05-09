<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="changePass.aspx.vb" Inherits="wf_changePass" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
    <script type="text/javascript" language="javascript">
        var isOldPassword = false;
        var isNewPassword = false;
        var isRetype = false;
        
        function checkEmpty(o,e) {
            if (o.value == '') {
                e.style.display = '';
                switch(o.id) {
                    case 'txtOldPassword': isOldPassword = false; break;
                    case 'txtNewPassword': isNewPassword = false; break;
                    case 'txtRetype': isRetype = false; break;
                }
            } else {
                    e.style.display = 'none';
                    switch(o.id) {
                        case 'txtOldPassword': isOldPassword = true; break;
                        case 'txtNewPassword': isNewPassword = true; break;
                        case 'txtRetype': isRetype = true; break;
                    }
                }
        }
        
        function doSave() {
            var errMsg = 'Please see again the error notification.';
            if (isOldPassword == false) {
                alert(errMsg);
                document.getElementById("txtOldPassword").focus();
            } else if (isNewPassword == false) {
                alert(errMsg);
                document.getElementById("txtNewPassword").focus();
            } else if (isRetype == false) {
                alert(errMsg);
                document.getElementById("txtRetype").focus();
            } else {
                document.getElementById("_save").value = "1";
                document.forms[0].submit();
            }
        }
        
        
        
        window.onload = function() {
            checkEmpty(document.getElementById("txtOldPassword"), document.getElementById("eOldPassword"));
            checkEmpty(document.getElementById("txtNewPassword"), document.getElementById("eNewPassword"));
            checkEmpty(document.getElementById("txtRetype"), document.getElementById("eRetype"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="" />
    <%--<div class="f2 bold bdrBottom ov">
        <div class="left">Home :: Change Password</div>
    </div>
    <div id="divNotif" class="bdrAll mt5 pd5 fNotif mb10 f2" runat="server">
        Notification: Please fill all the field below and click "Save".
    </div>
    <div class="row">
        <div class="lCol">Old Password</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtOldPassword" id="txtOldPassword" maxlength="20" type="password" value="<%=txtOldPassword%>" onkeyup="checkEmpty(this,document.getElementById('eOldPassword'));" />
            <div id="eOldPassword" class="fNotif mt3" style="display:none;">* Must be filled ...</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">New Password</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtNewPassword" id="txtNewPassword" maxlength="20" type="password" value="<%=txtNewPassword%>" onkeyup="checkEmpty(this,document.getElementById('eNewPassword'));" />
            <div id="eNewPassword" class="fNotif mt3" style="display:none;">* Must be filled ...</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol">Retype New Password</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input class="tb" name="txtRetype" id="txtRetype" maxlength="20" type="password" value="<%=txtRetype%>" onkeyup="checkEmpty(this,document.getElementById('eRetype'));" />
            <div id="eRetype" class="fNotif mt3" style="display:none;">* Must be filled ...</div>
        </div>
        <div class="clear"></div>
    </div>
        
    <div class="row">
        <div class="lCol"></div>
        <div class="mCol"></div>
        <div class="rCol">
            <div class="linkBtn left mr5">
                <a href="javascript:doSave();">Save</a>
            </div> 
        </div>
        <div class="clear"></div>
    </div>--%>

   <section class="section-wrap checkout pt-0 pb-50">
      <div class="container">

        <div class="row heading-row mt-50">
          <div class="col-md-12 text-center">
            <h2 class="heading uppercase"><small>Home :: Change Password</small></h2>
          </div>
        </div>
        <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
            <strong>Notification:</strong> Please fill all the field below and click "Save".
        </div>
        <!-- Forms -->		
			
		<div class="row">
			<div class="col-md-4">
                <h6>Old Password</h6>
                <%--<abbr class="required" title="required">* Must be filled</abbr>--%>
			</div>
			<div class="col-md-8">
				<input name="txtOldPassword" id="txtOldPassword" maxlength="20" type="password" value="<%=txtOldPassword%>" onkeyup="checkEmpty(this,document.getElementById('eOldPassword'));"  />
                
            </div>    
        </div>

        <div class="row">
			<div class="col-md-4">
                <h6>New Password</h6>
			</div>
			<div class="col-md-8">
				<input name="txtNewPassword" id="txtNewPassword" maxlength="20" type="password" value="<%=txtNewPassword%>" onkeyup="checkEmpty(this,document.getElementById('eNewPassword'));"/>
			</div>    
        </div>

        <div class="row">
			<div class="col-md-4">
                <h6>Retype New Password</h6>
			</div>
			<div class="col-md-8">
				<input  name="txtRetype" id="txtRetype" maxlength="20" type="password" value="<%=txtRetype%>" onkeyup="checkEmpty(this,document.getElementById('eRetype'));"/>
			</div>    
        </div>
		
		
		<div class="row" >
			<div class="col-md-8" style="float:right;">
                <a href="javascript:doSave();" class="btn btn-md btn-dark"><span>Save</span></a>	
			</div>
        </div>
			
			

          <!-- end col -->
        </div> <!-- end row -->
	   
	</section>
</asp:Content>

