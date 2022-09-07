using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MayCat
{
    public class CaiDatMayCatViewModel : INotifyPropertyChanged, IDataErrorInfo, ISupportParentViewModel
    {
        public virtual double Lang1_Adj { get; set; }
        public virtual double Lang2_Adj { get; set; }
        public virtual double Lang3_Adj { get; set; }
        public virtual double Lang4_Adj { get; set; }
        public virtual double Lang5_Adj { get; set; }
        public virtual double Lang6_Adj { get; set; }
        public virtual double Lang7_Adj { get; set; }
        public virtual double Lang8_Adj { get; set; }
        public virtual object ParentViewModel { get; set; }

        public virtual double Dao1_Adj { get; set; }
        public virtual double Dao2_Adj { get; set; }
        public virtual double Dao3_Adj { get; set; }
        public virtual double Dao4_Adj { get; set; }
        public virtual double Dao5_Adj { get; set; }

        public virtual Tag Lang1_PV { get; set; }
        public virtual Tag Lang2_PV { get; set; }
        public virtual Tag Lang3_PV { get; set; }
        public virtual Tag Lang4_PV { get; set; }
        public virtual Tag Lang5_PV { get; set; }
        public virtual Tag Lang6_PV { get; set; }
        public virtual Tag Lang7_PV { get; set; }
        public virtual Tag Lang8_PV { get; set; }

        public virtual Tag Dao1_PV { get; set; }
        public virtual Tag Dao2_PV { get; set; }
        public virtual Tag Dao3_PV { get; set; }
        public virtual Tag Dao4_PV { get; set; }
        public virtual Tag Dao5_PV { get; set; }

        public virtual string DaoDao { get; set; }
        public virtual string LangLang { get; set; }
        public virtual string Dao1Max { get; set; }
        public virtual string Dao2Max { get; set; }
        public virtual string Lang1Max { get; set; }
        public virtual string Lang2Max { get; set; }
        public virtual string Lang3Max { get; set; }
        public virtual string Lang4Max { get; set; }

        public virtual string Lang1Min { get; set; }
        public virtual string Lang2Min { get; set; }
        public virtual string Lang3Min { get; set; }
        public virtual string Lang4Min { get; set; }

        public virtual Tag ChieuDaiDao1 { get; set; }
        public virtual Tag ChieuDaiDao2 { get; set; }
        public virtual Tag ChieuDaiDao3 { get; set; }
        public virtual Tag ChieuDaiDao4 { get; set; }
        public virtual Tag ChieuDaiDao5 { get; set; }

        public virtual string CaiDatDao1 { get; set; }
        public virtual string CaiDatDao2 { get; set; }
        public virtual string CaiDatDao3 { get; set; }
        public virtual string CaiDatDao4 { get; set; }
        public virtual string CaiDatDao5 { get; set; }
      
        public virtual Tag ChieuDaiDaChayDao1 { get; set; }
        public virtual Tag ChieuDaiDaChayDao2 { get; set; }
        public virtual Tag ChieuDaiDaChayDao3 { get; set; }
        public virtual Tag ChieuDaiDaChayDao4 { get; set; }
        public virtual Tag ChieuDaiDaChayDao5 { get; set; }

        public virtual double KheHoAdj { get; set; }
        public virtual double HutAdj { get; set; }

        public TagContainerBase TagContainer { get; set; }

        public virtual bool ChoPhepChayMay { get; set; }
        public virtual bool IsChanged { get; set; }

        public ICurrentWindowService CurrentWindowService { get => this.GetService<ICurrentWindowService>(); }


        public CaiDatMayCatViewModel(TagContainerBase tagContainer)
        {
            TagContainer = tagContainer;
            Refresh();
            ChoPhepChayMay = tagContainer.ChoPhepMayChay;
            PropertyChanged += CaiDatMayCatViewModel_PropertyChanged;
        }

        private void CaiDatMayCatViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsChanged = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Cancel()
        {
            CurrentWindowService.Close();
        }

        public void Refresh()
        {
            Lang1_Adj = TagContainer.Lang1_Adj.Value;
            Lang2_Adj = TagContainer.Lang2_Adj.Value;
            Lang3_Adj = TagContainer.Lang3_Adj.Value;
            Lang4_Adj = TagContainer.Lang4_Adj.Value;
            Lang5_Adj = TagContainer.Lang5_Adj.Value;
            Lang6_Adj = TagContainer.Lang6_Adj.Value;
            Lang7_Adj = TagContainer.Lang7_Adj.Value;
            Lang8_Adj = TagContainer.Lang8_Adj.Value;

            Dao1_Adj = TagContainer.Dao1_Adj.Value;
            Dao2_Adj = TagContainer.Dao2_Adj.Value;
            Dao3_Adj = TagContainer.Dao3_Adj.Value;
            Dao4_Adj = TagContainer.Dao4_Adj.Value;
            Dao5_Adj = TagContainer.Dao5_Adj.Value;

            Lang1_PV = TagContainer.Lang1_PV;
            Lang2_PV = TagContainer.Lang2_PV;
            Lang3_PV = TagContainer.Lang3_PV;
            Lang4_PV = TagContainer.Lang4_PV;
            Lang5_PV = TagContainer.Lang5_PV;
            Lang6_PV = TagContainer.Lang6_PV;
            Lang7_PV = TagContainer.Lang7_PV;
            Lang8_PV = TagContainer.Lang8_PV;

            Dao1_PV = TagContainer.Dao1_PV;
            Dao2_PV = TagContainer.Dao2_PV;
            Dao3_PV = TagContainer.Dao3_PV;
            Dao4_PV = TagContainer.Dao4_PV;
            Dao5_PV = TagContainer.Dao5_PV;

            DaoDao = TagContainer.Dao_Dao.Value.ToString();
            LangLang = TagContainer.Lang_Lang.Value.ToString();
            Dao1Max = TagContainer.Dao1_Max.Value.ToString();
            Dao2Max = TagContainer.Dao2_Max.Value.ToString();
            Lang1Max = TagContainer.Lang1_Max.Value.ToString();
            Lang2Max = TagContainer.Lang2_Max.Value.ToString();
            Lang3Max = TagContainer.Lang3_Max.Value.ToString();
            Lang4Max = TagContainer.Lang4_Max.Value.ToString();
            Lang1Min = TagContainer.Lang1_Min.Value.ToString();
            Lang2Min = TagContainer.Lang2_Min.Value.ToString();
            Lang3Min = TagContainer.Lang3_Min.Value.ToString();
            Lang4Min = TagContainer.Lang4_Min.Value.ToString();

            ChieuDaiDao1 = TagContainer.MaiDao1_PV;
            ChieuDaiDao2 = TagContainer.MaiDao2_PV;
            ChieuDaiDao3 = TagContainer.MaiDao3_PV;
            ChieuDaiDao4 = TagContainer.MaiDao4_PV;
            ChieuDaiDao5 = TagContainer.MaiDao5_PV;

            CaiDatDao1 = TagContainer.MaiDao1_SV.Value.ToString();
            CaiDatDao2 = TagContainer.MaiDao2_SV.Value.ToString();
            CaiDatDao3 = TagContainer.MaiDao3_SV.Value.ToString();
            CaiDatDao4 = TagContainer.MaiDao4_SV.Value.ToString();
            CaiDatDao5 = TagContainer.MaiDao5_SV.Value.ToString();

            ChieuDaiDaChayDao1 = TagContainer.MaiDao1_TT;
            ChieuDaiDaChayDao2 = TagContainer.MaiDao2_TT;
            ChieuDaiDaChayDao3 = TagContainer.MaiDao3_TT;
            ChieuDaiDaChayDao4 = TagContainer.MaiDao4_TT;
            ChieuDaiDaChayDao5 = TagContainer.MaiDao5_TT;

            KheHoAdj = TagContainer.KheHo_Adj.Value;
            HutAdj = TagContainer.Hut_Adj.Value;

            this.RaisePropertiesChanged();
            IsChanged = false;
        }

        public void ResetDao(object param)
        {
            try
            {
                var mbr = MessageBox.Show($"Bạn có muốn reset dao {param} hay không ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool res = false;
                    switch (param.ToString())
                    {
                        case "1":
                            res = ChieuDaiDaChayDao1.Write(0);
                            break;
                        case "2":
                            res = ChieuDaiDaChayDao2.Write(0);
                            break;
                        case "3":
                            res = ChieuDaiDaChayDao3.Write(0);
                            break;
                        case "4":
                            res = ChieuDaiDaChayDao4.Write(0);
                            break;
                        case "5":
                            res = ChieuDaiDaChayDao5.Write(0);
                            break;
                        default:
                            break;
                    }

                    if (res)
                        MessageBox.Show($"Reset {param} thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show($"Reset {param} thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public void Save()
        {
            try
            {
                bool res1 = TagContainer.Hut_Adj.Write(HutAdj);
                bool res2 = TagContainer.KheHo_Adj.Write(KheHoAdj);
                
                if (TagContainer is May1Tags)
                {
                    Properties.Settings.Default.ChoPhepChayMay1 = ChoPhepChayMay;
                    TagContainer.ChoPhepMayChay = ChoPhepChayMay;
                }
                else
                {
                    Properties.Settings.Default.ChoPhepChayMay2 = ChoPhepChayMay;
                    TagContainer.ChoPhepMayChay = ChoPhepChayMay;
                }
                Properties.Settings.Default.Save();

                byte[] writeBuffers = new byte[68];
                int index = 0;
                byte[] temp = null;
                temp = Helper.GetBytes(TagContainer.Dao1_Max, Dao1Max);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Dao2_Max, Dao2Max);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Dao_Dao, DaoDao);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang1_Min, Lang1Min);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang1_Max, Lang1Max);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang2_Min, Lang2Min);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang2_Max, Lang2Max);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang3_Min, Lang3Min);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang3_Max, Lang3Max);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang4_Min, Lang4Min);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang4_Max, Lang4Max);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.Lang_Lang, LangLang);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.MaiDao1_SV, CaiDatDao1);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.MaiDao2_SV, CaiDatDao2);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.MaiDao3_SV, CaiDatDao3);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.MaiDao4_SV, CaiDatDao4);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                temp = Helper.GetBytes(TagContainer.MaiDao5_SV, CaiDatDao5);
                Array.Copy(temp, 0, writeBuffers, index, temp.Length);
                index += temp.Length;

                bool res3 = TagContainer.ModbusReader.WriteRegisters(TagContainer.Dao1_Max.Address, writeBuffers);

                if (res1 && res2 && res3)
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanSave()
        {
            return TagContainer != null && TagContainer.Cao1.Quality == Quality.Good && IsChanged;
        }

        public void Adjust(object param)
        {
            try
            {
                string name = param?.ToString();
                if (TagContainer.TagSource.FirstOrDefault(x => x.Name == name) is Tag tag)
                {
                    if (tag.Quality == Quality.Good)
                    {
                        object writeValue = this.GetType().GetProperty(name).GetValue(this);
                        if (tag.Write(writeValue))
                            MessageBox.Show($"Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show($"Cập nhật thất bại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CanAddjust(object param)
        {
            return TagContainer != null && TagContainer.Cao1.Quality == Quality.Good;
        }

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                switch (columnName)
                {
                    default:
                        break;
                }
                return Error;
            }
        }

    }
}
