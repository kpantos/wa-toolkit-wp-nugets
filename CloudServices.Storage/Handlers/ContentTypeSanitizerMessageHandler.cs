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

namespace Microsoft.WindowsAzure.Samples.CloudServices.Storage.Handlers
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;

    public class ContentTypeSanitizerMessageHandler : ControllerFilteredMessageProcessingHandler
    {
        protected override HttpRequestMessage ProcessRequestHandler(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content.Headers.ContentType == null)
            {
                // Set the default Content-Type when it is not specified
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            }

            return request;
        }
    }
}