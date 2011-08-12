//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2011
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace DotNetNuke.Modules.Forums.Components.Common {

    public class Utilities {

        /// <summary>
        /// Get the template as a string from the specified path
        /// </summary>
        /// <param name="filePath">Physical path to file</param>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static string GetFile(string filePath) {
            var sContents = string.Empty;
            if (File.Exists(filePath)) {
                try {
                    using (var sr = new StreamReader(filePath)) {
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

        public static string GetSharedResource(string key, bool isAdmin)
        {
            if (isAdmin) {
                return DotNetNuke.Services.Localization.Localization.GetString(key, "~/DesktopModules/DNNCorp/forums/App_LocalResources/ControlPanel.ascx.resx");
            }
            return DotNetNuke.Services.Localization.Localization.GetString(key, "~/DesktopModules/DNNCorp/forums/App_LocalResources/SharedResources.resx");
        }

        public static string LocalizeControl(string controlText) {
            return LocalizeControl(controlText, false);
        }

        public static string LocalizeControl(string controlText, bool isAdmin) {
            const string pattern = "(\\[RESX:.+?\\])";
            var regExp = new Regex(pattern);
            var matches = regExp.Matches(controlText);
            foreach (Match match in matches)
            {
                var sReplace = GetSharedResource(match.Value, isAdmin);
                if (!string.IsNullOrEmpty(sReplace)) {
                    controlText = controlText.Replace(match.Value, sReplace);
                }
                else {
                   controlText = controlText.Replace(match.Value, match.Value);
                }
            }
            return controlText;
        }

        public static object FillObjectFromRequest(object infoObject, System.Collections.Specialized.NameValueCollection request) {
            if (request.Keys.Count > 0) {
                var myType = infoObject.GetType();
                var myProperties = myType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                string sValue;
                foreach (var pItem in myProperties) {
                    sValue = string.Empty;
                    var sKey = pItem.Name.ToLower();
                    if ((request[sKey] != null)) {
                        sValue = request[sKey];
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
                            if (sValue != null) obj = sValue;
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
                        infoObject.GetType().GetProperty(pItem.Name).SetValue(infoObject, obj, BindingFlags.Public | BindingFlags.NonPublic, null, null, null);
                    }
               }
            }
            return infoObject;
        }

    }
}