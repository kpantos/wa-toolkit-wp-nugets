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

namespace Microsoft.WindowsAzure.Samples.CloudServices.Storage
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.WindowsAzure.Samples.CloudServices.Storage.Handlers;
    using Microsoft.WindowsAzure.Samples.CloudServices.Storage.Helpers;
    using Microsoft.WindowsAzure.Samples.CloudServices.Storage.Security;

    public class QueuesProxyController : ApiController
    {
        private static readonly AzureQueuesProxyHandler Proxy = new AzureQueuesProxyHandler();

        [AuthorizeQueuesAccess, CLSCompliant(false)]
        public HttpResponseMessage Post([FromUri]string path)
        {
            if (this.Request == null)
                throw Extensions.StorageException(HttpStatusCode.BadRequest, Constants.RequestCannotBeNullErrorMessage, Constants.RequestCannotBeNullErrorMessage);

            this.Request.Properties[StorageProxyHandler.RequestedPathPropertyName] = path ?? string.Empty;
            return Proxy.ProcessRequest(this.Request);
        }

        [AuthorizeQueuesAccess, CLSCompliant(false)]
        public HttpResponseMessage Put([FromUri]string path)
        {
            if (this.Request == null)
                throw Extensions.StorageException(HttpStatusCode.BadRequest, Constants.RequestCannotBeNullErrorMessage, Constants.RequestCannotBeNullErrorMessage);

            this.Request.Properties[StorageProxyHandler.RequestedPathPropertyName] = path ?? string.Empty;
            return Proxy.ProcessRequest(this.Request);
        }

        [AuthorizeQueuesAccess, CLSCompliant(false)]
        public HttpResponseMessage Get(string path)
        {
            if (this.Request == null)
                throw Extensions.StorageException(HttpStatusCode.BadRequest, Constants.RequestCannotBeNullErrorMessage, Constants.RequestCannotBeNullErrorMessage);

            this.Request.Properties[StorageProxyHandler.RequestedPathPropertyName] = path ?? string.Empty;
            return Proxy.ProcessRequest(this.Request);
        }

        [AuthorizeQueuesAccess, CLSCompliant(false)]
        public HttpResponseMessage Delete(string path)
        {
            if (this.Request == null)
                throw Extensions.StorageException(HttpStatusCode.BadRequest, Constants.RequestCannotBeNullErrorMessage, Constants.RequestCannotBeNullErrorMessage);

            this.Request.Properties[StorageProxyHandler.RequestedPathPropertyName] = path ?? string.Empty;
            return Proxy.ProcessRequest(this.Request);
        }

        [AuthorizeQueuesAccess, CLSCompliant(false)]
        [AcceptVerbs("HEAD")]
        public HttpResponseMessage Head(string path)
        {
            if (this.Request == null)
                throw Extensions.StorageException(HttpStatusCode.BadRequest, Constants.RequestCannotBeNullErrorMessage, Constants.RequestCannotBeNullErrorMessage);

            this.Request.Properties[StorageProxyHandler.RequestedPathPropertyName] = path ?? string.Empty;
            return Proxy.ProcessRequest(this.Request);
        }
    }
}