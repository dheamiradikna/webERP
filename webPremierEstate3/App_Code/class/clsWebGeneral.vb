Imports Microsoft.VisualBasic
Imports [class].clsIntContentDb
Imports [class].clsGeneralDB
Imports System.Configuration

Namespace [class]
    Public Class clsWebGeneral

#Region "General"
        Public Shared _rootPath As String = ConfigurationManager.AppSettings.Item("rootPath").ToString()
        Public Shared _parentRef As String = ConfigurationManager.AppSettings.Item("parentRef").ToString()
        Public Shared _domainRef As String = ConfigurationManager.AppSettings.Item("domainRef")
        Public Shared _contentRef As String = ConfigurationManager.AppSettings.Item("contentRef")
        Public Shared _rootPathCMS As String = ConfigurationManager.AppSettings.Item("rootPathCMS").ToString()
        Public Shared _domainName As String = ConfigurationManager.AppSettings.Item("domainName").ToString()

        'Public Shared _projectCode As String = ConfigurationManager.AppSettings.Item("defaultProjectCode").ToString()
        'Public Shared _projectName As String = ConfigurationManager.AppSettings.Item("defaultProjectName").ToString()
        'Public Shared _entityCode As String = ConfigurationManager.AppSettings.Item("defaultEntityCode").ToString()

        Public Shared _tagTypeMenu As String = ConfigurationManager.AppSettings.Item("tagTypeMenu")
        Public Shared _langStartUp As String = ConfigurationManager.AppSettings.Item("langStartUp")

        Public Shared _parentDisplayListWithThumbnail As String = "1"
        Public Shared _parentDisplayThumbnailProduct As String = "10"
        Public Shared _parentDisplayPresentation As String = "8"
        Public Shared _parentDisplayListWithoutThumbnail As String = "2"
        Public Shared _parentDisplaySingleContentStandard As String = "3"
        Public Shared _parentDisplaySingleContentWithFeedbackForm As String = "4"
        Public Shared _parentDisplayTabContent As String = "5"
        Public Shared _parentDisplayColumnContent As String = "9"
        Public Shared _parentDisplayListWithCalendar As String = "6"
        Public Shared _parentDisplayWithTurnJS As String = "7"
        Public Shared _parentDisplaySlogan As String = "20"
        Public Shared _contentRefContactUs As String = "15"

        Public Shared _imgSettingNoImage As String = "1"
        Public Shared _imgSettingSingle As String = "2"
        Public Shared _imgSettingSlideShow As String = "3"

        Public Shared _imgPosLeft As String = "1"
        Public Shared _imgPosTopCenter As String = "2"
        Public Shared _imgPosBottomCenter As String = "3"
        Public Shared _imgPosMiddle As String = "4"

        Public Shared _sourceAdsWebSEO As String = "6"
        Public Shared _responseRef As String = "0"
        Public Shared _followUpAction As String = "4"

        Public Shared _tagTypeRefContent As String = ConfigurationManager.AppSettings.Item("tagTypeRefContent")
        Public Shared _tagTypeRefArtikel As String = ConfigurationManager.AppSettings.Item("tagTypeRefArtikel")

        Public Shared _imageBgColor As String = ConfigurationManager.AppSettings.Item("imageBgColor")
        Public Shared _numOfDisplayPage As String = ConfigurationManager.AppSettings.Item("numOfDisplayPage").ToString()
        Public Shared _numOfRowCountPage As String = ConfigurationManager.AppSettings.Item("numOfRowCountPage").ToString()
        Public Shared _numOfRowImageCountPage As String = ConfigurationManager.AppSettings.Item("numOfRowImageCountPage").ToString()

        Public Shared _isDomainUnderPrime As String = ConfigurationManager.AppSettings.Item("isDomainUnderPrime")
        'Public Shared _categoryApartment As String = ConfigurationManager.AppSettings.Item("categoryApartment").ToString()
        'Public Shared _categoryHousing As String = ConfigurationManager.AppSettings.Item("categoryHousing").ToString()

#End Region

#Region "Database"
        Public Shared _conStr As String = ConfigurationManager.AppSettings.Item("conStr").ToString()
        Public Shared _conStrLDS As String = ConfigurationManager.AppSettings.Item("conStrLDS").ToString()
        Public Shared _conStrNC As String = ConfigurationManager.AppSettings.Item("conStrNC").ToString()
        'Public Shared _naproConStr As String = ConfigurationManager.AppSettings.Item("naproConStr").ToString()
        Public Shared _NSMainDB As String = ConfigurationManager.AppSettings.Item("namespaceMainDB").ToString()
#End Region

#Region "DBMasterRef, projectRef"
        'Public Shared _dbMasterRef As String = getDbMasterRefByProjectCode(_projectCode)
        'Public Shared _projectRef As String = getProjectRefByProjectCode(_projectCode)
#End Region

