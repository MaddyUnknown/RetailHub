using Logging.API.Correlation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.API.Time
{
    public class LogTimerAccessor
    {
        private static readonly AsyncLocal<LogTimerHolder> _timerHolder = new AsyncLocal<LogTimerHolder>();

        public ILogTimer? LogTimer
        {
            get
            {
                return _timerHolder.Value?.Timer;
            }

            set
            {
                if (_timerHolder.Value != null)
                {
                    _timerHolder.Value.Timer = null;
                }

                if (value != null)
                {
                    _timerHolder.Value = new LogTimerHolder { Timer = value };
                }
            }
        }


        private class LogTimerHolder
        {
            public ILogTimer? Timer;
        }
    }
}
