namespace DotNetNuke.Modules.Forums.Templating.Filters
{
    using System;
    using Helpers;

    public static class RelativeTimeSpanFilter
    {
        public static string RelativeDate(DateTime input)
        {
            return input.GetRelativeTimeSpan();
        }
    }
}