namespace DM.Web.SL.Common.Core.DragDrop
{
    /// <summary>
    ///   Internal classes used to represent boxed values of type <see cref = "AllowDrop" />
    /// </summary>
    public class AllowDropBoxes
    {
        #region Fields

        internal static readonly object Inherited = AllowDrop.Inherited;
        internal static readonly object False = AllowDrop.False;
        internal static readonly object True = AllowDrop.True;

        #endregion

        public static object Box(AllowDrop value)
        {
            return value == AllowDrop.Inherited ? Inherited : (value == AllowDrop.True ? True : False);
        }
    }
}