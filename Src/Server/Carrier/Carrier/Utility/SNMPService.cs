// -------------------------
// .Author : xiaole
// .Email : bardway@live.com
// -------------------------
namespace Carrier.Utility
{
    ///<summary>
    ///  SNMP服务
    ///</summary>
    internal static class SNMPService
    {
        private const int Row = 0;

        ///<summary>
        ///  发送信息
        ///</summary>
        ///<param name = "command"></param>
        ///<returns></returns>
        public static string Write(string command)
        {
            var commands = command.Split(",".ToCharArray());
            return WriteItem(commands[0], commands[1]).ToString();
        }

        ///<summary>
        ///  发送信息
        ///</summary>
        ///<param name = "id"></param>
        ///<param name = "value"></param>
        ///<returns></returns>
        private static bool WriteItem(string id, object value)
        {
            var ret = false;
            var valuetype = value.GetType();
            switch (valuetype.FullName)
            {
                case "System.Int32":
                    ret = SNMPClient.SnmpWriteItemInt(id, Row, (int) value);
                    break;
                case "System.Int64":
                    ret = SNMPClient.SnmpWriteItemLong(id, Row, (long) value);
                    break;
                case "System.String":
                    ret = SNMPClient.SnmpWriteItemString(id, Row, (string) value);
                    break;
            }
            return ret;
        }
    }
}