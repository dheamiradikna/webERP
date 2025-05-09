Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports [class].clsGeneral
Imports [class].clsGeneralDB

<ServiceContract(Namespace:="")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class wcfAddr

    ' Add <WebGet()> attribute to use HTTP GET 
    <OperationContract()> _
    Public Function JSON_getCountryList() As String
        Dim dt As New DataTable

        dt = getCountryList()
        Return convertDTtoJSON("data", dt)
    End Function

    <OperationContract()> _
    Public Function JSON_getProvinceList(ByVal countryCode As String) As String
        Dim dt As New DataTable

        dt = getProvinceList(countryCode)
        Return convertDTtoJSON("data", dt)
    End Function

    <OperationContract()> _
    Public Function JSON_getCityList(ByVal countryCode As String, ByVal provinceCode As String) As String
        Dim dt As New DataTable

        dt = getCityList(countryCode, provinceCode)
        Return convertDTtoJSON("data", dt)
    End Function

    <OperationContract()> _
    Public Function JSON_getCountryListAll() As String
        Dim dt As New DataTable

        dt = getCountryListAll()
        Return convertDTtoJSON("data", dt)
    End Function

    <OperationContract()> _
    Public Function JSON_getProvinceListAll(ByVal countryCode As String) As String
        Dim dt As New DataTable

        dt = getProvinceListAll(countryCode)
        Return convertDTtoJSON("data", dt)
    End Function

    <OperationContract()> _
    Public Function JSON_getCityListAll(ByVal countryCode As String, ByVal provinceCode As String) As String
        Dim dt As New DataTable

        dt = getCityListAll(countryCode, provinceCode)
        Return convertDTtoJSON("data", dt)
    End Function

    ' Add more operations here and mark them with <OperationContract()>

End Class
