using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace pizzeria_web_api.Repositories
{

    public interface ICustomLogger 
    {

        public void WriteLog(string message);
        public void WriteStartingLogWithHttpInfo(HttpContext httpContext, string? message = null);
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK);


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
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} [method: {method}, path: {path}] - {message}");
        }
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} [method: {method}, status code: {(int)statusCode} {statusCode}, path: {path}] - {message}");
        }

    }


    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} {message}\n");
        }
        public void WriteStartingLogWithHttpInfo(HttpContext httpContext, string? message = null)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} [method: {method}, path: {path}] - {message}\n");
        }
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} [method: {method}, status code: {(int)statusCode} {statusCode}, path: {path}] - {message}\n");
        }
    }
}
