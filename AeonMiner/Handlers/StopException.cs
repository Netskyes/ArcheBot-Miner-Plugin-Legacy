using System;

namespace AeonMiner.Handlers
{
    public class StopException : Exception
    {
        public StopException(string message) : base(message)
        {
        }
    }
}
