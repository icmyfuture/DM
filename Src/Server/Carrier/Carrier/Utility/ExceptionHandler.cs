using System;
using DM.Common.Utility.Log;

namespace Carrier.Utility
{
    /// <summary>
    ///   异常控制
    /// </summary>
    internal static class ExceptionHandler
    {
        public static string HandlException(this Exception exception)
        {
            var exmessage = exception.InnerException != null
                                ? exception.InnerException.Message
                                : exception.Message;
            LogHelper.ServerName = "Carrier";
            LogHelper.Writelog(LogType.Debug, "Carrier", string.Format("【carrier error】 -> {0}", exception));
            return exmessage + "\r\n";

        }
    }
}