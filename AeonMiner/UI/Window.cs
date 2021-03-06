﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArcheBot.Bot.Classes;

namespace AeonMiner.UI
{
    using Data;
    using Configs;
    using Helpers;

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
            GetMiningNodes();
            GetSkills();

            // Loads prefs
            LoadTasks();
            LoadSettings();

            // Register events
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

        public void GameLog(string text)
            => Utils.InvokeOn(this, () => txtbox_GameLog.AppendText(text + Environment.NewLine));

        public void UpdateLabel(Label label, string text)
            => Utils.InvokeOn(this, () => label.Text = text);

        private void PopFromList(ListBox lbox)
        {
            Utils.InvokeOn(this, () =>
            {
                var selected = lbox.SelectedItem;

                if (selected != null) lbox.Items.Remove(selected);
            });
        }

        private void AddItemToList(Control item, ListBox inLbox, bool checkExists = false)
        {
            Utils.InvokeOn(this, () =>
            {
                object selected = null;

                if (item is ListBox)
                {
                    selected = (item as ListBox).SelectedItem;
                }
                else if (item is ComboBox)
                {
                    selected = (item as ComboBox).SelectedItem;
                }


                if (selected == null || (checkExists && inLbox.Items.Contains(selected)))
                {
                    return;
                }

                inLbox.Items.Add(selected);
            });
        }

        private bool MoveListItem(int direction, ListBox box)
        {
            bool result = false;

            Utils.InvokeOn(this, () =>
            {
                var item = box.SelectedItem;

                if (item == null || box.SelectedIndex < 0)
                    return;

                
                int index = box.SelectedIndex, nIndex = (index + direction);

                if (nIndex < 0 || nIndex >= box.Items.Count)
                    return;


                box.Items.RemoveAt(index);
                box.Items.Insert(nIndex, item);

                box.SetSelected(nIndex, true);
                result = true;
            });

            return result;
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
            Utils.InvokeOn(this, () =>
            {
                var row = dtg_Items.Rows.OfType<DataGridViewRow>().FirstOrDefault(d => (uint)d.Tag == item.id);

                if (row != null)
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

                    return;
                }


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

        public Dictionary<string, MineTask> MiningTasks
        {
            get
            {
                lock (miningTasksLock) { return miningTasks; }
            }
        }

        private bool btnSwitch;
        private bool isLoadingTask;
        private Task queryTask;

        private Dictionary<string, MineTask> miningTasks = new Dictionary<string, MineTask>();
        private readonly object miningTasksLock = new object();

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
                chkbox_MailProducts.Checked = settings.MailProducts;
                chkbox_UseExpressDelivery.Checked = settings.UseExpressDelivery;
                chkbox_ExcludeUnwantedItems.Checked = settings.ExcludeUnwantedItems;
                chkbox_AuctionProducts.Checked = settings.AuctionProducts;
                chkbox_RemoveSuspect.Checked = settings.RemoveSuspect;
                chkbox_UseDash.Checked = settings.UseDash;
                chkbox_UseFreerunner.Checked = settings.UseFreerunner;
                chkbox_UseTeleportation.Checked = settings.UseTeleportation;
                chkbox_UseQuickstep.Checked = settings.UseQuickstep;
                chkbox_UseCometsBoon.Checked = settings.UseCometsBoon;
                chkbox_UseCampfire.Checked = settings.UseCampfire;
                chkbox_UseHastenerScroll.Checked = settings.UseHastenerScroll;
                chkbox_UseLaborPotion.Checked = settings.UseLaborPotion;
                chkbox_MakeLaborRecovery.Checked = settings.MakeLaborRecovery;


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

                var optionA = container_WhenDone.Controls.OfType<OptionBox>().FirstOrDefault(r => r.OptionName == settings.FinalAction);

                if (optionA != null)
                {
                    optionA.Checked = true;
                }

                txtbox_PluginRunName.Text = settings.PluginRunName;
                txtbox_MailRecipient.Text = settings.MailRecipient;

                var optionB = container_LaborRecovery.Controls.OfType<OptionBox>().FirstOrDefault(r => r.OptionName == settings.LaborRecoveryType);

                if (optionB != null)
                {
                    optionB.Checked = true;
                }

                lbox_CleanItems.Items.AddRange(settings.CleanItems.ToArray());


                num_MinLaborPoints.Value = settings.MinLaborPoints;
                num_ResumeWhenLabor.Value = settings.ResumeWhenLabor;
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
                settings.MailProducts = chkbox_MailProducts.Checked;
                settings.UseExpressDelivery = chkbox_UseExpressDelivery.Checked;
                settings.ExcludeUnwantedItems = chkbox_ExcludeUnwantedItems.Checked;
                settings.AuctionProducts = chkbox_AuctionProducts.Checked;
                settings.RemoveSuspect = chkbox_RemoveSuspect.Checked;
                settings.UseDash = chkbox_UseDash.Checked;
                settings.UseFreerunner = chkbox_UseFreerunner.Checked;
                settings.UseTeleportation = chkbox_UseTeleportation.Checked;
                settings.UseQuickstep = chkbox_UseQuickstep.Checked;
                settings.UseCometsBoon = chkbox_UseCometsBoon.Checked;
                settings.UseCampfire = chkbox_UseCampfire.Checked;
                settings.UseHastenerScroll = chkbox_UseHastenerScroll.Checked;
                settings.UseLaborPotion = chkbox_UseLaborPotion.Checked;
                settings.MakeLaborRecovery = chkbox_MakeLaborRecovery.Checked;
                settings.TaskName = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                settings.TravelMount = cmbox_MountsList.GetItemText(cmbox_MountsList.SelectedItem);
                settings.FinalAction = container_WhenDone.Controls.OfType<OptionBox>().FirstOrDefault(r => r.Checked)?.OptionName;
                settings.PluginRunName = txtbox_PluginRunName.Text;
                settings.MailRecipient = txtbox_MailRecipient.Text;
                settings.LaborRecoveryType = container_LaborRecovery.Controls.OfType<OptionBox>().FirstOrDefault(r => r.Checked)?.OptionName;
                settings.CleanItems = lbox_CleanItems.Items.OfType<string>().ToList();
                settings.MinLaborPoints = (int)num_MinLaborPoints.Value;
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

                foreach (var item in chklbox_IgnoreVeins.Items.OfType<string>().ToArray())
                {
                    int index = chklbox_IgnoreVeins.Items.IndexOf(item);

                    chklbox_IgnoreVeins.SetItemChecked(index, (mineTask.IgnoreVeins.Contains(item)));
                }

                num_TaskTime.Value = mineTask.TaskTime;
                chkbox_UseTaskTIme.Checked = mineTask.UseTaskTime;

                // ...
            });

