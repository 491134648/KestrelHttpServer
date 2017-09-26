// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets
{
    public sealed class SocketTransportFactory : ITransportFactory
    {
        public SocketTransportFactory(IOptions<SocketTransportOptions> options)
        {
        }

        public ITransport Create(IEndPointInformation endPointInformation, IConnectionHandler handler)
        {
            if (endPointInformation == null)
            {
                throw new ArgumentNullException(nameof(endPointInformation));
            }

            if (endPointInformation.Type != ListenType.IPEndPoint)
            {
                throw new ArgumentException(SocketsStrings.OnlyIPEndPointsSupported, nameof(endPointInformation));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return new SocketTransport(this, endPointInformation, handler);
        }


        // TODO: We never dispose pool
        internal BufferPool BufferPool { get; } = new MemoryPool();
    }
}
