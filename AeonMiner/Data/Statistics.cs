using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AeonMiner.Data
{
    using UI;

    [Serializable]
    public sealed class Statistics
    {
        private Window UI;
        
        public Statistics(Window ui)
        {
            UI = ui;
        }

        public string ZoneName { get; set; }

        #region Fields

        private int _runTime;
        private int _avgMinedPerHour;
        private int _laborStartedWith;
        private int _veinsMined;
        private int _fortunaVeins;
        private int _unidentifiedVeins;
        private int _laborBurned = 1;
        private int _suspectReports;
        private int _whispersReceived;
        private int _playersPeak;

        private List<int> _players = new List<int>();

        #endregion


        public void MakeAvgMinedPerHour()
        {
            AvgMinedPerHour = (int)((double)VeinsMined / (double)RunTime * 3600);
        }

        public void MakeEstimateBurnTime(int remaining)
        {
            double timePerLabor = (double)RunTime / (double)(LaborBurned);
            double timeTotal = timePerLabor * (double)remaining;

            var estimates = new DateTime(TimeSpan.FromSeconds(timeTotal).Ticks).ToString("HH:mm:ss");


            UI.UpdateLabel(UI.lbl_EstimatingTime, estimates);
        }

        public void Save()
        {
        }


        /// <summary>
        /// Time in seconds.
        /// </summary>
        public int RunTime
        {
            get { return _runTime; }
            set
            {
                (_runTime) = value;

                var elapse = new DateTime(TimeSpan.FromSeconds(RunTime).Ticks).ToString("HH:mm:ss");
                UI.UpdateLabel(UI.lbl_RunTime, elapse);
            }
        }

        public int AvgMinedPerHour
        {
            get { return _avgMinedPerHour; }
            set
            {
                (_avgMinedPerHour) = value;

                UI.UpdateLabel(UI.lbl_AvgMinedPerHour, AvgMinedPerHour.ToString());
            }
        }

        public int LaborStartedWith
        {
            get { return _laborStartedWith; }
            set
            {
                (_laborStartedWith) = value;

                UI.UpdateLabel(UI.lbl_LaborStartedWith, LaborStartedWith.ToString());
            }
        }

        public int LaborBurned
        {
            get { return _laborBurned; }
            set
            {
                (_laborBurned) = value;

                UI.UpdateLabel(UI.lbl_LaborBurned, LaborBurned.ToString());
            }
        }

        public int VeinsMined
        {
            get { return _veinsMined; }
            set
            {
                (_veinsMined) = value;

                UI.UpdateLabel(UI.lbl_VeinsMined, VeinsMined.ToString());
            }
        }

        public int FortunaVeins
        {
            get { return _fortunaVeins; }
            set
            {
                (_fortunaVeins) = value;

                UI.UpdateLabel(UI.lbl_FortunaVeins, FortunaVeins.ToString());
            }
        }

        public int UnidentifiedVeins
        {
            get { return _unidentifiedVeins; }
            set
            {
                (_unidentifiedVeins) = value;

                UI.UpdateLabel(UI.lbl_UniVeins, UnidentifiedVeins.ToString());
            }
        }

        public int SuspectReports
        {
            get { return _suspectReports; }
            set
            {
                (_suspectReports) = value;

                UI.UpdateLabel(UI.lbl_SuspectReports, SuspectReports.ToString());
            }
        }

        public int WhispersReceived
        {
            get { return _whispersReceived; }
            set
            {
                (_whispersReceived) = value;

                UI.UpdateLabel(UI.lbl_WhispersReceived, WhispersReceived.ToString());
            }
        }

        public int PlayersPeak
        {
            get { return _playersPeak; }
            set
            {
                (_playersPeak) = value;

                UI.UpdateLabel(UI.lbl_PlayersPeak, PlayersPeak.ToString());
            }
        }
    }
}
