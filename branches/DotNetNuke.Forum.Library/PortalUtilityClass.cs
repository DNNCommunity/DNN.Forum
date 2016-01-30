using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNuke.Forum.Library
{
    public class PortalUtilityClass
    {
        static Instrumentation.DnnLogger log = Instrumentation.DnnLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public static PortalAliasInfo PrimaryPortalAlias(int portalId)
        {
            string cacheKey = string.Format("PortalUtilityClass_PrimaryPortalAlias_" + portalId);
            PortalAliasInfo retval = System.Web.HttpRuntime.Cache[cacheKey] as PortalAliasInfo;
            if (retval == null)
            {
                foreach (PortalAliasInfo pinfo in PortalAliasController.Instance.GetPortalAliasesByPortalId(portalId))
                {
                    if (pinfo.IsPrimary == true)
                    {
                        retval = pinfo;                        
                        System.Web.HttpRuntime.Cache.Add(cacheKey, retval, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                        break;
                    }
                }
            }
            return retval;
        }

        //public static int SearchTagSearchTabID(int portalId)
        //{
        //    int retval = -1;
        //    ModuleController controller = new ModuleController();
        //    try
        //    {
        //        TabController tabC = new TabController();
        //        foreach (ModuleInfo mod in controller.GetModulesByDefinition(portalId, "Content List"))
        //        {
        //            TabInfo tabi = tabC.GetTab(mod.TabID, portalId);
        //            if (tabi != null)
        //            {
        //                if (tabi.TabName == "Tag Search Results")
        //                {
        //                    retval = tabi.TabID;
        //                }
        //                else
        //                {
        //                    log.Debug("Tab: " + tabi.TabName);
        //                }
        //            }
        //        }
        //    }
        //    finally
        //    {
        //    }
        //    if (retval == -1)
        //    {
        //        log.Error("Tag Search Results -page not found: " + controller.GetModulesByDefinition(portalId, "Content List").Count.ToString());
        //    }
        //    return retval;
        //}
 

 

    }
}
