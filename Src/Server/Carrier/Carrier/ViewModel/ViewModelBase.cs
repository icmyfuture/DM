using System;
using System.ComponentModel;

namespace Carrier.ViewModel
{
    /// <summary>
    ///   VIEWMODEL基类
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region IDisposable Members

        /// <summary>
        ///   释放
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        ///   属性改变引发器
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        ///   属性改变
        /// </summary>
        /// <param name = "propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler == null)
            {
                return;
            }
            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        /// <summary>
        ///   释放方法
        /// </summary>
        protected abstract void OnDispose();
    }
}