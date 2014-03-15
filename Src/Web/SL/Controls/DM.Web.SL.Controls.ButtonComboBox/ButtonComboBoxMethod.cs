using System;

namespace DM.Web.SL.Controls.ButtonComboBox
{
    ///<summary>
    /// ButtonComboBox属性
    ///</summary>
    public class ButtonComboBoxMethod
    {
        /// <summary>
        /// 下拉列表中变量
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 下拉列表中对应方法
        /// </summary>
        public EventHandler ItemEventHandler { get; set; }
    }
}
