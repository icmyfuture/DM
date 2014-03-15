using System;
using DM.Client.WPF.Controls.NotifyMessage.Enum;
using DM.Common.Extensions;

namespace DM.Client.WPF.Controls.NotifyMessage.Entitys
{
    [Serializable]
    public class NotifyWindowDataError : NotifyWindowBase
    {
        public NotifyWindowDataError()
        {

        }
        public NotifyWindowDataError(long dataIndex, string firstClounmData, ErrorType errorType, string errorData)
        {
            DataIndex = dataIndex;
            FirstClounmData = firstClounmData;
            ErrorType = errorType;
            ErrorData = errorData;
        }

        public string ErrorData { get; set; }
        public long DataIndex { get; set; }
        public string FirstClounmData { get; set; }
        public ErrorType ErrorType { get; set; }
        public override string ToString()
        {
            return this.ToXml();
        }
    }
}
