using System;
using System.Collections.Generic;
using System.Threading;

namespace EnCor.Logging
{
    public class LogEntry
    {
        private string _message = string.Empty;
        private LogLevel _level = LogLevel.Information;
        private IDictionary<string, object> _extendProperties;
        private string _appDomainName;
        private string _threadName;
        private DateTime _timeStamp = DateTime.MaxValue;

        private bool _threadNameInitialized;
        private bool _appDomainNameInitialized;
        private bool _timeStampInitialized;

        public LogEntry()
        {
        }

        public LogEntry(object message, LogLevel level)
            : this(message, level, null)
        {
        }

        public LogEntry(object message, LogLevel level, Exception exception)
        {
            _message = message.ToString();
            _level = level;
            Exception = exception;
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public LogLevel Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public IDictionary<string, object> ExtendProperties
        {
            get
            {
                return _extendProperties ?? (_extendProperties = new Dictionary<string, object>());
            }
            set
            {
                _extendProperties = value;
            }
        }

        public Exception Exception { get; set; }

        public string LoggerName { get; set; }

        public string AppDomainName
        {
            get
            {
                if (!_appDomainNameInitialized)
                {
                    InitializeAppDomainName();
                }

                return _appDomainName;
            }
            set
            {
                _appDomainName = value;
                _appDomainNameInitialized = true;
            }
        }

        public string ThreadName
        {
            get
            {
                if (!_threadNameInitialized)
                {
                    InitializeThreadName();
                }

                return _threadName;
            }
            set
            {
                _threadName = value;
                _threadNameInitialized = true;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                if (!_timeStampInitialized)
                {
                    InitializeTimeStamp();
                }

                return _timeStamp;
            }
            set
            {
                _timeStamp = value;
                _timeStampInitialized = true;
            }
        }

        private void InitializeTimeStamp()
        {
            TimeStamp = DateTime.UtcNow;
        }

        private void InitializeThreadName()
        {
            ThreadName = Thread.CurrentThread.Name;
        }

        private void InitializeAppDomainName()
        {
            AppDomainName = AppDomain.CurrentDomain.FriendlyName;
        }
    }
}
