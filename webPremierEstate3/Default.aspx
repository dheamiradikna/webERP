<%@ Page Language="VB"  MasterPageFile="~/wf/parent.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainSection" runat="Server">
     <div class="j-menu-container"></div>
    <div class="l-main-container">
        <asp:Literal ID="ltrHome" runat="server"></asp:Literal>
        <asp:Literal ID="ltrKeunggulan" runat="server"></asp:Literal>
        <asp:Literal ID="ltrArea" runat="server"></asp:Literal>
        <asp:Literal ID="ltrProperty" runat="server"></asp:Literal>
        <asp:Literal ID="ltrQtyManagement" runat="server"></asp:Literal>
        <asp:Literal ID="ltrMessage" runat="server"></asp:Literal>
        <asp:Literal ID="ltrNewsUpdates" runat="server"></asp:Literal>
   </div>


    <script language="javascript" type="text/javascript">
</script>

        <script type="text/javascript">
            var _noted = '<%=noted%>';

            window.addEventListener("load", function (event) {
                if (_noted != '') {
                    swal(_noted);
                }
            });
        </script>

</asp:Content>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>--%>
