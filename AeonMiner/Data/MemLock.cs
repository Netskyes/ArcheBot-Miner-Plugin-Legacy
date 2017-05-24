using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeonMiner.Data
{
    class MemoryNode
    {
        public bool Locked { get; set; }
        public int Times { get; set; }
        public int LockNum { get; set; }
    }

    internal class MemLock
    {
        private Dictionary<string, MemoryNode> nodes = new Dictionary<string, MemoryNode>();
        private readonly object memoryLock = new object();

        /// <summary>
        /// When memory is unlocked.
        /// </summary>
        public event MemoryUnlockHandler MemoryUnlocked;
        public delegate void MemoryUnlockHandler(string name);

        public MemLock()
        {

        }


        public void Lock(string name, int ms, int lockAt = 0)
        {
            lock (memoryLock)
            {
                if (!nodes.ContainsKey(name))
                {
                    nodes.Add(name, new MemoryNode());
                }

                if (nodes[name].Locked)
                    return;


                nodes[name].Times++;

                if (nodes[name].Times < lockAt)
                    return;

                // Lock node
                nodes[name].Locked = true;
            }

            Task.Run(() =>
            {
                Utils.Sleep(ms); Unlock(name);
            });
        }

        public bool IsLocked(string name)
        {
            lock (memoryLock)
            {
                return nodes.ContainsKey(name) && nodes[name].Locked;
            }
        }

        private void Unlock(string name)
        {
            lock (memoryLock)
            {
                nodes[name].Locked = false;
                nodes[name].Times = 0;
            }

            OnMemoryUnlock(name);
        }


        public virtual void OnMemoryUnlock(string name)
        {
            MemoryUnlocked?.Invoke(name);
        }
    }
}
