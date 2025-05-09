Imports System.Data
Imports [class].clsWebGeneral


Partial Class wc_parentFooter
    Inherits System.Web.UI.UserControl

    Protected rootPath As String = _rootPath

    Private Sub bindFooter()
        Dim result As New StringBuilder
        Dim rootPath As String = _rootPath

            result.Append("    <footer class=""footer-area"">")
            result.Append("      <div class=""footer-top pt-100 pb-70"">")
            result.Append("        <div class=""container"">")
            result.Append("          <div class=""row justify-content-between"">")
            result.Append("             <div class=""col-lg-4 col-xxl-3 col-md-6"">")
            result.Append("                 <div class=""single-footer-widget"">")
            result.Append("                  <a href=""#"" class=""logo"">")
            result.Append("                  <img width=""180"" height=""100"" src="""+_rootPath +"/Support/img/logoPremier3.png"" class=""footer-logo"" alt=""Logo""></a>")
            result.Append("                   <p> Lorem ipsum dolor sit ame consectetur adisicing elitsed do eiusmod tempor labet dolore magna aliquaUt</p>")

            result.Append("            </div>")
            result.Append("            </div>")
            'result.Append("                 <div class=""col-lg-2 col-xxl-3 col-md-6"">")
            'result.Append("                 <div class=""single-footer-widget pl-70"">")
            'result.Append("                 <h3>SERVICES</h3>")
            'result.Append("                     <ul class=""footer-list"">")
            'result.Append("					    </li>")
            'result.Append("					            <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    Property on Sale</a>")
            'result.Append("					        </li>")
            'result.Append("					        <li>")
            'result.Append("					             <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    About Us</a>")
            'result.Append("					                </li>")
            'result.Append("					        <li>")
            'result.Append("					             <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    Our Team</a>")
            'result.Append("					                </li>")
            'result.Append("					        <li>")
            'result.Append("					             <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    Terms of Use</a>")
            'result.Append("					                </li>")
            'result.Append("					       </ul>")
            'result.Append("					   </div>")
            'result.Append("					   </div>")
            'result.Append("                 <div class=""col-lg-3 col-xxl-3 col-md-6"">")
            'result.Append("                 <div class=""single-footer-widget pl-70"">")
            'result.Append("                 <h3>SERVICES</h3>")
            'result.Append("                     <ul class=""footer-list"">")
            'result.Append("					    </li>")
            'result.Append("					            <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    Property on Sale</a>")
            'result.Append("					        </li>")
            'result.Append("					        <li>")
            'result.Append("					             <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    About Us</a>")
            'result.Append("					                </li>")
            'result.Append("					        <li>")
            'result.Append("					             <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    Our Team</a>")
            'result.Append("					                </li>")
            'result.Append("					        <li>")
            'result.Append("					             <a href=""#"" target=""_blank"">")
            'result.Append("					                <i class=""bx bx-plus""></i>")
            'result.Append("					                    Terms of Use</a>")
            'result.Append("					                </li>")
            'result.Append("					       </ul>")
            'result.Append("					   </div>")
            'result.Append("					   </div>")
            result.Append("                 <div class=""col-lg-3 col-xxl-3 col-md-6"">")
            result.Append("                 <div class=""single-footer-widget pl-3"">")
            result.Append("                 <h3>CONTACT INFO</h3>")
            result.Append("                     <ul class=""footer-contact-list"">")
            result.Append("					        <li>")
            result.Append("					            JL. RAYA KRANGGAN NO. 50, JATIRADEN, KOTA BEKASI, JAWA BARAT 17433")
            result.Append("					        </li>")
            result.Append("					        <li>")
            result.Append("					          <a href=""tel:6282122816000"">+62 2122816000</a>")
            result.Append("					        </li>")
            result.Append("					        <li>")
            result.Append("					          <a href=""/cdn-cgi/l/email-protection#a5cccbc3cae5eac3d1cad5cccbc68bc6cac8""><span class=""__cf_email__"" data-cfemail=""b2dbdcd4ddf2ddd4c6ddc2dbdcd19cd1dddf"">premierindonesia.com</span></a>")
            result.Append("					        </li>")
            result.Append("					       </ul>")
            result.Append("					   </div>")
            result.Append("					   </div>")
            result.Append("					   </div>")
            result.Append("					   </div>")
            result.Append("					   </div>")
            
            result.Append("                    <div class=""footer-bottom"">")
            result.Append("                       <div class=""container"">")
            result.Append("                          <div class=""bottom-text"">")
            result.Append("                             <p>")
            result.Append("                             Copyright © <script data-cfasync=""false"" src=""../../cdn-cgi/scripts/5c5dd728/cloudflare-static/email-decode.min.js""></script><script>document.write(new Date().getFullYear())</script> Premier Estate 3. Prepared by")
            result.Append("                             <a href=""#""target=""_blank"">Nata Connexindo</a>")
            result.Append("                             </p>")
            result.Append("                         </div>")
            result.Append("                     </div>")
            result.Append("                   </div>")
            result.Append("     </footer>")


        ltrFooter.Text = result.ToString

    End Sub
    
    Private Sub bindWhatsapp()
        Dim result As New StringBuilder

        If _googleTrackingWACode <> "" Then
            'result.Append("                 <a onclick=""loadGTM();return gtag_report_conversion('https://api.whatsapp.com/send?phone=" + _WAnoHP + "&text=" + _WAgreetingMessage + "')"" style=""cursor: pointer;"" target=""_blank"">")
            result.Append("                 <a onclick=""doPopupRequestCall2();loadGTM();gtag_report_conversion()"" style=""cursor: pointer;"">")
        Else
            'result.Append("                 <a href=""https://api.whatsapp.com/send?phone=" + _WAnoHP + "&text=" + _WAgreetingMessage + """ style=""cursor: pointer;"" target=""_blank"">")    
            result.Append("                 <a onclick=""doPopupRequestCall2();"" style=""cursor: pointer;"">")
        End If
        result.Append("                           <img class=""Phone is-animating"" src=""" + _rootPath + "support/img/chatYuk.png"" alt=""imgWAchatYuk"">")
        result.Append("                     </a>")

        ltrWhatsapp.Text = result.ToString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim q_tagRef As String = Request.QueryString("t")
        Dim q_domainRef As String = Request.QueryString("d")

        Dim result As New StringBuilder
        If IsNothing(q_tagRef) Then q_tagRef = ""
        If IsNothing(q_domainRef) Then q_domainRef = ""

        If IsPostBack Then
            Response.Redirect(_rootPath + "Default.aspx")
        End If

        bindFooter()
        bindWhatsapp()
    End Sub

End Class
