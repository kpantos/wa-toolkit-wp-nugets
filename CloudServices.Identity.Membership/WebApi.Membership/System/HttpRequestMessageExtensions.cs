﻿// ----------------------------------------------------------------------------------
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

// <copyright file="HttpRequestMessageExtensions.cs" company="open-source" >
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
    using System.Security.Principal;
    using System.Threading;
    using System.Web;

    /// <summary>
    /// HttpRequestMessage extension methods.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Returns the IPrincipal instance authenticated and stored by the MembershipAuthenticationHandler
        /// on the ongoing request.
        /// </summary>
        /// <param name="message">this (request)</param>
        /// <returns>IPrincipal instance authenticated and stored by the MembershipAuthenticationHandler</returns>
        public static IPrincipal User(this HttpRequestMessage message)
        {
            if (message != null)
            {
                return message.Properties[MembershipAuthenticationHandler.MessagePrincipalKey] as IPrincipal;
            }

            if (Thread.CurrentPrincipal != null)
            {
                return Thread.CurrentPrincipal;
            }

            if (HttpContext.Current != null)
            {
                return HttpContext.Current.User;
            }

            return null;
        }
    }
}