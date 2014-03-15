namespace DM.Web.SL.Controls.Guide
{
    /// <summary>
    /// 父对象接口
    /// </summary>
    public interface IProjectable
    {
        #region Properties

        /// <summary>
        /// 父对象
        /// </summary>
        GuideBase ParentMap
        {
            get;
        }

        #endregion Properties
    }
}