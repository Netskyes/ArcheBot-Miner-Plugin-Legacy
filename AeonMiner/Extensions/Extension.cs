using System.Threading;

namespace AeonMiner
{
    public static class Extension
    {
        public static bool IsAlive(this CancellationToken token)
        {
            return !token.IsCancellationRequested;
        }
    }
}
