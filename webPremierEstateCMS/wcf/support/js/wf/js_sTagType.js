function updateRow(domainRef, ref, rowNo) {
    wcf_sTagType.JSON_getTagTypeInfoHTML(domainRef, ref, updateRowOnComplete, JSONOnError, rowNo);
}

function updateRowOnComplete(result, rowNo) {
    result = eval(result);
    if (result.length == 0) return 0;
    
    document.getElementById("divTagTypeName" + rowNo).innerHTML = MyURLDecode(result[0].tagTypeName);
}

function JSONOnError(result) { 
    //alert(result.get_message()); 
} 