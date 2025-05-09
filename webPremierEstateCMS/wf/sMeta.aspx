<%@ Page Language="VB" MasterPageFile="~/wf/defWeb.master" AutoEventWireup="false" CodeFile="sMeta.aspx.vb" Inherits="wf_sMeta" title="" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        .mcol {
              margin-top:-20px;
              margin-bottom: 20px;
            }
       
        .nicEdit-panel {background-color: #fff !important;}
        .nicEdit-button {background-color: #fff !important;}
        
    </style>
    <script language="javascript" type="text/javascript">
        var rootPath = '<%=rootPath%>';
    </script>
    <script language="javascript" type="text/javascript" src="<%=rootPath%>plugin/editor/nicEdit.js"></script>
    <script language="javascript" type="text/javascript">
        var isMetaTitle = false;
        var isMetaAuthor = false;
        var isMetaKeyword = false;
        var isMetaDescription = false;

        function checkEmpty(o, e) {
            if (o.value == '') {
                e.style.display = '';
                switch (o.id) {
                    case 'txtMetaTitle': isMetaTitle = false; break;
                    case 'txtMetaAuthor': isMetaAuthor = false; break;
                    case 'txtMetaKeyword': isMetaKeyword = false; break;
                    case 'txtMetaDescription': isMetaDescription = false; break;
                }
            } else {
                e.style.display = 'none';
                switch (o.id) {
                    case 'txtMetaTitle': isMetaTitle = true; break;
                    case 'txtMetaAuthor': isMetaAuthor = true; break;
                    case 'txtMetaKeyword': isMetaKeyword = true; break;
                    case 'txtMetaDescription': isMetaDescription = true; break;
                }
            }
        }


        function doSave() {
            var errMsg = 'Please see again the error notification.';
            if (isMetaTitle == false) {
                alert(errMsg);
                document.getElementById("txtMetaTitle").focus();
            } else if (isMetaAuthor == false) {
                alert(errMsg);
                document.getElementById("txtMetaAuthor").focus();
            } else if (isMetaKeyword == false) {
                alert(errMsg);
                document.getElementById("txtMetaKeyword").focus();
            } else if (isMetaDescription == false) {
                alert(errMsg);
                document.getElementById("txtMetaDescription").focus();
            } else {
                document.getElementById("_save").value = "1";
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
            checkEmpty(document.getElementById("txtMetaTitle"), document.getElementById("eMetaTitle"));
            checkEmpty(document.getElementById("txtMetaAuthor"), document.getElementById("eMetaAuthor"));
            checkEmpty(document.getElementById("txtMetaKeyword"), document.getElementById("eMetaKeyword"));
            checkEmpty(document.getElementById("txtMetaDescription"), document.getElementById("eMetaDescription"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>
    

   <section class="section-wrap checkout pt-0 pb-50">
    <div class="container">

    <div class="row heading-row mt-50">
        <div class="col-md-12 text-center">
         <div class="heading uppercase" id="divTitleTop" runat="server"></div>
        </div>
    </div>
    
    <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
        Info: Page Meta ini untuk setting meta secara general (di home)
        <br />
        <strong>Notification: -</strong>
    </div>
    
    
    <div id="divForm" class="mt10">
       <%--<div class="row">
            <div class="lCol"></div>
            <div class="mCol"></div>
            <div class="rCol">
               <div class="linkBtn left mr5">
                    <a href="javascript:doSave();">Save</a>
                </div> 
                <asp:Literal ID="ltrBtnTop" runat="server"></asp:Literal>
                
            </div>
            <div class="clear"></div>
        </div>--%>
        
        <div class="row">
			<div class="col-md-4">
                <h6>Meta Title</h6>
			</div>
			<div class="col-md-8">
				 <input name="txtMetaTitle" id="txtMetaTitle" type="text" value="<%=txtMetaTitle%>" onkeyup="countLengthTextArea(this,document.getElementById('divTitleLimit'),100); checkEmpty(this,document.getElementById('eMetaTitle'));" />
			     <div id="divTitleLimit" class="mcol">100 char left</div>
                 <div id="eMetaTitle" class="mcol" style="display:none;">* Must be filled ...</div>
            </div>    
        </div>

        <div class="row">
            <div class="col-md-4">
                <h6>Meta Author</h6>
			</div>
            <div class="col-md-8">
                <input class="tb" name="txtMetaAuthor" id="txtMetaAuthor" type="text" value="<%=txtMetaAuthor%>" onkeyup="countLengthTextArea(this,document.getElementById('divAuthorLimit'),100); checkEmpty(this,document.getElementById('eMetaAuthor'));" />
                <div id="divAuthorLimit" class="mcol">100 char left</div>
                <div id="eMetaAuthor" class="mcol" style="display:none;">* Must be filled ...</div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <h6>Meta Keyword</h6>
            </div>
            <div class="col-md-8">
                <input class="tb" name="txtMetaKeyword" id="txtMetaKeyword" type="text" value="<%=txtMetaKeyword%>" onkeyup="countLengthKeyword(this,document.getElementById('divKeywordLimit'),10); checkEmpty(this,document.getElementById('eMetaKeyword'));" />
                <div id="divKeywordLimit" class="mcol">10 keyword left, Separate by "," (coma)</div>
                <div id="eMetaKeyword" class="mcol" style="display:none;">* Must be filled ...</div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <h6>Meta Description</h6>
            </div>
            
            <div class="col-md-8">
                <textarea class="ta" id="txtMetaDescription" name="txtMetaDescription" cols="" rows="" onkeyup="countLengthTextArea(this,document.getElementById('divMetaDescLimit'),160); checkEmpty(this,document.getElementById('eMetaDescription'));"><%=txtMetaDescription%></textarea>
                <div id="divMetaDescLimit" class="mcol">160 char left</div>
                <div id="eMetaDescription" class="mcol" style="display:none;">* Must be filled ...</div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divHit" style="display:none;">
            <div class="lCol">Hit / View</div>
            <div class="mCol"></div>
            <div class="rCol">
                <div id="divHitView" runat="server" class="bold mb10 mt3"></div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row">
            <div class="col-md-8" style="float:right;">
                  <a href="javascript:doSave();" class="btn btn-md btn-dark"><span>Save</span></a>
                
                  <asp:Literal ID="ltrBtn" runat="server"></asp:Literal> 
             </div> 
                     
        </div>
    
</div>
    </div> <!-- end row -->
	</section>
</asp:Content>

