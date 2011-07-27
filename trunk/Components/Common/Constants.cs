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

namespace DotNetNuke.Modules.Forums.Components.Common {

	/// <summary>
	/// This class is used to store enumerators as well as other constants used throughout the module (such as module setting names, cache keys, etc.). 
	/// </summary>
	public class Constants
	{

		#region Settings

		internal const string SettingsSampleInteger = "DNN_FORUMS_SAMPLESETTING_INTEGER";

		internal const int DefaultSettingsSampleInteger = 1;
		
		#endregion

		#region Cache Keys

		public const string SampleCacheKey = ModulesCacheKey + "Sample-";

		#endregion

		/// <summary>
		/// A recommended limit for a meta page title for SEO purposes.
		/// </summary>
		public const int SeoTitleLimit = 64;

		/// <summary>
		/// A recommended limit for a meta page description for SEO purposes.
		/// </summary>
		public const int SeoDescriptionLimit = 150;

		/// <summary>
		/// A recommended limit for meta page keywords for SEO purposes.
		/// </summary>
		public const int SeoKeywordsLimit = 15;

		/// <summary>
		/// The cache prefix to use for all cached module objects.
		/// </summary>
		public const string ModulesCacheKey = "DNN_Forums_";

		/// <summary>
		/// The content type used for the core Content Item integration.
		/// </summary>
		public const string ContentTypeName = "DNN_Forums_Thread";

		#region Enumerators

		/// <summary>
		/// This enumerator is used to determine which control should be loaded into Dispatch.ascx.
		/// </summary>
		public enum PageScope
		{
			Home = 0
		}

		#endregion
	
	}
}