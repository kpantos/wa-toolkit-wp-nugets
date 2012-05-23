// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace System.Net.Http
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Security;

    public class AuthenticateAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var input = actionContext.Request;
            var principal = PrincipalHelper.GetPrincipalFromHttpRequest(input);
            if (principal == null)
            {
                throw UnauthorizedException();
            }

            var user = Membership.GetUser(principal.Identity.Name);
            if (user == null)
            {
                throw UnauthorizedException();
            }

            PrincipalHelper.SetPrincipal(input, principal);

            base.OnAuthorization(actionContext);
        }

        private static Exception UnauthorizedException()
        {
            // HACK: Prevent ASP.NET Forms Authentication to redirect the user to the login page.
            // This thread-safe approach adds a header with the suppression to be read on the 
            // OnEndRequest event of the pipelien. In order to fully support the supression you should have the ASP.NET Module
            // that does this (SuppressFormsAuthenticationRedirectModule).  
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            response.Headers.Add(SuppressFormsAuthenticationRedirectModule.SuppressFormsHeaderName, "true");

            return new HttpResponseException(response);
        }
    }
}