#Region "Meta"
        Public Shared _metaRef As String = "1"
        Public Shared _metaTitle As String = getMetaTitle(_domainRef, _metaRef)
        Public Shared _metaAuthor As String = getMetaAuthor(_domainRef, _metaRef)
        Public Shared _metaKeyword As String = getMetaKeyword(_domainRef, _metaRef)
        Public Shared _metaDescription As String = getMetaDescription(_domainRef, _metaRef)
#End Region

#Region "Whatsapp"
        Public Shared _WAnoHP As String = ConfigurationManager.AppSettings.Item("WAnoHP").ToString()
        Public Shared _WAnoHP1 As String = ConfigurationManager.AppSettings.Item("WAnoHP1").ToString()
        Public Shared _WAnoHP2 As String = ConfigurationManager.AppSettings.Item("WAnoHP2").ToString()
        Public Shared _WAnoHP3 As String = ConfigurationManager.AppSettings.Item("WAnoHP3").ToString()
        Public Shared _WAgreetingMessage As String = ConfigurationManager.AppSettings.Item("WAgreetingMessage").ToString()
        Public Shared _WAcallToAction As String = ConfigurationManager.AppSettings.Item("WAcallToAction").ToString()
#End Region

#Region "Tag"
        'Public Shared _settingRef As String = ConfigurationManager.AppSettings.Item("settingRef").ToString()

        Public Shared _linkIOS As String = ConfigurationManager.AppSettings.Item("linkIOS").ToString()
        Public Shared _linkPlayStore As String = ConfigurationManager.AppSettings.Item("linkPlayStore").ToString()
        Public Shared _linkInstagram As String = ConfigurationManager.AppSettings.Item("linkInstagram").ToString()
        Public Shared _linkFb As String = ConfigurationManager.AppSettings.Item("linkFb").ToString()
        Public Shared _callUsNow As String = ConfigurationManager.AppSettings("callUsNow")
        Public Shared _address As String = ConfigurationManager.AppSettings("address")
        Public Shared _websiteProjectName As String = ConfigurationManager.AppSettings("websiteProjectName")
        Public Shared _emailWeb As String = ConfigurationManager.AppSettings("emailWeb")
        Public Shared _urlWeb As String = ConfigurationManager.AppSettings("urlWeb")
        Public Shared _newsContentType As String = ConfigurationManager.AppSettings("newsContentType")

        Public Shared _tagRefHome As String = ConfigurationManager.AppSettings("tagRefHome")
        Public Shared _tagRefKeunggulan As String = ConfigurationManager.AppSettings("tagRefKeunggulan")
        Public Shared _tagRefAksesibilitas As String = ConfigurationManager.AppSettings("tagRefAksesibilitas")
        Public Shared _tagRefDetail As String = ConfigurationManager.AppSettings("tagRefDetail")
        Public Shared _tagRefTipe As String = ConfigurationManager.AppSettings("tagRefTipe")
        Public Shared _tagRefKeunggulan2 As String = ConfigurationManager.AppSettings("tagRefKeunggulan2")
        Public Shared _tagRefDetail1 As String = ConfigurationManager.AppSettings("tagRefDetail1")
        Public Shared _tagRefDetail2 As String = ConfigurationManager.AppSettings("tagRefDetail2")
        Public Shared _tagRefDetail3 As String = ConfigurationManager.AppSettings("tagRefDetail3")
        Public Shared _tagRefDetail4 As String = ConfigurationManager.AppSettings("tagRefDetail4")
        Public Shared _tagRefTestimoni As String = ConfigurationManager.AppSettings("tagRefTestimoni")
        Public Shared _tagRefBlog As String = ConfigurationManager.AppSettings("tagRefBlog")

        Public Shared _projectRef As String = ConfigurationManager.AppSettings.Item("projectRef")
        Public Shared _urlGenerateCaptcha As String = ConfigurationManager.AppSettings.Item("urlGenerateCaptcha").ToString()

        'untuk settingan google Analytic & google Ads
        Public Shared _googleAnalyticCode As String = ConfigurationManager.AppSettings.Item("googleAnalyticCode").ToString()
        Public Shared _googleAdsCode As String = ConfigurationManager.AppSettings.Item("googleAdsCode").ToString()
        Public Shared _googleTrackingWACode As String = ConfigurationManager.AppSettings.Item("googleTrackingWACode").ToString()
        'untuk settingan google Analytic & google Ads

#End Region

#Region "Email"
        Public Shared _emailHost As String = ConfigurationManager.AppSettings.Item("emailHost")
        Public Shared _emailUsername As String = ConfigurationManager.AppSettings.Item("emailUsername")
        Public Shared _emailPassword As String = ConfigurationManager.AppSettings.Item("emailPassword")
        Public Shared _emailPort As String = ConfigurationManager.AppSettings.Item("emailPort")
#End Region




    End Class

End Namespace
