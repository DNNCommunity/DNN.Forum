namespace DotNetNuke.Modules.Forums.Tests
{
    using System.Collections.Generic;
    using Components.Controllers;
    using Components.Entities;
    using MbUnit.Framework;
    using Moq;
    using Services;

    [TestFixture]
    public class ForumsServicePresenterTests
    {
        private Mock<IForumsServiceView> view;
        private ForumsServicePresenter presenter;
        private Mock<IForumsController> forumsController;

        [SetUp]
        public void SetUp()
        {
            view = new Mock<IForumsServiceView>();
            forumsController = new Mock<IForumsController>();
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        // [MethodName]_[StateUnderTest]_[ExpectedBehavior]

        [Test]
        public void GetForumEvent_ReturnsForum_WhenForumIdIsValid()
        {
            // Arrange
            var infos = new List<ForumInfo>();
            var forumInfo = new ForumInfo { Active = true, Description = "Awesome", ForumId = 1 };
            infos.Add(forumInfo);
            infos.Add(new ForumInfo { Active = true, Description = "What?", ForumId = 2 });
            forumsController.Setup(x => x.GetForum(1)).Returns(forumInfo);
            presenter = new ForumsServicePresenter(view.Object, forumsController.Object);

            // Act
            var getForumCalledEventArgs = new GetForumCalledEventArgs(1);
            view.Raise(x => x.ForumGetCalled += null, getForumCalledEventArgs);
            presenter.ReleaseView();
            
            // Assert
            Assert.AreEqual(getForumCalledEventArgs.Result.ForumId, 1);
        }

        [Test]
        public void GetForumEvent_ReturnsNull_WhenForumIdIsInvalid()
        {
            // Arrange
            var infos = new List<ForumInfo>();
            var forumInfo = new ForumInfo { Active = true, Description = "Awesome", ForumId = 1 };
            infos.Add(forumInfo);
            infos.Add(new ForumInfo { Active = true, Description = "What?", ForumId = 2 });
            forumsController.Setup(x => x.GetForum(1)).Returns(forumInfo);
            presenter = new ForumsServicePresenter(view.Object, forumsController.Object);

            // Act
            var getForumCalledEventArgs = new GetForumCalledEventArgs(6);
            view.Raise(x => x.ForumGetCalled += null, getForumCalledEventArgs);
            presenter.ReleaseView();

            // Assert
            Assert.IsNull(getForumCalledEventArgs.Result);
        }

    }
}