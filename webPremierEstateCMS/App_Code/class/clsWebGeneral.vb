Imports Microsoft.VisualBasic
Imports [class].clsGeneralDB

Namespace [class]
    Public Class clsWebGeneral
        Public Shared _rootPath As String = ConfigurationManager.AppSettings.Item("rootPath").ToString()
        Public Shared _conStr As String = ConfigurationManager.AppSettings.Item("conStr").ToString()
        Public Shared _conStrSite As String = ConfigurationManager.AppSettings.Item("conStrSite").ToString()
        Public Shared _rowCountDefault As String = ConfigurationManager.AppSettings.Item("rowCountDefault").ToString()
        Public Shared _numOfDisplayPage As String = ConfigurationManager.AppSettings.Item("numOfDisplayPage").ToString()
        Public Shared _defDomain As String = ConfigurationManager.AppSettings.Item("defDomain").ToString()
        Public Shared _defUserStatus As String = ConfigurationManager.AppSettings.Item("defUserStatus").ToString()
        Public Shared _defDomainLevel As String = ConfigurationManager.AppSettings.Item("defDomainLevel").ToString()
        Public Shared _rowCountLookupDefault As String = ConfigurationManager.AppSettings.Item("rowCountLookupDefault").ToString()

        Public Shared _userStatusApproved As String = ConfigurationManager.AppSettings.Item("userStatusApproved").ToString()

        'nanti di setting domain
        Public Shared _imageBgColor As String = "0|0|0"

        Public Shared _imgSettingNoImage As String = "1"
        Public Shared _imgSettingSingle As String = "2"
        Public Shared _imgSettingSlideShow As String = "3"

        'Public Shared _parentDisplayListWithVideoThumbnail As String = "7"
        'Public Shared _parentTagRefNilaiNilaiPerusahaan As String = "16"
        Public Shared _contentTagRefBackgroundSlider As String = ConfigurationManager.AppSettings.Item("contentTagRefBackgroundSlider").ToString()
        'Public Shared _contentTagRefSejarah As String = ConfigurationManager.AppSettings.Item("contentTagRefSejarah").ToString()
        'Public Shared _parentTagRefNewsEvents As String = ConfigurationManager.AppSettings.Item("parentTagRefNewsEvents").ToString()
        'Public Shared _parentTagRefKomisi As String = ConfigurationManager.AppSettings.Item("parentTagRefKomisi").ToString()
        'Public Shared _parentTagRefDireksi As String = ConfigurationManager.AppSettings.Item("parentTagRefDireksi").ToString()
        Public Shared _parentTagRefNewsletter As String = ConfigurationManager.AppSettings.Item("parentTagRefNewsletter").ToString()

#Region "Super Admin Data"
        Public Shared __superUser As String = "nata"
        Public Shared __superPass As String = "nata"
        'Public Shared __superPass As String = "n4t4!@#$"
#End Region

    End Class

End Namespace
