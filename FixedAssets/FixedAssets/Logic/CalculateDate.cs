using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixedAssets.Logic
{
    static public class CalculateDate
    {
        static public DateTime StringToDate(string inputDate, string separator, string firstRepl, string secondRepl)
        {
            inputDate = inputDate.Replace(firstRepl, separator).Replace(secondRepl, separator);
            int firstDot = inputDate.IndexOf(separator);
            int secondDot = inputDate.Substring(firstDot + 1).IndexOf(separator);
            int year, month, day;
            int.TryParse(inputDate.Substring(0, firstDot), out day);
            int.TryParse(inputDate.Substring(firstDot + 1, secondDot), out month);
            int.TryParse(inputDate.Substring(firstDot + secondDot + 2, 4), out year);

            return new DateTime(year, month, day);
        }
    }
}