using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArcheBot.Bot.Classes;

namespace AeonMiner.UI
{
    using Data;
    using Utility;
    using Preferences;
    using Navigation;

    public partial class Window : Form
    {
        private Host Host { get; set; }

        public Window(Host host)
        {
            InitializeComponent(); Host = host;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            SetWindowDetails();

            // Gets data
            GetMounts();
            GetMiningZones();
            GetPortals();

            // Loads prefs
            LoadTasks();
            LoadSettings();

            // Register events
            Host.BaseModule.StatsUpdated += StatsUpdated;
            #region Controls Events
            lbox_MiningTasks.SelectedIndexChanged += MiningTasks_SelectedIndexChanged;
            cmbox_ZonesList.SelectedIndexChanged += ZonesList_SelectedIndexChanged;
            cmbox_PortalsList.SelectedIndexChanged += PortalsList_SelectedIndexChanged;
            #endregion

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

        private bool MoveListItem(int direction, ListBox box)
        {
            if (box.SelectedItem == null || box.SelectedIndex < 0)
                return false;


            var item = box.SelectedItem;
            int index = box.SelectedIndex, nIndex = (index + direction);

            if (nIndex < 0 || nIndex >= box.Items.Count)
                return false;


            box.Items.RemoveAt(index);
            box.Items.Insert(nIndex, item);

            box.SetSelected(nIndex, true);

            return true;
        }

        public void UpdateRunTime(string text)
        {
            Utils.InvokeOn(this, () => lbl_RunTime.Text = text);
        }

        public void UpdateButtonState(string text, bool state = true)
        {
            Utils.InvokeOn(btn_Start, () =>
            {
                btn_Start.Text = text;
                btn_Start.Enabled = state;
            });
        }

        public bool ResetStats()
        {
            bool isReset = false;


            Utils.InvokeOn(this, () =>
            {
                isReset = chkbox_ResetStats.Checked;

                if (isReset)
                {
                    chkbox_ResetStats.Checked = false;
                }
            });

            return isReset;
        }

        public void AddToMined(Item item, int count)
        {
            DataGridViewRow row = null;

            try
            {
                Utils.InvokeOn(this, () =>
                {
                    row = dtg_Items.Rows.OfType<DataGridViewRow>().FirstOrDefault(d => (uint)d.Tag == item.id);
                });
            }
            catch
            {
            }

            if (row != null)
            {
                Utils.InvokeOn(this, () =>
                {
                    int amount = 0;

                    try
                    {
                        amount = Convert.ToInt32(row.Cells[1].Value);
                    }
                    catch
                    {
                    }

                    row.Cells[1].Value = (amount + count);
                });

                return;
            }

            
            Utils.InvokeOn(this, () =>
            {
                int index = dtg_Items.Rows.Add(item.name, count);

                try
                {
                    dtg_Items.Rows[index].Tag = item.id;
                }
                catch
                {
                }
            });
        }

        #endregion

        #region Props & Fields

        public bool ButtonSwitch
        {
            get { return btnSwitch; } set { btnSwitch = value; }
        }

        private bool btnSwitch;
        private bool isLoadingTask;
        private Task queryTask;
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
                chkbox_SkipBusyNodes.Checked = settings.SkipBusyNodes;
                chkbox_FightAggroMobs.Checked = settings.FightAggroMobs;
                chkbox_AutoLevelUp.Checked = settings.AutoLevelUp;
                chkbox_BeginDailyQuest.Checked = settings.BeginDailyQuest;
                chkbox_FinishDailyQuest.Checked = settings.FinishDailyQuest;
                chkbox_RemoveSuspect.Checked = settings.RemoveSuspect;
                chkbox_UseDash.Checked = settings.UseDash;
                chkbox_UseFreerunner.Checked = settings.UseFreerunner;
                chkbox_UseTeleportation.Checked = settings.UseTeleportation;
                chkbox_UseQuickstep.Checked = settings.UseQuickstep;
                chkbox_UseCometsBoon.Checked = settings.UseCometsBoon;
                

                if (settings.TaskName != string.Empty)
                {
                    int index = lbox_MiningTasks.Items.IndexOf(settings.TaskName);

                    if (index != -1)
                    {
                        lbox_MiningTasks.SelectedIndex = index;

                        // Initial task loading 
                        string name = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);

                        LoadTask(name);
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
                lbox_CleanItems.Items.AddRange(settings.CleanItems.ToArray());
            });
        }

        public Settings GetSettings()
        {
            Settings settings = new Settings();

            Utils.InvokeOn(this, () =>
            {
                settings.AutoStart = chkbox_AutoStart.Checked;
                settings.RunPlugin = chkbox_RunPlugin.Checked;
                settings.SkipBusyNodes = chkbox_SkipBusyNodes.Checked;
                settings.FightAggroMobs = chkbox_FightAggroMobs.Checked;
                settings.AutoLevelUp = chkbox_AutoLevelUp.Checked;
                settings.BeginDailyQuest = chkbox_BeginDailyQuest.Checked;
                settings.FinishDailyQuest = chkbox_FinishDailyQuest.Checked;
                settings.RemoveSuspect = chkbox_RemoveSuspect.Checked;
                settings.UseDash = chkbox_UseDash.Checked;
                settings.UseFreerunner = chkbox_UseFreerunner.Checked;
                settings.UseTeleportation = chkbox_UseTeleportation.Checked;
                settings.UseQuickstep = chkbox_UseQuickstep.Checked;
                settings.UseCometsBoon = chkbox_UseCometsBoon.Checked;
                settings.TaskName = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                settings.TravelMount = cmbox_MountsList.GetItemText(cmbox_MountsList.SelectedItem);
                settings.FinalAction = container_WhenDone.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name;
                settings.PluginRunName = txtbox_PluginRunName.Text;
                settings.CleanItems = lbox_CleanItems.Items.OfType<string>().ToList();
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

            // Lock events
            isLoadingTask = true;


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

                if (mineTask.NearbyDistrict != string.Empty)
                {
                    int index = cmbox_PortalsList.Items.IndexOf(mineTask.NearbyDistrict);

                    if (index != -1)
                    {
                        cmbox_PortalsList.SelectedIndex = index;
                    }
                }


                // ...
            });

            // Unlock events
            isLoadingTask = false;
        }

