namespace DotNetNuke.Modules.Forums.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Components.Common;
    using Components.Controllers;
    using Components.Entities;
    using Components.Models;
    using Components.Presenters;
    using Components.Views;
    using MbUnit.Framework;
    using Moq;
    using Templating;

    [TestFixture]
    public class HomePresenterTests
    {
        private Mock<IHomeView> view;
        private Mock<IForumsController> forumsController;
        private Mock<ITemplateFileManager> templateFileManager;
        private List<ForumInfo> forumInfos;
        private Mock<IModuleInstanceContext> moduleContext;
        private Mock<IDnnUserController> globalsWrapper;

        [SetUp]
        public void SetUp()
        {
            view = new Mock<IHomeView>();
            forumsController = new Mock<IForumsController>();
            templateFileManager = new Mock<ITemplateFileManager>();
            moduleContext = new Mock<IModuleInstanceContext>();
            globalsWrapper = new Mock<IDnnUserController>();

            forumInfos = new List<ForumInfo>
                            {
                                new ForumInfo { Name="Awesome Forum", Active = true, Description = "This is awesome!", ForumId = 1, LastPostId = 1, TopicCount = 3, ReplyCount = 4 },
                                new ForumInfo { Name="Crazy Forum", Active = true, Description = "This is crazy!", ForumId = 2, LastPostId = 2 }
                            };

            forumsController.Setup(x => x.GetModuleForums(333)).Returns(forumInfos);
            forumsController.Setup(x => x.GetPost(1)).Returns(new PostInfo { PostId = 1, Body = "post 1 body", DisplayName = "post 1 author name", Subject = "post 1 title", UserId = 2, CreatedDate = DateTime.Today.AddDays(-1) });
            forumsController.Setup(x => x.GetPost(2)).Returns(new PostInfo { PostId = 2, Body = "post 2 body" });

            moduleContext.Setup(x => x.PortalId).Returns(0);
            moduleContext.Setup(x => x.ModuleId).Returns(333);
            moduleContext.Setup(x => x.TabId).Returns(27);
            moduleContext.Setup(x => x.NavigateUrl(27, string.Empty, true, "forumid=1")).Returns("http://url.to/forumid/1");
            moduleContext.Setup(x => x.NavigateUrl(27, string.Empty, true, "forumid=2")).Returns("http://url.to/forumid/2");

            globalsWrapper.Setup(x => x.UserProfileURL(1)).Returns("http://url.to/userprofile/1");
            globalsWrapper.Setup(x => x.UserProfileURL(2)).Returns("http://url.to/userprofile/2");
            globalsWrapper.Setup(x => x.UserProfileImageUrl(0, 1)).Returns("http://url.to/userimage/1");
            globalsWrapper.Setup(x => x.UserProfileImageUrl(0, 2)).Returns("http://url.to/userimage/2");

            view.Setup(x => x.Model).Returns(new HomeModel());
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        // MethodNameStateUnderTestExpectedBehavior

        [Test]
        public void ViewLoad_FullDataAndFullTemplate_SetsModelHtml()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns(GetForumListViewModelTemplate());
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);
            
            // Act
            view.Raise(x => x.Load += null, null, null);
            
            // Assert
            Assert.IsNotNull(view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidUrl_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.url }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("http://url.to/forumid/1", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidName_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.name }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("Awesome Forum", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidDescription_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.description }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("This is awesome!", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidNumberOfReplies_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.number_of_replies }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("4", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidNumberOfTopics_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.number_of_topics }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("3", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidLastPostAuthorImageUrl_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.last_post_author_image_url }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("http://url.to/userimage/2", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidLastPostAuthorUrl_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.last_post_author_url }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("http://url.to/userprofile/2", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidLastPostAuthorName_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.last_post_author_name }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("post 1 author name", view.Object.Model.ForumListHtml);
        }

        //todo: wire up the following test once we have the ability to get a post's URL

        //[Test]
        //public void ViewLoad_ValidLastPostUrl_TemplateRendersProperly()
        //{
        //    // Arrange
        //    templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.last_post_url }}{% endif %}{% endfor %}");
        //    new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

        //    // Act
        //    view.Raise(x => x.Load += null, null, null);

        //    // Assert
        //    Assert.AreEqual("http://url.to/lastpost/inforum/1", view.Object.Model.ForumListHtml);
        //}

        [Test]
        public void ViewLoad_ValidLastPostTitle_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.last_post_title }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual("post 1 title", view.Object.Model.ForumListHtml);
        }

        [Test]
        public void ViewLoad_ValidLastPostDate_TemplateRendersProperly()
        {
            // Arrange
            templateFileManager.Setup(x => x.GetTemplate("Default", "ListForums.template")).Returns("{% for f in forums %}{% if forloop.first %}{{ f.last_post_date }}{% endif %}{% endfor %}");
            new HomePresenter(view.Object, forumsController.Object, templateFileManager.Object, moduleContext.Object, globalsWrapper.Object);

            // Act
            view.Raise(x => x.Load += null, null, null);

            // Assert
            Assert.AreEqual(DateTime.Today.AddDays(-1).ToString(), view.Object.Model.ForumListHtml);
        }

        private string GetForumListViewModelTemplate()
        {
            var s = new StringBuilder();
            s.Append("{% for f in forums %}" + Environment.NewLine);
            s.Append("{{ f.url }}" + Environment.NewLine);
            s.Append("{{ f.name }}" + Environment.NewLine);
            s.Append("{{ f.description }}" + Environment.NewLine);
            s.Append("{{ f.number_of_topics }}" + Environment.NewLine);
            s.Append("{{ f.number_of_replies }}" + Environment.NewLine);
            s.Append("{{ f.last_post_url }}" + Environment.NewLine);
            s.Append("{{ f.last_post_title }}" + Environment.NewLine);
            s.Append("{{ f.last_post_author_url }}" + Environment.NewLine);
            s.Append("{{ f.last_post_author_name }}" + Environment.NewLine);
            s.Append("{{ f.last_post_date | relative_date }}" + Environment.NewLine);
            s.Append("{{ f.last_post_author_image_url }}" + Environment.NewLine);
            s.Append("{% endfor %}" + Environment.NewLine);
            return s.ToString();
        }
    }
}