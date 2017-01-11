namespace AeonMiner.Data
{
    public class Mount
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }

        public Mount(uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
