<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uploadFormStandard.aspx.vb" Inherits="wf_support_uploadFormStandard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self" />
   <%-- <link id="cssMain" rel="stylesheet" type="text/css" href="../../support/css/style.css" />--%>


    <link rel="stylesheet" type="text/css" href="../../support/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../support/css/magnific-popup.css" />
    <link rel="stylesheet" type="text/css" href="../../support/css/font-icons.css" />
    <link rel="stylesheet" type="text/css" href="../../support/css/style.css" />
    <link rel="stylesheet" type="text/css" href="../../support/css/sliders.css" />
    <link rel="stylesheet" type="text/css" href="../../support/css/animate.min.css" />
    <link href="../../plugin/uploader/css/ui-lightness/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../plugin/uploader/css/fileUploader.css" rel="stylesheet" type="text/css" />
    <%--tambahan--%>
    <link rel="stylesheet" type="text/css" href="../../support/plugin/fileinput/css/fileinput.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../../support/plugin/fileinput/css/fileinput-rtl.min.css" media="all" />
    
   <%-- <script src="<%=rootPath%>plugin/uploader/js/jquery-1.6.2.min.js" type="text/javascript" language="javascript"></script>
    <script src="<%=rootPath%>plugin/uploader/js/jquery-ui-1.8.14.custom.min.js" type="text/javascript" language="javascript"></script>
    <script src="<%=rootPath%>plugin/uploader/js/jquery.fileUploader.js" type="text/javascript" language="javascript"></script>--%>

    <script src="<%=rootPath%>support/js/jquery.min.js" type="text/javascript"></script>
</head>
<body style="background-color: #efeeea;">
 <section class="section-wrap checkout pt-0 pb-50">
    <div class="container">
    <div id="wrapperPopup">
        <div id="titlePopup" class="alert alert-info fade in alert-dismissible" runat="server">
        </div>
        <div class="clear"></div>
        <div id="divNotif" runat="server"></div>
        <div id="main_container">
            <div id="contentPopup" class="ov">
                <form id="frmUpload" action="<%=action%>" method="post" enctype="multipart/form-data">
                    <div>
                        <input type="file" id="filename" name="filename" class="file" data-show-upload="false" data-preview-file-type="text" <%=type%> <%=tagRef%> />
                        <button id="px-submit" type="submit" class="btn btn-md btn-dark" style="height:42px">Upload</button>
	                    <button id="px-clear" type="reset" class="btn btn-md btn-dark" style="height:42px">Clear</button>
                    </div>
                </form>
               <%-- <script type="text/javascript">
		            jQuery(function($){
			            $('.fileUpload').fileUploader();
		            });
	            </script>--%>
	        </div>
        </div>
    </div>
  </div>
 </section>


    <script src="<%=rootPath%>support/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="<%=rootPath%>support/js/plugins.js" type="text/javascript"></script>
    <script src="<%=rootPath%>support/js/scripts.js" type="text/javascript"></script>
    
    <script src="<%=rootPath%>support/plugin/datepicker/js/bootstrap-datepicker-1.8.0.min.js" type="text/javascript"></script>

    <script src="<%=rootPath%>support/plugin/fileinput/js/piexif.min.js" type="text/javascript"></script>
    <script src="<%=rootPath%>support/plugin/fileinput/js/sortable.min.js" type="text/javascript"></script>
    <script src="<%=rootPath%>support/plugin/fileinput/js/purify.min.js" type="text/javascript"></script>
    <script src="<%=rootPath%>support/plugin/fileinput/js/popper.min.js" type="text/javascript"></script>
    <script src="<%=rootPath%>support/plugin/fileinput/js/fileinput.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function uploadFiles(url, docFile, success, error) {
            $.ajax({
                type: "POST",
                url: url,
                data: docFile,
                //async: false,
                cache: false,
                contentType: false,
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                enctype: 'multipart/form-data',
                processData: false,
                success: function (data) {
                    console.log('success upload');
                    console.log(data);
                    success.call(this, data);
                },
                error: function (data) {
                    console.log('error upload');
                    console.log(data);
                    error.call(this, data);
                }
            });
        }
    </script>

</body>
</html>
