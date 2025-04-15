using Xunit;
using AutogenDevelopmentTeam.Console.Services;

namespace AutogenDevelopmentTeam.Tests.UnitTests
{
    public class ApplicationBuilderTests
    {
        [Fact]
        public void Test_ApplicationBuilder_Creates_Application_Successfully()
        {
            // Arrange
            var applicationBuilder = new ApplicationBuilder();
            var expectedApplicationName = "TestApplication";

            // Act
            var result = applicationBuilder.CreateApplication(expectedApplicationName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedApplicationName, result.Name);
        }

        [Fact]
        public void Test_ApplicationBuilder_Adds_Feature_Successfully()
        {
            // Arrange
            var applicationBuilder = new ApplicationBuilder();
            var application = applicationBuilder.CreateApplication("TestApplication");
            var featureName = "TestFeature";

            // Act
            applicationBuilder.AddFeature(application, featureName);

            // Assert
            Assert.Contains(featureName, application.Features);
        }

        [Fact]
        public void Test_ApplicationBuilder_Removes_Feature_Successfully()
        {
            // Arrange
            var applicationBuilder = new ApplicationBuilder();
            var application = applicationBuilder.CreateApplication("TestApplication");
            var featureName = "TestFeature";
            applicationBuilder.AddFeature(application, featureName);

            // Act
            applicationBuilder.RemoveFeature(application, featureName);

            // Assert
            Assert.DoesNotContain(featureName, application.Features);
        }
    }
}