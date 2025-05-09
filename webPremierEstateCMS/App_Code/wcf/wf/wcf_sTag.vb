Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports [class].clsGeneral
Imports [class].cls_sTag

<ServiceContract(Namespace:="")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class wcf_sTag

    <OperationContract()> _
    Public Function JSON_getTagInfoHTML(ByVal domainRef As String, ByVal ref As String) As String
        Dim dt As New DataTable

        dt = getTagInfoHTML(domainRef, ref)
        Return convertDTtoJSON("data", dt)
    End Function

End Class
