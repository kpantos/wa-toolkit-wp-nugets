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

// <copyright file="MembershipAuthenticationHandler.cs" company="open-source" >
//  Copyright binary (c) 2011  by Johnny Halife, Juan Pablo Garcia, Mauro Krikorian, Mariano Converti,
//                                Damian Martinez, Nico Bello, and Ezequiel Morito
//   
//  Redistribution and use in source and binary forms, with or without modification, are permitted.
//
//  The names of its contributors may not be used to endorse or promote products derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>

namespace System.Net.Http
{
    using System.Threading;
    using System.Web;
    using System.Web.Http;
    using System.Web.Security;

    /// <summary>
    /// Authenticates the ongoing request using Membership and Forms Authentication cookie. 
    /// Grabs the forms authentication ticket token from the cookie header and performs the authentication
    /// due to WebApi requirement for ASP.NET we're also suppressing Forms Redirect by leveraging
    /// aspnet.suppressformsredirect Package.
    /// </summary>
    public class MembershipAuthenticationHandler : ControllerFilteredMessageProcessingHandler
    {
        /// <summary>
        /// Key used to stuff the Request with the authenticated principal after a
        /// successful login.
        /// </summary>
        public const string MessagePrincipalKey = "identity.currentprincipal";

        /// <summary>
        /// Performs Membership Authentication of the on going request. Turns Membership authentication into mandatory.
        /// </summary>
        /// <param name="request">Ongoing request</param>
        /// <param name="cancellationToken">Async Cancellation token</param>
        /// <returns>The HTTP request message that was processed</returns>
        protected override HttpRequestMessage ProcessRequestHandler(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var principal = PrincipalHelper.GetPrincipalFromHttpRequest(request);
                if (principal == null)
                {
                    this.Unauthorized();
                }

                var user = Membership.GetUser(principal.Identity.Name);
                if (user == null)
                {
                    this.Unauthorized();
                }

                PrincipalHelper.SetPrincipal(request, principal);
            }
            catch
            {
                this.Unauthorized();
            }

            return request;
        }

        private void Unauthorized()
        {
            // HACK: Prevent ASP.NET Forms Authentication to redirect the user to the login page.
            // This thread-safe approach adds a header with the suppression to be read on the
            // OnEndRequest event of the pipelien. In order to fully support the supression you should have the ASP.NET Module
            // that does this (SuppressFormsAuthenticationRedirectModule).
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            response.Headers.Add(SuppressFormsAuthenticationRedirectModule.SuppressFormsHeaderName, "true");

            throw new HttpResponseException(response);
        }
    }
}