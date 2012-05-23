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
    using System.Security.Principal;
    using System.Web.Security;

    public static class PrincipalHelper
    {
        /// <summary>
        /// Checks, extracs, and parses the SimpleWebToken from the Authentication Header.
        /// </summary>
        /// <param name="request">Ongoing request.</param>
        /// <returns>A parsed SimpleWebToken.</returns>
        internal static FormsAuthenticationTicket ExtractTicketFromHeader(HttpRequestMessage request)
        {
            var authorizationHeader = request.Headers.Authorization;

            if (authorizationHeader != null && authorizationHeader.Scheme.Equals("Membership", StringComparison.OrdinalIgnoreCase))
            {
                var ticket = FormsAuthentication.Decrypt(authorizationHeader.Parameter);

                if (ticket != null && !ticket.Expired)
                {
                    return ticket;
                }
            }

            return null;
        }

        internal static IPrincipal GetPrincipalFromHttpRequest(HttpRequestMessage request)
        {
            var ticket = PrincipalHelper.ExtractTicketFromHeader(request);
            if (ticket == null)
            {
                return null;
            }

            return new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
        }

        internal static void SetPrincipal(HttpRequestMessage request, IPrincipal principal)
        {
            // We authenticate the ongoing thread but also we stuff the principal on the message
            // after a brief chat with Glenn Block we understood that the only way to authenticate
            // a request and continue using it is to stuff it's authenticated user throught on 
            // the message properties.
            request.Properties[MembershipAuthenticationHandler.MessagePrincipalKey] = principal;
        }
    }
}
