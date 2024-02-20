using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.API.Time
{
    internal class LogTimer : ILogTimer
    {
        private DateTime _startingTime = DateTime.Now;

        public long GetTimeElapsed
        {
            get
            {
                return (long)(DateTime.Now - _startingTime).TotalMilliseconds;
            }
        }

        public void ResetTimer()
        {
            _startingTime = DateTime.Now;
        }
    }
}
