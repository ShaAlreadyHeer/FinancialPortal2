﻿using FinancialPortal.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FinancialPortal.Extentions
{
    public static class InvitationExtentions
    {
        public static async Task EmailInvitation(this Invitation invitation)
        {
            var Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var callbackUrl = Url.Action("RegisterHouseholdInvitation", "Account", new { recipientEmail = invitation.RecipientEmail, 
                code = invitation.Code }, protocol: HttpContext.Current.Request.Url.Scheme);
            var from = $"Portal<{WebConfigurationManager.AppSettings["emailfrom"]}>";

            var emailMessage = new MailMessage(from, invitation.RecipientEmail)
            {
                Subject = $"You have been invited to join the Portal Application!",
                Body = $"Please accept this invitation and register as a new household member " +
                $"<a href=\"{callbackUrl}\">here</a>"
            };
            
            var svc = new PersonalEmail();
            await svc.SendAsync(emailMessage);
        }

    }
    public static class Extentions
    { 
        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            context.GetOwinContext().Authentication.SignOut();
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user,
                isPersistent: false, rememberBrowser: false);
        }
    }
}