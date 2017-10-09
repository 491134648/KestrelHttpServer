// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Certificates.Generation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;

namespace Microsoft.AspNetCore.Server.Kestrel
{
    internal class DefaultHttpsProvider : IDefaultHttpsProvider
    {
        private static readonly CertificateManager _certificateManager = new CertificateManager();

        public void ConfigureHttps(ListenOptions listenOptions)
        {
            listenOptions.UseHttps(FindDevelopmentCertificate());
        }

        private static X509Certificate2 FindDevelopmentCertificate()
        {
            var certificate = _certificateManager
                .ListCertificates(CertificatePurpose.HTTPS, StoreName.My, StoreLocation.CurrentUser, isValid: true, requireExportable: false)
                .FirstOrDefault();

            if (certificate == null)
            {
                throw new InvalidOperationException("Unable to find ASP.NET Core development certificate.");
            }

            return certificate;
        }
    }
}
