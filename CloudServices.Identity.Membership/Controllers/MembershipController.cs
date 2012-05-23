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

namespace Microsoft.WindowsAzure.Samples.CloudServices.Identity.Membership
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Security;

    public class MembershipController : ApiController
    {
        private readonly MembershipProvider membershipProvider;

        public MembershipController()
            : this(Membership.Provider)
        {
        }

        [CLSCompliant(false)]
        public MembershipController(MembershipProvider membershipProvider)
        {
            this.membershipProvider = membershipProvider ?? Membership.Provider;
        }

        [HttpPost]
        public HttpResponseMessage Logon(LogOn logOn)
        {
            if ((logOn == null) || string.IsNullOrWhiteSpace(logOn.UserName) || string.IsNullOrWhiteSpace(logOn.Password))
            {
                throw WebException(Constants.InvalidCredentialsMessage, HttpStatusCode.BadRequest);
            }

            bool isValidUser;
            try
            {
                isValidUser = this.membershipProvider.ValidateUser(logOn.UserName, logOn.Password);
            }
            catch (Exception exception)
            {
                throw WebException(exception.Message, HttpStatusCode.InternalServerError);
            }

            if (!isValidUser)
            {
                throw WebException(HttpStatusCode.Unauthorized);
            }

            var token = this.GenerateMembershipToken(logOn.UserName);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(token) };
        }

        [Authenticate, HttpGet]
        public HttpResponseMessage UserName()
        {
            try
            {
                var username = this.Request.User().Identity.Name;

                if (username != null)
                {
                    var user = this.membershipProvider.GetUser(username, false);
                    if (user != null)
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(user.UserName)
                        };

                        return response;
                    }
                }

                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                throw WebException(exception.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public HttpResponseMessage<User> Users(User user)
        {
            MembershipCreateStatus createStatus;

            if ((user == null) || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                throw WebException(Constants.InvalidUserInformation, HttpStatusCode.BadRequest);
            }

            this.membershipProvider.CreateUser(user.Name, user.Password, user.Email, null, null, true, null, out createStatus);
            switch (createStatus)
            {
                case MembershipCreateStatus.Success:
                    return new HttpResponseMessage<User>(user, HttpStatusCode.OK);
                case MembershipCreateStatus.DuplicateUserName:
                    throw WebException(createStatus.ToString(), HttpStatusCode.Conflict);
                case MembershipCreateStatus.DuplicateEmail:
                    throw WebException(createStatus.ToString(), HttpStatusCode.Conflict);
                default:
                    throw WebException(createStatus.ToString(), HttpStatusCode.BadRequest);
            }
        }

        protected virtual string GenerateMembershipToken(string userName)
        {
            var ticket = new FormsAuthenticationTicket(userName, false, int.MaxValue);
            return FormsAuthentication.Encrypt(ticket);
        }

        private static HttpResponseException WebException(string message, HttpStatusCode code)
        {
            var response = new HttpResponseMessage(code);
            if (!string.IsNullOrEmpty(message))
            {
                response.Content = new StringContent(message);
            }

            var result = new HttpResponseException(response);

            if (code == HttpStatusCode.Unauthorized)
            {
                result.Response.Headers.Add(System.Web.SuppressFormsAuthenticationRedirectModule.SuppressFormsHeaderName, "true");
            }

            return result;
        }

        private static HttpResponseException WebException(HttpStatusCode code)
        {
            return WebException(null, code);
        }
    }
}