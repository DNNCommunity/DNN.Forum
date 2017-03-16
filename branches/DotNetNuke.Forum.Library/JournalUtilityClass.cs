using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DotNetNuke.Forum.Library
{
    public class JournalUtilityClass
    {
        private static string GetJournalKey(int ModuleId, int ForumId, int TopicId, int PostId)
        {
            return string.Format("DNN_Forum:{0}:{1}:{2}:{3}", ModuleId, ForumId, TopicId, PostId);
        }

        private static int GetAddTopicTypeId(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == "forum_topic" select t);
            int journalTypeId;
            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 1;
            }

            return journalTypeId;
        }
        private static int GetReplyTopicTypeId(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == "forum_reply" select t);
            int journalTypeId;
            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 1;
            }

            return journalTypeId;
        }
        public void AddThreadToJournal(int PortalId, int ModuleId, int TabId, int ForumId, int TopicId, int PostId, int UserId, string URL, string Subject, string Body)
        {
            ModuleInfo module = ModuleController.Instance.GetModule(ModuleId, TabId, false);
            string summary;
            JournalItem ji = new JournalItem
            {
                PortalId = PortalId,
                ProfileId = UserId,
                UserId = UserId,
                Title = Subject,    
                JournalTypeId = GetAddTopicTypeId (PortalId),            
                ItemData = new ItemData { Url = URL }
            };

            summary = TextUtilityClass.StripHTML(HttpUtility.HtmlDecode(Body));
            if (summary.Length > 150)
            {
                summary = summary.Substring(0, 150) + "...";
            }

            ji.Summary = summary;
            ji.Body = TextUtilityClass.StripHTML(Body);
            ji.ObjectKey = GetJournalKey(ModuleId, ForumId, TopicId, PostId);
            if (JournalController.Instance.GetJournalItemByKey(PortalId, ji.ObjectKey) != null)
            {
                JournalController.Instance.DeleteJournalItemByKey(PortalId, ji.ObjectKey);
            }
            ji.SecuritySet = "E,";
            JournalController.Instance.SaveJournalItem(ji, module);
        }

        public void AddReplyToJournal(int PortalId, int ModuleId, int TabId, int ForumId, int TopicId, int ReplyId, int UserId, string URL, string Subject, string Body)
        {
            //make sure that this is a User before trying to create a journal item, you can't post a JI without
            if (UserId > 0)
            {
                ModuleInfo module = ModuleController.Instance.GetModule(ModuleId, TabId, false);
                string summary;
                JournalItem ji = new JournalItem
                {
                    PortalId = PortalId,
                    ProfileId = UserId,
                    UserId = UserId,
                    Title = Subject,
                    JournalTypeId = GetReplyTopicTypeId(PortalId),
                    ItemData = new ItemData { Url = URL }
                };
                summary = TextUtilityClass.StripHTML(HttpUtility.HtmlDecode(Body));
                if (summary.Length > 150)
                {
                    summary = summary.Substring(0, 150) + "...";
                }
                ji.Summary = summary;
                ji.Body = TextUtilityClass.StripHTML(Body);
                ji.ObjectKey = GetJournalKey(ModuleId, ForumId, TopicId, ReplyId);
                if (JournalController.Instance.GetJournalItemByKey(PortalId, ji.ObjectKey) != null)
                {
                    JournalController.Instance.DeleteJournalItemByKey(PortalId, ji.ObjectKey);
                }
                ji.SecuritySet = "E,";
                JournalController.Instance.SaveJournalItem(ji, module);
            }
        }
    }
}
