using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DM.Web.SL.Controls.Validator.DefaultIndicator;

namespace DM.Web.SL.Controls.Validator.BLL
{
    /// <summary>
    /// 表示一个验证分组，提供验证控制功能。
    /// </summary>
    public class ValidatorManager : FrameworkElement,IDisposable
    {
        #region Fields

        /// <summary>
        /// 当前注册的验证项。
        /// </summary>
        private readonly List<ValidatorBase> _regItems = new List<ValidatorBase>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造
        /// </summary>
        public ValidatorManager()
        {
            Indicator = new DefaultIndicator.DefaultIndicator();
            InvalidBorder = new SolidColorBrush(Colors.Red);
            InvalidBorderThickness = new Thickness(1);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 获取或设置提示控件。
        /// </summary>
        public IIndicator Indicator
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置无效状态的背景色。
        /// </summary>
        public Brush InvalidBackground
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置无效状态的边框。
        /// </summary>
        public Brush InvalidBorder
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置无效状态的边框的厚度。
        /// </summary>
        public Thickness? InvalidBorderThickness
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置是否当按键弹起的时候验证控件。
        /// </summary>
        public bool ValidataWhenKeyUp { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 注册一个验证项。
        /// </summary>
        /// <param name="validatorBase">验证器。</param>
        public void Register(ValidatorBase validatorBase)
        {
            var find = _regItems.FirstOrDefault(i => i.Key == validatorBase.Key && !string.IsNullOrEmpty(i.Key) );
            if (find != null)
                throw new ArgumentException("Validator can not have duplicate key:" + validatorBase.Key);

            _regItems.Add(validatorBase);
            validatorBase.IsValidChanged += validatorBase_IsValidChanged;
        }

        /// <summary>
        /// 执行验证，判断该分组下的验证控件是否都通过验证。
        /// </summary>
        /// <returns>都通过验证返回true，否则返回false。</returns>
        public bool Validate()
        {
            if (ValidateAll().Count > 0)
                return false;
            return true;
        }

        /// <summary>
        /// 执行验证，返回未通过的验证器列表。
        /// </summary>
        /// <returns>返回未通过的验证器列表。</returns>
        public List<ValidatorBase> ValidateAll()
        {
            List<ValidatorBase> errors = new List<ValidatorBase>();

            foreach (var item in _regItems)
            {
                if (!item.Validate())
                    errors.Add(item);
            }
            return errors;
        }

        /// <summary>
        /// 获取指定的验证控件。
        /// </summary>
        /// <param name="key">验证控件的Key属性。</param>
        /// <returns>返回获取的验证控件，未找到返回null。</returns>
        public ValidatorBase GetValidator(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "GetValidator method param 'key' can not be null or string.Empty.");

            var find = _regItems.FirstOrDefault(i => i.Key == key);
            if (find != null)
                return find;
            return null;
        }

        /// <summary>
        /// 验证状态改变事件。
        /// </summary>
        private void validatorBase_IsValidChanged(object sender, RoutedEventArgs e)
        {
            ValidatorBase validatorBase = (ValidatorBase) sender;

            var textBoxValidators = (from item in _regItems
                        where item.ElementToValidate == validatorBase.ElementToValidate
                        select item).ToList();

            var validList = (from item in textBoxValidators
                             where item.IsValid
                             select item).ToList();

            var inValidList = (from item in textBoxValidators
                             where !item.IsValid
                             select item).ToList();

            if (inValidList.Count == 0)
            {
                validList.ForEach(i => i.SetValidStyle());
            }
            else
            {
                textBoxValidators.ForEach(i => i.SetValidStyle());
                inValidList[0].SetInvalidStyle();
            }
        }

        #endregion Methods

        #region IDisposable Members

        public void Dispose()
        {
            Indicator = null;
            InvalidBorder = null;
            InvalidBorderThickness = null;
        }

        #endregion
    }
}