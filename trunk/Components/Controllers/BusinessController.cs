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
using System.Collections;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
//using System.Xml;
using DotNetNuke.Services.Sitemap;
using DotNetNuke.Entities.Portals;
using System.Collections.Generic;

namespace DotNetNuke.Modules.ExtensionForge
{

	/// <summary>
	/// The Controller class for 6.0 Core Forums module.
	/// </summary>
	public class BusinessController 
	{

		#region Optional Interfaces

		#region IPortable Members

		///// -----------------------------------------------------------------------------
		///// <summary>
		///// ExportModule implements the IPortable ExportModule Interface
		///// </summary>
		///// <param name="ModuleID">The Id of the module to be exported</param>
		///// -----------------------------------------------------------------------------
		//public string ExportModule(int ModuleID)
		//{
		//     //string strXML = "";

		//     //List<ModuleNameInfo> colObject = GetItems(ModuleID);
		//     //if (colObject.Count != 0)
		//     //{
		//     //    strXML += "<ModuleNames>";

		//     //    foreach (ObjectInfo objObject in colObject)
		//     //    {
		//     //        strXML += "<ModuleName>";
		//     //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objObject.Content) + "</content>";
		//     //        strXML += "</ModuleName>";
		//     //    }
		//     //    strXML += "</ModuleNames>";
		//     //}

		//     //return strXML;

		//     throw new NotImplementedException("The method or operation is not implemented.");
		//}

		///// -----------------------------------------------------------------------------
		///// <summary>
		///// ImportModule implements the IPortable ImportModule Interface
		///// </summary>
		///// <param name="ModuleID">The Id of the module to be imported</param>
		///// <param name="Content">The content to be imported</param>
		///// <param name="Version">The version of the module to be imported</param>
		///// <param name="UserId">The Id of the user performing the import</param>
		///// -----------------------------------------------------------------------------
		//public void ImportModule(int ModuleID, string Content, string Version, int UserID)
		//{
		//    //XmlNode xmlModuleNames = DotNetNuke.Common.Globals.GetContent(Content, "ModuleNames");
		//    //foreach (XmlNode xmlModuleName in xmlModuleNames.SelectNodes("ModuleName"))
		//    //{
		//    //    ObjectInfo objModuleName = new ObjectInfo();
		//    //    objModuleName.ModuleId = ModuleID;
		//    //    objModuleName.Content = xmlModuleName.SelectSingleNode("content").InnerText;
		//    //    objModuleName.CreatedByUser = UserID;
		//    //    AddItem(objModuleName);
		//    //}

		//    throw new NotImplementedException("The method or operation is not implemented.");
		//}

		#endregion

		#region ISearchable Members

		///// -----------------------------------------------------------------------------
		///// <summary>
		///// GetSearchItems implements the ISearchable Interface
		///// </summary>
		///// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
		///// -----------------------------------------------------------------------------
		//public SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
		//{
		//     //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

		//     //List<ObjectInfo> colObjects = GetItems(ModInfo.portalId);

		//     //foreach (ObjectInfo objObject in colObjects)
		//     //{
		//     //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objObject.Content, objObject.CreatedByUser, objObject.CreatedDate, ModInfo.ModuleID, objObject.ItemId.ToString(), objObject.Content, "ItemId=" + objObject.ItemId.ToString());
		//     //    SearchItemCollection.Add(SearchItem);
		//     //}

		//     //return SearchItemCollection;

		//     throw new NotImplementedException("The method or operation is not implemented.");
		//}

		#endregion


		#endregion

	}
}