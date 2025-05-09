<%@ Page Language="VB" MasterPageFile="~/wf/def.master" AutoEventWireup="false" CodeFile="register.aspx.vb" Inherits="wf_register" title="natawebsite.com :: register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .row
        {
        	margin-bottom:7px;
        	overflow:hidden;
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
        .tb 
        {
        	width:250px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var _isDomain = '<%=isDomain%>';
        var _isEmail = '<%=isEmail%>';
        var _isPassword = '<%=isPassword%>';
        var _isName = '<%=isName%>';
        var _isHP = '<%=isHP%>';
        
        function doSubmit() {
            document.getElementById("_register").value = "1";
            document.forms[0].submit();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_register" name="_register" type="hidden" value="" />
    <div>
        <div style="width:920px; height:80px;"></div>
        <div class="clear"></div>
        <div style="padding-left:275px;" class="f3 mb10">
        
            <a href="<%=rootPath%>default.aspx">Login</a>
            &nbsp;&nbsp;|+|&nbsp;&nbsp;
            <a href="<%=rootPath%>wf/register.aspx"><span class="bdrLink">Register</span></a>
        </div>
        <div>
            <div class="left" style="width:180px; height:200px;"></div>
            <div class="left mr10">
                <img alt="natawebsite.com" src="<%=rootPath%>support/image/logo1.png" />
            </div>
            <div id="boxRegister">
                <div id="divNotif" class="bdrAll fNotif m10 pd5" runat="server">Notification: Username is Email</div>
                <div class="f3 mt5" style="padding-left:40px;">
                    <div class="row">
                        <div class="lCol">Domain <span id="spDomain" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input class="tb" id="txtDomain" name="txtDomain" type="text" value="<%=txtDomain%>" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">Username <span id="spEmail" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input class="tb" id="txtEmail" name="txtEmail" type="text" value="<%=txtEmail%>" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">Password <span id="spPassword" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input  class="tb" id="txtPassword" name="txtPassword" type="text" value="<%=txtPassword%>" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">Name <span id="spName" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input class="tb" id="txtName" name="txtName" type="text" value="<%=txtName%>" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">HP <span id="spHP" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input class="tb" id="txtHP" name="txtHP" type="text" value="<%=txtHP%>" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">&nbsp;</div>
                        <div class="mCol"></div>
                        <div class="rCol">
                            <div class="linkBtn">
                                <a href="javascript:doSubmit();">Submit</a>
                            </div>    
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="clear"></div>
        <div style="width:920px; height:100px;"></div>
    </div>
    <script type="text/javascript" language="javascript">
        if (_isDomain == "1") document.getElementById("spDomain").style.display = "";
        if (_isEmail == "1") document.getElementById("spEmail").style.display = "";
        if (_isPassword == "1") document.getElementById("spPassword").style.display = "";
        if (_isName == "1") document.getElementById("spName").style.display = "";
        if (_isHP == "1") document.getElementById("spHP").style.display = "";
    </script>
</asp:Content>

