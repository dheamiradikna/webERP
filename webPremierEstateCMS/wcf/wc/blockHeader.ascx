<%@ Control Language="VB" AutoEventWireup="false" CodeFile="blockHeader.ascx.vb" Inherits="wc_blockHeader" %>
<div id="header">
    <div class="mb10 ov">
        <div class="left"><img src="<%=rootPath%>wcf/support/image/Logo_Nasol.png" height="70" /></div>
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
</div>