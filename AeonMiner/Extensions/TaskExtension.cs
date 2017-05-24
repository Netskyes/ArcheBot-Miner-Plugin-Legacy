using System.Threading;

namespace AeonMiner
{
    public static class TaskExtension
    {
        public static bool IsAlive(this CancellationToken token)
        {
            return !token.IsCancellationRequested;
        }
    }
}
