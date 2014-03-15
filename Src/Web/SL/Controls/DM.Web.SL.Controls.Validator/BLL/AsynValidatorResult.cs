using System;
using System.Windows;

namespace DM.Web.SL.Controls.Validator.BLL
{
    /// <summary>
    /// 异步验证返回结果。
    /// </summary>
    public class AsynValidatorResult:RoutedEventArgs
    {
        /// <summary>
        /// 验证文本。
        /// </summary>
        public string ValidatorText { get; set; }

        /// <summary>
        /// 获取验证结果事件。
        /// </summary>
        public EventHandler GetResult;
        
        private bool status = true;
        /// <summary>
        /// 验证状态。
        /// </summary>
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
