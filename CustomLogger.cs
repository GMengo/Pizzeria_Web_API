using System.Diagnostics;

namespace pizzeria_web_api
{

    public interface ICustomLogger
    {
        public void WriteLog(string message);
    }


    public class CustomConsoleLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            Console.WriteLine($"LOG {DateTime.Now.ToString("G")} {message}");
        }
    }

    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("./log.txt",$"LOG {DateTime.Now.ToString("G")} {message}\n");
        }
    }
}
