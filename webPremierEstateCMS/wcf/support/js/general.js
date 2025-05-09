function Left(str, n){
	if (n <= 0) {
	    return "";
	} else if (n > String(str).length) {
	    return str;
	} else {
	    return String(str).substring(0,n);
	 }
}

function Right(str, n){
    if (n <= 0) {
       return "";
    } else if (n > String(str).length) {
       return str;
    } else {
       var iLen = String(str).length;
       return String(str).substring(iLen, iLen - n);
    }
}

function MyURLDecode (clearString) {
    var output = '';
    var huruf = new Array(" ", "\"", "#", "$", "%", "&", "+", ",", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "`", "{", "|", "}", "~", String.fromCharCode(13), String.fromCharCode(10), "'");            
    var hurufEncode = new Array("%20", "%22", "%23", "%24", "%25", "%26", "%2b", "%2c", "%2f", "%3a", "%3b", "%3c", "%3d", "%3e", "%3f", "%40", "%5b", "%5c", "%5d", "%5e", "%60", "%7b", "%7c", "%7d", "%7e", "%0d", "%0a", "%27");
            
    for (var j = 0; j < hurufEncode.length; j++) {
        while(clearString.indexOf(hurufEncode[j]) > -1)
            clearString = clearString.replace(hurufEncode[j], huruf[j]);
    }
    output = clearString;
    return output;
}

function formatNum(nStr) {
	nStr += '';
	x = nStr.split('.');
	x1 = x[0];
	x2 = x.length > 1 ? '.' + x[1] : '';
	var rgx = /(\d+)(\d{3})/;
	while (rgx.test(x1)) {
		x1 = x1.replace(rgx, '$1' + ',' + '$2');
	}
	return x1 + x2;
}

function return2br(dataStr) {
    return dataStr.replace(/(\r\n|[\r\n])/g, "<br />");
}

function toNumeric(o) {
    if(/[^0-9]/.test(o.value)){
        o.value=o.value.replace(/([^0-9])/g,"");
    }
}

function stripHTML(str){
    var re = /(<([^>]+)>)/gi;
    str = str.replace(re, "");
    return str;
}

function converStrToParam (str) {
    var temp = stripHTML(str);
    temp = temp.toLowerCase().replace(/([.])/g,'');
    temp = temp.replace(/([ ])/g,'.');
    var result = '';
    var isDot = false;
    
    for (var i=0;i<temp.length;i++) {
        if ((temp.charAt(i) >= 'a' && temp.charAt(i) <= 'z') || (temp.charAt(i) >= '0' && temp.charAt(i) <= '9') || (temp.charAt(i) == '.')) {
            if (temp.charAt(i) == '.') {
                if (isDot == false) {
                    result = result + temp.charAt(i);
                }   
            } else {
                result = result + temp.charAt(i);
            }
            
            if (temp.charAt(i) == '.') {
                isDot = true;
            } else {
                isDot = false;
            }
        }
    }
    if (result.charAt(result.length - 1) == '.') {
        result = Left(result, result.length - 1);
    }
    return result;
}


function isNumeric(sText) {
    var ValidChars = "0123456789.";
    var IsNumber=true;
    var Char;

    for (i = 0; i < sText.length && IsNumber == true; i++) { 
        Char = sText.charAt(i); 
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}

function emailValidation(addr) {
    var emailReg = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;
    var regex = new RegExp(emailReg);
    return regex.test(addr);
}

function toNumericMin(o) {
    if(/[^0-9,-]/.test(o.value)){
        o.value=o.value.replace(/([^0-9,-])/g,"");
    }
}

function check0(nStr) {
    if (nStr == '') return '0';
    return nStr;
}

String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, ""); 
};

function countLengthTextArea(o,d,maxL){
    var n = o.value.length;
    var result = "";
    
    if (n <= maxL){
        try {
            d.innerHTML = (maxL - n) + " char left";
        } catch(e) {
            d.innerText = (maxL - n) + " char left";
        }
    } else {
        o.value = o.value.substring(0,maxL);
    }
}