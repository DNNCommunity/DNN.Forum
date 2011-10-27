namespace DotNetNuke.Modules.Forums.Components.Common
{
    using DotNetNuke.Entities.Users;

    public interface IDnnUserController
    {
        string UserProfileURL(int userId);
        string UserProfileImageUrl(int portalId, int userId);
    }

    public class DnnUserController : IDnnUserController
    {
        public string UserProfileURL(int userId)
        {
            return DotNetNuke.Common.Globals.UserProfileURL(userId);
        }

        public string UserProfileImageUrl(int portalId, int userId)
        {
            var user = new UserController().GetUser(portalId, userId);
            return user != null && user.Profile != null ? user.Profile.PhotoURL : string.Empty;
        }
    }
}