using System.Threading;

namespace AeonMiner
{
    public static class Extensions
    {
        public static bool IsAlive(this CancellationToken token)
        {
            return !token.IsCancellationRequested;
        }
    }
}
