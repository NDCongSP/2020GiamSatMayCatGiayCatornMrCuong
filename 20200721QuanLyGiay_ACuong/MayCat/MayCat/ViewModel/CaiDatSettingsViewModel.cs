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
    public class CaiDatSettingsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public virtual string May1_Setting1 { get; set; }
        public virtual string May1_Setting2 { get; set; }
        public virtual string May1_Setting3 { get; set; }
        public virtual string May1_Setting4 { get; set; }
        public virtual string May1_Setting5 { get; set; }
        public virtual string May1_Setting6 { get; set; }
        public virtual string May1_Setting7 { get; set; }
        public virtual string May1_Setting8 { get; set; }
        public virtual string May1_Setting9 { get; set; }

        public virtual string May2_Setting1 { get; set; }
        public virtual string May2_Setting2 { get; set; }
        public virtual string May2_Setting3 { get; set; }
        public virtual string May2_Setting4 { get; set; }
        public virtual string May2_Setting5 { get; set; }
        public virtual string May2_Setting6 { get; set; }
        public virtual string May2_Setting7 { get; set; }
        public virtual string May2_Setting8 { get; set; }
        public virtual string May2_Setting9 { get; set; }
        public virtual bool IsChanged { get; set; }

        public ICurrentWindowService CurrentWindowService { get => this.GetService<ICurrentWindowService>(); }

        public CaiDatSettingsViewModel()
        {
            Refresh();
            PropertyChanged += CaiDatSettingsViewModel_PropertyChanged;
        }

        private void CaiDatSettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("May1") || e.PropertyName.StartsWith("May2"))
            {
                IsChanged = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            try
            {
                uint startAddress1 = May1Tags.Instance.Setting1.Address;
                byte[] writeBuffer1 = new byte[9 * 4];
                byte[] bufferMay1_1 = Helper.GetBytes(May1Tags.Instance.Setting1, May1_Setting1);
                byte[] bufferMay1_2 = Helper.GetBytes(May1Tags.Instance.Setting2, May1_Setting2);
                byte[] bufferMay1_3 = Helper.GetBytes(May1Tags.Instance.Setting3, May1_Setting3);
                byte[] bufferMay1_4 = Helper.GetBytes(May1Tags.Instance.Setting4, May1_Setting4);
                byte[] bufferMay1_5 = Helper.GetBytes(May1Tags.Instance.Setting5, May1_Setting5);
                byte[] bufferMay1_6 = Helper.GetBytes(May1Tags.Instance.Setting6, May1_Setting6);
                byte[] bufferMay1_7 = Helper.GetBytes(May1Tags.Instance.Setting7, May1_Setting7);
                byte[] bufferMay1_8 = Helper.GetBytes(May1Tags.Instance.Setting8, May1_Setting8);
                byte[] bufferMay1_9 = Helper.GetBytes(May1Tags.Instance.Setting9, May1_Setting9);

                int index = 0;
                Array.Copy(bufferMay1_1, 0, writeBuffer1, index, bufferMay1_1.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_2, 0, writeBuffer1, index, bufferMay1_2.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_3, 0, writeBuffer1, index, bufferMay1_3.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_4, 0, writeBuffer1, index, bufferMay1_4.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_5, 0, writeBuffer1, index, bufferMay1_5.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_6, 0, writeBuffer1, index, bufferMay1_6.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_7, 0, writeBuffer1, index, bufferMay1_7.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_8, 0, writeBuffer1, index, bufferMay1_8.Length);
                index += bufferMay1_1.Length;
                Array.Copy(bufferMay1_9, 0, writeBuffer1, index, bufferMay1_9.Length);

                bool result1 = Helper.ModbusMay1.WriteRegisters(startAddress1, writeBuffer1);

                uint startAddress2 = May2Tags.Instance.Setting1.Address;
                byte[] writeBuffer2 = new byte[9 * 4];
                byte[] bufferMay2_1 = Helper.GetBytes(May2Tags.Instance.Setting1, May2_Setting1);
                byte[] bufferMay2_2 = Helper.GetBytes(May2Tags.Instance.Setting2, May2_Setting2);
                byte[] bufferMay2_3 = Helper.GetBytes(May2Tags.Instance.Setting3, May2_Setting3);
                byte[] bufferMay2_4 = Helper.GetBytes(May2Tags.Instance.Setting4, May2_Setting4);
                byte[] bufferMay2_5 = Helper.GetBytes(May2Tags.Instance.Setting5, May2_Setting5);
                byte[] bufferMay2_6 = Helper.GetBytes(May2Tags.Instance.Setting6, May2_Setting6);
                byte[] bufferMay2_7 = Helper.GetBytes(May2Tags.Instance.Setting7, May2_Setting7);
                byte[] bufferMay2_8 = Helper.GetBytes(May2Tags.Instance.Setting8, May2_Setting8);
                byte[] bufferMay2_9 = Helper.GetBytes(May2Tags.Instance.Setting9, May2_Setting9);

                index = 0;
                Array.Copy(bufferMay2_1, 0, writeBuffer2, index, bufferMay2_1.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_2, 0, writeBuffer2, index, bufferMay2_2.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_3, 0, writeBuffer2, index, bufferMay2_3.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_4, 0, writeBuffer2, index, bufferMay2_4.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_5, 0, writeBuffer2, index, bufferMay2_5.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_6, 0, writeBuffer2, index, bufferMay2_6.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_7, 0, writeBuffer2, index, bufferMay2_7.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_8, 0, writeBuffer2, index, bufferMay2_8.Length);
                index += bufferMay2_1.Length;
                Array.Copy(bufferMay2_9, 0, writeBuffer2, index, bufferMay2_9.Length);
                bool result2 = Helper.ModbusMay2.WriteRegisters(startAddress2, writeBuffer2);

                string message = result1 ? "Ghi setting máy 1 thành công !" : "Ghi setting máy 1 thất bại";
                message += "\n";
                message += result2 ? "Ghi setting máy 2 thành công !" : "Ghi setting máy 2 thất bại";
                MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Refresh();

                if (result1 && result2)
                    IsChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool CanSave()
        {
            return IsChanged && string.IsNullOrEmpty(Error);
        }

        public virtual void Cancel()
        {
            CurrentWindowService.Close();
        }

        public void Refresh()
        {
            May1_Setting1 = May1Tags.Instance.Setting1.Value.ToString();
            May1_Setting2 = May1Tags.Instance.Setting2.Value.ToString();
            May1_Setting3 = May1Tags.Instance.Setting3.Value.ToString();
            May1_Setting4 = May1Tags.Instance.Setting4.Value.ToString();
            May1_Setting5 = May1Tags.Instance.Setting5.Value.ToString();
            May1_Setting6 = May1Tags.Instance.Setting6.Value.ToString();
            May1_Setting7 = May1Tags.Instance.Setting7.Value.ToString();
            May1_Setting8 = May1Tags.Instance.Setting8.Value.ToString();
            May1_Setting9 = May1Tags.Instance.Setting9.Value.ToString();

            May2_Setting1 = May2Tags.Instance.Setting1.Value.ToString();
            May2_Setting2 = May2Tags.Instance.Setting2.Value.ToString();
            May2_Setting3 = May2Tags.Instance.Setting3.Value.ToString();
            May2_Setting4 = May2Tags.Instance.Setting4.Value.ToString();
            May2_Setting5 = May2Tags.Instance.Setting5.Value.ToString();
            May2_Setting6 = May2Tags.Instance.Setting6.Value.ToString();
            May2_Setting7 = May2Tags.Instance.Setting7.Value.ToString();
            May2_Setting8 = May2Tags.Instance.Setting8.Value.ToString();
            May2_Setting9 = May2Tags.Instance.Setting9.Value.ToString();
        }

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                Error = string.Empty;
                switch (columnName)
                {
                    case "May1_Setting1":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 1 máy 1 phải là một số.";
                        break;
                    case "May1_Setting2":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 2 máy 1 phải là một số.";
                        break;
                    case "May1_Setting3":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 3 máy 1 phải là một số.";
                        break;
                    case "May1_Setting4":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 4 máy 1 phải là một số.";
                        break;
                    case "May1_Setting5":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 5 máy 1 phải là một số.";
                        break;
                    case "May1_Setting6":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 6 máy 1 phải là một số.";
                        break;
                    case "May1_Setting7":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 7 máy 1 phải là một số.";
                        break;
                    case "May1_Setting8":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 8 máy 1 phải là một số.";
                        break;
                    case "May1_Setting9":
                        if (!May1_Setting1.IsDWord())
                            Error = "Setting 9 máy 1 phải là một số.";
                        break;

                    case "May2_Setting1":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 1 máy 2 phải là một số.";
                        break;
                    case "May2_Setting2":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 2 máy 2 phải là một số.";
                        break;
                    case "May2_Setting3":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 3 máy 2 phải là một số.";
                        break;
                    case "May2_Setting4":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 4 máy 2 phải là một số.";
                        break;
                    case "May2_Setting5":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 5 máy 2 phải là một số.";
                        break;
                    case "May2_Setting6":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 6 máy 2 phải là một số.";
                        break;
                    case "May2_Setting7":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 7 máy 2 phải là một số.";
                        break;
                    case "May2_Setting8":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 8 máy 2 phải là một số.";
                        break;
                    case "May2_Setting9":
                        if (!May2_Setting1.IsDWord())
                            Error = "Setting 9 máy 2 phải là một số.";
                        break;

                    default:
                        break;
                }
                return Error;
            }
        }


    }
}
