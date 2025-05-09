function updateRow(domainRef, ref, rowNo) {
    wcf_sTag.JSON_getTagInfoHTML(domainRef, ref, updateRowOnComplete, JSONOnError, rowNo);
}

function updateRowOnComplete(result, rowNo) {
    result = eval(result);
    if (result.length == 0) return 0;
    
    document.getElementById("divTagTypeName" + rowNo).innerHTML = MyURLDecode(result[0].tagTypeName);
    document.getElementById("divTagName" + rowNo).innerHTML = MyURLDecode(result[0].tagName);
    document.getElementById("divActive" + rowNo).innerHTML = MyURLDecode(result[0].isActive);
    document.getElementById("divLevel" + rowNo).innerHTML = MyURLDecode(result[0].level);
}

function JSONOnError(result) { 
    //alert(result.get_message()); 
} 