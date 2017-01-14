namespace AeonMiner.Data
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint[] Phases { get; set; }

        public Node(int id, string name, uint[] phases)
        {
            Id = id;
            Name = name;
            Phases = phases;
        }
    }
}
