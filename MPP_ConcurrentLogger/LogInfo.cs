﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_ConcurrentLogger
{    
    public class LogInfo : LogInfoBase, ILogInfo
    {
        private LogLevel level;
        private string message;
        private DateTime time;

        public LogInfo(LogLevel level, string message)
        {
            this.level = level;
            this.message = message;
            time = DateTime.Now;
        }

        public LogLevel Level
        {
            get
            {
                return level;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
        }

        public DateTime Time
        {
            get
            {
                return time;
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1} {2}.", time, level, message);
        }
    }
}
