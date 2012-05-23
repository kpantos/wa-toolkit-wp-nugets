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

namespace Microsoft.WindowsAzure.Samples.CloudServices.Notifications.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Notifications;

    public class SqlEndpointRepository : IEndpointRepository
    {
        private readonly string nameOrConnectionString;

        public SqlEndpointRepository(string nameOrConnectionString)
        {
            if (string.IsNullOrWhiteSpace(nameOrConnectionString))
                throw new ArgumentNullException("nameOrConnectionString");

            this.nameOrConnectionString = nameOrConnectionString;
        }

        public IEnumerable<Endpoint> All()
        {
            using (var context = this.CreateContext())
            {
                return context.Endpoints.Select(Endpoint.To<Endpoint>).ToList();
            }
        }

        public IEnumerable<Endpoint> AllThat(Func<Endpoint, bool> filterExpression)
        {
            using (var context = this.CreateContext())
            {
                return context.Endpoints.Where(filterExpression).Select(Endpoint.To<Endpoint>).ToList();
            }
        }

        public Endpoint Find(Func<Endpoint, bool> filterExpression)
        {
            using (var context = this.CreateContext())
            {
                return context.Endpoints.SingleOrDefault(filterExpression);
            }
        }

        public Endpoint Find(string applicationId, string tileId, string clientId)
        {
            return this.Find(e => e.ApplicationId.Equals(applicationId) && e.TileId.Equals(tileId ?? string.Empty) && e.ClientId.Equals(clientId));
        }

        public void InsertOrUpdate(Endpoint endpoint)
        {
            using (var context = this.CreateContext())
            {
                var storedEndpoint = this.Find(endpoint.ApplicationId, endpoint.TileId, endpoint.ClientId);

                if (storedEndpoint == null)
                {
                    context.Endpoints.Add(Endpoint.To<SqlEndpointTableRow>(endpoint));
                }
                else
                {
                    var updatableEndpoint = Endpoint.To<SqlEndpointTableRow>(storedEndpoint);
                    context.Endpoints.Attach(updatableEndpoint);

                    // Update values
                    updatableEndpoint.ChannelUri = endpoint.ChannelUri;
                    updatableEndpoint.ExpirationTime = endpoint.ExpirationTime;
                    updatableEndpoint.UserId = endpoint.UserId;
                }

                context.SaveChanges();
            }
        }

        public void Delete(string applicationId, string tileId, string clientId)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentNullException("applicationId");
            
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException("clientId");

            using (var context = this.CreateContext())
            {
                var storedEndpoint = this.Find(applicationId, tileId, clientId);

                if (storedEndpoint != null)
                {
                    var removableEndpoint = Endpoint.To<SqlEndpointTableRow>(storedEndpoint);
                    context.Endpoints.Attach(removableEndpoint);
                    context.Endpoints.Remove(removableEndpoint);
                    context.SaveChanges();
                }
            }
        }

        protected virtual SqlEndpointContext CreateContext()
        {
            return new SqlEndpointContext(this.nameOrConnectionString);
        }
    }
}