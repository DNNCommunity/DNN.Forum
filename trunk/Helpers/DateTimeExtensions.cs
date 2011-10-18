namespace DotNetNuke.Modules.Forums.Helpers
{
    using System;

    public static class DateTimeExtensions
    {
        public static string GetRelativeTimeSpan(this DateTime postDate)
        {
            string stringy = string.Empty;
            TimeSpan diff = DateTime.Now.Subtract(postDate);
            double days = diff.Days;
            double hours = diff.Hours + days * 24;
            double minutes = diff.Minutes + hours * 60;
            if (minutes <= 1)
            {
                return "Just Now";
            }
            double years = Math.Floor(diff.TotalDays / 365);
            if (years >= 1)
            {
                return string.Format("{0} year{1} ago", years, years >= 2 ? "s" : null);
            }
            double weeks = Math.Floor(diff.TotalDays / 7);
            if (weeks >= 1)
            {
                double partOfWeek = days - weeks * 7;
                if (partOfWeek > 0)
                {
                    stringy = string.Format(", {0} day{1}", partOfWeek, partOfWeek > 1 ? "s" : null);
                }
                return string.Format("{0} week{1}{2} ago", weeks, weeks >= 2 ? "s" : null, stringy);
            }
            if (days >= 1)
            {
                double partOfDay = hours - days * 24;
                if (partOfDay > 0)
                {
                    stringy = string.Format(", {0} hour{1}", partOfDay, partOfDay > 1 ? "s" : null);
                }
                return string.Format("{0} day{1}{2} ago", days, days >= 2 ? "s" : null, stringy);
            }
            if (hours >= 1)
            {
                double partOfHour = minutes - hours * 60;
                if (partOfHour > 0)
                {
                    stringy = string.Format(", {0} minute{1}", partOfHour, partOfHour > 1 ? "s" : null);
                }
                return string.Format("{0} hour{1}{2} ago", hours, hours >= 2 ? "s" : null, stringy);
            }

            // Only condition left is minutes > 1
            return minutes.ToString("{0} minutes ago");
        }
    }
}