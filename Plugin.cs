#if BEPINEX6
using BepInEx.Unity.Mono;
#endif
using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using System.Reflection;

namespace Wayz.CS2.SchoolCapacityBalancer;

[BepInPlugin("Wayz.CS2.SchoolCapacityBalancer", "SchoolCapacityBalancer", "0.1.2")]
public class SchoolCapacityBalancer : BaseUnityPlugin
{
    public static ManualLogSource GameLogger = null!;

    public static IReadOnlyDictionary<string, SchoolOptions> SchoolOptions { get; private set; } = null!;

    private void Awake()
    {
        GameLogger = Logger;

        SchoolCapacityBalancerOptions options;
        // load the settings from the settings file
        if (!WayzSettingsManager.TryGetSettings<SchoolCapacityBalancerOptions>("SchoolCapacityBalancer_Wayz", "settings", out options))
        {
            options = SchoolCapacityBalancerOptions.Default;
            try
            {
                WayzSettingsManager.SaveSettings("SchoolCapacityBalancer_Wayz", "settings", options);
            }
            catch
            {
                Logger.LogWarning("Failed to save default config to settings file, using in-memory default config.");
            }
        }

        // update the settings to the latest version, and save to file if they were updated
        if(options.UpdateToLatestVersion() > 0)
        {
            try
            {
                WayzSettingsManager.SaveSettings("SchoolCapacityBalancer_Wayz", "settings", options);
            }
            catch
            {
                Logger.LogWarning("Failed to save updated config to settings file, using in-memory updated config.");
            }
        }

        // remove invalid entries read from the settings file (should only happen if the user enters in invalid data)
        var filtered = options.RemoveBadEntires();
        if(filtered != 0)
        {
            Logger.LogWarning($"Removed {filtered} bad entries loaded from settings file!");
        }
        SchoolCapacityBalancer.SchoolOptions = options.ToDictionary();

        var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID + "_Cities2Harmony");
        var patchedMethods = harmony.GetPatchedMethods();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION} is loaded! Patched methods: " + patchedMethods.Count());

        foreach (var patchedMethod in patchedMethods)
        {
            Logger.LogInfo($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
        }
    }
}