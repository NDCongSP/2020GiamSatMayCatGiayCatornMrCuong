using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MayCat
{
    public class CaiDatKheHoLangViewModel
    {
        public virtual object SelectedItem { get; set; }
        public virtual ObservableCollection<KheHoLang> Source { get; set; }
        public ICurrentWindowService CurrentWindowService { get => this.GetService<ICurrentWindowService>(); }

        public CaiDatKheHoLangViewModel()
        {
            Source = Helper.DanhSachKheHoLang;
            SelectedItem = Source.FirstOrDefault();
        }

        public void Save()
        {
            try
            {
                File.WriteAllText("KheHoLang.json", JsonConvert.SerializeObject(Source));
                MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool CanSave()
        {
            return Source != null;
        }

        public virtual void Create()
        {
            var item = new KheHoLang();
            Source.Add(item);
        }

        public virtual bool CanCreate()
        {
            return Source != null;
        }

        public virtual void Delete()
        {
            if (SelectedItem is KheHoLang kheHoLang)
            {
                if (Source.Contains(kheHoLang))
                {
                    var mbr = MessageBox.Show("Bạn có muốn xóa hàng này hay không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        Source.Remove(kheHoLang);
                        SelectedItem = Source.FirstOrDefault();
                    }
                }
            }
        }

        public bool CanDelete()
        {
            return SelectedItem != null;
        }

        public virtual void Cancel()
        {
            CurrentWindowService.Close();
        }
    }
}
