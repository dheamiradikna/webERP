<%@ Control Language="VB" AutoEventWireup="false" CodeFile="parentFooter.ascx.vb" Inherits="wc_parentFooter" %>
    <style>
      .Phone {
          width: 110px;
          border-style: none;
          position: fixed;
          bottom: 100px;
          right: 30px;
          cursor: pointer;
          background-size:cover;
          background-repeat:no-repeat;
          z-index:999;
          transform: translate3d(0, 0, 0) scale(1);

        }

        .Phone::before,
        .Phone::after {
          position: absolute;
          content: "";
        }

        .Phone::before {
          top: 0;
          left: 0;
          width: 1em;
          height: 1em;
          background-color: rgba(#fff, 0.1);
          border-radius: 100%;
          opacity: 1;
          transform: translate3d(0, 0, 0) scale(0);
        }

        .Phone::after {
          top: 0.25em;
          left: 0.25em;
          width: 0.5em;
          height: 0.5em;
          background-position: 50% 50%;
          background-repeat: no-repeat;
          background-size: cover;
          transform: translate3d(0, 0, 0);
        }

        .Phone.is-animating {
          animation: shake 5000ms infinite;
        }
           @keyframes shake {
              0% { transform: translate(1px, 1px) rotate(0deg); }
              2% { transform: translate(-1px, -2px) rotate(-1deg); }
              4% { transform: translate(-3px, 0px) rotate(1deg); }
              6% { transform: translate(3px, 2px) rotate(0deg); }
              8% { transform: translate(1px, -1px) rotate(1deg); }
              10% { transform: translate(-1px, 2px) rotate(-1deg); }
              12% { transform: translate(-3px, 1px) rotate(0deg); }
              14% { transform: translate(3px, 1px) rotate(-1deg); }
              16% { transform: translate(-1px, -1px) rotate(1deg); }
              18% { transform: translate(1px, 2px) rotate(0deg); }
              19% { transform: translate(1px, -2px) rotate(-1deg); }
              20% { transform: translate(0, 0); }

}
</style>

<div class="clear"></div>
<section>
     <div Class="bg-buildingsFooter">
          </div>
</section>
<footer>
<asp:Literal ID="ltrFooter" runat="server"></asp:Literal>
 <asp:Literal ID="ltrWhatsapp" runat="server"></asp:Literal>   

</footer>
