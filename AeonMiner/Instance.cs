namespace AeonMiner
{
    using UI;
    using Modules;

    public class Instance
    {
        public Host Host { get; set; }
        public UIContext UIContext { get; set; }
        public BaseModule BaseModule { get; set; }
    }
}
