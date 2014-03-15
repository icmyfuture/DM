// -------------------------
// .Author : xiaole
// .Email : bardway@live.com
// -------------------------
namespace Carrier.Utility
{
    #region Imports

    using System.Runtime.InteropServices;

    #endregion

    ///<summary>
    ///  snmp客户端引入
    ///</summary>
    internal static class SNMPClient
    {
        ///<summary>
        ///  FOR INT
        ///</summary>
        ///<param name = "id"></param>
        ///<param name = "row"></param>
        ///<param name = "value"></param>
        ///<returns></returns>
        [DllImport("V1Client.dll", CharSet = CharSet.Auto)]
        internal static extern bool SnmpWriteItemInt(string id, int row, int value);

        ///<summary>
        ///  FOR LONG
        ///</summary>
        ///<param name = "id"></param>
        ///<param name = "row"></param>
        ///<param name = "value"></param>
        ///<returns></returns>
        [DllImport("V1Client.dll", CharSet = CharSet.Auto)]
        internal static extern bool SnmpWriteItemLong(string id, int row, long value);

        ///<summary>
        ///  FOR STRING
        ///</summary>
        ///<param name = "id"></param>
        ///<param name = "row"></param>
        ///<param name = "value"></param>
        ///<returns></returns>
        [DllImport("V1Client.dll", CharSet = CharSet.Auto)]
        internal static extern bool SnmpWriteItemString(string id, int row, string value);
    }
}