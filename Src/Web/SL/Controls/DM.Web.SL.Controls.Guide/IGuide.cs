namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 向导接口
    /// </summary>
    public interface IGuide
    {
        #region Properties

        /// <summary>
        /// 向导父类
        /// </summary>
        GuideBase ParentGuide
        {
            set;
            get;
        }

        #endregion Properties
    }
}