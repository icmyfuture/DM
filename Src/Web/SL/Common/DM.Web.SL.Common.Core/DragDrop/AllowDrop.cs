namespace DM.Web.SL.Common.Core.DragDrop
{
    /// <summary>
    ///   控件是否允许拖放操作
    /// </summary>
    public enum AllowDrop
    {
        /// <summary>
        ///   继承
        /// </summary>
        Inherited,
        /// <summary>
        ///   控件不接受用户拖累。
        /// </summary>
        False,
        /// <summary>
        ///   控件可以接受用户拖累。
        /// </summary>
        True
    }
}