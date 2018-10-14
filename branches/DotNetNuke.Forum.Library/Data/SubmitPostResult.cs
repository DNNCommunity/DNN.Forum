using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNuke.Forum.Library.Data
{
    public enum PostMessage
    {
        ForumClosed = 0,
        ForumDoesntExist = 1,
        ForumIsParent = 2,
        ForumNoAttachments = 3,
        PostApproved = 4,
        PostEditExpired = 5,
        PostInvalidBody = 6,
        PostInvalidSubject = 7,
        PostModerated = 8,
        ThreadLocked = 9,
        UserAttachmentPerms = 10,
        UserBanned = 11,
        UserCannotEditPost = 12,
        UserCannotPostReply = 13,
        UserCannotStartThread = 14,
        UserCannotViewForum = 15
    };

    /// <summary>
    /// Struct to return result after post is submitted to db
    /// </summary>
    public class SubmitPostResult
    {
        /// <summary>
        /// PostId of either existing or new post that was submitted.
        /// Note that depending on the Result returned this value may or may not be actual PostId found in database.
        /// </summary>
        public int PostId;
        /// <summary>
        /// Post result
        /// </summary>
        public PostMessage Result;
    }
}
