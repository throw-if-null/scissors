﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Scissors
{
    internal static class Extensions
    {
        private static readonly Dictionary<HttpStatusCode, Func<HttpRequestInterceptorOptions, HttpResponseMessage>> _handlers = new Dictionary<HttpStatusCode, Func<HttpRequestInterceptorOptions, HttpResponseMessage>>
        {
            [HttpStatusCode.BadRequest] = x => x.CreateMessage(HttpStatusCode.BadRequest),
            [HttpStatusCode.Unauthorized] = x => x.CreateMessage(HttpStatusCode.Unauthorized),
            [HttpStatusCode.Forbidden] = x => x.CreateMessage(HttpStatusCode.Forbidden),
            [HttpStatusCode.NotFound] = x => x.CreateMessage(HttpStatusCode.NotFound),
            [HttpStatusCode.RequestTimeout] = x => x.CreateMessage(HttpStatusCode.RequestTimeout),

            [HttpStatusCode.InternalServerError] = x => x.CreateMessage(HttpStatusCode.InternalServerError),
            [HttpStatusCode.BadGateway] = x => x.CreateMessage(HttpStatusCode.BadGateway),
            [HttpStatusCode.GatewayTimeout] = x => x.CreateMessage(HttpStatusCode.GatewayTimeout),

            [HttpStatusCode.OK] = x => x.CreateMessage(HttpStatusCode.OK),
            [HttpStatusCode.Created] = x => x.CreateMessage(HttpStatusCode.Created)
        };

        internal static HttpResponseMessage TryCreateResponse(this HttpRequestInterceptorOptions options)
        {
            if (_handlers.TryGetValue((HttpStatusCode)options.ResponseStatusCode, out var handler))
                return handler(options);

           throw new NotSupportedException($"Response status: {options.ResponseStatusCode} is not supported.");
        }

        private static HttpResponseMessage CreateMessage(this HttpRequestInterceptorOptions options, HttpStatusCode statusCode)
        {
            var message = new HttpResponseMessage(statusCode);

            if (string.IsNullOrWhiteSpace(options.ResponseJsonContent))
                return message.AddResponseHeaders(options.Headers);

            message.Content = new StringContent(options.ResponseJsonContent, Encoding.UTF8, "application/json");

            return message.AddResponseHeaders(options.Headers);
        }

        private static HttpResponseMessage AddResponseHeaders(this HttpResponseMessage message, Collection<HttpRequestInterceptorOptions.HttpResponseHeader> headers)
        {
            foreach (HttpRequestInterceptorOptions.HttpResponseHeader header in headers)
            {
                message.Headers.Add(header.Name, header.Value);
            }

            return message;
        }
    }
}