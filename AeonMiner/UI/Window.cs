using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AeonMiner.UI
{
    using Data;
    using Utility;
    using Preferences;
    using Navigation;

    public partial class Window : Form
    {
        private Host Host
        {
            get { return Host.Instance; }
        }

        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            SetWindowDetails();

            // Populates
            GetMounts();
            GetMiningZones();

            // Load preferences
            LoadTasks();
            LoadSettings();

            // Events
            lbox_MiningTasks.SelectedIndexChanged += MiningTasks_SelectedIndexChanged;

            // Auto Start
            if (chkbox_AutoStart.Checked) Host.BaseModule.Start();
        }


        private void SetWindowDetails()
        {
            Text = string.Format("AeonMiner - {0} @ {1}", Host.me.name, Host.serverName());
        }



        #region Helpers

        private void PopFromList(ListBox lbox)
        {
            Utils.InvokeOn(this, () =>
            {
                var selected = lbox.SelectedItem;

                if (selected != null) lbox.Items.Remove(selected);
            });
        }

        private void MoveListItem(int direction, ListBox box)
        {
            if (box.SelectedItem != null && box.SelectedIndex >= 0)
            {
                object item = box.SelectedItem;
                int index = box.SelectedIndex, nIndex = (index + direction);

                if (nIndex >= 0 && nIndex < box.Items.Count)
                {
                    box.Items.RemoveAt(index);
                    box.Items.Insert(nIndex, item);

                    box.SetSelected(nIndex, true);
                }
            }
        }

        public void UpdateButtonState(string text, bool state = true)
        {
            Utils.InvokeOn(btn_Start, () =>
            {
                btn_Start.Text = text;
                btn_Start.Enabled = state;
            });
        }

        #endregion

        #region Props & Fields

        public bool ButtonSwitch
        {
            get { return btnSwitch; } set { btnSwitch = value; }
        }

        private bool btnSwitch = false;

        private Dictionary<string, MineTask> miningTasks = new Dictionary<string, MineTask>();

        #endregion

        #region Settings

        public Settings SaveSettings()
        {
            var settings = GetSettings();

            if (settings != null && Serializer.Save(settings, $"{Paths.Settings}{Host.me.name}@{Host.serverName()}.xml"))
            {
                return settings;
            }

            return null;
        }

        private void LoadSettings()
        {
            Settings settings = Serializer.Load(new Settings(), $"{Paths.Settings}{Host.me.name}@{Host.serverName()}.xml");

            if (settings == null)
                return;


            Utils.InvokeOn(this, () =>
            {
                chkbox_AutoStart.Checked = settings.AutoStart;
                chkbox_RunPlugin.Checked = settings.RunPlugin;

                
                if (settings.TaskName != string.Empty)
                {
                    int index = lbox_MiningTasks.Items.IndexOf(settings.TaskName);

                    if (index != -1)
                    {
                        lbox_MiningTasks.SelectedIndex = index;
                    }
                }
                
                if (settings.TravelMount != string.Empty)
                {
                    int index = cmbox_MountsList.Items.IndexOf(settings.TravelMount);

                    if (index != -1)
                    {
                        cmbox_MountsList.SelectedIndex = index;
                    }
                }

                var radio = container_WhenDone.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Name == settings.FinalAction);

                if (radio != null)
                {
                    radio.Checked = true;
                }

                txtbox_PluginRunName.Text = settings.PluginRunName;
            });
        }

        public Settings GetSettings()
        {
            Settings settings = new Settings();

            Utils.InvokeOn(this, () =>
            {
                settings.AutoStart = chkbox_AutoStart.Checked;
                settings.RunPlugin = chkbox_RunPlugin.Checked;
                settings.TaskName = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                settings.TravelMount = cmbox_MountsList.GetItemText(cmbox_MountsList.SelectedItem);
                settings.FinalAction = container_WhenDone.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name;
                settings.PluginRunName = txtbox_PluginRunName.Text;
            });

            return settings;
        }


        private void LoadTasks()
        {
            MiningTasks mining = Serializer.Load(new MiningTasks(), $"{Paths.Settings}Tasks.xml");

            if (mining == null || mining.Tasks.Count < 1)
                return;


            foreach (var task in mining.Tasks)
            {
                // Add to dictionary
                miningTasks.Add(task.Name, task);

                Utils.InvokeOn(this, () => lbox_MiningTasks.Items.Add(task.Name));
            }
        }

        private void LoadTask(string name)
        {
            if (!miningTasks.ContainsKey(name))
                return;


            MineTask mineTask = null;

            try
            {
                mineTask = miningTasks[name];
            }
            catch
            {
                return;
            }


            Utils.InvokeOn(this, () =>
            {
                if (mineTask.MiningZone != string.Empty)
                {
                    int index = cmbox_ZonesList.Items.IndexOf(mineTask.MiningZone);

                    if (index != -1)
                    {
                        cmbox_ZonesList.SelectedIndex = index;
                    }
                }
            });
        }

        public void SaveTasks()
        {
            var mining = new MiningTasks();
                mining.Tasks = miningTasks.Select(i => i.Value).ToList();

            Serializer.Save(mining, $"{Paths.Settings}Tasks.xml");
        }

        public MineTask GetTask()
        {
            var task = new MineTask();

            Utils.InvokeOn(this, () =>
            {
                task.Name = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                task.MiningZone = cmbox_ZonesList.GetItemText(cmbox_ZonesList.SelectedItem);
            });

            return task;
        }

        private void SaveTask()
        {
            var task = GetTask();

            Utils.InvokeOn(this, () =>
            {
                var item = lbox_MiningTasks.SelectedItem;

                if (item == null)
                    return;


                string name = item.ToString();

                if (miningTasks.ContainsKey(name))
                {
                    try
                    {
                        miningTasks[name] = task;
                    }
                    catch
                    {
                        return;
                    }
                }
            });

            SaveTasks();
        }

        #endregion

        #region Data Manipulation

        private void GetMounts()
        {
            var mounts = Mounts.GetAll().Where(i => Host.getInvItem(i.Id) != null).OrderBy(i => i.Name);

            if (mounts.Count() < 1)
                return;


            Utils.InvokeOn(this, () =>
            {
                foreach (var m in mounts)
                {
                    cmbox_MountsList.Items.Add(m.Name);
                }

                cmbox_MountsList.SelectedIndex = 0;
            });
        }

        private void GetMiningZones()
        {
            var zones = Maps.GetAll().Select(m => m.Name).OrderBy(m => m);

            if (zones.Count() < 1)
                return;


            Utils.InvokeOn(this, () =>
            {
                cmbox_ZonesList.Items.AddRange(zones.ToArray());
                cmbox_ZonesList.SelectedIndex = 0;
            });
        }

        #endregion

        #region Events Handlers

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (!ButtonSwitch)
            {
                Host.BaseModule.Start();
            }
            else
            {
                Host.BaseModule.Stop();
            }
        }

        private void btn_AddMiningTask_Click(object sender, EventArgs e)
        {
            string name = string.Empty;


            Utils.InvokeOn(this, () => name = txtbox_TaskName.Text);

            if (name.Trim().Length < 1)
                return;


            if (miningTasks.ContainsKey(name))
            {
                MessageBox.Show("Task with that name already exists!");

                return;
            }

            // Add task
            miningTasks.Add(name, new MineTask() { Name = name });

            SaveTasks();


            Utils.InvokeOn(this, () =>
            {
                if (!lbox_MiningTasks.Items.Contains(name)) { lbox_MiningTasks.Items.Add(name); }

                // Empty textbox
                txtbox_TaskName.Clear();
            });
        }

        #endregion

        #region Events

        private void btn_MoveTaskUp_Click(object sender, EventArgs e)
        {
            MoveListItem(-1, lbox_MiningTasks);
        }

        private void btn_MoveTaskDown_Click(object sender, EventArgs e)
        {
            MoveListItem(1, lbox_MiningTasks);
        }

        private void lbox_MiningTasks_DoubleClick(object sender, EventArgs e)
        {
            Utils.InvokeOn(this, () =>
            {
                var item = lbox_MiningTasks.SelectedItem;

                if (item == null)
                    return;


                string name = item.ToString();

                // Remove key
                if (miningTasks.ContainsKey(name))
                {
                    miningTasks.Remove(name);
                }
                
                // Remove item
                lbox_MiningTasks.Items.Remove(item);
            });
        }

        private void MiningTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.InvokeOn(this, () =>
            {
                var item = lbox_MiningTasks.SelectedItem;

                if (item != null)
                {
                    LoadTask(item.ToString());
                }
            });
        }

        #endregion 
    }
}
