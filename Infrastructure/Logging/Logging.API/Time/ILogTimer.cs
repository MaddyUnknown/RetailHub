using System.Diagnostics;

namespace Logging.API.Time
{
    public interface ILogTimer
    {
        public void ResetTimer();

        public long GetTimeElapsed { get; }
    }
}