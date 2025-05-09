function bindChooseProject(keyword, pageNo) {
    debugger;
    var iProjectName = document.getElementById("iProjectName");
    divProjectName.display = 'none';
    iProjectName.style.display = '';
    console.log(iProjectName);
    $.ajax({
        type: "POST",
        url: rootPath + "wcf/WebService.asmx/GetProjectList",
        data: { 'keyword': keyword, 'pageNo': pageNo },
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        dataType: "text", // the data type we want back, so text.  The data will come wrapped in xml
        success: function (data) {
            console.log(data);
            // As it is already a JSON object we can just start using it
            iProjectName.style.display = 'none';
            divProjectName.style.display = '';
            divProjectName.innerHTML = data;


        }
    });
}