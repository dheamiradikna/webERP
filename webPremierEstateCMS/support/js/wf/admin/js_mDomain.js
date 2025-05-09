function updateRow(ref, rowNo) {
    wcf_mDomain.JSON_getDomainInfoHTML(ref, updateRowOnComplete, JSONOnError, rowNo);
}

function updateRowOnComplete(result, rowNo) {
    result = eval(result);
    if (result.length == 0) return 0;
    
    document.getElementById("divDomainLevelName" + rowNo).innerHTML = MyURLDecode(result[0].domainLevelName);
    document.getElementById("divDomainName" + rowNo).innerHTML = MyURLDecode(result[0].domainName);
    document.getElementById("divIP" + rowNo).innerHTML = MyURLDecode(result[0].IP);
    document.getElementById("divCPName" + rowNo).innerHTML = MyURLDecode(result[0].CPName);
    document.getElementById("divCPEmail" + rowNo).innerHTML = MyURLDecode(result[0].CPEmail);
    document.getElementById("divCPHP" + rowNo).innerHTML = MyURLDecode(result[0].CPHP);
    document.getElementById("divActive" + rowNo).innerHTML = MyURLDecode(result[0].isActive);
    
}



function JSONOnError(result) { 
    //alert(result.get_message()); 
} 