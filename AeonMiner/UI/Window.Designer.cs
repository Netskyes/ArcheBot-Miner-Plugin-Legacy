namespace AeonMiner.UI
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab_Main = new System.Windows.Forms.TabControl();
            this.tab_Mining = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtbox_TaskName = new System.Windows.Forms.TextBox();
            this.btn_AddMiningTask = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tab_Options = new System.Windows.Forms.TabPage();
            this.label_WhenDone = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tab_Overview = new System.Windows.Forms.TabPage();
            this.tab_Extra = new System.Windows.Forms.TabPage();
            this.btn_Start = new System.Windows.Forms.Button();
            this.container4 = new Container();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.container6 = new Container();
            this.lbox_MiningTasks = new System.Windows.Forms.ListBox();
            this.btn_MoveTaskUp = new System.Windows.Forms.Button();
            this.btn_MoveTaskDown = new System.Windows.Forms.Button();
            this.container2 = new Container();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.container5 = new Container();
            this.cmbox_ZonesList = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.container_WhenDone = new Container();
            this.label_Finally = new System.Windows.Forms.Label();
            this._TerminateClient = new System.Windows.Forms.RadioButton();
            this.txtbox_PluginRunName = new System.Windows.Forms.TextBox();
            this._Nothing = new System.Windows.Forms.RadioButton();
            this._ToSelectionScreen = new System.Windows.Forms.RadioButton();
            this.chkbox_RunPlugin = new System.Windows.Forms.CheckBox();
            this.label_Name = new System.Windows.Forms.Label();
            this.container3 = new Container();
            this.chkbox_AutoStart = new System.Windows.Forms.CheckBox();
            this.container1 = new Container();
            this.cmbox_MountsList = new System.Windows.Forms.ComboBox();
            this.tab_Main.SuspendLayout();
            this.tab_Mining.SuspendLayout();
            this.tab_Options.SuspendLayout();
            this.container4.SuspendLayout();
            this.container6.SuspendLayout();
            this.container2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.container5.SuspendLayout();
            this.container_WhenDone.SuspendLayout();
            this.container3.SuspendLayout();
            this.container1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_Main
            // 
            this.tab_Main.Controls.Add(this.tab_Mining);
            this.tab_Main.Controls.Add(this.tab_Options);
            this.tab_Main.Controls.Add(this.tab_Overview);
            this.tab_Main.Controls.Add(this.tab_Extra);
            this.tab_Main.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tab_Main.Location = new System.Drawing.Point(0, 46);
            this.tab_Main.Name = "tab_Main";
            this.tab_Main.SelectedIndex = 0;
            this.tab_Main.Size = new System.Drawing.Size(603, 405);
            this.tab_Main.TabIndex = 0;
            // 
            // tab_Mining
            // 
            this.tab_Mining.Controls.Add(this.label6);
            this.tab_Mining.Controls.Add(this.container4);
            this.tab_Mining.Controls.Add(this.txtbox_TaskName);
            this.tab_Mining.Controls.Add(this.btn_AddMiningTask);
            this.tab_Mining.Controls.Add(this.container6);
            this.tab_Mining.Controls.Add(this.label4);
            this.tab_Mining.Controls.Add(this.container2);
            this.tab_Mining.Controls.Add(this.label3);
            this.tab_Mining.Location = new System.Drawing.Point(4, 22);
            this.tab_Mining.Name = "tab_Mining";
            this.tab_Mining.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Mining.Size = new System.Drawing.Size(595, 379);
            this.tab_Mining.TabIndex = 0;
            this.tab_Mining.Text = "Mining";
            this.tab_Mining.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(13, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Task options";
            // 
            // txtbox_TaskName
            // 
            this.txtbox_TaskName.Location = new System.Drawing.Point(8, 28);
            this.txtbox_TaskName.Name = "txtbox_TaskName";
            this.txtbox_TaskName.Size = new System.Drawing.Size(112, 22);
            this.txtbox_TaskName.TabIndex = 13;
            // 
            // btn_AddMiningTask
            // 
            this.btn_AddMiningTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AddMiningTask.Location = new System.Drawing.Point(126, 26);
            this.btn_AddMiningTask.Name = "btn_AddMiningTask";
            this.btn_AddMiningTask.Size = new System.Drawing.Size(44, 24);
            this.btn_AddMiningTask.TabIndex = 12;
            this.btn_AddMiningTask.Text = "Add";
            this.btn_AddMiningTask.UseVisualStyleBackColor = true;
            this.btn_AddMiningTask.Click += new System.EventHandler(this.btn_AddMiningTask_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(187, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Task setup";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(5, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Mining tasks";
            // 
            // tab_Options
            // 
            this.tab_Options.Controls.Add(this.label_WhenDone);
            this.tab_Options.Controls.Add(this.label2);
            this.tab_Options.Controls.Add(this.label1);
            this.tab_Options.Controls.Add(this.container_WhenDone);
            this.tab_Options.Controls.Add(this.container3);
            this.tab_Options.Controls.Add(this.container1);
            this.tab_Options.Location = new System.Drawing.Point(4, 22);
            this.tab_Options.Name = "tab_Options";
            this.tab_Options.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Options.Size = new System.Drawing.Size(595, 379);
            this.tab_Options.TabIndex = 1;
            this.tab_Options.Text = "Options";
            this.tab_Options.UseVisualStyleBackColor = true;
            // 
            // label_WhenDone
            // 
            this.label_WhenDone.AutoSize = true;
            this.label_WhenDone.ForeColor = System.Drawing.Color.DimGray;
            this.label_WhenDone.Location = new System.Drawing.Point(433, 176);
            this.label_WhenDone.Name = "label_WhenDone";
            this.label_WhenDone.Size = new System.Drawing.Size(68, 13);
            this.label_WhenDone.TabIndex = 34;
            this.label_WhenDone.Text = "When done";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(13, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Travel mount:";
            // 
            // tab_Overview
            // 
            this.tab_Overview.Location = new System.Drawing.Point(4, 22);
            this.tab_Overview.Name = "tab_Overview";
            this.tab_Overview.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Overview.Size = new System.Drawing.Size(595, 379);
            this.tab_Overview.TabIndex = 3;
            this.tab_Overview.Text = "Overview";
            this.tab_Overview.UseVisualStyleBackColor = true;
            // 
            // tab_Extra
            // 
            this.tab_Extra.Location = new System.Drawing.Point(4, 22);
            this.tab_Extra.Name = "tab_Extra";
            this.tab_Extra.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Extra.Size = new System.Drawing.Size(595, 379);
            this.tab_Extra.TabIndex = 2;
            this.tab_Extra.Text = "Extra";
            this.tab_Extra.UseVisualStyleBackColor = true;
            // 
            // btn_Start
            // 
            this.btn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Start.Location = new System.Drawing.Point(501, 12);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(89, 28);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "Start Mining";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // container4
            // 
            this.container4.Controls.Add(this.checkBox1);
            this.container4.Location = new System.Drawing.Point(8, 172);
            this.container4.Name = "container4";
            this.container4.Size = new System.Drawing.Size(162, 147);
            this.container4.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(10, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Next if";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // container6
            // 
            this.container6.Controls.Add(this.lbox_MiningTasks);
            this.container6.Controls.Add(this.btn_MoveTaskUp);
            this.container6.Controls.Add(this.btn_MoveTaskDown);
            this.container6.Location = new System.Drawing.Point(8, 56);
            this.container6.Name = "container6";
            this.container6.Padding = new System.Windows.Forms.Padding(3);
            this.container6.Size = new System.Drawing.Size(162, 104);
            this.container6.TabIndex = 11;
            // 
            // lbox_MiningTasks
            // 
            this.lbox_MiningTasks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbox_MiningTasks.FormattingEnabled = true;
            this.lbox_MiningTasks.Items.AddRange(new object[] {
            "MiningTask 1",
            "MiningTask 2"});
            this.lbox_MiningTasks.Location = new System.Drawing.Point(6, 6);
            this.lbox_MiningTasks.Name = "lbox_MiningTasks";
            this.lbox_MiningTasks.Size = new System.Drawing.Size(150, 65);
            this.lbox_MiningTasks.TabIndex = 0;
            this.lbox_MiningTasks.DoubleClick += new System.EventHandler(this.lbox_MiningTasks_DoubleClick);
            // 
            // btn_MoveTaskUp
            // 
            this.btn_MoveTaskUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MoveTaskUp.Image = global::AeonMiner.Properties.Resources.arrow_up;
            this.btn_MoveTaskUp.Location = new System.Drawing.Point(89, 77);
            this.btn_MoveTaskUp.Name = "btn_MoveTaskUp";
            this.btn_MoveTaskUp.Size = new System.Drawing.Size(32, 20);
            this.btn_MoveTaskUp.TabIndex = 15;
            this.btn_MoveTaskUp.UseVisualStyleBackColor = true;
            this.btn_MoveTaskUp.Click += new System.EventHandler(this.btn_MoveTaskUp_Click);
            // 
            // btn_MoveTaskDown
            // 
            this.btn_MoveTaskDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MoveTaskDown.Image = global::AeonMiner.Properties.Resources.arrow_down;
            this.btn_MoveTaskDown.Location = new System.Drawing.Point(124, 77);
            this.btn_MoveTaskDown.Name = "btn_MoveTaskDown";
            this.btn_MoveTaskDown.Size = new System.Drawing.Size(32, 20);
            this.btn_MoveTaskDown.TabIndex = 14;
            this.btn_MoveTaskDown.UseVisualStyleBackColor = true;
            this.btn_MoveTaskDown.Click += new System.EventHandler(this.btn_MoveTaskDown_Click);
            // 
            // container2
            // 
            this.container2.Controls.Add(this.label7);
            this.container2.Controls.Add(this.numericUpDown1);
            this.container2.Controls.Add(this.container5);
            this.container2.Controls.Add(this.label5);
            this.container2.Location = new System.Drawing.Point(182, 18);
            this.container2.Name = "container2";
            this.container2.Size = new System.Drawing.Size(404, 209);
            this.container2.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(211, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Min. Labor";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numericUpDown1.Location = new System.Drawing.Point(279, 41);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(67, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // container5
            // 
            this.container5.Controls.Add(this.cmbox_ZonesList);
            this.container5.Location = new System.Drawing.Point(16, 31);
            this.container5.Name = "container5";
            this.container5.Size = new System.Drawing.Size(169, 27);
            this.container5.TabIndex = 7;
            // 
            // cmbox_ZonesList
            // 
            this.cmbox_ZonesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbox_ZonesList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbox_ZonesList.FormattingEnabled = true;
            this.cmbox_ZonesList.Location = new System.Drawing.Point(3, 3);
            this.cmbox_ZonesList.Name = "cmbox_ZonesList";
            this.cmbox_ZonesList.Size = new System.Drawing.Size(163, 21);
            this.cmbox_ZonesList.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Mining zone:";
            // 
            // container_WhenDone
            // 
            this.container_WhenDone.Controls.Add(this.label_Finally);
            this.container_WhenDone.Controls.Add(this._TerminateClient);
            this.container_WhenDone.Controls.Add(this.txtbox_PluginRunName);
            this.container_WhenDone.Controls.Add(this._Nothing);
            this.container_WhenDone.Controls.Add(this._ToSelectionScreen);
            this.container_WhenDone.Controls.Add(this.chkbox_RunPlugin);
            this.container_WhenDone.Controls.Add(this.label_Name);
            this.container_WhenDone.Location = new System.Drawing.Point(424, 182);
            this.container_WhenDone.Name = "container_WhenDone";
            this.container_WhenDone.Size = new System.Drawing.Size(162, 189);
            this.container_WhenDone.TabIndex = 33;
            // 
            // label_Finally
            // 
            this.label_Finally.AutoSize = true;
            this.label_Finally.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_Finally.Location = new System.Drawing.Point(11, 90);
            this.label_Finally.Name = "label_Finally";
            this.label_Finally.Size = new System.Drawing.Size(43, 13);
            this.label_Finally.TabIndex = 34;
            this.label_Finally.Text = "Finally:";
            // 
            // _TerminateClient
            // 
            this._TerminateClient.AutoSize = true;
            this._TerminateClient.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._TerminateClient.Location = new System.Drawing.Point(14, 133);
            this._TerminateClient.Name = "_TerminateClient";
            this._TerminateClient.Size = new System.Drawing.Size(106, 17);
            this._TerminateClient.TabIndex = 24;
            this._TerminateClient.Text = "Terminate client";
            this._TerminateClient.UseVisualStyleBackColor = true;
            // 
            // txtbox_PluginRunName
            // 
            this.txtbox_PluginRunName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtbox_PluginRunName.Location = new System.Drawing.Point(14, 57);
            this.txtbox_PluginRunName.Name = "txtbox_PluginRunName";
            this.txtbox_PluginRunName.Size = new System.Drawing.Size(134, 22);
            this.txtbox_PluginRunName.TabIndex = 20;
            this.txtbox_PluginRunName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _Nothing
            // 
            this._Nothing.AutoSize = true;
            this._Nothing.Checked = true;
            this._Nothing.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._Nothing.Location = new System.Drawing.Point(14, 110);
            this._Nothing.Name = "_Nothing";
            this._Nothing.Size = new System.Drawing.Size(68, 17);
            this._Nothing.TabIndex = 22;
            this._Nothing.TabStop = true;
            this._Nothing.Text = "Nothing";
            this._Nothing.UseVisualStyleBackColor = true;
            // 
            // _ToSelectionScreen
            // 
            this._ToSelectionScreen.AutoSize = true;
            this._ToSelectionScreen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this._ToSelectionScreen.Location = new System.Drawing.Point(14, 156);
            this._ToSelectionScreen.Name = "_ToSelectionScreen";
            this._ToSelectionScreen.Size = new System.Drawing.Size(122, 17);
            this._ToSelectionScreen.TabIndex = 23;
            this._ToSelectionScreen.Text = "To selection screen";
            this._ToSelectionScreen.UseVisualStyleBackColor = true;
            // 
            // chkbox_RunPlugin
            // 
            this.chkbox_RunPlugin.AutoSize = true;
            this.chkbox_RunPlugin.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chkbox_RunPlugin.Location = new System.Drawing.Point(14, 16);
            this.chkbox_RunPlugin.Name = "chkbox_RunPlugin";
            this.chkbox_RunPlugin.Size = new System.Drawing.Size(84, 17);
            this.chkbox_RunPlugin.TabIndex = 19;
            this.chkbox_RunPlugin.Text = "Run plugin";
            this.chkbox_RunPlugin.UseVisualStyleBackColor = true;
            // 
            // label_Name
            // 
            this.label_Name.AutoSize = true;
            this.label_Name.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_Name.Location = new System.Drawing.Point(11, 40);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(39, 13);
            this.label_Name.TabIndex = 18;
            this.label_Name.Text = "Name:";
            // 
            // container3
            // 
            this.container3.Controls.Add(this.chkbox_AutoStart);
            this.container3.Location = new System.Drawing.Point(8, 18);
            this.container3.Name = "container3";
            this.container3.Size = new System.Drawing.Size(200, 100);
            this.container3.TabIndex = 5;
            // 
            // chkbox_AutoStart
            // 
            this.chkbox_AutoStart.AutoSize = true;
            this.chkbox_AutoStart.Location = new System.Drawing.Point(10, 17);
            this.chkbox_AutoStart.Name = "chkbox_AutoStart";
            this.chkbox_AutoStart.Size = new System.Drawing.Size(96, 17);
            this.chkbox_AutoStart.TabIndex = 2;
            this.chkbox_AutoStart.Text = "Auto Start Up";
            this.chkbox_AutoStart.UseVisualStyleBackColor = true;
            // 
            // container1
            // 
            this.container1.Controls.Add(this.cmbox_MountsList);
            this.container1.Location = new System.Drawing.Point(255, 44);
            this.container1.Name = "container1";
            this.container1.Size = new System.Drawing.Size(169, 27);
            this.container1.TabIndex = 3;
            // 
            // cmbox_MountsList
            // 
            this.cmbox_MountsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbox_MountsList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbox_MountsList.FormattingEnabled = true;
            this.cmbox_MountsList.Location = new System.Drawing.Point(3, 3);
            this.cmbox_MountsList.Name = "cmbox_MountsList";
            this.cmbox_MountsList.Size = new System.Drawing.Size(163, 21);
            this.cmbox_MountsList.TabIndex = 0;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(603, 451);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.tab_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MaximizeBox = false;
            this.Name = "Window";
            this.ShowIcon = false;
            this.Text = "AeonMiner";
            this.Load += new System.EventHandler(this.Window_Load);
            this.tab_Main.ResumeLayout(false);
            this.tab_Mining.ResumeLayout(false);
            this.tab_Mining.PerformLayout();
            this.tab_Options.ResumeLayout(false);
            this.tab_Options.PerformLayout();
            this.container4.ResumeLayout(false);
            this.container4.PerformLayout();
            this.container6.ResumeLayout(false);
            this.container2.ResumeLayout(false);
            this.container2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.container5.ResumeLayout(false);
            this.container_WhenDone.ResumeLayout(false);
            this.container_WhenDone.PerformLayout();
            this.container3.ResumeLayout(false);
            this.container3.PerformLayout();
            this.container1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_Main;
        private System.Windows.Forms.TabPage tab_Mining;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.CheckBox chkbox_AutoStart;
        private Container container1;
        private System.Windows.Forms.ComboBox cmbox_MountsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tab_Options;
        private System.Windows.Forms.Label label2;
        private Container container3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Container container2;
        private Container container5;
        private System.Windows.Forms.ComboBox cmbox_ZonesList;
        private Container container6;
        private System.Windows.Forms.ListBox lbox_MiningTasks;
        private System.Windows.Forms.TextBox txtbox_TaskName;
        private System.Windows.Forms.Button btn_AddMiningTask;
        private System.Windows.Forms.Button btn_MoveTaskUp;
        private System.Windows.Forms.Button btn_MoveTaskDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Container container4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tab_Extra;
        private System.Windows.Forms.Label label_WhenDone;
        private Container container_WhenDone;
        public System.Windows.Forms.Label label_Finally;
        private System.Windows.Forms.RadioButton _TerminateClient;
        private System.Windows.Forms.TextBox txtbox_PluginRunName;
        private System.Windows.Forms.RadioButton _Nothing;
        private System.Windows.Forms.RadioButton _ToSelectionScreen;
        private System.Windows.Forms.CheckBox chkbox_RunPlugin;
        public System.Windows.Forms.Label label_Name;
        private System.Windows.Forms.TabPage tab_Overview;
    }
}