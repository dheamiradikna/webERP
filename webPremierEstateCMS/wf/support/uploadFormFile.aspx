<%@ Page Language="VB" MasterPageFile="~/wf/defPopup.master" AutoEventWireup="false" CodeFile="uploadFormFile.aspx.vb" Inherits="wf_support_uploadFormFile" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self" />
    <link id="cssMain" rel="stylesheet" type="text/css" href="../../support/css/style.css" />
    <link href="../../plugin/uploader/css/ui-lightness/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../plugin/uploader/css/fileUploader.css" rel="stylesheet" type="text/css" />
    
    <script src="<%=rootPath%>plugin/uploader/js/jquery-1.6.2.min.js" type="text/javascript" language="javascript"></script>
    <script src="<%=rootPath%>plugin/uploader/js/jquery-ui-1.8.14.custom.min.js" type="text/javascript" language="javascript"></script>
    <script src="<%=rootPath%>plugin/uploader/js/jquery.fileUploader.js" type="text/javascript" language="javascript"></script>
</head>
<body>
    
    <div id="wrapperPopup">
        <div id="titlePopup" class="mt10 ov titlePopup mb10" runat="server">
        </div>
        <div class="clear"></div>
        <div id="divNotif" runat="server"></div>
        <div id="main_container">
            <div id="contentPopup" class="ov">
                <form id="frmUpload" action="<%=action%>" method="post" enctype="multipart/form-data">
                    <div>
                        <div style="border: solid 1px #cccccc; padding:10px; margin-right:5px;"><input type="file" name="filename" class="fileUpload" <%=type%> <%=tagRef%>></div>
                        <button id="px-submit" type="submit" style="height:42px">Upload</button>
	                    <button id="px-clear" type="reset" style="height:42px">Clear</button>
                    </div>
                </form>
                <script type="text/javascript">
		            jQuery(function($){
			            $('.fileUpload').fileUploader();
		            });
	            </script>
	        </div>
        </div>
    </div>
</body>
</html>
