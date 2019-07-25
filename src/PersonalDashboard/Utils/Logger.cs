using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Logger
    {
        public enum LogLevel
        {
            Log_Critical,
            Log_Error,
            Log_Warning,
            Log_Info,
            Log_Debug
        };

        // TODO: Enable thread sync
        // TODO: Write to a log aggregator like FluentD
        public static void Log(string log, LogLevel logLevel = LogLevel.Log_Info)
        {
            Console.WriteLine(string.Format(DateTime.Now.ToString(), @": {0}", log, logLevel));
        }
    }

}
