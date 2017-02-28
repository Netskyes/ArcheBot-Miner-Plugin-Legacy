using ArcheBot.Bot.Classes;

namespace AeonMiner.Helpers
{
    using Data;

    public class MountHelper
    {
        private Host Host { get; set; }
        private Mount slave;

        public MountHelper(string name, Host host)
        {
            Host = host;
            slave = Mounts.GetByName(name);
        }

        public uint Id
        {
            get
            {
                return slave.Id;
            }
        }

        public string Name
        {
            get
            {
                return slave.Name;
            }
        }


        public Item GetScroll() => (Host.getInvItem(slave.Id));
        public Creature GetMount() => Host.getMount();

        public bool ScrollExists() => (Host.getInvItem(slave.Id) != null);
        public bool Spawn() => (ScrollExists()) ? Host.UseItem(slave.Id) : false;
        public bool Despawn() => Host.DespawnMount();
        public bool IsSpawned() => (GetMount() != null);
        public bool MountUp() => Host.SitToMount();
        public bool Dismount() => Host.StandFromMount();
        public bool IsMeSitting() => (IsSpawned()) ? (GetMount() == Host.me.sitMountObj) : false;
        public bool UseSprint() => IsMeSitting() && Host.UseSkill(17092);
    }
}
