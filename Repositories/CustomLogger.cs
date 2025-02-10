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
        public void WriteRequest(HttpContext httpContext);
        public void WriteResponse(HttpContext httpContext);
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
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} Request arrivata: [method: {method}, path: {path}] - {message}");
        }
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} Response in uscita: [method: {method}, status code: {(int)statusCode} {statusCode}, path: {path}] - {message}");
        }

        public void WriteRequest(HttpContext httpContext)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} Request arrivata: [method: {method}, path: {path}]");
        }
        public void WriteResponse(HttpContext httpContext)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            int statusCode = httpContext.Response.StatusCode;
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} Response in uscita: [status code: {statusCode} {(HttpStatusCode)statusCode}, method: {method}, path: {path}]");
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
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} Request arrivata: [method: {method}, path: {path}] - {message}\n");
        }
        public void WriteResultLogWithHttpInfo(HttpContext httpContext, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} Response in uscita: [method: {method}, status code: {(int)statusCode} {statusCode}, path: {path}] - {message}\n");
        }
        public void WriteRequest(HttpContext httpContext)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} Request arrivata: [method: {method}, path: {path}]");
        }
        public void WriteResponse(HttpContext httpContext)
        {
            string method = httpContext.Request.Method;
            string path = httpContext.Request.Path;
            int statusCode = httpContext.Response.StatusCode;
            File.AppendAllText("./log.txt", $"LOG {DateTime.Now.ToString("G")} Response in uscita: [status code: {statusCode} {(HttpStatusCode)statusCode}, method: {method}, path: {path}]");
        }

    }
}
