using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace DotNetNuke.Modules.Forums.Common {
    public class Utilities {
        /// <summary>
        /// Get the template as a string from the specified path
        /// </summary>
        /// <param name="FilePath">Physical path to file</param>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string GetFile(string FilePath) {
            string sContents = string.Empty;
            if (System.IO.File.Exists(FilePath)) {
                try {
                    using (System.IO.StreamReader sr = new StreamReader(FilePath)) {
                        sContents = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch (Exception exc) {
                    sContents = exc.Message;
                }
            }
            return sContents;
        }
        public static string GetSharedResource(string key) {
            return GetSharedResource(key, false);
        }
        public static string GetSharedResource(string key, bool isAdmin) {
            if (isAdmin) {
                return DotNetNuke.Services.Localization.Localization.GetString(key, "~/DesktopModules/DNNCorp/forums/App_LocalResources/ControlPanel.ascx.resx");
            }
            else {
                return DotNetNuke.Services.Localization.Localization.GetString(key, "~/DesktopModules/DNNCorp/forums/App_LocalResources/SharedResources.resx");
            }

        }
        public static string LocalizeControl(string controlText) {
            return LocalizeControl(controlText, false);
        }
        public static string LocalizeControl(string controlText, bool isAdmin) {

            string sReplace = "";
            string pattern = "(\\[RESX:.+?\\])";
            Regex regExp = new Regex(pattern);
            MatchCollection matches = default(MatchCollection);
            matches = regExp.Matches(controlText);
            foreach (Match match in matches) {
                sReplace = GetSharedResource(match.Value, isAdmin);
                if (!string.IsNullOrEmpty(sReplace)) {
                    controlText = controlText.Replace(match.Value, sReplace);
                }
                else {
                   controlText = controlText.Replace(match.Value, match.Value);
                }

            }
            return controlText;
        }
        public static object FillObjectFromRequest(object InfoObject, System.Collections.Specialized.NameValueCollection Request) {

            if (Request.Keys.Count > 0) {
                Type myType = InfoObject.GetType();
                System.Reflection.PropertyInfo[] myProperties = myType.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                string sKey = string.Empty;
                string sValue = string.Empty;
                foreach (PropertyInfo pItem in myProperties) {
                    sValue = string.Empty;
                    sKey = pItem.Name.ToLower();
                    if ((Request[sKey] != null)) {
                        sValue = Request[sKey];
                    }
                    if (!string.IsNullOrEmpty(sValue)) {
                        sValue = HttpUtility.HtmlDecode(sValue);
                        sValue = HttpUtility.UrlDecode(sValue);
                    }
                    object obj = null;
                    switch (pItem.PropertyType.ToString()) {
                        case "System.Int16":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = "-1";
                            }
                            obj = Convert.ToInt16(sValue);
                            break;
                        case "System.Int32":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = "-1";
                            }
                            obj = Convert.ToInt32(sValue);
                            break;
                        case "System.Int64":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = "-1";
                            }
                            obj = Convert.ToInt64(sValue);
                            break;
                        case "System.Single":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = "-1";
                            }
                            obj = Convert.ToSingle(sValue);
                            break;
                        case "System.Double":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = "0";
                            }
                            obj = Convert.ToDouble(sValue);
                            break;
                        case "System.Decimal":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = "0.0";
                            }
                            obj = Convert.ToDecimal(sValue, System.Globalization.CultureInfo.InvariantCulture);
                            break;
                        case "System.DateTime":
                            if (string.IsNullOrEmpty(sValue)) {
                                obj = new DateTime(1900, 1, 1, 12, 00, 00);
                            }
                            else {
                                obj = DateTime.Parse(sValue);
                            }

                            break;
                        case "System.String":
                            obj = sValue.ToString();
                            break;
                        case "System.Char":
                            obj = Convert.ToString(sValue);
                            break;
                        case "System.Boolean":
                            obj = Convert.ToBoolean(sValue);
                            break;
                        case "System.Guid":
                            if (String.IsNullOrEmpty(sValue)) {
                                sValue = Guid.Empty.ToString();
                            }
                            obj = new Guid(sValue);

                            break;
                    }
                    if (obj != null) {
                        InfoObject.GetType().GetProperty(pItem.Name).SetValue(InfoObject, obj, BindingFlags.Public | BindingFlags.NonPublic, null, null, null);
                    }
               }
            }
            return InfoObject;

        }
    }
}