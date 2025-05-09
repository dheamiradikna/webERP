Imports System.Data
Imports System.Web
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization
Imports [class].clsGeneralDB
Imports [class].clsWebGeneral
Imports [class].clsGeneral
Imports model

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService
    

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetCountryLookupSvc()
        Dim dt As New DataTable
        Dim json As String = ""

        Try
            dt = getCountryList()
        Catch ex As Exception
            dt = New DataTable
        End Try

        Dim jsonserializersettings = New JsonSerializerSettings()
        jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()
        json = JsonConvert.SerializeObject(dt, Formatting.Indented, jsonserializersettings)
        Context.Response.Clear()
        Context.Response.ContentType = "application/json"
        Context.Response.Write(json)
    End Sub

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetProvinceLookupSvc(ByVal countryCode As String)
        Dim dt As New DataTable
        Dim json As String = ""

        Try
            dt = getListProvince(countryCode)
        Catch ex As Exception
            dt = New DataTable
        End Try

        Dim jsonserializersettings = New JsonSerializerSettings()
        jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()
        json = JsonConvert.SerializeObject(dt, Formatting.Indented, jsonserializersettings)
        Context.Response.Clear()
        Context.Response.ContentType = "application/json"
        Context.Response.Write(json)
    End Sub

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetCityLookupSvc(ByVal countryCode As String, ByVal provinceCode As String)
        Dim dt As New DataTable
        Dim json As String = ""

        Try
            dt = getListCity(countryCode, provinceCode)
        Catch ex As Exception
            dt = New DataTable
        End Try

        Dim jsonserializersettings = New JsonSerializerSettings()
        jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()
        json = JsonConvert.SerializeObject(dt, Formatting.Indented, jsonserializersettings)
        Context.Response.Clear()
        Context.Response.ContentType = "application/json"
        Context.Response.Write(json)
    End Sub

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetProjectList(ByVal keyword As String, ByVal pageNo As Integer)
        Dim dt As New DataTable
        Dim json As String = ""
        Dim result As New StringBuilder
        Dim numOfRowPerPage As Integer = _rowCountLookupDefault
        Dim numOfData As Integer

        Try
            dt = GetAllProjectList(keyword)
        Catch ex As Exception
            dt = New DataTable
        End Try

        'Dim jsonserializersettings = New JsonSerializerSettings()
        'jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()
        'json = JsonConvert.SerializeObject(dt, Formatting.Indented, jsonserializersettings)
        'Context.Response.Clear()
        'Context.Response.ContentType = "application/json"
        'Context.Response.Write(json)

        If dt.Rows.Count > 0 Then
                numOfData = dt.Rows.Count

                '''' setting pagging ''''
                Dim totalPage As Int16 = Math.Ceiling(dt.Rows.Count / numOfRowPerPage)
                If pageNo > totalPage Then
                    pageNo = totalPage
                End If
                Dim firstRow As Int16 = ((pageNo - 1) * numOfRowPerPage) + 1
                Dim countPage As Int16 = 0
                ''''' end setting pagging '''' 

                    '''' panel ''''
                    result.Append("<div class=""panel panel-default panel-table"">") 

                    '''' panel heading ''''
                    result.Append("<div class=""panel-heading"">")
                    result.Append("     <div class=""row"">")
                    result.Append("         <div class=""col-sm-6"">")
                    result.Append("             <div style=""font-size:10pt; color:red;"">" + dt.Rows.Count.ToString + " rows affected</div>")
                    result.Append("         </div>")
                    result.Append("         <div class=""col-sm-6""></div>")
                    result.Append("     </div>")
                    result.Append("</div>")
                    ''''' end panel heading ''''

                    '''' panel body ''''
                    result.Append("<div class=""panel-body"">")
                    result.Append("<div class=""table table-responsive"">")
                    result.Append("<table class=""table table-bordered table-hover"" style=""white-space: nowrap;"" cellpadding=""4"" cellspacing=""1"">")
                    result.Append("  <thead style=""background-color:#f5f5f5;"">")
                    result.Append("     <tr>")
                    result.Append("    <td align=""center"">Title Content</td>")
                    result.Append("    <td align=""center""></td>")
                    result.Append("     </tr>")
                    result.Append("  </thead>")
                
                    result.Append("  <tbody>")
                    For i = 0 To dt.Rows.Count - 1
                        If i + 1 >= firstRow Then

                            result.Append(" <tr>")
                            result.Append("     <td valign=""top"" align=""left"">" + dt.Rows(i).Item("title") + "</td>")
                            result.Append("     <td valign=""top"" align=""center""><a href=""javascript:doSelectProject(" + dt.Rows(i).Item("contentRef").ToString + ",'" + MyURLEncode(dt.Rows(i).Item("title")).ToString + "');"" style=""float:none;"">" + "select" + "</a></td>")
                            result.Append(" </tr>")

                            '''' to break after all row in a page done ''''
                            countPage = countPage + 1
                            If countPage >= numOfRowPerPage Then
                                Exit For
                            End If
                            '''' end break ''''
                        End If
                    Next

                    result.Append("  </tbody>")
                    result.Append("</table>")
                    result.Append("</div>")
                    result.Append("</div>")
                    ''''' end panel body ''''

                '''' paging ''''''
                    Dim urlPaging As String = ""
                    urlPaging = "javascript:bindChooseProject('" + keyword + "', @p);"
            
                '''' panel footer ''''
                    result.Append("<div class=""panel-footer"">")
                    result.Append("     <div class=""row"">")
                    result.Append("         <div style=""font-size:8pt;"">" + bindPaging(numOfRowPerPage, pageNo, numOfData, urlPaging, (countPage - 1)) + "</div>")
                    result.Append("     </div>")
                    result.Append("</div>")
                ''''' end panel footer ''''

                result.Append("</div>")
                    ''''' end of panel ''''

                Else
                    result.Append("<span style=""color:red;"">* No data found</span>")
                End If

            Context.Response.Clear()
            Context.Response.ContentType = "text/plain"
            Context.Response.Write(result.ToString())

    End Sub
    ' <WebMethod()> _
    '<ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    'Public Sub GetContentSvc(byval likeRef As String, ByVal contentRef As String, ByVal count As String)
    '    Dim dt As New DataTable
    '    Dim json As String = ""

    '    Try
    '        dt = getListContent(likeRef,contentRef, count)
    '    Catch ex As Exception
    '        dt = New DataTable
    '    End Try

    '    Dim jsonserializersettings = New JsonSerializerSettings()
    '    jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()
    '    json = JsonConvert.SerializeObject(dt, Formatting.Indented, jsonserializersettings)
    '    Context.Response.Clear()
    '    Context.Response.ContentType = "application/json"
    '    Context.Response.Write(json)
    'End Sub

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetProductType(byval categoryRef As String)
        Dim dt As New DataTable
        Dim json As String = ""
        Dim selProductTypeID As String = ""
        Dim result As New StringBuilder

        Try

                result.Append("<select class=""mdl-input"" id=""selProductTypeID"" name=""selProductTypeID"" >")
                result.Append("<option value="""" selected=""selected"" disabled=""disabled"" hidden=""hidden"">TIPE</option>")
            dt = getListProductType(categoryRef)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    If selProductTypeID = dt.Rows(i).Item("productTypeRef").ToString() Then
                        result.Append("<option value=""" + dt.Rows(i).Item("productTypeRef").ToString() + """ selected>" + dt.Rows(i).Item("productName").ToString() + "</option>")
                    Else
                        result.Append("<option value=""" + dt.Rows(i).Item("productTypeRef").ToString() + """>" + dt.Rows(i).Item("productName").ToString() + "</option>")
                    End If
                Next
            End If
                result.Append("</select>")
           
        Catch ex As Exception
            result = New StringBuilder
        End Try
        
        Context.Response.Clear()
        Context.Response.ContentType = "text/plain"
        Context.Response.Write(result.ToString())
    End Sub

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetContentSvc(ByVal contentRef As String, ByVal count As String)
        Dim json As Object
        Dim modelSurveyVendor As LikeModel = New LikeModel()

        Try
         
            Dim result As String = ""

            result = InsertListContent(contentRef,count)
            If IsNumeric(result) Then
                modelSurveyVendor.status = "200"
                modelSurveyVendor.message = "Success insert data survey vendor"
                'modelSurveyVendor.likeRef = likeRef
            Else
                modelSurveyVendor.status = "201"
                modelSurveyVendor.message = result
            End If
        Catch ex As Exception
            modelSurveyVendor.status = "201"
            modelSurveyVendor.message = ex.Message
        End Try

        Dim jsonserializersettings = New JsonSerializerSettings()
        jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()

        json = JsonConvert.SerializeObject(modelSurveyVendor, Formatting.Indented, jsonserializersettings)
        Context.Response.Clear()
        Context.Response.ContentType = "application/json"
        Context.Response.Write(json)
    End Sub

     <WebMethod()> _
    <ScriptMethod(UseHttpGet:=False, ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetContentProjectSvc(ByVal projectRef As String, ByVal count As String)
        Dim json As Object
        Dim modelSurveyVendor As LikeModel = New LikeModel()

        Try
         
            Dim result As String = ""

            result = InsertListContentProject(projectRef,count)
            If IsNumeric(result) Then
                modelSurveyVendor.status = "200"
                modelSurveyVendor.message = "Success insert data survey vendor"
                'modelSurveyVendor.likeRef = likeRef
            Else
                modelSurveyVendor.status = "201"
                modelSurveyVendor.message = result
            End If
        Catch ex As Exception
            modelSurveyVendor.status = "201"
            modelSurveyVendor.message = ex.Message
        End Try

        Dim jsonserializersettings = New JsonSerializerSettings()
        jsonserializersettings.ContractResolver = New CamelCasePropertyNamesContractResolver()

        json = JsonConvert.SerializeObject(modelSurveyVendor, Formatting.Indented, jsonserializersettings)
        Context.Response.Clear()
        Context.Response.ContentType = "application/json"
        Context.Response.Write(json)
    End Sub


End Class