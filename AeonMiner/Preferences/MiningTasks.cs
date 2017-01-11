using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AeonMiner.Preferences
{
    [Serializable]
    public class MiningTasks
    {
        [XmlArrayItem("Task")]
        public List<MineTask> Tasks;

        public MiningTasks()
        {
            Tasks = new List<MineTask>();
        }
    }
}
