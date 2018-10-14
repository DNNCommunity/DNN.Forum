using DotNetNuke.Services.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNuke.Forum.Library
{
    public class FileUtilityClass
    {
        static Instrumentation.DnnLogger log = Instrumentation.DnnLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public static IFolderInfo GetFolder(string fullFolderPath, int portalId)
        {
            string subFolder = DotNetNuke.Common.Globals.GetSubFolderPath(fullFolderPath, portalId);
            return FolderManager.Instance.GetFolder(portalId, subFolder);
        }

        public static IFolderInfo AddFolder(string fullFolderPath, int portalId)
        {
            string subFolder = DotNetNuke.Common.Globals.GetSubFolderPath(fullFolderPath, portalId);
            return FolderManager.Instance.AddFolder (portalId, subFolder);
        }

        public static bool DeleteFile(string fullParentFolderPath, string fileName, int portalId)
        {
            bool retval = false;
            IFolderInfo parent = GetFolder(fullParentFolderPath, portalId);
            if (parent != null)
            {
                IFileInfo file = FileManager.Instance.GetFile(parent, fileName);
                if (file != null)
                {
                    FileManager.Instance.DeleteFile(file);
                    retval = true;
                }
                else
                {
                    log.Error("File Not found: " + System.IO.Path.Combine(fullParentFolderPath, fileName));
                }
            }
                else
                {
                    log.Error("Parent folder Not found: " + fullParentFolderPath);
                }
            return retval;
        }
    }
}
