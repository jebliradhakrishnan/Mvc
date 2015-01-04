﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LoggingWebSite
{
    public class LogEntriesSerializerMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;

        public LogEntriesSerializerMiddleware(RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }

        public Task Invoke(HttpContext context, TestSink sink)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";

            var serializer = JsonSerializer.Create();
            using (var writer = new JsonTextWriter(new StreamWriter(stream: context.Response.Body,
                                                                    encoding: Encoding.UTF8,
                                                                    bufferSize: 1024,
                                                                    leaveOpen: true)))
            {
                serializer.Serialize(writer, sink.LogEntries);
            }

            return Task.FromResult(true);
        }
    }
}