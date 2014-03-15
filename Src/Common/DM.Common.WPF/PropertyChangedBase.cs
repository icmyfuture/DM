using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace DM.Common.WPF
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                  new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class PropertyChangedBaseEx
    {
        public static void NotifyPropertyChangedEx<T, TProperty>(this T propertyChangedBase,
          Expression<Func<T, TProperty>> expression) where T : PropertyChangedBase
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                propertyChangedBase.NotifyPropertyChanged(propertyName);
            }
        }
    }
}
