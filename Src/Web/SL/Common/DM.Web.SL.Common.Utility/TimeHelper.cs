using System;

namespace DM.Web.SL.Common.Utility
{
    /// <summary>
    /// 时间格式转换
    /// </summary>
    public class TimeHelper
    {
        #region Methods

        /// <summary>
        /// 返回时间差
        /// </summary>
        /// <param name="dateTime1">DateTime1</param>
        /// <param name="dateTime2">DateTime2</param>
        /// <returns></returns>
        public static string DateDiff( DateTime dateTime1, DateTime dateTime2 )
        {
            string dateDiff;
            //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            //TimeSpan ts = ts1.Subtract(ts2).Duration();
            TimeSpan ts = dateTime2 - dateTime1;
            if ( ts.Days >= 1 )
            {
                dateDiff = dateTime1.Month + "月" + dateTime1.Day + "日";
            }
            else
            {
                if ( ts.Hours > 1 )
                {
                    dateDiff = ts.Hours + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes + "分钟前";
                }
            }
            return dateDiff;
        }

        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate( int year, int month )
        {
            DateTime lastDay = new DateTime( year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth( year, month ) );
            int day = lastDay.Day;
            return day;
        }

        /// <summary>
        /// 把分钟转换成秒
        /// </summary>
        /// <param name="minute">分钟数</param>
        /// <returns>秒数</returns>
        public static int MinuteToSecond( int minute )
        {
            double mm = (double)( (decimal)minute * 60 );
            return Convert.ToInt32( Math.Ceiling( mm ) );
        }

        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <param name="second">秒数</param>
        /// <returns>分钟数</returns>
        public static int SecondToMinute( int second )
        {
            double mm = (double)( second / (decimal)60 );
            return Convert.ToInt32( Math.Ceiling( mm ) );
        }

        /// <summary>
        /// 时间转换为字符串(默认yyyy-MM-ddTHH:mm:ss)
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static string DateTimeToString( DateTime dateTime )
        {
            return DateTimeToString( "yyyy-MM-ddTHH:mm:ss", dateTime );
        }

        /// <summary>
        /// 时间转换为字符串
        /// </summary>
        /// <param name="fromat">格式</param>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static string DateTimeToString( string fromat, DateTime dateTime )
        {
            return string.Format( "{0:" + fromat + "}", dateTime );
        }

        /// <summary>
        /// 百纳秒转化为秒
        /// </summary>
        /// <param name="d100Ns">百纳秒</param>
        /// <returns>秒</returns>
        public static double D100NsToSecond( double d100Ns )
        {
            return d100Ns / Math.Pow( 10, 7 );
        }

        /// <summary>
        /// 秒转化为百纳秒
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns>百纳秒</returns>
        public static double SecondToD100Ns( double seconds )
        {
            return seconds * Math.Pow( 10, 7 );
        }

        /// <summary>
        /// 秒转换为时码
        /// </summary>
        /// <param name="fromat">格式</param>
        /// <param name="seconds">秒数</param>
        /// <returns></returns>
        public static string SecondToDateTimeString( string fromat, int seconds )
        {
            DateTime dateTime = new DateTime( 0, 0, 0, 0, 0, seconds );
            return DateTimeToString( fromat, dateTime );
        }

        /// <summary>
        /// 秒转换为时间字符串(默认HH:mm:ss)
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns>HH:mm:ss格式的时码</returns>
        public static string SecondToTimeString( int seconds )
        {
            int hours = Convert.ToInt32( ( seconds / 3600 ) );
            int minutes = Convert.ToInt32( ( seconds - 3600 * hours ) / 60 );
            int iseconds = Convert.ToInt32( seconds - ( 60 * minutes ) - ( 3600 * hours ) );

            string hoursStr = hours < 10 ? "0" + hours : hours.ToString();
            string minutesStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            string isecondsStr = iseconds < 10 ? "0" + iseconds : iseconds.ToString();
            return string.Format( "{0}:{1}:{2}", hoursStr, minutesStr, isecondsStr );
        }

        /// <summary>
        /// 秒转换为时间字符串专为音频时长计算使用(默认HH:mm:ss:ms)
        /// </summary>
        /// <param name="durationSeconds">秒数</param>
        /// <returns>HH:mm:ss:ms格式的时码</returns>
        public static string SecondToTimeStringForDuration( double durationSeconds )
        {
            //durationSeconds = Math.Ceiling( durationSeconds * 10 ) / 10;
            return SecondToTimeString( durationSeconds );
        }

        /// <summary>
        /// 秒转换为时间字符串(默认HH:mm:ss:ms)
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <returns>HH:mm:ss:ms格式的时码</returns>
        public static string SecondToTimeString( double seconds )
        {
            int hours = (int) (seconds / 3600);
            int minutes = (int) ((seconds - 3600 * hours) / 60);
            int iseconds = (int) (seconds - (60 * minutes) - (3600 * hours));

            decimal i100Milliseconds = (((decimal) (seconds * 10)) / 10 - (int) seconds) * 10;

            if (i100Milliseconds >= 10)
            {
                iseconds++;
                i100Milliseconds = i100Milliseconds - 10;
            }

            string hoursStr = hours < 10 ? "0" + hours : hours.ToString();
            string minutesStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            string isecondsStr = iseconds < 10 ? "0" + iseconds : iseconds.ToString();
            string i100MillisecondsStr = i100Milliseconds.ToString("0.0").Substring(0, 1);
            return string.Format("{0}:{1}:{2}:{3}", hoursStr, minutesStr, isecondsStr, i100MillisecondsStr);
        }

        #endregion Methods
    }
}