using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace pizzeria_web_api
{

    public interface ICustomLogger
    {
        public void WriteLog(string message);
        public void WriteStartingLogWithHttpInfo(HttpContext httpContext, string? message = null);
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null);

    }


    public class CustomConsoleLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} {message}");
        }
        public void WriteStartingLogWithHttpInfo(HttpContext httpContext, string? message = null)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} [{method} {path}] - {message}");
        }

        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null)
        {
            string method = httpContext.Request.Method;
            int statusCode = httpContext.Response.StatusCode;
            string path = httpContext.Request.Path;
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} [{method} {statusCode} {path}] - {message}");
        }

    }

    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("./log.txt",$"LOG {DateTime.Now.ToString("G")} {message}\n");
        }
        public void WriteStartingLogWithHttpInfo(HttpContext httpContext, string? message = null)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} [{method} {path}] - {message}\n");
        }
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null)
        {
            string method = httpContext.Request.Method;
            int statusCode = httpContext.Response.StatusCode;
            string path = httpContext.Request.Path;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} [{method} {statusCode} {path}] - {message}\n");
        }
    }
}
