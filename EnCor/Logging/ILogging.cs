using System;
using EnCor.ModuleLoader;

namespace EnCor.Logging
{
    public interface ILogging
    {
        void Info(string message);

        void Warn(string message);

        void Warn(string message, Exception exception);

        void Debug(string message);

        void Debug(string message, Exception exception);

        void Error(string message);

        void Error(string message, Exception exception);

    }
}
