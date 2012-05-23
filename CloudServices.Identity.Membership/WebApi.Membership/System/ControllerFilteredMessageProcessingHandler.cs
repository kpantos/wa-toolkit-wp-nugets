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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web.Http;

    public abstract class ControllerFilteredMessageProcessingHandler : MessageProcessingHandler
    {
        public IList<string> ConfiguredControllers { get; set; }

        protected abstract HttpRequestMessage ProcessRequestHandler(HttpRequestMessage request, CancellationToken cancellationToken);

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var routeData = request.GetRouteData();
            var controllerName = routeData.Values.ContainsKey("controller") ?
                routeData.Values["controller"].ToString() :
                string.Empty;

            if (this.ConfiguredControllers == null ||
                this.ConfiguredControllers.Any(c => c.Equals(controllerName, StringComparison.OrdinalIgnoreCase)))
            {
                return this.ProcessRequestHandler(request, cancellationToken);
            }

            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return response;
        }
    }
}