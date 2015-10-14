using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetNuke.Forum.Library
{
    public class UrlUtilityClass
    {        
        /// <summary>
        /// Return shorter url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlShortener(string url)
        {
            string retval = url;
            Match ret = Regex .Match (url, @">([^\s]*)<");
            if (ret.Success  ==  true)
            {
                string linkName = ret.Groups [1].Value;
                if (string.IsNullOrEmpty(linkName) == false && linkName.Length > 60)
                {
<<<<<<< HEAD
                    retval = string.Format("{0}...{1}", linkName.Substring(0, 37), linkName.Substring(linkName.Length - 15, 15));
                }
                else
                {
                    retval = linkName;
=======
                    string newLinkName = string.Format(">{0}...{1}<", linkName.Substring(0, 37), linkName.Substring(linkName.Length - 15, 15));
                    retval = url.Replace(ret.Groups[0].Value, newLinkName);
>>>>>>> 9f519d0... Fixed UrlShortener issue
                }
            }
            return retval;
        }
    }
}
