﻿namespace Zone.UmbracoPersonalisationGroups.Common.Tests.Criteria.PagesViewed
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Zone.UmbracoPersonalisationGroups.Common.Criteria.PagesViewed;

    [TestClass]
    public class PagesViewedPersonalisationGroupCriteriaTests
    {
        private const string DefinitionFormat = "{{ \"match\": \"{0}\", \"nodeIds\": [{1}] }}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithEmptyDefinition_ThrowsException()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);

            // Act
            criteria.MatchesVisitor((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithInvalidDefinition_ThrowsException()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = "invalid";

            // Act
            criteria.MatchesVisitor(definition);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesViewedAny_WithPageViewed_ReturnsTrue()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "ViewedAny", "1000");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesViewedAny_WithPageNotViewed_ReturnsFalse()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "ViewedAny", "1004");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesViewedAll_WithPagesViewed_ReturnsTrue()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "ViewedAll", "1001,1000,1002");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesViewedAll_WithPagesViewedAndMore_ReturnsTrue()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider(new[] { 1000, 1001, 1002, 1003, 1004 });
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "ViewedAll", "1001,1000,1002");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesViewedAll_WithPagesNotViewed_ReturnsFalse()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "ViewedAll", "1000,1003");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesNotViewedAny_WithPageViewed_ReturnsFalse()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "NotViewedAny", "1000");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesNotViewedAny_WithPageNotViewed_ReturnsTrue()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "NotViewedAny", "1004");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesNotViewedAll_WithPagesViewed_ReturnsFalse()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "NotViewedAll", "1001,1000,1002");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PagesViewedPersonalisationGroupCriteria_MatchesVisitor_WithDefinitionForPagesNotViewedAll_WithPagesNotViewed_ReturnsTrue()
        {
            // Arrange
            var mockPagesViewedProvider = MockPagesViewedProvider();
            var criteria = new PagesViewedPersonalisationGroupCriteria(mockPagesViewedProvider.Object);
            var definition = string.Format(DefinitionFormat, "NotViewedAll", "1000,1003");

            // Act
            var result = criteria.MatchesVisitor(definition);

            // Assert
            Assert.IsTrue(result);
        }

        #region Mocks

        private static Mock<IPagesViewedProvider> MockPagesViewedProvider(int[] pagesViewed = null)
        {
            var mock = new Mock<IPagesViewedProvider>();
            pagesViewed = pagesViewed ?? new[] {1000, 1001, 1002};

            mock.Setup(x => x.GetNodeIdsViewed()).Returns(pagesViewed);

            return mock;
        }

        #endregion
    }
}
