<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uploadForm.aspx.vb" Inherits="wf_support_uploadForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self" />
    <script src="<%=rootPath %>support/js/jquery.js" type="text/javascript" language="javascript"></script>

    <%--<script src="<%=rootPath%>plugin/uploader/js/jquery-1.6.2.min.js" type="text/javascript" language="javascript"></script>
    <script src="<%=rootPath%>plugin/uploader/js/jquery-ui-1.8.14.custom.min.js" type="text/javascript" language="javascript"></script>--%>
    <%--<script src="<%=rootPath%>plugin/uploader/js/jquery.fileUploader.js" type="text/javascript" language="javascript"></script>--%>

    <link rel="stylesheet" type="text/css" href="../../support/css/bootstrap.min.css" />
    <link id="cssMain" rel="stylesheet" type="text/css" href="../../support/css/style.css" />
    <%--<link href="../../plugin/uploader/css/ui-lightness/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../../plugin/uploader/css/fileUploader.css" rel="stylesheet" type="text/css" />--%>
    <link href="../support/cropper/dist/cropper.min.css" rel="stylesheet" type="text/css" />
    <link href="../support/cropper/dist/custom.min.css" rel="stylesheet" type="text/css" />

    
    <script src="<%=rootPath %>support/js/main-cropper.js" type="text/javascript" language="javascript"></script>
    <script src="<%=rootPath %>support/js/cropper.js" type="text/javascript" language="javascript"></script>

    <script src="<%=rootPath %>support/js/bootstrap.js" type="text/javascript" language="javascript"></script>
