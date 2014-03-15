using System;
using System.Collections.Generic;
using DM.Common.Service.Interfaces;
using DM.Common.Utility.Log;

namespace DM.Common.Service.Helpers
{
    /// <summary>
    /// 分发辅助
    /// </summary>
    public class DispatcherHelper
    {
        /// <summary>
        /// 添加命令执行器
        /// </summary>
        /// <param name="dic">执行器集合</param>
        /// <param name="ce">执行器</param>
        public static void AddCommandExecutor(Dictionary<string, ICommandExecutor> dic, ICommandExecutor ce)
        {
            try
            {
                if (ce != null && dic != null)
                {
                    if (!dic.ContainsKey(ce.GetCommandName()))
                    {
                        dic.Add(ce.GetCommandName(), ce);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ConfigHelper.ModuleName, ex);
            }
        }
    }
}