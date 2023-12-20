using Newtonsoft.Json;
using System.Text;

namespace Wayz.CS2;

/// <summary>
/// A static class that can be used to manage settings for your mod.
/// </summary>
/// <remarks>
/// This is a direct copy of the code from v0.2.2 due to .dll conflicts when using separate versions. This ensures that the mod is compatible with other mods using a separate Wayz.CS2.dll.
/// </remarks>
public static class WayzSettingsManager
{
    /// <summary>
    /// Gets the settings for the specified mod and setting name, if the file exists.
    /// </summary>
    /// <typeparam name="T">Type of the settings to load from JSON</typeparam>
    /// <param name="modIdentifier">A unique identifier for your mod. Ex. UnlockAllTiles_Wayz</param>
    /// <param name="settingName">The name of the setting instance to load. Ex. "settings" or "school_settings"</param>
    /// <returns><typeparamref name="T"/> settings if the settings file exists and contains valid JSON. <c>null</c> if the file contains invalid JSON, but the file exists.</returns>
    /// <exception cref="FileNotFoundException">If the mod settings file does not exist, this exception will be thrown.</exception>
    public static T? GetSettings<T>(string modIdentifier, string settingName)
    {
        var settingsPath = Path.Combine(UnityEngine.Application.persistentDataPath, "ModSettings", modIdentifier, $"{settingName}.json");
        if (!File.Exists(settingsPath))
        {
            throw new FileNotFoundException($"Settings file not found at {settingsPath}");
        }
        var settingsJson = File.ReadAllText(settingsPath, Encoding.UTF8);
        return JsonConvert.DeserializeObject<T>(settingsJson);
    }

    /// <summary>
    /// Saves the <typeparamref name="T"/> settings to disk.
    /// </summary>
    /// <typeparam name="T">Type of the settings to save</typeparam>
    /// <param name="modIdentifier">A unique identifier for your mod. Ex. UnlockAllTiles_Wayz</param>
    /// <param name="settingName">The name of the setting instance to load. Ex. "settings" or "school_settings"</param>
    /// <param name="settings">Settings to save</param>
    public static void SaveSettings<T>(string modIdentifier, string settingName, T settings)
    {
        if(!Directory.Exists(Path.Combine(UnityEngine.Application.persistentDataPath, "ModSettings", modIdentifier)))
        {
            Directory.CreateDirectory(Path.Combine(UnityEngine.Application.persistentDataPath, "ModSettings", modIdentifier));
        }
        var settingsPath = Path.Combine(UnityEngine.Application.persistentDataPath, "ModSettings", modIdentifier, $"{settingName}.json");
        var settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(settingsPath, settingsJson, Encoding.UTF8);
    }

    /// <summary>
    /// Attempts to read the settings from disk.
    /// </summary>
    /// <typeparam name="T">Type of the settings to save</typeparam>
    /// <param name="modIdentifier">A unique identifier for your mod. Ex. UnlockAllTiles_Wayz</param>
    /// <param name="settingName">The name of the setting instance to load. Ex. "settings" or "school_settings"</param>
    /// <param name="settings">The settings read from disk, if successful.</param>
    /// <returns><c>true</c> if the settings were successfully read, <c>false</c> otherwise.</returns>
    public static bool TryGetSettings<T>(string modIdentifier, string settingName, out T settings)
    {
        try
        {
            settings = GetSettings<T>(modIdentifier, settingName)!;
            return settings != null;
        }
        catch (FileNotFoundException)
        {
            settings = default!;
            return false;
        }
    }

    /// <summary>
    /// Attempts to read the settings from disk. If the settings file does not exist, it will be created with the default settings.
    /// </summary>
    /// <typeparam name="T">Type of the settings to save</typeparam>
    /// <param name="modIdentifier">A unique identifier for your mod. Ex. UnlockAllTiles_Wayz</param>
    /// <param name="settingName">The name of the setting instance to load. Ex. "settings" or "school_settings"</param>
    /// <returns>The settings read from disk if successful, or the newly created settings.</returns>
    public static T GetOrInitializeSettings<T>(string modIdentifier, string settingName) where T : new()
    {
        if (TryGetSettings<T>(modIdentifier, settingName, out var settings))
        {
            return settings;
        }
        else
        {
            var defaultSettings = new T();
            SaveSettings(modIdentifier, settingName, defaultSettings);
            return defaultSettings;
        }
    }
}
