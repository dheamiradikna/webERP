<%@ Page Language="VB" MasterPageFile="~/wf/def.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="wf_admin_default" title="" %>

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
        div
        {
        	overflow:hidden;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var _isEmail = '<%=isEmail%>';
        var _isPassword = '<%=isPassword%>';
        
        function doSubmit() {
            document.getElementById("_default").value = "1";
            document.forms[0].submit();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_default" name="_default" type="hidden" value="" />
    <div>
        <div style="width:920px; height:100px;"></div>
        <div class="clear"></div>
        <div style="">
            <div class="left" style="width:180px; height:100px;"></div>
            <div class="left mr10">
                <img alt="natawebsite.com" src="<%=rootPath%>support/image/logo1.png" />
            </div>
            <div id="boxLogin">
                <div id="divNotif" class="bdrAll fNotif m10 pd5" runat="server">Notification: Username is Email</div>
                <div class="f3" style="padding-left:40px; padding-top:10px;">
                    <div class="row">
                        <div class="f2 bold mb5" style="height:20px;">Admin Area</div>
                    </div>
                    <div class="row">
                        <div class="lCol">Username <span id="spEmail" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input id="txtEmail" name="txtEmail" type="text" style="width:250px;" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">Password <span id="spPassword" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input id="txtPassword" name="txtPassword" type="text" style="width:250px;" /></div>
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
        if (_isEmail == "1") document.getElementById("spEmail").style.display = "";
        if (_isPassword == "1") document.getElementById("spPassword").style.display = "";
    </script>
</asp:Content>

