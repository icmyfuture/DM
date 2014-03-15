namespace DM.Web.SL.Controls.Validator.Filters
{
    #region Enumerations

    /// <summary>
    /// 过滤的类型。
    /// </summary>
    public enum TextBoxFilterType
    {
        /// <summary>
        /// 无。
        /// </summary>
        None,

        /// <summary>
        /// 正整数。
        /// </summary>
        PositiveInteger,

        /// <summary>
        /// 整数。
        /// </summary>
        Integer,

        /// <summary>
        /// 正Decimal。
        /// </summary>
        PositiveDecimal,

        /// <summary>
        /// Decimal。
        /// </summary>
        Decimal,

        /// <summary>
        /// Alpha。
        /// </summary>
        Alpha,
    }

    #endregion Enumerations
}