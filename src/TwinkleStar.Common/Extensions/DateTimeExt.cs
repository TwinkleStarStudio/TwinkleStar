using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwinkleStar.Common.Extensions
{
    public static class DateTimeExt
    {
        #region 转换
        /// <summary>
        /// 转换为yyyyMMddHHmmss长整型
        /// </summary>
        /// <param name="dt">datetime</param>
        /// <returns>long</returns>
        public static long ToLong(this DateTime dt)
        {
            return Convert.ToInt64(dt.ToString("yyyyMMddHHmmss"));
        }

        /// <summary>
        /// 转换为yyyyMMdd整型
        /// </summary>
        /// <param name="dt">datetime</param>
        /// <returns>int</returns>
        public static int ToInt(this DateTime dt)
        {
            return Convert.ToInt32(dt.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 转换为yyyy年MM月dd日 HH时mm分ss秒
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="IsShowSecond">是否显示秒</param>
        /// <returns></returns>
        public static string ToChineseDate(this DateTime dt, bool IsShowSecond = false)
        {
            if (IsShowSecond)
            {
                return dt.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            }
            else
            {
                return dt.ToString("yyyy年MM月dd日 HH时mm分");
            }
        }

        /// <summary>
        /// 转换为中式yyyy-MM-dd HH:mm:ss格式
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="IsShowSecond">是否显示秒</param>
        /// <returns></returns>
        public static string ToChineseFormatDate(this DateTime dt, bool IsShowSecond = false)
        {
            if (IsShowSecond)
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return dt.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 转换为HH时mm分ss秒
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="IsShowSecond">是否显示秒</param>
        /// <returns></returns>
        public static string ToChineseTime(this DateTime dt, bool IsShowSecond = false)
        {
            if (IsShowSecond)
            {
                return dt.ToString("HH时mm分ss秒");
            }
            else
            {
                return dt.ToString("HH时mm分");
            }
        }

        /// <summary>
        /// 转换为中式HH:mm:ss格式
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="IsShowSecond">是否显示秒</param>
        /// <returns></returns>
        public static string ToChineseFormatTime(this DateTime dt, bool IsShowSecond = false)
        {
            if (IsShowSecond)
            {
                return dt.ToString("HH:mm:ss");
            }
            else
            {
                return dt.ToString("HH:mm");
            }
        } 
        #endregion

        #region 时间差
        /// <summary>
        /// 返回时间差 向上取整
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtEnd"></param>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public static long DateDiffCeiling(this DateTime dt, DateTime dtEnd, DateType dateType)
        {
            DateTime startDt;
            DateTime endDt;
            if (dt > dtEnd)
            {//保证起始时间和结束时间的大小关系
                startDt = dtEnd;
                endDt = dt;
            }
            else
            {
                startDt = dt;
                endDt = dtEnd;
            }

            TimeSpan ts = endDt.Subtract(startDt).Duration();
            switch (dateType)
            {
                case DateType.TICKS: return endDt.Ticks - startDt.Ticks;
                case DateType.MILLISECOND: return Convert.ToInt64(Math.Ceiling(ts.TotalMilliseconds));
                case DateType.SECOND: return Convert.ToInt64(Math.Ceiling(ts.TotalSeconds));
                case DateType.MINUTE: return Convert.ToInt64(Math.Ceiling(ts.TotalMinutes));
                case DateType.HOUR: return Convert.ToInt64(Math.Ceiling(ts.TotalHours));
                case DateType.DAY: return Convert.ToInt64(Math.Ceiling(ts.TotalDays));
                case DateType.MONTH: int day = DateTime.DaysInMonth(endDt.Year, endDt.Month);
                    return ((endDt.Year - startDt.Year) * 12 + ((endDt.Year - startDt.Year > 0) ? endDt.Month : (endDt.Month - startDt.Month))) + Convert.ToInt64(Math.Ceiling(((day + (double)(endDt.Day - startDt.Day)) % day) / day));
                case DateType.YEAR: return endDt.Year - startDt.Year + Convert.ToInt64(Math.Ceiling(((12 + (double)(endDt.Month - startDt.Month)) % 12) / 12));
            }

            return 0;
        }

        /// <summary>
        /// 返回时间差 四舍五入取整
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtEnd"></param>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public static long DateDiffRound(this DateTime dt, DateTime dtEnd, DateType dateType)
        {
            DateTime startDt;
            DateTime endDt;
            if (dt > dtEnd)
            {//保证起始时间和结束时间的大小关系
                startDt = dtEnd;
                endDt = dt;
            }
            else
            {
                startDt = dt;
                endDt = dtEnd;
            }

            TimeSpan ts = endDt.Subtract(startDt).Duration();
            switch (dateType)
            {
                case DateType.TICKS: return endDt.Ticks - startDt.Ticks;
                case DateType.MILLISECOND: return Convert.ToInt64(Math.Round(ts.TotalMilliseconds));
                case DateType.SECOND: return Convert.ToInt64(Math.Round(ts.TotalSeconds));
                case DateType.MINUTE: return Convert.ToInt64(Math.Round(ts.TotalMinutes));
                case DateType.HOUR: return Convert.ToInt64(Math.Round(ts.TotalHours));
                case DateType.DAY: return Convert.ToInt64(Math.Round(ts.TotalDays));
                case DateType.MONTH: int day = DateTime.DaysInMonth(endDt.Year, endDt.Month);
                    return ((endDt.Year - startDt.Year) * 12 + ((endDt.Year - startDt.Year > 0) ? endDt.Month : (endDt.Month - startDt.Month))) + Convert.ToInt64(Math.Round(((day + (double)(endDt.Day - startDt.Day)) % day) / day));
                case DateType.YEAR: return endDt.Year - startDt.Year + Convert.ToInt64(Math.Round(((12 + (double)(endDt.Month - startDt.Month)) % 12) / 12));
            }

            return 0;
        }

        /// <summary>
        /// 返回时间差 向下取整
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtEnd"></param>
        /// <param name="dateType"></param>
        /// <returns></returns>
        public static long DateDiffFloor(this DateTime dt, DateTime dtEnd, DateType dateType)
        {
            DateTime startDt;
            DateTime endDt;
            if (dt > dtEnd)
            {//保证起始时间和结束时间的大小关系
                startDt = dtEnd;
                endDt = dt;
            }
            else
            {
                startDt = dt;
                endDt = dtEnd;
            }

            TimeSpan ts = endDt.Subtract(startDt).Duration();
            switch (dateType)
            {
                case DateType.TICKS: return endDt.Ticks - startDt.Ticks;
                case DateType.MILLISECOND: return Convert.ToInt64(Math.Floor(ts.TotalMilliseconds));
                case DateType.SECOND: return Convert.ToInt64(Math.Floor(ts.TotalSeconds));
                case DateType.MINUTE: return Convert.ToInt64(Math.Floor(ts.TotalMinutes));
                case DateType.HOUR: return Convert.ToInt64(Math.Floor(ts.TotalHours));
                case DateType.DAY: return Convert.ToInt64(Math.Floor(ts.TotalDays));
                case DateType.MONTH: int day = DateTime.DaysInMonth(endDt.Year, endDt.Month);
                    return ((endDt.Year - startDt.Year) * 12 + ((endDt.Year - startDt.Year > 0) ? endDt.Month : (endDt.Month - startDt.Month))) + Convert.ToInt64(Math.Floor(((day + (double)(endDt.Day - startDt.Day)) % day) / day));
                case DateType.YEAR: return endDt.Year - startDt.Year + Convert.ToInt64(Math.Floor(((12 + (double)(endDt.Month - startDt.Month)) % 12) / 12));
            }

            return 0;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtEnd"></param>
        /// <param name="isChinese"></param>
        /// <returns></returns>
        public static string DateDiffString(this DateTime dt, DateTime dtEnd, bool isChinese = true)
        {
            string dateDiff = null;

            if (dt > dtEnd)
            {//保证起始时间和结束时间的大小关系
                return "起始时间大于结束时间";
            }

            TimeSpan ts = dtEnd - dt;
            if (ts.Days >= 1)
            {
                dateDiff = dt.Month.ToString() + (isChinese ? "月" : "-") + dt.Day.ToString() + (isChinese ? "日" : "");
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours.ToString() + (isChinese ? "小时前" : " hours before");
                }
                else
                {
                    dateDiff = ts.Minutes.ToString() + (isChinese ? "分钟前" : " minutes before");
                }
            }

            return dateDiff;
        }
        #endregion

        /// <summary>
        /// 获取当月的最后一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastDayInMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        /// <summary>
        /// 当月的天数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDaysInMonth(this DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }
    }
}
