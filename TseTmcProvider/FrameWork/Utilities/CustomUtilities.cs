using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FrameWork.Utilities;

namespace TadbirPardaz.AM.Common
{
    public static class AssetCustomUtility
    {
        #region Constants

        private static string _SEP = "-";

        #endregion

        //#region AMS 

        //#region  ProductByIsin
        //public static ProductType GetProductTypeByIsin(string isin)
        //{
        //    switch (isin.Substring(0, 3))
        //    {
        //        case "IRR":
        //        case "IRO":
        //        case "IRT":
        //        case "IRS":
        //            return ProductType.Stock;
        //        case "IRB":
        //            return ProductType.Bond;
        //        default:
        //            return ProductType.Fund;
        //    }
        //}
        //#endregion

        //#endregion
        public static char ConvertToEnglish(char input)
        {
            switch (input)
            {
                case 'ض':
                    return 'q';
                case 'ص':
                    return 'w';
                case 'ث':
                    return 'e';
                case 'ق':
                    return 'r';
                case 'ف':
                    return 't';
                case 'غ':
                    return 'y';
                case 'ع':
                    return 'u';
                case 'ه':
                    return 'i';
                case 'خ':
                    return 'o';
                case 'ح':
                    return 'p';
                case 'ج':
                    return '[';
                case 'چ':
                    return ']';
                case 'ش':
                    return 'a';
                case 'س':
                    return 's';
                case 'ی':
                    return 'd';
                case 'ب':
                    return 'f';
                case 'ل':
                    return 'g';
                case 'ا':
                    return 'h';
                case 'ت':
                    return 'j';
                case 'ن':
                    return 'k';
                case 'م':
                    return 'l';
                case 'ک':
                    return ';';
                case 'گ':
                    return '\'';
                case 'پ':
                    return '\\';
                case 'ظ':
                    return 'z';
                case 'ط':
                    return 'x';
                case 'ز':
                    return 'c';
                case 'ر':
                    return 'v';
                case 'ذ':
                    return 'b';
                case 'د':
                    return 'n';
                case 'و':
                    return ',';
            }

            return ' ';
        }

        public static DateTime MinDate => new DateTime(1907, 1, 1);

        //public static DateTime CustomAddMonth(this DateTime inDate, int months, Culture culture = null)
        //{
        //    var daysInMonth = 0;
        //    if (culture == null)
        //    {
        //        culture = new Culture("en", Languages.English);
        //    }
        //    var coEfficient = months < 0 ? -1 : 1;
        //    for (int i = 0; i < Math.Abs(months); i++)
        //    {
        //        daysInMonth = inDate.GetDaysInMonth(culture);
        //        inDate = inDate.AddDays(coEfficient * daysInMonth);
        //    }

        //    return inDate;
        //}
        static bool invalid = false;

        public static bool IsValidEmail(this string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", AssetCustomUtility.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public static DateTime MaxDate => new DateTime(2060, 1, 1);

        public static string Beautify(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        public static string RemoveControlCharacters(string inString)
        {
            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            foreach (char t in inString)
            {
                ch = t;
                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }

        public static string SafeSpace(this string str, string replacement = "")
        {
            if (String.IsNullOrEmpty(str))
                return str;
            str = str.Replace(" ", " ");
            str = str.Replace(" ", " ");
            str = str.Replace(" ", " ");
            str = str.Replace("¬", " ");
            str = str.Replace(" ", " ");
            str = str.Replace(" ", " ");
            str = str.Replace(" ", " ");
            str = str.Replace("\x200E", replacement);
            str = str.Replace("\x200F", replacement);
            str = str.Replace('\x0640'.ToString(), replacement);
            str = str.Replace(((char)65279).ToString(), replacement);
            string str1 = str;
            char ch = ' ';
            string oldValue1 = ch.ToString();
            string newValue1 = replacement;
            str = str1.Replace(oldValue1, newValue1);
            string str2 = str;
            ch = ' ';
            string oldValue2 = ch.ToString();
            string newValue2 = replacement;
            str = str2.Replace(oldValue2, newValue2);
            string str3 = str;
            ch = '\x200B';
            string oldValue3 = ch.ToString();
            string newValue3 = replacement;
            str = str3.Replace(oldValue3, newValue3);
            string str4 = str;
            ch = '\x200C';
            string oldValue4 = ch.ToString();
            string newValue4 = replacement;
            str = str4.Replace(oldValue4, newValue4);
            string str5 = str;
            ch = '\x200D';
            string oldValue5 = ch.ToString();
            string newValue5 = replacement;
            str = str5.Replace(oldValue5, newValue5);
            string str6 = str;
            ch = '\x200E';
            string oldValue6 = ch.ToString();
            string newValue6 = replacement;
            str = str6.Replace(oldValue6, newValue6);
            string str7 = str;
            ch = '\x200F';
            string oldValue7 = ch.ToString();
            string newValue7 = replacement;
            str = str7.Replace(oldValue7, newValue7);
            string str8 = str;
            ch = '‐';
            string oldValue8 = ch.ToString();
            string newValue8 = replacement;
            str = str8.Replace(oldValue8, newValue8);
            string str9 = str;
            ch = '‑';
            string oldValue9 = ch.ToString();
            string newValue9 = replacement;
            str = str9.Replace(oldValue9, newValue9);
            str = str.Replace("¬", replacement);
            str = str.Trim();

            return str;
        }

        public static string RmoveIllegalChars(this string illegal)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex($"[{Regex.Escape(regexSearch)}]");
            illegal = r.Replace(illegal, "");

            return illegal;
        }

        public static double ConvertBytesToMegabytes(this long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }


        public static int GetLowerPage(int pageSize, int pageIndex)
        {
            if (pageSize >= int.MaxValue)
            {
                return 1;
            }
            return (pageSize * pageIndex) + 1;
        }

        public static int GetUpperPage(int pageSize, int pageIndex)
        {
            if (pageSize >= int.MaxValue)
            {
                return int.MaxValue;
            }
            return GetLowerPage(pageSize, pageIndex) + pageIndex - 1;
        }
        //public static DateTime RemoveMinutes(this DateTime dt)
        //{
        //    return Convert.ToDateTime(dt.ToShortDateString());
        //}

        public static string ConvertToEximiusFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }

        public static string ConvertToEximiusFormatWithTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd000000");
        }

