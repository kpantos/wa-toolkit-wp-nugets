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
    public class Constants
    {
        public const string CloudStorageAccountNullArgumentErrorMessage = "The Storage Account setting cannot be null.";

        public const string CompMustBeSasArgumentErrorMessage = "Argument comp must be sas for this operation.";

        public const string ConfigureActionArgumentNullErrorMessage = "Parameter configureAction cannot be null.";

        public const string ContainerCannotBeNullErrorMessage = "Container can not be null.";

        public const string ContainerNameNullArgumentErrorMessage = "The containerName cannot be null.";

        public const string HttpClientDisposedErrorString = "The connection with Windows Azure has been closed before the result could be read.";

        public const string InvalidPublicAccessModeArgumentErrorMessage = "Container Access Mode is invalid.";

        public const string PublicAccessNotSpecifiedErrorMessage = "You have specified an ACL operation but not sent the required HTTP header.";

        public const string RequestCannotBeNullErrorMessage = "Request can not be null.";

        public const string RequestNullErrorMessage = "Called service with null HttpRequestMessage.";

        public const string ResponseNullArgumentErrorString = "Response can not be null.";

        public const string WindowsAzureStorageExceptionStringMessage = "There was an error when sending data to Windows Azure Storage.";
    }
}
