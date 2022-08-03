using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MayCat
{
    public abstract class TagContainerBase : INotifyPropertyChanged
    {
        public bool ChoPhepMayChay { get; set; } = false;

        public ModbusReader ModbusReader { get; set; }
        public List<Tag> TagSource { get; set; } = new List<Tag>();

        public virtual Tag STT1 { get; set; }
        public virtual Tag Xa1 { get; set; }
        public virtual Tag Nap11 { get; set; }
        public virtual Tag Cao1 { get; set; }
        public virtual Tag Nap12 { get; set; }
        public virtual Tag Lang1 { get; set; }
        public virtual Tag Song1 { get; set; }
        public virtual Tag KheHo1 { get; set; }
        public virtual Tag STT2 { get; set; }
        public virtual Tag Xa2 { get; set; }
        public virtual Tag Nap21 { get; set; }
        public virtual Tag Cao2 { get; set; }
        public virtual Tag Nap22 { get; set; }
        public virtual Tag Lang2 { get; set; }
        public virtual Tag Song2 { get; set; }
        public virtual Tag KheHo2 { get; set; }
        public virtual Tag LenhChuyenDon { get; set; }
        public virtual Tag CheDoChuyenDon { get; set; }
        public virtual Tag Run { get; set; }
        public virtual Tag Chieudaidon { get; set; }
        public virtual Tag DoiDon { get; set; }
        public virtual Tag Dao1_PV { get; set; }
        public virtual Tag Dao1_SV { get; set; }
        public virtual Tag Dao1_U { get; set; }
        public virtual Tag Dao2_PV { get; set; }
        public virtual Tag Dao2_SV { get; set; }
        public virtual Tag Dao2_U { get; set; }
        public virtual Tag Dao3_PV { get; set; }
        public virtual Tag Dao3_SV { get; set; }
        public virtual Tag Dao3_U { get; set; }
        public virtual Tag Dao4_PV { get; set; }
        public virtual Tag Dao4_SV { get; set; }
        public virtual Tag Dao4_U { get; set; }
        public virtual Tag Dao5_PV { get; set; }
        public virtual Tag Dao5_SV { get; set; }
        public virtual Tag Dao5_U { get; set; }
        public virtual Tag Hut_PV { get; set; }
        public virtual Tag Hut_SV { get; set; }
        public virtual Tag Lang1_PV { get; set; }
        public virtual Tag Lang1_SV { get; set; }
        public virtual Tag Lang1_U { get; set; }
        public virtual Tag Lang2_PV { get; set; }
        public virtual Tag Lang2_SV { get; set; }
        public virtual Tag Lang2_U { get; set; }
        public virtual Tag Lang3_PV { get; set; }
        public virtual Tag Lang3_SV { get; set; }
        public virtual Tag Lang3_U { get; set; }
        public virtual Tag Lang4_PV { get; set; }
        public virtual Tag Lang4_SV { get; set; }
        public virtual Tag Lang4_U { get; set; }
        public virtual Tag Lang5_PV { get; set; }
        public virtual Tag Lang5_SV { get; set; }
        public virtual Tag Lang5_U { get; set; }
        public virtual Tag Lang6_PV { get; set; }
        public virtual Tag Lang6_SV { get; set; }
        public virtual Tag Lang6_U { get; set; }
        public virtual Tag Lang7_PV { get; set; }
        public virtual Tag Lang7_SV { get; set; }
        public virtual Tag Lang7_U { get; set; }
        public virtual Tag Lang8_PV { get; set; }
        public virtual Tag Lang8_SV { get; set; }
        public virtual Tag Lang8_U { get; set; }
        public virtual Tag KheHo_PV { get; set; }
        public virtual Tag KheHo_SV { get; set; }
        public virtual Tag Dao1_Adj { get; set; }
        public virtual Tag Dao2_Adj { get; set; }
        public virtual Tag Dao3_Adj { get; set; }
        public virtual Tag Dao4_Adj { get; set; }
        public virtual Tag Dao5_Adj { get; set; }
        public virtual Tag Hut_Adj { get; set; }
        public virtual Tag Lang1_Adj { get; set; }
        public virtual Tag Lang2_Adj { get; set; }
        public virtual Tag Lang3_Adj { get; set; }
        public virtual Tag Lang4_Adj { get; set; }
        public virtual Tag Lang5_Adj { get; set; }
        public virtual Tag Lang6_Adj { get; set; }
        public virtual Tag Lang7_Adj { get; set; }
        public virtual Tag Lang8_Adj { get; set; }
        public virtual Tag KheHo_Adj { get; set; }
        public virtual Tag Dao1_Max { get; set; }
        public virtual Tag Dao2_Max { get; set; }
        public virtual Tag Dao_Dao { get; set; }
        public virtual Tag Lang1_Min { get; set; }
        public virtual Tag Lang1_Max { get; set; }
        public virtual Tag Lang2_Min { get; set; }
        public virtual Tag Lang2_Max { get; set; }
        public virtual Tag Lang3_Min { get; set; }
        public virtual Tag Lang3_Max { get; set; }
        public virtual Tag Lang4_Min { get; set; }
        public virtual Tag Lang4_Max { get; set; }
        public virtual Tag Lang_Lang { get; set; }
        public virtual Tag Setting1 { get; set; }
        public virtual Tag Setting2 { get; set; }
        public virtual Tag Setting3 { get; set; }
        public virtual Tag Setting4 { get; set; }
        public virtual Tag Setting5 { get; set; }
        public virtual Tag Setting6 { get; set; }
        public virtual Tag Setting7 { get; set; }
        public virtual Tag Setting8 { get; set; }
        public virtual Tag Setting9 { get; set; }
        public virtual Tag MaiDao1_PV { get; set; }
        public virtual Tag MaiDao1_SV { get; set; }
        public virtual Tag MaiDao1_TT { get; set; }
        public virtual Tag MaiDao2_PV { get; set; }
        public virtual Tag MaiDao2_SV { get; set; }
        public virtual Tag MaiDao2_TT { get; set; }
        public virtual Tag MaiDao3_PV { get; set; }
        public virtual Tag MaiDao3_SV { get; set; }
        public virtual Tag MaiDao3_TT { get; set; }
        public virtual Tag MaiDao4_PV { get; set; }
        public virtual Tag MaiDao4_SV { get; set; }
        public virtual Tag MaiDao4_TT { get; set; }
        public virtual Tag MaiDao5_PV { get; set; }
        public virtual Tag MaiDao5_SV { get; set; }
        public virtual Tag MaiDao5_TT { get; set; }
        public virtual Tag Dao1_L { get; set; }
        public virtual Tag Dao1_R { get; set; }
        public virtual Tag Dao2_L { get; set; }
        public virtual Tag Dao2_R { get; set; }
        public virtual Tag Dao3_L { get; set; }
        public virtual Tag Dao3_R { get; set; }
        public virtual Tag Dao4_L { get; set; }
        public virtual Tag Dao4_R { get; set; }
        public virtual Tag Dao5_L { get; set; }
        public virtual Tag Dao5_R { get; set; }
        public virtual Tag Lang1_L { get; set; }
        public virtual Tag Lang1_R { get; set; }
        public virtual Tag Lang2_L { get; set; }
        public virtual Tag Lang2_R { get; set; }
        public virtual Tag Lang3_L { get; set; }
        public virtual Tag Lang3_R { get; set; }
        public virtual Tag Lang4_L { get; set; }
        public virtual Tag Lang4_R { get; set; }
        public virtual Tag Lang5_L { get; set; }
        public virtual Tag Lang5_R { get; set; }
        public virtual Tag Lang6_L { get; set; }
        public virtual Tag Lang6_R { get; set; }
        public virtual Tag Lang7_L { get; set; }
        public virtual Tag Lang7_R { get; set; }
        public virtual Tag Lang8_L { get; set; }
        public virtual Tag Lang8_R { get; set; }

        public virtual Tag KheHo_Inc { get; set; }
        public virtual Tag KheHo_Dec { get; set; }

        public virtual TrangThaiDao TrangThaiLang1 { get; set; }
        public virtual TrangThaiDao TrangThaiLang2 { get; set; }
        public virtual TrangThaiDao TrangThaiLang3 { get; set; }
        public virtual TrangThaiDao TrangThaiLang4 { get; set; }
        public virtual TrangThaiDao TrangThaiLang5 { get; set; }
        public virtual TrangThaiDao TrangThaiLang6 { get; set; }
        public virtual TrangThaiDao TrangThaiLang7 { get; set; }
        public virtual TrangThaiDao TrangThaiLang8 { get; set; }

        public virtual TrangThaiDao TrangThaiDao1 { get; set; }
        public virtual TrangThaiDao TrangThaiDao2 { get; set; }
        public virtual TrangThaiDao TrangThaiDao3 { get; set; }
        public virtual TrangThaiDao TrangThaiDao4 { get; set; }
        public virtual TrangThaiDao TrangThaiDao5 { get; set; }

        public DispatcherTimer timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public TagContainerBase()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            try
            {
                TrangThaiDao1 = GetTrangThaiDao(Run.Value, Dao1_U.Value);
                TrangThaiDao2 = GetTrangThaiDao(Run.Value, Dao2_U.Value);
                TrangThaiDao3 = GetTrangThaiDao(Run.Value, Dao3_U.Value);
                TrangThaiDao4 = GetTrangThaiDao(Run.Value, Dao4_U.Value);
                TrangThaiDao5 = GetTrangThaiDao(Run.Value, Dao5_U.Value);

                TrangThaiLang1 = GetTrangThaiDao(Run.Value, Lang1_U.Value);
                TrangThaiLang2 = GetTrangThaiDao(Run.Value, Lang2_U.Value);
                TrangThaiLang3 = GetTrangThaiDao(Run.Value, Lang3_U.Value);
                TrangThaiLang4 = GetTrangThaiDao(Run.Value, Lang4_U.Value);
                TrangThaiLang5 = GetTrangThaiDao(Run.Value, Lang5_U.Value);
                TrangThaiLang6 = GetTrangThaiDao(Run.Value, Lang6_U.Value);
                TrangThaiLang7 = GetTrangThaiDao(Run.Value, Lang7_U.Value);
                TrangThaiLang8 = GetTrangThaiDao(Run.Value, Lang8_U.Value);
            }
            catch { }
            finally { timer.Start(); }
        }

        public TrangThaiDao GetTrangThaiDao(double runValue, double daoRunValue)
        {
            if (runValue != 0 && daoRunValue != 0)
                return TrangThaiDao.Down;   
            return TrangThaiDao.Up;
        }

        public virtual void RegisterEvent()
        {
            if (LenhChuyenDon != null)
            {
                LenhChuyenDon.ValueChanged += LenhChuyenDon_ValueChanged;
            }
        }

        private void LenhChuyenDon_ValueChanged(Tag arg1, double oldValue, double newValue)
        {
            if (LenhChuyenDon.Value != 0)
            {
                Messenger.Default.Send(new LenhChuyenDonMessage((long)STT1.Value, this));
            }
        }

        public virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
