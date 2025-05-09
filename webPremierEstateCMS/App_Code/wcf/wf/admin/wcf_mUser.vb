Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports [class].clsGeneral
Imports [class].cls_mUser

<ServiceContract(Namespace:="")> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class wcf_mUser

    <OperationContract()> _
    Public Function JSON_getUserInfoHTML(ByVal domainRef As String, ByVal userRef As String) As String
        Dim dt As New DataTable

        dt = getUserInfoHTML(domainRef, userRef)
        Return convertDTtoJSON("data", dt)
    End Function

End Class
