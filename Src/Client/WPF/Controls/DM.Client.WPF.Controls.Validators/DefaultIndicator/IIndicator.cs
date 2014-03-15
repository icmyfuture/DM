namespace DM.Client.WPF.Controls.Validators.DefaultIndicator
{
    /// <summary>
    /// 验证提示控件的接口。
    /// </summary>
    public interface IIndicator
    {
        #region Methods
            
        /// <summary>
        /// 设置错误信息。
        /// </summary>
        /// <param name="errMessage"></param>
        void SetErrMessage(string errMessage);

        #endregion Methods
    }
}