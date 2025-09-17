using FM20;

namespace FM20.Tests;

public class FeatureManagerTests
{
    [Fact]
    public void EnableFeature_ShouldSetFeatureAsEnabled()
    {
        // Arrange
        var featureManager = new FeatureManager();
        const string featureName = "TestFeature";

        // Act
        featureManager.EnableFeature(featureName);

        // Assert
        Assert.True(featureManager.IsFeatureEnabled(featureName));
    }

    [Fact]
    public void DisableFeature_ShouldSetFeatureAsDisabled()
    {
        // Arrange
        var featureManager = new FeatureManager();
        const string featureName = "TestFeature";
        featureManager.EnableFeature(featureName);

        // Act
        featureManager.DisableFeature(featureName);

        // Assert
        Assert.False(featureManager.IsFeatureEnabled(featureName));
    }

    [Fact]
    public void IsFeatureEnabled_ShouldReturnFalseForNonExistentFeature()
    {
        // Arrange
        var featureManager = new FeatureManager();

        // Act & Assert
        Assert.False(featureManager.IsFeatureEnabled("NonExistentFeature"));
    }

    [Fact]
    public void GetEnabledFeatures_ShouldReturnOnlyEnabledFeatures()
    {
        // Arrange
        var featureManager = new FeatureManager();
        featureManager.EnableFeature("Feature1");
        featureManager.EnableFeature("Feature2");
        featureManager.DisableFeature("Feature3");

        // Act
        var enabledFeatures = featureManager.GetEnabledFeatures().ToList();

        // Assert
        Assert.Contains("Feature1", enabledFeatures);
        Assert.Contains("Feature2", enabledFeatures);
        Assert.DoesNotContain("Feature3", enabledFeatures);
        Assert.Equal(2, enabledFeatures.Count);
    }

    [Fact]
    public void GetVersion_ShouldReturnCorrectVersion()
    {
        // Act
        var version = FeatureManager.GetVersion();

        // Assert
        Assert.Equal("1.0.0", version);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EnableFeature_ShouldThrowArgumentException_ForInvalidFeatureName(string invalidName)
    {
        // Arrange
        var featureManager = new FeatureManager();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => featureManager.EnableFeature(invalidName));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DisableFeature_ShouldThrowArgumentException_ForInvalidFeatureName(string invalidName)
    {
        // Arrange
        var featureManager = new FeatureManager();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => featureManager.DisableFeature(invalidName));
    }
}