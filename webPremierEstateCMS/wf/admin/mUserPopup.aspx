<%@ Page Language="VB" MasterPageFile="~/wf/defPopup.master" AutoEventWireup="false" CodeFile="mUserPopup.aspx.vb" Inherits="wf_admin_mUserPopup" title="Natawebsite.com :: Master :: User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
      .mcol {
              margin-top:-20px;
              margin-bottom: 20px;
            }
    </style>
    <script type="text/javascript" language="javascript">
        var isEmail = false;
        var isPassword = false;
        
        function checkEmpty(o,e) {
            if (o.value == '') {
                e.style.display = '';
                switch(o.id) {
                    case 'txtPassword': isPassword=false; break;
                }
            } else {
                    e.style.display = 'none';
                    switch(o.id) {
                        case 'txtPassword': isPassword=true; break;
                    }
                }
        }
        function emailValidation(addr) {
            var emailReg = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;
            var regex = new RegExp(emailReg);
            return regex.test(addr);
        }
        
        function checkEmail(o,e) {
            if (o.value.trim() != '') {
                if (emailValidation(o.value) == true) {
                    e.style.display = 'none';
                    isEmail = true;
                } else {
                    e.style.display = '';
                    isEmail = false;
                }
            } else {
                e.style.display = '';
                isEmail = false;
            }
        }
        
        function doSave(tipe) {
            var errMsg = 'Please see again the error notification.';
            if (isEmail == false) {
                alert(errMsg);
                document.getElementById("txtEmail").focus();
            } else if (isPassword == false) {
                alert(errMsg);
                document.getElementById("txtPassword").focus();
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
        
        window.onload = function() {
            checkEmpty(document.getElementById("txtPassword"),document.getElementById("ePassword"));
            checkEmail(document.getElementById("txtEmail"),document.getElementById("eEmail"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_saveClose" name="_saveClose" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>

    <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
        Notification: Please fill the data below
    </div>

    <div class="row">
        <div class="col-md-4">
			<h6>User Status</h6>
		</div>
        <div class="col-md-8">
            <asp:Literal ID="ltrUserStatus" runat="server"></asp:Literal>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
			<h6>Email</h6>
		</div>
        <div class="col-md-8">
            <input name="txtEmail" id="txtEmail" type="text" value="<%=txtEmail%>" onkeyup="checkEmail(this,document.getElementById('eEmail'));" />
            <div id="eEmail" class="mcol" style="display:none;">* Must be filled with valid email ...</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>Password</h6>
		</div>
        <div class="col-md-8">
            <input  name="txtPassword" id="txtPassword" type="text" value="<%=txtPassword%>" onkeyup="checkEmpty(this,document.getElementById('ePassword'));" />
            <div id="ePassword" class="mcol" style="display:none;">* Must be filled ...</div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>Name</h6>
		</div>
        <div class="col-md-8">
            <input  name="txtName" id="txtName" type="text" value="<%=txtName%>" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>HP</h6>
		</div>
        <div class="col-md-8">
            <input name="txtHP" id="txtHP" type="text" value="<%=txtHP%>" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>Phone</h6>
		</div>
        <div class="col-md-8">
            <input  name="txtPhone" id="txtPhone" type="text" value="<%=txtPhone%>" />
        </div>
    </div>
    <div class="row" style="display:none">
        <div class="col-md-4"></div>
        <div class="col-md-8">
            <div><input id="ckEmailNotif" name="ckEmailNotif" type="checkbox" style="vertical-align:middle;" /></div><div class="left" style="padding-top:4px;">Send email notification to this user.</div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <a href="javascript:doSave(2);" class="btn btn-md btn-dark"><span>Close</span></a>
            <a href="javascript:doSave(1);" class="btn btn-md btn-dark"><span>Save</span></a>
            <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
            <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();" class="btn btn-md btn-dark"><span>Close</span></a> 
        </div>
        <div class="clear"></div>
    </div>
    <script type="text/javascript"language="javascript">
    </script>
</asp:Content>

