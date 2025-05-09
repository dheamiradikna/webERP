Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports [class].clsGeneral
Imports [class].cls_mDomain

<ServiceContract(Namespace:="")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class wcf_mDomain

    <OperationContract()> _
    Public Function JSON_getDomainInfoHTML(ByVal domainRef As String) As String
        Dim dt As New DataTable

        dt = getDomainInfoHTML(domainRef)
        Return convertDTtoJSON("data", dt)
    End Function

End Class
