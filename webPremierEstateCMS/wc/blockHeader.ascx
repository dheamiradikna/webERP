<%@ Control Language="VB" AutoEventWireup="false" CodeFile="blockHeader.ascx.vb" Inherits="wc_blockHeader" %>
<%--<div id="header">
    <div class="mb10 ov">
        <div class="left"><img src="<%=rootPath%>support/image/logo1.png" height="70" /></div>
        <div class="right f0 black" style="vertical-align:baseline;"><div style="height:40px;"></div><div><%=domainName%></div></div>
    </div>
    <div>
        <div id="menuTopBox">
            <div id="menuTop" style="">
                <ul>
                    <li><a href="<%=rootPath%>wf/index.aspx">Home</a>
                        <ul>
                            <li><a href="<%=rootPath%>wf/changePass.aspx">Change Password</a></li>
                            <li><a href="<%=rootPath%>wf/logout.aspx">Logout</a></li>
                        </ul>
                    </li>
                    <li><a href="#">Setting</a>
                        <ul>
                            <li><a href="<%=rootPath%>wf/admin/mUser.aspx">User</a></li>
                            <li><a href="<%=rootPath%>wf/sFeedback.aspx">Feedback</a></li>
                            <li><a href="<%=rootPath%>wf/sMeta.aspx?hr=1">Meta</a></li>
                            <li><a href="<%=rootPath%>wf/sTagType.aspx">Tag Type</a></li>
                            <li><a href="<%=rootPath%>wf/sTag.aspx">Tag</a></li>
                        </ul>
                    </li>

                    <li><a href="#">Web Module</a>
                        <ul>
                            <li><a href="<%=rootPath%>wf/wmContent.aspx">Content</a></li>
                        </ul>
                    </li>
                   
                    
                    <asp:Literal ID="ltrTag" runat="server"></asp:Literal>
                </ul>

            </div>
        </div>

        <div class="clear">
        </div>
    </div>
</div>--%>
<header class="nav-type-1">
      <nav class="navbar navbar-static-top">
        <div class="navigation" id="sticky-nav">
          <div style="background-color: #efeeea;" class="container relative">
            <div class="row">
              <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-collapse">
                  <span class="sr-only">Toggle navigation</span>
                  <span class="icon-bar"></span>
                  <span class="icon-bar"></span>
                  <span class="icon-bar"></span>
                </button>                              
              </div>
              <div class="header-wrap">
                <div class="header-wrap-holder">
                  <div class="logo-container">
                    <div class="logo-wrap text-center">
                      <a href="index.aspx">
                        <img class="logo" src="<%=rootPath%>support/image/logo_NC_CMS.png" alt="logo" style="max-height:90px;">
                      </a>
                    </div>
                  </div>
                </div>
              </div>
              <div style="background-color: #999999;" class="nav-wrap">
                <div class="collapse navbar-collapse" id="navbar-collapse">
                  <ul class="nav navbar-nav">
                    <li class="dropdown">
                      <a href="<%=rootPath%>wf/index.aspx"">Home</a>
                        <i class="fa fa-angle-down dropdown-toggle" data-toggle="dropdown"></i>
                        <ul class="dropdown-menu">
                          <li><a href="<%=rootPath%>wf/changePass.aspx"">Change Password</a></li>
                          <li><a href="<%=rootPath%>wf/logout.aspx">Logout</a></li>
                      </ul>
                    </li>

                    <li class="dropdown">
                      <a href="#">Setting</a>
                      <i class="fa fa-angle-down dropdown-toggle" data-toggle="dropdown"></i>
                      <ul class="dropdown-menu">
                        <li><a href="<%=rootPath%>wf/admin/mUser.aspx">User</a></li>
                        <li><a href="<%=rootPath%>wf/sFeedback.aspx">Feedback</a></li>
                        <li><a href="<%=rootPath%>wf/sNewsletter.aspx">Newsletter</a></li>
                        <li><a href="<%=rootPath%>wf/sMeta.aspx?hr=1">Meta</a></li>
                        <li><a href="<%=rootPath%>wf/sTagType.aspx">Tag Type</a></li>
                        <li><a href="<%=rootPath%>wf/sTag.aspx">Tag</a></li>
                        <%--<li><a href="<%=rootPath%>wf/mWebsiteChecker.aspx">Website Checker</a></li>--%>
                      </ul>
                    </li>

                    <li class="dropdown">
                      <a href="#">Web Module</a>
                      <i class="fa fa-angle-down dropdown-toggle" data-toggle="dropdown"></i>
                      <ul class="dropdown-menu">
                        <li><a href="<%=rootPath%>wf/wmContent.aspx">Content</a></li>
                     </ul>
                    </li>

                    <asp:Literal ID="ltrTag" runat="server"></asp:Literal>

                  </ul>
                </div>
              </div> 
          
            </div>
          </div>
        </div>
      </nav> 
    </header>
