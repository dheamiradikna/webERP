Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports [class].clsGeneral
Imports [class].cls_sTagType

<ServiceContract(Namespace:="")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class wcf_sTagType

    <OperationContract()> _
    Public Function JSON_getTagTypeInfoHTML(ByVal domainRef As String, ByVal ref As String) As String
        Dim dt As New DataTable

        dt = gettagtypeInfoHTML(domainRef, ref)
        Return convertDTtoJSON("data", dt)
    End Function

End Class
