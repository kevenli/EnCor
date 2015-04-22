using System.IO;
using EnCor.ObjectBuilder;

namespace EnCor.Logging.Appenders
{
    [AssembleConfig(typeof(FileLogAppenderConfig))]
    public class FileLogAppender : LogAppender
    {
        private readonly string _filePath;

        public FileLogAppender(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            _filePath = filePath;
        }

        public override void Log(LogEntry logEntry)
        {
            using (var fs = new FileStream(_filePath, FileMode.Append, FileAccess.Write))
            {
                var sw = new StreamWriter(fs);

                sw.WriteLine(string.Format("date:{0} \r\nthread:{1} \r\nloglevel:{2} \r\nlogger:{3} \r\nmessage:{4}",
                logEntry.TimeStamp,
                logEntry.ThreadName,
                logEntry.Level,
                logEntry.LoggerName,
                logEntry.Message));
                if (logEntry.Exception != null)
                {
                    sw.WriteLine(logEntry.Exception.ToString());
                }

                sw.Flush();
            }
        }
    }
}