        public static string PadLeftString(string input, int totalWidth)
        {
            return input.PadLeft(totalWidth, '0');
        }

        public static string PadLeftString(this int input, int totalWidth)
        {
            return input.ToString().PadLeft(totalWidth, '0');
        }

        //public static string ToFirstMarketISIN(this string isin)
        //{
        //    if (isin.EndsWith("02"))
        //    {
        //        return isin.Replace("02", "01");
        //    }
        //    if (isin.EndsWith("03"))
        //    {
        //        return isin.Replace("03", "01");
        //    }
        //    if (isin.EndsWith("A2"))
        //    {
        //        return isin.Replace("A2", "A1");
        //    }
        //    if (isin.EndsWith("C2"))
        //    {
        //        return isin.Replace("C2", "C1");
        //    }
        //    if (isin.StartsWith("IRB") && isin.EndsWith("12"))
        //    {
        //        return isin.Replace("12", "11");
        //    }
        //    if (isin.StartsWith("IRB") && isin.EndsWith("52"))
        //    {
        //        return isin.Replace("52", "51");
        //    }
        //    return isin;
        //}

        public static string SafePersianPhrase(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return
                    str.SafePersianEncode().RemoveNoise()
                        .Replace((char)8204, (char)32)
                        .TrimStart()
                        .TrimEnd();
            }

            return str;
        }



        public static string SafeMutualFundTitle(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str =
                      str.SafePersianEncode()
                         .Replace("صندوق سرمایه گذاری اختصاصی بازارگردان", "")
                         .Replace("صندوق درآمد ثابت  جایزه علمی و فناوری", "")
                         .Replace("صندوق سرمایه گذاری با درآمد ثابت", "")
                         .Replace("صندوق سرمایه گذاری بادرآمد ثابت", "")
                         .Replace("صندوق درآمد ثابت  درآمد ثابت", "")
                         .Replace("صندوق سرمایه‌گذاری نیکوکاری", "")
                         .Replace("صندوق سهام ی", "")
                         .Replace("صندوق سهام ی ", "")
                         .Replace("صندوق سرمایه گذاری مشترک", "")
                         .Replace("صندوق سرمایه گذاری", "")
                         .Replace("صندوق سرمایه گذاری", "")
                         .Replace("اختصاصی بازارگردانی", "")
                         .Replace("کارگزاري بانک", "")
                         .Replace("سرمایه گذاری", "")
                         .Replace("اختصاصی", "")
                         .Replace("با درآمد ثابت", "")
                         .Replace("درآمد ثابت", "")
                         .Replace("بازارگردانی", "")
                         .Replace("بازارگردان", "")
                         .Replace("بانک", "")
                         .Replace("نیکوکاری", "")
                         .Replace("مشترک", "")
                         .Replace("صندوق", "")
                         .Replace("شاخصی", "")
                         .Replace("کارگزاری", "");

                str = Regex.Replace(str, @"\s+", " ");
            }

