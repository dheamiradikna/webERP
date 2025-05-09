<%@ Page Language="VB" MasterPageFile="~/wf/defPopup.master" AutoEventWireup="false" CodeFile="contentTagPopup.aspx.vb" Inherits="wf_contentTagPopup" title="" ValidateRequest="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<style type="text/css">
        .tb
        {
        	width: 400px;
        }
        .ta
        {
        	width: 400px;
        	height: 100px;
        }
        .nicEdit-panel {background-color: #fff !important;}
        .nicEdit-button {background-color: #fff !important;}
        
        
    </style>--%>
    <script language="javascript" type="text/javascript">
        var rootPath = '<%=rootPath%>';
    </script>
    <%--<script language="javascript" type="text/javascript" src="<%=rootPath%>plugin/editor/nicEdit.js"></script>--%>
    <script language="javascript" type="text/javascript" src="<%=rootPath%>plugin/editor/tinymce.min.js"></script>    
    <script type="text/javascript" language="javascript">
        var isContentDate = true;
        var isPublishDate = true;
        var isExpiredDate = true;
        
        var isFilePicture = true;

        var _rootPath = '<%=rootPath%>';
        var _isUpdate = '<%=isUpdate%>';
        var _isTitle = '<%=isTitle%>';
        var _isTitleDetail = '<%=isTitleDetail%>';
        var _isSynopsis = '<%=isSynopsis%>';
        var _isSynopsisNNP = '<%=isSynopsisNNP%>';
        var _isContent = '<%=isContent%>';

         var _isMap = '<%=isMap%>';

        var _isThumbnail = '<%=isThumbnail%>';
        var _isPicture = '<%=isPicture%>';
        var _isVideo = '<%=isVideo%>';
        var _isAttachment = '<%=isAttachment%>';
        var _isDate = '<%=isDate%>';
        var _isPublishDate = '<%=isPublishDate%>';
        var _isExpiredDate = '<%=isExpiredDate%>';
        var _tagRef = '<%=tagRef%>';
        
        function dateValidation(date) {
            var dateReg = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var regex = new RegExp(dateReg);
            return regex.test(date);
        }
        
        //function cekDate(ctrl, e) {
        //    var date = '';
        //    var selDay = document.getElementById("selDay" + ctrl);
        //    var selMonth = document.getElementById("selMonth" + ctrl);
        //    var txtYear = document.getElementById("txtYear" + ctrl);
            
        //    if (selDay.value == '-' && selMonth.value == '-' && txtYear.value == '') {
            
        //        if (ctrl == "Content") {
        //            if (_isDate=="1") isContentDate = false;
        //            e.style.display = '';
        //        } else if (ctrl == "Publish") {
        //            if (_isPublishDate=="1") isPublishDate = true;
        //            e.style.display = 'none';
        //        } else if (ctrl == "Expired") {
        //            if (_isExpiredDate=="1") isExpiredDate = true;
        //            e.style.display = 'none';
        //        }
                
        //    } else {
        //        date = Right('00' + selDay.value,2) + '/' + Right('00' + selMonth.value,2) + '/' + Right('0000' + txtYear.value, 4);
        //        if (dateValidation(date) == true) {
                    
        //            if (ctrl == "Content") {
        //                if (_isDate=="1") isContentDate = true; 
        //            } else if (ctrl == "Publish") {
        //                if (_isPublishDate=="1") isPublishDate = true;
        //            } else if (ctrl == "Expired") {
        //                if (_isExpiredDate=="1") isExpiredDate = true;
        //            }
        //            e.style.display = 'none';
        //        } else {
        //            if (ctrl == "Content") {
        //                if (_isDate=="1") isContentDate = false;
        //            } else if (ctrl == "Publish") {
        //                if (_isPublishDate=="1") isPublishDate = false;
        //            } else if (ctrl == "Expired") {
        //                if (_isExpiredDate=="1") isExpiredDate = false;
        //            }
        //            e.style.display = '';
        //        }
        //    }
            
        //}
        
        
        function doSave(tipe) {
            debugger;
            //var errMsg = 'Please see again the error notification.';
            if (isContentDate == false) {
                alert("\"Content Date\" still wrong ");
                document.getElementById("txtContentDate").focus();
            } if (isPublishDate == false) {
                alert("\"Publish Date\" still wrong ");
                document.getElementById("txtYearPublish").focus();
            } if (isExpiredDate == false) {
                alert("\"Expired Date\" still wrong ");
                document.getElementById("txtYearExpired").focus();
            } else {
                if (tipe == 1) {
                    document.getElementById("_save").value = "1";
                } else {
                    document.getElementById("_saveClose").value = "1";
                }
                document.getElementById("_listContentRelated").value = JSON.stringify(arrayList);

                //document.getElementById('txtContent').value = nicEditors.findEditor('txtContent').getContent();
                document.forms[0].submit();
            }
        }
        
        function doDelete(keyword) {
            if (confirm("Are you sure to delete this data \"" + keyword + "\" ?")) {
                document.getElementById("_delete").value = "1";
                document.forms[0].submit();
            }
        }
        
        function doDeleteImage(imgRef) {
            document.getElementById("_deleteImage").value = imgRef;
            document.forms[0].submit();
        }

        function doDeleteAttachment(attachRef) {
            document.getElementById("_deleteAttachment").value = attachRef;
            document.forms[0].submit();
        }
        
        function doRemoveImage(imgRef) {
            document.getElementById("_removeImage").value = imgRef;
            document.forms[0].submit();
        }
        
                
        window.onload = function() {
            

            var _isPB = '<%=isPB%>';

            //if (_isPB == "0") {
            //    cekDate("Content", document.getElementById("eDateContent"));
            //    cekDate("Publish", document.getElementById("eDatePublish"));
            //    cekDate("Expired", document.getElementById("eDateExpired"));
            //}
            
            countLengthTextArea(document.getElementById('txtSynopsis'), document.getElementById('divSynopsisLimit'), 150);
            countLengthTextArea(document.getElementById('txtSynopsisNNP'), document.getElementById('divSynopsisLimitNNP'), 450);

            //var iconPath = _rootPath + "plugin/editor/nicEditorIcons.gif"
            //var txtContent = new nicEditor({iconsPath: iconPath, fullPanel : true }).panelInstance('txtContent');
            
            var __slice = [].slice; (function (e, t) { var n; n = function () { function t(t, n) { var r, i, s, o = this; this.options = e.extend({}, this.defaults, n); this.$el = t; s = this.defaults; for (r in s) { i = s[r]; if (this.$el.data(r) != null) { this.options[r] = this.$el.data(r) } } this.createStars(); this.syncRating(); this.$el.on("mouseover.starrr", "span", function (e) { return o.syncRating(o.$el.find("span").index(e.currentTarget) + 1) }); this.$el.on("mouseout.starrr", function () { return o.syncRating() }); this.$el.on("click.starrr", "span", function (e) { return o.setRating(o.$el.find("span").index(e.currentTarget) + 1) }); this.$el.on("starrr:change", this.options.change) } t.prototype.defaults = { rating: void 0, numStars: 5, change: function (e, t) { } }; t.prototype.createStars = function () { var e, t, n; n = []; for (e = 1, t = this.options.numStars; 1 <= t ? e <= t : e >= t; 1 <= t ? e++ : e--) { n.push(this.$el.append("<span class='glyphicon .glyphicon-star-empty'></span>")) } return n }; t.prototype.setRating = function (e) { if (this.options.rating === e) { e = void 0 } this.options.rating = e; this.syncRating(); return this.$el.trigger("starrr:change", e) }; t.prototype.syncRating = function (e) { var t, n, r, i; e || (e = this.options.rating); if (e) { for (t = n = 0, i = e - 1; 0 <= i ? n <= i : n >= i; t = 0 <= i ? ++n : --n) { this.$el.find("span").eq(t).removeClass("glyphicon-star-empty").addClass("glyphicon-star") } } if (e && e < 5) { for (t = r = e; e <= 4 ? r <= 4 : r >= 4; t = e <= 4 ? ++r : --r) { this.$el.find("span").eq(t).removeClass("glyphicon-star").addClass("glyphicon-star-empty") } } if (!e) { return this.$el.find("span").removeClass("glyphicon-star").addClass("glyphicon-star-empty") } }; return t }(); return e.fn.extend({ starrr: function () { var t, r; r = arguments[0], t = 2 <= arguments.length ? __slice.call(arguments, 1) : []; return this.each(function () { var i; i = e(this).data("star-rating"); if (!i) { e(this).data("star-rating", i = new n(e(this), r)) } if (typeof r === "string") { return i[r].apply(i, t) } }) } }) })(window.jQuery, window); $(function () { return $(".starrr").starrr() })
            var ratingsField = $('#ratings-hidden');

            $('.starrr').on('starrr:change', function (e, value) {
                console.log(e.currentTarget.id);
                console.log(value);
                if (e.currentTarget.id == 'strLocation') {
                    $('#_strLocation').val(value);
                } else if (e.currentTarget.id == 'strDevelopment') {
                    $('#_strDevelopment').val(value);
                } else if (e.currentTarget.id == 'strMarketing') {
                    $('#_strMarketing').val(value);
                } else if (e.currentTarget.id == 'strManagement') {
                    $('#_strManagement').val(value);
                }
            });
            generateTable();
        }





        function checkFileV2(byte, o, e) {
            debugger;
            if (byte.value.length == 0) {
                e.style.display = '';
                switch (o.id) {
                    case 'filePicture': isFilePicture = false; break;
                   
                }
            } else {
                e.style.display = 'none';
                switch (o.id) {
                    case 'filePicture':
                        isAccountFile = true;
                        $("#divUploadPicture").removeClass('form-group has-error has-feedback');
                        break;
                }
            }
        }

        function readFile(type, input) {
            debugger;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {

                    var imageData = e.target.result;
                    var byteString = imageData.split(',')[1];
                    var fileName = input.files[0].name;

                    switch (type) {
                        
                        case "IMAGE":
                            document.getElementById('_fileImgByte').value = byteString;
                            document.getElementById("_fileImgName").value = fileName;
                            checkFileV2(document.getElementById('_filePICTUREByte'), document.getElementById("filePicture"), document.getElementById('eFilePICTURE'));
                            break;
                    }
                }

                //console.log(input.files[0]);
                reader.readAsDataURL(input.files[0]);

            }
        }

        $(document).ready(function () {
            debugger;
            var btnfilename = $("#fileImg")
            debugger;
            var ImgFileType = $("#_fileImgName").val().split('.')[1]
            var mimeType = getMimeType(ImgFileType);
            if (mimeType != '') {
                btnfilename.fileinput({
                    overwriteInitial: true,
                    initialPreview: [
                      '<%=_fileImgUrl%>'
                    ],
                    initialPreviewAsData: true,
                    initialPreviewConfig: [{
                        type: mimeType,
                        url: 'support/displayAttachment.aspx?is=',
                        caption: $("#_fileImgName").val()
                    }],
                    //initialCaption: $("#_fileAktaName").val()
                });
            }else{
                btnfilename.fileinput();
            }})

    </script>
 <script type="text/javascript" language="javascript">

        var useDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;

        tinymce.init({
            selector: 'textarea#full-featured-non-premium',
            plugins: 'print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
            imagetools_cors_hosts: ['picsum.photos'],
            menubar: 'file edit view insert format tools table help',
            toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
            toolbar_sticky: true,
            autosave_ask_before_unload: true,
            autosave_interval: '30s',
            autosave_prefix: '{path}{query}-{id}-',
            autosave_restore_when_empty: false,
            autosave_retention: '2m',
            image_advtab: true,
            link_list: [
              { title: 'My page 1', value: 'https://www.tiny.cloud' },
              { title: 'My page 2', value: 'http://www.moxiecode.com' }
            ],
            image_list: [
              { title: 'My page 1', value: 'https://www.tiny.cloud' },
              { title: 'My page 2', value: 'http://www.moxiecode.com' }
            ],
            image_class_list: [
              { title: 'None', value: '' },
              { title: 'Some class', value: 'class-name' }
            ],
            importcss_append: true,
            file_picker_callback: function (callback, value, meta) {
                /* Provide file and text for the link dialog */
                if (meta.filetype === 'file') {
                    callback('https://www.google.com/logos/google.jpg', { text: 'My text' });
                }

                /* Provide image and alt text for the image dialog */
                if (meta.filetype === 'image') {
                    callback('https://www.google.com/logos/google.jpg', { alt: 'My alt text' });
                }

                /* Provide alternative source and posted for the media dialog */
                if (meta.filetype === 'media') {
                    callback('movie.mp4', { source2: 'alt.ogg', poster: 'https://www.google.com/logos/google.jpg' });
                }
            },
            templates: [
                  { title: 'New Table', description: 'creates a new table', content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
              { title: 'Starting my story', description: 'A cure for writers block', content: 'Once upon a time...' },
              { title: 'New list with dates', description: 'New List with dates', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
            ],
            template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
            template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
            height: 600,
            image_caption: true,
            quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
            noneditable_noneditable_class: 'mceNonEditable',
            toolbar_mode: 'sliding',
            contextmenu: 'link image imagetools table',
            skin: useDarkMode ? 'oxide-dark' : 'oxide',
            content_css: useDarkMode ? 'dark' : 'default',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPMain" Runat="Server">
    <input id="_save" name="_save" type="hidden" value="0"/>
    <input id="_saveClose" name="_saveClose" type="hidden" value="0"/>
    <input id="_delete" name="_delete" type="hidden" value="0"/>
    <input id="_deleteImage" name="_deleteImage" type="hidden" value=""/>
    <input id="_deleteAttachment" name="_deleteAttachment" type="hidden" value=""/>
    <input id="_removeImage" name="_removeImage" type="hidden" value=""/>

    <input id="_fileImgByte" name="_fileImgByte" type="hidden" value="<%=_fileImgByte %>" />
    <input id="_fileImgName" name="_fileImgName" type="hidden" value="<%=_fileImgName %>" />
    <input id="ImgFileRef" name="ImgFileRef" type="hidden" value="<%=ImgFileRef%>"/>
    <input type="hidden" id="_listContentRelated" name="_listContentRelated" value="" />

    

    <div id="divNotif" class="alert alert-info fade in alert-dismissible" runat="server">
        <strong>Notification:</strong> Please fill all data below than click "save"
    </div>
    <div class="container relative">
    <div id="divForm" class="mt10">
        <%--<div class="row">
            <div class="lCol"></div>
            <div class="mCol"></div>
            <div class="rCol">
                <div class="linkBtn left mr5">
                    <a href="javascript:doSave(2);">Save & Close</a>
                </div> 
                <div class="linkBtn left mr5">
                    <a href="javascript:doSave(1);">Save</a>
                </div> 
                <asp:Literal ID="ltrBtnTop" runat="server"></asp:Literal>
                <div class="linkBtn left">
                    <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();">Close</a>
                </div> 
                
            </div>
            <div class="clear"></div>
        </div>--%>
    
        <div class="row">
            <div class="col-md-4">
				<h6>Content Type</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrContentType" runat="server"></asp:Literal>
            </div>
         </div>


         <div class="row">
            <div class="col-md-4">
                <h6>Content Related</h6>
            </div>
            <div class="col-md-8">
                <asp:Literal ID="ltrProject" runat="server"></asp:Literal>
            </div>
         </div>

        <div class="row">
            <div class="col-md-4">
                <h6>List Content Related</h6>
            </div>
            <div class="col-md-8">
                    <div id="tableContentRelated"></div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
				<h6>Image Setting</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImageSetting" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
				<h6>Image Position</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrImagePosition" runat="server"></asp:Literal>
            </div>
        </div>
     <div class="row">
         <div class="col-md-4">
			<h6>Meta Title</h6>
		 </div>
         <div class="col-md-8">
            <input  name="txtMetaTitle" id="txtMetaTitle" type="text" value="<%=txtMetaTitle%>" onkeyup="countLengthTextArea(this,document.getElementById('divTitleLimit'),100);" />
            <div id="divTitleLimit" class="mcol">100 char left</div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>Meta Author</h6>
		 </div>
         <div class="col-md-8">
            <input name="txtMetaAuthor" id="txtMetaAuthor" type="text" value="<%=txtMetaAuthor%>" onkeyup="countLengthTextArea(this,document.getElementById('divAuthorLimit'),100);" />
            <div id="divAuthorLimit" class="mcol">100 char left</div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>Meta Keyword</h6>
		 </div>
        <div class="col-md-8">
            <input name="txtKeyword" id="txtKeyword" type="text" value="<%=txtKeyword%>" onkeyup="countLengthKeyword(this,document.getElementById('divKeywordLimit'),10);" />
            <div id="divKeywordLimit" class="mcol">10 keyword left, Separate by "," (coma)</div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
			<h6>Meta Description</h6>
		 </div>
        <div class="col-md-8">
            <textarea  id="txtMetaDescription" name="txtMetaDescription" cols="" rows="" onkeyup="countLengthTextArea(this,document.getElementById('divMetaDescLimit'),160);"><%=txtMetaDescription%></textarea>
            <div id="divMetaDescLimit" class="mcol">160 char left</div>
        </div>
    </div>
        <div class="row" id="divThumbnail" style="display:none;">
            <div class="col-md-4"><h6>Thumbnail</h6></div>
            <div class="col-md-8">
                <div id="divUploadThumbnail">
					<%--<input name="fileThumbnail" id="fileThumbnail" type="file" class="file" data-show-upload="false"  data-preview-file-type="text"/> 
					<p id="eFileThumbnail"  style="margin-top:20px; display:none; font-size:12px; color:red;" ></p>--%>
				</div>
                <asp:Literal ID="ltrThumbnail" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divPicture" style="display:none;margin-top:20px;">
            <div class="col-md-4"><h6>Picture</h6></div>
            <div class="col-md-8">
                <div id="divUploadPicture">
					<%--<input name="fileImg" id="fileImg" type="file" class="file" data-show-upload="false" onchange="javascript:debugger;readFile('IMAGE',this);" data-preview-file-type="text"/>--%> 
					<%--<p id="eFilePicture"  style="margin-top:20px; display:none; font-size:12px; color:red;" ></p>--%>
				</div>
                    <asp:Literal ID="ltrPicture" runat="server"></asp:Literal>
                
            </div>
            <div class="clear"></div>
        </div>
        

         <div class="row" id="divAttachment" style="display:none;margin-top:20px;">
            <div class="col-md-4"><h6>Attachment</h6></div>
            <div class="col-md-8">
                <div id="divUploadAttachment">
					<%--<input name="fileAttachment" id="fileAttachment" type="file" class="file" data-show-upload="false"  data-preview-file-type="text"/> 
					<p id="eFileAttachment"  style="margin-top:20px; display:none; font-size:12px; color:red;" ></p>--%>
				</div>
                <asp:Literal ID="ltrAttachment" runat="server"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divVideo" style="display:none;margin-top:20px;">
            <div class="col-md-4">
			    <h6>Video</h6>
		    </div>
            <div class="col-md-4">
                 <label >https://www.youtube.com/embed/</label>   
            </div>
            <div class="col-md-4">
                <input name="txtVideo" id="txtVideo" type="text" placeholder="Insert your embed address video here" value="<%=txtVideo%>" />
                <div class="mcol">ex: https://www.youtube.com/embed/GeMqI519bH8 </div>
            </div>
            <div class="clear"></div>
        </div>
        
        <div class="row" id="divTitle" style="display:none;margin-top:20px;">
            <div class="col-md-4">
			    <h6 >Title</h6>
		    </div>
            <div class="col-md-8">
                <input name="txtTitle" id="txtTitle" type="text" value="<%=txtTitle%>" />
            </div>
        </div>

        <div class="row" id="divTitleDetail" style="display:none;">
            <div class="col-md-4">
			    <h6>Title Detail</h6>
		    </div>
            <div class="col-md-8">
                <input  name="txtTitleDetail" id="txtTitleDetail" type="text" value="<%=txtTitleDetail%>" />
            </div>
        </div>

        <div class="row" id="divSynopsis" style="display:none;">
            <div class="col-md-4">
                <h6>Synopsis</h6>  
            </div>
            <div class="col-md-8">
                <textarea id="txtSynopsis" name="txtSynopsis" onkeyup="countLengthTextArea(this,document.getElementById('divSynopsisLimit'),150);"><%=txtSynopsis%></textarea>
                <div id="divSynopsisLimit" class="mcol">150 char left</div>
            </div>
        </div>

        <div class="row" id="divSynopsisNNP" style="display:none;">
            <div class="col-md-4">
                <h6>Synopsis</h6>  
            </div>
            <div class="col-md-8">
                <textarea  id="txtSynopsisNNP" name="txtSynopsisNNP" onkeyup="countLengthTextArea(this,document.getElementById('divSynopsisLimitNNP'),450);"><%=txtSynopsisNNP%></textarea>
                <div id="divSynopsisLimitNNP" class="mcol">111 char left</div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divMapLatitude" style="display:none;">
            <div class="col-md-4"> 
                <h6>Map Latitude</h6>
            </div>
            <div class="col-md-8">
                <input name="txtMapLatitude" id="txtMapLatitude" type="text" value="<%=txtMapLatitude%>" />
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divMapLongitude" style="display:none;">
            <div class="col-md-4"> 
               <h6> Map Longitude</h6> 
            </div>
            <div class="col-md-8">
                <input name="txtMapLongitude" id="txtMapLongitude" type="text" value="<%=txtMapLongitude %>" />
            </div>
        </div>
                
        <div class="row" id="divContent" style="display:none;margin-bottom: 20px;">
            <div class="col-md-4">
                <h6>Content</h6>
            </div>
            <div class="col-md-8">
                <textarea id="full-featured-non-premium" name="txtContent" style="height: 300px;"><%=txtContent%>
                </textarea>
                <%--<textarea id="txtContent" name="txtContent" style="height: 300px;"><%=txtContent%></textarea>--%>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row" id="divContentDate" style="display:none;">
            <div class="col-md-4"><h6>Content Date</h6></div>
            <div class="col-md-8" style="margin-bottom:20px;">
                <div class="input-group date" id="datepicker_SIUP">
                    <div class="input-group-addon">
                        <i class="fa fa-calendar"></i>
                    </div>
                    <input type="text" class="form-control"  id="txtContentDate" name="txtContentDate" value="<%=txtContentDate %>" placeholder="dd/mm/yyyy" /> 
                   <%-- onchange="cekDate('Content', document.getElementById('etxtContentDate'));"--%>
                </div>
             
                
            </div>
            <div class="clear"></div>
        </div>
        <div class="row" id="divSortNo" style="margin-top:20px;">
            <div class="col-md-4">
			    <h6 >Sort No</h6>
		    </div>
            <div class="col-md-8">
                <input name="txtSortNo" id="txtSortNo" type="text" value="<%=txtSortNo%>" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
				<h6>Project</h6>
			</div>
            <div class="col-md-8">
                <asp:Literal ID="ltrProjectArtikel" runat="server"></asp:Literal>
            </div>
         </div>

      <%--  <div class="row" id="divPublishDate" style="display:none;">
            <div class="col-md-4"><h6>Publish Date</h6></div>
            <div class="col-md-8">
                <div>
                    <div class="left mr5"><span id="spanDayPublish" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthPublish" runat="server"></span></div>
                    <div class="left mr5"><input id="txtYearPublish" name="txtYearPublish" value="<%=txtYearPublish%>" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('Publish', document.getElementById('eDatePublish'));" /></div>
                </div>
                <div>
                    <div id="eDatePublish" class="fNotif" style="display: none;">* "Publish Date" still wrong</div>            
                </div>               
            </div>
            <div class="clear"></div>
        </div>--%>
     <%--   <div class="row" id="divExpiredDate" style="display:none;">
            <div class="col-md-4"><h6>Expired Date</h6></div>
            <div class="col-md-8">
                <div>
                    <div class="left mr5"><span id="spanDayExpired" runat="server"></span></div>
                    <div class="left mr5"><span id="spanMonthExpired" runat="server"></span></div>
                    <div class="left mr5"><input id="txtYearExpired" name="txtYearExpired" value="<%=txtYearExpired%>" type="text" size="2" maxlength="4" onkeyup="toNumeric(this); cekDate('Expired', document.getElementById('eDateExpired'));" /></div>
                </div>
                <div>
                    <div id="eDateExpired" class="fNotif" style="display: none;">* "Expired Date" still wrong</div>            
                </div>
                
            </div>
            <div class="clear"></div>
        </div>--%>

        <div class="row" id="divHit" style="display:none;">
            <div class="col-md-4"><h6>Hit / View</h6></div>
            <div class="col-md-8">
                <div id="divHitView" runat="server" class=""></div>
            </div>
            <div class="clear"></div>
        </div>

        <div class="row">
             <div class="col-md-12">
                <a href="javascript:doSave(2);" class="btn btn-md btn-dark"><span>Save & Close</span></a>
                <a href="javascript:doSave(1);" class="btn btn-md btn-dark"><span>Save</span></a>
                <asp:Literal ID="ltrBtn" runat="server"></asp:Literal>
                <a href="javascript:try{opener.doRefresh();}catch(e){} window.close();" class="btn btn-md btn-dark"><span>Close</span></a>
            </div>
        </div>
    </div>
    </div>
    
    <script type="text/javascript"language="javascript">
        var divThumbnail = document.getElementById("divThumbnail")
        var divPicture = document.getElementById("divPicture")
        var divVideo = document.getElementById("divVideo")
        var divAttachment = document.getElementById("divAttachment")
        var divTitle = document.getElementById("divTitle")
        var divTitleDetail = document.getElementById("divTitleDetail")
        var divSynopsis = document.getElementById("divSynopsis")
        var divSynopsisNNP = document.getElementById("divSynopsisNNP")
        var divContent = document.getElementById("divContent")
        var divContentDate = document.getElementById("divContentDate")
        var divPublishDate = document.getElementById("divPublishDate")
        var divExpiredDate = document.getElementById("divExpiredDate")
        var divHit = document.getElementById("divHit")
        var divMapLongitude = document.getElementById("divMapLongitude")
        var divMapLatitude = document.getElementById("divMapLatitude")

        var _isTitle = '<%=isTitle%>';
        var _isTitleDetail = '<%=isTitleDetail%>';
        var _isSynopsis = '<%=isSynopsis%>';
        var _isSynopsisNNP = '<%=isSynopsis%>';
        var _isContent = '<%=isContent%>';


        var _isMap = '<%=isMap%>';

        var _isThumbnail = '<%=isThumbnail%>';
        var _isPicture = '<%=isPicture%>';
        var _isVideo = '<%=isVideo%>';
        var _isAttachment = '<%=isAttachment%>';
     <%--   var _isDate = '<%=isDate%>';
        var _isPublishDate = '<%=isPublishDate%>';
        var _isExpiredDate = '<%=isExpiredDate%>';--%>
    
        if (_isTitle == '1') divTitle.style.display = ''; else divTitle.style.display = 'none';
        if (_isTitleDetail == '1') divTitleDetail.style.display = ''; else divTitleDetail.style.display = 'none';

        if (_isMap == '1') {
            divMapLongitude.style.display = '';
            divMapLatitude.style.display = '';
        } else {
            divMapLongitude.style.display = 'none';
            divMapLatitude.style.display = 'none';
        }
          

        if (_isSynopsis == '1' && _tagRef != '16') divSynopsis.style.display = ''; else divSynopsis.style.display = 'none';
        if (_isSynopsisNNP == '1' && _tagRef == '16') divSynopsis.style.display = ''; else divSynopsisNNP.style.display = 'none';
        if (_isContent == '1') divContent.style.display = ''; else divContent.style.display = 'none';
        if (_isThumbnail == '1') divThumbnail.style.display = ''; else divThumbnail.style.display = 'none';
        if (_isPicture == '1') divPicture.style.display = ''; else divPicture.style.display = 'none';
        if (_isVideo == '1') divVideo.style.display = ''; else divVideo.style.display = 'none';
        if (_isAttachment == '1') divAttachment.style.display = ''; else divAttachment.style.display = 'none';
        //if (_isDate == '1') divContentDate.style.display = ''; else divContentDate.style.display = 'none';
        //if (_isPublishDate == '1') divPublishDate.style.display = ''; else divPublishDate.style.display = 'none';
        //if (_isExpiredDate == '1') divExpiredDate.style.display = ''; else divExpiredDate.style.display = 'none';
        if (_isUpdate == '1') divHit.style.display = ''; else divHit.style.display = 'none';
        
    </script>

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
    <script type="text/javascript">
        var iProjectName = document.getElementById("iProjectName");
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="forJS" Runat="Server">

    <script type="text/javascript">
        var rootPath = '<%=rootPath%>';

        var _jsonContentRelated = '<%=_jsonContentRelated%>';
        var arrayList = JSON.parse(_jsonContentRelated);

        var txtKeyword = document.getElementById("txtKeyword");

        function doSelectProject(contentRef, title) {
            debugger;
            if (checkingContentRelated(contentRef)) {
                arrayList.push({
                    "contentRef": contentRef,
                    "title": title
                })
                generateTable();

            } else {
                alert("Content Sudah Ada")
            }

            divProjectName.innerHTML = "";
            txtKeyword.value = "";
            console.log(JSON.stringify(arrayList));
        }

        function generateTable() {
            if (arrayList.length > 0) {
                var content = document.getElementById("tableContentRelated");
                content.innerHTML = '';
                var div1 = document.createElement('div');
                var table = document.createElement('table');
                var tHead = document.createElement('tHead');
                var trHead = document.createElement('tr');
                var th1 = document.createElement('th');
                var th2 = document.createElement('th');
                var th3 = document.createElement('th');

                div1.setAttribute("class", "table-responsive");
                table.setAttribute("class", "table table-striped table-bordered dataTable");

                th1.innerHTML = 'No';
                th2.innerHTML = 'Title';
                th3.innerHTML = '';

                trHead.appendChild(th1);
                trHead.appendChild(th2);
                trHead.appendChild(th3);
                tHead.appendChild(trHead);
                table.appendChild(tHead);

                var tbody = document.createElement('tbody');
                for (var i = 0; i < arrayList.length; i++) {
                    var tr = document.createElement('tr');
                    var td1 = document.createElement('td');
                    var td2 = document.createElement('td');
                    var td3 = document.createElement('td');
                    var button = document.createElement('button');
                    button.type = 'button';
                    button.setAttribute("class", "btn btn-danger btn-xs");
                    button.setAttribute("onclick", "javascript:deleteContentList('" + arrayList[i].contentRef + "')");
                    button.innerHTML = 'Delete'

                    td1.innerHTML = (i + 1).toString();
                    td1.align = 'center';
                    td2.innerHTML = arrayList[i].title;
                    td2.align = 'left';
                    td3.appendChild(button);
                    td3.align = 'center';

                    tr.appendChild(td1);
                    tr.appendChild(td2);
                    tr.appendChild(td3);

                    tbody.appendChild(tr);
                }

                table.appendChild(tbody);
                div1.appendChild(table);
                content.appendChild(div1);
            } else {
                var content = document.getElementById("tableContentRelated");
                content.innerHTML = '*No Data Available';
            }
        }

        function deleteContentList(ref) {
            console.log('ref' + ref);
            if (arrayList.length > 0) {
                for (var i = 0; i < arrayList.length; i++) {
                    if (arrayList[i].contentRef == ref) {
                        arrayList.splice(i, 1);
                        generateTable();
                        break;
                    }
                }
            }
        }

        function doResetProject() {
            divProjectName.innerHTML = "";
            txtKeyword.value = "";

        }

        function checkingContentRelated(contentRef) {
            if (arrayList.length > 0) {
                for (var i = 0; i < arrayList.length; i++) {
                    if (arrayList[i].contentRef == contentRef) {
                        return false
                    }
                }
            }

            return true
        }

        //function doSave() {
        //        document.getElementById("_mRatingInput").value = "1";
        //        document.forms[0].submit();
        //}

        function doSearchProject() {
            debugger;
            var txtKeywordValue = txtKeyword.value;
            bindChooseProject(txtKeywordValue, 1);
        }

    </script>

</asp:Content>


