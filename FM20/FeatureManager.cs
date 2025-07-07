namespace FM20;

/// <summary>
/// FM20 Feature Manager - Provides feature management functionality
/// </summary>
public class FeatureManager
{
    private readonly Dictionary<string, bool> _features;

    public FeatureManager()
    {
        _features = new Dictionary<string, bool>();
    }

    /// <summary>
    /// Enables a feature by name
    /// </summary>
    /// <param name="featureName">Name of the feature to enable</param>
    public void EnableFeature(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
            throw new ArgumentException("Feature name cannot be null or empty", nameof(featureName));

        _features[featureName] = true;
    }

    /// <summary>
    /// Disables a feature by name
    /// </summary>
    /// <param name="featureName">Name of the feature to disable</param>
    public void DisableFeature(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
            throw new ArgumentException("Feature name cannot be null or empty", nameof(featureName));

        _features[featureName] = false;
    }

    /// <summary>
    /// Checks if a feature is enabled
    /// </summary>
    /// <param name="featureName">Name of the feature to check</param>
    /// <returns>True if enabled, false otherwise</returns>
    public bool IsFeatureEnabled(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
            return false;

        return _features.TryGetValue(featureName, out bool enabled) && enabled;
    }

    /// <summary>
    /// Gets all enabled features
    /// </summary>
    /// <returns>List of enabled feature names</returns>
    public IEnumerable<string> GetEnabledFeatures()
    {
        return _features.Where(f => f.Value).Select(f => f.Key);
    }

    /// <summary>
    /// Gets version information for the FM20 library
    /// </summary>
    /// <returns>Version string</returns>
    public static string GetVersion()
    {
        return "1.0.0";
    }
}
