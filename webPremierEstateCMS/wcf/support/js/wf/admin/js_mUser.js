function updateRow(domainRef, ref, rowNo) {
    wcf_mUser.JSON_getUserInfoHTML(domainRef, ref, updateRowOnComplete, JSONOnError, rowNo);
}

function updateRowOnComplete(result, rowNo) {
    result = eval(result);
    if (result.length == 0) return 0;
    
    document.getElementById("divEmail" + rowNo).innerHTML = MyURLDecode(result[0].email);
    document.getElementById("divPassword" + rowNo).innerHTML = MyURLDecode(result[0].password);
    document.getElementById("divName" + rowNo).innerHTML = MyURLDecode(result[0].name);
    document.getElementById("divHP" + rowNo).innerHTML = MyURLDecode(result[0].HP);
    document.getElementById("divPhone" + rowNo).innerHTML = MyURLDecode(result[0].phone);
    
}



function JSONOnError(result) { 
    //alert(result.get_message()); 
} 