using System;

namespace AeonMiner
{
    public class StopException : Exception
    {
        public StopException(string message) : base(message)
        {
        }
    }
}