</head>
<body style="background-color: #efeeea;">
   <div id="wrapperPopup">
        <div id="titlePopup" class="mt10 ov titlePopup mb10" runat="server">
        </div>
        <div class="clear"></div>
        <div id="divNotif" runat="server"></div>
        <div id="main_container">
            <div id="contentPopup" class="ov">
                <%--<form id="frmUpload" action="<%=action%>" method="post" enctype="multipart/form-data">
                    <div>
                        <div style="border: solid 1px #cccccc; padding:10px; margin-right:5px;"><input type="file" name="filename" class="fileUpload" <%=type%> <%=tagRef%> /></div>
                        <button id="px-submit" type="submit" style="height:42px">Upload</button>
	                    <button id="px-clear" type="reset" style="height:42px">Clear</button>
                    </div>
                </form>--%>
                <form id="cropper" name="cropper" runat="server">

                    <div class="col-md-3" style="padding-bottom: 15px;">
	                    <div class="form-group">
		                    <label style="font-weight:normal;">Pilih gambar</label>
                            <input type="file" id="inputImage" name="file" accept="image/*" class="form-control" onchange="javascript:document.getElementById('isUpload').value=1;" />

                            <input type="hidden" class="form-control" id="save_image" name="save_image" value="" />
                            <input type="hidden" class="form-control" id="image_value" name="image_value" value="" />
                            <input type="hidden" class="form-control" id="isExt" name="isExt" value="" />
                            <input type="hidden" class="form-control" id="isUpload" name="isUpload" value="1" />
	                    </div>
                    </div>
                    <div class="row">
	                    <div class="col-md-12">
		                    <div class="container cropper">
			                    <div class="row">
				                    <div class="col-md-9">
					                    <div class="img-container">
						                    <img id="image" src="http://1.bp.blogspot.com/-sGL3240zgpg/UQ0XTX386HI/AAAAAAAAAU4/gqjeoO9JHCo/s1600/blank!.png" alt="Picture">
					                    </div>
				                    </div>
				                    <div class="col-md-3">
					                    <div class="docs-preview clearfix">
						                    <div class="img-preview preview-lg">
						                    </div>
						                    <div class="img-preview preview-md" style="display:none;">
						                    </div>
						                    <div class="img-preview preview-sm" style="display:none;">
						                    </div>
						                    <div class="img-preview preview-xs" style="display:none;">
						                    </div>
					                    </div>
					                    <div class="docs-buttons">
						                    <div class="btn-group" style="display:none;">
							                    <button type="button" class="btn btn-primary" data-method="setDragMode" data-option="move" title="Move"><span class="docs-tooltip" data-toggle="tooltip" title="Move"><span class="fa fa-arrows"></span></span></button><button type="button" class="btn btn-primary" data-method="setDragMode" data-option="crop" title="Crop"><span class="docs-tooltip" data-toggle="tooltip" title="Crop"><span class="fa fa-crop"></span></span></button>
						                    </div>
						                    <div class="btn-group" style="display:none;">
							                    <button type="button" class="btn btn-primary" data-method="zoom" data-option="0.1" title="Zoom In"><span class="docs-tooltip" data-toggle="tooltip" title="Zoom In"><span class="fa fa-search-plus"></span></span></button><button type="button" class="btn btn-primary" data-method="zoom" data-option="-0.1" title="Zoom Out"><span class="docs-tooltip" data-toggle="tooltip" title="Zoom Out"><span class="fa fa-search-minus"></span></span></button>
						                    </div>
						                    <div class="btn-group" style="display:none;">
							                    <button type="button" class="btn btn-primary" data-method="move" data-option="-10" data-second-option="0" title="Move Left"><span class="docs-tooltip" data-toggle="tooltip" title="Move Left"><span class="fa fa-arrow-left"></span></span></button><button type="button" class="btn btn-primary" data-method="move" data-option="10" data-second-option="0" title="Move Right"><span class="docs-tooltip" data-toggle="tooltip" title="Move Right"><span class="fa fa-arrow-right"></span></span></button><button type="button" class="btn btn-primary" data-method="move" data-option="0" data-second-option="-10" title="Move Up"><span class="docs-tooltip" data-toggle="tooltip" title="Move Up"><span class="fa fa-arrow-up"></span></span></button><button type="button" class="btn btn-primary" data-method="move" data-option="0" data-second-option="10" title="Move Down"><span class="docs-tooltip" data-toggle="tooltip" title="Move Down"><span class="fa fa-arrow-down"></span></span></button>
						                    </div>
						                    <div class="btn-group" style="display:none;">
							                    <button type="button" class="btn btn-primary" data-method="rotate" data-option="-45" title="Rotate Left"><span class="docs-tooltip" data-toggle="tooltip" title="Rotate Left"><span class="fa fa-rotate-left"></span></span></button><button type="button" class="btn btn-primary" data-method="rotate" data-option="45" title="Rotate Right"><span class="docs-tooltip" data-toggle="tooltip" title="Rotate Right"><span class="fa fa-rotate-right"></span></span></button>
						                    </div>
						                    <div class="btn-group" style="display:none;">
							                    <button type="button" class="btn btn-primary" data-method="scaleX" data-option="-1" title="Flip Horizontal"><span class="docs-tooltip" data-toggle="tooltip" title="Flip Horizontal"><span class="fa fa-arrows-h"></span></span></button><button type="button" class="btn btn-primary" data-method="scaleY" data-option="-1" title="Flip Vertical"><span class="docs-tooltip" data-toggle="tooltip" title="Flip Vertical"><span class="fa fa-arrows-v"></span></span></button>
						                    </div>
						                    <div class="btn-group">
							                    <button type="button" class="btn btn-primary" data-method="getCroppedCanvas" title="Save Image"><span>Save Image</span></button>
						                    </div>
                                            <div class="btn-group">
							                    <button type="button" class="btn btn-primary" data-method="close" title="Close">
                                                    <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();">Close</a>
							                    </button>
						                    </div>
						                    <div class="btn-group" style="display:none;">
							                    <button type="button" class="btn btn-primary" data-method="reset" title="Reset"><span class="docs-tooltip" data-toggle="tooltip" title="Reset"><span class="fa fa-refresh"></span></span></button><label class="btn btn-primary btn-upload" for="inputImage" title="Upload image file"><span class="docs-tooltip" data-toggle="tooltip" title="Import image with Blob URLs"><span class="fa fa-upload"></span></span></label><button type="button" class="btn btn-primary" data-method="destroy" title="Destroy"><span class="docs-tooltip" data-toggle="tooltip" title="$().cropper(&quot;destroy&quot;)"><span class="fa fa-power-off"></span></span></button>
						                    </div>
					                    </div>
					                    <div class="docs-data">
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataX">X</label><input type="text" class="form-control" id="dataX" placeholder="x"><span class="input-group-addon">px</span>
						                    </div>
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataY">Y</label><input type="text" class="form-control" id="dataY" placeholder="y"><span class="input-group-addon">px</span>
						                    </div>
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataWidth">Width</label><input type="text" class="form-control" id="dataWidth" name="dataWidth" placeholder="width" /><span class="input-group-addon">px</span>
						                    </div>
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataHeight">Height</label><input type="text" class="form-control" id="dataHeight" name="dataHeight" placeholder="height" /><span class="input-group-addon">px</span>
						                    </div>
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataRotate">Rotate</label><input type="text" class="form-control" id="dataRotate" placeholder="rotate"><span class="input-group-addon">deg</span>
						                    </div>
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataScaleX">ScaleX</label><input type="text" class="form-control" id="dataScaleX" placeholder="scaleX">
						                    </div>
						                    <div class="input-group input-group-sm">
							                    <label class="input-group-addon" for="dataScaleY">ScaleY</label><input type="text" class="form-control" id="dataScaleY" placeholder="scaleY">
						                    </div>
					                    </div>
				                    </div>
			                    </div>
		                    </div>
	                    </div>
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
</body>
</html>
