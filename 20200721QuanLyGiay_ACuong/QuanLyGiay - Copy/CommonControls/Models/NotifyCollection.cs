using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CommonControls
{
    public class NotifyCollection<T> : ObservableCollection<T>
        where T : class
    {
        #region Public members

        public bool DisableNotifyChanged { get; set; }

        #endregion

        #region Constructors


        public NotifyCollection(IEnumerable<T> items) : base(items)
        {
        }

        public NotifyCollection() : base()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Hàm thêm nhiều đối tượng vào danh sách
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection) { Items.Add(item); }
            NotifyResetCollection();
        }

        /// <summary>
        /// Hàm xóa nhiều đối tượng trong danh sách
        /// </summary>
        /// <param name="collection"></param>
        public void RemoveRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                Items.Remove(item);
            NotifyResetCollection();
        }


        /// <summary>
        /// Hàm thông báo làm mối đối tượng
        /// </summary>
        public virtual void NotifyResetCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        }

        /// <summary>
        /// Hàm thông báo đối tượng cụ thể thay đổi
        /// </summary>
        /// <param name="item"></param>
        public virtual void NotifyItemInCollectionChanged(T item)
        {
            if (Contains(item))
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!DisableNotifyChanged)
            {
                base.OnCollectionChanged(e);
            }
        }

        #endregion
    }
}
