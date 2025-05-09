function updateRow(domainRef, ref, rowNo) {
    debugger;
    wcf_wmContent.JSON_getContentInfoHTML(domainRef, ref, updateRowOnComplete, JSONOnError, rowNo);
}

function updateRowOnComplete(result, rowNo) {
    
    debugger;
    result = eval(result);
    if (result.length == 0) return 0;
    
    document.getElementById("divType" + rowNo).innerHTML = MyURLDecode(result[0].contentTypeName);
    document.getElementById("divTitle" + rowNo).innerHTML = MyURLDecode(result[0].title);
    document.getElementById("divContentDate" + rowNo).innerHTML = MyURLDecode(result[0].contentDate);
    document.getElementById("divPubDate" + rowNo).innerHTML = MyURLDecode(result[0].publishDate);
    document.getElementById("divApprvDate" + rowNo).innerHTML = MyURLDecode(result[0].approvedDate);
    document.getElementById("divExpDate" + rowNo).innerHTML = MyURLDecode(result[0].expiredDate);
    document.getElementById("divTAG" + rowNo).innerHTML = MyURLDecode(result[0].TAG);
    document.getElementById("divHit" + rowNo).innerHTML = MyURLDecode(result[0].hit);
    
    
    
}

function JSONOnError(result) { 
    alert(result.get_message()); 
} 