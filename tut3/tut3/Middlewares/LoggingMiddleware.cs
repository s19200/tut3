using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.Middlewares
{
    public class LoggingMiddleware
    {

        private readonly RequestDelegate _next;

        public LoggingMiddleware (RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync (HttpContext httpContext)
        {
        
      httpContext.Request.EnableBuffering();
       
           if (httpContext.Request != null)
            {
                string method = httpContext.Request.Method;
                string path = httpContext.Request.Path;
                string body = httpContext.Request.Body.ToString();
                string query = httpContext.Request.QueryString.ToString();

                string text = "Method: " + method + "; Endpoint path: " + path + "; Body: " + body + "; Query string: " + query + "\n" ;

                File.AppendAllText(@"C:\Users\user\Desktop\tut3\tut3\tut3\requestsLog.txt", text);

            }
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin); 
            await _next(httpContext);
        }
    }
}