            return str;
        }

        public static decimal DividendTo(this decimal val, decimal fraction)
        {
            return fraction == 0 ? 0 : val / fraction;
        }


        public static decimal ToPercent(this decimal val)
        {
            return val / 100;
        }

        public static string ToRightISIN(this string isin)
        {
            return isin.Replace("IRO", "IRR").Replace("0001", "0101");
        }

        public static string ToStockISIN(this string isin)
        {
            return isin.Replace("IRR", "IRO").Replace("0101", "0001");
        }

        public static bool IsRight(this string isin)
        {
            return isin.StartsWith("IRR");
        }

        public static string ToISINFrormat(this string isin)
        {
            return !isin.StartsWith("IR") ? isin.PadLeft(12, '0') : isin;
        }


        public static string RemoveNoise(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return str.Replace("‏", "");
        }

        public static decimal ToNominalPrice(this decimal price) => (price / 1000000 * 100);

        /// <summary>
        /// برای سلف متفاوت خواهد بود
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        ///
        public static bool IsProductBond(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.StartsWith("IRB") && !str.StartsWith("IRBE") && !str.StartsWith("IRBK");
            }
            return false;
        }

        /// <summary>
        ///آیا سلف می باشد؟
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsProductForward(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.StartsWith("IRBE") || str.StartsWith("IRBK");
            }
            return false;
        }

        public static decimal CalculateDailyProfitPercentage(decimal dailyProfit)
        {
            return dailyProfit / 10000;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string GetUniqueKey()
        {
            int size = 16;
            byte[] data = new byte[size];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(data);
            return BitConverter.ToString(data).Replace("-", String.Empty);
        }

        public static string GetUniqueId()
        {
            var guidString = Guid.NewGuid().ToString("N");
            guidString = guidString + Get8Digits();
            return guidString;
        }

        public static string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return $"{random:D8}";
        }

        public static DateTime TseTmcDateTime(this DateTime dateTime, string date, string time, string format = "yyyyMMddHHmmss", CultureInfo culture = null)
        {
            DateTime dateValue;
            if (time.Length % 2 != 0)
            {
                time = time.PadLeft(6, '0');
            }
            var stringDateTime = date + time;
            if (culture == null)
            {
                culture = new CultureInfo("en-US");
            }
            if (DateTime.TryParseExact(stringDateTime, format, culture, DateTimeStyles.None, out dateValue)
)
            {
                return dateValue;
            }
            return DateTime.MinValue;
        }
        public static int ToTseTmcDate(this DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }

   

        public static int ToTseTmcTime(this DateTime time)
        {
            var intTime = Int32.Parse(time.TimeOfDay.ToString("hhmm"));
            return intTime;
        }

        public static double Since1970(this DateTime dateTime)
        {
            var milliseconds = dateTime.ToUniversalTime().Subtract(
                   new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                   ).TotalMilliseconds;
            return milliseconds;
        }

        public static DateTime ConvertmiliSecound1970ToDate(this string since1970)
        {
            var toLong = since1970.SafeDouble();
            return ConvertmiliSecound1970ToDate(toLong);
        }

        public static DateTime ConvertmiliSecound1970ToDate(this double since1970)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            return epoch.AddMilliseconds(since1970).ToLocalTime();
        }

        public static long GenerateUniqueId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }


        public static IEnumerable<string> SplitIntoChunks(string text, int chunkSize)
        {
            int offset = 0;
            while (offset < text.Length)
            {
                int size = Math.Min(chunkSize, text.Length - offset);
                yield return text.Substring(offset, size);
                offset += size;
            }
        }

        //public static DateTime SafeMmtpDateTime(this object d)
        //{
        //    try
        //    {
        //        if (d != null)
        //        {
        //            var y = Convert.ToInt32(d.ToString().Substring(0, 4));
        //            var m = Convert.ToInt32(d.ToString().Substring(4, 2));
        //            var day = Convert.ToInt32(d.ToString().Substring(6, 2));
        //            var hour = Convert.ToInt32(d.ToString().Substring(8, 2));
        //            var min = Convert.ToInt32(d.ToString().Substring(10, 2));
        //            var sec = Convert.ToInt32(d.ToString().Substring(12, 2));

        //            DateTime dt = new DateTime(y, m, day, hour, min, sec);
        //            //CultureInfo enUS = new CultureInfo("en-US");
        //            ////d.ToString("MMMM dd, yyyy");
        //            //DateTime.TryParseExact(d.ToString(), "yyyyMMddhhmmss", enUS, DateTimeStyles.None, out dt);
        //            //DateTime.TryParse(d.ToString(), out dt);
        //            return dt;
        //        }
        //    }
        //    catch
        //    {
        //        return DateTime.MinValue;
        //    }
        //    return DateTime.MinValue;
        //}

        public static TimeSpan SafeMmtpTime(this object d)
        {
            try
            {
                if (d != null)
                {
                    TimeSpan time;
                    string strTime;
                    if (d.ToString().Length < 6)
                    {
                        strTime = "0" + d;
                    }
                    else
                    {
                        strTime = d.ToString();
                    }
                    var hour = strTime.Substring(0, 2);
                    var min = strTime.Substring(2, 2);
                    var sec = strTime.Substring(4, 2);
                    time = TimeSpan.Parse(hour + ":" + min + ":" + sec);

                    return time;
                }
            }
            catch
            {
                return TimeSpan.MinValue;
            }
            return TimeSpan.MinValue;
        }

        public static bool IsNumeric(this string s)
        {
            int output;
            return int.TryParse(s, out output);
        }
        public static string ToFirstMarketISIN(this string isin)
        {
            if (isin.EndsWith("02"))
            {
                return isin.Replace("02", "01");
            }
            if (isin.EndsWith("03"))
            {
                return isin.Replace("03", "01");
            }
            if (isin.EndsWith("A2"))
            {
                return isin.Replace("A2", "A1");
            }
            if (isin.EndsWith("C2"))
            {
                return isin.Replace("C2", "C1");
            }
            if (isin.StartsWith("IRB") && isin.EndsWith("12"))
            {

                return isin.Replace("12", "11");
            }
            if (isin.StartsWith("IRB") && isin.EndsWith("52"))
            {

                return isin.Replace("52", "51");
            }
            return isin;
        }

        public static DateTime SafeMmtpDate(this object d)
        {
            try
            {
                if (d != null)
                {
                    DateTime dt;
                    var y = Convert.ToInt32(d.ToString().Substring(0, 4));
                    var m = Convert.ToInt32(d.ToString().Substring(4, 2));
                    var day = Convert.ToInt32(d.ToString().Substring(6, 2));
                    dt = new DateTime(y, m, day);
                    return dt;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
            return DateTime.MinValue;
        }


        //public static string GetSecurityAccountFormat(string brokerCode, string pamCode)
        //{
        //    return AccountPrefix.SEC + _SEP + pamCode + _SEP + brokerCode;
        //}

        //public static string GetCurrentCashAccountFormat(string pamCode, string brokerCode)
        //{
        //    return AccountPrefix.CC + _SEP + brokerCode.Trim() + _SEP + pamCode.Trim();
        //}

        //public static string GetSavingCashAccountFormat(string codeNumber)
        //{
        //    return AccountPrefix.CS + _SEP + codeNumber.Trim();
        //}


        //public static string GetOtherAccountFormat(string pamCode)
        //{
        //    return AccountPrefix.CO + _SEP + pamCode.Trim();
        //}

        //public static string GetTermAccountFormat(string codeNumber)
        //{
        //    return AccountPrefix.CT + _SEP + codeNumber.Trim();
        //}

        //public static string GetBlockedDiscounteddividendAccountFormat(string pamCode)
        //{
        //    return AccountPrefix.CDIV + _SEP + pamCode.Trim();
        //}
    }

    public static class EnumExtensions
    {
        private static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
                return null;
            return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }

        public static String GetEnumDescription(this Enum value)
        {
            var description = GetAttribute<DescriptionAttribute>(value);
            return description?.Description;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            //return default(T);
        }
        /// <summary>
        /// Converts datatable to list<T> dynamically
        /// </summary>
        /// <typeparam name="T">Class name</typeparam>
        /// <param name="dataTable">data table to convert</param>
        /// <returns>List<T></returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new List<T>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).Cast<PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                }).ToList();

            //Read Datatable column names and types
            var dtlFieldNames = dataTable.Columns.Cast<DataColumn>().
                Select(item => new {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var classObj = new T();

                foreach (var dtField in dtlFieldNames)
                {
                    PropertyInfo propertyInfos = classObj.GetType().GetProperty(dtField.Name);

                    var field = objFieldNames.Find(x => x.Name == dtField.Name);

                    if (field != null)
                    {

                        if (propertyInfos.PropertyType == typeof(DateTime))
                        {
                            propertyInfos.SetValue
                            (classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(int))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToInt(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(long))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToLong(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(decimal))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToDecimal(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(string))
                        {
                            if (dataRow[dtField.Name] is DateTime)
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToDateString(dataRow[dtField.Name]), null);
                            }
                            else
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToString(dataRow[dtField.Name]), null);
                            }
                        }
                    }
                }
                dataList.Add(classObj);
            }
            return dataList;
        }

        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return date.SafeDateTime().SafeString();
        }

        private static string ConvertToString(object value)
        {
            return value.SafeString();
        }

        private static int ConvertToInt(object value)
        {
            return value.SafeInt();
        }

        private static long ConvertToLong(object value)
        {
            return value.SafeLong();
        }

        private static decimal ConvertToDecimal(object value)
        {
            return value.SafeDecimal();
        }

        private static DateTime convertToDateTime(object date)
        {
            return date.SafeDateTime();
        }



        public static DateTime MinDate => new DateTime(1907, 1, 1);

        public static DateTime MaxDate => new DateTime(2060, 1, 1);


        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }


}