            // Unlock events
            isLoadingTask = false;
        }

        public MineTask GetTask()
        {
            var task = new MineTask();

            Utils.InvokeOn(this, () =>
            {
                task.Name = lbox_MiningTasks.GetItemText(lbox_MiningTasks.SelectedItem);
                task.MiningZone = cmbox_ZonesList.GetItemText(cmbox_ZonesList.SelectedItem);
                task.IgnoreVeins = chklbox_IgnoreVeins.CheckedItems.OfType<string>().ToList();
                task.TaskTime = (int)num_TaskTime.Value;
                task.UseTaskTime = chkbox_UseTaskTIme.Checked;
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
                cmbox_MountsList.Items.Clear();
                cmbox_MountsList.Items.AddRange(mounts.ToArray());
                cmbox_MountsList.SelectedIndex = 0;
            });
        }

        private void GetMiningZones()
        {
            var zones = MapsHelper.GetAll().Select(m => m.Name).OrderBy(m => m);

            if (zones.Count() < 1)
                return;


            Utils.InvokeOn(this, () =>
            {
                cmbox_ZonesList.Items.Clear();
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
                cmbox_PortalsList.Items.Clear();
                cmbox_PortalsList.Items.AddRange(book.getDistricts().Select(d => d.name).OrderBy(d => d).ToArray());
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
            var items = Host.getAllInvItems().Select(i => i.name).Where
                (i => i != string.Empty).Distinct();

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

        private void GetMiningNodes()
        {
            Utils.InvokeOn(this, () =>
            {
                chklbox_IgnoreVeins.Items.AddRange(MiningNodes.GetAll().Select(n => n.Name).ToArray());
            });
        }

        private void GetMiningProducts()
        {
            var items = MiningNodes.GetProducts().Select
                (i => (Host.clientVersion != ClientVersion.RussiaMailRu) ? i.Value : i.Value.ToRussian());

            Utils.InvokeOn(this, () =>
            {
                lbox_ItemsList.Items.Clear();
                lbox_ItemsList.Items.AddRange(items.ToArray());
            });
        }

        private void GetSkills()
        {
            var classes = Host.me.getAbilities().Where(a => a.active);


            var skills = Host.me.getSkills().Where
                (s => classes.Any(c => s.db.abilityId == (int)c.id)).Select(s => s.name);

            if (skills.Count() < 1)
                return;


            Utils.InvokeOn(this, () =>
            {
                cmbox_SkillsList.Items.Clear();
                cmbox_SkillsList.Items.AddRange(skills.ToArray());
                cmbox_SkillsList.SelectedIndex = 0;
            });
        }

        #endregion


        #region Click Events

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


            lock (miningTasksLock)
            {
                if (miningTasks.ContainsKey(name))
                {
                    MessageBox.Show("Task with that name already exists!");
                    return;
                }

                // Add task
                miningTasks.Add(name, new MineTask() { Name = name });

                SaveTasks();
            }


            Utils.InvokeOn(this, () =>
            {
                if (!lbox_MiningTasks.Items.Contains(name)) { lbox_MiningTasks.Items.Add(name); }

                // Empty textbox
                txtbox_TaskName.Clear();
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

        private void btn_MoveRotationUp_Click(object sender, EventArgs e)
        {
            MoveListItem(-1, lbox_Rotation);
        }

        private void btn_MoveRotationDown_Click(object sender, EventArgs e)
        {
            MoveListItem(1, lbox_Rotation);
        }

        private void btn_GetInventoryItems_Click(object sender, EventArgs e)
        {
            GetInventoryItems();
        }

        private void btn_GetProducts_Click(object sender, EventArgs e)
        {
            GetMiningProducts();
        }

        private void btn_AddToCleanItems_Click(object sender, EventArgs e)
        {
            AddItemToList(lbox_ItemsList, lbox_CleanItems, true);
        }

        private void btn_AddToRotation_Click(object sender, EventArgs e)
        {
            AddItemToList(cmbox_SkillsList, lbox_Rotation);
        }

        private void btn_AddToBuffs_Click(object sender, EventArgs e)
        {
            AddItemToList(cmbox_SkillsList, lbox_Buffs, true);
        }

        private void btn_AddToHeals_Click(object sender, EventArgs e)
        {
            AddItemToList(cmbox_SkillsList, lbox_Heals, true);
        }

        private void btn_MoveBuffsUp_Click(object sender, EventArgs e)
        {
            MoveListItem(-1, lbox_Buffs);
        }

        private void btn_MoveBuffsDown_Click(object sender, EventArgs e)
        {
            MoveListItem(1, lbox_Buffs);
        }

        private void btn_MoveHealsUp_Click(object sender, EventArgs e)
        {
            MoveListItem(-1, lbox_Heals);
        }

        private void btn_MoveHealsDown_Click(object sender, EventArgs e)
        {
            MoveListItem(1, lbox_Heals);
        }

        #endregion

        #region Double Click Events

        private void lbox_MiningTasks_DoubleClick(object sender, EventArgs e)
        {
            Utils.InvokeOn(this, () =>
            {
                var item = lbox_MiningTasks.SelectedItem;

                if (item == null)
                    return;

                string name = item.ToString();


                lock (miningTasksLock)
                {
                    // Remove key
                    if (miningTasks.ContainsKey(name))
                    {
                        miningTasks.Remove(name);

                        SaveTasks();
                    }
                }

                // Remove item
                lbox_MiningTasks.Items.Remove(item);
            });
        }

        private void lbox_CleanItems_DoubleClick(object sender, EventArgs e)
        {
            PopFromList(lbox_CleanItems);
        }

        private void lbox_Rotation_DoubleClick(object sender, EventArgs e)
        {
            PopFromList(lbox_Rotation);
        }

        private void lbox_Buffs_DoubleClick(object sender, EventArgs e)
        {
            PopFromList(lbox_Buffs);
        }

        private void lbox_Heals_DoubleClick(object sender, EventArgs e)
        {
            PopFromList(lbox_Heals);
        }

        #endregion

        #region Changed Events

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

        private void ZonesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadingTask) SaveTask();
        }

        private void PortalsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadingTask) SaveTask();
        }


        private void chkbox_HideGameClient_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                Host.HideGameClient();
            }
            else
            {
                Host.ShowGameClient();
            }
        }

        private void chkbox_UseTaskTime_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoadingTask) SaveTask();
        }


        private void chklbox_IgnoreVeins_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!isLoadingTask)
            {
                BeginInvoke(new Action(() => SaveTask()));
            }
        }


        private void num_TaskTime_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoadingTask) SaveTask();
        }

        #endregion

        #region Other Events

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

        #endregion
    }
}
