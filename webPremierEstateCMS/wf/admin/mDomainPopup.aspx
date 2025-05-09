<%@ Page Language="VB" MasterPageFile="~/wf/defPopup.master" AutoEventWireup="false" CodeFile="mDomainPopup.aspx.vb" Inherits="wf_admin_mDomainPopup" title="Natawebsite.com :: Master :: Mall" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .tb
        {
        	width: 400px;
        }
        .ta
        {
        	width: 400px;
        	height: 100px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var isDomainName = false;
        var _ckActive = '<%=ckActive%>';
        var _ckUpcoming = '<%=ckUpcoming%>';
        var _selCountry = '<%=selCountry%>';
        var _selProvince = '<%=selProvince%>';
        var _selCity = '<%=selCity%>';

        
        function checkEmpty(o,e) {
            if (o.value == '') {
                e.style.display = '';
                switch(o.id) {
                    case 'txtDomainName': isDomainName=false; break;
                }
            } else {
                    e.style.display = 'none';
                    switch(o.id) {
                        case 'txtDomainName': isDomainName=true; break;
                    }
                }
        }
        
        function doSave(tipe) {
            var errMsg = 'Please see again the error notification.';
            if (isDomainName == false) {
                alert(errMsg);
                document.getElementById("txtDomainName").focus();
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

        function doDeleteImage(domainRef) {
            document.getElementById("_deleteImage").value = domainRef;
            document.getElementById("_deleteLogo").value = "";
            document.forms[0].submit();
        }

     

        function doDeleteLogo(domainRef) {
            document.getElementById("_deleteLogo").value = domainRef;
            document.getElementById("_deleteImage").value = "";
            document.forms[0].submit();
        }

        window.onload = function() {
            checkEmpty(document.getElementById("txtDomainName"),document.getElementById("eDomainName"));
            countLengthTextArea(document.getElementById('txtCPAddr'), document.getElementById('divTALimit'), 500);
            countLengthTextArea(document.getElementById('txtDescription'), document.getElementById('divDescriptionLimit'), 500);
            bindSelCountry();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_saveClose" name="_saveClose" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>
    <input id="_deleteImage" name="_deleteImage" type="hidden" value=""/>
    <input id="_deleteLogo" name="_deleteLogo" type="hidden" value=""/>
    <div id="divNotif" class="bdrAll mt5 pd5 fNotif mb10" runat="server">
        Notification: Please fill the data below
    </div>
    <div class="bdrBottomGrey mb10">
        <div class="f2 fnotif mb10 right">Mall Information</div>
        <div class="clear"></div>
        <div class="row">
            <div class="lCol">Level</div>
            <div class="mCol"></div>
            <div class="rCol">
                <asp:Literal ID="ltrDomainLevel" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Mall Name</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtDomainName" id="txtDomainName" type="text" value="<%=txtDomainName%>" onkeyup="checkEmpty(this,document.getElementById('eDomainName'));" />
                <div id="eDomainName" class="fNotif" style="display:none;">* Must be filled ...</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Country</div>
            <div class="mCol"></div>
            <div class="rCol">
                <select id="selCountry" name="selCountry" style="display:none;" onchange="bindSelProvince(this.value);"></select>
                <img id="iCountry" src="<%=rootPath%>support/image/indicatorSmall.gif" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Province</div>
            <div class="mCol"></div>
            <div class="rCol">
                <select id="selProvince" name="selProvince" style="display:none;" onchange="bindSelCity(selCountry.value, this.value);"></select>
                <img id="iProvince" src="<%=rootPath%>support/image/indicatorSmall.gif" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">City</div>
            <div class="mCol"></div>
            <div class="rCol">
                <select id="selCity" name="selCity" style="display:none;" ></select>
                <img id="iCity" src="<%=rootPath%>support/image/indicatorSmall.gif" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">
                Description
                <div id="divDescriptionLimit" class="fNotif ov mt5">500 char left</div>
            </div>
            <div class="mCol"></div>
            <div class="rCol">
                <textarea class="ta" id="txtDescription" name="txtDescription" onkeyup="countLengthTextArea(this,document.getElementById('divDescriptionLimit'),500);"><%=txtDescription%></textarea>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Logo</div>
            <div class="mCol"></div>
            <div class="rCol ov">
                <asp:Literal ID="ltrDomainLogo" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Image</div>
            <div class="mCol"></div>
            <div class="rCol ov">
                <asp:Literal ID="ltrDomainImage" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">Is Upcoming Malls</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input id="ckUpcoming" name="ckUpcoming" type="checkbox" checked="checked" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">IP</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtIP" id="txtIP" type="text" value="<%=txtIP%>" />
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="bdrBottomGrey mb10">
        <div class="f2 fnotif mb10 right">Contact Person Information</div>
        <div class="clear"></div>
        <div class="row">
            <div class="lCol">CP Name</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtCPName" id="txtCPName" type="text" value="<%=txtCPName%>" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">CP Email</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtCPEmail" id="txtCPEmail" type="text" value="<%=txtCPEmail%>" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">CP HP</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtCPHP" id="txtCPHP" type="text" value="<%=txtCPHP%>" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">CP Phone</div>
            <div class="mCol"></div>
            <div class="rCol">
                <input class="tb" name="txtCPPhone" id="txtCPPhone" type="text" value="<%=txtCPPhone%>" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="row">
            <div class="lCol">CP Addr
                <div id="divTALimit" class="fNotif ov mt5">
                        500 char left</div>
            </div>
            <div class="mCol"></div>
            <div class="rCol">
                <textarea class="ta" id="txtCPAddr" name="txtCPAddr" onkeyup="countLengthTextArea(this,document.getElementById('divTALimit'),500);"><%=txtCPAddr%></textarea>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="row">
        <div class="lCol">Is Active</div>
        <div class="mCol"></div>
        <div class="rCol">
            <input id="ckActive" name="ckActive" type="checkbox" checked="checked" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="row">
        <div class="lCol"></div>
        <div class="mCol"></div>
        <div class="rCol">
            <div class="linkBtn left mr5">
                <a href="javascript:doSave(2);">Save & Close</a>
            </div> 
            <div class="linkBtn left mr5">
                <a href="javascript:doSave(1);">Save</a>
            </div> 
            <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
            <div class="linkBtn left">
                <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();">Close</a>
            </div> 
            
        </div>
        <div class="clear"></div>
    </div>
    <script type="text/javascript"language="javascript">
        if (_ckActive == '1') document.getElementById("ckActive").checked = true; else document.getElementById("ckActive").checked = false;
        if (_ckUpcoming == '1') document.getElementById("ckUpcoming").checked = true; else document.getElementById("ckUpcoming").checked = false;

        var selCountry = document.getElementById('selCountry');
        var selProvince = document.getElementById('selProvince');
        var selCity = document.getElementById('selCity');

        var iCountry = document.getElementById('iCountry');
        var iProvince = document.getElementById('iProvince');
        var iCity = document.getElementById('iCity');

    </script>
</asp:Content>

