﻿using System;
using NLog;

namespace FrameWork.Logger
{
    public class CustomLogger
    {
        private readonly NLog.Logger _logger;

        public CustomLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void ErrorException(string message, Exception exception)
        {
            LogEventInfo logEvent = new LogEventInfo(LogLevel.Error, _logger.Name, message)
            {
                Exception = exception
            };
            _logger.Log(typeof(CustomLogger), logEvent);
        }

        public void Error(string message)
        {
            LogEventInfo logEvent = new LogEventInfo(LogLevel.Error, _logger.Name, message)
            {
                Exception = new Exception(message)
            };
            _logger.Log(typeof(CustomLogger), logEvent);
        }

    }


}