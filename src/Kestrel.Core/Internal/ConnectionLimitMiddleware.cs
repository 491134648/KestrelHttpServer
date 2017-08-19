﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Protocols;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace Microsoft.AspNetCore.Server.Kestrel.Core.Internal
{
    public class ConnectionLimitMiddleware
    {
        private readonly ServiceContext _serviceContext;
        private readonly ConnectionDelegate _next;

        public ConnectionLimitMiddleware(ConnectionDelegate next, ServiceContext serviceContext)
        {
            _next = next;
            _serviceContext = serviceContext;
        }

        public Task OnConnectionAsync(ConnectionContext connection)
        {
            if (!_serviceContext.ConnectionManager.NormalConnectionCount.TryLockOne())
            {
                KestrelEventSource.Log.ConnectionRejected(connection.ConnectionId);
                _serviceContext.Log.ConnectionRejected(connection.ConnectionId);
                connection.Transport.Input.Complete();
                connection.Transport.Output.Complete();
                return Task.CompletedTask;
            }

            return _next(connection);
        }
    }

    public static class ConnectionLimitBuilderExtensions
    {
        public static IConnectionBuilder UseConnectionLimit(this IConnectionBuilder builder, ServiceContext serviceContext)
        {
            return builder.Use(next =>
            {
                var middleware = new ConnectionLimitMiddleware(next, serviceContext);
                return middleware.OnConnectionAsync;
            });
        }
    }
}