        public void SaveTasks()
        {
            List<string> names = new List<string>();

            Utils.InvokeOn(this, () => names = lbox_MiningTasks.Items.OfType<string>().ToList());


            var mining = new MiningTasks();

            mining.Tasks = miningTasks.Select
                (t => t.Value).OrderBy
                (t => names.IndexOf(t.Name)).ToList();

            Serializer.Save(mining, $"{Paths.Settings}Tasks.xml");
        }

        public MineTask GetTask()
        {
            var task = new MineTask();

            Utils.InvokeOn(this, () =>
            {
                task.Name = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                task.MiningZone = cmbox_ZonesList.GetItemText(cmbox_ZonesList.SelectedItem);
                task.NearbyDistrict = cmbox_PortalsList.GetItemText(cmbox_PortalsList.SelectedItem);
            });

            return task;
        }

        private void SaveTask()
        {
            Utils.InvokeOn(this, () =>
            {
                string name = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                
                if (miningTasks.ContainsKey(name))
                {
                    try
                    {
                        miningTasks[name] = GetTask();
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

        private void GetPortals()
        {
            var book = Host.me.portalBook;

            if (book == null || book.getDistricts().Count < 1)
                return;


            Utils.InvokeOn(this, () =>
            {
                cmbox_PortalsList.Items.AddRange
                    (book.getDistricts().Select(d => d.name).OrderBy(d => d).ToArray());
                cmbox_PortalsList.SelectedIndex = 0;
            });
        }

        private Task GetSQLItems(string match)
        {
            queryTask = Task.Run(() =>
            {
                var items = Host.sqlCore.sqlItems.Where
                    (i => i.Value.name.ToLower().Contains(match.ToLower())).Select(i => i.Value.name).Distinct()
                    .OrderBy(i => i);

                if (items.Count() == 0)
                    return;


                Utils.InvokeOn(this, () =>
                {
                    lbox_ItemsList.Items.Clear();
                    lbox_ItemsList.Items.AddRange(items.ToArray());
                });
            });

            return queryTask;
        }

        private void GetInventoryItems(string match = "")
        {
            var items = Host.getAllInvItems().Select(i => i.name).Distinct();

            if (items.Count() == 0)
                return;


            if (match != string.Empty)
            {
                items = items.Where(i => i.ToLower().Contains(match.ToLower()));
            }

            items = items.OrderBy(i => i);


            Utils.InvokeOn(this, () =>
            {
                lbox_ItemsList.Items.Clear();
                lbox_ItemsList.Items.AddRange(items.ToArray());
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

        private void btn_AddToCleanItems_Click(object sender, EventArgs e)
        {
            Utils.InvokeOn(this, () =>
            {
                var selected = lbox_ItemsList.SelectedItem;

                if (selected == null)
                    return;


                string name = selected.ToString();

                if (lbox_CleanItems.Items.Contains(name))
                    return;


                lbox_CleanItems.Items.Add(name);
            });
        }

        #endregion

        #region Events

        private void StatsUpdated(Stats stats)
        {
            Utils.InvokeOn(this, () =>
            {
                lbl_LaborStartedWith.Text = stats.LaborStartedWith.ToString();
                lbl_LaborBurned.Text = stats.LaborBurned.ToString();
                lbl_VeinsMined.Text = stats.VeinsMined.ToString();
                lbl_FortunaVeins.Text = stats.FortunaVeins.ToString();
                lbl_UniVeins.Text = stats.UnidentifiedVeins.ToString();
            });
        }

        private void btn_MoveTaskUp_Click(object sender, EventArgs e)
        {
            if (MoveListItem(-1, lbox_MiningTasks))
            {
                SaveTasks();
            }
        }

        private void btn_MoveTaskDown_Click(object sender, EventArgs e)
        {
            if (MoveListItem(1, lbox_MiningTasks))
            {
                SaveTasks();
            }
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

                    SaveTasks();
                }
                
                // Remove item
                lbox_MiningTasks.Items.Remove(item);
            });
        }

        private void MiningTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.InvokeOn(this, () =>
            {
                string name = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);

                if (name.Length > 0)
                {
                    LoadTask(name);
                }
            });
        }

        private async void txtbox_ItemSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter 
                || queryTask != null && queryTask.Status == TaskStatus.Running)
                return;

            // Suppress defaults
            e.SuppressKeyPress = true;


            string match = string.Empty;

            Utils.InvokeOn(this, () =>
            {
                match = txtbox_ItemSearch.Text;

                if (match.Length < 3)
                    return;


                txtbox_ItemSearch.Enabled = false;
            });

            if (match.Length < 3)
                return;
            

            await GetSQLItems(match);

            Utils.InvokeOn(this, () => txtbox_ItemSearch.Enabled = true);
        }

        private void btn_GetInventoryItems_Click(object sender, EventArgs e)
        {
            GetInventoryItems();
        }

        private void lbox_CleanItems_DoubleClick(object sender, EventArgs e)
        {
            PopFromList(lbox_CleanItems);
        }

        // Task values changes
        private void ZonesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadingTask) SaveTask();
        }

        private void PortalsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadingTask) SaveTask();
        }

        #endregion
    }
}
