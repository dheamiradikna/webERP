<%@ Page Language="VB" MasterPageFile="~/wf/def.master" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="_default" title="natawebsite.com" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        var _isDomain = '<%=isDomain%>';
        var _isEmail = '<%=isEmail%>';
        var _isPassword = '<%=isPassword%>';
        
        function doSubmit() {
            document.getElementById("_default").value = "1";
            document.forms[0].submit();
        }

        function submitenter(myfield, e) {
            var keycode;
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            else return true;

            if (keycode == 13) {
                doSubmit();
                return false;
            }
            else return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_default" name="_default" type="hidden" value="" />
   <%-- <div>
        <div style="width:920px; height:80px;"></div>
        <div class="clear"></div>
        <div style="padding-left:275px;" class="f3 mb10">
        </div>
        <div>
            <div class="left" style="width:180px; height:200px;"></div>
            <div class="left mr10">
                <img src="<%=rootPath%>wcf/support/image/Logo_Nasol.png" width="150px" />
            </div>
            <div id="boxLogin">
                <div id="divNotif" class="bdrAll fNotif m10 pd5" runat="server">Notification: Username is Email</div>
                <div class="f3 mt5" style="padding-left:40px;">
                    <div class="row">
                        <div class="lCol">Username <span id="spEmail" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input class="tb" id="txtEmail" name="txtEmail" type="text" value="<%=txtEmail%>" onkeypress="return submitenter(this,event)" /></div>
                    </div>
                    <div class="row">
                        <div class="lCol">Password <span id="spPassword" class="fNotif" style="display:none;">*</span></div>
                        <div class="mCol"></div>
                        <div class="rCol"><input class="tb" id="txtPassword" name="txtPassword" type="password" value="<%=txtPassword%>" onkeypress="return submitenter(this,event)" /></div>
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
    </div>--%>
    <%--<div class="logo-container">
                    <div class="logo-wrap text-center">
                      <a href="index.html">
                        <img class="logo" src="<%=rootPath%>support/image/logoDM.png" alt="logo">
                      </a>
                    </div>
                  </div>--%>

    <header class="nav-type-1">
      <nav class="navbar navbar-static-top">
        <div class="navigation" id="sticky-nav">
          <div class="container relative">
            <div class="row">
              <div class="header-wrap">
                <div class="header-wrap-holder">
                  <div class="logo-container">
                    <div class="logo-wrap text-center">
                      <a href="#">
                        <img class="logo" src="<%=rootPath%>support/image/logo_NC_CMS.png" alt="logo" style="max-height:90px;">
                      </a>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div> 
      </nav>
    </header> 

    <section class="section-wrap login-register pt-0 pb-40">
      <div class="container">
        <div class="row">
          <div class="col-sm-6 col-sm-offset-3 mb-40">
            <div class="login">
              <h4 class="uppercase">login</h4>
              <div class="row">
                <div class="col-md-12">
                    <asp:Literal ID="ltrNotif" runat="server"></asp:Literal>                        
                </div>
              </div>
              <p class="form-row form-row-wide">
                <label>username or email
                  <abbr class="required" title="required">*</abbr>
                </label>
                <input type="text" class="input-text" id="txtEmail" name="txtEmail"  value="<%=txtEmail%>" onkeypress="return submitenter(this,event)" />
              </p>
              <p class="form-row form-row-wide">
                <label>password
                  <abbr class="required" title="required">*</abbr>
                </label>
                <input type="password" class="input-text" id="txtPassword" name="txtPassword" value="<%=txtPassword%>" onkeypress="return submitenter(this,event)" />
              </p>
              <input style="color:#ffffff;" type="submit"  class="btn" onclick="doSubmit();">
              
            </div>
          </div>
          
        </div>
      </div>
    </section>



    <script type="text/javascript" language="javascript">
        if (_isDomain == "1") document.getElementById("spDomain").style.display = "";
        if (_isEmail == "1") document.getElementById("spEmail").style.display = "";
        if (_isPassword == "1") document.getElementById("spPassword").style.display = "";
    </script>
</asp:Content>

