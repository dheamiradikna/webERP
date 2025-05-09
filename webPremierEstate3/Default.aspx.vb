Imports System.Data
Imports System.Text
Imports [class].clsWebGeneral
Imports [class].clsContentDB
Imports [class].clsGeneralDB
Imports [class].clsGeneral
Imports [class].clsGeneralSetting

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected rootPath As String = _rootPath
    Protected projectRef As String = _projectRef
    Protected noted As String = ""

    <System.Web.Services.WebMethod()> _
    Public Shared Function generateLink(ByVal keyword As String) As String

        Dim hastable As New Hashtable
        hastable("keyword") = keyword

        Dim result As String = GetEncUrl(_rootPath + "search-all/?", hastable)

        Return result
    End Function


    Private Sub bindHome()
        Dim result As New StringBuilder
        Dim dt As New DataTable
        Dim dtImage As New DataTable

        dt = getContentInfoByTag(_domainRef, _tagRefHome, False)
        dtImage = getImageRefByContent(dt.Rows(0).Item("contentRef").ToString)
        'result.Append("  <li><img width=""250"" height=""500"" class=""lazyload image-banner"" src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/10/10" + ".jpg"" data-src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/1268/757" + ".jpg"" alt=""" + dt.Rows(0).Item("title") + """></li>")
      

        'result.Append("<div class=""home-slider-area"">")
        'result.Append("    <div class=""container-fluid m-0 p-0"">")
        'result.Append("        <div class=""home-slider owl-carousel owl-theme"">")
        'result.Append("            <div class=""slider-item"">")
        'result.Append("                <div class=""row align-items-center"">")
        'result.Append("                    <div class=""col-lg-5 col-xxl-6"">")
        'result.Append("                        <div class=""home-slider-content"">")
        'result.Append("                            "+dt.Rows(0).Item("content")+"")
        'result.Append("                            <div class=""home-slider-btn"">")
        'result.Append("                                <a href=""#"" class=""default-btn"">")
        'result.Append("                                    Check Appointment")
        'result.Append("                                    <i class=""bx bx-right-arrow-alt""></i>")
        'result.Append("                                </a>")
        'result.Append("                                <a href=""contact.html"" class=""default-btn active"">")
        'result.Append("                                    Contact Us")
        'result.Append("                                    <i class=""bx bx-right-arrow-alt""></i>")
        'result.Append("                                </a>")
        'result.Append("                            </div>")
        'result.Append("                        </div>")
        'result.Append("                    </div>")
        'result.Append("                    <div class=""col-lg-7 col-xxl-6 pr-0"">")
        'result.Append("                        <div class=""home-slider-img"">")
        'result.Append("                            <img width=""250"" height=""500"" class=""lazyload image-banner"" data-src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/10/10" + ".jpg"" src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/1268/757" + ".jpg"" alt=""" + dt.Rows(0).Item("title") + """>")
        'result.Append("                        </div>")
        'result.Append("                    </div>")
        'result.Append("                </div>")
        'result.Append("            </div>")
        'result.Append("</div>")
        'result.Append("</div>")
        'result.Append("</div>")
        'result.Append("</div>")


        result.Append("<div class=""home-slider-area"">")
        result.Append("    <div class=""container-fluid m-0 p-0"">")
        result.Append("        <div>")
        result.Append("            <div class=""slider-item"">")
        result.Append("                <div class=""row align-items-center"">")
        result.Append("                    <div class=""col-lg-5 col-xxl-6"">")
        result.Append("                        <div class=""home-slider-content"">")
        result.Append("                            " + dt.Rows(0).Item("content") + "")
        result.Append("                            <div class=""home-slider-btn"">")
        'result.Append("                                <a href=""#"" class=""default-btn"">")
        'result.Append("                                    Check Appointment")
        'result.Append("                                    <i class=""bx bx-right-arrow-alt""></i>")
        'result.Append("                                </a>")
        result.Append("                                <a href=""#"" class=""default-btn active"">")
        result.Append("                                    Contact Us")
        result.Append("                                    <i class=""bx bx-right-arrow-alt""></i>")
        result.Append("                                </a>")
        result.Append("                            </div>")
        result.Append("                        </div>")
        result.Append("                    </div>")
        result.Append("                    <div class=""col-lg-7 col-xxl-6 pr-0"">")
        result.Append("                        <div class=""home-slider-img"">")  
        result.Append("                            <img width=""1000"" height=""500"" class=""lazyload image-banner"" data-src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/10/10" + ".jpg"" src=""" + _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/1268/757" + ".jpg"" alt=""" + dt.Rows(0).Item("title") + """>")
        result.Append("                        </div>")
        result.Append("                    </div>")
        result.Append("                </div>")
        result.Append("            </div>")
        result.Append("        </div>")
        result.Append("    </div>")
        result.Append("</div>")


            ltrHome.Text = result.ToString
    End Sub

    Private Sub bindKeunggulan()
        Dim result As New StringBuilder
        Dim dtImage As New DataTable
        Dim dtKeunggulan As New DataTable

        dtKeunggulan = getContentInfoByTag(_domainRef, _tagRefKeunggulan, False)

        If dtKeunggulan.Rows.Count > 0
            result.Append("    <div class=""service-area pt-100 pb-70"">")
            result.Append("      <div class=""container"">")
            result.Append("        <div class=""row"">")

            For i As Integer = 0 To dtKeunggulan.Rows.Count - 1
                dtImage = getImageRefByContent(dtKeunggulan.Rows(0).Item("contentRef"))
                

                result.Append("           <div class=""col-lg-3 col-sm-6"">")
                result.Append("             <div class=""service-card service-card-bg"">") 
                Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
               
                If dtImage.Rows.Count > 0
                    Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".png"" alt=""" + dtKeunggulan.Rows(i).Item("title")
                    result.Append("                <img class=""thumb-img"" src="""+img+""" alt=""image "+dtKeunggulan.Rows(i).Item("title")+""" />")
                Else
                    result.Append("                <img style=""width:100%"" src="""+noimg+""" alt=""image "+dtKeunggulan.Rows(i).Item("title")+""" />")
                End If
                result.Append("                <a href=""#"">")
                result.Append("                     <h3>"+dtKeunggulan.Rows(i).Item("title")+"</h3>")
                result.Append("                </a>")
                'result.Append("                <p class=""text-break"">Lorem ipsum dolor sitameem adipiscing cnsectetur adisci- mod tur adipiscing</p>")
                result.Append("                <p> "+dtKeunggulan.Rows(i).Item("synopsis")+"</p>")
                result.Append("                    <a href=""#"" class=""learn-more-btn"">")
                result.Append("                     Learn More")
                result.Append("                     <i class=""bx bx-right-arrow-alt""></i>")
                result.Append("                    </a>")
                result.Append("             </div>")
                result.Append("           </div>")
            Next
         
            result.Append("           </div>")
            result.Append("         </div>")
            result.Append("       </div>")
        End If

      
        
        ltrKeunggulan.Text = result.ToString
    End Sub

    Private Sub bindAksesibilitas()
        Dim result As New StringBuilder
        Dim dtAksesibilitas As New DataTable
        Dim dtDetail As DataTable
        Dim dtImage As DataTable

        dtAksesibilitas = getContentInfoByTag(_domainRef, _tagRefAksesibilitas, False)
        dtDetail = getContentInfoByTag(_domainRef, _tagRefDetail, False)
        dtImage = getImageRefByContent(dtAksesibilitas.Rows(0).Item("contentRef"))
        Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
        If dtAksesibilitas.Rows.Count > 0 Then
        Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".png"" alt=""" + dtAksesibilitas.Rows(0).Item("title")
        result.Append("    <div class=""property-area pt-100 pb-70"">")
        result.Append("       <div class=""container-fluid"">")
        result.Append("          <div class=""row align-items-center"">")
        result.Append("             <div class=""col-lg-5 ps-0"">")
        result.Append("                 <div class=""property-img"">")
        result.Append("                    <a href=""#"">")
        'result.Append("                        <img width=""700"" height=""600"" src="""+_rootPath +"/Support/img/Image_H2.jpg"" alt=""Images"">")
        result.Append("                         <img width=""700"" height=""600"" src="""+img+""" alt=""image "+dtAksesibilitas.Rows(0).Item("title")+""" />")
        result.Append("                    </a>")
        result.Append("                 </div>")
        result.Append("             </div>")
        result.Append("             <div class=""col-lg-7"">")
        result.Append("                 <div class=""property-item"">")
        result.Append("                    <div class=""section-title"">")

        result.Append("                     <p>" + dtAksesibilitas.Rows(0).Item("content") + "</p>")
            'result.Append("                            " + dtAksesibilitas.Rows(0).Item("content") + "")
            'result.Append("                           <h2>")
            'result.Append("                              <a href=""property-details.html"">")
            'result.Append("                                 Lorem ipsum")
            'result.Append("                                     <b>dolor sit amet,</b>")
            'result.Append("                              </a>")
            'result.Append("                           </h2>")
        result.Append("<h2><a href=""#""> " + dtAksesibilitas.Rows(0).Item("title") + "</a></h2>")
        result.Append("                     <span> "+dtAksesibilitas.Rows(0).Item("synopsis")+ "</span>")
        result.Append("                     </div>")
        result.Append("                   <div class=""property-counter"">")
        result.Append("                     <div class=""row"">")
            For i As Integer = 0 To dtDetail.Rows.Count - 1
                result.Append("                        <div class=""col-lg-3 col-sm-6 col-md-3"">")
                result.Append("                           <div class=""counter-card counter-card-rs"">")
                result.Append("                               <h2>"+dtDetail.Rows(i).Item("title")+"</h2>")
                result.Append("                                 <h3>"+dtDetail.Rows(i).Item("titledetail")+"</h3>")
                result.Append("                                    <span>"+dtDetail.Rows(i).Item("synopsis")+"</span>")
                result.Append("                           </div>")
                result.Append("                         </div>")
            Next 
    
        result.Append("                     </div>")
        result.Append("                   </div>")
        result.Append("                 </div>")
        result.Append("                </div>")
        result.Append("              </div>")
        result.Append("            </div>")
        result.Append("         </div>")
        End If

         ltrArea.Text = result.ToString
    End Sub

    Private Sub bindTipe()
        Dim result As New StringBuilder
        Dim dtTipe As New DataTable
        Dim dtImage As New DataTable
        Dim dtPropertyType As New DataTable
        
        dtTipe = getContentInfoByTag(_domainRef,_tagRefTipe, False)
        If dtTipe.Rows.Count > 0

            result.Append("   <section class=""property-section pb-70"">")
            result.Append("     <div class=""container-fluid"">")
            result.Append("       <div class=""container-max"">")
            result.Append("         <div class=""property-section-text"">")
            result.Append("            <div class=""section-title"">")
            'result.Append("                            " +  dtTipe.Rows(0).Item("title") + "")
            'result.Append("                   <h2>")
            'result.Append("                      Unit")
            'result.Append("                         <b></b>")
            'result.Append("                   </h2>")
            result.Append("                     <p>"+dtTipe.Rows(0).Item("title")+"</p>")
            result.Append("                     <h2>"+dtTipe.Rows(0).Item("titledetail")+"</h2>")
            result.Append("            </div>")
            result.Append("         </div>")
            result.Append("         <div class=""row pt-45"">")

             For i As Integer = 0 To dtTipe.Rows.Count - 1
                dtImage = getImageRefByContent(dtTipe.Rows(i).Item("contentRef"))

                result.Append("            <div class=""col-lg-4 col-md-6"">")
                result.Append("               <div class=""single-property"">")
                     Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
                If dtImage.Rows.Count > 0
                    Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/300/350" + ".jpg"" alt=""" + dtTipe.Rows(i).Item("title")
                    result.Append("                <img style=""width:100%"" src="""+img+""" alt=""image "+dtTipe.Rows(i).Item("title")+""" />")
                Else
                    result.Append("                <img style=""width:100%"" src="""+noimg+""" alt=""image "+dtTipe.Rows(i).Item("title")+""" />")
                End If

            result.Append("                  <div class=""images"">")
            result.Append("                     <a href=""#"">")
            'result.Append("                        <img width=""300"" height=""350"" src="""+_rootPath +"/Support/img/Tipe Besancon.jpg"" alt=""Images"">")
            result.Append("                     </a>")
            result.Append("                  <div class=""property-content"">")
            result.Append("                     <span>IN PROGRESS</span>")
            result.Append("                        <a href=""#"">")
            result.Append("                     <h3>"+dtTipe.Rows(i).Item("content")+"</h3>")
            result.Append("                        </a>")
            'result.Append("                           <p>Details ipsum dolor sitameLorem adipiscing cnsectetur adipiscing mod</p>")
            result.Append("                                <a href=""#"" class=""learn-more-btn"">")
            result.Append("                                   <i class=""bx bx-right-arrow-alt""></i>")
            result.Append("                                      Learn More")
            result.Append("                                </a>")
            result.Append("                  </div>")
            result.Append("                </div>")
            result.Append("             </div>")
            result.Append("           </div>")
            Next
        result.Append("                     </div>")
        result.Append("                   </div>")
        result.Append("                 </div>")
        result.Append("     </section>")

        End If

         ltrProperty.Text = result.ToString
    End Sub

    Private Sub bindKeunggulan2()
        Dim result As New StringBuilder
        Dim dtKeunggulan2 As New DataTable
        Dim dtImage As New DataTable
        Dim dtDetail As DataTable

        dtKeunggulan2 = getContentInfoByTag(_domainRef,_tagRefKeunggulan2, False)

        If dtKeunggulan2.Rows.Count > 0 Then

            result.Append("     <div class=""room-details-area pb-70"">")
            result.Append("        <div class=""container-fluid"">")
            result.Append("           <div class=""container-max"">")
            result.Append("              <div class=""section-title text-center"">")
            result.Append("              <span>"+dtKeunggulan2.Rows(0).Item("synopsis")+"</span>")
            result.Append("                    <h2 class=""margin-auto"">" +  dtKeunggulan2.Rows(0).Item("titledetail") + "</h2>")
            result.Append("              </div>")
            result.Append("              <div class=""tab room-details-tab"">")
            result.Append("                 <ul class=""tabs"">")
            For x As Integer = 0 To dtKeunggulan2.Rows.Count - 1
                result.Append("                   <li class=""li-custom"">")
                result.Append("                     <a href=""#"">" + dtKeunggulan2.Rows(x).Item("title") + "</a>")
                result.Append("                   </li>")
            Next
            result.Append("                 </ul>")
            result.Append("                 <div class=""tab_content current active pt-45"">")
            For i As Integer = 0 To dtKeunggulan2.Rows.Count - 1

                result.Append("              <div class=""tabs_item"">")
                result.Append("                 <div class=""row"">")

                If dtKeunggulan2.Rows.Count > 3 Then
                    If i = 0 Then
                        dtDetail = getContentInfoByTag(_domainRef, _tagRefDetail1, False)

                        For z As Integer = 0 To dtDetail.Rows.Count - 1
                            dtImage = getImageRefByContent(dtDetail.Rows(z).Item("contentRef"))

                            result.Append("                     <div class=""col-lg-4 col-md-6"">")
                            result.Append("                         <div class=""room-details-card"">")
                            Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
                            If dtImage.Rows.Count > 0
                                Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".jpg"" alt=""" + dtDetail.Rows(z).Item("title")
                                result.Append("                         <img style=""width:100%"" src="""+img+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            Else
                                result.Append("                         <img style=""width:100%"" src="""+noimg+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            End If
                            result.Append("                             <a href=""#"">")
                            result.Append("                             </a>")
                            result.Append("                             <div class=""content"">")
                            result.Append("                                 <h4>"+dtDetail.Rows(z).Item("title")+"</h4>")
                            result.Append("                                 <p>"+dtDetail.Rows(z).Item("synopsis")+"</p>")
                            result.Append("                             </div>")
                            result.Append("                          </div>")
                            result.Append("                      </div>")
                        Next
                    End If

                    If i = 1 Then
                        dtDetail = getContentInfoByTag(_domainRef, _tagRefDetail2, False)

                        For z As Integer = 0 To dtDetail.Rows.Count - 1
                            dtImage = getImageRefByContent(dtDetail.Rows(z).Item("contentRef"))

                            result.Append("                     <div class=""col-lg-4 col-md-6"">")
                            result.Append("                         <div class=""room-details-card"">")
                            Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
                            If dtImage.Rows.Count > 0
                                Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".jpg"" alt=""" + dtDetail.Rows(z).Item("title")
                                result.Append("                         <img style=""width:100%"" src="""+img+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            Else
                                result.Append("                         <img style=""width:100%"" src="""+noimg+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            End If
                            result.Append("                             <a href=""#"">")
                            result.Append("                             </a>")
                            result.Append("                             <div class=""content"">")
                            result.Append("                                 <h4>"+dtDetail.Rows(z).Item("title")+"</h4>")
                            result.Append("                                 <p>"+dtDetail.Rows(z).Item("synopsis")+"</p>")
                            result.Append("                             </div>")
                            result.Append("                          </div>")
                            result.Append("                      </div>")
                        Next
                    End If

                     If i = 2 Then
                        dtDetail = getContentInfoByTag(_domainRef, _tagRefDetail3, False)

                        For z As Integer = 0 To dtDetail.Rows.Count - 1
                            dtImage = getImageRefByContent(dtDetail.Rows(z).Item("contentRef"))

                            result.Append("                     <div class=""col-lg-4 col-md-6"">")
                            result.Append("                         <div class=""room-details-card"">")
                            Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
                            If dtImage.Rows.Count > 0
                                Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".jpg"" alt=""" + dtDetail.Rows(z).Item("title")
                                result.Append("                         <img style=""width:100%"" src="""+img+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            Else
                                result.Append("                         <img style=""width:100%"" src="""+noimg+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            End If
                            result.Append("                             <a href=""#"">")
                            result.Append("                             </a>")
                            result.Append("                             <div class=""content"">")
                            result.Append("                                 <h4>"+dtDetail.Rows(z).Item("title")+"</h4>")
                            result.Append("                                 <p>"+dtDetail.Rows(z).Item("synopsis")+"</p>")
                            result.Append("                             </div>")
                            result.Append("                          </div>")
                            result.Append("                      </div>")
                        Next
                    End If

                     If i = 3 Then
                        dtDetail = getContentInfoByTag(_domainRef, _tagRefDetail4, False)

                        For z As Integer = 0 To dtDetail.Rows.Count - 1
                            dtImage = getImageRefByContent(dtDetail.Rows(z).Item("contentRef"))

                            result.Append("                     <div class=""col-lg-4 col-md-6"">")
                            result.Append("                         <div class=""room-details-card"">")
                            Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
                            If dtImage.Rows.Count > 0
                                Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".jpg"" alt=""" + dtDetail.Rows(z).Item("title")
                                result.Append("                         <img style=""width:100%"" src="""+img+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            Else
                                result.Append("                         <img style=""width:100%"" src="""+noimg+""" alt=""image "+dtDetail.Rows(z).Item("title")+""" />")
                            End If
                            result.Append("                             <a href=""#"">")
                            result.Append("                             </a>")
                            result.Append("                             <div class=""content"">")
                            result.Append("                                 <h4>"+dtDetail.Rows(z).Item("title")+"</h4>")
                            result.Append("                                 <p>"+dtDetail.Rows(z).Item("synopsis")+"</p>")
                            result.Append("                             </div>")
                            result.Append("                          </div>")
                            result.Append("                      </div>")
                        Next
                    End If

                End If

                result.Append("                  </div>")
                result.Append("              </div>")
            Next
            result.Append("         </div>")
            result.Append("       </div>")
            result.Append("     </div>")
            result.Append("    </div>")
            result.Append("   </div>")
        End If

         ltrQtyManagement.Text = result.ToString
    End Sub

    Private Sub bindTestimoni()
        Dim result As New StringBuilder
        Dim dtTestimoni As New DataTable
        Dim dtImage As New DataTable

        dtTestimoni = getContentInfoByTag(_domainRef,_tagRefTestimoni, False)
        dtImage = getImageRefByContent(dtTestimoni.Rows(0).Item("contentRef"))
        Dim noImg As String = _rootPath + "support/img/placeholder.jpg"
        If dtTestimoni.Rows.Count > 0 Then
        Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".jpg"" alt=""" + dtTestimoni.Rows(0).Item("title")
        result.Append("     <div class=""forward-area"">")
        result.Append("        <div class=""container"">")
        result.Append("           <div class=""row align-items-center"">")
        result.Append("              <div class=""col-lg-6"">")
        result.Append("                 <div class=""forward-img"">")
        'result.Append("                     <img width=""500"" height=""500"" src="""+_rootPath +"/Support/img/Testimoni.jpg"" alt=""Images"">")
        'result.Append("                         <img width=""500"" height=""500"" src="""+img+""" alt=""image "+dtTestimoni.Rows(0).Item("title")+""" />")
        result.Append("                 </div>")
        result.Append("              </div>")
        result.Append("           <div class=""col-lg-6"">")
        result.Append("           <div class=""forward-content"">")
        result.Append("             <div class=""section-title"">")
        result.Append("                 <p>"+dtTestimoni.Rows(0).Item("title")+"</p>")
        result.Append("                     <h2>"+dtTestimoni.Rows(0).Item("titledetail")+"</h2>")
        result.Append("                         <p>"+dtTestimoni.Rows(0).Item("synopsis")+"</p>")
        'result.Append("                             Lorem ipsum dolor sit ame consectetur adipisicing elit, sed do eiusmod tempor ")
        'result.Append("                             incididunt ut labore et dolore magna aliquaUt enim ad minim vequis nostrud exercitation")
        'result.Append("                         </p>")
        result.Append("             </div>")
        result.Append("             <div class=""signature"">")
        result.Append("                 <ul>")
        'result.Append("                     <li>")
        'result.Append("                         <img src=""assets/img/signature.png"" class=""signature-img1"" alt=""Images"">")
        'result.Append("                         <img src=""assets/img/signature2.png"" class=""signature-img2"" alt=""Images"">")
        'result.Append("                     </li>")
        result.Append("                      <li>")
        result.Append("                         <h3>"+dtTestimoni.Rows(0).Item("content")+"</h3>")
        'result.Append("                             <span>Penghuni Premier Estate 3</span>")
        result.Append("                 </li>")
        result.Append("                 </ul>")
        result.Append("             </div>")
        result.Append("             <a href=""#"" class=""default-btn default-bg-buttercup"">")
        result.Append("                 Finalize Meeting")
        result.Append("                 <i class=""bx bx-right-arrow-alt""></i>")
        result.Append("             </a>")
        result.Append("          </div>")
        result.Append("        </div>")
        result.Append("      </div>")
        result.Append("     </div>")
        result.Append("   </div>")
        End If


        ltrMessage.Text = result.ToString
    End Sub

    'Private Sub bindBlog()
    '    Dim result As New StringBuilder
    '    Dim dtBlog As New DataTable
    '    Dim dtImage As New DataTable
    '    Dim tagName As String = getTagName(_domainRef, _tagRefBlog, False)
    '    Dim strPublishDate As String = ""

    '    dtBlog = getContentInfoByTag(_domainRef,_tagRefBlog, False)

    '    result.Append("     <div class=""blog-area pb-70"">")
    '    result.Append("         <div class=""container"">")
    '    result.Append("             <div class=""section-title text-center"">")
    '    result.Append("                 <span>BLOG & NEWS</span>")
    '    result.Append("                     <h2 class=""margin-auto"">News & <b>Updates</b></h2>")
    '    result.Append("             </div>")
    '    result.Append("         <div class=""row pt-45"">")

    '          If dtBlog.Rows.Count > 0 Then
    '        If Not IsDBNull(dtBlog.Rows(0).Item("publishDate")) Then
    '            strPublishDate = Format(dtBlog.Rows(0).Item("publishDate"), "dd/MM/yyyy")
    '        End If

    '        For i As Integer = 0 To dtBlog.Rows.Count - 1
    '            dtImage = getImageRefByContent(dtBlog.Rows(i).Item("contentRef").ToString)
    '             If dtImage.Rows.Count > 0 Then
    '                Dim img As String = _rootPath + "content.images/content/" + _domainRef + "/" + dtImage.Rows(0).Item("imgRef").ToString + "/0/0/0" + ".jpg"" alt=""" + dtBlog.Rows(i).Item("title")
    '                result.Append("             <div class=""col-lg-4 col-md-6"">")
    '                result.Append("                 <div class=""blog-card"">")
    '                result.Append("                     <a href=""" + _rootPath + convertStrToParam(getTagNameByTagRef(_tagRefBlog).ToString) + "/" + convertStrToParam(getTitleByContentRef(dtBlog.Rows(i).Item("contentRef"))).ToString + """ aria-label=""Blog""class=""hover-fade"">")
    '                result.Append("                         <img style=""width:100%"" src="""+img+""" alt=""image "+dtBlog.Rows(i).Item("title")+""" />")
    '                'result.Append("                         <img class=""lazyload"" src=""" + _rootPath + "support/img/placeholder.jpg"" style=""height:350px;width:560px;"" alt=""" + dtBlog.Rows(i).Item("title") + """>")
    '                result.Append("                     </a>")
    '                result.Append("                 <div class=""content"">")
    '                result.Append("                     <span>"+dtBlog.Rows(i).Item("publishdate")+"</span>")
    '                result.Append("                         <a href=""#"">")
    '                result.Append("                             <h3 class=""title-blog"">"+dtBlog.Rows(i).Item("title")+"</h3>")
    '                result.Append("                         </a>")
    '                result.Append("                 </div>")
    '                result.Append("                 </div>")
    '                result.Append("             </div>")

    '             End If
    '        Next
    '    'result.Append("             <div class=""col-lg-4 col-md-6"">")
    '    'result.Append("                 <div class=""blog-card"">")
    '    'result.Append("                     <a href=""blog-details.html"">")
    '    'result.Append("                         <img width=""350"" height=""300"" src="""+_rootPath +"/Support/img/News 2.jpg"" alt=""Blog Images"">")
    '    'result.Append("                     </a>")
    '    'result.Append("                         <div class=""content"">")
    '    'result.Append("                             <span>August 24, 2023</span>")
    '    'result.Append("                                 <a href=""blog-details.html"">")
    '    'result.Append("                                     <h3>Ketua Real Estate Indonesia Berbagi Kiat Sukses ke Wisudawan UMM</h3>")
    '    'result.Append("                                 </a>")
    '    'result.Append("                         </div>")
    '    'result.Append("                 </div>")
    '    'result.Append("             </div>")
    '    'result.Append("             <div class=""col-lg-4 col-md-6 offset-md-3 offset-lg-0"">")
    '    'result.Append("                <div class=""blog-card"">")
    '    'result.Append("                     <a href=""blog-details.html"">")
    '    'result.Append("                         <img width=""350"" height=""300"" src="""+_rootPath +"/Support/img/News 3.jpg"" alt=""Blog Images"">")
    '    'result.Append("                     </a>")
    '    'result.Append("                <div class=""content"">")
    '    'result.Append("                   <span>November 20, 2023</span>")
    '    'result.Append("                       <a href=""blog-details.html"">")
    '    'result.Append("                            <h3>Menko Airlangga: Indonesia jadi tujuan investasi properti terbaik</h3>")
    '    'result.Append("                       </a>")
    '    'result.Append("                </div>")
    '    'result.Append("             </div>")
    '    'result.Append("            </div>")
    '    result.Append("           </div>")
    '    result.Append("         </div>")
    '    result.Append("        </div>")
    '    End If

    '    ltrNewsUpdates.Text = result.ToString
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim q_ref As String = Request.QueryString("r")
        Dim q_tagRef As String = Request.QueryString("t")

        Dim result As New StringBuilder
        Dim domainRef As String = ""
        Dim tagRef As String = ""
        'Dim tagRefNapro As String= ""
        Dim contentRef As String = ""
        Dim pageNo As Integer = 1

        If IsPostBack Then

        Else

             If Not Request.Params("x") Is Nothing Then
                Dim param As Hashtable = GetDecParam(Request.Params("x"))
                noted = param("note")
            End If

            bindHome()
            bindKeunggulan()
            bindAksesibilitas()
            bindTipe()
            bindKeunggulan2()
            bindTestimoni()
            'bindBlog()


        End If

        Dim title As New HtmlMeta()
        title.Name = "title"
        title.Content = _metaTitle
        Page.Header.Controls.Add(title)

        Dim author As New HtmlMeta()
        author.Name = "author"
        author.Content = _metaAuthor
        Page.Header.Controls.Add(author)

        Dim keywords As New HtmlMeta()
        keywords.Name = "keywords"
        keywords.Content = _metaKeyword
        Page.Header.Controls.Add(keywords)

        Dim description As New HtmlMeta()
        description.Name = "description"
        description.Content = _metaDescription
        Page.Header.Controls.Add(description)


    End Sub

End Class
