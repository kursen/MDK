using System;
using System.IO;
using System.Text;

namespace SKA.Utils
{
    public class FileLogger : ILogger
    {
        public void Trace(string message)
        {
            LogError("Trace", message, null, null);
        }

        public void Debug(string message)
        {
            LogError("Debug", message, null, null);
        }

        public void Info(string message)
        {
            LogError("Info", message, null, null);
        }

        public void Warn(string message)
        {
            LogError("Warn", message, null, null);
        }

        public void Error(string message)
        {
            LogError("Error", message, null, null);
        }

        public void Error(string message, Exception ex)
        {
            string technicalMessage = string.Empty;

            if (ex.InnerException != null)
                technicalMessage = ex.InnerException.ToString();

            LogError("Error", message, technicalMessage, ex.StackTrace);
        }

        public void Fatal(string message)
        {
            LogError("Fatal", message, null, null);
        }

        public void Fatal(string message, Exception ex)
        {
            string technicalMessage = string.Empty;

            if (ex.InnerException != null)
                technicalMessage = ex.InnerException.ToString();

            LogError("Fatal", message, technicalMessage, ex.StackTrace);
        }

        private static bool LogError(string loggerType, string errorMsg, string technicalMsg, string stackTrace)
        {
            try
            {
                string folderPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath + "\\Log";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string logFilePath = folderPath.Replace("\\", "/") + "/ErrorLog" + DateTime.Today.ToString("yyyyMMdd") + ".log";
                StringBuilder msg = new StringBuilder();

                msg.AppendLine("");
                msg.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffffK"));
                msg.AppendLine("  Type: " + loggerType);
                msg.AppendLine("  Message: " + errorMsg);

                if (!string.IsNullOrEmpty(stackTrace))
                    msg.AppendLine("  Stack trace: " + stackTrace);

                if (!string.IsNullOrEmpty(technicalMsg))
                    msg.AppendLine("  Inner exception: " + technicalMsg);

                msg.AppendLine("======================================");

                File.AppendAllText(logFilePath, msg.ToString());
                return true;
            }
            catch
            { }
            return false;
        }
    }
}