<%@ Page Language="VB" MasterPageFile="~/wf/parent.master" AutoEventWireup="false" CodeFile="searchAll.aspx.vb" Inherits="wf_searchAll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="topSection" Runat="Server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainSection" Runat="Server">
    <asp:Literal runat="server" ID="ltrContent"></asp:Literal>
    <script src="https://cdn.jsdelivr.net/npm/lazyload@2.0.0-beta.2/lazyload.js"></script>

    <script type="text/javascript">

        window.onload = function () {
            lazyload();
        }
    </script>
  
</asp:Content>
