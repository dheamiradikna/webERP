function stripHTML(str) {
    var re = /(<([^>]+)>)/gi;
    str = str.replace(re, "");
    return str;
}

function converStrToParam(str) {
    var temp = stripHTML(str);
    temp = temp.toLowerCase().replace(/([.])/g, '');
    temp = temp.replace(/([ ])/g, '.');
    var result = '';
    var isDot = false;

    for (var i = 0; i < temp.length; i++) {
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

function MyURLDecode(clearString) {
    var output = '';
    var huruf = new Array(" ", "\"", "#", "$", "%", "&", "+", ",", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "`", "{", "|", "}", "~", String.fromCharCode(13), String.fromCharCode(10), "'");
    var hurufEncode = new Array("%20", "%22", "%23", "%24", "%25", "%26", "%2b", "%2c", "%2f", "%3a", "%3b", "%3c", "%3d", "%3e", "%3f", "%40", "%5b", "%5c", "%5d", "%5e", "%60", "%7b", "%7c", "%7d", "%7e", "%0d", "%0a", "%27");

    for (var j = 0; j < hurufEncode.length; j++) {
        while (clearString.indexOf(hurufEncode[j]) > -1)
            clearString = clearString.replace(hurufEncode[j], huruf[j]);
    }
    output = clearString;
    return output;
}

function toNumeric(o) {
    if (/[^0-9]/.test(o.value)) {
        o.value = o.value.replace(/([^0-9])/g, "");
    }
}

function toNumericStrDotMin(o) {
    if (/[^0-9.-]/.test(o)) {
        o = o.replace(/([^0-9.-])/g, "");
    }
    return o;
}


function toNumericDot(o) {
    if (/[^0-9.]/.test(o.value)) {
        o.value = o.value.replace(/([^0-9.])/g, "");
    }
}

function toNumericDotMin(o) {
    if (/[^0-9.-]/.test(o.value)) {
        o.value = o.value.replace(/([^0-9.-])/g, "");
    }
}


function Left(str, n) {
    if (n <= 0) {
        return "";
    } else if (n > String(str).length) {
        return str;
    } else {
        return String(str).substring(0, n);
    }
}

function Right(str, n) {
    if (n <= 0) {
        return "";
    } else if (n > String(str).length) {
        return str;
    } else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}

function dateValidation(date) {
    var dateReg = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
    var regex = new RegExp(dateReg);
    return regex.test(date);
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function countLengthTextArea(o, d, maxL) {
    var n = o.value.length;
    var result = "";

    if (n <= maxL) {
        try {
            d.innerHTML = (maxL - n) + " char left";
        } catch (e) {
            d.innerText = (maxL - n) + " char left";
        }
    } else {
        o.value = o.value.substring(0, maxL);
    }
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function showNotifInformation(NotifInfo, statusDB) {
    var noteNotifEl = document.getElementById("noteNotif");
    var notifInfoEl = document.getElementById("notifInformation");
    if (statusDB === '') {
        noteNotifEl.innerHTML = "Please see information below.";
        noteNotifEl.style.color = '#297ca3';
        notifInfoEl.className = "info";
        notifInfoEl.innerHTML = NotifInfo;
    }
    else {
        if (statusDB === 'Success') {
            noteNotifEl.innerHTML = "Please see for success message in below.";
            noteNotifEl.style.color = '#3d6f2f';
            notifInfoEl.className = "success";
            notifInfoEl.innerHTML = NotifInfo;
        }
        else if (statusDB === 'Failed') {
            noteNotifEl.innerHTML = "Please see information below.";
            noteNotifEl.style.color = '#3d6f2f';
            notifInfoEl.className = "error";
            notifInfoEl.innerHTML = NotifInfo;
        }
    }
}

function showErrorInformation(errorList) {
    var noteNotifEl = document.getElementById("noteNotif");
    var notifInfoEl = document.getElementById("notifInformation");
    var appendEl = "";

    noteNotifEl.innerHTML = "Please check again for error message in below.";
    noteNotifEl.style.color = '#D8000C';
    notifInfoEl.className = "validation";
    noteNotifEl.style.fontWeight = "Bolder";

    appendEl += "<ul>";
    for (i = 0 ; i < errorList.length; i++) {
        appendEl += "<li>" + errorList[i] + "</li>";
    }
    appendEl += "</ul>";
    notifInfoEl.innerHTML = appendEl;
}

function getMimeType(extension) {
    var mimeType = '';
    switch (extension) {
        case 'jpg':
            mimeType = 'image';
            break;
        case 'jpeg':
            mimeType = 'image';
            break;
        case 'png':
            mimeType = 'image';
            break;
        case 'pdf':
            mimeType = 'pdf';
            break
        case 'xlsx':
            mimeType = 'office';
            break;
        case 'xls':
            mimeType = 'office';
            break;
        case 'docx':
            mimeType = 'office';
            break;
        case 'doc':
            mimeType = 'office';
            break;
        case 'pptx':
            mimeType = 'office';
            break;
        case 'ppt':
            mimeType = 'office';
            break;
        case 'mp4':
            mimeType = 'video';
            break;
        case '3gp':
            mimeType = 'video';
            break;
        case 'avi':
            mimeType = 'video';
            break;
        case 'mkv':
            mimeType = 'video';
            break;
        case 'flv':
            mimeType = 'video';
            break;
        case 'tif':
            mimeType = 'gdocs';
            break;
        case 'ai':
            mimeType = 'gdocs';
            break;
        case 'eps':
            mimeType = 'gdocs';
            break;
        case 'txt':
            mimeType = 'text';
            break;
        case 'htm':
            mimeType = 'html';
            break;
        case 'html':
            mimeType = 'html';
            break;
    }

    return mimeType;
}

function geKrajeetConfiguration(mimeType, extension) {
    if (mimeType == 'video') {
        return [{ type: mimeType, filetype: mimeType + '/' + extension }];
    } else if (mimeType == 'text') {
        return [{ type: mimeType, downloadUrl: false }];
    } else if (mimeType == 'html') {
        return [{ type: mimeType, downloadUrl: false }];
    } else {
        return [{ type: mimeType }];
    }
}

function countLengthKeyword(o, d, maxL) {
    var n = o.value.split(",").length;
    if (o.value == "") {
        n = 0;
    }
    var result = "";

    if (n <= maxL) {
        try {
            d.innerHTML = (maxL - n) + ' keyword left, Separate by "," (coma)';
        } catch (e) {
            d.innerText = (maxL - n) + ' keyword left, Separate by "," (coma)';
        }
    } else {
        o.value = o.value.substring(0, o.value.length - 1);
    }
}